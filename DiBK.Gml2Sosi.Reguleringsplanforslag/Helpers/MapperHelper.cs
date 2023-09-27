using DiBK.Gml2Sosi.Application.Helpers;
using DiBK.Gml2Sosi.Application.Models;
using DiBK.Gml2Sosi.Reguleringsplanforslag.Constants;
using System.Xml.Linq;
using Wmhelp.XPath2;
using Namespace = DiBK.Gml2Sosi.Application.Constants.Namespace;

namespace DiBK.Gml2Sosi.Reguleringsplanforslag.Helpers
{
    public class MapperHelper
    {
        private const double THRESHOLD_DISTANCE = 0.5;

        public static XElement GetRpOmrådeElement(XElement featureElement, GmlDocument document)
        {
            var gmlId = featureElement.Attribute(Namespace.Gml + "id")?.Value;

            if (string.IsNullOrWhiteSpace(gmlId))
                return null;

            if (!FeatureMembers.RpOmrådeReferanser.TryGetValue(featureElement.Name.LocalName, out var refElementLocalName))
                return null;

            return document.Document
                .Descendants()
                .SingleOrDefault(element => element.Name.LocalName == "RpOmråde" &&
                    element.Elements()
                        .Any(element =>
                        {
                            if (element.Name.LocalName != refElementLocalName)
                                return false;

                            var xLink = GmlHelper.GetXLink(element);

                            return xLink?.GmlId == gmlId;
                        }));
        }

        public static XElement GetRpOmrådeElementByGeometry(XElement featureElement, GmlDocument document)
        {
            var geomElement = GmlHelper.GetFeatureGeometryElements(featureElement).FirstOrDefault();
            
            if (geomElement == null)
                return null;

            var indexedGeom = document.GetOrCreateGeometry(geomElement);

            if (!indexedGeom.IsValid)
                return null;

            var rpOmrådeElements = document.GetFeatureElements("RpOmråde");

            foreach (var rpOmrådeElement in rpOmrådeElements)
            {
                var områdeGeomElement = rpOmrådeElement.XPath2SelectElement("*:område/*");
                var indexedOmrådeGeom = document.GetOrCreateGeometry(områdeGeomElement);

                if (!indexedOmrådeGeom.IsValid)
                    continue;

                if (indexedGeom.Geometry.Distance(indexedOmrådeGeom.Geometry) <= THRESHOLD_DISTANCE)
                    return rpOmrådeElement;
            }

            return null;
        }
    }
}
