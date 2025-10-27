using System.Text;
using System.Text.RegularExpressions;

namespace KBA.CoreUtilities.Utilities
{
    public static class StringUtils
    {
        private static readonly Regex PhoneRegex = new Regex(@"[^\d+]", RegexOptions.Compiled);
        private static readonly Regex NumberRegex = new Regex(@"[^\d.,-]", RegexOptions.Compiled);
        private static readonly Regex WhitespaceRegex = new Regex(@"\s+", RegexOptions.Compiled);
        private static readonly Regex EmailRegex = new Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", RegexOptions.Compiled);

        public static string RemoveWhitespace(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            return WhitespaceRegex.Replace(input.Trim(), "");
        }

        public static string NormalizeWhitespace(string input, string replacement = " ")
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            return WhitespaceRegex.Replace(input.Trim(), replacement);
        }

        public static string RemoveSpecialCharacters(string input, bool keepSpaces = false)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            var pattern = keepSpaces ? @"[^a-zA-Z0-9\s]" : @"[^a-zA-Z0-9]";
            return Regex.Replace(input, pattern, "");
        }

        public static string RemoveDiacritics(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            var normalizedString = input.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != System.Globalization.UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        public static string ExtractNumbers(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            return new string(input.Where(char.IsDigit).ToArray());
        }

        public static string ExtractLetters(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            return new string(input.Where(char.IsLetter).ToArray());
        }

        public static string ExtractAlphanumeric(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            return new string(input.Where(char.IsLetterOrDigit).ToArray());
        }

        public static string CleanPhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return string.Empty;

            return PhoneRegex.Replace(phoneNumber.Trim(), "");
        }

        public static string FormatPhoneNumber(string phoneNumber, string format = "### ### ### ##")
        {
            var cleanNumber = CleanPhoneNumber(phoneNumber);
            if (string.IsNullOrWhiteSpace(cleanNumber))
                return string.Empty;

            var digits = cleanNumber.ToCharArray();
            var result = new StringBuilder();
            var digitIndex = 0;

            foreach (var c in format)
            {
                if (c == '#')
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

        public static string MaskString(string input, int visibleChars = 4, char maskChar = '*')
        {
            if (string.IsNullOrWhiteSpace(input))
                return input;

            if (input.Length <= visibleChars)
                return new string(maskChar, input.Length);

            var visible = input.Substring(0, visibleChars);
            var masked = new string(maskChar, input.Length - visibleChars);
            return visible + masked;
        }

        public static string MaskStringMiddle(string input, int startVisible = 2, int endVisible = 2, char maskChar = '*')
        {
            if (string.IsNullOrWhiteSpace(input))
                return input;

            if (input.Length <= startVisible + endVisible)
                return new string(maskChar, input.Length);

            var start = input.Substring(0, startVisible);
            var end = input.Substring(input.Length - endVisible);
            var middle = new string(maskChar, input.Length - startVisible - endVisible);

            return start + middle + end;
        }

        public static string CapitalizeFirstLetter(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return input;

            return char.ToUpper(input[0]) + input.Substring(1).ToLower();
        }

        public static string CapitalizeWords(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return input;

            var words = input.ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < words.Length; i++)
            {
                if (words[i].Length > 0)
                {
                    words[i] = char.ToUpper(words[i][0]) + words[i].Substring(1);
                }
            }

            return string.Join(" ", words);
        }

        public static string ToTitleCase(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return input;

            var textInfo = new System.Globalization.CultureInfo("en-US", false).TextInfo;
            return textInfo.ToTitleCase(input.ToLower());
        }

        public static string ReverseString(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return input;

            return new string(input.Reverse().ToArray());
        }

        public static bool IsPalindrome(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return false;

            var cleaned = RemoveSpecialCharacters(input).ToLower();
            var reversed = ReverseString(cleaned);
            return cleaned == reversed;
        }

        public static string Truncate(string input, int maxLength, string suffix = "...")
        {
            if (string.IsNullOrWhiteSpace(input) || input.Length <= maxLength)
                return input;

            return input.Substring(0, maxLength - suffix.Length) + suffix;
        }

        public static string TruncateWords(string input, int maxWords, string suffix = "...")
        {
            if (string.IsNullOrWhiteSpace(input))
                return input;

            var words = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (words.Length <= maxWords)
                return input;

            return string.Join(" ", words.Take(maxWords)) + suffix;
        }

        public static string PadLeft(string input, int totalLength, char paddingChar = '0')
        {
            return input.PadLeft(totalLength, paddingChar);
        }

        public static string PadRight(string input, int totalLength, char paddingChar = '0')
        {
            return input.PadRight(totalLength, paddingChar);
        }

        public static string Left(string input, int length)
        {
            if (string.IsNullOrWhiteSpace(input) || length >= input.Length)
                return input;

            return input.Substring(0, length);
        }

        public static string Right(string input, int length)
        {
            if (string.IsNullOrWhiteSpace(input) || length >= input.Length)
                return input;

            return input.Substring(input.Length - length);
        }

        public static string Mid(string input, int startIndex, int length)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            if (startIndex < 0)
                startIndex = 0;

            if (startIndex >= input.Length)
                return string.Empty;

            if (startIndex + length > input.Length)
                length = input.Length - startIndex;

            return input.Substring(startIndex, length);
        }

        public static string InsertAt(string input, int index, string insertText)
        {
            if (string.IsNullOrWhiteSpace(input))
                return insertText ?? string.Empty;

            if (index < 0)
                index = 0;

            if (index > input.Length)
                index = input.Length;

            return input.Insert(index, insertText);
        }

        public static string RemoveAt(string input, int startIndex, int length)
        {
            if (string.IsNullOrWhiteSpace(input) || startIndex < 0 || startIndex >= input.Length)
                return input;

            if (startIndex + length > input.Length)
                length = input.Length - startIndex;

            return input.Remove(startIndex, length);
        }

        public static string ReplaceFirst(string input, string oldValue, string newValue)
        {
            if (string.IsNullOrWhiteSpace(input) || string.IsNullOrWhiteSpace(oldValue))
                return input;

            var index = input.IndexOf(oldValue, StringComparison.OrdinalIgnoreCase);
            if (index == -1)
                return input;

            return input.Remove(index, oldValue.Length).Insert(index, newValue);
        }

        public static string ReplaceLast(string input, string oldValue, string newValue)
        {
            if (string.IsNullOrWhiteSpace(input) || string.IsNullOrWhiteSpace(oldValue))
                return input;

            var index = input.LastIndexOf(oldValue, StringComparison.OrdinalIgnoreCase);
            if (index == -1)
                return input;

            return input.Remove(index, oldValue.Length).Insert(index, newValue);
        }

        public static int CountOccurrences(string input, string substring, bool ignoreCase = true)
        {
            if (string.IsNullOrWhiteSpace(input) || string.IsNullOrWhiteSpace(substring))
                return 0;

            var comparison = ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
            var count = 0;
            var index = 0;

            while ((index = input.IndexOf(substring, index, comparison)) != -1)
            {
                count++;
                index += substring.Length;
            }

            return count;
        }

        public static bool ContainsAll(string input, params string[] substrings)
        {
            if (string.IsNullOrWhiteSpace(input) || substrings == null || substrings.Length == 0)
                return false;

            return substrings.All(substring => !string.IsNullOrWhiteSpace(substring) && input.Contains(substring, StringComparison.OrdinalIgnoreCase));
        }

        public static bool ContainsAny(string input, params string[] substrings)
        {
            if (string.IsNullOrWhiteSpace(input) || substrings == null || substrings.Length == 0)
                return false;

            return substrings.Any(substring => !string.IsNullOrWhiteSpace(substring) && input.Contains(substring, StringComparison.OrdinalIgnoreCase));
        }

        public static string JoinStrings(string separator, params string[] values)
        {
            return string.Join(separator, values.Where(v => !string.IsNullOrWhiteSpace(v)));
        }

        public static string[] SplitString(string input, string separator, StringSplitOptions options = StringSplitOptions.RemoveEmptyEntries)
        {
            if (string.IsNullOrWhiteSpace(input))
                return Array.Empty<string>();

            return input.Split(new[] { separator }, options);
        }

        public static string GenerateRandomString(int length, bool includeNumbers = true, bool includeLetters = true, bool includeSymbols = false)
        {
            const string numbers = "0123456789";
            const string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            const string symbols = "!@#$%^&*()_+-=[]{}|;:,.<>?";

            var characters = string.Empty;
            if (includeNumbers) characters += numbers;
            if (includeLetters) characters += letters;
            if (includeSymbols) characters += symbols;

            if (string.IsNullOrWhiteSpace(characters))
                throw new ArgumentException("At least one character type must be included");

            var random = new Random();
            var result = new char[length];

            for (int i = 0; i < length; i++)
            {
                result[i] = characters[random.Next(characters.Length)];
            }

            return new string(result);
        }

        public static string GenerateRandomAlphaNumeric(int length)
        {
            return GenerateRandomString(length, true, true, false);
        }

        public static string GenerateRandomNumeric(int length)
        {
            return GenerateRandomString(length, true, false, false);
        }

        public static string GenerateRandomLetters(int length)
        {
            return GenerateRandomString(length, false, true, false);
        }

        public static string ToBase64(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            var bytes = Encoding.UTF8.GetBytes(input);
            return Convert.ToBase64String(bytes);
        }

        public static string FromBase64(string base64String)
        {
            if (string.IsNullOrWhiteSpace(base64String))
                return string.Empty;

            var bytes = Convert.FromBase64String(base64String);
            return Encoding.UTF8.GetString(bytes);
        }

        public static string UrlEncode(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            return Uri.EscapeDataString(input);
        }

        public static string UrlDecode(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            return Uri.UnescapeDataString(input);
        }

        public static string HtmlEncode(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            return System.Web.HttpUtility.HtmlEncode(input);
        }

        public static string HtmlDecode(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            return System.Web.HttpUtility.HtmlDecode(input);
        }

        public static string SanitizeForFileName(string input, string replacement = "_")
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            var invalidChars = Path.GetInvalidFileNameChars();
            var result = input;

            foreach (var c in invalidChars)
            {
                result = result.Replace(c, replacement[0]);
            }

            return result;
        }

        public static string SanitizeForPath(string input, string replacement = "_")
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            var invalidChars = Path.GetInvalidPathChars();
            var result = input;

            foreach (var c in invalidChars)
            {
                result = result.Replace(c, replacement[0]);
            }

            return result;
        }

        public static string FormatAccountNumber(string accountNumber, int groupSize = 4, string separator = " ")
        {
            if (string.IsNullOrWhiteSpace(accountNumber))
                return string.Empty;

            var cleanNumber = ExtractNumbers(accountNumber);
            var groups = new List<string>();

            for (int i = 0; i < cleanNumber.Length; i += groupSize)
            {
                var group = cleanNumber.Substring(i, Math.Min(groupSize, cleanNumber.Length - i));
                groups.Add(group);
            }

            return string.Join(separator, groups);
        }

        public static string FormatCardNumber(string cardNumber, int groupSize = 4, string separator = " ")
        {
            return FormatAccountNumber(cardNumber, groupSize, separator);
        }

        public static string FormatAmountForDisplay(decimal amount, string currency = "XOF", bool showCurrency = true)
        {
            var formattedAmount = amount.ToString("N2", new System.Globalization.CultureInfo("fr-FR"));
            return showCurrency ? $"{formattedAmount} {currency}" : formattedAmount;
        }

        public static string ExtractCurrencyCode(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            var match = Regex.Match(input, @"\b[A-Z]{3}\b");
            return match.Success ? match.Value : string.Empty;
        }

        public static bool IsValidEmail(string email)
        {
            return !string.IsNullOrWhiteSpace(email) && EmailRegex.IsMatch(email);
        }

        public static string ExtractEmail(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            var match = EmailRegex.Match(input);
            return match.Success ? match.Value : string.Empty;
        }

        public static string[] ExtractEmails(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return Array.Empty<string>();

            var matches = EmailRegex.Matches(input);
            return matches.Cast<Match>().Select(m => m.Value).ToArray();
        }

        public static string RemoveHtmlTags(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            return Regex.Replace(input, @"<[^>]*>", "");
        }

        public static string StripNonPrintableCharacters(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            return new string(input.Where(c => char.IsControl(c) == false || char.IsWhiteSpace(c)).ToArray());
        }

        public static string ConvertToCamelCase(string input, string separator = " ")
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            var words = input.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            if (words.Length == 0)
                return string.Empty;

            var result = words[0].ToLower();
            for (int i = 1; i < words.Length; i++)
            {
                result += CapitalizeFirstLetter(words[i]);
            }

            return result;
        }

        public static string ConvertToPascalCase(string input, string separator = " ")
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            var words = input.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            return string.Join("", words.Select(CapitalizeFirstLetter));
        }

        public static string ConvertToSnakeCase(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            var result = new StringBuilder();
            for (int i = 0; i < input.Length; i++)
            {
                var c = input[i];
                if (char.IsUpper(c))
                {
                    if (i > 0 && !char.IsUpper(input[i - 1]))
                        result.Append('_');
                    result.Append(char.ToLower(c));
                }
                else
                {
                    result.Append(c);
                }
            }

            return result.ToString();
        }

        public static string ConvertToKebabCase(string input)
        {
            return ConvertToSnakeCase(input).Replace('_', '-');
        }
    }
}
