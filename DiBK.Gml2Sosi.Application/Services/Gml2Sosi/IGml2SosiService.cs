using Microsoft.AspNetCore.Http;

namespace DiBK.Gml2Sosi.Application.Services.Gml2Sosi
{
    public interface IGml2SosiService
    {
        Task<MemoryStream> Gml2SosiAsync(IFormFile gmlFile);
    }
}
