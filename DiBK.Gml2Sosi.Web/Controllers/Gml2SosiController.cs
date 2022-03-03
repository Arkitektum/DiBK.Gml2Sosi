using Microsoft.AspNetCore.Mvc;

namespace DiBK.Gml2Sosi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Gml2SosiController : ControllerBase
    {
        private readonly ILogger<Gml2SosiController> _logger;

        public Gml2SosiController(ILogger<Gml2SosiController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public IActionResult Convert()
        {
            return Ok();
        }
    }
}