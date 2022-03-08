﻿using DiBK.Gml2Sosi.Application.Helpers;
using DiBK.Gml2Sosi.Application.Models.SosiObjects;

namespace DiBK.Gml2Sosi.Application.Models.Geometries
{
    public class SosiSegment
    {
        public int SequenceNumber { get; set; }
        public List<SosiPoint> Points { get; set; } = new();
        public bool IsReversed { get; set; }
        public SegmentType SegmentType { get; set; }
        public SosiCurveObject CurveObject { get; set; }

        public SosiSegment()
        {
        }

        public SosiSegment(SosiCurveObject curveObject) : this(curveObject.Points, curveObject.SequenceNumber, MapperHelper.CartographicElementTypeToSegmentType(curveObject.ElementType), curveObject)
        {
        }

        public SosiSegment(List<SosiPoint> points, int sequenceNumber, SegmentType segmentType, SosiCurveObject curveObject, bool isReversed = false)
        {
            Points = points;
            SequenceNumber = sequenceNumber;
            SegmentType = segmentType;
            IsReversed = isReversed;
            CurveObject = curveObject;
        }

        public SosiSegment Clone()
        {
            return new SosiSegment
            {
                Points = Points.ConvertAll(point => point.Clone()),
                SequenceNumber = SequenceNumber,
                CurveObject = CurveObject,
                SegmentType = SegmentType,
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
