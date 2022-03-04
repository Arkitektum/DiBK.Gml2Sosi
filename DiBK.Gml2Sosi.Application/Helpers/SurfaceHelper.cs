using DiBK.Gml2Sosi.Application.Models.Geometries;
using NetTopologySuite.Geometries;
using System.Xml.Linq;
using Wmhelp.XPath2;
using Point = NetTopologySuite.Geometries.Point;

namespace DiBK.Gml2Sosi.Application.Helpers
{
    public class SurfaceHelper
    {
        public static List<XElement> GetSegmentElementsOfSurfaceElement(XElement geomElement)
        {
            var segmentElements = new List<XElement>();
            var exteriorElement = geomElement.XPath2SelectElement("//*:exterior");
            var interiorElements = geomElement.XPath2SelectElements("//*:interior");

            void GetSegmentsOfRing(XElement boundaryElement)
            {
                var ringElement = boundaryElement.XPath2SelectElement("*:LinearRing");

                if (ringElement != null)
                {
                    segmentElements.Add(ringElement);
                }
                else
                {
                    ringElement = boundaryElement.XPath2SelectElement("*:Ring");

                    if (ringElement != null)
                        segmentElements.AddRange(ringElement.XPath2SelectElements("//*:segments/*"));
                }
            }

            GetSegmentsOfRing(exteriorElement);

            foreach (var interiorElement in interiorElements)
                GetSegmentsOfRing(interiorElement);

            return segmentElements;
        }

        public static List<Surface> SegmentsToSurfaces(IEnumerable<SosiSegment> segments)
        {
            var rings = new List<Ring> { new Ring() };
            var firstSegment = segments.First();
            var restSegments = segments.Skip(1).ToList();

            rings.Last().Segments.Add(firstSegment);

            while (restSegments.Any())
            {
                var lastPoint = rings.Last().Segments.Last().Points.Last();
                var found = false;

                foreach (var segment in restSegments)
                {
                    if (lastPoint.Equals(segment.Points.First()))
                    {
                        rings.Last().Segments.Add(segment);
                        restSegments.Remove(segment);
                        found = true;
                        break;
                    }
                    else if (lastPoint.Equals(segment.Points.Last()))
                    {
                        var newSegment = new SosiSegment(Enumerable.Reverse(segment.Points).ToList(), segment.SequenceNumber, segment.SegmentType, true);
                        rings.Last().Segments.Add(newSegment);
                        restSegments.Remove(segment);
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    var firstRestSegment = restSegments.First();
                    rings.Add(new Ring(new List<SosiSegment> { firstRestSegment }));
                    restSegments.Remove(firstRestSegment);
                }
            }

            List<Surface> surfaces;

            if (rings.Count == 1)
            {
                var exterior = rings.Single();
                exterior.IsExterior = true;
                surfaces = new List<Surface> { new Surface { Exterior = exterior } };
            }
            else
            {
                surfaces = ConvertRingsToSurfaces(rings);
            }

            // FixRingOrientation(surfaces);

            return surfaces;
        }

        private static List<Surface> ConvertRingsToSurfaces(List<Ring> rings)
        {
            foreach (var ring in rings)
                ring.Envelope = CreateEnvelope(ring);

            for (var i = 0; i < rings.Count; i++)
            {
                var ring = rings[i];
                var otherRings = rings.Where(rng => rng != ring).ToList();

                for (int j = 0; j < otherRings.Count; j++)
                {
                    var otherRing = otherRings[j];

                    if (ring.Envelope.Within(otherRing.Envelope))
                        rings.SingleOrDefault(rng => rng == ring).WithinRings.Add(otherRing);
                }
            }

            foreach (var ring in rings)
            {
                ring.IsExterior = ring.WithinRings.Count % 2 == 0;
                ring.WithinRings.RemoveAll(ring => !ring.IsExterior);
            }

            var surfaces = rings.Where(ring => ring.IsExterior).Select(ring => new Surface { Exterior = ring }).ToList();
            var interiors = rings.Where(ring => !ring.IsExterior).OrderBy(ring => ring.WithinRings.Count);

            foreach (var interior in interiors.ToList())
            {
                foreach (var ring in interior.WithinRings.ToList())
                {
                    var exterior = surfaces.SingleOrDefault(polygon => polygon.Exterior == ring);

                    if (exterior == null)
                        continue;

                    exterior.Interior.Add(interior);
                    interior.WithinRings.Remove(ring);

                    interiors
                        .Where(interior => interior.WithinRings.Count > 1)
                        .ToList()
                        .ForEach(interior => interior.WithinRings.Remove(ring));
                }
            }

            return surfaces;
        }

        private static void FixRingOrientation(List<Surface> surfaces)
        {
            foreach (var surface in surfaces)
            {
                if (!GeometryHelper.PointsAreClockWise(surface.Exterior.Segments.First().Points))
                {
                    foreach (var segment in surface.Exterior.Segments)
                        segment.IsReversed = !segment.IsReversed;

                    surface.Exterior.Segments.Reverse();
                }

                foreach (var interior in surface.Interior)
                {
                    if (GeometryHelper.PointsAreClockWise(interior.Segments.First().Points))
                    {
                        foreach (var segment in interior.Segments)
                            segment.IsReversed = !segment.IsReversed;

                        interior.Segments.Reverse();
                    }
                }
            }
        }

        private static Geometry CreateEnvelope(Ring ring)
        {
            var points = ring.Segments
                .SelectMany(segment => segment.Points)
                .Select(point => new Point(point.X, point.Y))
                .ToArray();

            var multiPoint = new MultiPoint(points);

            return multiPoint.Envelope;
        }
    }
}
