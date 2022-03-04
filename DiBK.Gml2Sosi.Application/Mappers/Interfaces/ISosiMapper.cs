using DiBK.Gml2Sosi.Application.Models;
using System.Xml.Linq;

namespace DiBK.Gml2Sosi.Application.Mappers.Interfaces
{
    public interface ISosiMapper<TSosiModel>
    {
        TSosiModel Map(XElement featureElement, GmlDocument document);
    }
}
