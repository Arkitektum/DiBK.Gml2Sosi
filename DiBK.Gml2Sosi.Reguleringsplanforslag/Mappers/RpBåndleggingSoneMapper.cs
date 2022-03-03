using DiBK.Gml2Sosi.Application.Mappers.Interfaces;
using DiBK.Gml2Sosi.Application.Models;
using DiBK.Gml2Sosi.Reguleringsplanforslag.Mappers.Interfaces;
using DiBK.Gml2Sosi.Reguleringsplanforslag.Models.SosiObjects;
using System.Xml.Linq;
using Wmhelp.XPath2;
using static DiBK.Gml2Sosi.Application.Helpers.MapperHelper;

namespace DiBK.Gml2Sosi.Reguleringsplanforslag.Mappers
{
    public class RpBåndleggingSoneMapper : ISosiElementMapper<RpBåndleggingSone>
    {
        private readonly IRpHensynSoneMapper _rpHensynSoneMapper;

        public RpBåndleggingSoneMapper(
            IRpHensynSoneMapper rpHensynSoneMapper)
        {
            _rpHensynSoneMapper = rpHensynSoneMapper;
        }

        public RpBåndleggingSone Map(XElement featureElement, GmlDocument document, ref int sequenceNumber)
        {
            var rpBåndleggingSone = _rpHensynSoneMapper.Map<RpBåndleggingSone>(featureElement, document, ref sequenceNumber);

            rpBåndleggingSone.Båndlegging = featureElement.XPath2SelectElement("*:båndlegging")?.Value;
            rpBåndleggingSone.BåndlagtFremTil = FormatDate(featureElement.XPath2SelectElement("*:båndlagtFremTil"));

            return rpBåndleggingSone;
        }
    }
}
