using System;
using System.IO;
using System.Threading.Tasks;
using Azure;
using Azure.AI.DocumentIntelligence;
using Microsoft.Extensions.Logging;

namespace DocumentDataParser.Services
{
    public class DataParserService : IDataParser
    {
        private readonly DocumentIntelligenceClient _documentIntelligenceClient;
        private readonly ILogger<DataParserService> _logger;

        // Constructor injection for DocumentIntelligenceClient and ILogger
        public DataParserService(DocumentIntelligenceClient documentIntelligenceClient, ILogger<DataParserService> logger)
        {
            _documentIntelligenceClient = documentIntelligenceClient;
            _logger = logger;
        }

        public async Task<AnalyzeResult> ParseDataAsync(MemoryStream memoryStream)
        {
            _logger.LogInformation("Started parsing data.");

            if (memoryStream == null || memoryStream.Length == 0)
            {
                _logger.LogWarning("Received empty or null MemoryStream.");
                return null;
            }

            try
            {
                using (var reader = new StreamReader(memoryStream))
                {
                    var fileContent = await reader.ReadToEndAsync();
                    _logger.LogInformation("Read file content successfully.");
                }

                memoryStream.Position = 0;

                var result = await CheckConnectionAsync(memoryStream);
                _logger.LogInformation("Data parsing completed.");

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while parsing data.");
                return null;
            }
        }

        private async Task<AnalyzeResult> CheckConnectionAsync(MemoryStream stream)
        {
            try
            {
                _logger.LogInformation("Checking connection to Document Intelligence API.");

                var content = new AnalyzeDocumentContent()
                {
                    Base64Source = new BinaryData(stream.ToArray())
                };

                Operation<AnalyzeResult> operation = await _documentIntelligenceClient.AnalyzeDocumentAsync(
                    WaitUntil.Completed, "prebuilt-layout", content);

                var responseData = operation.Value;

                if (responseData == null)
                {
                    _logger.LogError("AnalyzeDocumentAsync returned null result.");
                }
                else
                {
                    _logger.LogInformation("AnalyzeDocumentAsync returned a valid result.");
                }

                return responseData;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while calling AnalyzeDocumentAsync.");
                return null;
            }
        }
    }
}
