using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.Json;

namespace KBA.CoreUtilities.Extensions
{
    /// <summary>
    /// Extension methods for object manipulation, null handling, and conversions
    /// </summary>
    public static class ObjectExtensions
    {
        #region Null Checks and Handling

        /// <summary>
        /// Checks if object is null
        /// </summary>
        public static bool IsNull<T>(this T obj)
        {
            return obj == null;
        }

        /// <summary>
        /// Checks if object is not null
        /// </summary>
        public static bool IsNotNull<T>(this T obj)
        {
            return obj != null;
        }

        /// <summary>
        /// Returns default value if object is null
        /// </summary>
        public static T IfNull<T>(this T obj, T defaultValue)
        {
            return obj ?? defaultValue;
        }

        /// <summary>
        /// Returns value from function if object is null
        /// </summary>
        public static T IfNull<T>(this T obj, Func<T> defaultValueFunc)
        {
            return obj ?? (defaultValueFunc != null ? defaultValueFunc() : default);
        }

        /// <summary>
        /// Executes action if object is not null
        /// </summary>
        public static T IfNotNull<T>(this T obj, Action<T> action)
        {
            if (obj != null)
                action?.Invoke(obj);
            
            return obj;
        }

        /// <summary>
        /// Transforms object if not null
        /// </summary>
        public static TResult IfNotNull<T, TResult>(this T obj, Func<T, TResult> func, TResult defaultValue = default)
        {
            return obj != null ? func(obj) : defaultValue;
        }

        /// <summary>
        /// Throws exception if object is null
        /// </summary>
        public static T ThrowIfNull<T>(this T obj, string paramName = null)
        {
            if (obj == null)
                throw new ArgumentNullException(paramName ?? nameof(obj));
            
            return obj;
        }

        #endregion

        #region Type Conversions

