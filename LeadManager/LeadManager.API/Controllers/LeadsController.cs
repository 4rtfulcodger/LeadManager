using AutoMapper;
using LeadManager.API.Models;
using LeadManager.Core.Entities;
using LeadManager.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
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
        private readonly IMapper _mapper;
        public ILeadInfoRepository _leadInfoRepository;

        public LeadsController(ILeadInfoRepository leadInfoRepository,
            ILogger<FilesController> logger,
            IEmailService emailService,
            IMapper mapper)
        {
            _leadInfoRepository = leadInfoRepository ?? throw new ArgumentException(nameof(leadInfoRepository)); ;
            _logger = logger ?? throw new ArgumentException(nameof(logger));
            _emailService = emailService ?? throw new ArgumentException(nameof(emailService));
            _mapper = mapper;
        }


        [HttpGet()]
        public async Task<ActionResult<IEnumerable<LeadDto>>> GetLeadsForSupplier(int supplierId, bool includeSource, bool includeSupplier)
        {
            //Validation and filter logic should be removed from controllers
            //Temporarily adding these for testing

            var leads = await _leadInfoRepository.GetLeadsAsync(includeSource, includeSupplier);

            if (leads.Where(l => l.SupplierId == supplierId).Count() == 0)
                return NotFound();

            return Ok(_mapper.Map<IEnumerable<LeadDto>>(leads.Where(l => l.SupplierId == supplierId)));
        }

        [HttpGet("{id}", Name = "GetLead")]
        public async Task<ActionResult<LeadDto>> GetLead(int id, int supplierId)
        {
            //Validation and filter logic should be removed from controllers
            //Temporarily adding these for testing

            _logger.Log(LogLevel.Debug, "GET Request to LeadsController, GetLead action");

            var supplier = await _leadInfoRepository.GetSupplierWithIdAsync(supplierId);

            if (supplier == null)
                return NotFound();

            var leadToReturn = await _leadInfoRepository.GetLeadWithIdAsync(id, true, true);

            if (leadToReturn == null)
                return NotFound();

            if (leadToReturn?.SupplierId != supplierId)
                return BadRequest();

            return Ok(_mapper.Map<LeadDto>(leadToReturn));
        }

        [HttpPost]
        public async Task<ActionResult<LeadDto>> CreateLead(LeadForCreateDto leadDto, int supplierId)
        {
            var supplier = await _leadInfoRepository.GetSupplierWithIdAsync(supplierId);

            if (supplier == null)
                return NotFound();

            var source = await _leadInfoRepository.GetSourceWithIdAsync(leadDto.SourceId);

            if (source == null)
                return NotFound();

            var newLead = _mapper.Map<Lead>(leadDto);

            bool addLeadSuccess = await _leadInfoRepository.AddLeadAsync(newLead);

            if (addLeadSuccess)
            {
                _emailService.Send($"New lead (ID:{newLead.LeadId})", $"A new lead has been created: {JsonSerializer.Serialize(newLead)}");
            }
            else
            {
                _emailService.Send($"There was an error when trying to add a lead", $"There was an error when trying to add the following lead: {JsonSerializer.Serialize(newLead)}");
            }


            return CreatedAtRoute("GetLead", new { id = newLead.LeadId, supplierId = supplierId }, newLead);

        }

        [HttpPatch("{leadId}")]
        public async Task<ActionResult<LeadDto>> UpdateLead(JsonPatchDocument<LeadForUpdateDto> patchDocument, int supplierId, int leadId)
        {
            var supplier = await _leadInfoRepository.GetSupplierWithIdAsync(supplierId);

            if (supplier == null)
                return NotFound();
                              

            var leadEntity = await _leadInfoRepository.GetLeadWithIdAsync(leadId, false, false);
            if (leadEntity == null)
                return NotFound();

            var updatedLead = _mapper.Map<LeadForUpdateDto>(leadEntity);

            patchDocument.ApplyTo(updatedLead);

            var source = await _leadInfoRepository.GetSourceWithIdAsync(updatedLead.SourceId);

            if (source == null)
                return NotFound();

            _mapper.Map(updatedLead, leadEntity);
            bool updateresult =  await _leadInfoRepository.UpdateLeadAsync(leadId);

            if (updateresult)
            {
                return NoContent();
            }
            else
            {
                return Problem();
            }

        }

        [HttpDelete("{id}", Name = "DeleteLead")]
        public async Task<ActionResult<LeadDto>> DeleteLead(int id, int supplierId)
        {
            //Validation and filter logic should be removed from controllers
            //Temporarily adding these for testing

            _logger.Log(LogLevel.Debug, "Request to LeadsController, DeleteLead action");

            var supplier = await _leadInfoRepository.GetSupplierWithIdAsync(supplierId);

            if (supplier == null)
                return NotFound();

            var leadToDelete = await _leadInfoRepository.GetLeadWithIdAsync(id, true, true);

            if (leadToDelete == null)
                return NotFound();

            var deleteResult = await _leadInfoRepository.DeleteLead(leadToDelete);

            if (deleteResult)
            {
                return NoContent();
            }
            else
            {
                return Problem();
            }
        }
    }
}
