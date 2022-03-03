namespace DiBK.Gml2Sosi.Application.Models.Geometries
{
    public class SosiPoint
    {
        public long X { get; set; }
        public long Y { get; set; }
        public bool IsNode { get; set; }

        private SosiPoint()
        {
        }

        private SosiPoint(long x, long y)
        {
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            return $"{Y} {X}{(IsNode ? " ...KP 1" : "")}";
        }

        public override bool Equals(object obj)
        {
            return obj is SosiPoint point &&
                   X == point.X &&
                   Y == point.Y;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }

        public SosiPoint Clone()
        {
            return new SosiPoint
            {
                X = X,
                Y = Y,
                IsNode = IsNode
            };
        }

        public static SosiPoint Create(double x, double y, double resolution)
        {
            return new SosiPoint(AdjustCoordinate(x, resolution), AdjustCoordinate(y, resolution));
        }

        private static long AdjustCoordinate(double coordinate, double resolution)
        {
            return (long)Math.Round(coordinate / resolution, 0);
        }
    }
}
