using Microsoft.AspNetCore.Mvc;

namespace DoctorsNearMe.Controllers
{
    [ApiController]
    [Route("/")]
    public class RootController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<RootController> _logger;

        public RootController(ILogger<RootController> logger)
        {
            _logger = logger;
        }

        [HttpGet("all")]
        public IActionResult Get()
        {
            _logger.Log(LogLevel.Critical, "Look at this!");
            return Ok("d");
        }
    }
}
