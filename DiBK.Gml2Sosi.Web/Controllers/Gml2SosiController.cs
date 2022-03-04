using DiBK.Gml2Sosi.Application.Services.Gml2Sosi;
using DiBK.Gml2Sosi.Application.Services.MultipartRequest;
using Microsoft.AspNetCore.Mvc;

namespace DiBK.Gml2Sosi.Web.Controllers
{
    [ApiController]
    [Route("/")]
    public class Gml2SosiController : BaseController
    {
        private readonly IMultipartRequestService _multipartRequestService;
        private readonly IGml2SosiService _rpfGml2SosiService;

        public Gml2SosiController(
            IMultipartRequestService multipartRequestService,
            IGml2SosiService rpfGml2SosiService,
            ILogger<Gml2SosiController> logger) : base(logger)
        {
            _multipartRequestService = multipartRequestService;
            _rpfGml2SosiService = rpfGml2SosiService;
        }

        [HttpPost("reguleringsplanforslag")]
        public async Task<IActionResult> ConvertReguleringsplanforslag()
        {
            try
            {
                var gmlFile = await _multipartRequestService.GetFileFromMultipart();

                if (gmlFile == null)
                    return BadRequest();

                var document = await _rpfGml2SosiService.Gml2Sosi(gmlFile);

                return File(document, "text/vnd.sosi", $"{Path.ChangeExtension(gmlFile.FileName, "sos")}");
            }
            catch (Exception exception)
            {
                var result = HandleException(exception);

                if (result != null)
                    return result;

                throw;
            }
        }
    }
}