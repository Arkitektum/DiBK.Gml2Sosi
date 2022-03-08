using DiBK.Gml2Sosi.Application.Mappers.Interfaces;
using DiBK.Gml2Sosi.Application.Models;
using DiBK.Gml2Sosi.Application.Models.SosiObjects;
using DiBK.Gml2Sosi.Reguleringsplanforslag.Mappers.Interfaces;
using DiBK.Gml2Sosi.Reguleringsplanforslag.Models.SosiObjects;
using System.Xml.Linq;
using Wmhelp.XPath2;
using static DiBK.Gml2Sosi.Application.Helpers.MapperHelper;
using static DiBK.Gml2Sosi.Reguleringsplanforslag.Helpers.MapperHelper;

namespace DiBK.Gml2Sosi.Reguleringsplanforslag.Mappers
{
    public class RpHensynSoneMapper : IRpHensynSoneMapper
    {
        private readonly ISosiObjectTypeMapper _sosiObjectTypeMapper;
        private readonly ISosiMapper<NasjonalArealplanId> _nasjonalArealplanIdMapper;

        public RpHensynSoneMapper(
            ISosiObjectTypeMapper sosiObjectTypeMapper,
            ISosiMapper<NasjonalArealplanId> nasjonalArealplanIdMapper)
        {
            _sosiObjectTypeMapper = sosiObjectTypeMapper;
            _nasjonalArealplanIdMapper = nasjonalArealplanIdMapper;
        }

        public TSosiModel Map<TSosiModel>(XElement featureElement, GmlDocument document) where TSosiModel : RpHensynSone, new()
        {
            var rpHensynSone = _sosiObjectTypeMapper.Map<TSosiModel>(featureElement, document);
            var rpOmrådeElement = GetReferencedRpOmrådeElement(featureElement, document);

            rpHensynSone.NasjonalArealplanId = _nasjonalArealplanIdMapper.Map(featureElement, document);
            rpHensynSone.Vertikalnivå = rpOmrådeElement.XPath2SelectElement("*:vertikalnivå")?.Value;
            rpHensynSone.Beskrivelse = FormatText(featureElement.XPath2SelectElement("*:beskrivelse"));
            rpHensynSone.HensynSonenavn = FormatText(featureElement.XPath2SelectElement("*:hensynSonenavn"));
            rpHensynSone.ElementType = CartographicElementType.Flate;

            return rpHensynSone;
        }
    }
}
