using DiBK.Gml2Sosi.Application.Models.Config;
using DiBK.Gml2Sosi.Application.Models.SosiObjects;
using System.Xml.Linq;

namespace DiBK.Gml2Sosi.Application.Mappers.Interfaces
{
    public interface IHodeMapper
    {
        void Map(XDocument document, DatasetSettings datasetSettings, List<SosiElement> sosiElements);
    }
}
