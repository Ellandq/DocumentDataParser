using Azure.AI.DocumentIntelligence;
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
            Logger.LogInfo("Halo");
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                memoryStream.Position = 0;
                
                AnalyzeResult result;
                try{
                    result = await _dataParserService.ParseDataAsync(memoryStream);
                }catch(Exception e){
                    result = null;
                    Logger.LogError($"Error: {e.Message}", e);
                }
                

                if (result != null)
                {
                    Logger.LogInfo("TEST");
                    return Ok($"File processed successfully. ");
                }
                else
                {
                    return StatusCode(500, "Error processing file.");
                }
            }
        }

    }
}
