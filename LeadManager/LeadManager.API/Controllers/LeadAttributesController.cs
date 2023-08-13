using AutoMapper;
using LeadManager.Core.Interfaces;
using LeadManager.Core.Interfaces.Lead;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using LeadManager.Core.ViewModels;
using LeadManager.Core.Entities;

namespace LeadManager.API.Controllers
{
    [ApiController]
    [Route("api/leadattributes")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class LeadAttributesController : ControllerBase
    {
        private readonly ILogger<LeadAttributesController> _logger;
        private readonly IMapper _mapper;
        public ILeadService _leadService;
        private readonly IApiEndpointHandler _apiEndpointHandler;

        public LeadAttributesController(ILogger<LeadAttributesController> logger,
            ILeadService leadService,
            IMapper mapper,
            IApiEndpointHandler apiEndpointHandler)
        {
            _logger = logger ?? throw new ArgumentException(nameof(logger));
            _leadService = leadService ?? throw new ArgumentException(nameof(leadService));
            _mapper = mapper;
            _apiEndpointHandler = apiEndpointHandler;
        }

        [HttpGet("{id}", Name = "GetLeadAttribute")]
        public async Task<IActionResult> GetLeadAttribute(int id)
        {
            _logger.Log(LogLevel.Debug, "GET request to LeadAttributesController, GetLeadAttribute action");

            var leadAttributeToReturn = await _leadService.GetLeadAttributeAsync(id);
            return _apiEndpointHandler.ReturnSearchResult<LeadAttribute, LeadAttributeDto>(leadAttributeToReturn);
        }

        [HttpPost]
        public async Task<IActionResult> CreateLeadAttribute(LeadAttributeForCreateDto leadAttributeForCreateDto)
        {
            var newLeadAttribute = _mapper.Map<LeadAttribute>(leadAttributeForCreateDto);
            return _apiEndpointHandler.ReturnCreateResult<LeadAttributeForCreateDto>(await _leadService.CreateLeadAttributeAsync(newLeadAttribute),
                "GetLeadAttribute",
                newLeadAttribute.LeadAttributeId.ToString(),
                newLeadAttribute);
        }

        [HttpPatch("{leadAttributeId}")]
        public async Task<IActionResult> UpdateLeadType(JsonPatchDocument<LeadAttributeForUpdateDto> patchDocument, int leadAttributeId)
        {
            var leadAttributeEntity = await _leadService.GetLeadAttributeAsync(leadAttributeId);
            if (!_apiEndpointHandler.IsValidEntitySearchResult<LeadAttribute>(leadAttributeEntity))
                return BadRequest();

            var leadAttributeDto = _mapper.Map<LeadAttributeForUpdateDto>(leadAttributeEntity);
            patchDocument.ApplyTo(leadAttributeDto);


            _mapper.Map(leadAttributeDto, leadAttributeEntity);
            return _apiEndpointHandler.ReturndUpdateResult(await _leadService.UpdateLeadAttributeAsync(leadAttributeId));
        }


        [HttpDelete("{id}", Name = "DeleteLeadAttribute")]
        public async Task<IActionResult> DeleteLeadAttribute(int id)
        {
            _logger.Log(LogLevel.Debug, "Request to LeadAttributesController, DeleteLeadAttribute action");

            var leadAttributeToDelete = await _leadService.GetLeadAttributeAsync(id);
            if (!_apiEndpointHandler.IsValidEntitySearchResult<LeadAttribute>(leadAttributeToDelete))
                return BadRequest();

            return _apiEndpointHandler.ReturnDeleteResult(await _leadService.DeleteLeadAttribute(leadAttributeToDelete));
        }
    }
}
