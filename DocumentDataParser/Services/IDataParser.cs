using System.IO;
using System.Threading.Tasks;
using Azure.AI.DocumentIntelligence;
using DocumentDataParser.Model;

namespace DocumentDataParser.Services
{
    public interface IDataParser
    {
        Task<ReturnObject> ParseDataAsync(AnalyzeDocumentContent content);
    }
}
