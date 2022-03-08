using DiBK.Gml2Sosi.Application.Mappers.Interfaces;
using DiBK.Gml2Sosi.Application.Models;
using DiBK.Gml2Sosi.Reguleringsplanforslag.Mappers.Interfaces;
using DiBK.Gml2Sosi.Reguleringsplanforslag.Models.SosiObjects;
using System.Xml.Linq;
using Wmhelp.XPath2;

namespace DiBK.Gml2Sosi.Reguleringsplanforslag.Mappers
{
    public class RpFareSoneMapper : ISosiElementMapper<RpFareSone>
    {
        private readonly IRpHensynSoneMapper _rpHensynSoneMapper;

        public RpFareSoneMapper(
            IRpHensynSoneMapper rpHensynSoneMapper)
        {
            _rpHensynSoneMapper = rpHensynSoneMapper;
        }

        public RpFareSone Map(XElement featureElement, GmlDocument document)
        {
            var rpFareSone = _rpHensynSoneMapper.Map<RpFareSone>(featureElement, document);

            rpFareSone.Fare = featureElement.XPath2SelectElement("*:fare")?.Value;

            return rpFareSone;
        }
    }
}
