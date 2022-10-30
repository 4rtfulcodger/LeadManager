using Microsoft.AspNetCore.Mvc;

namespace LeadManager.API.Controllers
{
    [ApiController]
    [Route("/api/sources")]
    public class SourceController : ControllerBase
    {
        [HttpGet()]
        public JsonResult GetSources()
        {
            return new JsonResult(TestDataStore.Current);
        }

        [HttpGet("{id}")]
        public JsonResult GetSources(int id)
        {
            return new JsonResult(TestDataStore.Current.Sources.FirstOrDefault(x => x.Id == id));
        }
    }
}
