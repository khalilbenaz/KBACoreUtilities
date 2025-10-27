using System.Text;
using System.Text.RegularExpressions;

namespace KBA.CoreUtilities.Utilities
{
    public static class PhoneUtils
    {
        private static readonly Dictionary<string, PhonePattern> _phonePatterns = new()
        {
            // UEMOA - Format: 8 chiffres
            { "BF", new PhonePattern("BF", "Burkina Faso", "226", @"^(\+226)?(0)?([2-7]\d{7})$", 8, "XX XX XX XX") },
            { "BJ", new PhonePattern("BJ", "Bénin", "229", @"^(\+229)?(0)?([1-9]\d{7})$", 8, "XX XX XX XX") },
            { "CI", new PhonePattern("CI", "Côte d'Ivoire", "225", @"^(\+225)?(0)?([0-9]{2})([0-9]{2})([0-9]{2})([0-9]{2})$", 8, "XX XX XX XX") },
            { "GN", new PhonePattern("GN", "Guinée", "224", @"^(\+224)?(0)?([6-7]\d{7})$", 8, "XX XX XX XX") },
            { "ML", new PhonePattern("ML", "Mali", "223", @"^(\+223)?(0)?([2-9]\d{7})$", 8, "XX XX XX XX") },
            { "NE", new PhonePattern("NE", "Niger", "227", @"^(\+227)?(0)?([2-9]\d{7})$", 8, "XX XX XX XX") },
            { "SN", new PhonePattern("SN", "Sénégal", "221", @"^(\+221)?(0)?([3-9]\d{8})$", 9, "XX XXX XX XX") },
            { "TG", new PhonePattern("TG", "Togo", "228", @"^(\+228)?(0)?([2-9]\d{7})$", 8, "XX XX XX XX") },
            
            // CEMAC
            { "CM", new PhonePattern("CM", "Cameroun", "237", @"^(\+237)?(0)?([2-9]\d{8})$", 9, "X XX XX XX XX") },
            { "CF", new PhonePattern("CF", "République Centrafricaine", "236", @"^(\+236)?(0)?([2-9]\d{7})$", 8, "XX XX XX XX") },
            { "TD", new PhonePattern("TD", "Tchad", "235", @"^(\+235)?(0)?([2-9]\d{7})$", 8, "XX XX XX XX") },
            { "CG", new PhonePattern("CG", "Congo-Brazzaville", "242", @"^(\+242)?(0)?([2-9]\d{8})$", 9, "XX XX XX XX") },
            { "GA", new PhonePattern("GA", "Gabon", "241", @"^(\+241)?(0)?([2-9]\d{7})$", 8, "XX XX XX XX") },
            { "GQ", new PhonePattern("GQ", "Guinée Équatoriale", "240", @"^(\+240)?(0)?([2-9]\d{8})$", 9, "XX XX XX XX") },
            
            // Maghreb
            { "MA", new PhonePattern("MA", "Maroc", "212", @"^(\+212)?(0)?([5-9]\d{8})$", 9, "X XX XX XX XX") },
            { "DZ", new PhonePattern("DZ", "Algérie", "213", @"^(\+213)?(0)?([5-9]\d{8})$", 9, "X XX XX XX XX") },
            { "TN", new PhonePattern("TN", "Tunisie", "216", @"^(\+216)?(0)?([2-9]\d{7})$", 8, "XX XX XX XX") },
            
            // Afrique de l'Ouest (non-UEMOA)
            { "NG", new PhonePattern("NG", "Nigeria", "234", @"^(\+234)?(0)?([7-9]\d{9})$", 10, "XXX XXX XXXX") },
            { "GH", new PhonePattern("GH", "Ghana", "233", @"^(\+233)?(0)?([2-9]\d{8})$", 9, "XX XXX XXXX") },
            { "LR", new PhonePattern("LR", "Libéria", "231", @"^(\+231)?(0)?([2-9]\d{7})$", 8, "XX XX XX XX") },
            { "SL", new PhonePattern("SL", "Sierra Leone", "232", @"^(\+232)?(0)?([2-9]\d{7})$", 8, "XX XX XX XX") },
            { "GM", new PhonePattern("GM", "Gambie", "220", @"^(\+220)?(0)?([2-9]\d{6})$", 7, "XXX XXXX") },
            { "GW", new PhonePattern("GW", "Guinée-Bissau", "245", @"^(\+245)?(0)?([2-9]\d{7})$", 8, "XX XX XX XX") },
            { "CV", new PhonePattern("CV", "Cap-Vert", "238", @"^(\+238)?(0)?([2-9]\d{7})$", 8, "XX XX XX XX") },
            { "ST", new PhonePattern("ST", "Sao Tomé-et-Principe", "239", @"^(\+239)?(0)?([2-9]\d{6})$", 7, "XXX XXXX") },
            
            // Afrique de l'Est
            { "KE", new PhonePattern("KE", "Kenya", "254", @"^(\+254)?(0)?([1-9]\d{8})$", 9, "XXX XXX XXX") },
            { "UG", new PhonePattern("UG", "Ouganda", "256", @"^(\+256)?(0)?([3-9]\d{8})$", 9, "XXX XXX XXX") },
            { "TZ", new PhonePattern("TZ", "Tanzanie", "255", @"^(\+255)?(0)?([1-9]\d{8})$", 9, "XXX XXX XXX") },
            { "RW", new PhonePattern("RW", "Rwanda", "250", @"^(\+250)?(0)?([2-9]\d{8})$", 9, "XXX XXX XXX") },
            { "BI", new PhonePattern("BI", "Burundi", "257", @"^(\+257)?(0)?([2-9]\d{7})$", 8, "XX XX XX XX") },
            { "ET", new PhonePattern("ET", "Éthiopie", "251", @"^(\+251)?(0)?([1-9]\d{8})$", 9, "XX XXX XXXX") },
            { "SO", new PhonePattern("SO", "Somalie", "252", @"^(\+252)?(0)?([1-9]\d{8})$", 9, "XXX XXX XXX") },
            { "DJ", new PhonePattern("DJ", "Djibouti", "253", @"^(\+253)?(0)?([2-9]\d{7})$", 8, "XX XX XX XX") },
            { "ER", new PhonePattern("ER", "Érythrée", "291", @"^(\+291)?(0)?([1-9]\d{6})$", 7, "X XXX XXX") },
            
            // Afrique Australe
            { "ZA", new PhonePattern("ZA", "Afrique du Sud", "27", @"^(\+27)?(0)?([1-9]\d{8})$", 9, "XX XXX XXXX") },
            { "ZW", new PhonePattern("ZW", "Zimbabwe", "263", @"^(\+263)?(0)?([1-9]\d{8})$", 9, "XX XXX XXXX") },
            { "ZM", new PhonePattern("ZM", "Zambie", "260", @"^(\+260)?(0)?([1-9]\d{8})$", 9, "XX XXX XXXX") },
            { "MW", new PhonePattern("MW", "Malawi", "265", @"^(\+265)?(0)?([1-9]\d{7})$", 8, "XX XXX XXX") },
            { "BW", new PhonePattern("BW", "Botswana", "267", @"^(\+267)?(0)?([2-9]\d{7})$", 8, "XX XXX XXX") },
            { "NA", new PhonePattern("NA", "Namibie", "264", @"^(\+264)?(0)?([6-9]\d{7})$", 8, "XX XXX XXX") },
            { "SZ", new PhonePattern("SZ", "Eswatini", "268", @"^(\+268)?(0)?([2-9]\d{7})$", 8, "XX XXX XXX") },
            { "LS", new PhonePattern("LS", "Lesotho", "266", @"^(\+266)?(0)?([2-9]\d{7})$", 8, "XX XXX XXX") },
            
            // Afrique Centrale
            { "CD", new PhonePattern("CD", "République Démocratique du Congo", "243", @"^(\+243)?(0)?([1-9]\d{8})$", 9, "XX XXX XXXX") },
            { "AO", new PhonePattern("AO", "Angola", "244", @"^(\+244)?(0)?([2-9]\d{8})$", 9, "XX XXX XXXX") },
            { "MZ", new PhonePattern("MZ", "Mozambique", "258", @"^(\+258)?(0)?([2-9]\d{8})$", 9, "XX XXX XXXX") },
            
            // Océan Indien
            { "MU", new PhonePattern("MU", "Maurice", "230", @"^(\+230)?(0)?([2-9]\d{7})$", 8, "XX XX XX XX") },
            { "SC", new PhonePattern("SC", "Seychelles", "248", @"^(\+248)?(0)?([2-9]\d{6})$", 7, "X XX XX XX") },
            { "KM", new PhonePattern("KM", "Comores", "269", @"^(\+269)?(0)?([2-9]\d{6})$", 7, "XXX XXXX") },
            { "MG", new PhonePattern("MG", "Madagascar", "261", @"^(\+261)?(0)?([2-9]\d{8})$", 9, "XX XXX XXXX") },
            
            // Afrique du Nord
            { "EG", new PhonePattern("EG", "Égypte", "20", @"^(\+20)?(0)?([1-9]\d{9})$", 10, "XX XXXX XXXX") },
            { "LY", new PhonePattern("LY", "Libye", "218", @"^(\+218)?(0)?([1-9]\d{8})$", 9, "XX XXX XXXX") },
            { "SD", new PhonePattern("SD", "Soudan", "249", @"^(\+249)?(0)?([1-9]\d{8})$", 9, "XX XXX XXXX") },
            { "SS", new PhonePattern("SS", "Soudan du Sud", "211", @"^(\+211)?(0)?([1-9]\d{8})$", 9, "XX XXX XXXX") },
            
            // Europe complète
            { "FR", new PhonePattern("FR", "France", "33", @"^(\+33)?(0)?([1-9]\d{8})$", 9, "X XX XX XX XX") },
            { "BE", new PhonePattern("BE", "Belgique", "32", @"^(\+32)?(0)?([1-9]\d{8})$", 9, "XXX XX XX XX") },
            { "CH", new PhonePattern("CH", "Suisse", "41", @"^(\+41)?(0)?([2-9]\d{8})$", 9, "XXX XX XX XX") },
            { "DE", new PhonePattern("DE", "Allemagne", "49", @"^(\+49)?(0)?([1-9]\d{10})$", 11, "XXX XXXXXXX") },
            { "IT", new PhonePattern("IT", "Italie", "39", @"^(\+39)?(0)?([2-9]\d{8,9})$", 9, "XXX XXX XXXX") },
            { "ES", new PhonePattern("ES", "Espagne", "34", @"^(\+34)?(0)?([6-9]\d{8})$", 9, "XXX XXX XXX") },
            { "PT", new PhonePattern("PT", "Portugal", "351", @"^(\+351)?(0)?([2-9]\d{8})$", 9, "XXX XXX XXX") },
            { "NL", new PhonePattern("NL", "Pays-Bas", "31", @"^(\+31)?(0)?([1-9]\d{8})$", 9, "XX XXX XXXX") },
            { "LU", new PhonePattern("LU", "Luxembourg", "352", @"^(\+352)?(0)?([2-9]\d{8})$", 9, "XXX XXX XXX") },
            { "GB", new PhonePattern("GB", "Royaume-Uni", "44", @"^(\+44)?(0)?([1-9]\d{9,10})$", 10, "XXXX XXXXXX") },
            { "IE", new PhonePattern("IE", "Irlande", "353", @"^(\+353)?(0)?([1-9]\d{7,8})$", 9, "XX XXX XXXX") },
            { "AT", new PhonePattern("AT", "Autriche", "43", @"^(\+43)?(0)?([1-9]\d{10})$", 11, "XXX XXXXXXX") },
            { "SE", new PhonePattern("SE", "Suède", "46", @"^(\+46)?(0)?([1-9]\d{8,9})$", 9, "XX XXX XXXX") },
            { "NO", new PhonePattern("NO", "Norvège", "47", @"^(\+47)?(0)?([2-9]\d{7})$", 8, "XX XX XX XX") },
            { "DK", new PhonePattern("DK", "Danemark", "45", @"^(\+45)?(0)?([2-9]\d{7})$", 8, "XX XX XX XX") },
            { "FI", new PhonePattern("FI", "Finlande", "358", @"^(\+358)?(0)?([1-9]\d{8,9})$", 9, "XX XXX XXXX") },
            { "PL", new PhonePattern("PL", "Pologne", "48", @"^(\+48)?(0)?([1-9]\d{8})$", 9, "XXX XXX XXX") },
            { "CZ", new PhonePattern("CZ", "République Tchèque", "420", @"^(\+420)?(0)?([1-9]\d{8})$", 9, "XXX XXX XXX") },
            { "SK", new PhonePattern("SK", "Slovaquie", "421", @"^(\+421)?(0)?([2-9]\d{8})$", 9, "XXX XXX XXX") },
            { "HU", new PhonePattern("HU", "Hongrie", "36", @"^(\+36)?(0)?([1-9]\d{8})$", 9, "XX XXX XXXX") },
            { "RO", new PhonePattern("RO", "Roumanie", "40", @"^(\+40)?(0)?([2-9]\d{8})$", 9, "XXX XXX XXX") },
            { "BG", new PhonePattern("BG", "Bulgarie", "359", @"^(\+359)?(0)?([2-9]\d{7})$", 8, "XX XX XX XX") },
            { "HR", new PhonePattern("HR", "Croatie", "385", @"^(\+385)?(0)?([1-9]\d{8})$", 9, "XX XXX XXXX") },
            { "SI", new PhonePattern("SI", "Slovénie", "386", @"^(\+386)?(0)?([1-9]\d{7})$", 8, "XX XXX XXX") },
            { "EE", new PhonePattern("EE", "Estonie", "372", @"^(\+372)?(0)?([5-8]\d{7})$", 8, "XX XXX XXX") },
            { "LV", new PhonePattern("LV", "Lettonie", "371", @"^(\+371)?(0)?([2-9]\d{7})$", 8, "XX XX XX XX") },
            { "LT", new PhonePattern("LT", "Lituanie", "370", @"^(\+370)?(0)?([6-8]\d{7})$", 8, "XX XXX XXX") },
            { "GR", new PhonePattern("GR", "Grèce", "30", @"^(\+30)?(0)?([2-9]\d{9})$", 10, "XXX XXX XXXX") },
            { "CY", new PhonePattern("CY", "Chypre", "357", @"^(\+357)?(0)?([9-9]\d{7})$", 8, "XX XX XX XX") },
            { "MT", new PhonePattern("MT", "Malte", "356", @"^(\+356)?(0)?([2-9]\d{7})$", 8, "XX XX XX XX") },
            { "IS", new PhonePattern("IS", "Islande", "354", @"^(\+354)?(0)?([1-9]\d{6})$", 7, "XXX XXXX") },
            { "AL", new PhonePattern("AL", "Albanie", "355", @"^(\+355)?(0)?([4-9]\d{7})$", 8, "XX XX XX XX") },
            { "MK", new PhonePattern("MK", "Macédoine du Nord", "389", @"^(\+389)?(0)?([2-9]\d{7})$", 8, "XX XX XX XX") },
            { "ME", new PhonePattern("ME", "Monténégro", "382", @"^(\+382)?(0)?([6-9]\d{7})$", 8, "XX XX XX XX") },
            { "RS", new PhonePattern("RS", "Serbie", "381", @"^(\+381)?(0)?([6-9]\d{7,8})$", 9, "XX XXX XXXX") },
            { "BA", new PhonePattern("BA", "Bosnie-Herzégovine", "387", @"^(\+387)?(0)?([3-9]\d{7})$", 8, "XX XX XX XX") },
            { "AD", new PhonePattern("AD", "Andorre", "376", @"^(\+376)?(0)?([3-9]\d{5})$", 6, "XXX XXX") },
            { "MC", new PhonePattern("MC", "Monaco", "377", @"^(\+377)?(0)?([4-9]\d{7})$", 8, "XX XX XX XX") },
            { "LI", new PhonePattern("LI", "Liechtenstein", "423", @"^(\+423)?(0)?([2-9]\d{6})$", 7, "XXX XXXX") },
            { "VA", new PhonePattern("VA", "Vatican", "379", @"^(\+379)?(0)?([6-9]\d{7})$", 8, "XX XX XX XX") },
            { "SM", new PhonePattern("SM", "Saint-Marin", "378", @"^(\+378)?(0)?([5-9]\d{7})$", 8, "XX XX XX XX") },
            
            // Amérique complète
            { "US", new PhonePattern("US", "États-Unis", "1", @"^(\+1)?(0)?([2-9]\d{9})$", 10, "(XXX) XXX-XXXX") },
            { "CA", new PhonePattern("CA", "Canada", "1", @"^(\+1)?(0)?([2-9]\d{9})$", 10, "(XXX) XXX-XXXX") },
            { "MX", new PhonePattern("MX", "Mexique", "52", @"^(\+52)?(0)?([1-9]\d{9})$", 10, "XXX XXX XXXX") },
            { "BR", new PhonePattern("BR", "Brésil", "55", @"^(\+55)?(0)?([1-9]\d{10})$", 11, "(XX) XXXXX-XXXX") },
            { "AR", new PhonePattern("AR", "Argentine", "54", @"^(\+54)?(0)?([1-9]\d{9})$", 10, "XXX XXX-XXXX") },
            { "CL", new PhonePattern("CL", "Chili", "56", @"^(\+56)?(0)?([2-9]\d{8})$", 9, "XXX XXX XXX") },
            { "CO", new PhonePattern("CO", "Colombie", "57", @"^(\+57)?(0)?([1-9]\d{9})$", 10, "XXX XXX XXXX") },
            { "PE", new PhonePattern("PE", "Pérou", "51", @"^(\+51)?(0)?([1-9]\d{8})$", 9, "XXX XXX XXX") },
            { "VE", new PhonePattern("VE", "Venezuela", "58", @"^(\+58)?(0)?([2-9]\d{9})$", 10, "XXX XXX XXXX") },
            { "EC", new PhonePattern("EC", "Équateur", "593", @"^(\+593)?(0)?([2-9]\d{8})$", 9, "XXX XXX XXX") },
            { "BO", new PhonePattern("BO", "Bolivie", "591", @"^(\+591)?(0)?([2-9]\d{7})$", 8, "XX XXX XXX") },
            { "PY", new PhonePattern("PY", "Paraguay", "595", @"^(\+595)?(0)?([2-9]\d{8})$", 9, "XXX XXX XXX") },
            { "UY", new PhonePattern("UY", "Uruguay", "598", @"^(\+598)?(0)?([2-9]\d{7})$", 8, "XX XXX XXX") },
            { "GY", new PhonePattern("GY", "Guyana", "592", @"^(\+592)?(0)?([2-9]\d{6})$", 7, "XXX XXXX") },
            { "SR", new PhonePattern("SR", "Suriname", "597", @"^(\+597)?(0)?([2-9]\d{6})$", 7, "XXX XXXX") },
            { "GF", new PhonePattern("GF", "Guyane Française", "594", @"^(\+594)?(0)?([5-9]\d{8})$", 9, "XXX XX XX XX") },
            { "CR", new PhonePattern("CR", "Costa Rica", "506", @"^(\+506)?(0)?([2-9]\d{7})$", 8, "XX XX XX XX") },
            { "PA", new PhonePattern("PA", "Panama", "507", @"^(\+507)?(0)?([2-9]\d{7})$", 8, "XXX XXXX") },
            { "GT", new PhonePattern("GT", "Guatemala", "502", @"^(\+502)?(0)?([2-9]\d{7})$", 8, "XXX XXXXX") },
            { "SV", new PhonePattern("SV", "El Salvador", "503", @"^(\+503)?(0)?([2-9]\d{7})$", 8, "XX XX XX XX") },
            { "HN", new PhonePattern("HN", "Honduras", "504", @"^(\+504)?(0)?([2-9]\d{7})$", 8, "XX XX XX XX") },
            { "NI", new PhonePattern("NI", "Nicaragua", "505", @"^(\+505)?(0)?([2-9]\d{7})$", 8, "XX XX XX XX") },
            { "CU", new PhonePattern("CU", "Cuba", "53", @"^(\+53)?(0)?([2-9]\d{7})$", 8, "XX XX XX XX") },
            { "JM", new PhonePattern("JM", "Jamaïque", "1", @"^(\+1)?(0)?([8-9]\d{9})$", 10, "(XXX) XXX-XXXX") },
            { "HT", new PhonePattern("HT", "Haïti", "509", @"^(\+509)?(0)?([2-9]\d{7})$", 8, "XX XX XX XX") },
            { "DO", new PhonePattern("DO", "République Dominicaine", "1", @"^(\+1)?(0)?([8-9]\d{9})$", 10, "(XXX) XXX-XXXX") },
            { "BB", new PhonePattern("BB", "Barbade", "1", @"^(\+1)?(0)?([2-9]\d{9})$", 10, "(XXX) XXX-XXXX") },
            { "TT", new PhonePattern("TT", "Trinité-et-Tobago", "1", @"^(\+1)?(0)?([2-9]\d{9})$", 10, "(XXX) XXX-XXXX") },
            { "BS", new PhonePattern("BS", "Bahamas", "1", @"^(\+1)?(0)?([2-9]\d{9})$", 10, "(XXX) XXX-XXXX") },
            { "BZ", new PhonePattern("BZ", "Belize", "501", @"^(\+501)?(0)?([2-9]\d{6})$", 7, "XXX XXXX") },
            { "GL", new PhonePattern("GL", "Groenland", "299", @"^(\+299)?(0)?([2-9]\d{5})$", 6, "XX XX XX") },
            
            // Asie complète
            { "CN", new PhonePattern("CN", "Chine", "86", @"^(\+86)?(0)?([1-9]\d{10})$", 11, "XXX XXXX XXXX") },
            { "JP", new PhonePattern("JP", "Japon", "81", @"^(\+81)?(0)?([1-9]\d{9})$", 10, "XX XXXX XXXX") },
            { "IN", new PhonePattern("IN", "Inde", "91", @"^(\+91)?(0)?([1-9]\d{9})$", 10, "XXXXX XXXXX") },
            { "KR", new PhonePattern("KR", "Corée du Sud", "82", @"^(\+82)?(0)?([1-9]\d{8,9})$", 10, "XX XXXX XXXX") },
            { "KP", new PhonePattern("KP", "Corée du Nord", "850", @"^(\+850)?(0)?([1-9]\d{8})$", 9, "XX XXX XXXX") },
            { "TH", new PhonePattern("TH", "Thaïlande", "66", @"^(\+66)?(0)?([1-9]\d{8})$", 9, "X XXXX XXXX") },
            { "VN", new PhonePattern("VN", "Vietnam", "84", @"^(\+84)?(0)?([1-9]\d{8,9})$", 9, "X XXX XXXXX") },
            { "PH", new PhonePattern("PH", "Philippines", "63", @"^(\+63)?(0)?([2-9]\d{8,9})$", 10, "XXX XXX XXXX") },
            { "MY", new PhonePattern("MY", "Malaisie", "60", @"^(\+60)?(0)?([1-9]\d{8,9})$", 9, "XX XXX XXXX") },
            { "SG", new PhonePattern("SG", "Singapour", "65", @"^(\+65)?(0)?([3-9]\d{7})$", 8, "XXXX XXXX") },
            { "ID", new PhonePattern("ID", "Indonésie", "62", @"^(\+62)?(0)?([2-9]\d{9,11})$", 10, "XXX XXXX XXXX") },
            { "BN", new PhonePattern("BN", "Brunei", "673", @"^(\+673)?(0)?([2-9]\d{6})$", 7, "XXX XXXX") },
            { "KH", new PhonePattern("KH", "Cambodge", "855", @"^(\+855)?(0)?([1-9]\d{8,9})$", 9, "XX XXX XXXX") },
            { "LA", new PhonePattern("LA", "Laos", "856", @"^(\+856)?(0)?([2-9]\d{8})$", 9, "XX XXX XXXX") },
            { "MM", new PhonePattern("MM", "Myanmar", "95", @"^(\+95)?(0)?([1-9]\d{8,9})$", 9, "XX XXX XXXX") },
            { "BD", new PhonePattern("BD", "Bangladesh", "880", @"^(\+880)?(0)?([1-9]\d{9})$", 10, "XX XXX XXXX") },
            { "LK", new PhonePattern("LK", "Sri Lanka", "94", @"^(\+94)?(0)?([1-9]\d{8})$", 9, "XX XXX XXXX") },
            { "MV", new PhonePattern("MV", "Maldives", "960", @"^(\+960)?(0)?([3-9]\d{6})$", 7, "XXX XXXX") },
            { "NP", new PhonePattern("NP", "Népal", "977", @"^(\+977)?(0)?([1-9]\d{8})$", 9, "XX XXX XXXX") },
            { "BT", new PhonePattern("BT", "Bhoutan", "975", @"^(\+975)?(0)?([1-9]\d{7})$", 8, "XX XXX XXX") },
            { "PK", new PhonePattern("PK", "Pakistan", "92", @"^(\+92)?(0)?([3-9]\d{9})$", 10, "XXX XXXXXXX") },
            { "AF", new PhonePattern("AF", "Afghanistan", "93", @"^(\+93)?(0)?([2-9]\d{8})$", 9, "XX XXX XXXX") },
            { "IR", new PhonePattern("IR", "Iran", "98", @"^(\+98)?(0)?([1-9]\d{9})$", 10, "XXX XXX XXXX") },
            { "IQ", new PhonePattern("IQ", "Irak", "964", @"^(\+964)?(0)?([1-9]\d{8,9})$", 10, "XXX XXX XXXX") },
            { "SY", new PhonePattern("SY", "Syrie", "963", @"^(\+963)?(0)?([1-9]\d{8})$", 9, "XXX XXX XXXX") },
            { "LB", new PhonePattern("LB", "Liban", "961", @"^(\+961)?(0)?([1-9]\d{7})$", 8, "XX XXX XXX") },
            { "JO", new PhonePattern("JO", "Jordanie", "962", @"^(\+962)?(0)?([7-9]\d{7})$", 8, "X XXX XXXX") },
            { "PS", new PhonePattern("PS", "Palestine", "970", @"^(\+970)?(0)?([2-9]\d{8})$", 9, "XXX XXX XXXX") },
            { "IL", new PhonePattern("IL", "Israël", "972", @"^(\+972)?(0)?([2-9]\d{8})$", 9, "X XXX XXXX") },
            { "KZ", new PhonePattern("KZ", "Kazakhstan", "7", @"^(\+7)?(0)?([3-9]\d{9})$", 10, "XXX XXX XXXX") },
            { "KG", new PhonePattern("KG", "Kirghizistan", "996", @"^(\+996)?(0)?([3-9]\d{8})$", 9, "XXX XX XX XX") },
            { "UZ", new PhonePattern("UZ", "Ouzbékistan", "998", @"^(\+998)?(0)?([3-9]\d{8})$", 9, "XX XXX XX XX") },
            { "TM", new PhonePattern("TM", "Turkménistan", "993", @"^(\+993)?(0)?([1-9]\d{7})$", 8, "XX XX XX XX") },
            { "TJ", new PhonePattern("TJ", "Tadjikistan", "992", @"^(\+992)?(0)?([3-9]\d{8})$", 9, "XXX XX XX XX") },
            { "MN", new PhonePattern("MN", "Mongolie", "976", @"^(\+976)?(0)?([1-9]\d{7})$", 8, "XX XX XX XX") },
            { "AZ", new PhonePattern("AZ", "Azerbaïdjan", "994", @"^(\+994)?(0)?([1-9]\d{8})$", 9, "XX XXX XX XX") },
            { "GE", new PhonePattern("GE", "Géorgie", "995", @"^(\+995)?(0)?([3-9]\d{8})$", 9, "XXX XX XX XX") },
            { "AM", new PhonePattern("AM", "Arménie", "374", @"^(\+374)?(0)?([1-9]\d{7})$", 8, "XX XX XX XX") },
            
            // Moyen-Orient complet
            { "SA", new PhonePattern("SA", "Arabie Saoudite", "966", @"^(\+966)?(0)?([5]\d{8})$", 9, "5 XXX XXXX") },
            { "AE", new PhonePattern("AE", "Émirats Arabes Unis", "971", @"^(\+971)?(0)?([5]\d{8})$", 9, "5X XXX XXXX") },
            { "QA", new PhonePattern("QA", "Qatar", "974", @"^(\+974)?(0)?([3-9]\d{7})$", 8, "XXXX XXXX") },
            { "KW", new PhonePattern("KW", "Koweït", "965", @"^(\+965)?(0)?([5-9]\d{7})$", 8, "XXXX XXXX") },
            { "BH", new PhonePattern("BH", "Bahreïn", "973", @"^(\+973)?(0)?([3-9]\d{7})$", 8, "XXXX XXXX") },
            { "OM", new PhonePattern("OM", "Oman", "968", @"^(\+968)?(0)?([7-9]\d{7})$", 8, "XXXX XXXX") },
            { "YE", new PhonePattern("YE", "Yémen", "967", @"^(\+967)?(0)?([7-9]\d{8})$", 9, "XXX XXX XXXX") },
            
            // Océanie complète
            { "AU", new PhonePattern("AU", "Australie", "61", @"^(\+61)?(0)?([2-9]\d{8,9})$", 9, "X XXXX XXXX") },
            { "NZ", new PhonePattern("NZ", "Nouvelle-Zélande", "64", @"^(\+64)?(0)?([2-9]\d{7,8})$", 9, "XXX XXX XXXX") },
            { "FJ", new PhonePattern("FJ", "Fidji", "679", @"^(\+679)?(0)?([2-9]\d{6})$", 7, "XXX XXXX") },
            { "PG", new PhonePattern("PG", "Papouasie-Nouvelle-Guinée", "675", @"^(\+675)?(0)?([2-9]\d{7})$", 8, "XXX XXXXX") },
            { "VU", new PhonePattern("VU", "Vanuatu", "678", @"^(\+678)?(0)?([2-9]\d{6})$", 7, "XXXXX") },
            { "WS", new PhonePattern("WS", "Samoa", "685", @"^(\+685)?(0)?([2-9]\d{6})$", 7, "XXXXX") },
            { "TO", new PhonePattern("TO", "Tonga", "676", @"^(\+676)?(0)?([2-9]\d{5})$", 6, "XXXXX") },
            { "SB", new PhonePattern("SB", "Îles Salomon", "677", @"^(\+677)?(0)?([2-9]\d{6})$", 7, "XXXXX") },
            { "KI", new PhonePattern("KI", "Kiribati", "686", @"^(\+686)?(0)?([2-9]\d{5})$", 6, "XXXXX") },
            { "TV", new PhonePattern("TV", "Tuvalu", "688", @"^(\+688)?(0)?([2-9]\d{5})$", 6, "XXXXX") },
            { "NR", new PhonePattern("NR", "Nauru", "674", @"^(\+674)?(0)?([2-9]\d{6})$", 7, "XXXXX") },
            { "PW", new PhonePattern("PW", "Palaos", "680", @"^(\+680)?(0)?([2-9]\d{6})$", 7, "XXXXX") },
            { "FM", new PhonePattern("FM", "États Fédérés de Micronésie", "691", @"^(\+691)?(0)?([2-9]\d{6})$", 7, "XXXXX") },
            { "MH", new PhonePattern("MH", "Îles Marshall", "692", @"^(\+692)?(0)?([2-9]\d{6})$", 7, "XXXXX") },
            { "NC", new PhonePattern("NC", "Nouvelle-Calédonie", "687", @"^(\+687)?(0)?([2-9]\d{6})$", 7, "XXXXX") },
            { "PF", new PhonePattern("PF", "Polynésie Française", "689", @"^(\+689)?(0)?([2-9]\d{7})$", 8, "XXXXX XXX") },
            { "AS", new PhonePattern("AS", "Samoa Américaines", "1", @"^(\+1)?(0)?([6-8]\d{9})$", 10, "(XXX) XXX-XXXX") },
            { "GU", new PhonePattern("GU", "Guam", "1", @"^(\+1)?(0)?([6-8]\d{9})$", 10, "(XXX) XXX-XXXX") },
            { "MP", new PhonePattern("MP", "Îles Mariannes du Nord", "1", @"^(\+1)?(0)?([6-8]\d{9})$", 10, "(XXX) XXX-XXXX") },
            { "VI", new PhonePattern("VI", "Îles Vierges Américaines", "1", @"^(\+1)?(0)?([3-4]\d{9})$", 10, "(XXX) XXX-XXXX") },
            { "CK", new PhonePattern("CK", "Îles Cook", "682", @"^(\+682)?(0)?([2-9]\d{5})$", 6, "XXXXX") },
            { "NU", new PhonePattern("NU", "Niue", "683", @"^(\+683)?(0)?([2-9]\d{5})$", 6, "XXXXX") },
            { "TK", new PhonePattern("TK", "Tokelau", "690", @"^(\+690)?(0)?([2-9]\d{5})$", 6, "XXXXX") },
            { "WF", new PhonePattern("WF", "Wallis-et-Futuna", "681", @"^(\+681)?(0)?([2-9]\d{6})$", 7, "XXXXX") },
            { "PN", new PhonePattern("PN", "Pitcairn", "64", @"^(\+64)?(0)?([2-9]\d{8})$", 9, "XXX XXX XXXX") }
        };

