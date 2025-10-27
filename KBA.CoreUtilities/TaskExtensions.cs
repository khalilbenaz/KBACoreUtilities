using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace KBA.CoreUtilities.Extensions
{
    /// <summary>
    /// Extension methods for Task and async operations
    /// </summary>
    public static class TaskExtensions
    {
        #region Timeout

        /// <summary>
        /// Adds timeout to a task
        /// </summary>
        public static async Task<T> WithTimeout<T>(this Task<T> task, TimeSpan timeout)
        {
            if (task == await Task.WhenAny(task, Task.Delay(timeout)))
            {
                return await task;
            }

            throw new TimeoutException($"The operation timed out after {timeout.TotalSeconds} seconds");
        }

        /// <summary>
        /// Adds timeout to a task
        /// </summary>
        public static async Task WithTimeout(this Task task, TimeSpan timeout)
        {
            if (task == await Task.WhenAny(task, Task.Delay(timeout)))
            {
                await task;
                return;
            }

            throw new TimeoutException($"The operation timed out after {timeout.TotalSeconds} seconds");
        }

        /// <summary>
        /// Adds timeout with cancellation token
        /// </summary>
        public static async Task<T> WithTimeout<T>(this Task<T> task, TimeSpan timeout, CancellationToken cancellationToken)
        {
            using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            cts.CancelAfter(timeout);

            try
            {
                return await task.WaitAsync(cts.Token);
            }
            catch (OperationCanceledException) when (!cancellationToken.IsCancellationRequested)
            {
                throw new TimeoutException($"The operation timed out after {timeout.TotalSeconds} seconds");
            }
        }

        #endregion

        #region Retry

        /// <summary>
        /// Retries task on failure with exponential backoff
        /// </summary>
        public static async Task<T> Retry<T>(
            this Func<Task<T>> taskFactory,
            int maxRetries = 3,
            TimeSpan? initialDelay = null,
            double backoffMultiplier = 2.0)
        {
            var delay = initialDelay ?? TimeSpan.FromSeconds(1);
            Exception lastException = null;

            for (var i = 0; i <= maxRetries; i++)
            {
                try
                {
                    return await taskFactory();
                }
                catch (Exception ex)
                {
                    lastException = ex;

                    if (i < maxRetries)
                    {
                        await Task.Delay(delay);
                        delay = TimeSpan.FromMilliseconds(delay.TotalMilliseconds * backoffMultiplier);
                    }
                }
            }

            throw new AggregateException($"Operation failed after {maxRetries + 1} attempts", lastException);
        }

        /// <summary>
        /// Retries task on failure
        /// </summary>
        public static async Task Retry(
            this Func<Task> taskFactory,
            int maxRetries = 3,
            TimeSpan? initialDelay = null,
            double backoffMultiplier = 2.0)
        {
            var delay = initialDelay ?? TimeSpan.FromSeconds(1);
            Exception lastException = null;

            for (var i = 0; i <= maxRetries; i++)
            {
                try
                {
                    await taskFactory();
                    return;
                }
                catch (Exception ex)
                {
                    lastException = ex;

                    if (i < maxRetries)
                    {
                        await Task.Delay(delay);
                        delay = TimeSpan.FromMilliseconds(delay.TotalMilliseconds * backoffMultiplier);
                    }
                }
            }

            throw new AggregateException($"Operation failed after {maxRetries + 1} attempts", lastException);
        }

        #endregion

        #region WaitAll/WhenAll

        /// <summary>
        /// Waits for all tasks with cancellation support
        /// </summary>
        public static async Task WaitAllAsync(this Task[] tasks, CancellationToken cancellationToken = default)
        {
            await Task.WhenAll(tasks).WaitAsync(cancellationToken);
        }

        /// <summary>
        /// Waits for all tasks with timeout
        /// </summary>
        public static async Task WaitAllAsync(this Task[] tasks, TimeSpan timeout)
        {
            await Task.WhenAll(tasks).WithTimeout(timeout);
        }

        /// <summary>
        /// Waits for any task to complete
        /// </summary>
        public static async Task<Task> WhenAnyAsync(this Task[] tasks, CancellationToken cancellationToken = default)
        {
            return await Task.WhenAny(tasks).WaitAsync(cancellationToken);
        }

        #endregion

        #region Fire and Forget

        /// <summary>
        /// Fires and forgets a task (handles exceptions silently)
        /// </summary>
        public static async void FireAndForget(this Task task, Action<Exception> onException = null)
        {
            try
            {
                await task;
            }
            catch (Exception ex)
            {
                onException?.Invoke(ex);
            }
        }

        /// <summary>
        /// Continues with another task
        /// </summary>
        public static async Task<TResult> Then<T, TResult>(this Task<T> task, Func<T, Task<TResult>> continuation)
        {
            var result = await task;
            return await continuation(result);
        }

        /// <summary>
        /// Continues with another task
        /// </summary>
        public static async Task<TResult> Then<T, TResult>(this Task<T> task, Func<T, TResult> continuation)
        {
            var result = await task;
            return continuation(result);
        }

        #endregion

        #region ConfigureAwait

        /// <summary>
        /// Suppresses context capture (equivalent to ConfigureAwait(false))
        /// </summary>
        public static ConfiguredTaskAwaitable SuppressContext(this Task task)
        {
            return task.ConfigureAwait(false);
        }

        /// <summary>
        /// Suppresses context capture (equivalent to ConfigureAwait(false))
        /// </summary>
        public static ConfiguredTaskAwaitable<T> SuppressContext<T>(this Task<T> task)
        {
            return task.ConfigureAwait(false);
        }

        #endregion

        #region Parallel Operations

        /// <summary>
        /// Runs tasks in parallel with degree of parallelism limit
        /// </summary>
        public static async Task<T[]> RunInParallel<T>(
            this Task<T>[] tasks,
            int maxDegreeOfParallelism = 10)
        {
            var semaphore = new SemaphoreSlim(maxDegreeOfParallelism);
            var wrappedTasks = tasks.Select(async task =>
            {
                await semaphore.WaitAsync();
                try
                {
                    return await task;
                }
                finally
                {
                    semaphore.Release();
                }
            });

            return await Task.WhenAll(wrappedTasks);
        }

        #endregion

        #region Exception Handling

        /// <summary>
        /// Handles exceptions from task
        /// </summary>
        public static async Task<T> HandleException<T>(
            this Task<T> task,
            Func<Exception, T> handler)
        {
            try
            {
                return await task;
            }
            catch (Exception ex)
            {
                return handler(ex);
            }
        }

        /// <summary>
        /// Handles exceptions from task
        /// </summary>
        public static async Task HandleException(
            this Task task,
            Action<Exception> handler)
        {
            try
            {
                await task;
            }
            catch (Exception ex)
            {
                handler(ex);
            }
        }

        /// <summary>
        /// Returns default value on exception
        /// </summary>
        public static async Task<T> OrDefault<T>(this Task<T> task, T defaultValue = default)
        {
            try
            {
                return await task;
            }
            catch
            {
                return defaultValue;
            }
        }

        #endregion

        #region Lazy Async

        /// <summary>
        /// Creates a lazy async operation
        /// </summary>
        public static Lazy<Task<T>> ToLazy<T>(this Func<Task<T>> taskFactory)
        {
            return new Lazy<Task<T>>(taskFactory);
        }

        #endregion
    }
}
