using AutoMapper;
using LeadManager.API.Models;
using LeadManager.Core.Entities;
using LeadManager.Core.Interfaces;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace LeadManager.API.Controllers
{
    [ApiController]
    [Route("/api/sources")]
    public class SourcesController : ControllerBase
    {
        private readonly ILogger<FilesController> _logger;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;
        public ILeadInfoRepository _leadInfoRepository;

        public SourcesController(ILeadInfoRepository leadInfoRepository,
            ILogger<FilesController> logger,
            IEmailService emailService,
            IMapper mapper)
        {
            _leadInfoRepository = leadInfoRepository ?? throw new ArgumentException(nameof(leadInfoRepository)); ;
            _logger = logger ?? throw new ArgumentException(nameof(logger));
            _emailService = emailService ?? throw new ArgumentException(nameof(emailService));
            _mapper = mapper;
        }       

        [HttpGet()]
        public async Task<ActionResult<IEnumerable<SourceDto>>> GetSources()
        {
            //Need to add a filter parameter
            return Ok(_mapper.Map<SourceDto[]>(await _leadInfoRepository.GetSourcesAsync()));
        }

        [HttpGet("{id}", Name = "GetSource")]
        public async Task<ActionResult<SourceDto>> GetSources(int id)
        {
            var sourceToReturn = await _leadInfoRepository.GetSourceWithIdAsync(id);

            if (sourceToReturn == null)
                return NotFound();

            return Ok(_mapper.Map<SourceDto>(sourceToReturn));
        }

        [HttpPost]
        public async Task<ActionResult<SourceDto>> CreateSource(SourceForCreateDto sourceDto)
        {
            var newSource = _mapper.Map<Source>(sourceDto);
            bool addLeadSuccess = await _leadInfoRepository.AddSourceAsync(newSource);

            return CreatedAtRoute("GetSource", new { id = newSource.SourceId }, _mapper.Map<SourceDto>(newSource)); 

        }

        [HttpPatch("{sourceId}")]
        public async Task<ActionResult<LeadDto>> UpdateSource(JsonPatchDocument<SourceForUpdateDto> patchDocument, int sourceId)
        {
            var sourceEntity = await _leadInfoRepository.GetSourceWithIdAsync(sourceId);
            if (sourceEntity == null)
                return NotFound();

            var sourceDto = _mapper.Map<SourceForUpdateDto>(sourceEntity);
            patchDocument.ApplyTo(sourceDto);                       

            _mapper.Map(sourceDto, sourceEntity);
            bool updateresult = await _leadInfoRepository.UpdateSourceAsync(sourceId);

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

            var sourceToDelete = await _leadInfoRepository.GetSourceWithIdAsync(id);

            if (sourceToDelete == null)
                return NotFound();

            var deleteResult = await _leadInfoRepository.DeleteSource(sourceToDelete);

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
