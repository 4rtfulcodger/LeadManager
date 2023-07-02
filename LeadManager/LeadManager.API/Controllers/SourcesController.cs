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
        private readonly IApiEndpointValidation _apiEndpointValidation;

        public SourcesController(ISourceService sourceService,
            ILogger<FilesController> logger,
            IMapper mapper,
            IApiEndpointValidation apiEndpointValidation)
        {
            _logger = logger ?? throw new ArgumentException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentException(nameof(mapper));
            _sourceService = sourceService ?? throw new ArgumentException(nameof(sourceService));
            _apiEndpointValidation = apiEndpointValidation ?? throw new ArgumentException(nameof(apiEndpointValidation));
        }       

        [HttpGet()]
        public async Task<IActionResult> GetSources()
        {
            //Need to add a filter parameter
            var searchResult = _mapper.Map<SourceDto[]>(await _sourceService.GetSourcesAsync());
            if(!searchResult.Any())
                return NotFound();

            return Ok(searchResult);
        }

        [HttpGet("{id}", Name = "GetSource")]
        public async Task<IActionResult> GetSources(int id)
        {
           var sourceToReturn = await _sourceService.GetSourceWithIdAsync(id);
           if (!_apiEndpointValidation.IsValidEntitySearchResult<Source>(sourceToReturn))
                return NotFound(sourceToReturn);                          

            return Ok(_mapper.Map<SourceDto>(sourceToReturn));
        }

        [HttpPost]
        public async Task<IActionResult> CreateSource(SourceForCreateDto sourceDto)
        {
            var newSource = _mapper.Map<Source>(sourceDto);
            if(!(await _sourceService.CreateSourceAsync(newSource)))
                return BadRequest();

            return CreatedAtRoute("GetSource", new { id = newSource.SourceId }, _mapper.Map<SourceDto>(newSource)); 

        }

        [HttpPatch("{sourceId}")]
        public async Task<IActionResult> UpdateSource(JsonPatchDocument<SourceForUpdateDto> patchDocument, int sourceId)
        {
            var sourceEntity = await _sourceService.GetSourceWithIdAsync(sourceId);
            if(!_apiEndpointValidation.IsValidEntitySearchResult<Source>(sourceEntity))
                return BadRequest();

            var sourceDto = _mapper.Map<SourceForUpdateDto>(sourceEntity);
            patchDocument.ApplyTo(sourceDto);                       

            _mapper.Map(sourceDto, sourceEntity);
            if(!(await _sourceService.UpdateSourceAsync(sourceId)))
                return BadRequest();

            return NoContent();
        }

        [HttpDelete("{id}", Name = "DeleteSource")]
        public async Task<IActionResult> DeleteSource(int id)
        {         
            _logger.Log(LogLevel.Debug, "Request to SourcesController, DeleteSource action");

            var sourceToDelete = await _sourceService.GetSourceWithIdAsync(id);
            _apiEndpointValidation.IsValidDeleteResult(sourceToDelete);

            _apiEndpointValidation.IsValidUpdateResult(await _sourceService.DeleteSource(sourceToDelete));
            return NoContent();
        }
    }
}
