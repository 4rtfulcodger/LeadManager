using LeadManager.API.Models;
using LeadManager.Core.Entities;
using LeadManager.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Text.Json;

namespace LeadManager.API.Controllers
{
    [Route("api/suppliers/{supplierId}/leads")]
    [ApiController]
    public class LeadsController : ControllerBase
    {
        private readonly ILogger<FilesController> _logger;
        private readonly IEmailService _emailService;
        public ILeadInfoRepository _leadInfoRepository;

        public LeadsController(ILeadInfoRepository leadInfoRepository, ILogger<FilesController> logger, IEmailService emailService)
        {
            _leadInfoRepository = leadInfoRepository ?? throw new ArgumentException(nameof(leadInfoRepository)); ;
            _logger = logger ?? throw new ArgumentException(nameof(logger));
            _emailService = emailService ?? throw new ArgumentException(nameof(emailService));
        }
               

        [HttpGet()]
        public async Task<ActionResult<IEnumerable<LeadDto>>> GetLeadsForSupplier(int supplierId)
        {
            //Validation and filter logic should be removed from controllers
            //Temporarily adding these for testing

            var leads = await _leadInfoRepository.GetLeadsAsync();           

            if (leads.Where(l => l.SupplierId == supplierId).Count() == 0)
                return NotFound();

            return Ok(leads.Where(l => l.SupplierId == supplierId));
        }

        [HttpGet("{id}", Name = "GetLead")]
        public async Task<ActionResult<LeadDto>> GetLead(int id,int supplierId)
        {
            //Validation and filter logic should be removed from controllers
            //Temporarily adding these for testing

            _logger.Log( LogLevel.Debug, "GET Request to LeadsController, GetLead action");
            
            var supplier = await _leadInfoRepository.GetSupplierWithIdAsync(supplierId);

            if (supplier == null)
                return NotFound();

            var leadToReturn = await _leadInfoRepository.GetLeadWithIdAsync(id, true, true);

            if(leadToReturn==null)
                return NotFound();

            if (leadToReturn?.SupplierId != supplierId)
                return BadRequest();

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

            _emailService.Send($"New lead (ID:{newLead.Id})", $"A new lead has been created: {JsonSerializer.Serialize(newLead)}");

            return CreatedAtRoute("GetLead", new { id = newLead.Id, supplierId = supplierId }, newLead);

        }
    }
}
