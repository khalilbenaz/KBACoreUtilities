using System;
using System.Collections.Generic;
using System.Globalization;

namespace KBA.CoreUtilities.Utilities
{
    /// <summary>
    /// Comprehensive date and time utility class for date manipulation, formatting, and calculations
    /// </summary>
    public static class DateTimeUtils
    {
        #region DateTime Formatting

        /// <summary>
        /// Formats a DateTime to ISO 8601 format (yyyy-MM-ddTHH:mm:ssZ)
        /// </summary>
        public static string ToIso8601(DateTime dateTime)
        {
            return dateTime.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Formats a DateTime to RFC 3339 format
        /// </summary>
        public static string ToRfc3339(DateTime dateTime)
        {
            return dateTime.ToUniversalTime().ToString("yyyy-MM-dd'T'HH:mm:ss.fffK", CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Formats a DateTime to Unix timestamp (seconds since 1970-01-01)
        /// </summary>
        public static long ToUnixTimestamp(DateTime dateTime)
        {
            return ((DateTimeOffset)dateTime).ToUnixTimeSeconds();
        }

        /// <summary>
        /// Formats a DateTime to Unix timestamp in milliseconds
        /// </summary>
        public static long ToUnixTimestampMilliseconds(DateTime dateTime)
        {
            return ((DateTimeOffset)dateTime).ToUnixTimeMilliseconds();
        }

        /// <summary>
        /// Converts Unix timestamp to DateTime
        /// </summary>
        public static DateTime FromUnixTimestamp(long timestamp)
        {
            return DateTimeOffset.FromUnixTimeSeconds(timestamp).DateTime;
        }

        /// <summary>
        /// Converts Unix timestamp (milliseconds) to DateTime
        /// </summary>
        public static DateTime FromUnixTimestampMilliseconds(long timestampMs)
        {
            return DateTimeOffset.FromUnixTimeMilliseconds(timestampMs).DateTime;
        }

        /// <summary>
        /// Formats DateTime using custom pattern
        /// </summary>
        public static string Format(DateTime dateTime, string pattern, CultureInfo culture = null)
        {
            culture ??= CultureInfo.CurrentCulture;
            return dateTime.ToString(pattern, culture);
        }

        /// <summary>
        /// Formats DateTime in short date format (locale-aware)
        /// </summary>
        public static string ToShortDate(DateTime dateTime, CultureInfo culture = null)
        {
            culture ??= CultureInfo.CurrentCulture;
            return dateTime.ToString("d", culture);
        }

        /// <summary>
        /// Formats DateTime in long date format (locale-aware)
        /// </summary>
        public static string ToLongDate(DateTime dateTime, CultureInfo culture = null)
        {
            culture ??= CultureInfo.CurrentCulture;
            return dateTime.ToString("D", culture);
        }

        /// <summary>
        /// Formats DateTime in short time format (locale-aware)
        /// </summary>
        public static string ToShortTime(DateTime dateTime, CultureInfo culture = null)
        {
            culture ??= CultureInfo.CurrentCulture;
            return dateTime.ToString("t", culture);
        }

        /// <summary>
        /// Formats DateTime in long time format (locale-aware)
        /// </summary>
        public static string ToLongTime(DateTime dateTime, CultureInfo culture = null)
        {
            culture ??= CultureInfo.CurrentCulture;
            return dateTime.ToString("T", culture);
        }

        /// <summary>
        /// Formats DateTime to human-readable relative time (e.g., "2 hours ago", "in 3 days")
        /// </summary>
        public static string ToRelativeTime(DateTime dateTime, DateTime? referenceTime = null)
        {
            var reference = referenceTime ?? DateTime.UtcNow;
            var timeSpan = reference - dateTime;

            if (timeSpan.TotalSeconds < 60)
                return timeSpan.TotalSeconds < 0 ? "in a few seconds" : "a few seconds ago";

            if (timeSpan.TotalMinutes < 60)
            {
                var minutes = (int)Math.Abs(timeSpan.TotalMinutes);
                return timeSpan.TotalMinutes < 0
                    ? $"in {minutes} minute{(minutes != 1 ? "s" : "")}"
                    : $"{minutes} minute{(minutes != 1 ? "s" : "")} ago";
            }

            if (timeSpan.TotalHours < 24)
            {
                var hours = (int)Math.Abs(timeSpan.TotalHours);
                return timeSpan.TotalHours < 0
                    ? $"in {hours} hour{(hours != 1 ? "s" : "")}"
                    : $"{hours} hour{(hours != 1 ? "s" : "")} ago";
            }

            if (timeSpan.TotalDays < 30)
            {
                var days = (int)Math.Abs(timeSpan.TotalDays);
                return timeSpan.TotalDays < 0
                    ? $"in {days} day{(days != 1 ? "s" : "")}"
                    : $"{days} day{(days != 1 ? "s" : "")} ago";
            }

            if (timeSpan.TotalDays < 365)
            {
                var months = (int)Math.Abs(timeSpan.TotalDays / 30);
                return timeSpan.TotalDays < 0
                    ? $"in {months} month{(months != 1 ? "s" : "")}"
                    : $"{months} month{(months != 1 ? "s" : "")} ago";
            }

            var years = (int)Math.Abs(timeSpan.TotalDays / 365);
            return timeSpan.TotalDays < 0
                ? $"in {years} year{(years != 1 ? "s" : "")}"
                : $"{years} year{(years != 1 ? "s" : "")} ago";
        }

        #endregion

        #region DateTime Parsing

        /// <summary>
        /// Parses ISO 8601 string to DateTime
        /// </summary>
        public static DateTime? ParseIso8601(string isoString)
        {
            if (string.IsNullOrWhiteSpace(isoString))
                return null;

            if (DateTime.TryParse(isoString, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out var result))
                return result;

            return null;
        }

        /// <summary>
        /// Tries to parse date string with multiple formats
        /// </summary>
        public static DateTime? ParseFlexible(string dateString, CultureInfo culture = null)
        {
            if (string.IsNullOrWhiteSpace(dateString))
                return null;

            culture ??= CultureInfo.CurrentCulture;

            // Try standard parsing first
            if (DateTime.TryParse(dateString, culture, DateTimeStyles.None, out var result))
                return result;

            // Try common formats
            var formats = new[]
            {
                "yyyy-MM-dd",
                "yyyy/MM/dd",
                "dd/MM/yyyy",
                "MM/dd/yyyy",
                "dd-MM-yyyy",
                "MM-dd-yyyy",
                "yyyy-MM-dd HH:mm:ss",
                "yyyy/MM/dd HH:mm:ss",
                "dd/MM/yyyy HH:mm:ss",
                "MM/dd/yyyy HH:mm:ss"
            };

            foreach (var format in formats)
            {
                if (DateTime.TryParseExact(dateString, format, culture, DateTimeStyles.None, out result))
                    return result;
            }

            return null;
        }

        #endregion

        #region DateTime Calculations

        /// <summary>
        /// Adds business days to a date (excludes weekends)
        /// </summary>
        public static DateTime AddBusinessDays(DateTime date, int days)
        {
            var sign = days < 0 ? -1 : 1;
            var unsignedDays = Math.Abs(days);
            
            for (var i = 0; i < unsignedDays; i++)
            {
                do
                {
                    date = date.AddDays(sign);
                }
                while (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday);
            }
            
            return date;
        }

        /// <summary>
        /// Calculates the number of business days between two dates
        /// </summary>
        public static int GetBusinessDaysDifference(DateTime startDate, DateTime endDate)
        {
            var days = 0;
            var current = startDate.Date;
            var end = endDate.Date;

            while (current < end)
            {
                if (current.DayOfWeek != DayOfWeek.Saturday && current.DayOfWeek != DayOfWeek.Sunday)
                    days++;
                current = current.AddDays(1);
            }

            return days;
        }

        /// <summary>
        /// Gets the start of the day (00:00:00)
        /// </summary>
        public static DateTime StartOfDay(DateTime date)
        {
            return date.Date;
        }

        /// <summary>
        /// Gets the end of the day (23:59:59.999)
        /// </summary>
        public static DateTime EndOfDay(DateTime date)
        {
            return date.Date.AddDays(1).AddMilliseconds(-1);
        }

        /// <summary>
        /// Gets the start of the week (Monday)
        /// </summary>
        public static DateTime StartOfWeek(DateTime date, DayOfWeek startOfWeek = DayOfWeek.Monday)
        {
            var diff = (7 + (date.DayOfWeek - startOfWeek)) % 7;
            return date.AddDays(-diff).Date;
        }

        /// <summary>
        /// Gets the end of the week (Sunday)
        /// </summary>
        public static DateTime EndOfWeek(DateTime date, DayOfWeek startOfWeek = DayOfWeek.Monday)
        {
            return StartOfWeek(date, startOfWeek).AddDays(7).AddMilliseconds(-1);
        }

        /// <summary>
        /// Gets the start of the month
        /// </summary>
        public static DateTime StartOfMonth(DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1);
        }

        /// <summary>
        /// Gets the end of the month
        /// </summary>
        public static DateTime EndOfMonth(DateTime date)
        {
            return StartOfMonth(date).AddMonths(1).AddMilliseconds(-1);
        }

        /// <summary>
        /// Gets the start of the year
        /// </summary>
        public static DateTime StartOfYear(DateTime date)
        {
            return new DateTime(date.Year, 1, 1);
        }

        /// <summary>
        /// Gets the end of the year
        /// </summary>
        public static DateTime EndOfYear(DateTime date)
        {
            return new DateTime(date.Year, 12, 31, 23, 59, 59, 999);
        }

        /// <summary>
        /// Gets the start of the quarter
        /// </summary>
        public static DateTime StartOfQuarter(DateTime date)
        {
            var quarterMonth = ((date.Month - 1) / 3) * 3 + 1;
            return new DateTime(date.Year, quarterMonth, 1);
        }

        /// <summary>
        /// Gets the end of the quarter
        /// </summary>
        public static DateTime EndOfQuarter(DateTime date)
        {
            return StartOfQuarter(date).AddMonths(3).AddMilliseconds(-1);
        }

        /// <summary>
        /// Gets the age in years from a birth date
        /// </summary>
        public static int GetAge(DateTime birthDate, DateTime? referenceDate = null)
        {
            var reference = referenceDate ?? DateTime.Today;
            var age = reference.Year - birthDate.Year;
            
            if (birthDate.Date > reference.AddYears(-age))
                age--;
            
            return age;
        }

        /// <summary>
        /// Calculates the difference between two dates
        /// </summary>
        public static TimeSpan GetDifference(DateTime startDate, DateTime endDate)
        {
            return endDate - startDate;
        }

        /// <summary>
        /// Gets the number of days in a specific month
        /// </summary>
        public static int GetDaysInMonth(int year, int month)
        {
            return DateTime.DaysInMonth(year, month);
        }

        /// <summary>
        /// Gets the number of days in the current month
        /// </summary>
        public static int GetDaysInCurrentMonth()
        {
            var now = DateTime.Now;
            return GetDaysInMonth(now.Year, now.Month);
        }

        #endregion

        #region DateTime Validation

        /// <summary>
        /// Checks if a date is a weekend (Saturday or Sunday)
        /// </summary>
        public static bool IsWeekend(DateTime date)
        {
            return date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;
        }

        /// <summary>
        /// Checks if a date is a weekday (Monday to Friday)
        /// </summary>
        public static bool IsWeekday(DateTime date)
        {
            return !IsWeekend(date);
        }

        /// <summary>
        /// Checks if a year is a leap year
        /// </summary>
        public static bool IsLeapYear(int year)
        {
            return DateTime.IsLeapYear(year);
        }

        /// <summary>
        /// Checks if a date is in the past
        /// </summary>
        public static bool IsInPast(DateTime date)
        {
            return date < DateTime.Now;
        }

        /// <summary>
        /// Checks if a date is in the future
        /// </summary>
        public static bool IsInFuture(DateTime date)
        {
            return date > DateTime.Now;
        }

        /// <summary>
        /// Checks if a date is today
        /// </summary>
        public static bool IsToday(DateTime date)
        {
            return date.Date == DateTime.Today;
        }

        /// <summary>
        /// Checks if a date is yesterday
        /// </summary>
        public static bool IsYesterday(DateTime date)
        {
            return date.Date == DateTime.Today.AddDays(-1);
        }

        /// <summary>
        /// Checks if a date is tomorrow
        /// </summary>
        public static bool IsTomorrow(DateTime date)
        {
            return date.Date == DateTime.Today.AddDays(1);
        }

        /// <summary>
        /// Checks if a date falls within a date range
        /// </summary>
        public static bool IsInRange(DateTime date, DateTime startDate, DateTime endDate)
        {
            return date >= startDate && date <= endDate;
        }

        #endregion

        #region Time Zone Utilities

        /// <summary>
        /// Converts a DateTime from one time zone to another
        /// </summary>
        public static DateTime ConvertTimeZone(DateTime dateTime, TimeZoneInfo sourceTimeZone, TimeZoneInfo destinationTimeZone)
        {
            var utc = TimeZoneInfo.ConvertTimeToUtc(dateTime, sourceTimeZone);
            return TimeZoneInfo.ConvertTimeFromUtc(utc, destinationTimeZone);
        }

        /// <summary>
        /// Converts a DateTime to UTC
        /// </summary>
        public static DateTime ToUtc(DateTime dateTime, TimeZoneInfo sourceTimeZone = null)
        {
            sourceTimeZone ??= TimeZoneInfo.Local;
            return TimeZoneInfo.ConvertTimeToUtc(dateTime, sourceTimeZone);
        }

        /// <summary>
        /// Converts a UTC DateTime to local time
        /// </summary>
        public static DateTime FromUtc(DateTime utcDateTime)
        {
            return TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, TimeZoneInfo.Local);
        }

        /// <summary>
        /// Gets all available time zones
        /// </summary>
        public static IEnumerable<TimeZoneInfo> GetAllTimeZones()
        {
            return TimeZoneInfo.GetSystemTimeZones();
        }

        /// <summary>
        /// Gets time zone by ID
        /// </summary>
        public static TimeZoneInfo GetTimeZoneById(string timeZoneId)
        {
            return TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
        }

        #endregion

        #region Common Date Ranges

        /// <summary>
        /// Gets date range for today
        /// </summary>
        public static (DateTime start, DateTime end) GetTodayRange()
        {
            return (StartOfDay(DateTime.Today), EndOfDay(DateTime.Today));
        }

        /// <summary>
        /// Gets date range for yesterday
        /// </summary>
        public static (DateTime start, DateTime end) GetYesterdayRange()
        {
            var yesterday = DateTime.Today.AddDays(-1);
            return (StartOfDay(yesterday), EndOfDay(yesterday));
        }

        /// <summary>
        /// Gets date range for this week
        /// </summary>
        public static (DateTime start, DateTime end) GetThisWeekRange(DayOfWeek startOfWeek = DayOfWeek.Monday)
        {
            return (StartOfWeek(DateTime.Today, startOfWeek), EndOfWeek(DateTime.Today, startOfWeek));
        }

        /// <summary>
        /// Gets date range for last week
        /// </summary>
        public static (DateTime start, DateTime end) GetLastWeekRange(DayOfWeek startOfWeek = DayOfWeek.Monday)
        {
            var lastWeek = DateTime.Today.AddDays(-7);
            return (StartOfWeek(lastWeek, startOfWeek), EndOfWeek(lastWeek, startOfWeek));
        }

        /// <summary>
        /// Gets date range for this month
        /// </summary>
        public static (DateTime start, DateTime end) GetThisMonthRange()
        {
            return (StartOfMonth(DateTime.Today), EndOfMonth(DateTime.Today));
        }

        /// <summary>
        /// Gets date range for last month
        /// </summary>
        public static (DateTime start, DateTime end) GetLastMonthRange()
        {
            var lastMonth = DateTime.Today.AddMonths(-1);
            return (StartOfMonth(lastMonth), EndOfMonth(lastMonth));
        }

        /// <summary>
        /// Gets date range for this year
        /// </summary>
        public static (DateTime start, DateTime end) GetThisYearRange()
        {
            return (StartOfYear(DateTime.Today), EndOfYear(DateTime.Today));
        }

        /// <summary>
        /// Gets date range for last year
        /// </summary>
        public static (DateTime start, DateTime end) GetLastYearRange()
        {
            var lastYear = DateTime.Today.AddYears(-1);
            return (StartOfYear(lastYear), EndOfYear(lastYear));
        }

        /// <summary>
        /// Gets date range for last N days
        /// </summary>
        public static (DateTime start, DateTime end) GetLastNDaysRange(int days)
        {
            var start = DateTime.Today.AddDays(-days);
            return (StartOfDay(start), EndOfDay(DateTime.Today));
        }

        /// <summary>
        /// Gets date range for next N days
        /// </summary>
        public static (DateTime start, DateTime end) GetNextNDaysRange(int days)
        {
            var end = DateTime.Today.AddDays(days);
            return (StartOfDay(DateTime.Today), EndOfDay(end));
        }

        #endregion

        #region Working with TimeSpan

        /// <summary>
        /// Formats TimeSpan in human-readable format
        /// </summary>
        public static string FormatDuration(TimeSpan duration)
        {
            if (duration.TotalDays >= 1)
                return $"{(int)duration.TotalDays}d {duration.Hours}h {duration.Minutes}m";
            
            if (duration.TotalHours >= 1)
                return $"{(int)duration.TotalHours}h {duration.Minutes}m {duration.Seconds}s";
            
            if (duration.TotalMinutes >= 1)
                return $"{(int)duration.TotalMinutes}m {duration.Seconds}s";
            
            return $"{duration.Seconds}s";
        }

        /// <summary>
        /// Parses duration string (e.g., "2h 30m", "1d 5h") to TimeSpan
        /// </summary>
        public static TimeSpan? ParseDuration(string durationString)
        {
            if (string.IsNullOrWhiteSpace(durationString))
                return null;

            var totalSeconds = 0.0;
            var parts = durationString.ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries);

            foreach (var part in parts)
            {
                if (part.EndsWith("d") && double.TryParse(part.TrimEnd('d'), out var days))
                    totalSeconds += days * 86400;
                else if (part.EndsWith("h") && double.TryParse(part.TrimEnd('h'), out var hours))
                    totalSeconds += hours * 3600;
                else if (part.EndsWith("m") && double.TryParse(part.TrimEnd('m'), out var minutes))
                    totalSeconds += minutes * 60;
                else if (part.EndsWith("s") && double.TryParse(part.TrimEnd('s'), out var seconds))
                    totalSeconds += seconds;
            }

            return totalSeconds > 0 ? TimeSpan.FromSeconds(totalSeconds) : null;
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Gets the quarter number (1-4) for a date
        /// </summary>
        public static int GetQuarter(DateTime date)
        {
            return (date.Month - 1) / 3 + 1;
        }

        /// <summary>
        /// Gets the week number of the year
        /// </summary>
        public static int GetWeekOfYear(DateTime date, CultureInfo culture = null)
        {
            culture ??= CultureInfo.CurrentCulture;
            return culture.Calendar.GetWeekOfYear(date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }

        /// <summary>
        /// Gets all dates between two dates
        /// </summary>
        public static IEnumerable<DateTime> GetDatesBetween(DateTime startDate, DateTime endDate)
        {
            for (var date = startDate.Date; date <= endDate.Date; date = date.AddDays(1))
            {
                yield return date;
            }
        }

        /// <summary>
        /// Gets all business days between two dates
        /// </summary>
        public static IEnumerable<DateTime> GetBusinessDaysBetween(DateTime startDate, DateTime endDate)
        {
            for (var date = startDate.Date; date <= endDate.Date; date = date.AddDays(1))
            {
                if (IsWeekday(date))
                    yield return date;
            }
        }

        /// <summary>
        /// Rounds DateTime to nearest interval
        /// </summary>
        public static DateTime Round(DateTime dateTime, TimeSpan interval)
        {
            var ticks = (long)Math.Round((double)dateTime.Ticks / interval.Ticks) * interval.Ticks;
            return new DateTime(ticks, dateTime.Kind);
        }

        /// <summary>
        /// Truncates DateTime to specific interval (floor)
        /// </summary>
        public static DateTime Truncate(DateTime dateTime, TimeSpan interval)
        {
            var ticks = (dateTime.Ticks / interval.Ticks) * interval.Ticks;
            return new DateTime(ticks, dateTime.Kind);
        }

        #endregion
    }
}
