using System;
using System.Collections.Generic;
using System.Linq;

namespace KBA.CoreUtilities.Extensions
{
    /// <summary>
    /// Extension methods for collections (IEnumerable, ICollection, List)
    /// </summary>
    public static class CollectionExtensions
    {
        #region Null and Empty Checks

        /// <summary>
        /// Checks if collection is null or empty
        /// </summary>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> source)
        {
            return source == null || !source.Any();
        }

        /// <summary>
        /// Checks if collection has any elements
        /// </summary>
        public static bool HasAny<T>(this IEnumerable<T> source)
        {
            return source != null && source.Any();
        }

        /// <summary>
        /// Returns empty collection if null
        /// </summary>
        public static IEnumerable<T> OrEmpty<T>(this IEnumerable<T> source)
        {
            return source ?? Enumerable.Empty<T>();
        }

        #endregion

        #region ForEach and Actions

        /// <summary>
        /// Executes action for each element
        /// </summary>
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            if (source == null || action == null)
                return;

            foreach (var item in source)
            {
                action(item);
            }
        }

        /// <summary>
        /// Executes action for each element with index
        /// </summary>
        public static void ForEach<T>(this IEnumerable<T> source, Action<T, int> action)
        {
            if (source == null || action == null)
                return;

            var index = 0;
            foreach (var item in source)
            {
                action(item, index++);
            }
        }

        #endregion

        #region Distinct and Unique

        /// <summary>
        /// Returns distinct elements by key selector
        /// </summary>
        public static IEnumerable<T> DistinctBy<T, TKey>(this IEnumerable<T> source, Func<T, TKey> keySelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            var knownKeys = new HashSet<TKey>();
            foreach (var element in source)
            {
                if (knownKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

        #endregion

        #region Chunking and Batching

        /// <summary>
        /// Splits collection into chunks of specified size
        /// </summary>
        public static IEnumerable<IEnumerable<T>> Chunk<T>(this IEnumerable<T> source, int chunkSize)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            
            if (chunkSize <= 0)
                throw new ArgumentException("Chunk size must be greater than 0", nameof(chunkSize));

            var chunk = new List<T>(chunkSize);
            
            foreach (var item in source)
            {
                chunk.Add(item);
                
                if (chunk.Count == chunkSize)
                {
                    yield return chunk;
                    chunk = new List<T>(chunkSize);
                }
            }

            if (chunk.Any())
            {
                yield return chunk;
            }
        }

        /// <summary>
        /// Splits collection into specified number of batches
        /// </summary>
        public static IEnumerable<IEnumerable<T>> Batch<T>(this IEnumerable<T> source, int batchCount)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            var list = source.ToList();
            var batchSize = (int)Math.Ceiling((double)list.Count / batchCount);

            return list.Chunk(batchSize);
        }

        #endregion

        #region Shuffle and Random

        /// <summary>
        /// Shuffles collection randomly
        /// </summary>
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            var list = source.ToList();
            var rng = new Random();
            var n = list.Count;

            while (n > 1)
            {
                n--;
                var k = rng.Next(n + 1);
                (list[k], list[n]) = (list[n], list[k]);
            }

            return list;
        }

        /// <summary>
        /// Returns random element from collection
        /// </summary>
        public static T Random<T>(this IEnumerable<T> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            var list = source.ToList();
            if (!list.Any())
                throw new InvalidOperationException("Collection is empty");

            var rng = new Random();
            return list[rng.Next(list.Count)];
        }

