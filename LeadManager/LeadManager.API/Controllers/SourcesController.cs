using AutoMapper;
using LeadManager.Core.Entities.Source;
using LeadManager.Core.Interfaces;
using LeadManager.Core.Interfaces.Source;
using LeadManager.Core.ViewModels;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace LeadManager.API.Controllers
{
    [ApiController]
    [Route("/api/sources")]
    public class SourcesController : ControllerBase
    {
        private readonly ILogger<FilesController> _logger;
        private readonly IMapper _mapper;
        private readonly ISourceService _sourceService;
        private readonly IApiResponseHandler _apiEndpointHandler;

        public SourcesController(ISourceService sourceService,
            ILogger<FilesController> logger,
            IMapper mapper,
            IApiResponseHandler apiEndpointValidation)
        {
            _logger = logger ?? throw new ArgumentException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentException(nameof(mapper));
            _sourceService = sourceService ?? throw new ArgumentException(nameof(sourceService));
            _apiEndpointHandler = apiEndpointValidation ?? throw new ArgumentException(nameof(apiEndpointValidation));
        }       

        [HttpGet()]
        public async Task<IActionResult> GetSources()
        {
            //Need to add a filter parameter
            var searchResult = await _sourceService.GetSourcesAsync();
            return _apiEndpointHandler.ReturnSearchResult<IEnumerable<Source>, SourceDto[]>(searchResult);
        }

        [HttpGet("{id}", Name = "GetSource")]
        public async Task<IActionResult> GetSources(int id)
        {
           var sourceToReturn = await _sourceService.GetSourceWithIdAsync(id);
           return _apiEndpointHandler.ReturnSearchResult<Source, SourceDto>(sourceToReturn);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSource(SourceForCreateDto sourceDto)
        {
            var newSource = _mapper.Map<Source>(sourceDto);

            return _apiEndpointHandler.ReturnCreateResult<SourceDto>(await _sourceService.CreateSourceAsync(newSource),
                "GetSource",
                newSource.SourceId.ToString(),
                newSource);
        }

        [HttpPatch("{sourceId}")]
        public async Task<IActionResult> UpdateSource(JsonPatchDocument<SourceForUpdateDto> patchDocument, int sourceId)
        {
            var sourceEntity = await _sourceService.GetSourceWithIdAsync(sourceId);
            if(!_apiEndpointHandler.IsValidEntitySearchResult<Source>(sourceEntity))
                return BadRequest();

            var sourceForUpdateDto = _mapper.Map<SourceForUpdateDto>(sourceEntity);
            patchDocument.ApplyTo(sourceForUpdateDto);                     
            _mapper.Map(sourceForUpdateDto, sourceEntity);
            
            return _apiEndpointHandler.ReturndUpdateResult(await _sourceService.UpdateSourceAsync(sourceId));
        }

        [HttpDelete("{id}", Name = "DeleteSource")]
        public async Task<IActionResult> DeleteSource(int id)
        {         
            _logger.Log(LogLevel.Debug, "Request to SourcesController, DeleteSource action");

            var sourceToDelete = await _sourceService.GetSourceWithIdAsync(id);

            if(_apiEndpointHandler.IsValidEntitySearchResult<Source>(sourceToDelete))            
               return _apiEndpointHandler.ReturnDeleteResult(await _sourceService.DeleteSource(sourceToDelete));

            return BadRequest();
        }
    }
}
