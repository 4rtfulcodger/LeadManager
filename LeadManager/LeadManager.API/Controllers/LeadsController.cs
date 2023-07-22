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
using LeadManager.Core.Interfaces.Source;
using LeadManager.Core.Interfaces.Supplier;
using LeadManager.Infrastructure.Services;
using System.Collections.Generic;
using LeadManager.Core.Entities.Source;
using LeadManager.Core.Entities.Supplier;
using LeadManager.Core.Entities;

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
        private readonly IApiEndpointHandler _apiEndpointHandler;

        public LeadsController(ILogger<FilesController> logger,
            IEmailService emailService,
            ISourceService sourceService,
            ILeadService leadService,
            ISupplierService supplierService,
            IMapper mapper,
            IApiEndpointHandler apiEndpointHandler)
        {
            _logger = logger ?? throw new ArgumentException(nameof(logger));
            _emailService = emailService ?? throw new ArgumentException(nameof(emailService));
            _sourceService = sourceService ?? throw new ArgumentException(nameof(sourceService));
            _supplierService = supplierService ?? throw new ArgumentException(nameof(supplierService));
            _leadService = leadService ?? throw new ArgumentException(nameof(leadService));
            _mapper = mapper;
            _apiEndpointHandler = apiEndpointHandler;
        }


        [HttpGet()]
        public async Task<IActionResult> GetLeads(LeadFilter leadFilter)
        {            
            var leads = await _leadService.GetLeadsAsync(leadFilter);
            return _apiEndpointHandler.ReturnSearchResult<IEnumerable<Lead>,IEnumerable<LeadDto>>(leads);
        }

        [HttpGet("{id}", Name = "GetLead")]
        public async Task<IActionResult> GetLead(int id)
        {
            _logger.Log(LogLevel.Debug, "GET Request to LeadsController, GetLead action");
                        
            var leadToReturn = await _leadService.GetLeadWithIdAsync(id, true, true);
            return _apiEndpointHandler.ReturnSearchResult<Lead,LeadDto>(leadToReturn);
        }

        [HttpPost]
        public async Task<IActionResult> CreateLead(LeadForCreateDto leadDto)
        {
            var supplier = await _supplierService.GetSupplierWithIdAsync(leadDto.SupplierId);
            if(!_apiEndpointHandler.IsValidEntitySearchResult<Supplier>(supplier))
                return BadRequest();

            var source = await _sourceService.GetSourceWithIdAsync(leadDto.SourceId);
            if (!_apiEndpointHandler.IsValidEntitySearchResult<Source>(source))
                return BadRequest();

            var newLead = _mapper.Map<Lead>(leadDto);

            return _apiEndpointHandler.ReturnCreateResult<LeadForCreateDto>(await _leadService.AddLeadAsync(newLead),
                "GetLead",
                newLead.LeadId.ToString(),
                newLead);
        }

        [HttpPatch("{leadId}")]
        public async Task<IActionResult> UpdateLead(JsonPatchDocument<LeadForUpdateDto> patchDocument, int leadId)
        {
            var leadEntity = await _leadService.GetLeadWithIdAsync(leadId, false, false);
            if (!_apiEndpointHandler.IsValidEntitySearchResult<Lead>(leadEntity))
                return BadRequest();

            var leadDto = _mapper.Map<LeadForUpdateDto>(leadEntity);
            patchDocument.ApplyTo(leadDto);

            var supplier = await _supplierService.GetSupplierWithIdAsync(leadDto.SupplierId);
            if (!_apiEndpointHandler.IsValidEntitySearchResult<Supplier>(supplier))
                return BadRequest();

            var source = await _sourceService.GetSourceWithIdAsync(leadDto.SourceId);
            if (!_apiEndpointHandler.IsValidEntitySearchResult<Source>(source))
                return BadRequest();

            _mapper.Map(leadDto, leadEntity);          
            return _apiEndpointHandler.ReturndUpdateResult(await _leadService.UpdateLeadAsync(leadId));
        }

        [HttpDelete("{id}", Name = "DeleteLead")]
        public async Task<IActionResult> DeleteLead(int id)
        {
            _logger.Log(LogLevel.Debug, "Request to LeadsController, DeleteLead action");                       

            var leadToDelete = await _leadService.GetLeadWithIdAsync(id);
            if (!_apiEndpointHandler.IsValidEntitySearchResult<Lead>(leadToDelete))
                return BadRequest();

            return _apiEndpointHandler.ReturnDeleteResult(await _leadService.DeleteLead(leadToDelete));
        }
    }
}
