using DiBK.Gml2Sosi.Application.Models;
using DiBK.Gml2Sosi.Application.Models.SosiObjects;

namespace DiBK.Gml2Sosi.Reguleringsplanforslag.Mappers.Interfaces
{
    public interface IRpHandlingOmrådeMapper
    {
        void Map(GmlDocument document, List<SosiElement> sosiObjects);
    }
}
