using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Caching;
using System.Threading.Tasks;

namespace KBA.CoreUtilities.Utilities
{
    /// <summary>
    /// Performance monitoring and caching utilities
    /// </summary>
    public static class PerformanceUtils
    {
        #region Execution Time Measurement

        /// <summary>
        /// Measures execution time of an action
        /// </summary>
        public static TimeSpan Measure(Action action)
        {
            var sw = Stopwatch.StartNew();
            action();
            sw.Stop();
            return sw.Elapsed;
        }

        /// <summary>
        /// Measures execution time of a function
        /// </summary>
        public static (T result, TimeSpan elapsed) Measure<T>(Func<T> func)
        {
            var sw = Stopwatch.StartNew();
            var result = func();
            sw.Stop();
            return (result, sw.Elapsed);
        }

        /// <summary>
        /// Measures execution time of an async action
        /// </summary>
        public static async Task<TimeSpan> MeasureAsync(Func<Task> asyncAction)
        {
            var sw = Stopwatch.StartNew();
            await asyncAction();
            sw.Stop();
            return sw.Elapsed;
        }

        /// <summary>
        /// Measures execution time of an async function
        /// </summary>
        public static async Task<(T result, TimeSpan elapsed)> MeasureAsync<T>(Func<Task<T>> asyncFunc)
        {
            var sw = Stopwatch.StartNew();
            var result = await asyncFunc();
            sw.Stop();
            return (result, sw.Elapsed);
        }

        /// <summary>
        /// Executes action and logs execution time
        /// </summary>
        public static T MeasureAndLog<T>(Func<T> func, string operationName, Action<string, TimeSpan> logger = null)
        {
            var sw = Stopwatch.StartNew();
            var result = func();
            sw.Stop();
            
            logger?.Invoke(operationName, sw.Elapsed);
            
            return result;
        }

        #endregion

        #region Memory Monitoring

        /// <summary>
        /// Gets current memory usage in bytes
        /// </summary>
        public static long GetMemoryUsage()
        {
            return GC.GetTotalMemory(false);
        }

        /// <summary>
        /// Gets current memory usage formatted
        /// </summary>
        public static string GetMemoryUsageFormatted()
        {
            var bytes = GetMemoryUsage();
            return FormatBytes(bytes);
        }

        /// <summary>
        /// Measures memory used by an action
        /// </summary>
        public static (T result, long memoryUsed) MeasureMemory<T>(Func<T> func)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            var memoryBefore = GC.GetTotalMemory(true);
            var result = func();
            var memoryAfter = GC.GetTotalMemory(true);

            return (result, memoryAfter - memoryBefore);
        }

