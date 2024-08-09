using System.Text;
using Azure.AI.DocumentIntelligence;
using DocumentDataParser.Enums;
using DocumentDataParser.Model;
using DocumentDataParser.Utils;
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

            var ignoredSections = new List<SectionName>();
            var dictionary = new Dictionary<SectionName, string>();

            _logger.LogError("SHOULD STILL WORK");

            foreach (var kvp in analyzeResult.KeyValuePairs){
                _logger.LogError($"ATTEMPT 111111111111111111111 {kvp.Value.Content}");
                var sectionName = SectionHandler.GetSectionName(kvp.Key.Content, ignoredSections);
                _logger.LogError($"wooooooooooow {sectionName}");
                
                if (sectionName == SectionName.NotFound) continue;
                _logger.LogError($"IM HERE NOW");
                ignoredSections.Add(sectionName);

                var section = SectionHandler.GetRule(sectionName);
                _logger.LogError($"RULE {section.Name}");

                if (section.IsPrefered){
                    ignoredSections.AddRange(section.ConnectedSections);
                }

                dictionary.Add(sectionName, kvp.Value.Content);
            }

            foreach (var paragraph in analyzeResult.Paragraphs){
                var sectionName = SectionHandler.GetSectionName(paragraph.Content, ignoredSections);
                
                if (sectionName == SectionName.NotFound) continue;

                ignoredSections.Add(sectionName);

                var section = SectionHandler.GetRule(sectionName);

                if (section.IsPrefered){
                    ignoredSections.AddRange(section.ConnectedSections);
                }

                dictionary.Add(sectionName, paragraph.Content);
            }

            StringBuilder result = new StringBuilder();

            foreach (var kvp in dictionary)
            {
                result.AppendLine($"Section: {kvp.Key}, Value: {kvp.Value}");
            }

            _logger.LogError(result.ToString());

            return returnObject;
        }
    }

    
}

