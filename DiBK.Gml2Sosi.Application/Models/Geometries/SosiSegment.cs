using DiBK.Gml2Sosi.Application.Helpers;
using DiBK.Gml2Sosi.Application.Models.SosiObjects;

namespace DiBK.Gml2Sosi.Application.Models.Geometries
{
    public class SosiSegment
    {
        public int SequenceNumber { get; set; }
        public List<SosiPoint> Points { get; set; } = new();
        public bool IsReversed { get; set; }
        public SegmentType SegmentType { get; set; }

        public SosiSegment()
        {
        }

        public SosiSegment(SosiCurveObject curveObject) : this(curveObject.Points, curveObject.SequenceNumber, MapperHelper.CartographicElementTypeToSegmentType(curveObject.ElementType))
        {
        }

        public SosiSegment(List<SosiPoint> points, int sequenceNumber, SegmentType segmentType, bool isReversed = false)
        {
            Points = points;
            SequenceNumber = sequenceNumber;
            SegmentType = segmentType;
            IsReversed = isReversed;
        }

        public SosiSegment Clone()
        {
            return new SosiSegment
            {
                Points = Points.ConvertAll(point => point.Clone()),
                SequenceNumber = SequenceNumber,
                SegmentType = SegmentType,
                IsReversed = IsReversed
            };
        }
    }
}
