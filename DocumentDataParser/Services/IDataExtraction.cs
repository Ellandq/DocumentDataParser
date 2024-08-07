using Azure.AI.DocumentIntelligence;
using DocumentDataParser.Model;

namespace DocumentDataParser.Services
{
    public interface IDataExtraction{
        Task<ReturnObject> ExtractDataToObject(ReturnObject returnObject, AnalyzeResult analyzeResult);
    }
}
