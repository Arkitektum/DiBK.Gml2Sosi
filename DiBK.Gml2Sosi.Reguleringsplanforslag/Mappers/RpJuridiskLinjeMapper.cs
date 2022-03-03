using DiBK.Gml2Sosi.Application.Mappers;
using DiBK.Gml2Sosi.Application.Mappers.Interfaces;
using DiBK.Gml2Sosi.Application.Models;
using DiBK.Gml2Sosi.Reguleringsplanforslag.Models.SosiObjects;
using System.Xml.Linq;
using Wmhelp.XPath2;
using static DiBK.Gml2Sosi.Reguleringsplanforslag.Helpers.MapperHelper;

namespace DiBK.Gml2Sosi.Reguleringsplanforslag.Mappers
{
    public class RpJuridiskLinjeMapper : SosiCurveObjectMapper, ISosiCurveObjectMapper<RpJuridiskLinje>
    {
        private readonly ISosiMapper<NasjonalArealplanId> _nasjonalArealplanIdMapper;

        public RpJuridiskLinjeMapper(
            ISosiObjectTypeMapper sosiObjectTypeMapper,
            ISosiMapper<NasjonalArealplanId> nasjonalArealplanIdMapper) : base(sosiObjectTypeMapper)
        {
            _nasjonalArealplanIdMapper = nasjonalArealplanIdMapper;
        }

        public RpJuridiskLinje Map(XElement featureElement, XElement geomElement, GmlDocument document, ref int sequenceNumber)
        {
            var rpJuridiskLinje = MapCurveObject<RpJuridiskLinje>(featureElement, geomElement, document, ref sequenceNumber);

            rpJuridiskLinje.NasjonalArealplanId = _nasjonalArealplanIdMapper.Map(featureElement, document);
            rpJuridiskLinje.JuridiskLinje = featureElement.XPath2SelectElement("*:juridisklinje")?.Value;

            var rpOmrådeElement = GetReferencedRpOmrådeElement(featureElement, document);

            if (rpOmrådeElement != null)
                rpJuridiskLinje.Vertikalnivå = rpOmrådeElement.XPath2SelectElement("*:vertikalnivå")?.Value;

            return rpJuridiskLinje;
        }
    }
}
