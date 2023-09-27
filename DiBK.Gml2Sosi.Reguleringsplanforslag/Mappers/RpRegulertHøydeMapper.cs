using DiBK.Gml2Sosi.Application.HttpClients.Codelist;
using DiBK.Gml2Sosi.Application.Mappers;
using DiBK.Gml2Sosi.Application.Mappers.Interfaces;
using DiBK.Gml2Sosi.Application.Models;
using DiBK.Gml2Sosi.Application.Models.Config;
using DiBK.Gml2Sosi.Reguleringsplanforslag.Constants;
using DiBK.Gml2Sosi.Reguleringsplanforslag.Models.SosiObjects;
using System.Xml.Linq;
using Wmhelp.XPath2;
using static DiBK.Gml2Sosi.Reguleringsplanforslag.Helpers.MapperHelper;

namespace DiBK.Gml2Sosi.Reguleringsplanforslag.Mappers
{
    public class RpRegulertHøydeMapper : SosiCurveObjectMapper, ISosiCurveObjectMapper<RpRegulertHøyde>
    {
        private readonly ISosiMapper<NasjonalArealplanId> _nasjonalArealplanIdMapper;
        private readonly DatasetSettings _settings;

        public RpRegulertHøydeMapper(
            ISosiObjectTypeMapper sosiBaseObjectMapper,
            ISosiMapper<NasjonalArealplanId> nasjonalArealplanIdMapper,
            ICodelistHttpClient codelistHttpClient,
            Datasets datasets) : base(sosiBaseObjectMapper, codelistHttpClient)
        {
            _nasjonalArealplanIdMapper = nasjonalArealplanIdMapper;
            _settings = datasets.GetSettings(Dataset.Reguleringsplanforslag);
        }

        public RpRegulertHøyde Map(XElement featureElement, XElement geomElement, GmlDocument document)
        {
            var rpRegulertHøyde = MapCurveObject<RpRegulertHøyde>(featureElement, geomElement, document, _settings.Resolution);
            var rpOmrådeElement = GetRpOmrådeElementByGeometry(featureElement, document);

            rpRegulertHøyde.NasjonalArealplanId = _nasjonalArealplanIdMapper.Map(featureElement, document);
            rpRegulertHøyde.Vertikalnivå = rpOmrådeElement.XPath2SelectElement("*:vertikalnivå")?.Value;

            var høydeFraPlanbestemmelseElement = featureElement.XPath2SelectElement("*:høydeFraPlanbestemmelse/*:HøydeFraPlanbestemmelse");

            rpRegulertHøyde.HøydeFraPlanbestemmelse.Regulerthøyde = høydeFraPlanbestemmelseElement.XPath2SelectElement("*:regulertHøyde")?.Value;
            rpRegulertHøyde.HøydeFraPlanbestemmelse.Høydereferansesystem = høydeFraPlanbestemmelseElement.XPath2SelectElement("//*:høydereferansesystem")?.Value;

            return rpRegulertHøyde;
        }
    }
}
