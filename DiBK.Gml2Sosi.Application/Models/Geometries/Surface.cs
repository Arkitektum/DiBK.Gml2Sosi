namespace DiBK.Gml2Sosi.Application.Models.Geometries
{
    public class Surface
    {
        public Ring Exterior { get; set; }
        public List<Ring> Interior { get; set; } = new();

        public string ToWkt()
        {
            var curvePolygon = "CURVEPOLYGON ({0})";
            var compoundCurve = "COMPOUNDCURVE ({0})";

            var extSegments = string.Join(", ", Exterior.Segments.Select(segment => segment.ToWkt()));
            var extWkt = string.Format(compoundCurve, extSegments);

            var wkts = Interior
                .Select(interior =>
                {
                    var intSegments = string.Join(", ", interior.Segments.Select(segment => segment.ToWkt()));
                    return $"({string.Format(compoundCurve, intSegments)}";
                })
                .ToList();

            wkts.Insert(0, extWkt);

            return string.Format(curvePolygon, string.Join(", ", wkts));
        }
    }
}
