﻿using AutoMapper;
using LeadManager.Core.Interfaces;
using LeadManager.Core.Interfaces.Lead;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using LeadManager.Core.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using LeadManager.Core.ViewModels;
using LeadManager.Core.Interfaces.Source;
using LeadManager.Core.Interfaces.Supplier;
using LeadManager.Core.Entities.Source;
using LeadManager.Core.Entities.Supplier;
using LeadManager.Core.Entities;
using LeadManager.API.BusinessLogic.Common;

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
        private readonly IApiResponseHandler _apiResponseHandler;

        public LeadsController(ILogger<FilesController> logger,
            IEmailService emailService,
            ISourceService sourceService,
            ILeadService leadService,
            ISupplierService supplierService,
            IMapper mapper,
            IApiResponseHandler apiEndpointHandler)
        {
            _logger = logger ?? throw new ArgumentException(nameof(logger));
            _emailService = emailService ?? throw new ArgumentException(nameof(emailService));
            _sourceService = sourceService ?? throw new ArgumentException(nameof(sourceService));
            _supplierService = supplierService ?? throw new ArgumentException(nameof(supplierService));
            _leadService = leadService ?? throw new ArgumentException(nameof(leadService));
            _mapper = mapper;
            _apiResponseHandler = apiEndpointHandler;
        }


        [HttpGet()]
        public async Task<IActionResult> GetLeads(LeadFilter leadFilter)
        {
            return _apiResponseHandler.ReturnSearchResult<IEnumerable<Lead>,IEnumerable<LeadDto>>(
                await _leadService.GetLeadsAsync(leadFilter));        
        }

        [HttpGet("{id}", Name = "GetLead")]
        public async Task<IActionResult> GetLead(int id)
        {
            _logger.Log(LogLevel.Debug, "GET request to LeadsController, GetLead action");
                        
            var leadToReturn = await _leadService.GetLeadWithIdAsync(id, true, true, true, true);
            return _apiResponseHandler.ReturnSearchResult<Lead,LeadDto>(leadToReturn);
        }        

        [HttpPost]
        public async Task<IActionResult> CreateLead(LeadForCreateDto leadDto)
        {
            await ValidateLeadForCreateDto(leadDto);
            var newLead = _mapper.Map<Lead>(leadDto);

            return _apiResponseHandler.ReturnCreateResult<LeadForCreateDto>(await _leadService.CreateLeadAsync(newLead),
                "GetLead",
                newLead.LeadId.ToString(),
                newLead);
        }

        private async Task ValidateLeadForCreateDto(LeadForCreateDto leadDto)
        {
            var leadType = await _leadService.GetLeadTypeAsync(leadDto.LeadTypeId);
            _apiResponseHandler.ReturnNotFoundIfEntityDoesNotExist<LeadType>(leadType);

            var supplier = await _supplierService.GetSupplierWithIdAsync(leadDto.SupplierId);
            _apiResponseHandler.ReturnNotFoundIfEntityDoesNotExist<Supplier>(supplier);

            var source = await _sourceService.GetSourceWithIdAsync(leadDto.SourceId);
            _apiResponseHandler.ReturnNotFoundIfEntityDoesNotExist<Source>(source);

            var leadAttributesForLeadType = await _leadService.GetLeadAttributesAsync(leadType.LeadTypeId);

            foreach (var leadAttributeValue in leadDto.LeadAttributeValues)
            {
                var leadAttributeForLeadType = leadAttributesForLeadType.Where(l => l.Name == leadAttributeValue.LeadAttributeName).FirstOrDefault();

                _apiResponseHandler.ReturnBadRequestIfNull<LeadAttribute>(leadAttributeForLeadType,
                    $"The lead attribute {leadAttributeValue.LeadAttributeName} is not valid for lead type {leadType.Name}");

                leadAttributeValue.LeadAttributeId = leadAttributeForLeadType.LeadAttributeId; 
            }
        }

        [HttpPatch("{leadId}")]
        public async Task<IActionResult> UpdateLead(JsonPatchDocument<LeadForUpdateDto> patchDocument, int leadId)
        {
            await ValidateLeadDetailsAndMapChanges(patchDocument, leadId);
            return _apiResponseHandler.ReturndUpdateResult(await _leadService.UpdateLeadAsync(leadId));
        }

        private async Task ValidateLeadDetailsAndMapChanges(JsonPatchDocument<LeadForUpdateDto> patchDocument, int leadId)
        {
            var leadEntity = await _leadService.GetLeadWithIdAsync(leadId, false, false);
            _apiResponseHandler.ReturnNotFoundIfEntityDoesNotExist<Lead>(leadEntity);

            var leadDto = _mapper.Map<LeadForUpdateDto>(leadEntity);
            patchDocument.ApplyTo(leadDto);

            var supplier = await _supplierService.GetSupplierWithIdAsync(leadDto.SupplierId);
            _apiResponseHandler.ReturnNotFoundIfEntityDoesNotExist<Supplier>(supplier);

            var source = await _sourceService.GetSourceWithIdAsync(leadDto.SourceId);
            _apiResponseHandler.ReturnNotFoundIfEntityDoesNotExist<Source>(source);

            _mapper.Map(leadDto, leadEntity);
        }

        [HttpDelete("{id}", Name = "DeleteLead")]
        public async Task<IActionResult> DeleteLead(int id)
        {
            _logger.Log(LogLevel.Debug, "Request to LeadsController, DeleteLead action");                       

            var leadToDelete = await _leadService.GetLeadWithIdAsync(id);
            _apiResponseHandler.ReturnNotFoundIfEntityDoesNotExist<Lead>(leadToDelete);

            return _apiResponseHandler.ReturnDeleteResult(await _leadService.DeleteLead(leadToDelete));
        }
    }
}
