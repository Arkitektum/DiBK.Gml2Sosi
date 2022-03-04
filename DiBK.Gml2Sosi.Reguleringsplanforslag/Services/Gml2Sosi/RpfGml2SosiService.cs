using DiBK.Gml2Sosi.Application.Mappers.Interfaces;
using DiBK.Gml2Sosi.Application.Models;
using DiBK.Gml2Sosi.Application.Models.Config;
using DiBK.Gml2Sosi.Application.Models.SosiObjects;
using DiBK.Gml2Sosi.Application.Services.Gml2Sosi;
using DiBK.Gml2Sosi.Reguleringsplanforslag.Constants;
using DiBK.Gml2Sosi.Reguleringsplanforslag.Mappers.Interfaces;
using DiBK.Gml2Sosi.Reguleringsplanforslag.Models.SosiObjects;
using Microsoft.AspNetCore.Http;

namespace DiBK.Gml2Sosi.Reguleringsplanforslag.Services.Gml2Sosi
{
    public class RpfGml2SosiService : IGml2SosiService
    {
        private readonly IHodeMapper _hodeMapper;
        private readonly ISosiObjectMapper _sosiObjectMapper;
        private readonly IRpHandlingOmrådeMapper _rpHandlingOmrådeMapper;
        private readonly DatasetSettings _settings;

        public RpfGml2SosiService(
            IHodeMapper hodeMapper,
            ISosiObjectMapper sosiObjectMapper,
            IRpHandlingOmrådeMapper rpHandlingOmrådeMapper,
            Datasets datasets)
        {
            _hodeMapper = hodeMapper;
            _sosiObjectMapper = sosiObjectMapper;
            _rpHandlingOmrådeMapper = rpHandlingOmrådeMapper;
            _settings = datasets.GetSettings(Dataset.Reguleringsplanforslag);
        }

        public async Task<MemoryStream> Gml2Sosi(IFormFile gmlFile)
        {
            var document = await GmlDocument.CreateAsync(gmlFile);
            var sosiElements = new List<SosiElement>();
            var resolution = _settings.Resolution;
            var sequenceNumber = 1;

            _hodeMapper.Map(document, _settings, sosiElements);
            _sosiObjectMapper.MapSosiSurfaceAndCurveObjects<RpOmråde, RpGrense>(document, _settings.Resolution, ref sequenceNumber, sosiElements);
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
            _sosiObjectMapper.MapSosiObjects<RpPåskrift>(document, ref sequenceNumber, sosiElements);
            _rpHandlingOmrådeMapper.Map(document, ref sequenceNumber, sosiElements);

            return SosiElement.WriteAllToStream(sosiElements);
        }
    }
}
