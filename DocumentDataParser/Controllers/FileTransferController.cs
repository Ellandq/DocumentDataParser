using Azure.AI.DocumentIntelligence;
using DocumentDataParser.Services;
using DocumentDataParser.Utils;
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

            AnalyzeDocumentContent content;

            try{
                content = await StreamParser.GetAnalyzeDocumentContentFromFile(file);

                if (content == null){
                    _logger.LogError("Error while converting file.");
                    return StatusCode(500, "Error while converting file.");
                }
            }catch (Exception e){
                _logger.LogError(e, "Error while converting file.");
                return StatusCode(500, $"Error while converting file: {e.Message}");
            }
            
            try
            {

                var result = await _dataParserService.ParseDataAsync(content);

                if (result != null)
                {
                    _logger.LogInformation("File processed successfully.");
                    return Ok("File processed successfully.");
                }
                else
                {
                    _logger.LogError("Error processing file.");
                    return StatusCode(500, "Error processing file. Wierd huh?");
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
