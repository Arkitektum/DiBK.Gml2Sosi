using DiBK.Gml2Sosi.Application.Mappers.Interfaces;
using DiBK.Gml2Sosi.Application.Models;
using DiBK.Gml2Sosi.Reguleringsplanforslag.Mappers.Interfaces;
using DiBK.Gml2Sosi.Reguleringsplanforslag.Models.SosiObjects;
using System.Xml.Linq;
using Wmhelp.XPath2;

namespace DiBK.Gml2Sosi.Reguleringsplanforslag.Mappers
{
    public class RpStøySoneMapper : ISosiElementMapper<RpStøySone>
    {
        private readonly IRpHensynSoneMapper _rpHensynSoneMapper;

        public RpStøySoneMapper(
            IRpHensynSoneMapper rpHensynSoneMapper)
        {
            _rpHensynSoneMapper = rpHensynSoneMapper;
        }

        public RpStøySone Map(XElement featureElement, GmlDocument document)
        {
            var rpStøySone = _rpHensynSoneMapper.Map<RpStøySone>(featureElement, document);

            rpStøySone.Støy = featureElement.XPath2SelectElement("*:støy")?.Value;

            return rpStøySone;
        }
    }
}
