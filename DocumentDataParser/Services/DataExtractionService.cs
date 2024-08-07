using System.Text;
using Azure.AI.DocumentIntelligence;
using DocumentDataParser.Model;
using  Newtonsoft.Json;

namespace DocumentDataParser.Services{
    public class DataExtractionService : IDataExtraction{

        private readonly ILogger _logger;
        
        public DataExtractionService(ILogger logger){
            _logger = logger;
        }
        
       public async Task<ReturnObject> ExtractDataToObject(ReturnObject returnObject, AnalyzeResult analyzeResult)
        {
            if (analyzeResult == null)
            {
                throw new ArgumentNullException(nameof(analyzeResult), "AnalyzeResult cannot be null.");
            }

            var sb = new StringBuilder();

            string jsonString = JsonConvert.SerializeObject(analyzeResult, Formatting.Indented);
            _logger.LogError(jsonString);

            return returnObject;
        }


    }
}