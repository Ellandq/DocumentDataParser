namespace DocumentDataParser.Enums{
    public enum SectionName{
        
        // Buyer Data
        BuyerNames, BuyerSurnames, BuyerNamesAndSurnames, 
        // Address
        BuyerStreet, BuyerHomeAndApartamentNumber, BuyerPostalCode, BuyerAddress, 

        BuyerPESEL, BuyerNIP,

        // Seller Data
        SellerNames, SellerSurnames, SellerNamesAndSurnames, 
        // Address
        SellerStreet, SellerHomeAndApartamentNumber, SellerPostalCode, SellerAddress, 

        SellerPESEL, SellerNIP,

        // Edge cases
        NotFound,
    }
}