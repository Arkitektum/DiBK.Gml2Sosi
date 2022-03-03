using DiBK.Gml2Sosi.Application.Mappers.Interfaces;
using DiBK.Gml2Sosi.Application.Models;
using DiBK.Gml2Sosi.Application.Models.Config;
using DiBK.Gml2Sosi.Application.Models.SosiObjects;
using DiBK.Gml2Sosi.Application.Services.Gml2Sosi;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace DiBK.Gml2Sosi.Reguleringsplanforslag.Services.Gml2Sosi
{
    public class RpfGml2SosiService : Gml2SosiServiceBase, IGml2SosiService
    {
        private readonly DatasetSettings _settings;

        public RpfGml2SosiService(
            IHodeMapper hodeMapper,
            IOptions<DatasetConfiguration> options) : base(hodeMapper)
        {
            _settings = options.Value.Reguleringsplanforslag;
        }

        public async Task<MemoryStream> Gml2Sosi(IFormFile gmlFile)
        {
            var document = await GmlDocument.CreateAsync(gmlFile);
            var sosiElements = new List<SosiElement>();
            var sequenceNumber = 1;

            MapHode(document.Document, _settings, sosiElements);

            return WriteToStream(sosiElements);
        }
    }
}
