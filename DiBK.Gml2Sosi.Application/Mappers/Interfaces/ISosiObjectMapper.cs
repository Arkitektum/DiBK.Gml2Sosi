using DiBK.Gml2Sosi.Application.Models;
using DiBK.Gml2Sosi.Application.Models.SosiObjects;

namespace DiBK.Gml2Sosi.Application.Mappers.Interfaces
{
    public interface ISosiObjectMapper
    {
        void MapSosiObjects<TSosiModel>(GmlDocument document, ref int sequenceNumber, List<SosiElement> sosiElements)
            where TSosiModel : SosiElement, new();
        
        void MapSosiCurveObjects<TSosiCurveModel>(GmlDocument document, bool addNodes, ref int sequenceNumber, List<SosiElement> sosiElements)
            where TSosiCurveModel : SosiCurveObject, new();

        void MapSosiSurfaceAndCurveObjects<TSosiSurfaceModel, TSosiCurveModel>(GmlDocument document, ref int sequenceNumber, List<SosiElement> sosiElements, double resolution)
            where TSosiSurfaceModel : SosiSurfaceObject
            where TSosiCurveModel : SosiCurveObject, new();
    }
}
