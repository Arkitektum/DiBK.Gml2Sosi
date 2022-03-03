using DiBK.Gml2Sosi.Application.Extensions;
using NetTopologySuite.Geometries;

namespace DiBK.Gml2Sosi.Application.Utils
{
    public class CustomLineStringComparer : IEqualityComparer<LineString>
    {
        public bool Equals(LineString x, LineString y)
        {
            if (ReferenceEquals(x, y))
                return true;

            if (x is null || y is null)
                return false;

            var pointsX = x.GetPoints();
            var pointsXReversed = Enumerable.Reverse(pointsX);
            var pointsY = y.GetPoints();

            return pointsX.SequenceEqual(pointsY) || pointsXReversed.SequenceEqual(pointsY);
        }

        public int GetHashCode(LineString segment) => 0;
    }
}
