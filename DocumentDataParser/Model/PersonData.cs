

using Microsoft.AspNetCore.Mvc;

namespace DocumentDataParser.Model{
    [Serializable]
    public class PersonData{
        

        // Personal Info
        
        public List<DataObject> Names {get;set;}

        public List<DataObject> Surnames {get;set;}

        public DataObject PESEL {get;set;}
      
        public DataObject ID_Type {get;set;}

        public DataObject ID_Number {get;set;}

        public DataObject NIP {get;set;}

        public DataObject Regon {get;set;}

        public AddressObject Address {get;set;}

        public PersonData (  ){

                Names =  new List<DataObject>(); 
                Surnames =  new List<DataObject>(); 
                PESEL = new DataObject();
                ID_Type = new DataObject();
                ID_Number = new DataObject();
                NIP = new DataObject();
                Regon = new DataObject();
                Address = new AddressObject();
        }
    }
}