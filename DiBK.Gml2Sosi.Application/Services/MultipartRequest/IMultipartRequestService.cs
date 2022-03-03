using Microsoft.AspNetCore.Http;

namespace DiBK.Gml2Sosi.Application.Services.MultipartRequest
{
    public interface IMultipartRequestService
    {
        Task<IFormFile> GetFileFromMultipart();
    }
}
