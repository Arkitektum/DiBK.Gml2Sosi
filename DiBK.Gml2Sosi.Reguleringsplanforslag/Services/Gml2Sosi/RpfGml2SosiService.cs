using DiBK.Gml2Sosi.Application.Mappers.Interfaces;
using DiBK.Gml2Sosi.Application.Models;
using DiBK.Gml2Sosi.Application.Models.Config;
using DiBK.Gml2Sosi.Application.Models.SosiObjects;
using DiBK.Gml2Sosi.Application.Services.Gml2Sosi;
using DiBK.Gml2Sosi.Reguleringsplanforslag.Constants;
using DiBK.Gml2Sosi.Reguleringsplanforslag.Mappers.Interfaces;
using DiBK.Gml2Sosi.Reguleringsplanforslag.Models.SosiObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace DiBK.Gml2Sosi.Reguleringsplanforslag.Services.Gml2Sosi
{
    public class RpfGml2SosiService : IGml2SosiService
    {
        private readonly IHodeMapper _hodeMapper;
        private readonly ISosiObjectMapper _sosiObjectMapper;
        private readonly IRpHandlingOmrådeMapper _rpHandlingOmrådeMapper;
        private readonly DatasetSettings _settings;
        private readonly ILogger<RpfGml2SosiService> _logger;

        public RpfGml2SosiService(
            IHodeMapper hodeMapper,
            ISosiObjectMapper sosiObjectMapper,
            IRpHandlingOmrådeMapper rpHandlingOmrådeMapper,
            Datasets datasets,
            ILogger<RpfGml2SosiService> logger)
        {
            _hodeMapper = hodeMapper;
            _sosiObjectMapper = sosiObjectMapper;
            _rpHandlingOmrådeMapper = rpHandlingOmrådeMapper;
            _settings = datasets.GetSettings(Dataset.Reguleringsplanforslag);
            _logger = logger;
        }

        public async Task<MemoryStream> Gml2Sosi(IFormFile gmlFile)
        {
            var start = DateTime.Now;
            var document = await GmlDocument.CreateAsync(gmlFile);
            var sosiElements = new List<SosiElement>();
            var resolution = _settings.Resolution;
            var sequenceNumber = 1;

            _hodeMapper.Map(document, _settings, sosiElements);
            
            var tasks = new List<Task>
            {
                Task.Run(() => _sosiObjectMapper.MapSosiSurfaceAndCurveObjects<RpOmråde, RpGrense>(document, _settings.Resolution, sosiElements)),
                Task.Run(() => _sosiObjectMapper.MapSosiSurfaceAndCurveObjects<RpArealformålOmråde, RpFormålGrense>(document, _settings.Resolution, sosiElements)),
                Task.Run(() => _sosiObjectMapper.MapSosiSurfaceAndCurveObjects<RpBestemmelseOmråde, RpBestemmelseGrense>(document, _settings.Resolution, sosiElements)),
                Task.Run(() => _sosiObjectMapper.MapSosiSurfaceAndCurveObjects<RpAngittHensynSone, RpAngittHensynGrense>(document, _settings.Resolution, sosiElements)),
                Task.Run(() => _sosiObjectMapper.MapSosiSurfaceAndCurveObjects<RpBåndleggingSone, RpBåndleggingGrense>(document, _settings.Resolution, sosiElements)),
                Task.Run(() => _sosiObjectMapper.MapSosiSurfaceAndCurveObjects<RpDetaljeringSone, RpDetaljeringGrense>(document, _settings.Resolution, sosiElements)),
                Task.Run(() => _sosiObjectMapper.MapSosiSurfaceAndCurveObjects<RpFareSone, RpFareGrense>(document, _settings.Resolution, sosiElements)),
                Task.Run(() => _sosiObjectMapper.MapSosiSurfaceAndCurveObjects<RpGjennomføringSone, RpGjennomføringGrense>(document, _settings.Resolution, sosiElements)),
                Task.Run(() => _sosiObjectMapper.MapSosiSurfaceAndCurveObjects<RpInfrastrukturSone, RpInfrastrukturGrense>(document, _settings.Resolution, sosiElements)),
                Task.Run(() => _sosiObjectMapper.MapSosiSurfaceAndCurveObjects<RpSikringSone, RpSikringGrense>(document, _settings.Resolution, sosiElements)),
                Task.Run(() => _sosiObjectMapper.MapSosiSurfaceAndCurveObjects<RpStøySone, RpStøyGrense>(document, _settings.Resolution, sosiElements)),
                Task.Run(() => _sosiObjectMapper.MapSosiSurfaceAndCurveObjects<PblMidlByggAnleggOmråde, PblMidlByggAnleggGrense>(document, _settings.Resolution, sosiElements)),
                Task.Run(() => _sosiObjectMapper.MapSosiCurveObjects<RpRegulertHøyde>(document, false, sosiElements)),
                Task.Run(() => _sosiObjectMapper.MapSosiCurveObjects<RpJuridiskLinje>(document, true, sosiElements)),
                Task.Run(() => _sosiObjectMapper.MapSosiObjects<RpJuridiskPunkt>(document, sosiElements)),
                Task.Run(() => _sosiObjectMapper.MapSosiObjects<RpPåskrift>(document, sosiElements)),
                Task.Run(() => _rpHandlingOmrådeMapper.Map(document, sosiElements))
            };

            await Task.WhenAll(tasks);

            /*_sosiObjectMapper.MapSosiSurfaceAndCurveObjects<RpOmråde, RpGrense>(document, _settings.Resolution, ref sequenceNumber, sosiElements);
            _sosiObjectMapper.MapSosiSurfaceAndCurveObjects<RpArealformålOmråde, RpFormålGrense>(document, resolution, ref sequenceNumber, sosiElements);
            _sosiObjectMapper.MapSosiSurfaceAndCurveObjects<RpBestemmelseOmråde, RpBestemmelseGrense>(document, resolution, ref sequenceNumber, sosiElements);
            _sosiObjectMapper.MapSosiSurfaceAndCurveObjects<RpAngittHensynSone, RpAngittHensynGrense>(document, resolution, ref sequenceNumber, sosiElements);
            _sosiObjectMapper.MapSosiSurfaceAndCurveObjects<RpBåndleggingSone, RpBåndleggingGrense>(document, resolution, ref sequenceNumber, sosiElements);
            _sosiObjectMapper.MapSosiSurfaceAndCurveObjects<RpDetaljeringSone, RpDetaljeringGrense>(document, resolution, ref sequenceNumber, sosiElements);
            _sosiObjectMapper.MapSosiSurfaceAndCurveObjects<RpFareSone, RpFareGrense>(document, resolution, ref sequenceNumber, sosiElements);
            _sosiObjectMapper.MapSosiSurfaceAndCurveObjects<RpGjennomføringSone, RpGjennomføringGrense>(document, resolution, ref sequenceNumber, sosiElements);
            _sosiObjectMapper.MapSosiSurfaceAndCurveObjects<RpInfrastrukturSone, RpInfrastrukturGrense>(document, resolution, ref sequenceNumber, sosiElements);
            _sosiObjectMapper.MapSosiSurfaceAndCurveObjects<RpSikringSone, RpSikringGrense>(document, resolution, ref sequenceNumber, sosiElements);
            _sosiObjectMapper.MapSosiSurfaceAndCurveObjects<RpStøySone, RpStøyGrense>(document, resolution, ref sequenceNumber, sosiElements);
            _sosiObjectMapper.MapSosiSurfaceAndCurveObjects<PblMidlByggAnleggOmråde, PblMidlByggAnleggGrense>(document, resolution, ref sequenceNumber, sosiElements);
            _sosiObjectMapper.MapSosiCurveObjects<RpRegulertHøyde>(document, false, ref sequenceNumber, sosiElements);
            _sosiObjectMapper.MapSosiCurveObjects<RpJuridiskLinje>(document, true, ref sequenceNumber, sosiElements);
            _sosiObjectMapper.MapSosiObjects<RpJuridiskPunkt>(document, ref sequenceNumber, sosiElements);
            _sosiObjectMapper.MapSosiObjects<RpPåskrift>(document, ref sequenceNumber, sosiElements);*/

            var stream = await SosiElement.WriteAllToStreamAsync(sosiElements);
            var timeUsed = Math.Round(DateTime.Now.Subtract(start).TotalSeconds, 5);

            _logger.LogInformation("Genererte {elementCount} elementer på {timeUsed} sek.", sosiElements.Count, timeUsed);

            return stream;
        }
    }
}
