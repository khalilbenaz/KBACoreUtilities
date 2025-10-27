using System.Diagnostics;
using System.Text.Json;

namespace KBA.CoreUtilities.Utilities
{
    public static class LoggingUtils
    {
        private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false
        };

        public static void LogInformation(string message, object? context = null)
        {
            var logMessage = FormatMessage(message, context);
            Console.WriteLine($"[INFO] {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} - {logMessage}");
        }

        public static void LogWarning(string message, object? context = null)
        {
            var logMessage = FormatMessage(message, context);
            Console.WriteLine($"[WARN] {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} - {logMessage}");
        }

        public static void LogError(string message, Exception? exception = null, object? context = null)
        {
            var logMessage = FormatMessage(message, context);
            if (exception != null)
            {
                Console.WriteLine($"[ERROR] {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} - {logMessage} - Exception: {exception.Message}");
            }
            else
            {
                Console.WriteLine($"[ERROR] {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} - {logMessage}");
            }
        }

        public static void LogDebug(string message, object? context = null)
        {
            var logMessage = FormatMessage(message, context);
            Console.WriteLine($"[DEBUG] {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} - {logMessage}");
        }

        public static void LogTransactionEvent(string transactionId, string eventType, object? details = null)
        {
            LogInformation($"Transaction {eventType}", new { TransactionId = transactionId, EventType = eventType, Details = details });
        }

        public static void LogApiCall(string endpoint, string method, int statusCode, long duration, object? request = null, object? response = null)
        {
            LogInformation($"API call completed", new 
            { 
                Endpoint = endpoint, 
                Method = method, 
                StatusCode = statusCode, 
                Duration = duration,
                Request = request,
                Response = response
            });
        }

        public static void LogSecurityEvent(string eventType, string? userId = null, string? ipAddress = null, object? details = null)
        {
            LogWarning($"Security event: {eventType}", new 
            { 
                EventType = eventType, 
                UserId = userId, 
                IpAddress = ipAddress, 
                Details = details,
                Timestamp = DateTime.UtcNow
            });
        }

        public static void LogBusinessEvent(string eventType, string? category = null, object? details = null)
        {
            LogInformation($"Business event: {eventType}", new 
            { 
                EventType = eventType, 
                Category = category, 
                Details = details,
                Timestamp = DateTime.UtcNow
            });
        }

        public static void LogDataAccess(string operation, string entity, long duration, bool success, object? parameters = null)
        {
            var message = $"Data access {operation} on {entity} {(success ? "completed" : "failed")} in {duration}ms";
            
            if (success)
            {
                LogInformation(message, new { Operation = operation, Entity = entity, Duration = duration, Parameters = parameters });
            }
            else
            {
                LogError(message, null, new { Operation = operation, Entity = entity, Duration = duration, Parameters = parameters });
            }
        }

        public static void LogCacheOperation(string operation, string key, bool success, long? durationMs = null)
        {
            var message = $"Cache {operation} for key '{key}' {(success ? "completed" : "failed")}";
            
            if (success)
            {
                LogInformation(message, new { Operation = operation, Key = key, DurationMs = durationMs });
            }
            else
            {
                LogWarning(message, new { Operation = operation, Key = key, DurationMs = durationMs });
            }
        }

        private static string FormatMessage(string message, object? context)
        {
            if (context == null)
                return message;

            try
            {
                var contextJson = JsonSerializer.Serialize(context, JsonOptions);
                return $"{message} | Context: {contextJson}";
            }
            catch
            {
                return $"{message} | Context: {context}";
            }
        }

        public class PerformanceLogger : IDisposable
        {
            private readonly string _operationName;
            private readonly Stopwatch _stopwatch;

            public PerformanceLogger(string operationName)
            {
                _operationName = operationName;
                _stopwatch = Stopwatch.StartNew();
                LogInformation($"Starting operation: {_operationName}");
            }

            public void Dispose()
            {
                _stopwatch.Stop();
                LogInformation($"Completed operation: {_operationName} in {_stopwatch.ElapsedMilliseconds}ms");
            }

            public void LogFailure(Exception exception)
            {
                _stopwatch.Stop();
                LogError($"Operation {_operationName} failed after {_stopwatch.ElapsedMilliseconds}ms", exception);
            }
        }
    }
}
