using DiBK.Gml2Sosi.Application.Mappers.Interfaces;
using DiBK.Gml2Sosi.Application.Models;
using DiBK.Gml2Sosi.Application.Models.Config;
using DiBK.Gml2Sosi.Application.Services.Gml2Sosi;
using DiBK.Gml2Sosi.Reguleringsplanforslag.Constants;
using DiBK.Gml2Sosi.Reguleringsplanforslag.Mappers.Interfaces;
using DiBK.Gml2Sosi.Reguleringsplanforslag.Models.SosiObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace DiBK.Gml2Sosi.Reguleringsplanforslag.Services.Gml2Sosi
{
    public class RpfGml2SosiService : Gml2SosiServiceBase, IGml2SosiService
    {
        private readonly ISosiObjectMapper _sosiObjectMapper;
        private readonly IRpHandlingOmrådeMapper _rpHandlingOmrådeMapper;
        private readonly DatasetSettings _settings;

        public RpfGml2SosiService(
            IHodeMapper hodeMapper,
            ISosiObjectMapper sosiObjectMapper,
            IRpHandlingOmrådeMapper rpHandlingOmrådeMapper,
            Datasets datasets,
            ILogger<RpfGml2SosiService> logger) : base(hodeMapper, logger)
        {
            _sosiObjectMapper = sosiObjectMapper;
            _rpHandlingOmrådeMapper = rpHandlingOmrådeMapper;
            _settings = datasets.GetSettings(Dataset.Reguleringsplanforslag);
        }

        public async Task<MemoryStream> Gml2SosiAsync(IFormFile gmlFile)
        {
            var document = await GmlDocument.CreateAsync(gmlFile);
            var resolution = _settings.Resolution;

            return await CreateSosiDocumentAsync(document, _settings, new []
            {
                () => _sosiObjectMapper.MapSosiSurfaceAndCurveObjects<RpOmråde, RpGrense>(document, resolution),
                () => _sosiObjectMapper.MapSosiSurfaceAndCurveObjects<RpArealformålOmråde, RpFormålGrense>(document, resolution),
                () => _sosiObjectMapper.MapSosiSurfaceAndCurveObjects<RpBestemmelseOmråde, RpBestemmelseGrense>(document, resolution),
                () => _sosiObjectMapper.MapSosiSurfaceAndCurveObjects<RpAngittHensynSone, RpAngittHensynGrense>(document, resolution),
                () => _sosiObjectMapper.MapSosiSurfaceAndCurveObjects<RpBåndleggingSone, RpBåndleggingGrense>(document, resolution),
                () => _sosiObjectMapper.MapSosiSurfaceAndCurveObjects<RpDetaljeringSone, RpDetaljeringGrense>(document, resolution),
                () => _sosiObjectMapper.MapSosiSurfaceAndCurveObjects<RpFareSone, RpFareGrense>(document, resolution),
                () => _sosiObjectMapper.MapSosiSurfaceAndCurveObjects<RpGjennomføringSone, RpGjennomføringGrense>(document, resolution),
                () => _sosiObjectMapper.MapSosiSurfaceAndCurveObjects<RpInfrastrukturSone, RpInfrastrukturGrense>(document, resolution),
                () => _sosiObjectMapper.MapSosiSurfaceAndCurveObjects<RpSikringSone, RpSikringGrense>(document, resolution),
                () => _sosiObjectMapper.MapSosiSurfaceAndCurveObjects<RpStøySone, RpStøyGrense>(document, resolution),
                () => _sosiObjectMapper.MapSosiSurfaceAndCurveObjects<PblMidlByggAnleggOmråde, PblMidlByggAnleggGrense>(document, resolution),
                () => _sosiObjectMapper.MapSosiCurveObjects<RpRegulertHøyde>(document, false),
                () => _sosiObjectMapper.MapSosiCurveObjects<RpJuridiskLinje>(document, true),
                () => _sosiObjectMapper.MapSosiObjects<RpJuridiskPunkt>(document),
                () => _sosiObjectMapper.MapSosiObjects<RpPåskrift>(document),
                () => _rpHandlingOmrådeMapper.Map(document)
            });
        }
    }
}