        /// <summary>
        /// Converts object to specified type
        /// </summary>
        public static T ConvertTo<T>(this object obj, T defaultValue = default)
        {
            if (obj == null)
                return defaultValue;

            try
            {
                var targetType = typeof(T);

                // Handle nullable types
                if (targetType.IsGenericType && targetType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    targetType = Nullable.GetUnderlyingType(targetType);
                }

                // Handle enums
                if (targetType.IsEnum)
                {
                    return (T)Enum.Parse(targetType, obj.ToString(), true);
                }

                // Use TypeConverter
                var converter = TypeDescriptor.GetConverter(targetType);
                if (converter.CanConvertFrom(obj.GetType()))
                {
                    return (T)converter.ConvertFrom(obj);
                }

                // Direct conversion
                return (T)Convert.ChangeType(obj, targetType);
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Tries to convert object to specified type
        /// </summary>
        public static bool TryConvertTo<T>(this object obj, out T result)
        {
            try
            {
                result = obj.ConvertTo<T>();
                return true;
            }
            catch
            {
                result = default;
                return false;
            }
        }

        /// <summary>
        /// Casts object to type or returns default
        /// </summary>
        public static T As<T>(this object obj, T defaultValue = default) where T : class
        {
            return obj as T ?? defaultValue;
        }

        /// <summary>
        /// Checks if object is of specified type
        /// </summary>
        public static bool Is<T>(this object obj)
        {
            return obj is T;
        }

        #endregion

        #region JSON Conversions

        /// <summary>
        /// Converts object to JSON string
        /// </summary>
        public static string ToJson(this object obj, bool indented = false)
        {
            if (obj == null)
                return null;

            var options = new JsonSerializerOptions
            {
                WriteIndented = indented
            };

            return JsonSerializer.Serialize(obj, options);
        }

        /// <summary>
        /// Converts JSON string to object
        /// </summary>
        public static T FromJson<T>(this string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return default;

            try
            {
                return JsonSerializer.Deserialize<T>(json);
            }
            catch
            {
                return default;
            }
        }

        /// <summary>
        /// Converts JSON string to dictionary
        /// </summary>
        public static Dictionary<string, object> JsonToDictionary(this string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return new Dictionary<string, object>();

            try
            {
                return JsonSerializer.Deserialize<Dictionary<string, object>>(json);
            }
            catch
            {
                return new Dictionary<string, object>();
            }
        }

        #endregion

        #region Cloning

        /// <summary>
        /// Creates deep clone via JSON serialization
        /// </summary>
        public static T DeepClone<T>(this T obj)
        {
            if (obj == null)
                return default;

            var json = obj.ToJson();
            return json.FromJson<T>();
        }

        #endregion

        #region Range and Between

        /// <summary>
        /// Checks if value is between min and max (inclusive)
        /// </summary>
        public static bool IsBetween<T>(this T value, T min, T max) where T : IComparable<T>
        {
            return value.CompareTo(min) >= 0 && value.CompareTo(max) <= 0;
        }

        /// <summary>
        /// Clamps value between min and max
        /// </summary>
        public static T Clamp<T>(this T value, T min, T max) where T : IComparable<T>
        {
            if (value.CompareTo(min) < 0)
                return min;
            
            if (value.CompareTo(max) > 0)
                return max;
            
            return value;
        }

        #endregion

        #region In and NotIn

        /// <summary>
        /// Checks if value is in collection
        /// </summary>
        public static bool In<T>(this T value, params T[] values)
        {
            return values != null && values.Contains(value);
        }

        /// <summary>
        /// Checks if value is in collection
        /// </summary>
        public static bool In<T>(this T value, IEnumerable<T> values)
        {
            return values != null && values.Contains(value);
        }

        /// <summary>
        /// Checks if value is not in collection
        /// </summary>
        public static bool NotIn<T>(this T value, params T[] values)
        {
            return !value.In(values);
        }

        /// <summary>
        /// Checks if value is not in collection
        /// </summary>
        public static bool NotIn<T>(this T value, IEnumerable<T> values)
        {
            return !value.In(values);
        }

        #endregion

        #region Exception Handling

        /// <summary>
        /// Executes action with exception handling
        /// </summary>
        public static T Try<T>(this T obj, Action<T> action, Action<Exception> onError = null)
        {
            try
            {
                action?.Invoke(obj);
            }
            catch (Exception ex)
            {
                onError?.Invoke(ex);
            }

            return obj;
        }

        /// <summary>
        /// Executes function with exception handling
        /// </summary>
        public static TResult Try<T, TResult>(this T obj, Func<T, TResult> func, TResult defaultValue = default, Action<Exception> onError = null)
        {
            try
            {
                return func != null ? func(obj) : defaultValue;
            }
            catch (Exception ex)
            {
                onError?.Invoke(ex);
                return defaultValue;
            }
        }

        #endregion

        #region Validation

        /// <summary>
        /// Throws exception if condition is false
        /// </summary>
        public static T ThrowIf<T>(this T obj, Func<T, bool> predicate, string message = null)
        {
            if (predicate(obj))
                throw new InvalidOperationException(message ?? "Validation failed");
            
            return obj;
        }

        /// <summary>
        /// Throws exception if condition is true
        /// </summary>
        public static T ThrowUnless<T>(this T obj, Func<T, bool> predicate, string message = null)
        {
            if (!predicate(obj))
                throw new InvalidOperationException(message ?? "Validation failed");
            
            return obj;
        }

        #endregion

        #region Chaining

        /// <summary>
        /// Pipes object through function
        /// </summary>
        public static TResult Pipe<T, TResult>(this T obj, Func<T, TResult> func)
        {
            return func(obj);
        }

        /// <summary>
        /// Applies action and returns object (for chaining)
        /// </summary>
        public static T Tap<T>(this T obj, Action<T> action)
        {
            action?.Invoke(obj);
            return obj;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets property value by name
        /// </summary>
        public static object GetPropertyValue(this object obj, string propertyName)
        {
            if (obj == null || string.IsNullOrWhiteSpace(propertyName))
                return null;

            var type = obj.GetType();
            var property = type.GetProperty(propertyName);
            
            return property?.GetValue(obj);
        }

        /// <summary>
        /// Sets property value by name
        /// </summary>
        public static void SetPropertyValue(this object obj, string propertyName, object value)
        {
            if (obj == null || string.IsNullOrWhiteSpace(propertyName))
                return;

            var type = obj.GetType();
            var property = type.GetProperty(propertyName);
            
            property?.SetValue(obj, value);
        }

        /// <summary>
        /// Checks if object has property
        /// </summary>
        public static bool HasProperty(this object obj, string propertyName)
        {
            if (obj == null || string.IsNullOrWhiteSpace(propertyName))
                return false;

            var type = obj.GetType();
            return type.GetProperty(propertyName) != null;
        }

        #endregion

        #region Query String

        /// <summary>
        /// Converts object to query string
        /// </summary>
        public static string ToQueryString(this object obj)
        {
            if (obj == null)
                return string.Empty;

            var properties = obj.GetType().GetProperties()
                .Where(p => p.CanRead && p.GetValue(obj) != null)
                .Select(p => $"{Uri.EscapeDataString(p.Name)}={Uri.EscapeDataString(p.GetValue(obj).ToString())}");

            return string.Join("&", properties);
        }

        /// <summary>
        /// Parses query string to dictionary
        /// </summary>
        public static Dictionary<string, string> QueryStringToDictionary(this string queryString)
        {
            if (string.IsNullOrWhiteSpace(queryString))
                return new Dictionary<string, string>();

            queryString = queryString.TrimStart('?');

            return queryString.Split('&')
                .Select(part => part.Split('='))
                .Where(parts => parts.Length == 2)
                .ToDictionary(
                    parts => Uri.UnescapeDataString(parts[0]),
                    parts => Uri.UnescapeDataString(parts[1]));
        }

        #endregion

        #region Equality

        /// <summary>
        /// Checks equality with any of the values
        /// </summary>
        public static bool EqualsAny<T>(this T obj, params T[] values)
        {
            return values != null && values.Any(v => EqualityComparer<T>.Default.Equals(obj, v));
        }

        /// <summary>
        /// Checks equality with all of the values
        /// </summary>
        public static bool EqualsAll<T>(this T obj, params T[] values)
        {
            return values != null && values.All(v => EqualityComparer<T>.Default.Equals(obj, v));
        }

        #endregion
    }
}