        private static readonly Dictionary<string, string> _phoneCodeToIso2 = new();
        private static readonly Regex _cleanPhoneRegex = new Regex(@"[^\d+]", RegexOptions.Compiled);

        static PhoneUtils()
        {
            // Initialiser le mapping indicatif -> ISO2
            foreach (var kvp in _phonePatterns)
            {
                var phoneCode = kvp.Value.PhoneCode;
                if (!_phoneCodeToIso2.ContainsKey(phoneCode))
                {
                    _phoneCodeToIso2[phoneCode] = kvp.Key;
                }
            }
        }

        public static bool IsValidPhoneNumber(string phoneNumber, string iso2Code)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber) || string.IsNullOrWhiteSpace(iso2Code))
                return false;

            var normalizedIso2 = iso2Code.ToUpperInvariant().Trim();
            if (!_phonePatterns.TryGetValue(normalizedIso2, out var pattern))
                return false;

            var cleanNumber = CleanPhoneNumber(phoneNumber);
            return Regex.IsMatch(cleanNumber, pattern.RegexPattern);
        }

        public static bool IsValidPhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return false;

            var cleanNumber = CleanPhoneNumber(phoneNumber);
            
            // Essayer de déterminer le pays par l'indicatif
            var country = DetectCountryFromPhoneNumber(phoneNumber);
            if (country != null)
            {
                return IsValidPhoneNumber(phoneNumber, country);
            }

            // Si on ne peut pas déterminer le pays, vérifier les formats généraux
            return cleanNumber.Length >= 7 && cleanNumber.Length <= 15 && cleanNumber.All(char.IsDigit);
        }

        public static string? DetectCountryFromPhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return null;

            var cleanNumber = CleanPhoneNumber(phoneNumber);
            
            // Extraire l'indicatif du numéro
            foreach (var kvp in _phonePatterns.OrderByDescending(x => x.Value.PhoneCode.Length))
            {
                var pattern = kvp.Value;
                if (cleanNumber.StartsWith($"+{pattern.PhoneCode}") || cleanNumber.StartsWith($"00{pattern.PhoneCode}"))
                {
                    return kvp.Key;
                }
            }

            return null;
        }

        public static string FormatPhoneNumber(string phoneNumber, string iso2Code, bool includeCountryCode = true)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber) || string.IsNullOrWhiteSpace(iso2Code))
                return phoneNumber;

            var normalizedIso2 = iso2Code.ToUpperInvariant().Trim();
            if (!_phonePatterns.TryGetValue(normalizedIso2, out var pattern))
                return phoneNumber;

            var cleanNumber = CleanPhoneNumber(phoneNumber);
            var match = Regex.Match(cleanNumber, pattern.RegexPattern);
            
            if (!match.Success)
                return phoneNumber;

            // Extraire le numéro local
            var localNumber = ExtractLocalNumber(cleanNumber, pattern.PhoneCode);
            
            // Formater selon le pattern du pays
            var formattedNumber = FormatLocalNumber(localNumber, pattern.Format);
            
            if (includeCountryCode)
            {
                return $"+{pattern.PhoneCode} {formattedNumber}";
            }
            
            return formattedNumber;
        }

        public static string FormatPhoneNumberInternational(string phoneNumber, string iso2Code)
        {
            return FormatPhoneNumber(phoneNumber, iso2Code, true);
        }

        public static string FormatPhoneNumberLocal(string phoneNumber, string iso2Code)
        {
            return FormatPhoneNumber(phoneNumber, iso2Code, false);
        }

        public static string NormalizePhoneNumber(string phoneNumber, string iso2Code)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber) || string.IsNullOrWhiteSpace(iso2Code))
                return CleanPhoneNumber(phoneNumber);

            var normalizedIso2 = iso2Code.ToUpperInvariant().Trim();
            if (!_phonePatterns.TryGetValue(normalizedIso2, out var pattern))
                return CleanPhoneNumber(phoneNumber);

            var cleanNumber = CleanPhoneNumber(phoneNumber);
            var localNumber = ExtractLocalNumber(cleanNumber, pattern.PhoneCode);
            
            return $"+{pattern.PhoneCode}{localNumber}";
        }

        public static string CleanPhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return string.Empty;

            return _cleanPhoneRegex.Replace(phoneNumber.Trim(), "");
        }

        public static string ExtractLocalNumber(string phoneNumber, string phoneCode)
        {
            var cleanNumber = CleanPhoneNumber(phoneNumber);
            
            // Supprimer l'indicatif du pays
            if (cleanNumber.StartsWith($"+{phoneCode}"))
            {
                return cleanNumber.Substring(phoneCode.Length + 1);
            }
            
            if (cleanNumber.StartsWith($"00{phoneCode}"))
            {
                return cleanNumber.Substring(phoneCode.Length + 2);
            }
            
            if (cleanNumber.StartsWith(phoneCode) && cleanNumber.Length > phoneCode.Length)
            {
                return cleanNumber.Substring(phoneCode.Length);
            }
            
            return cleanNumber;
        }

        public static string FormatLocalNumber(string localNumber, string format)
        {
            if (string.IsNullOrWhiteSpace(localNumber) || string.IsNullOrWhiteSpace(format))
                return localNumber;

            var digits = localNumber.ToCharArray();
            var result = new StringBuilder();
            var digitIndex = 0;

            foreach (var c in format)
            {
                if (c == 'X')
                {
                    if (digitIndex < digits.Length)
                    {
                        result.Append(digits[digitIndex]);
                        digitIndex++;
                    }
                }
                else
                {
                    result.Append(c);
                }
            }

            return result.ToString();
        }

        public static string GetPhoneCode(string iso2Code)
        {
            var normalizedIso2 = iso2Code.ToUpperInvariant().Trim();
            return _phonePatterns.TryGetValue(normalizedIso2, out var pattern) ? pattern.PhoneCode : string.Empty;
        }

        public static string GetCountryName(string iso2Code)
        {
            var normalizedIso2 = iso2Code.ToUpperInvariant().Trim();
            return _phonePatterns.TryGetValue(normalizedIso2, out var pattern) ? pattern.CountryName : string.Empty;
        }

        public static int GetPhoneNumberLength(string iso2Code)
        {
            var normalizedIso2 = iso2Code.ToUpperInvariant().Trim();
            return _phonePatterns.TryGetValue(normalizedIso2, out var pattern) ? pattern.LocalNumberLength : 0;
        }

        public static string GetPhoneNumberFormat(string iso2Code)
        {
            var normalizedIso2 = iso2Code.ToUpperInvariant().Trim();
            return _phonePatterns.TryGetValue(normalizedIso2, out var pattern) ? pattern.Format : string.Empty;
        }

        public static PhonePattern? GetPhonePattern(string iso2Code)
        {
            var normalizedIso2 = iso2Code.ToUpperInvariant().Trim();
            return _phonePatterns.TryGetValue(normalizedIso2, out var pattern) ? pattern : null;
        }

        public static IEnumerable<PhonePattern> GetAllPhonePatterns()
        {
            return _phonePatterns.Values.OrderBy(p => p.CountryName);
        }

        public static IEnumerable<PhonePattern> GetPhonePatternsByRegion(string region)
        {
            var countries = CountryUtils.GetCountriesByRegion(region);
            return countries
                .Select(c => GetPhonePattern(c.Iso2))
                .Where(p => p != null)
                .OrderBy(p => p!.CountryName)
                .Cast<PhonePattern>();
        }

        public static bool IsMobileNumber(string phoneNumber, string iso2Code)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber) || string.IsNullOrWhiteSpace(iso2Code))
                return false;

            var normalizedIso2 = iso2Code.ToUpperInvariant().Trim();
            if (!_phonePatterns.TryGetValue(normalizedIso2, out var pattern))
                return false;

            var cleanNumber = CleanPhoneNumber(phoneNumber);
            var localNumber = ExtractLocalNumber(cleanNumber, pattern.PhoneCode);
            
            // Vérifier si le numéro commence par un préfixe mobile
            return IsMobilePrefix(localNumber, normalizedIso2);
        }

        public static bool IsLandlineNumber(string phoneNumber, string iso2Code)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber) || string.IsNullOrWhiteSpace(iso2Code))
                return false;

            var normalizedIso2 = iso2Code.ToUpperInvariant().Trim();
            if (!_phonePatterns.TryGetValue(normalizedIso2, out var pattern))
                return false;

            var cleanNumber = CleanPhoneNumber(phoneNumber);
            var localNumber = ExtractLocalNumber(cleanNumber, pattern.PhoneCode);
            
            // Vérifier si le numéro commence par un préfixe fixe
            return IsLandlinePrefix(localNumber, normalizedIso2);
        }

        private static bool IsMobilePrefix(string localNumber, string iso2Code)
        {
            if (string.IsNullOrWhiteSpace(localNumber))
                return false;

            return iso2Code switch
            {
                // UEMOA
                "BF" => localNumber.StartsWith("6") || localNumber.StartsWith("7"),
                "BJ" => localNumber.StartsWith("9") || localNumber.StartsWith("6") || localNumber.StartsWith("5"),
                "CI" => localNumber.StartsWith("0") || localNumber.StartsWith("5") || localNumber.StartsWith("7"),
                "GN" => localNumber.StartsWith("6") || localNumber.StartsWith("7"),
                "ML" => localNumber.StartsWith("6") || localNumber.StartsWith("7") || localNumber.StartsWith("8"),
                "NE" => localNumber.StartsWith("9") || localNumber.StartsWith("8"),
                "SN" => localNumber.StartsWith("7") || localNumber.StartsWith("8") || localNumber.StartsWith("9"),
                "TG" => localNumber.StartsWith("9") || localNumber.StartsWith("7") || localNumber.StartsWith("1"),
                
                // CEMAC
                "CM" => localNumber.StartsWith("6") || localNumber.StartsWith("7"),
                "CF" => localNumber.StartsWith("7"),
                "TD" => localNumber.StartsWith("9") || localNumber.StartsWith("6"),
                "CG" => localNumber.StartsWith("0") || localNumber.StartsWith("5"),
                "GA" => localNumber.StartsWith("0") || localNumber.StartsWith("6") || localNumber.StartsWith("7"),
                "GQ" => localNumber.StartsWith("5") || localNumber.StartsWith("2") || localNumber.StartsWith("3"),
                
                // Maghreb
                "MA" => localNumber.StartsWith("6") || localNumber.StartsWith("7"),
                "DZ" => localNumber.StartsWith("5") || localNumber.StartsWith("6") || localNumber.StartsWith("7"),
                "TN" => localNumber.StartsWith("2") || localNumber.StartsWith("4") || localNumber.StartsWith("9"),
                
                // Afrique de l'Ouest (non-UEMOA)
                "NG" => localNumber.StartsWith("7") || localNumber.StartsWith("8") || localNumber.StartsWith("9"),
                "GH" => localNumber.StartsWith("2") || localNumber.StartsWith("5"),
                "LR" => localNumber.StartsWith("7") || localNumber.StartsWith("8"),
                "SL" => localNumber.StartsWith("7") || localNumber.StartsWith("8") || localNumber.StartsWith("9"),
                "GM" => localNumber.StartsWith("7") || localNumber.StartsWith("9"),
                "GW" => localNumber.StartsWith("9") || localNumber.StartsWith("6"),
                "CV" => localNumber.StartsWith("5") || localNumber.StartsWith("9"),
                "ST" => localNumber.StartsWith("9") || localNumber.StartsWith("6"),
                
                // Afrique de l'Est
                "KE" => localNumber.StartsWith("7") || localNumber.StartsWith("1"),
                "UG" => localNumber.StartsWith("7") || localNumber.StartsWith("3") || localNumber.StartsWith("4"),
                "TZ" => localNumber.StartsWith("7") || localNumber.StartsWith("6"),
                "RW" => localNumber.StartsWith("7") || localNumber.StartsWith("8"),
                "BI" => localNumber.StartsWith("7") || localNumber.StartsWith("6"),
                "ET" => localNumber.StartsWith("9") || localNumber.StartsWith("7"),
                "SO" => localNumber.StartsWith("6") || localNumber.StartsWith("7"),
                "DJ" => localNumber.StartsWith("7") || localNumber.StartsWith("8"),
                "ER" => localNumber.StartsWith("1"),
                
                // Afrique Australe
                "ZA" => localNumber.StartsWith("6") || localNumber.StartsWith("7") || localNumber.StartsWith("8"),
                "ZW" => localNumber.StartsWith("7") || localNumber.StartsWith("8"),
                "ZM" => localNumber.StartsWith("9") || localNumber.StartsWith("7"),
                "MW" => localNumber.StartsWith("9") || localNumber.StartsWith("8"),
                "BW" => localNumber.StartsWith("7") || localNumber.StartsWith("8"),
                "NA" => localNumber.StartsWith("6") || localNumber.StartsWith("8") || localNumber.StartsWith("7"),
                "SZ" => localNumber.StartsWith("7") || localNumber.StartsWith("8"),
                "LS" => localNumber.StartsWith("5") || localNumber.StartsWith("6"),
                
                // Afrique Centrale
                "CD" => localNumber.StartsWith("8") || localNumber.StartsWith("9"),
                "AO" => localNumber.StartsWith("9") || localNumber.StartsWith("8"),
                "MZ" => localNumber.StartsWith("8") || localNumber.StartsWith("7"),
                
                // Océan Indien
                "MU" => localNumber.StartsWith("5") || localNumber.StartsWith("7"),
                "SC" => localNumber.StartsWith("2") || localNumber.StartsWith("5"),
                "KM" => localNumber.StartsWith("3") || localNumber.StartsWith("9"),
                "MG" => localNumber.StartsWith("3") || localNumber.StartsWith("4"),
                
                // Afrique du Nord
                "EG" => localNumber.StartsWith("1") || localNumber.StartsWith("0") || localNumber.StartsWith("2"),
                "LY" => localNumber.StartsWith("9") || localNumber.StartsWith("1"),
                
                // Europe
                "FR" => localNumber.StartsWith("6") || localNumber.StartsWith("7"),
                "BE" => localNumber.StartsWith("4") || localNumber.StartsWith("5"),
                "CH" => localNumber.StartsWith("7") || localNumber.StartsWith("8"),
                "DE" => localNumber.StartsWith("1") || localNumber.StartsWith("6") || localNumber.StartsWith("7"),
                "IT" => localNumber.StartsWith("3"),
                "ES" => localNumber.StartsWith("6") || localNumber.StartsWith("7"),
                "PT" => localNumber.StartsWith("9") || localNumber.StartsWith("2"),
                "NL" => localNumber.StartsWith("6"),
                "LU" => localNumber.StartsWith("6"),
                "GB" => localNumber.StartsWith("7"),
                "IE" => localNumber.StartsWith("8") || localNumber.StartsWith("5"),
                
                // Amérique
                "US" => localNumber.StartsWith("2") || localNumber.StartsWith("3") || localNumber.StartsWith("5") || localNumber.StartsWith("6") || localNumber.StartsWith("7") || localNumber.StartsWith("8") || localNumber.StartsWith("9"),
                "CA" => localNumber.StartsWith("2") || localNumber.StartsWith("3") || localNumber.StartsWith("4") || localNumber.StartsWith("5") || localNumber.StartsWith("6") || localNumber.StartsWith("7") || localNumber.StartsWith("8") || localNumber.StartsWith("9"),
                "MX" => localNumber.StartsWith("1"),
                "BR" => localNumber.StartsWith("9") || localNumber.StartsWith("8") || localNumber.StartsWith("7"),
                "AR" => localNumber.StartsWith("1"),
                
                // Asie
                "CN" => localNumber.StartsWith("1"),
                "JP" => localNumber.StartsWith("7") || localNumber.StartsWith("8") || localNumber.StartsWith("9"),
                "IN" => localNumber.StartsWith("9") || localNumber.StartsWith("8") || localNumber.StartsWith("7") || localNumber.StartsWith("6"),
                "KR" => localNumber.StartsWith("1"),
                
                // Moyen-Orient
                "SA" => localNumber.StartsWith("5"),
                "AE" => localNumber.StartsWith("5"),
                "TR" => localNumber.StartsWith("5"),
                
                _ => false
            };
        }

        private static bool IsLandlinePrefix(string localNumber, string iso2Code)
        {
            if (string.IsNullOrWhiteSpace(localNumber))
                return false;

            return iso2Code switch
            {
                // UEMOA
                "BF" => localNumber.StartsWith("2") || localNumber.StartsWith("4") || localNumber.StartsWith("5"),
                "BJ" => localNumber.StartsWith("2") || localNumber.StartsWith("4"),
                "CI" => localNumber.StartsWith("2") || localNumber.StartsWith("3") || localNumber.StartsWith("4"),
                "GN" => localNumber.StartsWith("3") || localNumber.StartsWith("4"),
                "ML" => localNumber.StartsWith("2") || localNumber.StartsWith("4") || localNumber.StartsWith("5"),
                "NE" => localNumber.StartsWith("2") || localNumber.StartsWith("3") || localNumber.StartsWith("4"),
                "SN" => localNumber.StartsWith("3") || localNumber.StartsWith("4") || localNumber.StartsWith("5"),
                "TG" => localNumber.StartsWith("2") || localNumber.StartsWith("3") || localNumber.StartsWith("4"),
                
                // CEMAC
                "CM" => localNumber.StartsWith("2") || localNumber.StartsWith("3") || localNumber.StartsWith("4"),
                "CF" => localNumber.StartsWith("2") || localNumber.StartsWith("3") || localNumber.StartsWith("4"),
                "TD" => localNumber.StartsWith("2") || localNumber.StartsWith("3"),
                "CG" => localNumber.StartsWith("2") || localNumber.StartsWith("4"),
                "GA" => localNumber.StartsWith("1") || localNumber.StartsWith("2") || localNumber.StartsWith("4"),
                "GQ" => localNumber.StartsWith("3") || localNumber.StartsWith("4") || localNumber.StartsWith("6"),
                
                // Maghreb
                "MA" => localNumber.StartsWith("5") || localNumber.StartsWith("8"),
                "DZ" => localNumber.StartsWith("2") || localNumber.StartsWith("3") || localNumber.StartsWith("4") || localNumber.StartsWith("9"),
                "TN" => localNumber.StartsWith("3") || localNumber.StartsWith("7") || localNumber.StartsWith("8"),
                
                // Autres pays...
                _ => !IsMobilePrefix(localNumber, iso2Code)
            };
        }

        public static string MaskPhoneNumber(string phoneNumber, int visibleDigits = 4, char maskChar = '*')
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return phoneNumber;

            var cleanNumber = CleanPhoneNumber(phoneNumber);
            if (cleanNumber.Length <= visibleDigits)
                return new string(maskChar, cleanNumber.Length);

            var visible = cleanNumber.Substring(cleanNumber.Length - visibleDigits);
            var masked = new string(maskChar, cleanNumber.Length - visibleDigits);
            return masked + visible;
        }

        public static string MaskPhoneNumberMiddle(string phoneNumber, int visibleStart = 2, int visibleEnd = 2, char maskChar = '*')
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return phoneNumber;

            var cleanNumber = CleanPhoneNumber(phoneNumber);
            if (cleanNumber.Length <= visibleStart + visibleEnd)
                return new string(maskChar, cleanNumber.Length);

            var start = cleanNumber.Substring(0, visibleStart);
            var end = cleanNumber.Substring(cleanNumber.Length - visibleEnd);
            var middle = new string(maskChar, cleanNumber.Length - visibleStart - visibleEnd);

            return start + middle + end;
        }

        public static PhoneNumberInfo ParsePhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return new PhoneNumberInfo();

            var cleanNumber = CleanPhoneNumber(phoneNumber);
            var country = DetectCountryFromPhoneNumber(phoneNumber);
            
            if (country == null)
            {
                return new PhoneNumberInfo
                {
                    OriginalNumber = phoneNumber,
                    CleanNumber = cleanNumber,
                    IsValid = false,
                    CountryCode = null,
                    LocalNumber = cleanNumber,
                    FormattedInternational = cleanNumber,
                    FormattedLocal = cleanNumber
                };
            }

            var pattern = GetPhonePattern(country);
            var localNumber = ExtractLocalNumber(cleanNumber, pattern!.PhoneCode);
            var isValid = IsValidPhoneNumber(phoneNumber, country);
            var isMobile = isValid ? IsMobileNumber(phoneNumber, country) : false;
            var isLandline = isValid ? IsLandlineNumber(phoneNumber, country) : false;

            return new PhoneNumberInfo
            {
                OriginalNumber = phoneNumber,
                CleanNumber = cleanNumber,
                IsValid = isValid,
                CountryCode = country,
                PhoneCode = pattern.PhoneCode,
                CountryName = pattern.CountryName,
                LocalNumber = localNumber,
                FormattedInternational = FormatPhoneNumberInternational(phoneNumber, country),
                FormattedLocal = FormatPhoneNumberLocal(phoneNumber, country),
                IsMobile = isMobile,
                IsLandline = isLandline
            };
        }

        public static bool AreSamePhoneNumber(string phone1, string phone2, string? iso2Code = null)
        {
            if (string.IsNullOrWhiteSpace(phone1) || string.IsNullOrWhiteSpace(phone2))
                return false;

            var clean1 = CleanPhoneNumber(phone1);
            var clean2 = CleanPhoneNumber(phone2);

            if (iso2Code != null)
            {
                var normalized1 = NormalizePhoneNumber(phone1, iso2Code);
                var normalized2 = NormalizePhoneNumber(phone2, iso2Code);
                return normalized1.Equals(normalized2, StringComparison.OrdinalIgnoreCase);
            }

            return clean1.Equals(clean2, StringComparison.OrdinalIgnoreCase);
        }

        public static string GenerateRandomPhoneNumber(string iso2Code, bool isMobile = true)
        {
            var pattern = GetPhonePattern(iso2Code);
            if (pattern == null)
                throw new ArgumentException($"Country code '{iso2Code}' not supported");

            var random = new Random();
            var localNumber = new StringBuilder();
            
            // Générer un numéro local selon le format du pays
            for (int i = 0; i < pattern.LocalNumberLength; i++)
            {
                if (i == 0 && isMobile)
                {
                    // Utiliser un préfixe mobile
                    var mobilePrefixes = GetMobilePrefixes(iso2Code);
                    if (mobilePrefixes.Length > 0)
                    {
                        var prefix = mobilePrefixes[random.Next(mobilePrefixes.Length)];
                        localNumber.Append(prefix);
                        i += prefix.Length - 1;
                        continue;
                    }
                }
                
                localNumber.Append(random.Next(0, 10));
            }

            return $"+{pattern.PhoneCode}{localNumber}";
        }

        private static string[] GetMobilePrefixes(string iso2Code)
        {
            return iso2Code switch
            {
                "BF" => new[] { "6", "7" },
                "BJ" => new[] { "9", "6", "5" },
                "CI" => new[] { "0", "5", "7" },
                "GN" => new[] { "6", "7" },
                "ML" => new[] { "6", "7", "8" },
                "NE" => new[] { "9", "8" },
                "SN" => new[] { "7", "8", "9" },
                "TG" => new[] { "9", "7", "1" },
                "FR" => new[] { "6", "7" },
                "BE" => new[] { "4", "5" },
                "CH" => new[] { "7", "8" },
                "DE" => new[] { "1", "6", "7" },
                "IT" => new[] { "3" },
                "ES" => new[] { "6", "7" },
                "PT" => new[] { "9", "2" },
                "NL" => new[] { "6" },
                "LU" => new[] { "6" },
                "GB" => new[] { "7" },
                "IE" => new[] { "8", "5" },
                "US" => new[] { "2", "3", "5", "6", "7", "8", "9" },
                "CA" => new[] { "2", "3", "4", "5", "6", "7", "8", "9" },
                "MX" => new[] { "1" },
                "BR" => new[] { "9", "8", "7" },
                "AR" => new[] { "1" },
                "CN" => new[] { "1" },
                "JP" => new[] { "7", "8", "9" },
                "IN" => new[] { "9", "8", "7", "6" },
                "KR" => new[] { "1" },
                "SA" => new[] { "5" },
                "AE" => new[] { "5" },
                "TR" => new[] { "5" },
                _ => new[] { "7" }
            };
        }
    }

    public class PhonePattern
    {
        public string CountryCode { get; set; } = string.Empty;
        public string CountryName { get; set; } = string.Empty;
        public string PhoneCode { get; set; } = string.Empty;
        public string RegexPattern { get; set; } = string.Empty;
        public int LocalNumberLength { get; set; }
        public string Format { get; set; } = string.Empty;

        public PhonePattern(string countryCode, string countryName, string phoneCode, string regexPattern, int localNumberLength, string format)
        {
            CountryCode = countryCode;
            CountryName = countryName;
            PhoneCode = phoneCode;
            RegexPattern = regexPattern;
            LocalNumberLength = localNumberLength;
            Format = format;
        }

        public override string ToString()
        {
            return $"{CountryName} (+{PhoneCode})";
        }
    }

    public class PhoneNumberInfo
    {
        public string OriginalNumber { get; set; } = string.Empty;
        public string CleanNumber { get; set; } = string.Empty;
        public bool IsValid { get; set; }
        public string? CountryCode { get; set; }
        public string PhoneCode { get; set; } = string.Empty;
        public string CountryName { get; set; } = string.Empty;
        public string LocalNumber { get; set; } = string.Empty;
        public string FormattedInternational { get; set; } = string.Empty;
        public string FormattedLocal { get; set; } = string.Empty;
        public bool IsMobile { get; set; }
        public bool IsLandline { get; set; }

        public override string ToString()
        {
            return FormattedInternational;
        }
    }
}
