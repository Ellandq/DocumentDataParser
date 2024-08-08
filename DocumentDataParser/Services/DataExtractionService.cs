using System.Text;
using Azure.AI.DocumentIntelligence;
using DocumentDataParser.Model;
using Newtonsoft.Json;

namespace DocumentDataParser.Services{
    public class DataExtractionService : IDataExtraction
    {

        private readonly ILogger<DataExtractionService> _logger;

        public DataExtractionService(ILogger<DataExtractionService> logger){
            _logger = logger;
        }
        
       public async Task<ReturnObject> ExtractDataToObject(ReturnObject returnObject, AnalyzeResult analyzeResult)
        {
            if (analyzeResult == null)
            {
                throw new ArgumentNullException(nameof(analyzeResult), "AnalyzeResult cannot be null.");
            }

            string jsonString = JsonConvert.SerializeObject(analyzeResult, Formatting.Indented);
            _logger.LogError(jsonString);



            foreach (var paraghraph in analyzeResult.Paragraphs)
            {

            }




            return returnObject;
        }

        private enum SectionName{
            BuyerNames, BuyerSurnames,
        }


    }

    
}