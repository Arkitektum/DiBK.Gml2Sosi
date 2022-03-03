using NetTopologySuite.Geometries;

namespace DiBK.Gml2Sosi.Application.Models.Geometries
{
    public class Arc : CurveSegment
    {
        public Arc(string surfaceLocalId, LineString lineString) : base(surfaceLocalId, lineString)
        {
        }
    }
}
