using System.IO;
using System.Threading.Tasks;
using Azure.AI.DocumentIntelligence;

namespace DocumentDataParser.Services
{
    public interface IDataParser
    {
        Task<AnalyzeResult> ParseDataAsync(MemoryStream fileStream);
    }
}
