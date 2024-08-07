

using Microsoft.AspNetCore.Mvc;

namespace DocumentDataParser.Model{
    public class PersonData{

        // Personal Info
        
        public List<DataObject> Names;
        public List<DataObject> Surnames;
        public DataObject PESEL;
        public DataObject ID_Type;
        public DataObject ID_Number;
        public DataObject NIP;

        // Address

        public AddressObject Address;

        
    }
}