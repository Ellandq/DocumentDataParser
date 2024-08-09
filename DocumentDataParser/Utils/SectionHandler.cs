using DocumentDataParser.Enums;
using Azure.AI.DocumentIntelligence;
using DocumentDataParser.Model;
using System.Text.RegularExpressions;

namespace DocumentDataParser.Utils{

    public static class SectionHandler
    {
        public static readonly List<SectionRule> SectionRules = [
            new SectionRule(
                SectionName.BuyerNames,
                [
                    @"imi[eę]|imiona"
                ],
                [
                    SectionName.BuyerNamesAndSurnames
                ]
            ),
            new SectionRule(
                SectionName.BuyerSurnames,
                [
                    @"nazwisk[oa]",
                ],
                [
                    SectionName.BuyerNamesAndSurnames
                ]
            ),
            new SectionRule(
                SectionName.BuyerNamesAndSurnames,
                [
                    @"imi[eę]|imiona", @"nazwisk[oa]",
                ],
                [
                    SectionName.BuyerNames, SectionName.BuyerSurnames
                ],
                true
            ),
            new SectionRule(
                SectionName.NotFound,
                [],
                []
            ),
            
        ];



        public static SectionName GetSectionName(string content, List<SectionName> ignoredSections){
            foreach (var section in SectionRules){
                var match = GetMatch(section, content);
                if (match.Name == SectionName.NotFound || ignoredSections.Contains(match.Name)) continue;
                return match.Name;
            }
            return SectionName.NotFound;
        }

        public static SectionRule GetMatch(SectionRule section, string content){
            if (section.KeyWords.All(keyWord => Regex.IsMatch(content, keyWord, RegexOptions.IgnoreCase))){
                if (section.IsPrefered || section.ConnectedSections.Count == 0) return section;
                else{
                    foreach (var connectedSectionName in section.ConnectedSections)
                    {
                        var connectedSection = GetRule(connectedSectionName);
                        if (connectedSection.IsPrefered)
                        {
                            var result = GetMatch(connectedSection, content);
                            if (result.Name != SectionName.NotFound)
                            {
                                return result;
                            }
                        }
                    }
                }
            }

            return GetRule(SectionName.NotFound);
        }

        public static SectionRule GetRule(SectionName name){
            return SectionRules.Find(section => section.Name == name)
            ?? GetRule(SectionName.NotFound);
        }
    } 


}