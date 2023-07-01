using AutoMapper;
using LeadManager.Core.Interfaces;
using LeadManager.Core.Interfaces.Lead;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using LeadManager.Core.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using LeadManager.Core.ViewModels;
using LeadManager.Core.Entities.Lead;
using LeadManager.Core.Interfaces.Source;
using LeadManager.Core.Interfaces.Supplier;
using LeadManager.Infrastructure.Services;

namespace LeadManager.API.Controllers
{
    [ApiController]
    [Route("api/leads")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class LeadsController : ControllerBase
    {
        private readonly ILogger<FilesController> _logger;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;        
        public ILeadService _leadService;
        public ISourceService _sourceService;
        public ISupplierService _supplierService;

        public LeadsController(ILogger<FilesController> logger,
            IEmailService emailService,
            ISourceService sourceService,
            ILeadService leadService,
            ISupplierService supplierService,
            IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentException(nameof(logger));
            _emailService = emailService ?? throw new ArgumentException(nameof(emailService));
            _sourceService = sourceService ?? throw new ArgumentException(nameof(sourceService));
            _supplierService = supplierService ?? throw new ArgumentException(nameof(supplierService));
            _leadService = leadService ?? throw new ArgumentException(nameof(leadService));
            _mapper = mapper;
        }


        [HttpGet()]
        public async Task<IActionResult> GetLeads(LeadFilter leadFilter)
        {
            //Validation and filter logic should be removed from controllers
            //Temporarily adding these for testing

            var leads = await _leadService.GetLeadsAsync(leadFilter);

            if (leads.Count() == 0)
                return NotFound();

            return Ok(_mapper.Map<IEnumerable<LeadDto>>(leads));
        }

        [HttpGet("{id}", Name = "GetLead")]
        public async Task<IActionResult> GetLead(int id)
        {
            //Validation and filter logic should be removed from controllers
            //Temporarily adding these for testing

            _logger.Log(LogLevel.Debug, "GET Request to LeadsController, GetLead action");
                        
            var leadToReturn = await _leadService.GetLeadWithIdAsync(id, true, true);

            if (leadToReturn == null)
                return NotFound();

            return Ok(_mapper.Map<LeadDto>(leadToReturn));
        }

        [HttpPost]
        public async Task<IActionResult> CreateLead(LeadForCreateDto leadDto)
        {
            var supplier = await _supplierService.GetSupplierWithIdAsync(leadDto.SupplierId);

            if (supplier == null)
                return NotFound();

            var source = await _sourceService.GetSourceWithIdAsync(leadDto.SourceId);

            if (source == null)
                return NotFound();

            var newLead = _mapper.Map<Lead>(leadDto);

            bool addLeadSuccess = await _leadService.AddLeadAsync(newLead);

            if (addLeadSuccess)
            {
                _emailService.Send($"New lead (ID:{newLead.LeadId})", $"A new lead has been created: {JsonSerializer.Serialize(newLead)}");
            }
            else
            {
                _emailService.Send($"There was an error when trying to add a lead", $"There was an error when trying to add the following lead: {JsonSerializer.Serialize(newLead)}");
            }


            return CreatedAtRoute("GetLead", new { id = newLead.LeadId}, newLead);

        }

        [HttpPatch("{leadId}")]
        public async Task<IActionResult> UpdateLead(JsonPatchDocument<LeadForUpdateDto> patchDocument, int leadId)
        {
            var leadEntity = await _leadService.GetLeadWithIdAsync(leadId, false, false);
            if (leadEntity == null)
                return NotFound();

            var leadDto = _mapper.Map<LeadForUpdateDto>(leadEntity);

            patchDocument.ApplyTo(leadDto);

            var supplier = await _supplierService.GetSupplierWithIdAsync(leadDto.SupplierId);

            if (supplier == null)
                return NotFound();

            var source = await _sourceService.GetSourceWithIdAsync(leadDto.SourceId);

            if (source == null)
                return NotFound();

            _mapper.Map(leadDto, leadEntity);
            bool updateresult =  await _leadService.UpdateLeadAsync(leadId);

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
        public async Task<IActionResult> DeleteLead(int id)
        {
            //Validation and filter logic should be removed from controllers
            //Temporarily adding these for testing

            _logger.Log(LogLevel.Debug, "Request to LeadsController, DeleteLead action");                       

            var leadToDelete = await _leadService.GetLeadWithIdAsync(id);

            if (leadToDelete == null)
                return NotFound();

            var deleteResult = await _leadService.DeleteLead(leadToDelete);

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
