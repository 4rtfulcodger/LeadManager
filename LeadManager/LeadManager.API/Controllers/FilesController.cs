using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace LeadManager.API.Controllers
{
    [Route("api/files")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly FileExtensionContentTypeProvider _fileExtensionContentTypeProvider;

        //public FilesController(FileExtensionContentTypeProvider fileExtensionContentTypeProvider)
        //{
        //    _fileExtensionContentTypeProvider = fileExtensionContentTypeProvider
        //        ?? throw new System.ArgumentNullException(
        //            nameof(fileExtensionContentTypeProvider));
        //}


        [HttpGet("{fileId}")]
        public ActionResult GetFile(string fileId)
        {
            //The logic to lookup the file path needs to be written
            //This is just for demo porposes
            string pathToFile = "Files/sns-menukort-2022-08-uk-150x380-low.pdf";

            //First check if the file exists, if not return a not found response
            if (!System.IO.File.Exists(pathToFile))
                return NotFound();

            var bytes = System.IO.File.ReadAllBytes(pathToFile);

            //if (!_fileExtensionContentTypeProvider.TryGetContentType(pathToFile, out var contentType))
            //    contentType = "application/octet-stream";

            return File(bytes, "txt/plain", Path.GetFileName(pathToFile));
            
        }

    }
}
