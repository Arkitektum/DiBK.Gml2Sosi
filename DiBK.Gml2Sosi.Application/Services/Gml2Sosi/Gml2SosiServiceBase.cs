using DiBK.Gml2Sosi.Application.Mappers.Interfaces;
using DiBK.Gml2Sosi.Application.Models;
using DiBK.Gml2Sosi.Application.Models.Config;
using DiBK.Gml2Sosi.Application.Models.SosiObjects;
using Microsoft.Extensions.Logging;

namespace DiBK.Gml2Sosi.Application.Services.Gml2Sosi
{
    public abstract class Gml2SosiServiceBase
    {
        private readonly IHodeMapper _hodeMapper;
        private readonly ILogger<Gml2SosiServiceBase> _logger;

        protected Gml2SosiServiceBase(
            IHodeMapper hodeMapper,
            ILogger<Gml2SosiServiceBase> logger)
        {
            _hodeMapper = hodeMapper;
            _logger = logger;
        }

        protected async Task<MemoryStream> CreateSosiDocumentAsync(
            GmlDocument document, DatasetSettings settings, Func<List<SosiElement>>[] mappingActions)
        {
            var start = DateTime.Now;
            var sosiElements = await RunMappingActionsAsync(mappingActions);
            var hode = _hodeMapper.Map(document, settings);

            sosiElements.Insert(0, hode);

            for (var i = 0; i < sosiElements.Count; i++)
                sosiElements[i].SequenceNumber = i;

            var stream = await SosiElement.WriteAllToStreamAsync(sosiElements);
            var timeUsed = Math.Round(DateTime.Now.Subtract(start).TotalSeconds, 5);

            _logger.LogInformation("Genererte {elementCount} elementer på {timeUsed} sek.", sosiElements.Count, timeUsed);

            return stream;
        }

        private static async Task<List<SosiElement>> RunMappingActionsAsync(Func<List<SosiElement>>[] mappingActions)
        {
            var tasks = new List<Task<List<SosiElement>>>();
            var sosiElements = new List<SosiElement>();

            foreach (var action in mappingActions)
                tasks.Add(Task.Run(action));

            await Task.WhenAll(tasks);

            foreach (var task in tasks)
                sosiElements.AddRange(task.Result);

            return sosiElements;
        }
    }
}
