using NetTopologySuite.Geometries;

namespace DiBK.Gml2Sosi.Application.Extensions
{
    public static class NetTopologySuiteExtensions
    {
        public static List<Point> GetPoints(this LineString lineString)
        {
            var points = new List<Point>();

            for (int i = 0; i < lineString.NumPoints; i++)
                points.Add(lineString.GetPointN(i));

            return points;
        }
    }
}
