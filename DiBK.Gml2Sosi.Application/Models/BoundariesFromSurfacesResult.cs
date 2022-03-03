using DiBK.Gml2Sosi.Application.Models.Geometries;
using NetTopologySuite.Geometries;

namespace DiBK.Gml2Sosi.Application.Models
{
    public class BoundariesFromSurfacesResult
    {
        public Dictionary<string, FeatureElement> FeatureElements { get; set; }
        public IEnumerable<IGrouping<LineString, Arc>> GroupedArcs { get; set; }
        public IEnumerable<IGrouping<LineString, LineStringSegment>> GroupedDifferences { get; set; }
        public IEnumerable<IGrouping<LineString, LineStringIntersection>> GroupedIntersections { get; set; }
    }
}
