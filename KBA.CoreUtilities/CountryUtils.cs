using System.Globalization;

namespace KBA.CoreUtilities.Utilities
{
    public static class CountryUtils
    {
        private static readonly Dictionary<string, CountryInfo> _countries = new()
        {
            // Afrique de l'Ouest (UEMOA)
            { "BF", new CountryInfo("BF", "BFA", "Burkina Faso", "226", "XOF", "Ouagadougou", "fr") },
            { "BJ", new CountryInfo("BJ", "BEN", "Bénin", "229", "XOF", "Porto-Novo", "fr") },
            { "CI", new CountryInfo("CI", "CIV", "Côte d'Ivoire", "225", "XOF", "Yamoussoukro", "fr") },
            { "GN", new CountryInfo("GN", "GIN", "Guinée", "224", "GNF", "Conakry", "fr") },
            { "ML", new CountryInfo("ML", "MLI", "Mali", "223", "XOF", "Bamako", "fr") },
            { "NE", new CountryInfo("NE", "NER", "Niger", "227", "XOF", "Niamey", "fr") },
            { "SN", new CountryInfo("SN", "SEN", "Sénégal", "221", "XOF", "Dakar", "fr") },
            { "TG", new CountryInfo("TG", "TGO", "Togo", "228", "XOF", "Lomé", "fr") },
            
            // Afrique Centrale (CEMAC)
            { "CM", new CountryInfo("CM", "CMR", "Cameroun", "237", "XAF", "Yaoundé", "fr") },
            { "CF", new CountryInfo("CF", "CAF", "République Centrafricaine", "236", "XAF", "Bangui", "fr") },
            { "TD", new CountryInfo("TD", "TCD", "Tchad", "235", "XAF", "N'Djamena", "fr") },
            { "CG", new CountryInfo("CG", "COG", "Congo-Brazzaville", "242", "XAF", "Brazzaville", "fr") },
            { "GA", new CountryInfo("GA", "GAB", "Gabon", "241", "XAF", "Libreville", "fr") },
            { "GQ", new CountryInfo("GQ", "GNQ", "Guinée Équatoriale", "240", "XAF", "Malabo", "fr") },
            
            // Autres pays africains
            { "MA", new CountryInfo("MA", "MAR", "Maroc", "212", "MAD", "Rabat", "ar") },
            { "DZ", new CountryInfo("DZ", "DZA", "Algérie", "213", "DZD", "Alger", "ar") },
            { "TN", new CountryInfo("TN", "TUN", "Tunisie", "216", "TND", "Tunis", "ar") },
            { "EG", new CountryInfo("EG", "EGY", "Égypte", "20", "EGP", "Le Caire", "ar") },
            { "ZA", new CountryInfo("ZA", "ZAF", "Afrique du Sud", "27", "ZAR", "Pretoria", "en") },
            { "NG", new CountryInfo("NG", "NGA", "Nigeria", "234", "NGN", "Abuja", "en") },
            { "KE", new CountryInfo("KE", "KEN", "Kenya", "254", "KES", "Nairobi", "en") },
            { "GH", new CountryInfo("GH", "GHA", "Ghana", "233", "GHS", "Accra", "en") },
            { "ET", new CountryInfo("ET", "ETH", "Éthiopie", "251", "ETB", "Addis-Abeba", "am") },
            { "UG", new CountryInfo("UG", "UGA", "Ouganda", "256", "UGX", "Kampala", "en") },
            { "TZ", new CountryInfo("TZ", "TZA", "Tanzanie", "255", "TZS", "Dodoma", "sw") },
            { "RW", new CountryInfo("RW", "RWA", "Rwanda", "250", "RWF", "Kigali", "en") },
            { "BI", new CountryInfo("BI", "BDI", "Burundi", "257", "BIF", "Bujumbura", "fr") },
            { "DJ", new CountryInfo("DJ", "DJI", "Djibouti", "253", "DJF", "Djibouti", "fr") },
            { "ER", new CountryInfo("ER", "ERI", "Érythrée", "291", "ERN", "Asmara", "ti") },
            { "SO", new CountryInfo("SO", "SOM", "Somalie", "252", "SOS", "Mogadiscio", "so") },
            { "LR", new CountryInfo("LR", "LBR", "Libéria", "231", "LRD", "Monrovia", "en") },
            { "SL", new CountryInfo("SL", "SLE", "Sierra Leone", "232", "SLL", "Freetown", "en") },
            { "GM", new CountryInfo("GM", "GMB", "Gambie", "220", "GMD", "Banjul", "en") },
            { "GW", new CountryInfo("GW", "GNB", "Guinée-Bissau", "245", "XOF", "Bissau", "pt") },
            { "CV", new CountryInfo("CV", "CPV", "Cap-Vert", "238", "CVE", "Praia", "pt") },
            { "ST", new CountryInfo("ST", "STP", "Sao Tomé-et-Principe", "239", "STN", "São Tomé", "pt") },
            { "MU", new CountryInfo("MU", "MUS", "Maurice", "230", "MUR", "Port Louis", "en") },
            { "SC", new CountryInfo("SC", "SYC", "Seychelles", "248", "SCR", "Victoria", "en") },
            { "KM", new CountryInfo("KM", "COM", "Comores", "269", "KMF", "Moroni", "ar") },
            { "MG", new CountryInfo("MG", "MDG", "Madagascar", "261", "MGA", "Antananarivo", "mg") },
            { "MW", new CountryInfo("MW", "MWI", "Malawi", "265", "MWK", "Lilongwe", "en") },
            { "ZM", new CountryInfo("ZM", "ZMB", "Zambie", "260", "ZMW", "Lusaka", "en") },
            { "ZW", new CountryInfo("ZW", "ZWE", "Zimbabwe", "263", "ZWL", "Harare", "en") },
            { "BW", new CountryInfo("BW", "BWA", "Botswana", "267", "BWP", "Gaborone", "en") },
            { "NA", new CountryInfo("NA", "NAM", "Namibie", "264", "NAD", "Windhoek", "en") },
            { "SZ", new CountryInfo("SZ", "SWZ", "Eswatini", "268", "SZL", "Mbabane", "en") },
            { "LS", new CountryInfo("LS", "LSO", "Lesotho", "266", "LSL", "Maseru", "en") },
            { "AO", new CountryInfo("AO", "AGO", "Angola", "244", "AOA", "Luanda", "pt") },
            { "MZ", new CountryInfo("MZ", "MOZ", "Mozambique", "258", "MZN", "Maputo", "pt") },
            { "CD", new CountryInfo("CD", "COD", "République Démocratique du Congo", "243", "CDF", "Kinshasa", "fr") },
            
            // Autres pays africains manquants
            { "LY", new CountryInfo("LY", "LBY", "Libye", "218", "LYD", "Tripoli", "ar") },
            { "SD", new CountryInfo("SD", "SDN", "Soudan", "249", "SDG", "Khartoum", "ar") },
            { "SS", new CountryInfo("SS", "SSD", "Soudan du Sud", "211", "SSP", "Juba", "en") },
            { "CM", new CountryInfo("CM", "CMR", "Cameroun", "237", "XAF", "Yaoundé", "fr") },
            { "CF", new CountryInfo("CF", "CAF", "République Centrafricaine", "236", "XAF", "Bangui", "fr") },
            { "TD", new CountryInfo("TD", "TCD", "Tchad", "235", "XAF", "N'Djamena", "fr") },
            { "CG", new CountryInfo("CG", "COG", "Congo-Brazzaville", "242", "XAF", "Brazzaville", "fr") },
            { "GA", new CountryInfo("GA", "GAB", "Gabon", "241", "XAF", "Libreville", "fr") },
            { "GQ", new CountryInfo("GQ", "GNQ", "Guinée Équatoriale", "240", "XAF", "Malabo", "fr") },
            { "AO", new CountryInfo("AO", "AGO", "Angola", "244", "AOA", "Luanda", "pt") },
            { "MZ", new CountryInfo("MZ", "MOZ", "Mozambique", "258", "MZN", "Maputo", "pt") },
            { "ZM", new CountryInfo("ZM", "ZMB", "Zambie", "260", "ZMW", "Lusaka", "en") },
            { "ZW", new CountryInfo("ZW", "ZWE", "Zimbabwe", "263", "ZWL", "Harare", "en") },
            { "BW", new CountryInfo("BW", "BWA", "Botswana", "267", "BWP", "Gaborone", "en") },
            { "NA", new CountryInfo("NA", "NAM", "Namibie", "264", "NAD", "Windhoek", "en") },
            { "SZ", new CountryInfo("SZ", "SWZ", "Eswatini", "268", "SZL", "Mbabane", "en") },
            { "LS", new CountryInfo("LS", "LSO", "Lesotho", "266", "LSL", "Maseru", "en") },
            { "MW", new CountryInfo("MW", "MWI", "Malawi", "265", "MWK", "Lilongwe", "en") },
            { "SZ", new CountryInfo("SZ", "SWZ", "Eswatini", "268", "SZL", "Mbabane", "en") },
            { "LS", new CountryInfo("LS", "LSO", "Lesotho", "266", "LSL", "Maseru", "en") },
            
            // Europe complète
            { "FR", new CountryInfo("FR", "FRA", "France", "33", "EUR", "Paris", "fr") },
            { "BE", new CountryInfo("BE", "BEL", "Belgique", "32", "EUR", "Bruxelles", "fr") },
            { "CH", new CountryInfo("CH", "CHE", "Suisse", "41", "CHF", "Berne", "de") },
            { "DE", new CountryInfo("DE", "DEU", "Allemagne", "49", "EUR", "Berlin", "de") },
            { "IT", new CountryInfo("IT", "ITA", "Italie", "39", "EUR", "Rome", "it") },
            { "ES", new CountryInfo("ES", "ESP", "Espagne", "34", "EUR", "Madrid", "es") },
            { "PT", new CountryInfo("PT", "PRT", "Portugal", "351", "EUR", "Lisbonne", "pt") },
            { "NL", new CountryInfo("NL", "NLD", "Pays-Bas", "31", "EUR", "Amsterdam", "nl") },
            { "LU", new CountryInfo("LU", "LUX", "Luxembourg", "352", "EUR", "Luxembourg", "fr") },
            { "GB", new CountryInfo("GB", "GBR", "Royaume-Uni", "44", "GBP", "Londres", "en") },
            { "IE", new CountryInfo("IE", "IRL", "Irlande", "353", "EUR", "Dublin", "en") },
            { "AT", new CountryInfo("AT", "AUT", "Autriche", "43", "EUR", "Vienne", "de") },
            { "SE", new CountryInfo("SE", "SWE", "Suède", "46", "SEK", "Stockholm", "sv") },
            { "NO", new CountryInfo("NO", "NOR", "Norvège", "47", "NOK", "Oslo", "no") },
            { "DK", new CountryInfo("DK", "DNK", "Danemark", "45", "DKK", "Copenhague", "da") },
            { "FI", new CountryInfo("FI", "FIN", "Finlande", "358", "EUR", "Helsinki", "fi") },
            { "PL", new CountryInfo("PL", "POL", "Pologne", "48", "PLN", "Varsovie", "pl") },
            { "CZ", new CountryInfo("CZ", "CZE", "République Tchèque", "420", "CZK", "Prague", "cs") },
            { "SK", new CountryInfo("SK", "SVK", "Slovaquie", "421", "EUR", "Bratislava", "sk") },
            { "HU", new CountryInfo("HU", "HUN", "Hongrie", "36", "HUF", "Budapest", "hu") },
            { "RO", new CountryInfo("RO", "ROU", "Roumanie", "40", "RON", "Bucarest", "ro") },
            { "BG", new CountryInfo("BG", "BGR", "Bulgarie", "359", "BGN", "Sofia", "bg") },
            { "HR", new CountryInfo("HR", "HRV", "Croatie", "385", "HRK", "Zagreb", "hr") },
            { "SI", new CountryInfo("SI", "SVN", "Slovénie", "386", "EUR", "Ljubljana", "sl") },
            { "EE", new CountryInfo("EE", "EST", "Estonie", "372", "EUR", "Tallinn", "et") },
            { "LV", new CountryInfo("LV", "LVA", "Lettonie", "371", "EUR", "Riga", "lv") },
            { "LT", new CountryInfo("LT", "LTU", "Lituanie", "370", "EUR", "Vilnius", "lt") },
            { "GR", new CountryInfo("GR", "GRC", "Grèce", "30", "EUR", "Athènes", "el") },
            { "CY", new CountryInfo("CY", "CYP", "Chypre", "357", "EUR", "Nicosie", "el") },
            { "MT", new CountryInfo("MT", "MLT", "Malte", "356", "EUR", "La Valette", "mt") },
            { "IS", new CountryInfo("IS", "ISL", "Islande", "354", "ISK", "Reykjavik", "is") },
            { "AL", new CountryInfo("AL", "ALB", "Albanie", "355", "ALL", "Tirana", "sq") },
            { "MK", new CountryInfo("MK", "MKD", "Macédoine du Nord", "389", "MKD", "Skopje", "mk") },
            { "ME", new CountryInfo("ME", "MNE", "Monténégro", "382", "EUR", "Podgorica", "sr") },
            { "RS", new CountryInfo("RS", "SRB", "Serbie", "381", "RSD", "Belgrade", "sr") },
            { "BA", new CountryInfo("BA", "BIH", "Bosnie-Herzégovine", "387", "BAM", "Sarajevo", "bs") },
            { "AD", new CountryInfo("AD", "AND", "Andorre", "376", "EUR", "Andorre-la-Vieille", "ca") },
            { "MC", new CountryInfo("MC", "MCO", "Monaco", "377", "EUR", "Monaco", "fr") },
            { "LI", new CountryInfo("LI", "LIE", "Liechtenstein", "423", "CHF", "Vaduz", "de") },
            { "VA", new CountryInfo("VA", "VAT", "Vatican", "379", "EUR", "Vatican", "la") },
            { "SM", new CountryInfo("SM", "SMR", "Saint-Marin", "378", "EUR", "Saint-Marin", "it") },
            
            // Amérique complète
            { "US", new CountryInfo("US", "USA", "États-Unis", "1", "USD", "Washington", "en") },
            { "CA", new CountryInfo("CA", "CAN", "Canada", "1", "CAD", "Ottawa", "en") },
            { "MX", new CountryInfo("MX", "MEX", "Mexique", "52", "MXN", "Mexico", "es") },
            { "BR", new CountryInfo("BR", "BRA", "Brésil", "55", "BRL", "Brasilia", "pt") },
            { "AR", new CountryInfo("AR", "ARG", "Argentine", "54", "ARS", "Buenos Aires", "es") },
            { "CL", new CountryInfo("CL", "CHL", "Chili", "56", "CLP", "Santiago", "es") },
            { "CO", new CountryInfo("CO", "COL", "Colombie", "57", "COP", "Bogota", "es") },
            { "PE", new CountryInfo("PE", "PER", "Pérou", "51", "PEN", "Lima", "es") },
            { "VE", new CountryInfo("VE", "VEN", "Venezuela", "58", "VES", "Caracas", "es") },
            { "EC", new CountryInfo("EC", "ECU", "Équateur", "593", "USD", "Quito", "es") },
            { "BO", new CountryInfo("BO", "BOL", "Bolivie", "591", "BOB", "La Paz", "es") },
            { "PY", new CountryInfo("PY", "PRY", "Paraguay", "595", "PYG", "Asuncion", "es") },
            { "UY", new CountryInfo("UY", "URY", "Uruguay", "598", "UYU", "Montevideo", "es") },
            { "GY", new CountryInfo("GY", "GUY", "Guyana", "592", "GYD", "Georgetown", "en") },
            { "SR", new CountryInfo("SR", "SUR", "Suriname", "597", "SRD", "Paramaribo", "nl") },
            { "GF", new CountryInfo("GF", "GUF", "Guyane Française", "594", "EUR", "Cayenne", "fr") },
            { "CR", new CountryInfo("CR", "CRI", "Costa Rica", "506", "CRC", "San Jose", "es") },
            { "PA", new CountryInfo("PA", "PAN", "Panama", "507", "PAB", "Panama", "es") },
            { "GT", new CountryInfo("GT", "GTM", "Guatemala", "502", "GTQ", "Guatemala", "es") },
            { "SV", new CountryInfo("SV", "SLV", "El Salvador", "503", "USD", "San Salvador", "es") },
            { "HN", new CountryInfo("HN", "HND", "Honduras", "504", "HNL", "Tegucigalpa", "es") },
            { "NI", new CountryInfo("NI", "NIC", "Nicaragua", "505", "NIO", "Managua", "es") },
            { "CU", new CountryInfo("CU", "CUB", "Cuba", "53", "CUP", "La Havane", "es") },
            { "JM", new CountryInfo("JM", "JAM", "Jamaïque", "1", "JMD", "Kingston", "en") },
            { "HT", new CountryInfo("HT", "HTI", "Haïti", "509", "HTG", "Port-au-Prince", "fr") },
            { "DO", new CountryInfo("DO", "DOM", "République Dominicaine", "1", "DOP", "Saint-Domingue", "es") },
            { "BB", new CountryInfo("BB", "BRB", "Barbade", "1", "BBD", "Bridgetown", "en") },
            { "TT", new CountryInfo("TT", "TTO", "Trinité-et-Tobago", "1", "TTD", "Port of Spain", "en") },
            { "BS", new CountryInfo("BS", "BHS", "Bahamas", "1", "BSD", "Nassau", "en") },
            { "BZ", new CountryInfo("BZ", "BLZ", "Belize", "501", "BZD", "Belmopan", "en") },
            { "GL", new CountryInfo("GL", "GRL", "Groenland", "299", "DKK", "Nuuk", "kl") },
            
            // Asie complète
            { "CN", new CountryInfo("CN", "CHN", "Chine", "86", "CNY", "Pékin", "zh") },
            { "JP", new CountryInfo("JP", "JPN", "Japon", "81", "JPY", "Tokyo", "ja") },
            { "IN", new CountryInfo("IN", "IND", "Inde", "91", "INR", "New Delhi", "hi") },
            { "KR", new CountryInfo("KR", "KOR", "Corée du Sud", "82", "KRW", "Séoul", "ko") },
            { "KP", new CountryInfo("KP", "PRK", "Corée du Nord", "850", "KPW", "Pyongyang", "ko") },
            { "TH", new CountryInfo("TH", "THA", "Thaïlande", "66", "THB", "Bangkok", "th") },
            { "VN", new CountryInfo("VN", "VNM", "Vietnam", "84", "VND", "Hanoi", "vi") },
            { "PH", new CountryInfo("PH", "PHL", "Philippines", "63", "PHP", "Manille", "en") },
            { "MY", new CountryInfo("MY", "MYS", "Malaisie", "60", "MYR", "Kuala Lumpur", "ms") },
            { "SG", new CountryInfo("SG", "SGP", "Singapour", "65", "SGD", "Singapour", "en") },
            { "ID", new CountryInfo("ID", "IDN", "Indonésie", "62", "IDR", "Jakarta", "id") },
            { "BN", new CountryInfo("BN", "BRN", "Brunei", "673", "BND", "Bandar Seri Begawan", "ms") },
            { "KH", new CountryInfo("KH", "KHM", "Cambodge", "855", "KHR", "Phnom Penh", "km") },
            { "LA", new CountryInfo("LA", "LAO", "Laos", "856", "LAK", "Vientiane", "lo") },
            { "MM", new CountryInfo("MM", "MMR", "Myanmar", "95", "MMK", "Naypyidaw", "my") },
            { "BD", new CountryInfo("BD", "BGD", "Bangladesh", "880", "BDT", "Dhaka", "bn") },
            { "LK", new CountryInfo("LK", "LKA", "Sri Lanka", "94", "LKR", "Colombo", "si") },
            { "MV", new CountryInfo("MV", "MDV", "Maldives", "960", "MVR", "Male", "dv") },
            { "NP", new CountryInfo("NP", "NPL", "Népal", "977", "NPR", "Kathmandou", "ne") },
            { "BT", new CountryInfo("BT", "BTN", "Bhoutan", "975", "BTN", "Thimphou", "dz") },
            { "PK", new CountryInfo("PK", "PAK", "Pakistan", "92", "PKR", "Islamabad", "ur") },
            { "AF", new CountryInfo("AF", "AFG", "Afghanistan", "93", "AFN", "Kaboul", "ps") },
            { "IR", new CountryInfo("IR", "IRN", "Iran", "98", "IRR", "Téhéran", "fa") },
            { "IQ", new CountryInfo("IQ", "IRQ", "Irak", "964", "IQD", "Bagdad", "ar") },
            { "SY", new CountryInfo("SY", "SYR", "Syrie", "963", "SYP", "Damas", "ar") },
            { "LB", new CountryInfo("LB", "LBN", "Liban", "961", "LBP", "Beyrouth", "ar") },
            { "JO", new CountryInfo("JO", "JOR", "Jordanie", "962", "JOD", "Amman", "ar") },
            { "PS", new CountryInfo("PS", "PSE", "Palestine", "970", "ILS", "Ramallah", "ar") },
            { "IL", new CountryInfo("IL", "ISR", "Israël", "972", "ILS", "Jérusalem", "he") },
            { "KZ", new CountryInfo("KZ", "KAZ", "Kazakhstan", "7", "KZT", "Noursoultan", "kk") },
            { "KG", new CountryInfo("KG", "KGZ", "Kirghizistan", "996", "KGS", "Bichkek", "ky") },
            { "UZ", new CountryInfo("UZ", "UZB", "Ouzbékistan", "998", "UZS", "Tachkent", "uz") },
            { "TM", new CountryInfo("TM", "TKM", "Turkménistan", "993", "TMT", "Achgabat", "tk") },
            { "TJ", new CountryInfo("TJ", "TJK", "Tadjikistan", "992", "TJS", "Douchanbé", "tg") },
            { "MN", new CountryInfo("MN", "MNG", "Mongolie", "976", "MNT", "Oulan-Bator", "mn") },
            { "AZ", new CountryInfo("AZ", "AZE", "Azerbaïdjan", "994", "AZN", "Bakou", "az") },
            { "GE", new CountryInfo("GE", "GEO", "Géorgie", "995", "GEL", "Tbilissi", "ka") },
            { "AM", new CountryInfo("AM", "ARM", "Arménie", "374", "AMD", "Erevan", "hy") },
            
            // Moyen-Orient complet
            { "SA", new CountryInfo("SA", "SAU", "Arabie Saoudite", "966", "SAR", "Riyad", "ar") },
            { "AE", new CountryInfo("AE", "ARE", "Émirats Arabes Unis", "971", "AED", "Abu Dhabi", "ar") },
            { "QA", new CountryInfo("QA", "QAT", "Qatar", "974", "QAR", "Doha", "ar") },
            { "KW", new CountryInfo("KW", "KWT", "Koweït", "965", "KWD", "Koweït City", "ar") },
            { "BH", new CountryInfo("BH", "BHR", "Bahreïn", "973", "BHD", "Manama", "ar") },
            { "OM", new CountryInfo("OM", "OMN", "Oman", "968", "OMR", "Mascate", "ar") },
            { "YE", new CountryInfo("YE", "YEM", "Yémen", "967", "YER", "Sanaa", "ar") },
            
            // Océanie complète
            { "AU", new CountryInfo("AU", "AUS", "Australie", "61", "AUD", "Canberra", "en") },
            { "NZ", new CountryInfo("NZ", "NZL", "Nouvelle-Zélande", "64", "NZD", "Wellington", "en") },
            { "FJ", new CountryInfo("FJ", "FJI", "Fidji", "679", "FJD", "Suva", "en") },
            { "PG", new CountryInfo("PG", "PNG", "Papouasie-Nouvelle-Guinée", "675", "PGK", "Port Moresby", "en") },
            { "VU", new CountryInfo("VU", "VUT", "Vanuatu", "678", "VUV", "Port Vila", "en") },
            { "WS", new CountryInfo("WS", "WSM", "Samoa", "685", "WST", "Apia", "en") },
            { "TO", new CountryInfo("TO", "TON", "Tonga", "676", "TOP", "Nuku'alofa", "en") },
            { "SB", new CountryInfo("SB", "SLB", "Îles Salomon", "677", "SBD", "Honiara", "en") },
            { "KI", new CountryInfo("KI", "KIR", "Kiribati", "686", "KID", "Tarawa", "en") },
            { "TV", new CountryInfo("TV", "TUV", "Tuvalu", "688", "TVD", "Funafuti", "en") },
            { "NR", new CountryInfo("NR", "NRU", "Nauru", "674", "AUD", "Yaren", "en") },
            { "PW", new CountryInfo("PW", "PLW", "Palaos", "680", "USD", "Ngerulmud", "en") },
            { "FM", new CountryInfo("FM", "FSM", "États Fédérés de Micronésie", "691", "USD", "Palikir", "en") },
            { "MH", new CountryInfo("MH", "MHL", "Îles Marshall", "692", "USD", "Majuro", "en") },
            { "NC", new CountryInfo("NC", "NCL", "Nouvelle-Calédonie", "687", "XPF", "Nouméa", "fr") },
            { "PF", new CountryInfo("PF", "PYF", "Polynésie Française", "689", "XPF", "Papeete", "fr") },
            { "AS", new CountryInfo("AS", "ASM", "Samoa Américaines", "1", "USD", "Pago Pago", "en") },
            { "GU", new CountryInfo("GU", "GUM", "Guam", "1", "USD", "Hagåtña", "en") },
            { "MP", new CountryInfo("MP", "MNP", "Îles Mariannes du Nord", "1", "USD", "Saipan", "en") },
            { "VI", new CountryInfo("VI", "VIR", "Îles Vierges Américaines", "1", "USD", "Charlotte Amalie", "en") },
            { "CK", new CountryInfo("CK", "COK", "Îles Cook", "682", "NZD", "Avarua", "en") },
            { "NU", new CountryInfo("NU", "NIU", "Niue", "683", "NZD", "Alofi", "en") },
            { "TK", new CountryInfo("TK", "TKL", "Tokelau", "690", "NZD", "Fakaofo", "en") },
            { "WF", new CountryInfo("WF", "WLF", "Wallis-et-Futuna", "681", "XPF", "Mata-Utu", "fr") },
            { "PN", new CountryInfo("PN", "PCN", "Pitcairn", "64", "NZD", "Adamstown", "en") }
        };

