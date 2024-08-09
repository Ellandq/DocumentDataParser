
using DocumentDataParser.Enums;

namespace DocumentDataParser.Model{
    public class DataObject{
      public SectionName Section;
      public string Value;

      public float Certainty;
      
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