using Microsoft.AspNetCore.Mvc;
using Publisher.Utils;
using Publisher.Entity;

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
            return "Hello from " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Name + " Api";
        }

        [HttpPost]
        public void Post([FromBody] QueueMessage message)
        {
            _eventBus.SendToQueue(message);
        }
    }
}
