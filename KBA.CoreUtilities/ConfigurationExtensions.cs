using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KBA.CoreUtilities.Extensions
{
    /// <summary>
    /// Extension methods for configuration access
    /// </summary>
    public static class ConfigurationExtensions
    {
        #region Typed Value Retrieval

        /// <summary>
        /// Gets configuration value as string with default
        /// </summary>
        public static string GetString(this IConfiguration configuration, string key, string defaultValue = null)
        {
            return configuration[key] ?? defaultValue;
        }

        /// <summary>
        /// Gets configuration value as int
        /// </summary>
        public static int GetInt(this IConfiguration configuration, string key, int defaultValue = 0)
        {
            var value = configuration[key];
            return int.TryParse(value, out var result) ? result : defaultValue;
        }

        /// <summary>
        /// Gets configuration value as long
        /// </summary>
        public static long GetLong(this IConfiguration configuration, string key, long defaultValue = 0)
        {
            var value = configuration[key];
            return long.TryParse(value, out var result) ? result : defaultValue;
        }

        /// <summary>
        /// Gets configuration value as bool
        /// </summary>
        public static bool GetBool(this IConfiguration configuration, string key, bool defaultValue = false)
        {
            var value = configuration[key];
            return bool.TryParse(value, out var result) ? result : defaultValue;
        }

        /// <summary>
        /// Gets configuration value as decimal
        /// </summary>
        public static decimal GetDecimal(this IConfiguration configuration, string key, decimal defaultValue = 0)
        {
            var value = configuration[key];
            return decimal.TryParse(value, out var result) ? result : defaultValue;
        }

        /// <summary>
        /// Gets configuration value as double
        /// </summary>
        public static double GetDouble(this IConfiguration configuration, string key, double defaultValue = 0)
        {
            var value = configuration[key];
            return double.TryParse(value, out var result) ? result : defaultValue;
        }

        /// <summary>
        /// Gets configuration value as DateTime
        /// </summary>
        public static DateTime GetDateTime(this IConfiguration configuration, string key, DateTime? defaultValue = null)
        {
            var value = configuration[key];
            return DateTime.TryParse(value, out var result) ? result : defaultValue ?? DateTime.MinValue;
        }

        /// <summary>
        /// Gets configuration value as enum
        /// </summary>
        public static T GetEnum<T>(this IConfiguration configuration, string key, T defaultValue = default) where T : struct, Enum
        {
            var value = configuration[key];
            return Enum.TryParse<T>(value, true, out var result) ? result : defaultValue;
        }

        /// <summary>
        /// Gets configuration value as TimeSpan
        /// </summary>
        public static TimeSpan GetTimeSpan(this IConfiguration configuration, string key, TimeSpan? defaultValue = null)
        {
            var value = configuration[key];
            return TimeSpan.TryParse(value, out var result) ? result : defaultValue ?? TimeSpan.Zero;
        }

        #endregion

        #region Required Values

        /// <summary>
        /// Gets required configuration value (throws if not found)
        /// </summary>
        public static string GetRequired(this IConfiguration configuration, string key)
        {
            var value = configuration[key];
            if (string.IsNullOrWhiteSpace(value))
                throw new InvalidOperationException($"Configuration key '{key}' is required but was not found or is empty");

            return value;
        }

        /// <summary>
        /// Gets required configuration value as type T
        /// </summary>
        public static T GetRequired<T>(this IConfiguration configuration, string key)
        {
            var value = configuration.GetRequired(key);
            
            try
            {
                return (T)Convert.ChangeType(value, typeof(T));
            }
            catch
            {
                throw new InvalidOperationException($"Configuration key '{key}' could not be converted to type {typeof(T).Name}");
            }
        }

        #endregion

        #region Environment Variables

        /// <summary>
        /// Gets value from configuration or environment variable
        /// </summary>
        public static string GetFromConfigOrEnv(this IConfiguration configuration, string configKey, string envKey = null)
        {
            var value = configuration[configKey];
            if (!string.IsNullOrWhiteSpace(value))
                return value;

            return Environment.GetEnvironmentVariable(envKey ?? configKey);
        }

        /// <summary>
        /// Gets connection string with fallback to environment variable
        /// </summary>
        public static string GetConnectionStringOrEnv(this IConfiguration configuration, string name)
        {
            var connString = configuration.GetConnectionString(name);
            if (!string.IsNullOrWhiteSpace(connString))
                return connString;

            return Environment.GetEnvironmentVariable($"ConnectionStrings__{name}");
        }

        #endregion

        #region Section Operations

        /// <summary>
        /// Gets section and binds to object
        /// </summary>
        public static T GetSection<T>(this IConfiguration configuration, string sectionName) where T : new()
        {
            var section = configuration.GetSection(sectionName);
            var instance = new T();
            section.Bind(instance);
            return instance;
        }

        /// <summary>
        /// Checks if section exists and has values
        /// </summary>
        public static bool SectionExists(this IConfiguration configuration, string sectionName)
        {
            var section = configuration.GetSection(sectionName);
            return section.Exists();
        }

        /// <summary>
        /// Gets all keys from section
        /// </summary>
        public static IEnumerable<string> GetSectionKeys(this IConfiguration configuration, string sectionName)
        {
            var section = configuration.GetSection(sectionName);
            return section.GetChildren().Select(c => c.Key);
        }

        #endregion

        #region Array/List Values

        /// <summary>
        /// Gets configuration value as array
        /// </summary>
        public static string[] GetArray(this IConfiguration configuration, string key, char separator = ',')
        {
            var value = configuration[key];
            if (string.IsNullOrWhiteSpace(value))
                return Array.Empty<string>();

            return value.Split(separator)
                       .Select(s => s.Trim())
                       .Where(s => !string.IsNullOrEmpty(s))
                       .ToArray();
        }

        /// <summary>
        /// Gets configuration section as list of T
        /// </summary>
        public static List<T> GetList<T>(this IConfiguration configuration, string sectionName) where T : new()
        {
            var section = configuration.GetSection(sectionName);
            var list = new List<T>();
            section.Bind(list);
            return list;
        }

        #endregion

        #region Dictionary Operations

        /// <summary>
        /// Gets section as dictionary
        /// </summary>
        public static Dictionary<string, string> GetDictionary(this IConfiguration configuration, string sectionName)
        {
            var section = configuration.GetSection(sectionName);
            return section.GetChildren()
                         .ToDictionary(x => x.Key, x => x.Value);
        }

        #endregion

        #region Validation

        /// <summary>
        /// Validates that required keys exist in configuration
        /// </summary>
        public static void ValidateRequired(this IConfiguration configuration, params string[] requiredKeys)
        {
            var missingKeys = new List<string>();

            foreach (var key in requiredKeys)
            {
                var value = configuration[key];
                if (string.IsNullOrWhiteSpace(value))
                {
                    missingKeys.Add(key);
                }
            }

            if (missingKeys.Any())
            {
                throw new InvalidOperationException(
                    $"Required configuration keys are missing: {string.Join(", ", missingKeys)}");
            }
        }

        /// <summary>
        /// Validates configuration section exists
        /// </summary>
        public static void ValidateSection(this IConfiguration configuration, string sectionName)
        {
            if (!configuration.SectionExists(sectionName))
            {
                throw new InvalidOperationException($"Required configuration section '{sectionName}' is missing");
            }
        }

        #endregion

        #region URL and Path

        /// <summary>
        /// Gets configuration value as URL with validation
        /// </summary>
        public static string GetUrl(this IConfiguration configuration, string key, string defaultValue = null)
        {
            var value = configuration[key] ?? defaultValue;
            
            if (!string.IsNullOrWhiteSpace(value) && Uri.TryCreate(value, UriKind.Absolute, out _))
            {
                return value;
            }

            return defaultValue;
        }

        /// <summary>
        /// Gets configuration value as file path
        /// </summary>
        public static string GetFilePath(this IConfiguration configuration, string key, string defaultValue = null)
        {
            var value = configuration[key] ?? defaultValue;
            return !string.IsNullOrWhiteSpace(value) ? value : defaultValue;
        }

        #endregion

        #region Debug Helpers

        /// <summary>
        /// Gets all configuration keys and values for debugging
        /// </summary>
        public static Dictionary<string, string> GetAllSettings(this IConfiguration configuration)
        {
            var settings = new Dictionary<string, string>();

            void AddSettings(IConfiguration config, string prefix = "")
            {
                foreach (var child in config.GetChildren())
                {
                    var key = string.IsNullOrEmpty(prefix) ? child.Key : $"{prefix}:{child.Key}";

                    if (child.Value != null)
                    {
                        settings[key] = child.Value;
                    }
                    else
                    {
                        AddSettings(child, key);
                    }
                }
            }

            AddSettings(configuration);
            return settings;
        }

        /// <summary>
        /// Dumps configuration to string for debugging
        /// </summary>
        public static string DumpConfiguration(this IConfiguration configuration, bool includeSensitive = false)
        {
            var settings = configuration.GetAllSettings();
            var sensitiveKeys = new[] { "password", "secret", "key", "token", "connectionstring" };

            var lines = settings.Select(kvp =>
            {
                var value = kvp.Value;

                if (!includeSensitive)
                {
                    var isSensitive = sensitiveKeys.Any(sk => 
                        kvp.Key.Contains(sk, StringComparison.OrdinalIgnoreCase));

                    if (isSensitive)
                        value = "***REDACTED***";
                }

                return $"{kvp.Key} = {value}";
            });

            return string.Join(Environment.NewLine, lines);
        }

        #endregion
    }
}
