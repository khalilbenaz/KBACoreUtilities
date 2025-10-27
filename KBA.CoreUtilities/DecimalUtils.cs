using System.Globalization;

namespace KBA.CoreUtilities.Utilities
{
    public static class DecimalUtils
    {
        private static readonly CultureInfo FrenchCulture = new CultureInfo("fr-FR");
        private static readonly CultureInfo EnglishCulture = new CultureInfo("en-US");
        private static readonly CultureInfo InvariantCulture = CultureInfo.InvariantCulture;

        public static decimal RoundToDecimalPlaces(decimal value, int decimalPlaces)
        {
            return Math.Round(value, decimalPlaces, MidpointRounding.AwayFromZero);
        }

        public static decimal RoundToTwoDecimals(decimal value)
        {
            return RoundToDecimalPlaces(value, 2);
        }

        public static decimal RoundToThreeDecimals(decimal value)
        {
            return RoundToDecimalPlaces(value, 3);
        }

        public static decimal TruncateToDecimalPlaces(decimal value, int decimalPlaces)
        {
            var factor = (decimal)Math.Pow(10, decimalPlaces);
            return Math.Truncate(value * factor) / factor;
        }

        public static decimal TruncateToTwoDecimals(decimal value)
        {
            return TruncateToDecimalPlaces(value, 2);
        }

        public static decimal TruncateToThreeDecimals(decimal value)
        {
            return TruncateToDecimalPlaces(value, 3);
        }

        public static decimal FloorToDecimalPlaces(decimal value, int decimalPlaces)
        {
            var factor = (decimal)Math.Pow(10, decimalPlaces);
            return Math.Floor(value * factor) / factor;
        }

        public static decimal CeilingToDecimalPlaces(decimal value, int decimalPlaces)
        {
            var factor = (decimal)Math.Pow(10, decimalPlaces);
            return Math.Ceiling(value * factor) / factor;
        }

        public static string FormatWithTwoDecimals(decimal value, string culture = "fr-FR")
        {
            var cultureInfo = GetCultureInfo(culture);
            return value.ToString("N2", cultureInfo);
        }

        public static string FormatWithThreeDecimals(decimal value, string culture = "fr-FR")
        {
            var cultureInfo = GetCultureInfo(culture);
            return value.ToString("N3", cultureInfo);
        }

        public static string FormatWithDecimals(decimal value, int decimalPlaces, string culture = "fr-FR")
        {
            var cultureInfo = GetCultureInfo(culture);
            var format = $"N{decimalPlaces}";
            return value.ToString(format, cultureInfo);
        }

        public static string FormatCurrency(decimal value, string currencyCode = "XOF", string culture = "fr-FR")
        {
            var cultureInfo = GetCultureInfo(culture);
            return value.ToString("C", cultureInfo).Replace(cultureInfo.NumberFormat.CurrencySymbol, $"{currencyCode} ");
        }

        public static string FormatCurrencyWithDecimals(decimal value, string currencyCode = "XOF", int decimalPlaces = 2, string culture = "fr-FR")
        {
            var cultureInfo = GetCultureInfo(culture);
            var format = decimalPlaces == 2 ? "C" : $"C{decimalPlaces}";
            return value.ToString(format, cultureInfo).Replace(cultureInfo.NumberFormat.CurrencySymbol, $"{currencyCode} ");
        }

        public static decimal ParseDecimal(string value, string culture = "fr-FR")
        {
            if (string.IsNullOrWhiteSpace(value))
                return 0m;

            var cultureInfo = GetCultureInfo(culture);
            return decimal.Parse(value, NumberStyles.Any, cultureInfo);
        }

        public static decimal? TryParseDecimal(string value, string culture = "fr-FR")
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;

            var cultureInfo = GetCultureInfo(culture);
            return decimal.TryParse(value, NumberStyles.Any, cultureInfo, out var result) ? result : null;
        }

        public static decimal ParseDecimalWithInvariantCulture(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return 0m;

            return decimal.Parse(value, NumberStyles.Any, InvariantCulture);
        }

        public static decimal? TryParseDecimalWithInvariantCulture(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;

            return decimal.TryParse(value, NumberStyles.Any, InvariantCulture, out var result) ? result : null;
        }

        public static string RemoveCurrencySymbols(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return value;

            return System.Text.RegularExpressions.Regex.Replace(value, @"[^\d.,-]", "").Trim();
        }

        public static decimal ExtractDecimalFromString(string value, string culture = "fr-FR")
        {
            var cleanValue = RemoveCurrencySymbols(value);
            return ParseDecimal(cleanValue, culture);
        }

        public static decimal? TryExtractDecimalFromString(string value, string culture = "fr-FR")
        {
            var cleanValue = RemoveCurrencySymbols(value);
            return TryParseDecimal(cleanValue, culture);
        }

        public static bool IsValidDecimalString(string value, string culture = "fr-FR")
        {
            return TryParseDecimal(value, culture) != null;
        }

        public static decimal AddWithPrecision(decimal value1, decimal value2, int decimalPlaces = 2)
        {
            return RoundToDecimalPlaces(value1 + value2, decimalPlaces);
        }

