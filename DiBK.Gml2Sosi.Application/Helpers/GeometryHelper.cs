using DiBK.Gml2Sosi.Application.Extensions;
using DiBK.Gml2Sosi.Application.Models.Geometries;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;
using System.Globalization;
using System.Xml.Linq;
using Wmhelp.XPath2;
using OgrGeometry = OSGeo.OGR.Geometry;

namespace DiBK.Gml2Sosi.Application.Helpers
{
    public class GeometryHelper
    {
        private static readonly WKTReader _wktReader = new();

        public static List<Models.Geometries.Point> GetPoints(XElement geomElement)
        {
            return GetCoordinates(geomElement)
                .Select(coordPair => new Models.Geometries.Point(coordPair[0], coordPair[1]))
                .ToList();
        }

        public static LineString CreateLineString(XElement geomElement)
        {
            var coordinates = GetPoints(geomElement)
                .Select(point => new Coordinate(point.X, point.Y))
                .ToArray();

            return new LineString(coordinates);
        }

        public static List<SosiPoint> GetSosiPoints(XElement geomElement, double resolution)
        {
            return GetCoordinates(geomElement)
                .Select(coordPair => SosiPoint.Create(coordPair[0], coordPair[1], resolution))
                .ToList();
        }

        public static List<SosiPoint> GetSosiPoints(LineString lineString, double resolution)
        {
            return lineString.GetPoints()
                .Select(point => SosiPoint.Create(point.X, point.Y, resolution))
                .ToList();
        }

        public static List<double[]> GetCoordinates(XElement geomElement)
        {
            return GetPosLists(geomElement)
                .SelectMany(posList => PosListToCoordinates(posList))
                .ToList();
        }

        public static List<string> GetPosLists(XElement geomElement)
        {
            return geomElement.XPath2SelectElements("//*:posList | //*:pos")
                .Select(element => element.Value)
                .ToList();
        }

        public static List<double[]> PosListToCoordinates(string posList)
        {
            if (posList == null)
                throw new Exception("Element gml:posList eksisterer ikke");

            var posStrings = posList.Split(" ");

            if (posStrings.Length == 0 || posStrings.Length % 2 != 0)
                throw new Exception($"Element gml:posList har ugyldig antall koordinater: '{posStrings.Length}'");

            static double posStringToDouble(string posString)
            {
                try
                {
                    return double.Parse(posString, CultureInfo.InvariantCulture);
                }
                catch (Exception)
                {
                    throw new Exception($"Element gml:posList har ugyldig koordinat: '{posString}'");
                }
            }

            var positions = new List<double[]>();

            for (var i = 1; i < posStrings.Length; i += 2)
            {
                var x = posStringToDouble(posStrings[i - 1]);
                var y = posStringToDouble(posStrings[i]);

                positions.Add(new[] { x, y });
            }

            return positions;
        }

        public static bool PointsAreClockWise(List<SosiPoint> points)
        {
            double sum = 0;

            for (int i = 1; i < points.Count; i++)
                sum += (points[i].X - points[i - 1].X) * (points[i].Y + points[i - 1].Y);

            return sum >= 0;
        }

        public static Geometry OgrGeometryToNtsGeometry(OgrGeometry geometry, double maxAngleStepSizeDegrees = 0.05)
        {
            try
            {
                using var linearGeometry = geometry.GetLinearGeometry(maxAngleStepSizeDegrees, Array.Empty<string>());
                geometry.ExportToWkt(out var wkt);

                return _wktReader.Read(wkt);
            }
            catch
            {
                return null;
            }
        }

        public static TGeometry GeometryFromGml<TGeometry>(XElement geomElement) where TGeometry : Geometry
        {
            using var geometry = OgrGeometryFromGml(geomElement);

            return OgrGeometryToNtsGeometry(geometry) as TGeometry;
        }

        public static Geometry GeometryFromGml(XElement geomElement)
        {
            using var geometry = OgrGeometryFromGml(geomElement);

            return OgrGeometryToNtsGeometry(geometry);
        }

        public static OgrGeometry OgrGeometryFromGml(XElement geomElement)
        {
            try
            {
                return OgrGeometry.CreateFromGML(geomElement.ToString());
            }
            catch
            {
                return null;
            }
        }

        public static OgrGeometry OgrGeometryFromWkt(string wkt)
        {
            try
            {
                return OgrGeometry.CreateFromWkt(wkt);
            }
            catch
            {
                return null;
            }
        }
    }
}
