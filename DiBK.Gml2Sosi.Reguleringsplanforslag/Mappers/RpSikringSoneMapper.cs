using DiBK.Gml2Sosi.Application.Mappers.Interfaces;
using DiBK.Gml2Sosi.Application.Models;
using DiBK.Gml2Sosi.Reguleringsplanforslag.Mappers.Interfaces;
using DiBK.Gml2Sosi.Reguleringsplanforslag.Models.SosiObjects;
using System.Xml.Linq;
using Wmhelp.XPath2;

namespace DiBK.Gml2Sosi.Reguleringsplanforslag.Mappers
{
    public class RpSikringSoneMapper : ISosiElementMapper<RpSikringSone>
    {
        private readonly IRpHensynSoneMapper _rpHensynSoneMapper;

        public RpSikringSoneMapper(
            IRpHensynSoneMapper rpHensynSoneMapper)
        {
            _rpHensynSoneMapper = rpHensynSoneMapper;
        }

        public RpSikringSone Map(XElement featureElement, GmlDocument document, ref int sequenceNumber)
        {
            var rpSikringSone = _rpHensynSoneMapper.Map<RpSikringSone>(featureElement, document, ref sequenceNumber);

            rpSikringSone.Sikring = featureElement.XPath2SelectElement("*:sikring")?.Value;

            return rpSikringSone;
        }
    }
}