        private static readonly Dictionary<string, string> _iso2ToIso3 = _countries.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Iso3);
        private static readonly Dictionary<string, string> _iso3ToIso2 = _countries.ToDictionary(kvp => kvp.Value.Iso3, kvp => kvp.Key);
        private static readonly Dictionary<string, string> _phoneToIso2 = new();
        private static readonly Dictionary<string, CountryInfo> _currencyToCountries = new();

        static CountryUtils()
        {
            // Initialiser les mappings téléphone -> ISO2
            foreach (var kvp in _countries)
            {
                var phoneCode = kvp.Value.PhoneCode;
                if (!_phoneToIso2.ContainsKey(phoneCode))
                {
                    _phoneToIso2[phoneCode] = kvp.Key;
                }
            }

            // Initialiser les mappings devise -> pays
            foreach (var kvp in _countries)
            {
                var currency = kvp.Value.CurrencyCode;
                if (!_currencyToCountries.ContainsKey(currency))
                {
                    _currencyToCountries[currency] = kvp.Value;
                }
            }
        }

        public static CountryInfo? GetCountryByIso2(string iso2Code)
        {
            if (string.IsNullOrWhiteSpace(iso2Code))
                return null;

            var normalizedCode = iso2Code.ToUpperInvariant().Trim();
            return _countries.TryGetValue(normalizedCode, out var country) ? country : null;
        }