        public static decimal SubtractWithPrecision(decimal value1, decimal value2, int decimalPlaces = 2)
        {
            return RoundToDecimalPlaces(value1 - value2, decimalPlaces);
        }

        public static decimal MultiplyWithPrecision(decimal value1, decimal value2, int decimalPlaces = 2)
        {
            return RoundToDecimalPlaces(value1 * value2, decimalPlaces);
        }

        public static decimal DivideWithPrecision(decimal value1, decimal value2, int decimalPlaces = 2)
        {
            if (value2 == 0)
                throw new DivideByZeroException("Cannot divide by zero");

            return RoundToDecimalPlaces(value1 / value2, decimalPlaces);
        }

        public static decimal CalculatePercentage(decimal value, decimal percentage, int decimalPlaces = 2)
        {
            return MultiplyWithPrecision(value, DivideWithPrecision(percentage, 100m, decimalPlaces + 2), decimalPlaces);
        }

        public static decimal CalculatePercentageOf(decimal part, decimal total, int decimalPlaces = 2)
        {
            if (total == 0)
                return 0m;

            return MultiplyWithPrecision(DivideWithPrecision(part, total, decimalPlaces + 2), 100m, decimalPlaces);
        }

        public static decimal ApplyVat(decimal amount, decimal vatRate, int decimalPlaces = 2)
        {
            return AddWithPrecision(amount, CalculatePercentage(amount, vatRate, decimalPlaces + 2), decimalPlaces);
        }

        public static decimal RemoveVat(decimal amountWithVat, decimal vatRate, int decimalPlaces = 2)
        {
            return DivideWithPrecision(amountWithVat, AddWithPrecision(1m, DivideWithPrecision(vatRate, 100m, decimalPlaces + 2), decimalPlaces + 2), decimalPlaces);
        }

        public static decimal CalculateVatAmount(decimal amount, decimal vatRate, int decimalPlaces = 2)
        {
            return CalculatePercentage(amount, vatRate, decimalPlaces);
        }

        public static bool IsApproximatelyEqual(decimal value1, decimal value2, decimal tolerance = 0.01m)
        {
            return Math.Abs(value1 - value2) <= tolerance;
        }

        public static bool IsApproximatelyEqualWithPrecision(decimal value1, decimal value2, int decimalPlaces)
        {
            var rounded1 = RoundToDecimalPlaces(value1, decimalPlaces);
            var rounded2 = RoundToDecimalPlaces(value2, decimalPlaces);
            return rounded1 == rounded2;
        }

        public static decimal Clamp(decimal value, decimal min, decimal max)
        {
            return Math.Min(Math.Max(value, min), max);
        }

        public static decimal NormalizeRange(decimal value, decimal min, decimal max, int decimalPlaces = 2)
        {
            if (max <= min)
                throw new ArgumentException("Max must be greater than min");

            var normalized = (value - min) / (max - min);
            return RoundToDecimalPlaces(normalized, decimalPlaces);
        }

        public static decimal Interpolate(decimal start, decimal end, decimal factor, int decimalPlaces = 2)
        {
            factor = Clamp(factor, 0m, 1m);
            return RoundToDecimalPlaces(start + (end - start) * factor, decimalPlaces);
        }

        public static decimal CalculateCompoundInterest(decimal principal, decimal annualRate, int years, int decimalPlaces = 2)
        {
            var rate = DivideWithPrecision(annualRate, 100m, decimalPlaces + 4);
            var amount = principal * (decimal)Math.Pow((double)rate + 1.0, years);
            return RoundToDecimalPlaces(amount, decimalPlaces);
        }

        public static decimal CalculateSimpleInterest(decimal principal, decimal annualRate, int years, int decimalPlaces = 2)
        {
            var interest = MultiplyWithPrecision(principal, MultiplyWithPrecision(DivideWithPrecision(annualRate, 100m, decimalPlaces + 2), years, decimalPlaces + 2), decimalPlaces);
            return AddWithPrecision(principal, interest, decimalPlaces);
        }

        public static decimal ConvertCurrency(decimal amount, decimal exchangeRate, int decimalPlaces = 2)
        {
            return MultiplyWithPrecision(amount, exchangeRate, decimalPlaces);
        }

        public static string FormatAsAccounting(decimal value, string culture = "fr-FR")
        {
            var cultureInfo = GetCultureInfo(culture);
            var format = value < 0 ? $"({Math.Abs(value):N2})" : $"{value:N2}";
            return format;
        }

        public static string FormatWithSeparator(decimal value, int decimalPlaces = 2, string culture = "fr-FR")
        {
            var cultureInfo = GetCultureInfo(culture);
            return value.ToString($"N{decimalPlaces}", cultureInfo);
        }

        public static string FormatWithoutSeparator(decimal value, int decimalPlaces = 2)
        {
            var format = $"0.{new string('0', decimalPlaces)}";
            return value.ToString(format, InvariantCulture);
        }

        public static decimal GetDecimalPart(decimal value)
        {
            return value - Math.Truncate(value);
        }

