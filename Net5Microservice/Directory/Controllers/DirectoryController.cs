using Directory.Entity;
using Directory.Integration;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace Directory.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DirectoryController : ControllerBase
    {
        private readonly DirectoryContext _context;

        public DirectoryController(DirectoryContext context)
        {
            _context = context;
        }

        [HttpGet]
        public string Get()
        {
            return "Hello from " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Name + " Api. Running on " + System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        }

        [HttpGet("GetContactList")]
        public List<Contact> GetContactList()
        {
            var list = _context.Contacts.ToList();
            return list;
        }

        [HttpGet("GetContact")]
        public ActionResult GetContact(string ID)
        {
            var _contact = _context.Contacts.Where(x => x.ID.ToString() == ID).FirstOrDefault();
            _contact.CommunicationList = _context.Communications.Where(x => x.ContactID == _contact.ID).ToList();
            return CreatedAtAction("Get", new { id = _contact.ID }, _contact);
        }

        [HttpPost, Route("AddContact")]
        public ActionResult AddContact(Contact contact)
        {
            var newContact = new Contact(contact.Name, contact.Surname, contact.FirmName);
            _context.Contacts.Add(newContact);
            _context.SaveChanges();
            PublisherCall.SendToQueue("AddContact", newContact);
            return CreatedAtAction("Get", new { id = newContact.ID }, newContact);
        }

        [HttpGet("RemoveContact")]
        public ActionResult RemoveContact(string ID)
        {
            var contact = _context.Contacts.Where(x => x.ID.ToString() == ID).FirstOrDefault();
            _context.Contacts.Remove(contact);
            _context.SaveChanges();
            PublisherCall.SendToQueue("RemoveContact", contact);
            return Ok();
        }

        [HttpPost, Route("AddCommunication")]
        public ActionResult AddCommunication(Communication communication)
        {
            var newCommunication = new Communication(communication.ContactID, communication.Type, communication.Content);
            _context.Communications.Add(newCommunication);
            _context.SaveChanges();
            PublisherCall.SendToQueue("AddCommunication", newCommunication);
            return CreatedAtAction("Get", new { id = newCommunication.ID }, newCommunication);
        }

        [HttpGet("RemoveCommunication")]
        public ActionResult RemoveCommunication(string ID)
        {
            var communication = _context.Communications.Where(x => x.ID.ToString() == ID).FirstOrDefault();
            _context.Communications.Remove(communication);
            _context.SaveChanges();
            PublisherCall.SendToQueue("RemoveCommunication", communication);
            return Ok();
        }

        [HttpGet("GetReport")]
        public ActionResult GetReport()
        {
            PublisherCall.SendToQueue("GetReport", "Create a new Report");
            return Ok("New report request received");
        }
    }
}