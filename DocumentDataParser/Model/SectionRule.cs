using System.ComponentModel.Design;
using DocumentDataParser.Enums;

namespace DocumentDataParser.Model{
    [Serializable]

    public class SectionRule{

        public SectionName Name { get; }

        public List<string> KeyWords { get; }

        public List<SectionName> ConnectedSections { get; }
 
        public bool IsPrefered { get; }

        public SectionRule(SectionName name, List<string> keyWords, List<SectionName> connectedSections, bool isPrefered = false){
            Name = name;
            KeyWords = keyWords;
            ConnectedSections = connectedSections;
            IsPrefered = isPrefered;
        }
    }

}