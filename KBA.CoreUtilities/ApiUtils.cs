using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace KBA.CoreUtilities.Utilities
{
    /// <summary>
    /// Utility class for REST API consumption operations
    /// </summary>
    public static class ApiUtils
    {
        private static readonly HttpClient DefaultHttpClient = new();
        private static readonly JsonSerializerOptions DefaultJsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        };

        #region GET Operations

        /// <summary>
        /// Makes a GET request and returns JSON response as specified type
        /// </summary>
        public static async Task<T> GetJsonAsync<T>(string url, HttpClient httpClient = null)
        {
            var client = httpClient ?? DefaultHttpClient;
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(content, DefaultJsonOptions);
        }

        /// <summary>
        /// Makes a GET request and returns JSON response as specified type with cancellation token
        /// </summary>
        public static async Task<T> GetJsonAsync<T>(string url, CancellationToken cancellationToken, HttpClient httpClient = null)
        {
            var client = httpClient ?? DefaultHttpClient;
            var response = await client.GetAsync(url, cancellationToken);
            response.EnsureSuccessStatusCode();
            
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(content, DefaultJsonOptions);
        }

        /// <summary>
        /// Makes a GET request and returns string response
        /// </summary>
        public static async Task<string> GetStringAsync(string url, HttpClient httpClient = null)
        {
            var client = httpClient ?? DefaultHttpClient;
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// Makes a GET request and returns byte array response
        /// </summary>
        public static async Task<byte[]> GetByteArrayAsync(string url, HttpClient httpClient = null)
        {
            var client = httpClient ?? DefaultHttpClient;
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsByteArrayAsync();
        }

        /// <summary>
        /// Makes a GET request and returns stream response
        /// </summary>
        public static async Task<Stream> GetStreamAsync(string url, HttpClient httpClient = null)
        {
            var client = httpClient ?? DefaultHttpClient;
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStreamAsync();
        }

        #endregion

        #region POST Operations

        /// <summary>
        /// Makes a POST request with JSON payload and returns JSON response as specified type
        /// </summary>
        public static async Task<TResponse> PostJsonAsync<TRequest, TResponse>(string url, TRequest data, HttpClient httpClient = null)
        {
            var client = httpClient ?? DefaultHttpClient;
            var json = JsonSerializer.Serialize(data, DefaultJsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            var response = await client.PostAsync(url, content);
            response.EnsureSuccessStatusCode();
            
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<TResponse>(responseContent, DefaultJsonOptions);
        }

        /// <summary>
        /// Makes a POST request with JSON payload and returns string response
        /// </summary>
        public static async Task<string> PostJsonAsync<T>(string url, T data, HttpClient httpClient = null)
        {
            var client = httpClient ?? DefaultHttpClient;
            var json = JsonSerializer.Serialize(data, DefaultJsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            var response = await client.PostAsync(url, content);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// Makes a POST request with form data and returns string response
        /// </summary>
        public static async Task<string> PostFormAsync(string url, Dictionary<string, string> formData, HttpClient httpClient = null)
        {
            var client = httpClient ?? DefaultHttpClient;
            var content = new FormUrlEncodedContent(formData);
            
            var response = await client.PostAsync(url, content);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// Makes a POST request with multipart form data and returns string response
        /// </summary>
        public static async Task<string> PostMultipartAsync(string url, MultipartFormDataContent content, HttpClient httpClient = null)
        {
            var client = httpClient ?? DefaultHttpClient;
            var response = await client.PostAsync(url, content);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        #endregion

        #region PUT Operations

        /// <summary>
        /// Makes a PUT request with JSON payload and returns JSON response as specified type
        /// </summary>
        public static async Task<TResponse> PutJsonAsync<TRequest, TResponse>(string url, TRequest data, HttpClient httpClient = null)
        {
            var client = httpClient ?? DefaultHttpClient;
            var json = JsonSerializer.Serialize(data, DefaultJsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            var response = await client.PutAsync(url, content);
            response.EnsureSuccessStatusCode();
            
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<TResponse>(responseContent, DefaultJsonOptions);
        }

        /// <summary>
        /// Makes a PUT request with JSON payload and returns string response
        /// </summary>
        public static async Task<string> PutJsonAsync<T>(string url, T data, HttpClient httpClient = null)
        {
            var client = httpClient ?? DefaultHttpClient;
            var json = JsonSerializer.Serialize(data, DefaultJsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            var response = await client.PutAsync(url, content);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        #endregion

        #region DELETE Operations

        /// <summary>
        /// Makes a DELETE request and returns JSON response as specified type
        /// </summary>
        public static async Task<T> DeleteJsonAsync<T>(string url, HttpClient httpClient = null)
        {
            var client = httpClient ?? DefaultHttpClient;
            var response = await client.DeleteAsync(url);
            response.EnsureSuccessStatusCode();
            
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(content, DefaultJsonOptions);
        }

        /// <summary>
        /// Makes a DELETE request and returns string response
        /// </summary>
        public static async Task<string> DeleteStringAsync(string url, HttpClient httpClient = null)
        {
            var client = httpClient ?? DefaultHttpClient;
            var response = await client.DeleteAsync(url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        #endregion

        #region GraphQL Operations

        /// <summary>
        /// Executes GraphQL query and returns response as specified type
        /// </summary>
        public static async Task<T> ExecuteGraphQLAsync<T>(string url, string query, object variables = null, HttpClient httpClient = null)
        {
            var client = httpClient ?? DefaultHttpClient;
            
            var request = new
            {
                query = query,
                variables = variables
            };
            
            var json = JsonSerializer.Serialize(request, DefaultJsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            var response = await client.PostAsync(url, content);
            response.EnsureSuccessStatusCode();
            
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(responseContent, DefaultJsonOptions);
        }

        #endregion

        #region Utility Methods

        /// <summary>
        /// Creates HttpClient with custom headers
        /// </summary>
        public static HttpClient CreateHttpClient(Dictionary<string, string> headers = null, TimeSpan? timeout = null)
        {
            var client = new HttpClient();
            
            if (headers != null)
            {
                foreach (var header in headers)
                {
                    client.DefaultRequestHeaders.TryAddWithoutValidation(header.Key, header.Value);
                }
            }
            
            if (timeout.HasValue)
            {
                client.Timeout = timeout.Value;
            }
            
            return client;
        }

        /// <summary>
        /// Creates HttpClient with Bearer token authentication
        /// </summary>
        public static HttpClient CreateAuthenticatedHttpClient(string bearerToken, TimeSpan? timeout = null)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
            
            if (timeout.HasValue)
            {
                client.Timeout = timeout.Value;
            }
            
            return client;
        }

        /// <summary>
        /// Creates HttpClient with Basic authentication
        /// </summary>
        public static HttpClient CreateBasicAuthHttpClient(string username, string password, TimeSpan? timeout = null)
        {
            var client = new HttpClient();
            var byteArray = Encoding.ASCII.GetBytes($"{username}:{password}");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
            
            if (timeout.HasValue)
            {
                client.Timeout = timeout.Value;
            }
            
            return client;
        }

        /// <summary>
        /// Builds query string from parameters
        /// </summary>
        public static string BuildQueryString(string baseUrl, Dictionary<string, object> parameters)
        {
            if (parameters == null || parameters.Count == 0)
                return baseUrl;
            
            var queryString = HttpUtility.ParseQueryString(string.Empty);
            foreach (var param in parameters)
            {
                if (param.Value != null)
                {
                    queryString[param.Key] = param.Value.ToString();
                }
            }
            
            var separator = baseUrl.Contains("?") ? "&" : "?";
            return $"{baseUrl}{separator}{queryString}";
        }

        /// <summary>
        /// Makes HTTP request with retry policy
        /// </summary>
        public static async Task<T> ExecuteWithRetryAsync<T>(Func<Task<T>> operation, int maxRetries = 3, int delayMs = 1000)
        {
            int retryCount = 0;
            
            while (true)
            {
                try
                {
                    return await operation();
                }
                catch (HttpRequestException) when (retryCount < maxRetries)
                {
                    retryCount++;
                    await Task.Delay(delayMs * retryCount);
                }
            }
        }

        /// <summary>
        /// Downloads file from URL to specified path
        /// </summary>
        public static async Task DownloadFileAsync(string url, string filePath, HttpClient httpClient = null)
        {
            var client = httpClient ?? DefaultHttpClient;
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            
            using var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None);
            await response.Content.CopyToAsync(fileStream);
        }

        /// <summary>
        /// Uploads file to URL with multipart form data
        /// </summary>
        public static async Task<string> UploadFileAsync(string url, string filePath, string parameterName = "file", Dictionary<string, string> additionalFields = null, HttpClient httpClient = null)
        {
            var client = httpClient ?? DefaultHttpClient;
            
            using var content = new MultipartFormDataContent();
            using var fileStream = File.OpenRead(filePath);
            using var fileContent = new StreamContent(fileStream);
            
            fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/octet-stream");
            content.Add(fileContent, parameterName, Path.GetFileName(filePath));
            
            if (additionalFields != null)
            {
                foreach (var field in additionalFields)
                {
                    content.Add(new StringContent(field.Value), field.Key);
                }
            }
            
            var response = await client.PostAsync(url, content);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        #endregion
    }
}
