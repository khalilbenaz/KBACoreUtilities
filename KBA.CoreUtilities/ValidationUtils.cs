using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace KBA.CoreUtilities.Utilities
{
    /// <summary>
    /// Provides advanced validation utilities for financial and business data
    /// </summary>
    public static class ValidationUtils
    {
        #region IBAN Validation

        /// <summary>
        /// Validates an IBAN (International Bank Account Number)
        /// </summary>
        public static bool IsValidIban(string iban)
        {
            if (string.IsNullOrWhiteSpace(iban))
                return false;

            // Remove spaces and convert to uppercase
            iban = iban.Replace(" ", "").ToUpperInvariant();

            // IBAN must be between 15 and 34 characters
            if (iban.Length < 15 || iban.Length > 34)
                return false;

            // First two characters must be letters (country code)
            if (!char.IsLetter(iban[0]) || !char.IsLetter(iban[1]))
                return false;

            // Next two characters must be digits (check digits)
            if (!char.IsDigit(iban[2]) || !char.IsDigit(iban[3]))
                return false;

            // Rearrange: move first 4 characters to end
            var rearranged = iban.Substring(4) + iban.Substring(0, 4);

            // Convert letters to numbers (A=10, B=11, ..., Z=35)
            var numericString = string.Concat(rearranged.Select(c =>
                char.IsLetter(c) ? ((int)c - 55).ToString() : c.ToString()));

            // Calculate mod 97
            return Mod97(numericString) == 1;
        }

        /// <summary>
        /// Formats an IBAN with spaces for readability
        /// </summary>
        public static string FormatIban(string iban)
        {
            if (string.IsNullOrWhiteSpace(iban))
                return iban;

            iban = iban.Replace(" ", "").ToUpperInvariant();
            
            if (!IsValidIban(iban))
                return iban;

            // Format with spaces every 4 characters
            return Regex.Replace(iban, ".{4}", "$0 ").Trim();
        }

        /// <summary>
        /// Gets the country code from an IBAN
        /// </summary>
        public static string GetIbanCountryCode(string iban)
        {
            if (string.IsNullOrWhiteSpace(iban))
                return string.Empty;

            iban = iban.Replace(" ", "").ToUpperInvariant();
            
            if (iban.Length >= 2)
                return iban.Substring(0, 2);

            return string.Empty;
        }

        private static int Mod97(string number)
        {
            var remainder = 0;
            foreach (var digit in number)
            {
                remainder = (remainder * 10 + (digit - '0')) % 97;
            }
            return remainder;
        }

        #endregion

        #region Credit Card Validation

        /// <summary>
        /// Validates a credit card number using Luhn algorithm
        /// </summary>
        public static bool IsValidCreditCard(string cardNumber)
        {
            if (string.IsNullOrWhiteSpace(cardNumber))
                return false;

            // Remove spaces and dashes
            cardNumber = cardNumber.Replace(" ", "").Replace("-", "");

            // Must be digits only
            if (!cardNumber.All(char.IsDigit))
                return false;

            // Must be between 13 and 19 digits
            if (cardNumber.Length < 13 || cardNumber.Length > 19)
                return false;

            // Luhn algorithm
            var sum = 0;
            var alternate = false;

            for (var i = cardNumber.Length - 1; i >= 0; i--)
            {
                var digit = cardNumber[i] - '0';

                if (alternate)
                {
                    digit *= 2;
                    if (digit > 9)
                        digit -= 9;
                }

                sum += digit;
                alternate = !alternate;
            }

            return sum % 10 == 0;
        }

        /// <summary>
        /// Gets the credit card type (Visa, MasterCard, Amex, etc.)
        /// </summary>
        public static string GetCreditCardType(string cardNumber)
        {
            if (string.IsNullOrWhiteSpace(cardNumber))
                return "Unknown";

            cardNumber = cardNumber.Replace(" ", "").Replace("-", "");

            if (!cardNumber.All(char.IsDigit))
                return "Unknown";

            // Visa: starts with 4
            if (cardNumber.StartsWith("4"))
                return "Visa";

            // MasterCard: starts with 51-55 or 2221-2720
            if (Regex.IsMatch(cardNumber, "^5[1-5]") || 
                (cardNumber.Length >= 4 && int.TryParse(cardNumber.Substring(0, 4), out var first4) && first4 >= 2221 && first4 <= 2720))
                return "MasterCard";

            // American Express: starts with 34 or 37
            if (cardNumber.StartsWith("34") || cardNumber.StartsWith("37"))
                return "American Express";

            // Discover: starts with 6011, 622126-622925, 644-649, 65
            if (cardNumber.StartsWith("6011") || cardNumber.StartsWith("65") ||
                Regex.IsMatch(cardNumber, "^64[4-9]") ||
                (cardNumber.Length >= 6 && int.TryParse(cardNumber.Substring(0, 6), out var first6) && first6 >= 622126 && first6 <= 622925))
                return "Discover";

            // Diners Club: starts with 36 or 38 or 300-305
            if (cardNumber.StartsWith("36") || cardNumber.StartsWith("38") ||
                (cardNumber.Length >= 3 && int.TryParse(cardNumber.Substring(0, 3), out var first3) && first3 >= 300 && first3 <= 305))
                return "Diners Club";

            // JCB: starts with 3528-3589
            if (cardNumber.Length >= 4 && int.TryParse(cardNumber.Substring(0, 4), out var jcbFirst4) && jcbFirst4 >= 3528 && jcbFirst4 <= 3589)
                return "JCB";

            return "Unknown";
        }

        /// <summary>
        /// Masks a credit card number (shows only last 4 digits)
        /// </summary>
        public static string MaskCreditCard(string cardNumber, char maskChar = '*')
        {
            if (string.IsNullOrWhiteSpace(cardNumber))
                return cardNumber;

            cardNumber = cardNumber.Replace(" ", "").Replace("-", "");

            if (cardNumber.Length <= 4)
                return cardNumber;

            return new string(maskChar, cardNumber.Length - 4) + cardNumber.Substring(cardNumber.Length - 4);
        }

        #endregion

        #region BIC/SWIFT Validation

        /// <summary>
        /// Validates a BIC/SWIFT code
        /// </summary>
        public static bool IsValidBic(string bic)
        {
            if (string.IsNullOrWhiteSpace(bic))
                return false;

            bic = bic.Replace(" ", "").ToUpperInvariant();

            // BIC must be 8 or 11 characters
            if (bic.Length != 8 && bic.Length != 11)
                return false;

            // Format: AAAABBCCXXX
            // AAAA: Bank code (4 letters)
            // BB: Country code (2 letters)
            // CC: Location code (2 letters or digits)
            // XXX: Branch code (3 letters or digits) - optional

            // First 4 must be letters
            if (!bic.Substring(0, 4).All(char.IsLetter))
                return false;

            // Next 2 must be letters (country code)
            if (!bic.Substring(4, 2).All(char.IsLetter))
                return false;

            // Next 2 must be alphanumeric
            if (!bic.Substring(6, 2).All(char.IsLetterOrDigit))
                return false;

            // If 11 characters, last 3 must be alphanumeric
            if (bic.Length == 11 && !bic.Substring(8, 3).All(char.IsLetterOrDigit))
                return false;

            return true;
        }

        /// <summary>
        /// Gets the country code from a BIC/SWIFT code
        /// </summary>
        public static string GetBicCountryCode(string bic)
        {
            if (string.IsNullOrWhiteSpace(bic))
                return string.Empty;

            bic = bic.Replace(" ", "").ToUpperInvariant();

            if (bic.Length >= 6)
                return bic.Substring(4, 2);

            return string.Empty;
        }

        #endregion

        #region VAT Number Validation

        /// <summary>
        /// Validates a European VAT number
        /// </summary>
        public static bool IsValidVatNumber(string vatNumber, string countryCode = null)
        {
            if (string.IsNullOrWhiteSpace(vatNumber))
                return false;

            vatNumber = vatNumber.Replace(" ", "").Replace("-", "").Replace(".", "").ToUpperInvariant();

            // If no country code provided, try to extract it
            if (string.IsNullOrWhiteSpace(countryCode) && vatNumber.Length >= 2)
            {
                if (char.IsLetter(vatNumber[0]) && char.IsLetter(vatNumber[1]))
                {
                    countryCode = vatNumber.Substring(0, 2);
                    vatNumber = vatNumber.Substring(2);
                }
            }

            if (string.IsNullOrWhiteSpace(countryCode))
                return false;

            countryCode = countryCode.ToUpperInvariant();

            // Basic format validation by country
            switch (countryCode)
            {
                case "FR": // France: FR + 11 digits (2 letters/digits + 9 digits)
                    return Regex.IsMatch(vatNumber, @"^[A-Z0-9]{2}\d{9}$");
                
                case "DE": // Germany: DE + 9 digits
                    return Regex.IsMatch(vatNumber, @"^\d{9}$");
                
                case "ES": // Spain: ES + letter/digit + 7-8 digits + letter/digit
                    return Regex.IsMatch(vatNumber, @"^[A-Z0-9]\d{7,8}[A-Z0-9]$");
                
                case "IT": // Italy: IT + 11 digits
                    return Regex.IsMatch(vatNumber, @"^\d{11}$");
                
                case "GB": // UK: GB + 9 or 12 digits
                    return Regex.IsMatch(vatNumber, @"^\d{9}$") || Regex.IsMatch(vatNumber, @"^\d{12}$");
                
                case "BE": // Belgium: BE + 10 digits (starting with 0 or 1)
                    return Regex.IsMatch(vatNumber, @"^[01]\d{9}$");
                
                case "NL": // Netherlands: NL + 9 digits + B + 2 digits
                    return Regex.IsMatch(vatNumber, @"^\d{9}B\d{2}$");
                
                case "PT": // Portugal: PT + 9 digits
                    return Regex.IsMatch(vatNumber, @"^\d{9}$");
                
                case "AT": // Austria: AT + U + 8 digits
                    return Regex.IsMatch(vatNumber, @"^U\d{8}$");
                
                case "SE": // Sweden: SE + 12 digits
                    return Regex.IsMatch(vatNumber, @"^\d{12}$");
                
                case "DK": // Denmark: DK + 8 digits
                    return Regex.IsMatch(vatNumber, @"^\d{8}$");
                
                case "FI": // Finland: FI + 8 digits
                    return Regex.IsMatch(vatNumber, @"^\d{8}$");
                
                case "IE": // Ireland: IE + 7 digits + letter or 7 digits + letter + letter/digit
                    return Regex.IsMatch(vatNumber, @"^\d{7}[A-Z]$") || Regex.IsMatch(vatNumber, @"^\d{7}[A-Z][A-Z0-9]$");
                
                case "LU": // Luxembourg: LU + 8 digits
                    return Regex.IsMatch(vatNumber, @"^\d{8}$");
                
                case "PL": // Poland: PL + 10 digits
                    return Regex.IsMatch(vatNumber, @"^\d{10}$");
                
                default:
                    // For other countries, just check if it contains digits
                    return vatNumber.Any(char.IsDigit);
            }
        }

        #endregion

        #region SSN/Tax ID Validation

        /// <summary>
        /// Validates a US Social Security Number
        /// </summary>
        public static bool IsValidUsSSN(string ssn)
        {
            if (string.IsNullOrWhiteSpace(ssn))
                return false;

            ssn = ssn.Replace("-", "").Replace(" ", "");

            // Must be exactly 9 digits
            if (!Regex.IsMatch(ssn, @"^\d{9}$"))
                return false;

            // Cannot be all zeros in any group
            if (ssn.StartsWith("000") || ssn.Substring(3, 2) == "00" || ssn.Substring(5, 4) == "0000")
                return false;

            // Cannot be 666 in first group
            if (ssn.StartsWith("666"))
                return false;

            // Cannot start with 9 (reserved for ITIN)
            if (ssn.StartsWith("9"))
                return false;

            return true;
        }

        /// <summary>
        /// Validates a US Employer Identification Number (EIN)
        /// </summary>
        public static bool IsValidUsEIN(string ein)
        {
            if (string.IsNullOrWhiteSpace(ein))
                return false;

            ein = ein.Replace("-", "").Replace(" ", "");

            // Must be exactly 9 digits
            if (!Regex.IsMatch(ein, @"^\d{9}$"))
                return false;

            // First two digits must be valid campus/area numbers
            var prefix = int.Parse(ein.Substring(0, 2));
            
            // Valid prefixes: 01-06, 10-16, 20-27, 30-39, 40-48, 50-68, 71-77, 80-88, 90-95, 98-99
            return (prefix >= 1 && prefix <= 6) ||
                   (prefix >= 10 && prefix <= 16) ||
                   (prefix >= 20 && prefix <= 27) ||
                   (prefix >= 30 && prefix <= 39) ||
                   (prefix >= 40 && prefix <= 48) ||
                   (prefix >= 50 && prefix <= 68) ||
                   (prefix >= 71 && prefix <= 77) ||
                   (prefix >= 80 && prefix <= 88) ||
                   (prefix >= 90 && prefix <= 95) ||
                   (prefix >= 98 && prefix <= 99);
        }

        #endregion

        #region ISBN Validation

        /// <summary>
        /// Validates an ISBN-10 or ISBN-13 number
        /// </summary>
        public static bool IsValidIsbn(string isbn)
        {
            if (string.IsNullOrWhiteSpace(isbn))
                return false;

            isbn = isbn.Replace("-", "").Replace(" ", "").ToUpperInvariant();

            if (isbn.Length == 10)
                return IsValidIsbn10(isbn);
            
            if (isbn.Length == 13)
                return IsValidIsbn13(isbn);

            return false;
        }

        private static bool IsValidIsbn10(string isbn)
        {
            if (isbn.Length != 10)
                return false;

            var sum = 0;
            for (var i = 0; i < 9; i++)
            {
                if (!char.IsDigit(isbn[i]))
                    return false;
                
                sum += (isbn[i] - '0') * (10 - i);
            }

            // Last character can be 'X' (representing 10)
            var lastChar = isbn[9];
            if (lastChar == 'X')
                sum += 10;
            else if (char.IsDigit(lastChar))
                sum += lastChar - '0';
            else
                return false;

            return sum % 11 == 0;
        }

        private static bool IsValidIsbn13(string isbn)
        {
            if (isbn.Length != 13 || !isbn.All(char.IsDigit))
                return false;

            var sum = 0;
            for (var i = 0; i < 12; i++)
            {
                var digit = isbn[i] - '0';
                sum += (i % 2 == 0) ? digit : digit * 3;
            }

            var checkDigit = (10 - (sum % 10)) % 10;
            return checkDigit == (isbn[12] - '0');
        }

        #endregion

        #region Additional Validations

        /// <summary>
        /// Validates a MAC address
        /// </summary>
        public static bool IsValidMacAddress(string macAddress)
        {
            if (string.IsNullOrWhiteSpace(macAddress))
                return false;

            // Accept formats: XX:XX:XX:XX:XX:XX or XX-XX-XX-XX-XX-XX
            return Regex.IsMatch(macAddress, @"^([0-9A-Fa-f]{2}[:-]){5}([0-9A-Fa-f]{2})$");
        }

        /// <summary>
        /// Validates an IPv4 address
        /// </summary>
        public static bool IsValidIPv4(string ipAddress)
        {
            if (string.IsNullOrWhiteSpace(ipAddress))
                return false;

            var parts = ipAddress.Split('.');
            if (parts.Length != 4)
                return false;

            foreach (var part in parts)
            {
                if (!int.TryParse(part, out var num) || num < 0 || num > 255)
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Validates an IPv6 address
        /// </summary>
        public static bool IsValidIPv6(string ipAddress)
        {
            if (string.IsNullOrWhiteSpace(ipAddress))
                return false;

            return Regex.IsMatch(ipAddress, 
                @"^(([0-9a-fA-F]{1,4}:){7,7}[0-9a-fA-F]{1,4}|([0-9a-fA-F]{1,4}:){1,7}:|([0-9a-fA-F]{1,4}:){1,6}:[0-9a-fA-F]{1,4}|([0-9a-fA-F]{1,4}:){1,5}(:[0-9a-fA-F]{1,4}){1,2}|([0-9a-fA-F]{1,4}:){1,4}(:[0-9a-fA-F]{1,4}){1,3}|([0-9a-fA-F]{1,4}:){1,3}(:[0-9a-fA-F]{1,4}){1,4}|([0-9a-fA-F]{1,4}:){1,2}(:[0-9a-fA-F]{1,4}){1,5}|[0-9a-fA-F]{1,4}:((:[0-9a-fA-F]{1,4}){1,6})|:((:[0-9a-fA-F]{1,4}){1,7}|:)|fe80:(:[0-9a-fA-F]{0,4}){0,4}%[0-9a-zA-Z]{1,}|::(ffff(:0{1,4}){0,1}:){0,1}((25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])\.){3,3}(25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])|([0-9a-fA-F]{1,4}:){1,4}:((25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])\.){3,3}(25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9]))$");
        }

        /// <summary>
        /// Validates a hexadecimal color code
        /// </summary>
        public static bool IsValidHexColor(string color)
        {
            if (string.IsNullOrWhiteSpace(color))
                return false;

            return Regex.IsMatch(color, @"^#?([0-9A-Fa-f]{3}|[0-9A-Fa-f]{6})$");
        }

        /// <summary>
        /// Validates a strong password (min 8 chars, uppercase, lowercase, digit, special char)
        /// </summary>
        public static bool IsStrongPassword(string password, int minLength = 8)
        {
            if (string.IsNullOrWhiteSpace(password) || password.Length < minLength)
                return false;

            var hasUpperCase = password.Any(char.IsUpper);
            var hasLowerCase = password.Any(char.IsLower);
            var hasDigit = password.Any(char.IsDigit);
            var hasSpecialChar = password.Any(c => !char.IsLetterOrDigit(c));

            return hasUpperCase && hasLowerCase && hasDigit && hasSpecialChar;
        }

        #endregion
    }
}
