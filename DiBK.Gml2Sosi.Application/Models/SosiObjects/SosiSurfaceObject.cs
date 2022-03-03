using DiBK.Gml2Sosi.Application.Helpers;
using DiBK.Gml2Sosi.Application.Models.Geometries;
using System.Xml.Linq;

namespace DiBK.Gml2Sosi.Application.Models.SosiObjects
{
    public abstract class SosiSurfaceObject : SosiObjectType
    {
        public string Referanser { get; private set; }
        public SosiPoint Representasjonspunkt { get; private set; }

        protected override void SetSosiValues()
        {
            base.SetSosiValues();

            if (string.IsNullOrWhiteSpace(Referanser))
                return;

            SosiValues.Add($"..REF {Referanser}");
            SosiValues.Add("..NØ");
            SosiValues.Add(Representasjonspunkt.ToString());
        }

        public void SetReferences(IEnumerable<SosiCurveObject> boundaryObjects)
        {
            var segments = boundaryObjects.Select(boundaryObject => new SosiSegment(boundaryObject));
            var surface = SurfaceHelper.SegmentsToSurfaces(segments).FirstOrDefault();

            SetReferences(surface);
        }

        public void SetReferences(Surface surface)
        {
            if (surface == null)
                return;

            var exteriorRefs = surface.Exterior.Segments
                .Select(segment => $":{(segment.IsReversed ? "-" : "")}{segment.SequenceNumber}");

            var refs = string.Join(" ", exteriorRefs);

            if (surface.Interior.Any())
            {
                foreach (var interior in surface.Interior)
                {
                    var interiorRefs = interior.Segments
                        .Select(segment => $":{(segment.IsReversed ? "-" : "")}{segment.SequenceNumber}");

                    refs += $" ({string.Join(" ", interiorRefs)})";
                }
            }

            Referanser = refs;
        }

        public void SetPointOnSurface(XElement geomElement, double resolution)
        {
            using var surfaceGeometry = GeometryHelper.OgrGeometryFromGml(geomElement);

            if (surfaceGeometry == null)
                return;

            using var point = surfaceGeometry.PointOnSurface();

            Representasjonspunkt = SosiPoint.Create(point.GetX(0), point.GetY(0), resolution);
        }
    }
}
