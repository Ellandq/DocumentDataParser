using System.IO;
using System.Threading.Tasks;
using Azure;
using Azure.Core;
using Azure.AI.DocumentIntelligence;
using DocumentDataParser.Services;

namespace DocumentDataParser.Services
{
    public class DataParserService(DocumentIntelligenceClient documentIntelligenceClient) : IDataParser
    {
        public async Task<bool> ParseDataAsync(Stream fileStream)
        {
            using (var reader = new StreamReader(fileStream))
            {
                var fileContent = await reader.ReadToEndAsync();
            }

            return CheckConnectionAsync() == null; 
        }

        private async Task<BinaryData> CheckConnectionAsync()
        {
            try
            {
                using RequestContent content = null;
                Operation<BinaryData> operation = documentIntelligenceClient.AnalyzeDocument(WaitUntil.Completed, "<modelId>", content);
                BinaryData responseData = operation.Value;

                return responseData; 
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
