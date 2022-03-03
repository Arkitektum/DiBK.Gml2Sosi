using NetTopologySuite.Geometries;

namespace DiBK.Gml2Sosi.Application.Models.Geometries
{
    public class LineStringIntersection
    {
        public string SurfaceLocalId { get; set; }
        public string IntersectingSurfaceLocalId { get; set; }
        public LineString LineString { get; set; }

        public LineStringIntersection(string surfaceLocalId, string intersectingSurfaceLocalId, LineString lineString)
        {
            SurfaceLocalId = surfaceLocalId;
            IntersectingSurfaceLocalId = intersectingSurfaceLocalId;
            LineString = lineString;
        }
    }
}
