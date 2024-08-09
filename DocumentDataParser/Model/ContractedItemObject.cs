
using System.Net.Http.Headers;

namespace DocumentDataParser.Model{
    [Serializable]
    public class ContractedItemObject{
        public DataObject Brand {get;set;}

        public DataObject Model {get;set;}

        public DataObject YearOfProduction {get;set;}

        public DataObject EngineNumber {get;set;}

        public DataObject BodyNumber {get;set;}

        public DataObject RegistrationNumber {get;set;}

        public DataObject Milage {get;set;}

        public ContractedItemObject(){
            Brand = new DataObject();
            Model = new DataObject();
            YearOfProduction = new DataObject();
            BodyNumber = new DataObject();
            RegistrationNumber = new DataObject();
            Milage = new DataObject();
        }
        



        
    }
}