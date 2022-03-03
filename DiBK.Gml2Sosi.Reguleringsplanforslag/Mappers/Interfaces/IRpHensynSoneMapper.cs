using DiBK.Gml2Sosi.Application.Models;
using DiBK.Gml2Sosi.Reguleringsplanforslag.Models.SosiObjects;
using System.Xml.Linq;

namespace DiBK.Gml2Sosi.Reguleringsplanforslag.Mappers.Interfaces
{
    public interface IRpHensynSoneMapper
    {
        TSosiModel Map<TSosiModel>(XElement featureElement, GmlDocument document, ref int sequenceNumber) where TSosiModel : RpHensynSone, new();
    }
}