        public static CountryInfo? GetCountryByIso3(string iso3Code)
        {
            if (string.IsNullOrWhiteSpace(iso3Code))
                return null;

            var normalizedCode = iso3Code.ToUpperInvariant().Trim();
            return _iso3ToIso2.TryGetValue(normalizedCode, out var iso2) ? _countries[iso2] : null;
        }

        public static CountryInfo? GetCountryByPhoneCode(string phoneCode)
        {
            if (string.IsNullOrWhiteSpace(phoneCode))
                return null;

            var normalizedCode = phoneCode.Trim();
            return _phoneToIso2.TryGetValue(normalizedCode, out var iso2) ? _countries[iso2] : null;
        }

        public static CountryInfo? GetCountryByCurrency(string currencyCode)
        {
            if (string.IsNullOrWhiteSpace(currencyCode))
                return null;

            var normalizedCode = currencyCode.ToUpperInvariant().Trim();
            return _currencyToCountries.TryGetValue(normalizedCode, out var country) ? country : null;
        }

        public static CountryInfo? GetCountryByName(string countryName)
        {
            if (string.IsNullOrWhiteSpace(countryName))
                return null;

            var normalizedName = countryName.Trim().ToLowerInvariant();
            return _countries.Values.FirstOrDefault(c => 
                c.Name.ToLowerInvariant().Contains(normalizedName) || 
                normalizedName.Contains(c.Name.ToLowerInvariant()));
        }

