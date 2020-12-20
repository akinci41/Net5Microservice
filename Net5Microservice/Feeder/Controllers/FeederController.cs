using Microsoft.AspNetCore.Mvc;
using Feeder.Entity;
using System.Collections.Generic;
using System.Linq;

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
            return "Hello from " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Name + " Api";
        }

        [HttpPost, Route("AddContact")]
        public ActionResult AddContact(Contact contact)
        {
            _context.Contacts.Add(contact);
            _context.SaveChanges();
            return Ok();
        }

        [HttpGet("RemoveContact")]
        public ActionResult RemoveContact(string ID)
        {
            var contact = _context.Contacts.Where(x => x.ID.ToString() == ID).FirstOrDefault();
            _context.Contacts.Remove(contact);
            _context.SaveChanges();
            return Ok();
        }

        [HttpPost, Route("AddCommunication")]
        public ActionResult AddCommunication(Communication communication)
        {
            _context.Communications.Add(communication);
            _context.SaveChanges();
            return Ok();
        }

        [HttpGet("RemoveCommunication")]
        public ActionResult RemoveCommunication(string ID)
        {
            var communication = _context.Communications.Where(x => x.ID.ToString() == ID).FirstOrDefault();
            _context.Communications.Remove(communication);
            _context.SaveChanges();
            return Ok();
        }
    }
}