using DiBK.Gml2Sosi.Application.Extensions;
using DiBK.Gml2Sosi.Application.Models.Geometries;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;
using System.Globalization;
using System.Xml.Linq;
using Wmhelp.XPath2;
using OgrGeometry = OSGeo.OGR.Geometry;
using Point = NetTopologySuite.Geometries.Point;

namespace DiBK.Gml2Sosi.Application.Helpers
{
    public class GeometryHelper
    {
        private static readonly WKTReader _wktReader = new();

        public static List<Models.Geometries.Point> GetPoints(XElement geomElement)
        {
            return GetCoordinates(geomElement)
                .Select(coordPair => new Models.Geometries.Point(coordPair[0], coordPair[1]))
                .ToList();
        }

        public static LineString CreateLineString(XElement geomElement)
        {
            var coordinates = GetPoints(geomElement)
                .Select(point => new Coordinate(point.X, point.Y))
                .ToArray();

            return new LineString(coordinates);
        }

        public static List<SosiPoint> GetSosiPoints(XElement geomElement, double resolution)
        {
            return GetCoordinates(geomElement)
                .Select(coordPair => SosiPoint.Create(coordPair[0], coordPair[1], resolution))
                .ToList();
        }

        public static List<SosiPoint> GetSosiPoints(LineString lineString, double resolution)
        {
            return lineString.GetPoints()
                .Select(point => SosiPoint.Create(point.X, point.Y, resolution))
                .ToList();
        }

        public static List<double[]> GetCoordinates(XElement geomElement)
        {
            return GetPosLists(geomElement)
                .SelectMany(posList => PosListToCoordinates(posList))
                .ToList();
        }

        public static List<string> GetPosLists(XElement geomElement)
        {
            return geomElement.XPath2SelectElements("//*:posList | //*:pos")
                .Select(element => element.Value)
                .ToList();
        }

        public static List<double[]> PosListToCoordinates(string posList)
        {
            if (posList == null)
                throw new Exception("Element gml:posList eksisterer ikke");

            var posStrings = posList.Split(" ");

            if (posStrings.Length == 0 || posStrings.Length % 2 != 0)
                throw new Exception($"Element gml:posList har ugyldig antall koordinater: '{posStrings.Length}'");

            static double posStringToDouble(string posString)
            {
                try
                {
                    return double.Parse(posString, CultureInfo.InvariantCulture);
                }
                catch (Exception)
                {
                    throw new Exception($"Element gml:posList har ugyldig koordinat: '{posString}'");
                }
            }

            var positions = new List<double[]>();

            for (var i = 1; i < posStrings.Length; i += 2)
            {
                var x = posStringToDouble(posStrings[i - 1]);
                var y = posStringToDouble(posStrings[i]);

                positions.Add(new[] { x, y });
            }

            return positions;
        }

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
                        var newSegment = new SosiSegment(Enumerable.Reverse(segment.Points).ToList(), segment.SegmentType, segment.CurveObject, true);
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

        public static bool PointsAreClockWise(List<SosiPoint> points)
        {
            double sum = 0;

            for (int i = 1; i < points.Count; i++)
                sum += (points[i].X - points[i - 1].X) * (points[i].Y + points[i - 1].Y);

            return sum >= 0;
        }

        public static Geometry OgrGeometryToNtsGeometry(OgrGeometry geometry, double maxAngleStepSizeDegrees = 0.05)
        {
            try
            {
                using var linearGeometry = geometry.GetLinearGeometry(maxAngleStepSizeDegrees, Array.Empty<string>());
                geometry.ExportToWkt(out var wkt);

                return _wktReader.Read(wkt);
            }
            catch
            {
                return null;
            }
        }

        public static TGeometry GeometryFromGml<TGeometry>(XElement geomElement) where TGeometry : Geometry
        {
            using var geometry = OgrGeometryFromGml(geomElement);

            return OgrGeometryToNtsGeometry(geometry) as TGeometry;
        }

        public static Geometry GeometryFromGml(XElement geomElement)
        {
            using var geometry = OgrGeometryFromGml(geomElement);

            return OgrGeometryToNtsGeometry(geometry);
        }

        public static OgrGeometry OgrGeometryFromGml(XElement geomElement)
        {
            try
            {
                return OgrGeometry.CreateFromGML(geomElement.ToString());
            }
            catch
            {
                return null;
            }
        }

        public static OgrGeometry OgrGeometryFromWkt(string wkt)
        {
            try
            {
                return OgrGeometry.CreateFromWkt(wkt);
            }
            catch
            {
                return null;
            }
        }

        private static void FixRingOrientation(List<Surface> surfaces)
        {
            foreach (var surface in surfaces)
            {
                if (!PointsAreClockWise(surface.Exterior.Segments.First().Points))
                {
                    foreach (var segment in surface.Exterior.Segments)
                        segment.IsReversed = !segment.IsReversed;

                    surface.Exterior.Segments.Reverse();
                }

                foreach (var interior in surface.Interior)
                {
                    if (PointsAreClockWise(interior.Segments.First().Points))
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
