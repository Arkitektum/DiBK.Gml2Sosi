using DiBK.Gml2Sosi.Application.Models;
using System.Xml.Linq;

namespace DiBK.Gml2Sosi.Application.HttpClients.Codelist
{
    public interface ICodelistHttpClient
    {
        Task<List<CodelistItem>> GetMålemetoderAsync();
        Task<List<CodelistItem>> GetMålemetodeKoderAsync();
        Task<string> GetMålemetodeAsync(XElement featureElement);
    }
}
