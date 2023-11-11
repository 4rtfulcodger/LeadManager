using AutoMapper;
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
using System.Text.Json;

namespace LeadManager.API.Controllers
{
    
    [ApiController]
    [Route("api/leads/leadtypes")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class LeadTypesController : ControllerBase
    {
        private readonly ILogger<LeadTypesController> _logger;
        private readonly IMapper _mapper;
        public ILeadService _leadService;
        private readonly IApiResponseHandler _apiResponseHandler;

        public LeadTypesController(ILogger<LeadTypesController> logger,
            ILeadService leadService,
            IMapper mapper,
            IApiResponseHandler apiEndpointHandler)
        {
            _logger = logger ?? throw new ArgumentException(nameof(logger));
            _leadService = leadService ?? throw new ArgumentException(nameof(leadService));
            _mapper = mapper;
            _apiResponseHandler = apiEndpointHandler;
        }

        [HttpGet()]
        public async Task<IActionResult> GetLeadTypes(LeadTypeFilter filter)
        {
            _logger.Log(LogLevel.Debug, "GET request to LeadTypesController, GetLeadTypes action");

            var filteredLeadTypes = await _leadService.GetLeadTypesAsync(filter);

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(PagedList<LeadType>.GetPaginationMetadata(filteredLeadTypes)));
            return _apiResponseHandler.ReturnSearchResult<IEnumerable<LeadType>, LeadTypeDto[]>(filteredLeadTypes);            
        }

        [HttpGet("{id}", Name = "GetLeadType")]
        public async Task<IActionResult> GetLeadType(int id)
        {
            _logger.Log(LogLevel.Debug, "GET request to LeadTypesController, GetLeadType action");

            var leadTypeToReturn = await _leadService.GetLeadTypeAsync(id);
            return _apiResponseHandler.ReturnSearchResult<LeadType, LeadTypeDto>(leadTypeToReturn);
        }

        [HttpPost]
        public async Task<IActionResult> CreateLeadType(LeadTypeForCreateDto leadTypeForCreateDto)
        {
            var newLeadType = _mapper.Map<LeadType>(leadTypeForCreateDto);
            return _apiResponseHandler.ReturnCreateResult<LeadTypeForCreateDto>(await _leadService.CreateLeadTypeAsync(newLeadType),
                "GetLeadType",
                newLeadType.LeadTypeId.ToString(),
                newLeadType);
        }

        [HttpPatch("{leadTypeId}")]
        public async Task<IActionResult> UpdateLeadType(JsonPatchDocument<LeadTypeForUpdateDto> patchDocument, int leadTypeId)
        {
            var leadTypeEntity = await _leadService.GetLeadTypeAsync(leadTypeId);
            _apiResponseHandler.ReturnNotFoundIfEntityDoesNotExist<LeadType>(leadTypeEntity);

            var leadTypeDto = _mapper.Map<LeadTypeForUpdateDto>(leadTypeEntity);
            patchDocument.ApplyTo(leadTypeDto);
            _mapper.Map(leadTypeDto, leadTypeEntity);

            return _apiResponseHandler.ReturndUpdateResult(await _leadService.UpdateLeadTypeAsync(leadTypeId));
        }

        [HttpDelete("{id}", Name = "DeleteLeadType")]
        public async Task<IActionResult> DeleteLeadType(int id)
        {
            _logger.Log(LogLevel.Debug, "Request to LeadTypesController, DeleteLeadType action");

            var leadTypeToDelete = await _leadService.GetLeadTypeAsync(id);
            _apiResponseHandler.ReturnNotFoundIfEntityDoesNotExist<LeadType>(leadTypeToDelete);

            return _apiResponseHandler.ReturnDeleteResult(await _leadService.DeleteLeadType(leadTypeToDelete));
        }
    }
}
