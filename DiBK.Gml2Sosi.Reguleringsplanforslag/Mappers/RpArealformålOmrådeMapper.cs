using DiBK.Gml2Sosi.Application.Mappers.Interfaces;
using DiBK.Gml2Sosi.Application.Models;
using DiBK.Gml2Sosi.Application.Models.SosiObjects;
using DiBK.Gml2Sosi.Reguleringsplanforslag.Models.SosiObjects;
using System.Xml.Linq;
using Wmhelp.XPath2;
using static DiBK.Gml2Sosi.Application.Helpers.MapperHelper;
using static DiBK.Gml2Sosi.Reguleringsplanforslag.Helpers.MapperHelper;

namespace DiBK.Gml2Sosi.Reguleringsplanforslag.Mappers
{
    public class RpArealformålOmrådeMapper : ISosiElementMapper<RpArealformålOmråde>
    {
        private readonly ISosiObjectTypeMapper _sosiObjectTypeMapper;
        private readonly ISosiMapper<NasjonalArealplanId> _nasjonalArealplanIdMapper;

        public RpArealformålOmrådeMapper(
            ISosiObjectTypeMapper sosiObjectTypeMapper,
            ISosiMapper<NasjonalArealplanId> nasjonalArealplanIdMapper)
        {
            _sosiObjectTypeMapper = sosiObjectTypeMapper;
            _nasjonalArealplanIdMapper = nasjonalArealplanIdMapper;
        }

        public RpArealformålOmråde Map(XElement featureElement, GmlDocument document)
        {
            var rpArealformålOmråde = _sosiObjectTypeMapper.Map<RpArealformålOmråde>(featureElement, document);
            var rpOmrådeElement = GetReferencedRpOmrådeElement(featureElement, document);

            rpArealformålOmråde.NasjonalArealplanId = _nasjonalArealplanIdMapper.Map(featureElement, document);
            rpArealformålOmråde.Vertikalnivå = rpOmrådeElement.XPath2SelectElement("*:vertikalnivå")?.Value;
            rpArealformålOmråde.Arealformål = featureElement.XPath2SelectElement("*:arealformål")?.Value;
            rpArealformålOmråde.Eierform = featureElement.XPath2SelectElement("*:eierform")?.Value;
            rpArealformålOmråde.Beskrivelse = FormatText(featureElement.XPath2SelectElement("*:beskrivelse"));
            rpArealformålOmråde.Feltnavn = FormatText(featureElement.XPath2SelectElement("*:feltnavn"));
            rpArealformålOmråde.Uteoppholdsareal = featureElement.XPath2SelectElement("*:uteoppholdsareal")?.Value;
            rpArealformålOmråde.Byggverkbestemmelse = featureElement.XPath2SelectElement("*:byggverkbestemmelse")?.Value;
            rpArealformålOmråde.Avkjørselsbestemmelse = featureElement.XPath2SelectElement("*:avkjørselsbestemmelse")?.Value;
            rpArealformålOmråde.ElementType = CartographicElementType.Flate;

            var utnyttingElement = featureElement.XPath2SelectElement("*:utnytting/*:Utnytting");

            if (utnyttingElement != null)
            {
                rpArealformålOmråde.Utnytting = new()
                {
                    Utnyttingstype = utnyttingElement.XPath2SelectElement("*:utnyttingstype")?.Value,
                    Utnyttingstall = utnyttingElement.XPath2SelectElement("*:utnyttingstall")?.Value,
                    UtnyttingstallMinimum = utnyttingElement.XPath2SelectElement("*:utnyttingstall_minimum")?.Value
                };
            }

            return rpArealformålOmråde;
        }
    }
}
