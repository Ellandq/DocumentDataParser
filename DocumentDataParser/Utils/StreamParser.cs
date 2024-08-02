
using Azure.AI.DocumentIntelligence;

namespace DocumentDataParser.Utils
{
    public class StreamParser
    {
        public static async Task<AnalyzeDocumentContent> GetAnalyzeDocumentContentFromFile(IFormFile file){

            var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            memoryStream.Position = 0;

            return new AnalyzeDocumentContent()
            {
                Base64Source = new BinaryData(memoryStream.ToArray())
            };
        }
    }
}