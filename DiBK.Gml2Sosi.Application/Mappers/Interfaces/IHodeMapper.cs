using DiBK.Gml2Sosi.Application.Models;
using DiBK.Gml2Sosi.Application.Models.Config;
using DiBK.Gml2Sosi.Application.Models.SosiObjects;

namespace DiBK.Gml2Sosi.Application.Mappers.Interfaces
{
    public interface IHodeMapper
    {
        Hode Map(GmlDocument document, DatasetSettings settings);
    }
}
