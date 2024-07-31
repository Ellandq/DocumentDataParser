using DocumentDataParser.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Threading.Tasks;

namespace DocumentDataParser.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FileTransferController : ControllerBase
    {
        private readonly IDataParser _dataParserService;
        private readonly ILogger<FileTransferController> _logger;

        public FileTransferController(IDataParser dataParserService, ILogger<FileTransferController> logger)
        {
            _dataParserService = dataParserService;
            _logger = logger;
        }

        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                _logger.LogWarning("No file uploaded.");
                return BadRequest("No file uploaded.");
            }

            try
            {
                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    memoryStream.Position = 0;

                    var result = await _dataParserService.ParseDataAsync(memoryStream);

                    if (result != null)
                    {
                        _logger.LogInformation("File processed successfully.");
                        return Ok("File processed successfully.");
                    }
                    else
                    {
                        _logger.LogError("Error processing file.");
                        return StatusCode(500, "Error processing file.");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while processing file.");
                return StatusCode(500, "An error occurred while processing the file.");
            }
        }
    }
}
