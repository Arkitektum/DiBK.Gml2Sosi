using DiBK.Gml2Sosi.Application.Models;
using DiBK.Gml2Sosi.Application.Models.SosiObjects;

namespace DiBK.Gml2Sosi.Reguleringsplanforslag.Mappers.Interfaces
{
    public interface IRpHandlingOmrådeMapper
    {
        List<SosiElement> Map(GmlDocument document);
    }
}
