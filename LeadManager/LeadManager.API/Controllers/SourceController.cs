using LeadManager.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace LeadManager.API.Controllers
{
    [ApiController]
    [Route("/api/sources")]
    public class SourceController : ControllerBase
    {
        [HttpGet()]
        public ActionResult<IEnumerable<SourceDto>> GetSources()
        {
            return Ok(TestDataStore.Current);
        }

        [HttpGet("{id}")]
        public ActionResult<SourceDto> GetSources(int id)
        {
           var sourceToReturn = TestDataStore.Current.Sources.FirstOrDefault(x => x.Id == id);

            if (sourceToReturn == null)
                return NotFound();

            return Ok(sourceToReturn);
        }
    }
}
