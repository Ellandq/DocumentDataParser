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
        public async Task<AnalyzeResult> ParseDataAsync(MemoryStream memoryStream)
        {
            using (var reader = new StreamReader(memoryStream))
            {
                var fileContent = await reader.ReadToEndAsync();
            }
            var result = await CheckConnectionAsync(memoryStream);
            return result; 
        }

        private async Task<AnalyzeResult> CheckConnectionAsync(MemoryStream stream)
        {
            Logger.LogInfo($"Trying to connect to {documentIntelligenceClient}");
            try
            {
                var content = new AnalyzeDocumentContent(){
                    Base64Source = new BinaryData(stream.ToArray())
                };

                Operation<AnalyzeResult> operation = await documentIntelligenceClient.AnalyzeDocumentAsync(WaitUntil.Completed, "prebuilt-layout", content);
                var responseData = operation.Value;

                return responseData; 
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
