using OSGeo.OGR;

namespace DiBK.Gml2Sosi.Application.Models.Geometries
{
    public class Ring
    {
        public List<SosiSegment> Segments { get; set; } = new();
        public Geometry Polygon { get; set; }
        public List<Ring> WithinRings { get; set; } = new();
        public bool IsExterior { get; set; }

        public Ring()
        {
        }

        public Ring(List<SosiSegment> segments)
        {
            Segments = segments;
        }

        public string ToWkt()
        {
            var compoundCurve = "COMPOUNDCURVE ({0})";
            var segments = string.Join(", ", Segments.Select(segment => segment.ToWkt()));
            
            return string.Format(compoundCurve, segments);
        }
    }
}
