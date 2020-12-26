using Microsoft.AspNetCore.Mvc;
using Report.Entity;
using System.Collections.Generic;
using System.Linq;

namespace Report.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReportController : ControllerBase
    {
        private readonly ReportContext _context;

        public ReportController(ReportContext context)
        {
            _context = context;
        }

        [HttpGet]
        public string Get()
        {
            return "Hello from " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Name + " Api. Running on " + System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        }

        [HttpGet("GetReportList")]
        public List<Entity.Report> GetReportList()
        {
            var list = _context.Reports.ToList();
            return list;
        }

        [HttpGet("GetReport")]
        public List<ReportDetail> GetReport(string ID)
        {
            var list = _context.ReportDetails.Where(x => x.ReportID.ToString() == ID).ToList();
            return list;
        }
    }
}
