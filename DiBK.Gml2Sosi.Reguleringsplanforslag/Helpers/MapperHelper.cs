using DiBK.Gml2Sosi.Application.Helpers;
using DiBK.Gml2Sosi.Application.Models;
using System.Xml.Linq;
using Wmhelp.XPath2;

namespace DiBK.Gml2Sosi.Reguleringsplanforslag.Helpers
{
    public class MapperHelper
    {
        public static XElement GetReferencedRpOmrådeElement(XElement featureElement, GmlDocument document)
        {
            var xLinkElement = featureElement.XPath2SelectElement("*:planområde");
            var xLink = GmlHelper.GetXLink(xLinkElement);

            return document.GetElementByGmlId(xLink?.GmlId ?? "");
        }
    }
}
