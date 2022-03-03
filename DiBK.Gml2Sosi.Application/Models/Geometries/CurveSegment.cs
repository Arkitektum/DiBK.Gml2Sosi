using NetTopologySuite.Geometries;

namespace DiBK.Gml2Sosi.Application.Models.Geometries
{
    public class CurveSegment
    {
        public string SurfaceLocalId { get; set; }
        public LineString LineString { get; set; }

        public CurveSegment(string surfaceLocalId, LineString lineString)
        {
            SurfaceLocalId = surfaceLocalId;
            LineString = lineString;
        }
    }
}
