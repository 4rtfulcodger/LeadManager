using LeadManager.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace LeadManager.API.Controllers
{
    [ApiController]
    [Route("/api/sources")]
    public class SourcesController : ControllerBase
    {
        [HttpGet()]
        public ActionResult<IEnumerable<SourceDto>> GetSources()
        {
            return Ok(TestDataStore.Current.Sources);
        }

        [HttpGet("{id}", Name = "GetSource")]
        public ActionResult<SourceDto> GetSources(int id)
        {
           var sourceToReturn = TestDataStore.Current.Sources.FirstOrDefault(x => x.Id == id);

            if (sourceToReturn == null)
                return NotFound();

            return Ok(sourceToReturn);
        }

        [HttpPost]
        public ActionResult<SourceDto> CreateSource(SourceForCreateDto source)
        {
            var newSource = new SourceDto
            {
                Id = TestDataStore.Current.Sources.Count() + 1,
                Name = source.Name,
                Description = source.Description
            };

            TestDataStore.Current.Sources.Add(newSource);

            return CreatedAtRoute("GetSource", new { id = newSource.Id }, newSource); 

        }
    }
}
