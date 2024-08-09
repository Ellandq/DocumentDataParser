
namespace DocumentDataParser.Model{
    public class DataObject{

        public string Value {get;set;}


        public float Certainty {get;set;}


        public DataObject()
        {
        }

          public DataObject(string value, float certainty)
        {
            Value = value;
            Certainty = certainty;
        }

        
    }
}