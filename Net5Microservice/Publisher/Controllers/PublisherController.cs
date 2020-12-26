using Microsoft.AspNetCore.Mvc;
using Publisher.Utils;

namespace Publisher.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PublisherController : ControllerBase
    {
        private readonly IEventBus _eventBus;

        public PublisherController(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        [HttpGet]
        public string Get()
        {
            return "Hello from " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Name + " Api. Running on " + System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        }

        [HttpPost]
        public void Post(string queueName, string message)
        {
            _eventBus.SendToQueue(queueName, message);
        }
    }
}
