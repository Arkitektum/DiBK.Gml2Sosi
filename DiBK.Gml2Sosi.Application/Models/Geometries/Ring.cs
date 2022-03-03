using NetTopologySuite.Geometries;

namespace DiBK.Gml2Sosi.Application.Models.Geometries
{
    public class Ring
    {
        public List<SosiSegment> Segments { get; set; } = new();
        public Geometry Envelope { get; set; }
        public List<Ring> WithinRings { get; set; } = new();
        public bool IsExterior { get; set; }

        public Ring()
        {
        }

        public Ring(List<SosiSegment> segments)
        {
            Segments = segments;
        }
    }
}
