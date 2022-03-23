using DiBK.Gml2Sosi.Application.Helpers;
using DiBK.Gml2Sosi.Application.HttpClients.Codelist;
using DiBK.Gml2Sosi.Application.Mappers.Interfaces;
using DiBK.Gml2Sosi.Application.Models;
using DiBK.Gml2Sosi.Application.Models.Config;
using DiBK.Gml2Sosi.Application.Models.Geometries;
using DiBK.Gml2Sosi.Application.Models.SosiObjects;
using DiBK.Gml2Sosi.Reguleringsplanforslag.Constants;
using DiBK.Gml2Sosi.Reguleringsplanforslag.Models.SosiObjects;
using System.Xml.Linq;
using Wmhelp.XPath2;
using Namespace = DiBK.Gml2Sosi.Application.Constants.Namespace;

namespace DiBK.Gml2Sosi.Reguleringsplanforslag.Mappers
{
    public class RpPåskriftMapper : ISosiElementMapper<RpPåskrift>
    {
        private readonly ISosiObjectTypeMapper _sosiObjectTypeMapper;
        private readonly ISosiMapper<NasjonalArealplanId> _nasjonalArealplanIdMapper;
        private readonly ICodelistHttpClient _codelistHttpClient;
        private readonly DatasetSettings _settings;

        public RpPåskriftMapper(
            ISosiObjectTypeMapper sosiObjectTypeMapper,
            ISosiMapper<NasjonalArealplanId> nasjonalArealplanIdMapper,
            ICodelistHttpClient codelistHttpClient,
            Datasets datasets)
        {
            _sosiObjectTypeMapper = sosiObjectTypeMapper;
            _nasjonalArealplanIdMapper = nasjonalArealplanIdMapper;
            _codelistHttpClient = codelistHttpClient;
            _settings = datasets.GetSettings(Dataset.Reguleringsplanforslag);
        }

        public RpPåskrift Map(XElement featureElement, GmlDocument document)
        {
            var rpPåskrift = _sosiObjectTypeMapper.Map<RpPåskrift>(featureElement, document);
            var rpOmrådeElement = GetRpOmrådeElement(featureElement, document);
            var geomElement = featureElement.XPath2SelectElement("*:tekstplassering/*");
            var punkter = GeometryHelper.GetSosiPoints(geomElement, _settings.Resolution);
            var firstPunkt = punkter.First();

            punkter.Insert(0, SosiPoint.Create(firstPunkt.X * _settings.Resolution, firstPunkt.Y * _settings.Resolution, _settings.Resolution));

            rpPåskrift.NasjonalArealplanId = _nasjonalArealplanIdMapper.Map(featureElement, document);
            rpPåskrift.PåskriftType = featureElement.XPath2SelectElement("*:påskriftType")?.Value;
            rpPåskrift.Tekststreng = MapperHelper.FormatText(featureElement.XPath2SelectElement("*:tekststreng"));
            rpPåskrift.Vertikalnivå = rpOmrådeElement?.XPath2SelectElement("*:vertikalnivå")?.Value;
            rpPåskrift.Kvalitet = _codelistHttpClient.GetMålemetodeAsync(featureElement).Result;
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
