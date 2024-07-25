using System.IO;
using System.Threading.Tasks;
using DocumentDataParser.Services;

namespace DocumentDataParser.Services
{
    public class DataParserService : IDataParser
    {
        public async Task<bool> ParseDataAsync(Stream fileStream)
        {
            using (var reader = new StreamReader(fileStream))
            {
                var fileContent = await reader.ReadToEndAsync();
            }

            return true; 
        }
    }
}