        public static string? GetIso2FromIso3(string iso3Code)
        {
            if (string.IsNullOrWhiteSpace(iso3Code))
                return null;

            var normalizedCode = iso3Code.ToUpperInvariant().Trim();
            return _iso3ToIso2.TryGetValue(normalizedCode, out var iso2) ? iso2 : null;
        }

        public static string? GetIso3FromIso2(string iso2Code)
        {
            if (string.IsNullOrWhiteSpace(iso2Code))
                return null;

            var normalizedCode = iso2Code.ToUpperInvariant().Trim();
            return _iso2ToIso3.TryGetValue(normalizedCode, out var iso3) ? iso3 : null;
        }

        public static string? GetPhoneCodeFromIso2(string iso2Code)
        {
            var country = GetCountryByIso2(iso2Code);
            return country?.PhoneCode;
        }

        public static string? GetPhoneCodeFromIso3(string iso3Code)
        {
            var country = GetCountryByIso3(iso3Code);
            return country?.PhoneCode;
        }

        public static string? GetCurrencyFromIso2(string iso2Code)
        {
            var country = GetCountryByIso2(iso2Code);
            return country?.CurrencyCode;
        }

        public static string? GetCurrencyFromIso3(string iso3Code)
        {
            var country = GetCountryByIso3(iso3Code);
            return country?.CurrencyCode;
        }

