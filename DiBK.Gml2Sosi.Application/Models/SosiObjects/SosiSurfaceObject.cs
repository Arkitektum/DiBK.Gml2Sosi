using DiBK.Gml2Sosi.Application.Helpers;
using DiBK.Gml2Sosi.Application.Models.Geometries;

namespace DiBK.Gml2Sosi.Application.Models.SosiObjects
{
    public abstract class SosiSurfaceObject : SosiObjectType
    {
        public Surface Surface { get; set; }
        public string Referanser { get; private set; }
        public SosiPoint Representasjonspunkt { get; private set; }

        protected override void SetSosiValues()
        {
            base.SetSosiValues();

            SetReferences();
            SetPointOnSurface();

            if (string.IsNullOrWhiteSpace(Referanser))
                return;

            SosiValues.Add($"..REF {Referanser}");
            SosiValues.Add("..NØ");
            SosiValues.Add(Representasjonspunkt.ToString());
        }

        private void SetReferences()
        {
            if (Surface == null)
                return;

            var exteriorRefs = Surface.Exterior.Segments
                .Select(segment => $":{(segment.IsReversed ? "-" : "")}{segment.CurveObject.SequenceNumber}");

            var refs = string.Join(" ", exteriorRefs);

            if (Surface.Interior.Any())
            {
                foreach (var interior in Surface.Interior)
                {
                    var interiorRefs = interior.Segments
                        .Select(segment => $":{(segment.IsReversed ? "-" : "")}{segment.CurveObject.SequenceNumber}");

                    refs += $" ({string.Join(" ", interiorRefs)})";
                }
            }

            Referanser = refs;
        }

        private void SetPointOnSurface()
        {
            if (Surface == null)
                return;

            using var surfaceGeometry = GeometryHelper.OgrGeometryFromWkt(Surface.ToWkt());

            if (surfaceGeometry == null)
                return;

            using var point = surfaceGeometry.PointOnSurface();

            Representasjonspunkt = SosiPoint.Create(point.GetX(0), point.GetY(0), 1);
        }
    }
}
