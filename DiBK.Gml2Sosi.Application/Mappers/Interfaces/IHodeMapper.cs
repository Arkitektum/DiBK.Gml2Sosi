using DiBK.Gml2Sosi.Application.Models.SosiObjects;
using System.Xml.Linq;

namespace DiBK.Gml2Sosi.Application.Mappers.Interfaces
{
    public interface IHodeMapper
    {
        void Map(XDocument document, string sosiVersion, string sosiLevel, string objectCatalog, double resolution, string verticalDatum, List<SosiElement> sosiElements);
    }
}
