using DiBK.Gml2Sosi.Application.Models;
using DiBK.Gml2Sosi.Application.Models.SosiObjects;
using System.Xml.Linq;

namespace DiBK.Gml2Sosi.Application.Mappers.Interfaces
{
    public interface ISosiObjectTypeMapper
    {
        TSosiModel Map<TSosiModel>(XElement featureElement, GmlDocument document) 
            where TSosiModel : SosiObjectType, new();
    }
}
