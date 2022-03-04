namespace DiBK.Gml2Sosi.Application.Models.Geometries
{
    public class Surface
    {
        public Ring Exterior { get; set; }
        public List<Ring> Interior { get; set; } = new();

        public string ToWkt()
        {
            var curvePolygon = "CURVEPOLYGON ({0})";

            var wkts = Interior
                .Select(interior => $"({interior.ToWkt()})")
                .ToList();

            wkts.Insert(0, Exterior.ToWkt());

            return string.Format(curvePolygon, string.Join(", ", wkts));
        }
    }
}
