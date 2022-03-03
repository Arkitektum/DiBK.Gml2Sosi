using DiBK.Gml2Sosi.Application.Mappers.Interfaces;
using DiBK.Gml2Sosi.Application.Models;
using DiBK.Gml2Sosi.Reguleringsplanforslag.Mappers.Interfaces;
using DiBK.Gml2Sosi.Reguleringsplanforslag.Models.SosiObjects;
using System.Xml.Linq;
using Wmhelp.XPath2;

namespace DiBK.Gml2Sosi.Reguleringsplanforslag.Mappers
{
    public class RpInfrastrukturSoneMapper : ISosiElementMapper<RpInfrastrukturSone>
    {
        private readonly IRpHensynSoneMapper _rpHensynSoneMapper;

        public RpInfrastrukturSoneMapper(
            IRpHensynSoneMapper rpHensynSoneMapper)
        {
            _rpHensynSoneMapper = rpHensynSoneMapper;
        }

        public RpInfrastrukturSone Map(XElement featureElement, GmlDocument document, ref int sequenceNumber)
        {
            var rpInfrastrukturSone = _rpHensynSoneMapper.Map<RpInfrastrukturSone>(featureElement, document, ref sequenceNumber);

            rpInfrastrukturSone.Infrastruktur = featureElement.XPath2SelectElement("*:infrastruktur")?.Value;

            return rpInfrastrukturSone;
        }
    }
}
