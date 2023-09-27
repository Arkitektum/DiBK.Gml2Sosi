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
    public class RpJuridiskLinjeMapper : SosiCurveObjectMapper, ISosiCurveObjectMapper<RpJuridiskLinje>
    {
        private readonly ISosiMapper<NasjonalArealplanId> _nasjonalArealplanIdMapper;        
        private readonly DatasetSettings _settings;

        public RpJuridiskLinjeMapper(
            ISosiObjectTypeMapper sosiObjectTypeMapper,
            ISosiMapper<NasjonalArealplanId> nasjonalArealplanIdMapper,
            ICodelistHttpClient codelistHttpClient,
            Datasets datasets) : base(sosiObjectTypeMapper, codelistHttpClient)
        {
            _nasjonalArealplanIdMapper = nasjonalArealplanIdMapper;
            _settings = datasets.GetSettings(Dataset.Reguleringsplanforslag);
        }

        public RpJuridiskLinje Map(XElement featureElement, XElement geomElement, GmlDocument document)
        {
            var rpJuridiskLinje = MapCurveObject<RpJuridiskLinje>(featureElement, geomElement, document, _settings.Resolution);

            rpJuridiskLinje.NasjonalArealplanId = _nasjonalArealplanIdMapper.Map(featureElement, document);
            rpJuridiskLinje.JuridiskLinje = featureElement.XPath2SelectElement("*:juridiskLinjetype")?.Value;

            var rpOmrådeElement = GetRpOmrådeElementByGeometry(featureElement, document);

            if (rpOmrådeElement != null)
                rpJuridiskLinje.Vertikalnivå = rpOmrådeElement.XPath2SelectElement("*:vertikalnivå")?.Value;

            return rpJuridiskLinje;
        }
    }
}
