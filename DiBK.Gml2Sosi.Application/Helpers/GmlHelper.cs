using DiBK.Gml2Sosi.Application.Constants;
using DiBK.Gml2Sosi.Application.Models;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace DiBK.Gml2Sosi.Application.Helpers
{
    public class GmlHelper
    {
        private static readonly Regex _srsNameRegex =
            new(@"^(http:\/\/www\.opengis\.net\/def\/crs\/EPSG\/0\/|urn:ogc:def:crs:EPSG::)(?<epsg>\d+)$", RegexOptions.Compiled);

        public static string GetFeatureType(XElement element)
        {
            return GetFeatureElement(element)?.Name.LocalName;
        }

        public static XElement GetBaseGmlElement(XElement element)
        {
            return element.AncestorsAndSelf()
                .FirstOrDefault(element => element.Parent.Name.Namespace != Namespace.Gml);
        }

        public static XElement GetFeatureElement(XElement element)
        {
            return element.AncestorsAndSelf()
                .FirstOrDefault(element => element.Parent.Name.LocalName == "featureMember" || element.Parent.Name.LocalName == "featureMembers");
        }

        public static IEnumerable<XElement> GetFeatureGeometryElements(XElement featureElement)
        {
            return featureElement.Descendants()
                .Where(element => GmlGeometry.ElementNames.Contains(element.Name.LocalName) &&
                    element.Parent.Name.Namespace != element.Parent.GetNamespaceOfPrefix("gml"));
        }

        public static string GetFeatureGmlId(XElement element)
        {
            return GetFeatureElement(element)?.Attribute(Namespace.Gml + "id")?.Value;
        }

        public static XElement GetClosestGmlIdElement(XElement element)
        {
            return element.AncestorsAndSelf()
                .FirstOrDefault(element => element.Attribute(Namespace.Gml + "id") != null);
        }

        public static string GetClosestGmlId(XElement element)
        {
            return GetClosestGmlIdElement(element)?.Attribute(Namespace.Gml + "id")?.Value;
        }

        public static XLink GetXLink(XElement element)
        {
            if (element == null)
                return null;

            XName name = Namespace.XLink + "href";

            var xlink = element.Attribute(name)?.Value.Split("#") ?? Array.Empty<string>();

            if (xlink.Length != 2)
                return null;

            var fileName = !string.IsNullOrWhiteSpace(xlink[0]) ? xlink[0] : null;
            var gmlId = !string.IsNullOrWhiteSpace(xlink[1]) ? xlink[1] : null;

            return new XLink(fileName, gmlId);
        }

        public static string GetCoordinateSystem(string srsName, Dictionary<string, string> coordinateSystems)
        {
            if (string.IsNullOrWhiteSpace(srsName))
                return string.Empty;

            var match = _srsNameRegex.Match(srsName);

            if (!match.Success)
                return string.Empty;

            var epsg = match.Groups["epsg"].Value;

            return coordinateSystems.TryGetValue(epsg, out var coordinateSystem) ? coordinateSystem : string.Empty;
        }
    }
}
