using DiBK.Gml2Sosi.Application.Mappers;
using DiBK.Gml2Sosi.Application.Mappers.Interfaces;
using DiBK.Gml2Sosi.Application.Models;
using DiBK.Gml2Sosi.Reguleringsplanforslag.Models.SosiObjects;
using System.Xml.Linq;
using Wmhelp.XPath2;
using static DiBK.Gml2Sosi.Reguleringsplanforslag.Helpers.MapperHelper;

namespace DiBK.Gml2Sosi.Reguleringsplanforslag.Mappers
{
    public class RpRegulertHøydeMapper : SosiCurveObjectMapper, ISosiCurveObjectMapper<RpRegulertHøyde>
    {
        private readonly ISosiMapper<NasjonalArealplanId> _nasjonalArealplanIdMapper;

        public RpRegulertHøydeMapper(
            ISosiObjectTypeMapper sosiBaseObjectMapper,
            ISosiMapper<NasjonalArealplanId> nasjonalArealplanIdMapper) : base(sosiBaseObjectMapper)
        {
            _nasjonalArealplanIdMapper = nasjonalArealplanIdMapper;
        }

        public RpRegulertHøyde Map(XElement featureElement, XElement geomElement, GmlDocument document, ref int sequenceNumber)
        {
            var rpRegulertHøyde = MapCurveObject<RpRegulertHøyde>(featureElement, geomElement, document, ref sequenceNumber);
            var rpOmrådeElement = GetReferencedRpOmrådeElement(featureElement, document);

            rpRegulertHøyde.NasjonalArealplanId = _nasjonalArealplanIdMapper.Map(featureElement, document);
            rpRegulertHøyde.Vertikalnivå = rpOmrådeElement.XPath2SelectElement("*:vertikalnivå")?.Value;

            var høydeFraPlanbestemmelseElement = featureElement.XPath2SelectElement("*:høydeFraPlanbestemmelse/*:HøydeFraPlanbestemmelse");

            rpRegulertHøyde.HøydeFraPlanbestemmelse.Regulerthøyde = høydeFraPlanbestemmelseElement.XPath2SelectElement("*:regulerthøyde")?.Value;
            rpRegulertHøyde.HøydeFraPlanbestemmelse.Høydereferansesystem = høydeFraPlanbestemmelseElement.XPath2SelectElement("*:høydereferansesystem")?.Value;

            return rpRegulertHøyde;
        }
    }
}
