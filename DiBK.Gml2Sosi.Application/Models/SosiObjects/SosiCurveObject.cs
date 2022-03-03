using DiBK.Gml2Sosi.Application.Models.Geometries;
using System.Xml.Linq;

namespace DiBK.Gml2Sosi.Application.Models.SosiObjects
{
    public abstract class SosiCurveObject : SosiObjectType
    {
        public SosiSegment Segment { get; set; }

        protected override void SetSosiValues()
        {
            base.SetSosiValues();

            if (!Points.Any())
                return;

            SosiValues.Add("..NØ");
            SosiValues.Add(Points.First().ToString());

            var restPoints = Points.Skip(1);

            if (!restPoints.Any())
                return;

            SosiValues.Add("..NØ");

            foreach (var point in restPoints)
                SosiValues.Add(point.ToString());
        }

        public static void AddNodesToCurves(IEnumerable<SosiCurveObject> curveObjects)
        {
            foreach (var curveObject in curveObjects)
            {
                var firstPoint = curveObject.Points.FirstOrDefault();
                var lastPoint = curveObject.Points.LastOrDefault();

                if (firstPoint.Equals(lastPoint))
                {
                    firstPoint.IsNode = true;
                    lastPoint.IsNode = true;
                    continue;
                }

                var otherCurveObjects = curveObjects.Where(curveObj => curveObj != curveObject).ToList();

                foreach (var otherCurveObject in otherCurveObjects)
                {
                    var otherFirstPoint = otherCurveObject.Points.FirstOrDefault();
                    var otherLastPoint = otherCurveObject.Points.LastOrDefault();

                    if (firstPoint.Equals(otherFirstPoint))
                    {
                        firstPoint.IsNode = true;
                        otherFirstPoint.IsNode = true;
                    }
                    else if (firstPoint.Equals(otherLastPoint))
                    {
                        firstPoint.IsNode = true;
                        otherLastPoint.IsNode = true;
                    }
                    else if (lastPoint.Equals(otherFirstPoint))
                    {
                        lastPoint.IsNode = true;
                        otherFirstPoint.IsNode = true;
                    }
                    else if (lastPoint.Equals(otherLastPoint))
                    {
                        lastPoint.IsNode = true;
                        otherLastPoint.IsNode = true;
                    }
                }
            }
        }

        public static CartographicElementType GetElementType(XElement geomElement)
        {
            return geomElement.Name.LocalName switch
            {
                "LineStringSegment" or "LinearRing" => CartographicElementType.Kurve,
                "Arc" => CartographicElementType.BueP,
                _ => CartographicElementType.Unknown,
            };
        }
    }
}
