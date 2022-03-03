using DiBK.Gml2Sosi.Application.Mappers.Interfaces;
using DiBK.Gml2Sosi.Application.Models;
using DiBK.Gml2Sosi.Reguleringsplanforslag.Mappers.Interfaces;
using DiBK.Gml2Sosi.Reguleringsplanforslag.Models.SosiObjects;
using System.Xml.Linq;
using Wmhelp.XPath2;

namespace DiBK.Gml2Sosi.Reguleringsplanforslag.Mappers
{
    public class RpAngittHensynSoneMapper : ISosiElementMapper<RpAngittHensynSone>
    {
        private readonly IRpHensynSoneMapper _rpHensynSoneMapper;

        public RpAngittHensynSoneMapper(
            IRpHensynSoneMapper rpHensynSoneMapper)
        {
            _rpHensynSoneMapper = rpHensynSoneMapper;
        }

        public RpAngittHensynSone Map(XElement featureElement, GmlDocument document, ref int sequenceNumber)
        {
            var rpAngittHensynSone = _rpHensynSoneMapper.Map<RpAngittHensynSone>(featureElement, document, ref sequenceNumber);

            rpAngittHensynSone.AngittHensyn = featureElement.XPath2SelectElement("*:angittHensyn")?.Value;

            return rpAngittHensynSone;
        }
    }
}
