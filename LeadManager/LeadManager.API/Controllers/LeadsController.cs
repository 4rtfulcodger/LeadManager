using LeadManager.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LeadManager.API.Controllers
{
    [Route("api/suppliers/{supplierId}/leads")]
    [ApiController]
    public class LeadsController : ControllerBase
    {
        private readonly ILogger<FilesController> _logger;

        public LeadsController(ILogger<FilesController> logger)
        {
            _logger = logger ?? throw new ArgumentException(nameof(logger));
        }

        [HttpGet()]
        public ActionResult<IEnumerable<LeadDto>> GetLeads(int supplierId)
        {
            var supplier = TestDataStore.Current.Suppliers.FirstOrDefault(x => x.Id == supplierId);

            if (supplier == null)
                return NotFound();

            return Ok(TestDataStore.Current.Leads);
        }

        [HttpGet("{id}", Name = "GetLead")]
        public ActionResult<LeadDto> GetLead(int id,int supplierId)
        {
            _logger.Log( LogLevel.Debug, "GET Request to LeadsController, GetLead action");

            var supplier = TestDataStore.Current.Suppliers.FirstOrDefault(x => x.Id == supplierId);

            if (supplier == null)
                return NotFound();

            var leadToReturn = TestDataStore.Current.Leads.FirstOrDefault(x => x.Id == id);

            if (leadToReturn == null)
                return NotFound();

            return Ok(leadToReturn);
        }

        [HttpPost]
        public ActionResult<LeadDto> CreateLead(LeadForCreateDto lead, int supplierId)
        {
            var supplier = TestDataStore.Current.Suppliers.FirstOrDefault(x => x.Id == supplierId);

            if (supplier == null)
                return NotFound();

            var newLead = new LeadDto
            {
                Id = TestDataStore.Current.Leads.Count() + 1,
                Name = lead.Name,
                Description = lead.Description
            };

            TestDataStore.Current.Leads.Add(newLead);

            return CreatedAtRoute("GetLead", new { id = newLead.Id, supplierId = supplierId }, newLead);

        }
    }
}
