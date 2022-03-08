using DiBK.Gml2Sosi.Application.Mappers.Interfaces;
using DiBK.Gml2Sosi.Application.Models;
using DiBK.Gml2Sosi.Reguleringsplanforslag.Mappers.Interfaces;
using DiBK.Gml2Sosi.Reguleringsplanforslag.Models.SosiObjects;
using System.Xml.Linq;
using Wmhelp.XPath2;

namespace DiBK.Gml2Sosi.Reguleringsplanforslag.Mappers
{
    public class RpGjennomføringSoneMapper : ISosiElementMapper<RpGjennomføringSone>
    {
        private readonly IRpHensynSoneMapper _rpHensynSoneMapper;

        public RpGjennomføringSoneMapper(
            IRpHensynSoneMapper rpHensynSoneMapper)
        {
            _rpHensynSoneMapper = rpHensynSoneMapper;
        }

        public RpGjennomføringSone Map(XElement featureElement, GmlDocument document)
        {
            var rpGjennomføringSone = _rpHensynSoneMapper.Map<RpGjennomføringSone>(featureElement, document);

            rpGjennomføringSone.Gjennomføring = featureElement.XPath2SelectElement("*:gjennomføring")?.Value;

            return rpGjennomføringSone;
        }
    }
}
