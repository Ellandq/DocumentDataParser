
namespace DocumentDataParser.Model
{
    public class ReturnObject{

        public PersonData Buyer {get;set;}

        public PersonData Seller {get;set;}

        public ContractedItemObject ContractedItem {get;set;}

        public DataObject ObjectValue {get;set;}

        public ReturnObject(){
            Buyer = new PersonData();
            Seller = new PersonData();
            ContractedItem = new ContractedItemObject();
            ObjectValue = new DataObject();


        }
        

    }
}