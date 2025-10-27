using System.Collections.Generic;

namespace KBA.CoreUtilities.Utilities
{
    /// <summary>
    /// EMV Merchant Category Codes and Currency Codes
    /// </summary>
    public static class EmvCodes
    {
        /// <summary>
        /// Merchant Category Codes (MCC) - ISO 18245
        /// </summary>
        public static Dictionary<string, string> MCC = new Dictionary<string, string>
        {
            // Retail
            ["5411"] = "Grocery Stores",
            ["5812"] = "Restaurants",
            ["5814"] = "Fast Food",
            ["5912"] = "Pharmacies",
            ["5999"] = "Other Services",
            
            // Transportation
            ["4121"] = "Taxis",
            ["4511"] = "Airlines",
            ["4789"] = "Transport Services",
            
            // Financial
            ["6011"] = "ATM/Cash Disbursement",
            ["6012"] = "Financial Merchants",
            ["6051"] = "Money Transfer",
            
            // Professional
            ["7011"] = "Hotels",
            ["8011"] = "Doctors",
            ["8062"] = "Hospitals"
        };

        /// <summary>
        /// Currency Codes - ISO 4217
        /// </summary>
        public static Dictionary<string, string> Currencies = new Dictionary<string, string>
        {
            // Africa
            ["952"] = "XOF - West African CFA",
            ["950"] = "XAF - Central African CFA",
            
            // Major World Currencies
            ["840"] = "USD - US Dollar",
            ["978"] = "EUR - Euro",
            ["826"] = "GBP - British Pound",
            ["392"] = "JPY - Japanese Yen",
            ["156"] = "CNY - Chinese Yuan"
        };
    }
}