        public static string? GetCapitalFromIso2(string iso2Code)
        {
            var country = GetCountryByIso2(iso2Code);
            return country?.Capital;
        }

        public static string? GetCapitalFromIso3(string iso3Code)
        {
            var country = GetCountryByIso3(iso3Code);
            return country?.Capital;
        }

        public static string? GetLanguageFromIso2(string iso2Code)
        {
            var country = GetCountryByIso2(iso2Code);
            return country?.LanguageCode;
        }

        public static string? GetLanguageFromIso3(string iso3Code)
        {
            var country = GetCountryByIso3(iso3Code);
            return country?.LanguageCode;
        }

        public static bool IsValidIso2Code(string iso2Code)
        {
            return !string.IsNullOrWhiteSpace(iso2Code) && 
                   _countries.ContainsKey(iso2Code.ToUpperInvariant().Trim());
        }

        public static bool IsValidIso3Code(string iso3Code)
        {
            return !string.IsNullOrWhiteSpace(iso3Code) && 
                   _iso3ToIso2.ContainsKey(iso3Code.ToUpperInvariant().Trim());
        }

        public static bool IsValidPhoneCode(string phoneCode)
        {
            return !string.IsNullOrWhiteSpace(phoneCode) && 
                   _phoneToIso2.ContainsKey(phoneCode.Trim());
        }

