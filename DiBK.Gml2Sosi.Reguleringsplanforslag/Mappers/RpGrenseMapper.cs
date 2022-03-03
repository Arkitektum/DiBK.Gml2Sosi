using DiBK.Gml2Sosi.Application.Mappers;
using DiBK.Gml2Sosi.Application.Mappers.Interfaces;
using DiBK.Gml2Sosi.Application.Models;
using DiBK.Gml2Sosi.Reguleringsplanforslag.Models.SosiObjects;
using System.Xml.Linq;
using Wmhelp.XPath2;

namespace DiBK.Gml2Sosi.Reguleringsplanforslag.Mappers
{
    public class RpGrenseMapper : SosiCurveObjectMapper, ISosiElementMapper<RpGrense>
    {
        public RpGrenseMapper(
            ISosiObjectTypeMapper sosiObjectTypeMapper) : base(sosiObjectTypeMapper)
        {
        }

        public RpGrense Map(XElement featureElement, GmlDocument document, ref int sequenceNumber)
        {
            var geomElement = featureElement.XPath2SelectElement("*:grense/*");
            var rpGrense = MapCurveObject<RpGrense>(featureElement, geomElement, document, ref sequenceNumber);

            return rpGrense;
        }
    }
}
