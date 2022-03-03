using DiBK.Gml2Sosi.Application.Mappers;
using DiBK.Gml2Sosi.Application.Mappers.Interfaces;
using DiBK.Gml2Sosi.Application.Models;
using DiBK.Gml2Sosi.Reguleringsplanforslag.Models.SosiObjects;
using System.Xml.Linq;
using Wmhelp.XPath2;

namespace DiBK.Gml2Sosi.Reguleringsplanforslag.Mappers
{
    public class RpFormålGrenseMapper : SosiCurveObjectMapper, ISosiElementMapper<RpFormålGrense>
    {
        public RpFormålGrenseMapper(
            ISosiObjectTypeMapper sosiBaseObjectMapper) : base(sosiBaseObjectMapper)
        {
        }

        public RpFormålGrense Map(XElement featureElement, GmlDocument document, ref int sequenceNumber)
        {
            var geomElement = featureElement.XPath2SelectElement("*:grense/*");
            var rpFormålGrense = MapCurveObject<RpFormålGrense>(featureElement, geomElement, document, ref sequenceNumber);

            return rpFormålGrense;
        }
    }
}
