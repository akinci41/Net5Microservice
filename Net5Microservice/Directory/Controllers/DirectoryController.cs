using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Directory.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DirectoryController : ControllerBase
    {
        private readonly ILogger<DirectoryController> _logger;

        public DirectoryController(ILogger<DirectoryController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public string Get()
        {
            return "Hello from " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Name + " Api";
        }
    }
}