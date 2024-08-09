using System.Runtime.Serialization;
using System.Security.Cryptography.Xml;

namespace DocumentDataParser{
    [Serializable]
    
    public class AddressObject{
        public DataObject Street {get;set;}
        public DataObject City {get;set;}

        public DataObject BuildingNumber {get;set;}

        public DataObject PostalCode {get;set;}

        public DataObject ApartmentNumber {get;set;}

        
      
        public AddressObject(){
            Street =new DataObject();
            City = new DataObject();
            BuildingNumber = new DataObject();
            PostalCode = new DataObject();
            ApartmentNumber = new DataObject();
        }

       

        
    }
}