namespace DiBK.Gml2Sosi.Application.Models.Geometries
{
    public class Segment
    {
        public List<Point> Points { get; set; }

        public Segment()
        {
        }

        public Segment(List<Point> points)
        {
            Points = points;
        }

        public override bool Equals(object obj)
        {
            return obj is Segment segment && (segment.Points.SequenceEqual(Points) || segment.Points.SequenceEqual(Enumerable.Reverse(Points)));
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Points);
        }
    }
}