        public static bool IsValidCurrencyCode(string currencyCode)
        {
            return !string.IsNullOrWhiteSpace(currencyCode) && 
                   _currencyToCountries.ContainsKey(currencyCode.ToUpperInvariant().Trim());
        }

        public static IEnumerable<CountryInfo> GetAllCountries()
        {
            return _countries.Values.OrderBy(c => c.Name);
        }

        public static IEnumerable<CountryInfo> GetCountriesByRegion(string region)
        {
            var normalizedRegion = region.ToLowerInvariant().Trim();
            return _countries.Values
                .Where(c => GetRegion(c.Iso2).ToLowerInvariant().Contains(normalizedRegion))
                .OrderBy(c => c.Name);
        }

        public static IEnumerable<CountryInfo> GetCountriesByCurrency(string currencyCode)
        {
            var normalizedCurrency = currencyCode.ToUpperInvariant().Trim();
            return _countries.Values
                .Where(c => c.CurrencyCode.Equals(normalizedCurrency, StringComparison.OrdinalIgnoreCase))
                .OrderBy(c => c.Name);
        }

        public static IEnumerable<CountryInfo> GetCountriesByLanguage(string languageCode)
        {
            var normalizedLanguage = languageCode.ToLowerInvariant().Trim();
            return _countries.Values
                .Where(c => c.LanguageCode.Equals(normalizedLanguage, StringComparison.OrdinalIgnoreCase))
                .OrderBy(c => c.Name);
        }

        public static IEnumerable<CountryInfo> SearchCountries(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return Enumerable.Empty<CountryInfo>();

            var normalizedTerm = searchTerm.ToLowerInvariant().Trim();
            return _countries.Values
                .Where(c => c.Name.ToLowerInvariant().Contains(normalizedTerm) ||
                           c.Iso2.ToLowerInvariant().Contains(normalizedTerm) ||
                           c.Iso3.ToLowerInvariant().Contains(normalizedTerm) ||
                           c.Capital.ToLowerInvariant().Contains(normalizedTerm))
                .OrderBy(c => c.Name);
        }

        public static string GetRegion(string iso2Code)
        {
            var country = GetCountryByIso2(iso2Code);
            if (country == null)
                return "Unknown";

            return country.Iso2 switch
            {
                // UEMOA
                "BF" or "BJ" or "CI" or "GN" or "ML" or "NE" or "SN" or "TG" => "UEMOA",
                
                // CEMAC
                "CM" or "CF" or "TD" or "CG" or "GA" or "GQ" => "CEMAC",
                
                // Maghreb
                "MA" or "DZ" or "TN" => "Maghreb",
                
                // Afrique du Nord
                "EG" or "LY" => "Afrique du Nord",
                
                // Afrique de l'Est
                "KE" or "UG" or "TZ" or "RW" or "BI" or "ET" or "SO" or "DJ" or "ER" => "Afrique de l'Est",
                
                // Afrique Australe
                "ZA" or "ZW" or "ZM" or "MW" or "BW" or "NA" or "SZ" or "LS" => "Afrique Australe",
                
                // Afrique Centrale
                "CD" or "AO" or "MZ" => "Afrique Centrale",
                
                // Afrique de l'Ouest (non-UEMOA)
                "NG" or "GH" or "LR" or "SL" or "GM" or "GW" or "CV" or "ST" => "Afrique de l'Ouest",
                
                // Océan Indien
                "MU" or "SC" or "KM" or "MG" => "Océan Indien",
                
                // Europe
                "FR" or "BE" or "CH" or "DE" or "IT" or "ES" or "PT" or "NL" or "LU" or "GB" or "IE" => "Europe",
                
                // Amérique du Nord
                "US" or "CA" or "MX" => "Amérique du Nord",
                
                // Amérique du Sud
                "BR" or "AR" => "Amérique du Sud",
                
                // Asie
                "CN" or "JP" or "IN" or "KR" => "Asie",
                
                // Moyen-Orient
                "SA" or "AE" or "TR" => "Moyen-Orient",
                
                _ => "Autre"
            };
        }

