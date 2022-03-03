namespace DiBK.Gml2Sosi.Application.Models.Geometries
{
    public class Point
    {
        public double X { get; set; }
        public double Y { get; set; }

        public Point()
        {
        }

        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }

        public override bool Equals(object obj)
        {
            return obj is Point point &&
                   X == point.X &&
                   Y == point.Y;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }

        public override string ToString()
        {
            return $"{X} {Y}";
        }
    }
}
