using DocumentDataParser.Enums;
using Azure.AI.DocumentIntelligence;
using DocumentDataParser.Model;
using System.Text.RegularExpressions;

namespace DocumentDataParser.Utils{

    public static class SectionHandler
    {
        public static readonly List<SectionRule> SectionRules = new List<SectionRule>
        {
            // BUYER
            new SectionRule(
                SectionName.BuyerNames,
                new List<string> { @"imi[eę]|imiona" },
                new List<SectionName> { SectionName.BuyerNamesAndSurnames }
            ),
            new SectionRule(
                SectionName.BuyerSurnames,
                new List<string> { @"nazwisk[oa]" },
                new List<SectionName> { SectionName.BuyerNamesAndSurnames }
            ),
            new SectionRule(
                SectionName.BuyerNamesAndSurnames,
                new List<string> { @"imi[eę]|imiona", @"nazwisk[oa]" },
                new List<SectionName> { SectionName.BuyerNames, SectionName.BuyerSurnames },
                true
            ),
            new SectionRule(
                SectionName.BuyerStreet,
                new List<string> { @"ulica|ul" },
                new List<SectionName> { SectionName.BuyerAddress }
            ),
            new SectionRule(
                SectionName.BuyerHomeAndApartamentNumber,
                new List<string> { @"numer( mieszkania)?.*|nr( mieszkania)?.*" },
                new List<SectionName> { SectionName.BuyerAddress }
            ),
            new SectionRule(
                SectionName.BuyerPostalCode,
                new List<string> { @"pocztowy|poczta|kod, poczta|kod pocztowy" },
                new List<SectionName> { SectionName.BuyerAddress }
            ),
            new SectionRule(
                SectionName.BuyerAddress,
                new List<string> { @"ulica", @"(numer( mieszkania)?.*|nr( mieszkania)?.*)?", @"pocztowy|poczta|kod, poczta|kod pocztowy" },
                new List<SectionName> { SectionName.BuyerStreet, SectionName.BuyerHomeAndApartamentNumber, SectionName.BuyerPostalCode },
                true
            ),
            new SectionRule(
                SectionName.BuyerPESEL,
                new List<string> { @"PESEL" },
                new List<SectionName>()
            ),
            new SectionRule(
                SectionName.BuyerNIP,
                new List<string> { @"NIP" },
                new List<SectionName>()
            ),

            // SELLER

            new SectionRule(
                SectionName.SellerNames,
                new List<string> { @"imi[eę]|imiona" },
                new List<SectionName> { SectionName.SellerNamesAndSurnames }
            ),
            new SectionRule(
                SectionName.SellerSurnames,
                new List<string> { @"nazwisk[oa]" },
                new List<SectionName> { SectionName.SellerNamesAndSurnames }
            ),
            new SectionRule(
                SectionName.SellerNamesAndSurnames,
                new List<string> { @"imi[eę]|imiona", @"nazwisk[oa]" },
                new List<SectionName> { SectionName.SellerNames, SectionName.SellerSurnames },
                true
            ),
            new SectionRule(
                SectionName.SellerStreet,
                new List<string> { @"ulica|ul" },
                new List<SectionName> { SectionName.SellerAddress }
            ),
            new SectionRule(
                SectionName.SellerHomeAndApartamentNumber,
                new List<string> { @"numer( mieszkania)?.*|nr( mieszkania)?.*" },
                new List<SectionName> { SectionName.SellerAddress }
            ),
            new SectionRule(
                SectionName.SellerPostalCode,
                new List<string> { @"pocztowy|poczta|kod, poczta|kod pocztowy" },
                new List<SectionName> { SectionName.SellerAddress }
            ),
            new SectionRule(
                SectionName.SellerAddress,
                new List<string> { @"ulica", @"(numer( mieszkania)?.*|nr( mieszkania)?.*)?", @"pocztowy|poczta|kod, poczta|kod pocztowy" },
                new List<SectionName> { SectionName.SellerStreet, SectionName.SellerHomeAndApartamentNumber, SectionName.SellerPostalCode },
                true
            ),
            new SectionRule(
                SectionName.SellerPESEL,
                new List<string> { @"PESEL" },
                new List<SectionName>()
            ),
            new SectionRule(
                SectionName.SellerNIP,
                new List<string> { @"NIP" },
                new List<SectionName>()
            ),
            new SectionRule(
                SectionName.NotFound,
                new List<string>(),
                new List<SectionName>()
            )
        };

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

        public static string TrimSection(string section, SectionName name){
            var rules = GetRule(name);
            var result = section;

            foreach(var keyWord in rules.KeyWords){
                var regex = new Regex(keyWord, RegexOptions.IgnoreCase);
                result = regex.Replace(result, "");
            }

            return result.Trim();
        }

    } 

    

}