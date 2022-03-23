using DiBK.Gml2Sosi.Application.Helpers;
using DiBK.Gml2Sosi.Application.HttpClients.Codelist;
using DiBK.Gml2Sosi.Application.Mappers;
using DiBK.Gml2Sosi.Application.Mappers.Interfaces;
using DiBK.Gml2Sosi.Application.Models;
using DiBK.Gml2Sosi.Application.Models.Config;
using DiBK.Gml2Sosi.Application.Models.SosiObjects;
using DiBK.Gml2Sosi.Reguleringsplanforslag.Constants;
using DiBK.Gml2Sosi.Reguleringsplanforslag.Mappers.Interfaces;
using DiBK.Gml2Sosi.Reguleringsplanforslag.Models.SosiObjects;
using System.Xml.Linq;
using Wmhelp.XPath2;
using static DiBK.Gml2Sosi.Application.Helpers.MapperHelper;

namespace DiBK.Gml2Sosi.Reguleringsplanforslag.Mappers
{
    public class RpHandlingOmrådeMapper : SosiCurveObjectMapper, IRpHandlingOmrådeMapper
    {     
        private readonly ISosiMapper<NasjonalArealplanId> _nasjonalArealplanIdMapper;
        private readonly DatasetSettings _settings;
        private const string FeatureMemberName = "RpHandlingOmråde";
        private const string KodeByggeGrense = "1211";

        public RpHandlingOmrådeMapper(
            ISosiObjectTypeMapper sosiObjectTypeMapper,
            ISosiMapper<NasjonalArealplanId> nasjonalArealplanIdMapper,
            ICodelistHttpClient codelistHttpClient,
            Datasets datasets) : base(sosiObjectTypeMapper, codelistHttpClient)
        {
            _nasjonalArealplanIdMapper = nasjonalArealplanIdMapper;
            _settings = datasets.GetSettings(Dataset.Reguleringsplanforslag);
        }

        public List<SosiElement> Map(GmlDocument document)
        {
            var start = DateTime.Now;
            var sosiElements = new List<SosiElement>();
            var featureElements = document.GetFeatureElements(FeatureMemberName);
            var elementCount = 0;

            foreach (var featureElement in featureElements)
            {
                var geomElement = GmlHelper.GetFeatureGeometryElements(featureElement).FirstOrDefault();
                var segmentElements = GeometryHelper.GetSegmentElementsOfSurfaceElement(geomElement);
                var curveObjects = new List<SosiCurveObject>();

                foreach (var segmentElement in segmentElements)
                    curveObjects.Add(Map(featureElement, segmentElement, document));

                SosiCurveObject.AddNodesToCurves(curveObjects);
                elementCount += curveObjects.Count;

                sosiElements.AddRange(curveObjects);
            }

            LogInformation<RpJuridiskLinje>(elementCount, start);

            return sosiElements;
        }

        private RpJuridiskLinje Map(XElement featureElement, XElement geomElement, GmlDocument document)
        {
            var rpJuridiskLinje = MapCurveObject<RpJuridiskLinje>(featureElement, geomElement, document, _settings.Resolution);
            var rpOmrådeElement = GetRpOmrådeElement(featureElement, document);

            rpJuridiskLinje.NasjonalArealplanId = _nasjonalArealplanIdMapper.Map(featureElement, document);
            rpJuridiskLinje.Vertikalnivå = rpOmrådeElement.XPath2SelectElement("*:vertikalnivå")?.Value;
            rpJuridiskLinje.JuridiskLinje = KodeByggeGrense;

            return rpJuridiskLinje;
        }

        private static XElement GetRpOmrådeElement(XElement featureElement, GmlDocument document)
        {
            var formålsområdeElement = featureElement.XPath2SelectElement("*:formålsområde");
            var formålsområdeXLink = GmlHelper.GetXLink(formålsområdeElement);
            var rpArealformålOmrådeElement = document.GetElementByGmlId(formålsområdeXLink.GmlId);

            if (rpArealformålOmrådeElement == null)
                return null;

            var planområdeElement = featureElement.XPath2SelectElement("*:planområde");
            var planområdeXLink = GmlHelper.GetXLink(planområdeElement);
            
            return document.GetElementByGmlId(planområdeXLink.GmlId);
        }
    }
}