        public static bool IsInUEMOA(string iso2Code)
        {
            var region = GetRegion(iso2Code);
            return region.Equals("UEMOA", StringComparison.OrdinalIgnoreCase);
        }

        public static bool IsInCEMAC(string iso2Code)
        {
            var region = GetRegion(iso2Code);
            return region.Equals("CEMAC", StringComparison.OrdinalIgnoreCase);
        }

        public static bool IsInWestAfrica(string iso2Code)
        {
            var region = GetRegion(iso2Code);
            return region.Equals("UEMOA", StringComparison.OrdinalIgnoreCase) || 
                   region.Equals("Afrique de l'Ouest", StringComparison.OrdinalIgnoreCase);
        }

        public static bool IsInCentralAfrica(string iso2Code)
        {
            var region = GetRegion(iso2Code);
            return region.Equals("CEMAC", StringComparison.OrdinalIgnoreCase) || 
                   region.Equals("Afrique Centrale", StringComparison.OrdinalIgnoreCase);
        }

        public static bool IsInAfrica(string iso2Code)
        {
            var region = GetRegion(iso2Code);
            return !region.Equals("Europe") && 
                   !region.Equals("Amérique du Nord") && 
                   !region.Equals("Amérique du Sud") && 
                   !region.Equals("Asie") && 
                   !region.Equals("Moyen-Orient") &&
                   !region.Equals("Autre");
        }

        public static bool IsFrancophone(string iso2Code)
        {
            var country = GetCountryByIso2(iso2Code);
            return country?.LanguageCode.Equals("fr", StringComparison.OrdinalIgnoreCase) ?? false;
        }

        public static bool IsAnglophone(string iso2Code)
        {
            var country = GetCountryByIso2(iso2Code);
            return country?.LanguageCode.Equals("en", StringComparison.OrdinalIgnoreCase) ?? false;
        }

        public static bool IsLusophone(string iso2Code)
        {
            var country = GetCountryByIso2(iso2Code);
            return country?.LanguageCode.Equals("pt", StringComparison.OrdinalIgnoreCase) ?? false;
        }

        public static bool IsArabophone(string iso2Code)
        {
            var country = GetCountryByIso2(iso2Code);
            return country?.LanguageCode.Equals("ar", StringComparison.OrdinalIgnoreCase) ?? false;
        }

        public static bool UsesXOF(string iso2Code)
        {
            var country = GetCountryByIso2(iso2Code);
            return country?.CurrencyCode.Equals("XOF", StringComparison.OrdinalIgnoreCase) ?? false;
        }

        public static bool UsesXAF(string iso2Code)
        {
            var country = GetCountryByIso2(iso2Code);
            return country?.CurrencyCode.Equals("XAF", StringComparison.OrdinalIgnoreCase) ?? false;
        }

        public static bool UsesEuro(string iso2Code)
        {
            var country = GetCountryByIso2(iso2Code);
            return country?.CurrencyCode.Equals("EUR", StringComparison.OrdinalIgnoreCase) ?? false;
        }

        public static bool UsesUSD(string iso2Code)
        {
            var country = GetCountryByIso2(iso2Code);
            return country?.CurrencyCode.Equals("USD", StringComparison.OrdinalIgnoreCase) ?? false;
        }

        public static CultureInfo GetCultureInfo(string iso2Code)
        {
            var country = GetCountryByIso2(iso2Code);
            if (country == null)
                return CultureInfo.InvariantCulture;

            return country.LanguageCode.ToLowerInvariant() switch
            {
                "fr" => new CultureInfo("fr-FR"),
                "en" => new CultureInfo("en-US"),
                "pt" => new CultureInfo("pt-PT"),
                "ar" => new CultureInfo("ar-MA"),
                "es" => new CultureInfo("es-ES"),
                "de" => new CultureInfo("de-DE"),
                "it" => new CultureInfo("it-IT"),
                "nl" => new CultureInfo("nl-NL"),
                "zh" => new CultureInfo("zh-CN"),
                "ja" => new CultureInfo("ja-JP"),
                "ko" => new CultureInfo("ko-KR"),
                "hi" => new CultureInfo("hi-IN"),
                "sw" => new CultureInfo("sw-KE"),
                "am" => new CultureInfo("am-ET"),
                "ti" => new CultureInfo("ti-ER"),
                "so" => new CultureInfo("so-SO"),
                "mg" => new CultureInfo("mg-MG"),
                "tr" => new CultureInfo("tr-TR"),
                _ => CultureInfo.InvariantCulture
            };
        }

        public static string GetCurrencySymbol(string iso2Code)
        {
            var country = GetCountryByIso2(iso2Code);
            if (country == null)
                return "";

            var culture = GetCultureInfo(iso2Code);
            return culture.NumberFormat.CurrencySymbol;
        }

        public static string FormatCurrency(decimal amount, string iso2Code)
        {
            var culture = GetCultureInfo(iso2Code);
            return amount.ToString("C", culture);
        }

        public static string FormatNumber(decimal number, string iso2Code)
        {
            var culture = GetCultureInfo(iso2Code);
            return number.ToString("N", culture);
        }

        public static string FormatDate(DateTime date, string iso2Code)
        {
            var culture = GetCultureInfo(iso2Code);
            return date.ToString("d", culture);
        }

