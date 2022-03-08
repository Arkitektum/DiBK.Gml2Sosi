using DiBK.Gml2Sosi.Application.Helpers;
using DiBK.Gml2Sosi.Application.Models.SosiObjects;

namespace DiBK.Gml2Sosi.Application.Models.Geometries
{
    public class SosiSegment
    {
        public List<SosiPoint> Points { get; set; } = new();
        public SegmentType SegmentType { get; set; }
        public SosiCurveObject CurveObject { get; set; }
        public bool IsReversed { get; set; }

        public SosiSegment()
        {
        }

        public SosiSegment(SosiCurveObject curveObject) : this(curveObject.Points, MapperHelper.CartographicElementTypeToSegmentType(curveObject.ElementType), curveObject)
        {
        }

        public SosiSegment(List<SosiPoint> points, SegmentType segmentType, SosiCurveObject curveObject, bool isReversed = false)
        {
            Points = points;
            SegmentType = segmentType;
            CurveObject = curveObject;
            IsReversed = isReversed;
        }

        public SosiSegment Clone()
        {
            return new SosiSegment
            {
                Points = Points.ConvertAll(point => point.Clone()),
                SegmentType = SegmentType,
                CurveObject = CurveObject,
                IsReversed = IsReversed
            };
        }

        public string ToWkt()
        {
            var type = SegmentType == SegmentType.BueP ? "CIRCULARSTRING ({0})" : "LINESTRING ({0})";
            var points = IsReversed ? Enumerable.Reverse(Points) : Points;
            var pointsString = string.Join(", ", points.Select(point => $"{point.X} {point.Y}"));

            return string.Format(type, pointsString);
        }
    }
}
