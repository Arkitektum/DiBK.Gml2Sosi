using DiBK.Gml2Sosi.Application.Models;
using DiBK.Gml2Sosi.Application.Models.SosiObjects;

namespace DiBK.Gml2Sosi.Application.Mappers.Interfaces
{
    public interface ISosiObjectMapper
    {
        List<SosiElement> MapSosiObjects<TSosiModel>(GmlDocument document)
            where TSosiModel : SosiElement, new();

        List<SosiElement> MapSosiCurveObjects<TSosiCurveModel>(GmlDocument document, bool addNodes)
            where TSosiCurveModel : SosiCurveObject, new();

        List<SosiElement> MapSosiSurfaceAndCurveObjects<TSosiSurfaceModel, TSosiCurveModel>(GmlDocument document, double resolution)
            where TSosiSurfaceModel : SosiSurfaceObject
            where TSosiCurveModel : SosiCurveObject, new();
    }
}
