using LeadManager.API.Models;
using LeadManager.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace LeadManager.API.Controllers
{
    [Route("api/suppliers/{supplierId}/leads")]
    [ApiController]
    public class LeadsController : ControllerBase
    {
        private readonly ILogger<FilesController> _logger;
        private readonly IEmailService _emailService;
        public ILeadDataRepository _leadDataRepository { get; }

        public LeadsController(ILeadDataRepository leadDataRepository, ILogger<FilesController> logger, IEmailService emailService)
        {
            _leadDataRepository = leadDataRepository ?? throw new ArgumentException(nameof(leadDataRepository)); ;
            _logger = logger ?? throw new ArgumentException(nameof(logger));
            _emailService = emailService ?? throw new ArgumentException(nameof(emailService));
        }

        [HttpGet()]
        public ActionResult<IEnumerable<LeadDto>> GetLeads(int supplierId)
        {
            var supplier = TestDataStore.Current.Suppliers.FirstOrDefault(x => x.Id == supplierId);

            if (supplier == null)
                return NotFound();

            return Ok(_leadDataRepository.GetLeads());
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

            _emailService.Send($"New lead (ID:{newLead.Id})", "audit@testlm.com", $"A new lead has been created: {JsonSerializer.Serialize(newLead)}");

            return CreatedAtRoute("GetLead", new { id = newLead.Id, supplierId = supplierId }, newLead);

        }
    }
}
