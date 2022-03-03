namespace DiBK.Gml2Sosi.Application.Models.Geometries
{
    public class SosiPoint
    {
        private const double DatasetResolution = 0.01;
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

        public static SosiPoint Create(double x, double y)
        {
            return new SosiPoint(AdjustCoordinate(x), AdjustCoordinate(y));
        }

        private static long AdjustCoordinate(double coordinate)
        {
            return (long)Math.Round(coordinate / DatasetResolution, 0);
        }
    }
}
