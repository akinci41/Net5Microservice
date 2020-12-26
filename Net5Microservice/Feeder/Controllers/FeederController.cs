using Feeder.Entity;
using Feeder.Utils;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Text.Json;

namespace Feeder.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FeederController : ControllerBase
    {
        private readonly FeederContext _context;

        public FeederController(FeederContext context)
        {
            _context = context;
        }

        [HttpGet]
        public string Get()
        {
            return "Hello from " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Name + " Api. Running on " + System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        }

        [HttpPost, Route("AddContact")]
        public ActionResult AddContact(string message)
        {
            var contact = JsonSerializer.Deserialize<Contact>(message);
            _context.Contacts.Add(contact);
            _context.SaveChanges();
            return Ok();
        }

        [HttpPost, Route("RemoveContact")]
        public ActionResult RemoveContact(string message)
        {
            var contact = JsonSerializer.Deserialize<Contact>(message);
            var _contact = _context.Contacts.Where(x => x.ID == contact.ID).FirstOrDefault();
            _context.Contacts.Remove(_contact);
            _context.SaveChanges();
            return Ok();
        }

        [HttpPost, Route("AddCommunication")]
        public ActionResult AddCommunication(string message)
        {
            var communication = JsonSerializer.Deserialize<Communication>(message);
            _context.Communications.Add(communication);
            _context.SaveChanges();
            return Ok();
        }

        [HttpPost, Route("RemoveCommunication")]
        public ActionResult RemoveCommunication(string message)
        {
            var communication = JsonSerializer.Deserialize<Communication>(message);
            var _communication = _context.Communications.Where(x => x.ID == communication.ID).FirstOrDefault();
            _context.Communications.Remove(_communication);
            _context.SaveChanges();
            return Ok();
        }

        [HttpPost, Route("GetReport")]
        public ActionResult GetReport(string message)
        {
            var report = new Report();
            _context.Reports.Add(report);
            _context.SaveChanges();

            var reporter = new Reporter();
            reporter.GenerateReportAsync(report.ID);

            return Ok("New report request received");
        }

        [HttpPost, Route("GetEnv")]
        public string GetEnv()
        {
            return "RabbitMQ_Url : " + Startup.StaticConfig.GetSection("Parameters")["RabbitMQ_Url"] + " ; CS : " + Startup.StaticConfig.GetSection("ConnectionStrings")["DefaultConnection"];
        }
    }
}