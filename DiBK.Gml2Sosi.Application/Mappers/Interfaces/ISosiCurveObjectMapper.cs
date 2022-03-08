using DiBK.Gml2Sosi.Application.Models;
using DiBK.Gml2Sosi.Application.Models.SosiObjects;
using System.Xml.Linq;

namespace DiBK.Gml2Sosi.Application.Mappers.Interfaces
{
    public interface ISosiCurveObjectMapper<TSosiCurveModel> where TSosiCurveModel : SosiCurveObject
    {
        TSosiCurveModel Map(XElement featureElement, XElement geomElement, GmlDocument document);
    }
}
