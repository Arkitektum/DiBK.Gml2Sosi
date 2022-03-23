using DiBK.Gml2Sosi.Application.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Xml.Linq;
using Wmhelp.XPath2;

namespace DiBK.Gml2Sosi.Application.HttpClients.Codelist
{
    public class CodelistHttpClient : ICodelistHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _memoryCache;
        private readonly CodelistSettings _settings;
        private readonly ILogger<CodelistHttpClient> _logger;

        public CodelistHttpClient(
            HttpClient httpClient,
            IMemoryCache memoryCache,
            IOptions<CodelistSettings> options,
            ILogger<CodelistHttpClient> logger)
        {
            _httpClient = httpClient;
            _memoryCache = memoryCache;
            _settings = options.Value;
            _logger = logger;
        }

        public async Task<List<CodelistItem>> GetMålemetoderAsync()
        {
            return await GetCodelistAsync(_settings.Målemetode);
        }

        public async Task<List<CodelistItem>> GetMålemetodeKoderAsync()
        {
            return await GetCodelistAsync(_settings.MålemetodeKode);
        }

        public async Task<string> GetMålemetodeAsync(XElement featureElement)
        {
            var målemetodeValue = featureElement.XPath2SelectElement("*:kvalitet//*:målemetode")?.Value;
            var målemetodeKoder = await GetMålemetodeKoderAsync();

            if (målemetodeKoder.Any(metode => metode.Value == målemetodeValue))
                return målemetodeValue;

            var målemetoder = await GetMålemetoderAsync();
            var målemetode = målemetoder.SingleOrDefault(metode => metode.Value == målemetodeValue);

            if (målemetode == null)
                return målemetodeValue;

            return målemetodeKoder.SingleOrDefault(metode => metode.Name == målemetode.Name)?.Value ?? målemetodeValue;
        }

        private async Task<List<CodelistItem>> GetCodelistAsync(Uri uri)
        {
            return await _memoryCache.GetOrCreateAsync(uri, async entry =>
            {
                entry.SlidingExpiration = TimeSpan.FromDays(_settings.CacheDurationDays);
                return await FetchCodelistAsync(uri);
            });
        }

        private async Task<List<CodelistItem>> FetchCodelistAsync(Uri uri)
        {
            try
            {
                using var response = await _httpClient.GetAsync(uri);
                response.EnsureSuccessStatusCode();
                using var stream = await response.Content.ReadAsStreamAsync();

                return await CreateCodelistAsync(stream);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Kunne ikke laste ned data fra {absoluteUri}.", uri.AbsoluteUri);
                return new();
            }
        }

        private static async Task<List<CodelistItem>> CreateCodelistAsync(Stream stream)
        {
            var document = await XDocument.LoadAsync(stream, LoadOptions.None, default);

            return document
                .XPath2SelectElements("//*:dictionaryEntry/*:Definition")
                .Select(element =>
                {
                    return new CodelistItem(
                        element.XPath2SelectElement("*:name")?.Value,
                        element.XPath2SelectElement("*:identifier")?.Value,
                        element.XPath2SelectElement("*:description")?.Value
                    );
                })
                .OrderBy(feltnavn => feltnavn.Value)
                .ToList();
        }
    }
}
