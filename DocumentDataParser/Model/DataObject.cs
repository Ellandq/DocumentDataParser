
using DocumentDataParser.Enums;

namespace DocumentDataParser.Model{
    [Serializable]
    public class DataObject{
      public SectionName Section {get; set;}
      public string Value {get; set;}

      public float Certainty{get; set;}
      
      public DataObject()
        {
            Section = new SectionName();
        }

          public DataObject(string value, float certainty)
        {
            Value = value;
            Certainty = certainty;
        }
    }
}