using AutoMapper;
using LeadManager.Core.Entities.Source;
using LeadManager.Core.Interfaces;
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

        public SourcesController(ISourceService sourceService,
            ILogger<FilesController> logger,
            IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentException(nameof(mapper));
            _sourceService = sourceService ?? throw new ArgumentException(nameof(sourceService)); ;
        }       

        [HttpGet()]
        public async Task<ActionResult<IEnumerable<SourceDto>>> GetSources()
        {
            //Need to add a filter parameter
            return Ok(_mapper.Map<SourceDto[]>(await _sourceService.GetSourcesAsync()));
        }

        [HttpGet("{id}", Name = "GetSource")]
        public async Task<ActionResult<SourceDto>> GetSources(int id)
        {
            var sourceToReturn = await _sourceService.GetSourceWithIdAsync(id);

            if (sourceToReturn == null)
                return NotFound();

            return Ok(_mapper.Map<SourceDto>(sourceToReturn));
        }

        [HttpPost]
        public async Task<ActionResult<SourceDto>> CreateSource(SourceForCreateDto sourceDto)
        {
            var newSource = _mapper.Map<Source>(sourceDto);
            bool addLeadSuccess = await _sourceService.CreateSourceAsync(newSource);

            return CreatedAtRoute("GetSource", new { id = newSource.SourceId }, _mapper.Map<SourceDto>(newSource)); 

        }

        [HttpPatch("{sourceId}")]
        public async Task<ActionResult<LeadDto>> UpdateSource(JsonPatchDocument<SourceForUpdateDto> patchDocument, int sourceId)
        {
            var sourceEntity = await _sourceService.GetSourceWithIdAsync(sourceId);
            if (sourceEntity == null)
                return NotFound();

            var sourceDto = _mapper.Map<SourceForUpdateDto>(sourceEntity);
            patchDocument.ApplyTo(sourceDto);                       

            _mapper.Map(sourceDto, sourceEntity);
            bool updateresult = await _sourceService.UpdateSourceAsync(sourceId);

            if (updateresult)
            {
                return NoContent();
            }
            else
            {
                return Problem();
            }

        }

        [HttpDelete("{id}", Name = "DeleteSource")]
        public async Task<ActionResult<SourceDto>> DeleteSource(int id)
        {
            //Validation and filter logic should be removed from controllers
            //Temporarily adding these for testing

            _logger.Log(LogLevel.Debug, "Request to SourcesController, DeleteSource action");

            var sourceToDelete = await _sourceService.GetSourceWithIdAsync(id);

            if (sourceToDelete == null)
                return NotFound();

            var deleteResult = await _sourceService.DeleteSource(sourceToDelete);

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
