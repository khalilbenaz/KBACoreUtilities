using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace KBA.CoreUtilities.Extensions
{
    /// <summary>
    /// Extension methods for string manipulation and validation
    /// </summary>
    public static class StringExtensions
    {
        #region Validation

        /// <summary>
        /// Checks if string is a valid email address
        /// </summary>
        public static bool IsEmail(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return false;

            try
            {
                return Regex.IsMatch(value,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Checks if string is a valid IPv4 address
        /// </summary>
        public static bool IsValidIPv4(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return false;

            var parts = value.Split('.');
            if (parts.Length != 4)
                return false;

            return parts.All(part => byte.TryParse(part, out _));
        }

        /// <summary>
        /// Checks if string is a valid IPv6 address
        /// </summary>
        public static bool IsValidIPv6(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return false;

            return Regex.IsMatch(value,
                @"^(([0-9a-fA-F]{1,4}:){7,7}[0-9a-fA-F]{1,4}|([0-9a-fA-F]{1,4}:){1,7}:|([0-9a-fA-F]{1,4}:){1,6}:[0-9a-fA-F]{1,4})$");
        }

        /// <summary>
        /// Checks if string contains only numeric characters
        /// </summary>
        public static bool IsNumeric(this string value)
        {
            return !string.IsNullOrEmpty(value) && value.All(char.IsDigit);
        }

        /// <summary>
        /// Checks if string contains only alphanumeric characters
        /// </summary>
        public static bool IsAlphanumeric(this string value)
        {
            return !string.IsNullOrEmpty(value) && value.All(char.IsLetterOrDigit);
        }

        /// <summary>
        /// Checks if string contains only alphabetic characters
        /// </summary>
        public static bool IsAlpha(this string value)
        {
            return !string.IsNullOrEmpty(value) && value.All(char.IsLetter);
        }

        /// <summary>
        /// Checks if string is null or empty
        /// </summary>
        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        /// <summary>
        /// Checks if string is null, empty or whitespace
        /// </summary>
        public static bool IsNullOrWhiteSpace(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        #endregion

        #region Manipulation

        /// <summary>
        /// Truncates string to specified length
        /// </summary>
        public static string Truncate(this string value, int maxLength, string suffix = "...")
        {
            if (string.IsNullOrEmpty(value) || value.Length <= maxLength)
                return value;

            return value.Substring(0, maxLength) + suffix;
        }

        /// <summary>
        /// Capitalizes first letter of string
        /// </summary>
        public static string Capitalize(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            return char.ToUpper(value[0]) + value.Substring(1);
        }

        /// <summary>
        /// Converts string to title case
        /// </summary>
        public static string ToTitleCase(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(value.ToLower());
        }

        /// <summary>
        /// Removes prefix from string if present
        /// </summary>
        public static string RemovePrefix(this string value, string prefix)
        {
            if (string.IsNullOrEmpty(value) || string.IsNullOrEmpty(prefix))
                return value;

            return value.StartsWith(prefix) ? value.Substring(prefix.Length) : value;
        }

        /// <summary>
        /// Removes suffix from string if present
        /// </summary>
        public static string RemoveSuffix(this string value, string suffix)
        {
            if (string.IsNullOrEmpty(value) || string.IsNullOrEmpty(suffix))
                return value;

            return value.EndsWith(suffix) ? value.Substring(0, value.Length - suffix.Length) : value;
        }

        /// <summary>
        /// Reverses the string
        /// </summary>
        public static string Reverse(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            var charArray = value.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        /// <summary>
        /// Counts occurrences of a substring
        /// </summary>
        public static int CountOccurrences(this string value, string substring)
        {
            if (string.IsNullOrEmpty(value) || string.IsNullOrEmpty(substring))
                return 0;

            var count = 0;
            var index = 0;

            while ((index = value.IndexOf(substring, index, StringComparison.Ordinal)) != -1)
            {
                count++;
                index += substring.Length;
            }

            return count;
        }

        /// <summary>
        /// Removes all whitespace from string
        /// </summary>
        public static string RemoveWhitespace(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            return new string(value.Where(c => !char.IsWhiteSpace(c)).ToArray());
        }

        /// <summary>
        /// Repeats string n times
        /// </summary>
        public static string Repeat(this string value, int count)
        {
            if (string.IsNullOrEmpty(value) || count <= 0)
                return string.Empty;

            return string.Concat(Enumerable.Repeat(value, count));
        }

        #endregion

        #region Conversions

        /// <summary>
        /// Converts string to int with default value
        /// </summary>
        public static int ToInt(this string value, int defaultValue = 0)
        {
            return int.TryParse(value, out var result) ? result : defaultValue;
        }

        /// <summary>
        /// Converts string to long with default value
        /// </summary>
        public static long ToLong(this string value, long defaultValue = 0)
        {
            return long.TryParse(value, out var result) ? result : defaultValue;
        }

        /// <summary>
        /// Converts string to decimal with default value
        /// </summary>
        public static decimal ToDecimal(this string value, decimal defaultValue = 0)
        {
            return decimal.TryParse(value, out var result) ? result : defaultValue;
        }

        /// <summary>
        /// Converts string to double with default value
        /// </summary>
        public static double ToDouble(this string value, double defaultValue = 0)
        {
            return double.TryParse(value, out var result) ? result : defaultValue;
        }

        /// <summary>
        /// Converts string to bool with default value
        /// </summary>
        public static bool ToBool(this string value, bool defaultValue = false)
        {
            return bool.TryParse(value, out var result) ? result : defaultValue;
        }

        /// <summary>
        /// Converts string to DateTime with default value
        /// </summary>
        public static DateTime ToDateTime(this string value, DateTime? defaultValue = null)
        {
            return DateTime.TryParse(value, out var result) ? result : defaultValue ?? DateTime.MinValue;
        }

        /// <summary>
        /// Converts string to enum
        /// </summary>
        public static T ToEnum<T>(this string value, T defaultValue = default) where T : struct, Enum
        {
            return Enum.TryParse<T>(value, true, out var result) ? result : defaultValue;
        }

        /// <summary>
        /// Converts string to Guid
        /// </summary>
        public static Guid? ToGuid(this string value)
        {
            return Guid.TryParse(value, out var result) ? result : null;
        }

        /// <summary>
        /// Converts to Base64
        /// </summary>
        public static string ToBase64(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            var bytes = Encoding.UTF8.GetBytes(value);
            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// Converts from Base64
        /// </summary>
        public static string FromBase64(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            try
            {
                var bytes = Convert.FromBase64String(value);
                return Encoding.UTF8.GetString(bytes);
            }
            catch
            {
                return value;
            }
        }

        #endregion

        #region Hashing

        /// <summary>
        /// Computes SHA256 hash of string
        /// </summary>
        public static string ToSHA256(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(value);
            var hash = sha256.ComputeHash(bytes);
            return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
        }

        /// <summary>
        /// Computes SHA512 hash of string
        /// </summary>
        public static string ToSHA512(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            using var sha512 = SHA512.Create();
            var bytes = Encoding.UTF8.GetBytes(value);
            var hash = sha512.ComputeHash(bytes);
            return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
        }

        /// <summary>
        /// Computes MD5 hash of string
        /// </summary>
        public static string ToMD5(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            using var md5 = MD5.Create();
            var bytes = Encoding.UTF8.GetBytes(value);
            var hash = md5.ComputeHash(bytes);
            return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
        }

        #endregion

        #region Null Handling

        /// <summary>
        /// Returns empty string if value is null
        /// </summary>
        public static string GetEmptyStringIfNull(this string value)
        {
            return value ?? string.Empty;
        }

        /// <summary>
        /// Returns default value if string is null or empty
        /// </summary>
        public static string GetDefaultIfEmpty(this string value, string defaultValue)
        {
            return string.IsNullOrEmpty(value) ? defaultValue : value;
        }

        /// <summary>
        /// Returns default value if string is null or whitespace
        /// </summary>
        public static string GetDefaultIfWhiteSpace(this string value, string defaultValue)
        {
            return string.IsNullOrWhiteSpace(value) ? defaultValue : value;
        }

        /// <summary>
        /// Executes action if string is not null or empty
        /// </summary>
        public static string IfNotEmpty(this string value, Action<string> action)
        {
            if (!string.IsNullOrEmpty(value))
                action?.Invoke(value);
            
            return value;
        }

        #endregion

        #region Format & Parsing

        /// <summary>
        /// Formats string with arguments
        /// </summary>
        public static string FormatWith(this string value, params object[] args)
        {
            return string.Format(value, args);
        }

        /// <summary>
        /// Splits string by separator and removes empty entries
        /// </summary>
        public static string[] SplitAndTrim(this string value, char separator)
        {
            if (string.IsNullOrEmpty(value))
                return Array.Empty<string>();

            return value.Split(separator)
                       .Select(s => s.Trim())
                       .Where(s => !string.IsNullOrEmpty(s))
                       .ToArray();
        }

        /// <summary>
        /// Converts string to camelCase
        /// </summary>
        public static string ToCamelCase(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            var words = value.Split(new[] { ' ', '-', '_' }, StringSplitOptions.RemoveEmptyEntries);
            if (words.Length == 0)
                return value;

            var result = words[0].ToLower();
            for (var i = 1; i < words.Length; i++)
            {
                result += char.ToUpper(words[i][0]) + words[i].Substring(1).ToLower();
            }

            return result;
        }

        /// <summary>
        /// Converts string to PascalCase
        /// </summary>
        public static string ToPascalCase(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            var words = value.Split(new[] { ' ', '-', '_' }, StringSplitOptions.RemoveEmptyEntries);
            return string.Concat(words.Select(word => char.ToUpper(word[0]) + word.Substring(1).ToLower()));
        }

        /// <summary>
        /// Converts string to snake_case
        /// </summary>
        public static string ToSnakeCase(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            return Regex.Replace(value, @"(\p{Ll})(\p{Lu})", "$1_$2")
                        .Replace(" ", "_")
                        .Replace("-", "_")
                        .ToLowerInvariant();
        }

        /// <summary>
        /// Converts string to kebab-case
        /// </summary>
        public static string ToKebabCase(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            return Regex.Replace(value, @"(\p{Ll})(\p{Lu})", "$1-$2")
                        .Replace(" ", "-")
                        .Replace("_", "-")
                        .ToLowerInvariant();
        }

        /// <summary>
        /// Removes diacritics/accents from string
        /// </summary>
        public static string RemoveDiacritics(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            var normalizedString = value.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        /// <summary>
        /// Masks part of string (useful for credit cards, phones)
        /// </summary>
        public static string Mask(this string value, int visibleChars = 4, char maskChar = '*')
        {
            if (string.IsNullOrEmpty(value) || value.Length <= visibleChars)
                return value;

            return new string(maskChar, value.Length - visibleChars) + value.Substring(value.Length - visibleChars);
        }

        /// <summary>
        /// Checks if string matches regex pattern
        /// </summary>
        public static bool MatchesPattern(this string value, string pattern)
        {
            if (string.IsNullOrEmpty(value) || string.IsNullOrEmpty(pattern))
                return false;

            try
            {
                return Regex.IsMatch(value, pattern);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Extracts numbers from string
        /// </summary>
        public static string ExtractNumbers(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            return new string(value.Where(char.IsDigit).ToArray());
        }

        /// <summary>
        /// Extracts letters from string
        /// </summary>
        public static string ExtractLetters(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            return new string(value.Where(char.IsLetter).ToArray());
        }

        /// <summary>
        /// Ensures string ends with specified suffix
        /// </summary>
        public static string EnsureEndsWith(this string value, string suffix)
        {
            if (string.IsNullOrEmpty(value) || string.IsNullOrEmpty(suffix))
                return value;

            return value.EndsWith(suffix) ? value : value + suffix;
        }

        /// <summary>
        /// Ensures string starts with specified prefix
        /// </summary>
        public static string EnsureStartsWith(this string value, string prefix)
        {
            if (string.IsNullOrEmpty(value) || string.IsNullOrEmpty(prefix))
                return value;

            return value.StartsWith(prefix) ? value : prefix + value;
        }

        #endregion
    }
}
