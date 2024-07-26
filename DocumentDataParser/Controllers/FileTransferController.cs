using DocumentDataParser.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace DocumentDataParser.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FileTransferController(IDataParser _dataParserService) : ControllerBase
    {
        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                memoryStream.Position = 0;

                var result = await _dataParserService.ParseDataAsync(memoryStream);

                if (result != null)
                {
                    return Ok($"File processed successfully. {result.Content}");
                }
                else
                {
                    return StatusCode(500, "Error processing file.");
                }
            }
        }

    }
}
