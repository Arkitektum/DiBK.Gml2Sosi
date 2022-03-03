using DiBK.Gml2Sosi.Application.Helpers;
using DiBK.Gml2Sosi.Application.Mappers;
using DiBK.Gml2Sosi.Application.Mappers.Interfaces;
using DiBK.Gml2Sosi.Application.Models;
using DiBK.Gml2Sosi.Application.Models.SosiObjects;
using DiBK.Gml2Sosi.Reguleringsplanforslag.Mappers.Interfaces;
using DiBK.Gml2Sosi.Reguleringsplanforslag.Models.SosiObjects;
using System.Xml.Linq;
using Wmhelp.XPath2;

namespace DiBK.Gml2Sosi.Reguleringsplanforslag.Mappers
{
    public class RpHandlingOmrådeMapper : SosiCurveObjectMapper, IRpHandlingOmrådeMapper
    {
        private readonly ISosiMapper<NasjonalArealplanId> _nasjonalArealplanIdMapper;
        private const string FeatureMemberName = "RpHandlingOmråde";
        private const string KodeByggeGrense = "1211";

        public RpHandlingOmrådeMapper(
            ISosiObjectTypeMapper sosiObjectTypeMapper,
            ISosiMapper<NasjonalArealplanId> nasjonalArealplanIdMapper) : base(sosiObjectTypeMapper)
        {
            _nasjonalArealplanIdMapper = nasjonalArealplanIdMapper;
        }

        public void Map(GmlDocument document, ref int sequenceNumber, List<SosiElement> sosiObjects)
        {
            var featureElements = document.GetFeatureElements(FeatureMemberName);

            foreach (var featureElement in featureElements)
            {
                var geomElement = GmlHelper.GetFeatureGeometryElements(featureElement).FirstOrDefault();
                var segmentElements = SurfaceHelper.GetSegmentElementsOfSurfaceElement(geomElement);
                var curveObjects = new List<SosiCurveObject>();

                foreach (var segmentElement in segmentElements)
                    curveObjects.Add(Map(featureElement, segmentElement, document, ref sequenceNumber));

                SosiCurveObject.AddNodesToCurves(curveObjects);

                sosiObjects.AddRange(curveObjects);
            }
        }

        private RpJuridiskLinje Map(XElement featureElement, XElement geomElement, GmlDocument document, ref int sequenceNumber)
        {
            var rpJuridiskLinje = MapCurveObject<RpJuridiskLinje>(featureElement, geomElement, document, ref sequenceNumber);
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
