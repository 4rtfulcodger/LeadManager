using AutoMapper;
using LeadManager.API.BusinessLogic.Common;
using LeadManager.Core.Entities.Source;
using LeadManager.Core.Entities.Supplier;
using LeadManager.Core.Helpers;
using LeadManager.Core.Interfaces;
using LeadManager.Core.Interfaces.Source;
using LeadManager.Core.ViewModels;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace LeadManager.API.Controllers
{
    [ApiController]
    [Route("/api/sources")]
    public class SourcesController : ControllerBase
    {
        private readonly ILogger<FilesController> _logger;
        private readonly IMapper _mapper;
        private readonly ISourceService _sourceService;
        private readonly IApiResponseHandler _apiResponseHandler;

        public SourcesController(ISourceService sourceService,
            ILogger<FilesController> logger,
            IMapper mapper,
            IApiResponseHandler apiEndpointValidation)
        {
            _logger = logger ?? throw new ArgumentException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentException(nameof(mapper));
            _sourceService = sourceService ?? throw new ArgumentException(nameof(sourceService));
            _apiResponseHandler = apiEndpointValidation ?? throw new ArgumentException(nameof(apiEndpointValidation));
        }

        [HttpGet()]
        public async Task<IActionResult> GetSources(SourceFilter filter)
        {
            var filteredSources = await _sourceService.GetSourcesAsync(filter);
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(PagedList<Source>.GetPaginationMetadata(filteredSources)));

            return _apiResponseHandler.ReturnSearchResult<IEnumerable<Source>, SourceDto[]>(filteredSources);
        }

        [HttpGet("{id}", Name = "GetSource")]
        public async Task<IActionResult> GetSources(int id)
        {           
           return _apiResponseHandler.ReturnSearchResult<Source, SourceDto>(
               await _sourceService.GetSourceWithIdAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> CreateSource(SourceForCreateDto sourceDto)
        {
            var newSource = _mapper.Map<Source>(sourceDto);

            return _apiResponseHandler.ReturnCreateResult<SourceDto>(await _sourceService.CreateSourceAsync(newSource),
                "GetSource",
                newSource.SourceId.ToString(),
                newSource);
        }

        [HttpPatch("{sourceId}")]
        public async Task<IActionResult> UpdateSource(JsonPatchDocument<SourceForUpdateDto> patchDocument, int sourceId)
        {
            var sourceEntity = await _sourceService.GetSourceWithIdAsync(sourceId);
            _apiResponseHandler.ReturnNotFoundIfEntityDoesNotExist<Source>(sourceEntity);

            var sourceForUpdateDto = _mapper.Map<SourceForUpdateDto>(sourceEntity);
            patchDocument.ApplyTo(sourceForUpdateDto);                     
            _mapper.Map(sourceForUpdateDto, sourceEntity);
            
            return _apiResponseHandler.ReturndUpdateResult(await _sourceService.UpdateSourceAsync(sourceId));
        }

        [HttpDelete("{id}", Name = "DeleteSource")]
        public async Task<IActionResult> DeleteSource(int id)
        {         
            _logger.Log(LogLevel.Debug, "Request to SourcesController, DeleteSource action");

            var sourceToDelete = await _sourceService.GetSourceWithIdAsync(id);
            _apiResponseHandler.ReturnNotFoundIfEntityDoesNotExist<Source>(sourceToDelete);

            return _apiResponseHandler.ReturnDeleteResult(await _sourceService.DeleteSource(sourceToDelete));
        }
    }
}
