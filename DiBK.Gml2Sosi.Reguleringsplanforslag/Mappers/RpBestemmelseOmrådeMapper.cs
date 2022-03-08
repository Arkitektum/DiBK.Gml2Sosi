using DiBK.Gml2Sosi.Application.Mappers.Interfaces;
using DiBK.Gml2Sosi.Application.Models;
using DiBK.Gml2Sosi.Application.Models.SosiObjects;
using DiBK.Gml2Sosi.Reguleringsplanforslag.Models.SosiObjects;
using System.Xml.Linq;
using Wmhelp.XPath2;
using static DiBK.Gml2Sosi.Reguleringsplanforslag.Helpers.MapperHelper;

namespace DiBK.Gml2Sosi.Reguleringsplanforslag.Mappers
{
    public class RpBestemmelseOmrådeMapper : ISosiElementMapper<RpBestemmelseOmråde>
    {
        private readonly ISosiObjectTypeMapper _sosiObjectTypeMapper;
        private readonly ISosiMapper<NasjonalArealplanId> _nasjonalArealplanIdMapper;

        public RpBestemmelseOmrådeMapper(
            ISosiObjectTypeMapper sosiObjectTypeMapper,
            ISosiMapper<NasjonalArealplanId> nasjonalArealplanIdMapper)
        {
            _sosiObjectTypeMapper = sosiObjectTypeMapper;
            _nasjonalArealplanIdMapper = nasjonalArealplanIdMapper;
        }

        public RpBestemmelseOmråde Map(XElement featureElement, GmlDocument document)
        {
            var rpBestemmelseOmråde = _sosiObjectTypeMapper.Map<RpBestemmelseOmråde>(featureElement, document);
            var rpOmrådeElement = GetReferencedRpOmrådeElement(featureElement, document);

            rpBestemmelseOmråde.NasjonalArealplanId = _nasjonalArealplanIdMapper.Map(featureElement, document);
            rpBestemmelseOmråde.Vertikalnivå = rpOmrådeElement.XPath2SelectElement("*:vertikalnivå")?.Value;
            rpBestemmelseOmråde.BestemmelseOmrådeNavn = featureElement.XPath2SelectElement("*:bestemmelseOmrådeNavn")?.Value;
            rpBestemmelseOmråde.BestemmelseHjemmel = featureElement.XPath2SelectElement("*:bestemmelseHjemmel")?.Value;
            rpBestemmelseOmråde.ElementType = CartographicElementType.Flate;

            return rpBestemmelseOmråde;
        }
    }
}
