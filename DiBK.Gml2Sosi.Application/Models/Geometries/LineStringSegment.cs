using NetTopologySuite.Geometries;

namespace DiBK.Gml2Sosi.Application.Models.Geometries
{
    public class LineStringSegment : CurveSegment
    {
        public List<LineStringIntersection> Intersections { get; set; } = new();

        public LineStringSegment(string surfaceLocalId, LineString lineString) : base(surfaceLocalId, lineString)
        {
        }
    }
}
