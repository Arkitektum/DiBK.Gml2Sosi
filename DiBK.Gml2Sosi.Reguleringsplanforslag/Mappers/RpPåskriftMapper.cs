using DiBK.Gml2Sosi.Application.Helpers;
using DiBK.Gml2Sosi.Application.Mappers.Interfaces;
using DiBK.Gml2Sosi.Application.Models;
using DiBK.Gml2Sosi.Application.Models.Config;
using DiBK.Gml2Sosi.Application.Models.Geometries;
using DiBK.Gml2Sosi.Application.Models.SosiObjects;
using DiBK.Gml2Sosi.Reguleringsplanforslag.Models.SosiObjects;
using Microsoft.Extensions.Options;
using System.Xml.Linq;
using Wmhelp.XPath2;
using Namespace = DiBK.Gml2Sosi.Application.Constants.Namespace;

namespace DiBK.Gml2Sosi.Reguleringsplanforslag.Mappers
{
    public class RpPåskriftMapper : ISosiElementMapper<RpPåskrift>
    {
        private readonly ISosiObjectTypeMapper _sosiObjectTypeMapper;
        private readonly ISosiMapper<NasjonalArealplanId> _nasjonalArealplanIdMapper;
        private readonly DatasetSettings _settings;

        public RpPåskriftMapper(
            ISosiObjectTypeMapper sosiObjectTypeMapper,
            ISosiMapper<NasjonalArealplanId> nasjonalArealplanIdMapper,
            IOptions<DatasetConfiguration> options)
        {
            _sosiObjectTypeMapper = sosiObjectTypeMapper;
            _nasjonalArealplanIdMapper = nasjonalArealplanIdMapper;
            _settings = options.Value.Reguleringsplanforslag;
        }

        public RpPåskrift Map(XElement element, GmlDocument document, ref int sequenceNumber)
        {
            var rpPåskrift = _sosiObjectTypeMapper.Map<RpPåskrift>(element, document, ref sequenceNumber);
            var rpOmrådeElement = GetRpOmrådeElement(element, document);
            var geomElement = element.XPath2SelectElement("*:tekstplassering/*");
            var punkter = GeometryHelper.GetSosiPoints(geomElement, _settings.Resolution);
            var firstPunkt = punkter.First();

            punkter.Insert(0, SosiPoint.Create(firstPunkt.X * _settings.Resolution, firstPunkt.Y * _settings.Resolution, _settings.Resolution));

            rpPåskrift.NasjonalArealplanId = _nasjonalArealplanIdMapper.Map(element, document);
            rpPåskrift.PåskriftType = element.XPath2SelectElement("*:påskriftType")?.Value;
            rpPåskrift.Tekststreng = MapperHelper.FormatText(element.XPath2SelectElement("*:tekststreng"));
            rpPåskrift.Vertikalnivå = rpOmrådeElement?.XPath2SelectElement("*:vertikalnivå")?.Value;
            rpPåskrift.Points = punkter;
            rpPåskrift.ElementType = CartographicElementType.Tekst;

            return rpPåskrift;
        }

        private static XElement GetRpOmrådeElement(XElement element, GmlDocument document)
        {
            XName name = Namespace.XLink + "href";

            var refElement = GetReferencedElement(element, document, element => element.Attribute(name) != null);

            if (refElement.Name.LocalName == "RpOmråde")
                return refElement;

            var rpOmrådeElement = GetReferencedElement(refElement, document, element => element.Name.LocalName == "planområde" && element.Attribute(name) != null);

            return rpOmrådeElement.Name.LocalName == "RpOmråde" ? rpOmrådeElement : null;
        }

        private static XElement GetReferencedElement(XElement element, GmlDocument document, Func<XElement, bool> predicate)
        {
            var xLinkElement = element.Elements().FirstOrDefault(predicate);

            if (xLinkElement == null)
                return null;

            var xLink = GmlHelper.GetXLink(xLinkElement);

            if (xLink?.GmlId == null)
                return null;

            return document.GetElementByGmlId(xLink.GmlId);
        }
    }
}
