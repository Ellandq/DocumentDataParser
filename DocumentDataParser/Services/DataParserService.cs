using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Azure;
using Azure.AI.DocumentIntelligence;
using DocumentDataParser.Model;
using Microsoft.Extensions.Logging;

namespace DocumentDataParser.Services
{
    public class DataParserService : IDataParser
    {
        private readonly DocumentIntelligenceClient _documentIntelligenceClient;
        private readonly ILogger<DataParserService> _logger;
        private readonly IDataExtraction _dataExtraction;


        private const string ModelID = "prebuilt-layout";
        private const string Features = "?features=keyValuePairs";

        public DataParserService(DocumentIntelligenceClient documentIntelligenceClient, 
            IDataExtraction dataExtractionService, ILogger<DataParserService> logger)
        {
            _documentIntelligenceClient = documentIntelligenceClient;
            _dataExtraction = dataExtractionService;
            _logger = logger;
        }

        public async Task<ReturnObject> ParseDataAsync(AnalyzeDocumentContent content)
        { 
            Operation<AnalyzeResult> operation;
            try {
                operation = await _documentIntelligenceClient.AnalyzeDocumentAsync(
                    WaitUntil.Completed, 
                    ModelID, 
                    content
                );
            } catch (Exception e){
                _logger.LogError($"There was an issue while connecting to DocumentIntelligence: {e.Message}");
                throw new Exception($"There was an issue while connecting to DocumentIntelligence: {e.Message}");
            }

            var responseData = operation.Value;

            if (responseData == null)
            {
                _logger.LogError("AnalyzeDocumentAsync returned null result.");
                throw new Exception("AnalyzeDocumentAsync returned null result.");
            }

            ReturnObject returnObject = new();

            try {
                return await _dataExtraction.ExtractDataToObject(returnObject, responseData);
            }catch(Exception e){
                _logger.LogError($"There was an issue while parsing to json: {e.Message}");
                throw new Exception($"There was an issue while parsing to json: {e.Message}");
            }


        }
    }
}


                // Operation<AnalyzeResult> operation = await _documentIntelligenceClient.AnalyzeDocumentAsync(
                //     WaitUntil.Completed, 
                //     "prebuilt-read", 
                //     content
                // );
