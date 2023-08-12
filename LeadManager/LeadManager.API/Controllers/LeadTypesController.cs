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
using LeadManager.Infrastructure.Services;

namespace LeadManager.API.Controllers
{
    
    [ApiController]
    [Route("api/leads/leadtypes")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class LeadTypesController : ControllerBase
    {
        private readonly ILogger<FilesController> _logger;
        private readonly IMapper _mapper;
        public ILeadService _leadService;
        private readonly IApiEndpointHandler _apiEndpointHandler;

        public LeadTypesController(ILogger<FilesController> logger,
            ILeadService leadService,
            IMapper mapper,
            IApiEndpointHandler apiEndpointHandler)
        {
            _logger = logger ?? throw new ArgumentException(nameof(logger));
            _leadService = leadService ?? throw new ArgumentException(nameof(leadService));
            _mapper = mapper;
            _apiEndpointHandler = apiEndpointHandler;
        }       

        [HttpGet("{id}", Name = "GetLeadType")]
        public async Task<IActionResult> GetLeadType(int id)
        {
            _logger.Log(LogLevel.Debug, "GET request to LeadTypesController, GetLeadType action");

            var leadTypeToReturn = await _leadService.GetLeadTypeAsync(id);
            return _apiEndpointHandler.ReturnSearchResult<LeadType, LeadTypeDto>(leadTypeToReturn);
        }

        [HttpPost]
        public async Task<IActionResult> CreateLeadType(LeadTypeForCreateDto leadTypeForCreateDto)
        {
            var newLeadType = _mapper.Map<LeadType>(leadTypeForCreateDto);
            return _apiEndpointHandler.ReturnCreateResult<LeadTypeForCreateDto>(await _leadService.CreateLeadTypeAsync(newLeadType),
                "GetLeadType",
                newLeadType.LeadTypeId.ToString(),
                newLeadType);
        }

        [HttpPatch("{leadTypeId}")]
        public async Task<IActionResult> UpdateLeadType(JsonPatchDocument<LeadTypeForUpdateDto> patchDocument, int leadTypeId)
        {
            var leadTypeEntity = await _leadService.GetLeadTypeAsync(leadTypeId);
            if (!_apiEndpointHandler.IsValidEntitySearchResult<LeadType>(leadTypeEntity))
                return BadRequest();

            var leadTypeDto = _mapper.Map<LeadTypeForUpdateDto>(leadTypeEntity);
            patchDocument.ApplyTo(leadTypeDto);
                      

            _mapper.Map(leadTypeDto, leadTypeEntity);
            return _apiEndpointHandler.ReturndUpdateResult(await _leadService.UpdateLeadTypeAsync(leadTypeId));
        }


        [HttpDelete("{id}", Name = "DeleteLeadType")]
        public async Task<IActionResult> DeleteLeadType(int id)
        {
            _logger.Log(LogLevel.Debug, "Request to LeadTypesController, DeleteLeadType action");

            var leadTypeToDelete = await _leadService.GetLeadTypeAsync(id);
            if (!_apiEndpointHandler.IsValidEntitySearchResult<LeadType>(leadTypeToDelete))
                return BadRequest();

            return _apiEndpointHandler.ReturnDeleteResult(await _leadService.DeleteLeadType(leadTypeToDelete));
        }
    }
}