        public static int GetIntegerPart(decimal value)
        {
            return (int)Math.Truncate(value);
        }

        public static bool IsWholeNumber(decimal value)
        {
            return GetDecimalPart(value) == 0m;
        }

        public static decimal AbsoluteValue(decimal value)
        {
            return Math.Abs(value);
        }

        public static decimal Negate(decimal value)
        {
            return -value;
        }

        public static decimal Max(decimal value1, decimal value2)
        {
            return Math.Max(value1, value2);
        }

        public static decimal Min(decimal value1, decimal value2)
        {
            return Math.Min(value1, value2);
        }

        public static decimal Sum(IEnumerable<decimal> values, int decimalPlaces = 2)
        {
            return RoundToDecimalPlaces(values.Sum(), decimalPlaces);
        }

        public static decimal Average(IEnumerable<decimal> values, int decimalPlaces = 2)
        {
            return RoundToDecimalPlaces(values.Average(), decimalPlaces);
        }

        public static decimal Median(IEnumerable<decimal> values, int decimalPlaces = 2)
        {
            var sortedValues = values.OrderBy(x => x).ToList();
            var count = sortedValues.Count;
            
            if (count == 0)
                return 0m;
            
            if (count % 2 == 0)
                return RoundToDecimalPlaces((sortedValues[count / 2 - 1] + sortedValues[count / 2]) / 2m, decimalPlaces);
            
            return RoundToDecimalPlaces(sortedValues[count / 2], decimalPlaces);
        }

        private static CultureInfo GetCultureInfo(string culture)
        {
            return culture.ToLowerInvariant() switch
            {
                "fr" or "fr-fr" => FrenchCulture,
                "en" or "en-us" => EnglishCulture,
                "invariant" => InvariantCulture,
                _ => new CultureInfo(culture)
            };
        }

        public static decimal SafeDivide(decimal numerator, decimal denominator, decimal defaultValue = 0m, int decimalPlaces = 2)
        {
            if (denominator == 0)
                return RoundToDecimalPlaces(defaultValue, decimalPlaces);

            return RoundToDecimalPlaces(numerator / denominator, decimalPlaces);
        }

        public static decimal CalculateStandardDeviation(IEnumerable<decimal> values, int decimalPlaces = 2)
        {
            var valuesList = values.ToList();
            if (valuesList.Count < 2)
                return 0m;

            var mean = Average(valuesList, decimalPlaces + 4);
            var sumOfSquares = valuesList.Sum(x => (x - mean) * (x - mean));
            var variance = sumOfSquares / (valuesList.Count - 1);
            var stdDev = (decimal)Math.Sqrt((double)variance);

            return RoundToDecimalPlaces(stdDev, decimalPlaces);
        }

        public static decimal RoundToNearest(decimal value, decimal increment)
        {
            if (increment <= 0)
                throw new ArgumentException("Increment must be greater than zero");

            return Math.Round(value / increment) * increment;
        }

        public static decimal RoundUpToNearest(decimal value, decimal increment)
        {
            if (increment <= 0)
                throw new ArgumentException("Increment must be greater than zero");

            return Math.Ceiling(value / increment) * increment;
        }

        public static decimal RoundDownToNearest(decimal value, decimal increment)
        {
            if (increment <= 0)
                throw new ArgumentException("Increment must be greater than zero");

            return Math.Floor(value / increment) * increment;
        }
    }

    public class DecimalFormatter
    {
        public string Culture { get; set; } = "fr-FR";
        public int DecimalPlaces { get; set; } = 2;
        public bool UseGroupSeparator { get; set; } = true;
        public string CurrencySymbol { get; set; } = "";
        public bool ShowCurrencySymbol { get; set; } = false;

        public string Format(decimal value)
        {
            var cultureInfo = GetCultureInfo(Culture);
            var format = UseGroupSeparator ? $"N{DecimalPlaces}" : $"F{DecimalPlaces}";
            var result = value.ToString(format, cultureInfo);

            if (ShowCurrencySymbol && !string.IsNullOrWhiteSpace(CurrencySymbol))
            {
                result = $"{CurrencySymbol} {result}";
            }

            return result;
        }

        public decimal Parse(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return 0m;

            var cleanValue = value;
            if (ShowCurrencySymbol && !string.IsNullOrWhiteSpace(CurrencySymbol))
            {
                cleanValue = value.Replace(CurrencySymbol, "").Trim();
            }

            var cultureInfo = GetCultureInfo(Culture);
            return decimal.Parse(cleanValue, NumberStyles.Any, cultureInfo);
        }

        public decimal? TryParse(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;

            try
            {
                return Parse(value);
            }
            catch
            {
                return null;
            }
        }

        private CultureInfo GetCultureInfo(string culture)
        {
            return culture.ToLowerInvariant() switch
            {
                "fr" or "fr-fr" => new CultureInfo("fr-FR"),
                "en" or "en-us" => new CultureInfo("en-US"),
                "invariant" => CultureInfo.InvariantCulture,
                _ => new CultureInfo(culture)
            };
        }
    }
}
