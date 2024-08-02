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

        public DataParserService(DocumentIntelligenceClient documentIntelligenceClient, ILogger<DataParserService> logger)
        {
            _documentIntelligenceClient = documentIntelligenceClient;
            _logger = logger;
        }

        public async Task<AnalyzeResult> ParseDataAsync(AnalyzeDocumentContent content)
        {
            _logger.LogInformation("Started parsing data.");

            try
            {

                var result = await CheckConnectionAsync(content);
                _logger.LogInformation("Data parsing completed.");

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while parsing data.");
                return null;
            }
        }

        private async Task<AnalyzeResult> CheckConnectionAsync(AnalyzeDocumentContent content)
        {
            try
            {
                _logger.LogInformation("Checking connection to Document Intelligence API.");

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