        /// <summary>
        /// Returns n random elements from collection
        /// </summary>
        public static IEnumerable<T> RandomElements<T>(this IEnumerable<T> source, int count)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Shuffle().Take(count);
        }

        #endregion

        #region String Operations

        /// <summary>
        /// Joins elements into string with separator
        /// </summary>
        public static string JoinString<T>(this IEnumerable<T> source, string separator = ", ")
        {
            if (source == null)
                return string.Empty;

            return string.Join(separator, source);
        }

        /// <summary>
        /// Joins elements with custom formatter
        /// </summary>
        public static string JoinString<T>(this IEnumerable<T> source, Func<T, string> formatter, string separator = ", ")
        {
            if (source == null)
                return string.Empty;

            return string.Join(separator, source.Select(formatter));
        }

        #endregion

        #region Filtering

        /// <summary>
        /// Filters out null values
        /// </summary>
        public static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T?> source) where T : class
        {
            return source?.Where(x => x != null)!;
        }

        /// <summary>
        /// Filters collection by type
        /// </summary>
        public static IEnumerable<T> OfTypeOrDefault<T>(this IEnumerable<object> source)
        {
            return source?.OfType<T>() ?? Enumerable.Empty<T>();
        }

        #endregion

        #region Min/Max

        /// <summary>
        /// Returns element with minimum value by selector
        /// </summary>
        public static T MinBy<T, TKey>(this IEnumerable<T> source, Func<T, TKey> selector) where TKey : IComparable<TKey>
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.OrderBy(selector).First();
        }

        /// <summary>
        /// Returns element with maximum value by selector
        /// </summary>
        public static T MaxBy<T, TKey>(this IEnumerable<T> source, Func<T, TKey> selector) where TKey : IComparable<TKey>
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.OrderByDescending(selector).First();
        }

        #endregion

        #region Dictionary Operations

        /// <summary>
        /// Converts IEnumerable to Dictionary safely
        /// </summary>
        public static Dictionary<TKey, TValue> ToDictionarySafe<TSource, TKey, TValue>(
            this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector,
            Func<TSource, TValue> valueSelector) where TKey : notnull
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            var dictionary = new Dictionary<TKey, TValue>();
            
            foreach (var item in source)
            {
                var key = keySelector(item);
                if (!dictionary.ContainsKey(key))
                {
                    dictionary[key] = valueSelector(item);
                }
            }

            return dictionary;
        }

        /// <summary>
        /// Gets value from dictionary or default
        /// </summary>
        public static TValue GetValueOrDefault<TKey, TValue>(
            this IDictionary<TKey, TValue> dictionary,
            TKey key,
            TValue defaultValue = default)
        {
            return dictionary != null && dictionary.TryGetValue(key, out var value) ? value : defaultValue;
        }

        /// <summary>
        /// Adds value to dictionary if key doesn't exist
        /// </summary>
        public static bool AddIfNotExists<TKey, TValue>(
            this IDictionary<TKey, TValue> dictionary,
            TKey key,
            TValue value)
        {
            if (dictionary == null)
                throw new ArgumentNullException(nameof(dictionary));

            if (dictionary.ContainsKey(key))
                return false;

            dictionary[key] = value;
            return true;
        }

        #endregion

        #region Paging

        /// <summary>
        /// Returns page of results
        /// </summary>
        public static IEnumerable<T> Page<T>(this IEnumerable<T> source, int pageNumber, int pageSize)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        }

        #endregion

        #region Conversion

        /// <summary>
        /// Converts to HashSet
        /// </summary>
        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> source)
        {
            return source == null ? new HashSet<T>() : new HashSet<T>(source);
        }

        /// <summary>
        /// Converts to Queue
        /// </summary>
        public static Queue<T> ToQueue<T>(this IEnumerable<T> source)
        {
            return source == null ? new Queue<T>() : new Queue<T>(source);
        }

        /// <summary>
        /// Converts to Stack
        /// </summary>
        public static Stack<T> ToStack<T>(this IEnumerable<T> source)
        {
            return source == null ? new Stack<T>() : new Stack<T>(source);
        }

        #endregion

        #region Aggregation

        /// <summary>
        /// Sums values by selector
        /// </summary>
        public static decimal SumOrDefault<T>(this IEnumerable<T> source, Func<T, decimal> selector, decimal defaultValue = 0)
        {
            return source?.Sum(selector) ?? defaultValue;
        }

        /// <summary>
        /// Gets average or default
        /// </summary>
        public static double AverageOrDefault<T>(this IEnumerable<T> source, Func<T, double> selector, double defaultValue = 0)
        {
            return source != null && source.Any() ? source.Average(selector) : defaultValue;
        }

        #endregion

        #region Contains and Existence

        /// <summary>
        /// Checks if collection contains any of the values
        /// </summary>
        public static bool ContainsAny<T>(this IEnumerable<T> source, params T[] values)
        {
            if (source == null || values == null)
                return false;

            return values.Any(source.Contains);
        }

        /// <summary>
        /// Checks if collection contains all of the values
        /// </summary>
        public static bool ContainsAll<T>(this IEnumerable<T> source, params T[] values)
        {
            if (source == null || values == null)
                return false;

            return values.All(source.Contains);
        }

        #endregion

        #region Comparison

        /// <summary>
        /// Checks if two collections have same elements (order independent)
        /// </summary>
        public static bool SequenceEqual<T>(this IEnumerable<T> first, IEnumerable<T> second, bool orderIndependent = false)
        {
            if (first == null && second == null)
                return true;

            if (first == null || second == null)
                return false;

            if (!orderIndependent)
                return Enumerable.SequenceEqual(first, second);

            var firstList = first.ToList();
            var secondList = second.ToList();

            if (firstList.Count != secondList.Count)
                return false;

            return !firstList.Except(secondList).Any() && !secondList.Except(firstList).Any();
        }

        #endregion

        #region Add/Remove Range

        /// <summary>
        /// Adds range of items to collection
        /// </summary>
        public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> items)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));

            if (items == null)
                return;

            foreach (var item in items)
            {
                collection.Add(item);
            }
        }

        /// <summary>
        /// Removes all items matching predicate
        /// </summary>
        public static int RemoveAll<T>(this ICollection<T> collection, Func<T, bool> predicate)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));

            var itemsToRemove = collection.Where(predicate).ToList();
            foreach (var item in itemsToRemove)
            {
                collection.Remove(item);
            }

            return itemsToRemove.Count;
        }

        #endregion
    }
}