        /// <summary>
        /// Forces garbage collection
        /// </summary>
        public static void ForceGC()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }

        private static string FormatBytes(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            double len = bytes;
            var order = 0;

            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len /= 1024;
            }

            return $"{len:0.##} {sizes[order]}";
        }

        #endregion

        #region In-Memory Caching

        private static readonly MemoryCache DefaultCache = MemoryCache.Default;

        /// <summary>
        /// Gets value from cache or executes factory and caches result
        /// </summary>
        public static T GetOrAdd<T>(string key, Func<T> factory, TimeSpan? expiration = null)
        {
            if (DefaultCache.Contains(key))
            {
                return (T)DefaultCache[key];
            }

            var value = factory();
            var policy = new CacheItemPolicy();
            
            if (expiration.HasValue)
            {
                policy.AbsoluteExpiration = DateTimeOffset.Now.Add(expiration.Value);
            }

            DefaultCache.Add(key, value, policy);
            return value;
        }

        /// <summary>
        /// Gets value from cache or executes async factory and caches result
        /// </summary>
        public static async Task<T> GetOrAddAsync<T>(string key, Func<Task<T>> factory, TimeSpan? expiration = null)
        {
            if (DefaultCache.Contains(key))
            {
                return (T)DefaultCache[key];
            }

            var value = await factory();
            var policy = new CacheItemPolicy();

            if (expiration.HasValue)
            {
                policy.AbsoluteExpiration = DateTimeOffset.Now.Add(expiration.Value);
            }

            DefaultCache.Add(key, value, policy);
            return value;
        }

        /// <summary>
        /// Removes item from cache
        /// </summary>
        public static void RemoveFromCache(string key)
        {
            DefaultCache.Remove(key);
        }

        /// <summary>
        /// Clears all cache
        /// </summary>
        public static void ClearCache()
        {
            foreach (var item in DefaultCache)
            {
                DefaultCache.Remove(item.Key);
            }
        }

        /// <summary>
        /// Checks if key exists in cache
        /// </summary>
        public static bool ExistsInCache(string key)
        {
            return DefaultCache.Contains(key);
        }

        /// <summary>
        /// Sets value in cache
        /// </summary>
        public static void SetCache<T>(string key, T value, TimeSpan? expiration = null)
        {
            var policy = new CacheItemPolicy();

            if (expiration.HasValue)
            {
                policy.AbsoluteExpiration = DateTimeOffset.Now.Add(expiration.Value);
            }

            DefaultCache.Set(key, value, policy);
        }

        /// <summary>
        /// Gets value from cache
        /// </summary>
        public static T GetFromCache<T>(string key, T defaultValue = default)
        {
            if (DefaultCache.Contains(key))
            {
                return (T)DefaultCache[key];
            }

            return defaultValue;
        }

        #endregion

        #region Profiling

        /// <summary>
        /// Simple profiler to track operation durations
        /// </summary>
        public class SimpleProfiler : IDisposable
        {
            private readonly Stopwatch _stopwatch;
            private readonly string _operationName;
            private readonly Action<string, TimeSpan> _onComplete;

            public SimpleProfiler(string operationName, Action<string, TimeSpan> onComplete = null)
            {
                _operationName = operationName;
                _onComplete = onComplete;
                _stopwatch = Stopwatch.StartNew();
            }

            public void Dispose()
            {
                _stopwatch.Stop();
                _onComplete?.Invoke(_operationName, _stopwatch.Elapsed);
            }

            public TimeSpan Elapsed => _stopwatch.Elapsed;
        }

        /// <summary>
        /// Creates a profiler for an operation
        /// </summary>
        public static SimpleProfiler Profile(string operationName, Action<string, TimeSpan> onComplete = null)
        {
            return new SimpleProfiler(operationName, onComplete);
        }

        #endregion

        #region Rate Limiting

        private static readonly Dictionary<string, (DateTime lastCall, int callCount)> RateLimiters = new();

        /// <summary>
        /// Checks if operation is rate limited
        /// </summary>
        public static bool IsRateLimited(string key, int maxCalls, TimeSpan timeWindow)
        {
            lock (RateLimiters)
            {
                if (!RateLimiters.ContainsKey(key))
                {
                    RateLimiters[key] = (DateTime.UtcNow, 1);
                    return false;
                }

                var (lastCall, callCount) = RateLimiters[key];
                var elapsed = DateTime.UtcNow - lastCall;

                if (elapsed > timeWindow)
                {
                    RateLimiters[key] = (DateTime.UtcNow, 1);
                    return false;
                }

                if (callCount >= maxCalls)
                {
                    return true;
                }

                RateLimiters[key] = (lastCall, callCount + 1);
                return false;
            }
        }

        /// <summary>
        /// Resets rate limiter for key
        /// </summary>
        public static void ResetRateLimit(string key)
        {
            lock (RateLimiters)
            {
                RateLimiters.Remove(key);
            }
        }

        #endregion

        #region Benchmarking

        /// <summary>
        /// Runs benchmark on an action multiple times
        /// </summary>
        public static BenchmarkResult Benchmark(Action action, int iterations = 1000)
        {
            var times = new List<TimeSpan>();

            // Warmup
            action();

            for (var i = 0; i < iterations; i++)
            {
                var sw = Stopwatch.StartNew();
                action();
                sw.Stop();
                times.Add(sw.Elapsed);
            }

            return new BenchmarkResult
            {
                Iterations = iterations,
                TotalTime = TimeSpan.FromTicks(times.Sum(t => t.Ticks)),
                AverageTime = TimeSpan.FromTicks((long)times.Average(t => t.Ticks)),
                MinTime = TimeSpan.FromTicks(times.Min(t => t.Ticks)),
                MaxTime = TimeSpan.FromTicks(times.Max(t => t.Ticks))
            };
        }

        public class BenchmarkResult
        {
            public int Iterations { get; set; }
            public TimeSpan TotalTime { get; set; }
            public TimeSpan AverageTime { get; set; }
            public TimeSpan MinTime { get; set; }
            public TimeSpan MaxTime { get; set; }

            public override string ToString()
            {
                return $"Iterations: {Iterations}, " +
                       $"Total: {TotalTime.TotalMilliseconds:F2}ms, " +
                       $"Avg: {AverageTime.TotalMilliseconds:F2}ms, " +
                       $"Min: {MinTime.TotalMilliseconds:F2}ms, " +
                       $"Max: {MaxTime.TotalMilliseconds:F2}ms";
            }
        }

        #endregion
    }
}