        public static string GetTimeZone(string iso2Code)
        {
            return iso2Code.ToUpperInvariant() switch
            {
                // Afrique de l'Ouest (UTC+0)
                "BF" or "BJ" or "CI" or "GN" or "ML" or "NE" or "SN" or "TG" or "GH" or "SL" or "GM" or "GW" or "LR" => "UTC+0",
                
                // Afrique de l'Ouest (UTC+1)
                "NG" => "UTC+1",
                
                // Afrique Centrale (UTC+1)
                "CM" or "CF" or "TD" or "CG" or "GA" or "GQ" or "CD" or "AO" => "UTC+1",
                
                // Afrique de l'Est (UTC+2)
                "KE" or "UG" or "RW" or "BI" or "TZ" or "ET" or "DJ" => "UTC+3",
                
                // Afrique de l'Est (UTC+3)
                "SO" => "UTC+3",
                
                // Afrique Australe (UTC+2)
                "ZA" or "ZW" or "ZM" or "MW" => "UTC+2",
                
                // Afrique Australe (UTC+2)
                "BW" or "NA" => "UTC+2",
                
                // Afrique Australe (UTC+2)
                "SZ" or "LS" => "UTC+2",
                
                // Océan Indien (UTC+3)
                "MU" or "SC" => "UTC+4",
                
                // Océan Indien (UTC+3)
                "KM" => "UTC+3",
                
                // Océan Indien (UTC+3)
                "MG" => "UTC+3",
                
                // Maghreb (UTC+1)
                "MA" or "DZ" or "TN" => "UTC+1",
                
                // Afrique du Nord (UTC+2)
                "EG" => "UTC+2",
                
                // Europe (UTC+1)
                "FR" or "DE" or "CH" or "IT" or "ES" or "NL" or "BE" or "LU" => "UTC+1",
                
                // Europe (UTC+0)
                "GB" or "IE" or "PT" => "UTC+0",
                
                // Amérique du Nord
                "US" => "Multiple (EST, CST, MST, PST)",
                "CA" => "Multiple (EST, CST, MST, PST)",
                "MX" => "UTC-6 to UTC-8",
                
                // Amérique du Sud
                "BR" => "UTC-3 to UTC-5",
                "AR" => "UTC-3",
                
                // Asie
                "CN" => "UTC+8",
                "JP" => "UTC+9",
                "KR" => "UTC+9",
                "IN" => "UTC+5:30",
                
                // Moyen-Orient
                "SA" => "UTC+3",
                "AE" => "UTC+4",
                "TR" => "UTC+3",
                
                _ => "Unknown"
            };
        }

        public static bool IsSameTimeZone(string iso2Code1, string iso2Code2)
        {
            var timezone1 = GetTimeZone(iso2Code1);
            var timezone2 = GetTimeZone(iso2Code2);
            return timezone1.Equals(timezone2, StringComparison.OrdinalIgnoreCase);
        }

        public static TimeSpan GetTimeOffset(string iso2Code)
        {
            var timezone = GetTimeZone(iso2Code);
            
            return timezone switch
            {
                "UTC+0" => TimeSpan.FromHours(0),
                "UTC+1" => TimeSpan.FromHours(1),
                "UTC+2" => TimeSpan.FromHours(2),
                "UTC+3" => TimeSpan.FromHours(3),
                "UTC+4" => TimeSpan.FromHours(4),
                "UTC+5:30" => TimeSpan.FromHours(5.5),
                "UTC+8" => TimeSpan.FromHours(8),
                "UTC+9" => TimeSpan.FromHours(9),
                "UTC-3" => TimeSpan.FromHours(-3),
                "UTC-5" => TimeSpan.FromHours(-5),
                "UTC-6" => TimeSpan.FromHours(-6),
                "UTC-8" => TimeSpan.FromHours(-8),
                _ => TimeSpan.Zero
            };
        }

        public static DateTime ConvertToCountryTime(DateTime utcDateTime, string iso2Code)
        {
            var offset = GetTimeOffset(iso2Code);
            return utcDateTime.Add(offset);
        }

        public static DateTime ConvertFromCountryTime(DateTime localDateTime, string iso2Code)
        {
            var offset = GetTimeOffset(iso2Code);
            return localDateTime.Subtract(offset);
        }
    }

    public class CountryInfo
    {
        public string Iso2 { get; set; } = string.Empty;
        public string Iso3 { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string PhoneCode { get; set; } = string.Empty;
        public string CurrencyCode { get; set; } = string.Empty;
        public string Capital { get; set; } = string.Empty;
        public string LanguageCode { get; set; } = string.Empty;

        public CountryInfo(string iso2, string iso3, string name, string phoneCode, string currencyCode, string capital, string languageCode)
        {
            Iso2 = iso2;
            Iso3 = iso3;
            Name = name;
            PhoneCode = phoneCode;
            CurrencyCode = currencyCode;
            Capital = capital;
            LanguageCode = languageCode;
        }

        public override string ToString()
        {
            return $"{Name} ({Iso2})";
        }
    }

    public static class RegionInfo
    {
        public static readonly string[] UEMOA_Countries = { "BF", "BJ", "CI", "GN", "ML", "NE", "SN", "TG" };
        public static readonly string[] CEMAC_Countries = { "CM", "CF", "TD", "CG", "GA", "GQ" };
        public static readonly string[] Maghreb_Countries = { "MA", "DZ", "TN", "LY" };
        public static readonly string[] WestAfrica_Countries = { "BF", "BJ", "CI", "GN", "ML", "NE", "SN", "TG", "NG", "GH", "LR", "SL", "GM", "GW", "CV", "ST" };
        public static readonly string[] CentralAfrica_Countries = { "CM", "CF", "TD", "CG", "GA", "GQ", "CD", "AO" };
        public static readonly string[] EastAfrica_Countries = { "KE", "UG", "TZ", "RW", "BI", "ET", "SO", "DJ", "ER" };
        public static readonly string[] SouthernAfrica_Countries = { "ZA", "ZW", "ZM", "MW", "BW", "NA", "SZ", "LS" };
        public static readonly string[] Francophone_Countries = { "BF", "BJ", "CI", "GN", "ML", "NE", "SN", "TG", "CM", "CF", "TD", "CG", "GA", "GQ", "CD", "MA", "DZ", "TN", "FR", "BE", "CH", "LU" };
        public static readonly string[] Anglophone_Countries = { "NG", "GH", "LR", "SL", "GM", "KE", "UG", "TZ", "RW", "ET", "ZA", "ZW", "ZM", "MW", "BW", "NA", "SZ", "LS", "GB", "IE", "US", "CA" };
        public static readonly string[] Lusophone_Countries = { "GW", "CV", "ST", "PT", "AO", "MZ", "BR" };
        public static readonly string[] Arabophone_Countries = { "MA", "DZ", "TN", "LY", "EG", "SA", "AE", "TD", "DJ", "KM", "SO" };
    }
}
