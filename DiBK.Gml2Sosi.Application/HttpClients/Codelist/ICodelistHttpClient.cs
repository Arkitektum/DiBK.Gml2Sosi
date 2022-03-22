using DiBK.Gml2Sosi.Application.Models;

namespace DiBK.Gml2Sosi.Application.HttpClients.Codelist
{
    public interface ICodelistHttpClient
    {
        Task<List<CodelistItem>> GetMålemetoderAsync();
        Task<List<CodelistItem>> GetMålemetodeKoderAsync();
    }
}
