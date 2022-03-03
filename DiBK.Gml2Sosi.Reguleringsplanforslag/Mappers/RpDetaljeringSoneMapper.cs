using DiBK.Gml2Sosi.Application.Mappers.Interfaces;
using DiBK.Gml2Sosi.Application.Models;
using DiBK.Gml2Sosi.Reguleringsplanforslag.Mappers.Interfaces;
using DiBK.Gml2Sosi.Reguleringsplanforslag.Models.SosiObjects;
using System.Xml.Linq;
using Wmhelp.XPath2;

namespace DiBK.Gml2Sosi.Reguleringsplanforslag.Mappers
{
    public class RpDetaljeringSoneMapper : ISosiElementMapper<RpDetaljeringSone>
    {
        private readonly IRpHensynSoneMapper _rpHensynSoneMapper;

        public RpDetaljeringSoneMapper(
            IRpHensynSoneMapper rpHensynSoneMapper)
        {
            _rpHensynSoneMapper = rpHensynSoneMapper;
        }

        public RpDetaljeringSone Map(XElement featureElement, GmlDocument document, ref int sequenceNumber)
        {
            var rpDetaljeringSone = _rpHensynSoneMapper.Map<RpDetaljeringSone>(featureElement, document, ref sequenceNumber);

            rpDetaljeringSone.Detaljering = featureElement.XPath2SelectElement("*:detaljering")?.Value;

            return rpDetaljeringSone;
        }
    }
}
