using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace KBA.CoreUtilities.Utilities
{
    /// <summary>
    /// Simple REST API builder for minimal configuration
    /// </summary>
    public class RestApiBuilder
    {
        private readonly List<RouteEndpoint> _endpoints = new();
        private readonly Dictionary<string, object> _services = new();
        private Action<IApplicationBuilder> _configureCallback;

        /// <summary>
        /// Maps a GET endpoint
        /// </summary>
        public RestApiBuilder MapGet<T>(string pattern, Func<T> handler)
        {
            _endpoints.Add(new RouteEndpoint
            {
                Method = "GET",
                Pattern = pattern,
                Handler = async context =>
                {
                    try
                    {
                        var result = handler();
                        await WriteJsonResponse(context, result);
                    }
                    catch (Exception ex)
                    {
                        await WriteErrorResponse(context, ex);
                    }
                }
            });
            return this;
        }

        /// <summary>
        /// Maps a GET endpoint with async handler
        /// </summary>
        public RestApiBuilder MapGet<T>(string pattern, Func<Task<T>> handler)
        {
            _endpoints.Add(new RouteEndpoint
            {
                Method = "GET",
                Pattern = pattern,
                Handler = async context =>
                {
                    try
                    {
                        var result = await handler();
                        await WriteJsonResponse(context, result);
                    }
                    catch (Exception ex)
                    {
                        await WriteErrorResponse(context, ex);
                    }
                }
            });
            return this;
        }

        /// <summary>
        /// Maps a GET endpoint with HttpContext parameter
        /// </summary>
        public RestApiBuilder MapGet(string pattern, Func<HttpContext, Task> handler)
        {
            _endpoints.Add(new RouteEndpoint
            {
                Method = "GET",
                Pattern = pattern,
                Handler = async context =>
                {
                    try
                    {
                        await handler(context);
                    }
                    catch (Exception ex)
                    {
                        await WriteErrorResponse(context, ex);
                    }
                }
            });
            return this;
        }

        /// <summary>
        /// Maps a POST endpoint
        /// </summary>
        public RestApiBuilder MapPost<TRequest, TResponse>(string pattern, Func<TRequest, TResponse> handler)
        {
            _endpoints.Add(new RouteEndpoint
            {
                Method = "POST",
                Pattern = pattern,
                Handler = async context =>
                {
                    try
                    {
                        var request = await ReadJsonRequest<TRequest>(context);
                        var result = handler(request);
                        await WriteJsonResponse(context, result);
                    }
                    catch (Exception ex)
                    {
                        await WriteErrorResponse(context, ex);
                    }
                }
            });
            return this;
        }

        /// <summary>
        /// Maps a POST endpoint with async handler
        /// </summary>
        public RestApiBuilder MapPost<TRequest, TResponse>(string pattern, Func<TRequest, Task<TResponse>> handler)
        {
            _endpoints.Add(new RouteEndpoint
            {
                Method = "POST",
                Pattern = pattern,
                Handler = async context =>
                {
                    try
                    {
                        var request = await ReadJsonRequest<TRequest>(context);
                        var result = await handler(request);
                        await WriteJsonResponse(context, result);
                    }
                    catch (Exception ex)
                    {
                        await WriteErrorResponse(context, ex);
                    }
                }
            });
            return this;
        }

        /// <summary>
        /// Maps a POST endpoint with HttpContext parameter
        /// </summary>
        public RestApiBuilder MapPost(string pattern, Func<HttpContext, Task> handler)
        {
            _endpoints.Add(new RouteEndpoint
            {
                Method = "POST",
                Pattern = pattern,
                Handler = async context =>
                {
                    try
                    {
                        await handler(context);
                    }
                    catch (Exception ex)
                    {
                        await WriteErrorResponse(context, ex);
                    }
                }
            });
            return this;
        }

        /// <summary>
        /// Maps a PUT endpoint
        /// </summary>
        public RestApiBuilder MapPut<TRequest, TResponse>(string pattern, Func<TRequest, TResponse> handler)
        {
            _endpoints.Add(new RouteEndpoint
            {
                Method = "PUT",
                Pattern = pattern,
                Handler = async context =>
                {
                    try
                    {
                        var request = await ReadJsonRequest<TRequest>(context);
                        var result = handler(request);
                        await WriteJsonResponse(context, result);
                    }
                    catch (Exception ex)
                    {
                        await WriteErrorResponse(context, ex);
                    }
                }
            });
            return this;
        }

        /// <summary>
        /// Maps a DELETE endpoint
        /// </summary>
        public RestApiBuilder MapDelete<T>(string pattern, Func<T> handler)
        {
            _endpoints.Add(new RouteEndpoint
            {
                Method = "DELETE",
                Pattern = pattern,
                Handler = async context =>
                {
                    try
                    {
                        var result = handler();
                        await WriteJsonResponse(context, result);
                    }
                    catch (Exception ex)
                    {
                        await WriteErrorResponse(context, ex);
                    }
                }
            });
            return this;
        }

        /// <summary>
        /// Maps a DELETE endpoint with HttpContext parameter
        /// </summary>
        public RestApiBuilder MapDelete(string pattern, Func<HttpContext, Task> handler)
        {
            _endpoints.Add(new RouteEndpoint
            {
                Method = "DELETE",
                Pattern = pattern,
                Handler = async context =>
                {
                    try
                    {
                        await handler(context);
                    }
                    catch (Exception ex)
                    {
                        await WriteErrorResponse(context, ex);
                    }
                }
            });
            return this;
        }

        /// <summary>
        /// Adds a service to the dependency injection container
        /// </summary>
        public RestApiBuilder AddService<T>(T service) where T : class
        {
            _services[typeof(T).Name] = service;
            return this;
        }

        /// <summary>
        /// Configures additional application settings
        /// </summary>
        public RestApiBuilder Configure(Action<IApplicationBuilder> configure)
        {
            _configureCallback = configure;
            return this;
        }

        /// <summary>
        /// Builds the web application
        /// </summary>
        public WebApplication Build()
        {
            var builder = WebApplication.CreateBuilder();
            
            // Add services
            foreach (var service in _services)
            {
                builder.Services.AddSingleton(service.Value.GetType(), service.Value);
            }
            
            var app = builder.Build();
            
            // Configure endpoints
            foreach (var endpoint in _endpoints)
            {
                switch (endpoint.Method.ToUpper())
                {
                    case "GET":
                        app.MapGet(endpoint.Pattern, endpoint.Handler);
                        break;
                    case "POST":
                        app.MapPost(endpoint.Pattern, endpoint.Handler);
                        break;
                    case "PUT":
                        app.MapPut(endpoint.Pattern, endpoint.Handler);
                        break;
                    case "DELETE":
                        app.MapDelete(endpoint.Pattern, endpoint.Handler);
                        break;
                }
            }
            
            // Apply additional configuration
            _configureCallback?.Invoke(app);
            
            return app;
        }

        /// <summary>
        /// Builds and runs the web application
        /// </summary>
        public void Run()
        {
            var app = Build();
            app.Run();
        }

        /// <summary>
        /// Builds and runs the web application on specific URL
        /// </summary>
        public void Run(string url)
        {
            var app = Build();
            app.Urls.Add(url);
            app.Run();
        }

        #region Private Helper Methods

        private static async Task<T> ReadJsonRequest<T>(HttpContext context)
        {
            context.Request.EnableBuffering();
            using var reader = new StreamReader(context.Request.Body);
            var json = await reader.ReadToEndAsync();
            context.Request.Body.Position = 0;
            
            return JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
        }

        private static async Task WriteJsonResponse(HttpContext context, object data)
        {
            context.Response.ContentType = "application/json";
            var json = JsonSerializer.Serialize(data, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            });
            await context.Response.WriteAsync(json);
        }

        private static async Task WriteErrorResponse(HttpContext context, Exception ex)
        {
            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json";
            
            var error = new
            {
                error = new
                {
                    message = ex.Message,
                    type = ex.GetType().Name,
                    stackTrace = ex.StackTrace
                }
            };
            
            var json = JsonSerializer.Serialize(error, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            });
            
            await context.Response.WriteAsync(json);
        }

        #endregion
    }

    /// <summary>
    /// Represents a route endpoint configuration
    /// </summary>
    public class RouteEndpoint
    {
        public string Method { get; set; }
        public string Pattern { get; set; }
        public Func<HttpContext, Task> Handler { get; set; }
    }

    /// <summary>
    /// Utility class for creating REST APIs from controllers
    /// </summary>
    public static class RestApiGenerator
    {
        /// <summary>
        /// Creates REST API from controller class
        /// </summary>
        public static RestApiBuilder CreateFromController<T>() where T : class
        {
            var builder = new RestApiBuilder();
            var controllerType = typeof(T);
            var instance = Activator.CreateInstance<T>();
            
            // Add controller instance to services
            builder.AddService(instance);
            
            // Extract methods and create endpoints
            var methods = controllerType.GetMethods(BindingFlags.Public | BindingFlags.Instance);
            
            foreach (var method in methods)
            {
                var httpMethod = GetHttpMethod(method);
                var route = GetRoute(method);
                
                if (httpMethod != null && route != null)
                {
                    CreateEndpoint(builder, httpMethod, route, method, instance);
                }
            }
            
            return builder;
        }

        /// <summary>
        /// Extracts HTTP method from method attributes or naming convention
        /// </summary>
        private static string GetHttpMethod(MethodInfo method)
        {
            // Check for method name prefixes
            var methodName = method.Name.ToUpper();
            if (methodName.StartsWith("GET")) return "GET";
            if (methodName.StartsWith("POST")) return "POST";
            if (methodName.StartsWith("PUT")) return "PUT";
            if (methodName.StartsWith("DELETE")) return "DELETE";
            
            return null;
        }

        /// <summary>
        /// Extracts route pattern from method
        /// </summary>
        private static string GetRoute(MethodInfo method)
        {
            var methodName = method.Name.ToUpper();
            
            // Remove HTTP method prefix
            if (methodName.StartsWith("GET")) return "/" + method.Name.Substring(3);
            if (methodName.StartsWith("POST")) return "/" + method.Name.Substring(4);
            if (methodName.StartsWith("PUT")) return "/" + method.Name.Substring(3);
            if (methodName.StartsWith("DELETE")) return "/" + method.Name.Substring(6);
            
            return "/" + method.Name;
        }

        /// <summary>
        /// Creates endpoint from method info
        /// </summary>
        private static void CreateEndpoint(RestApiBuilder builder, string httpMethod, string route, MethodInfo method, object instance)
        {
            var parameters = method.GetParameters();
            
            if (parameters.Length == 0)
            {
                // No parameters
                switch (httpMethod)
                {
                    case "GET":
                        builder.MapGet(route, () => method.Invoke(instance, null));
                        break;
                    case "POST":
                        builder.MapPost(route, async context => await context.Response.WriteAsync("Method not supported"));
                        break;
                    case "PUT":
                        builder.MapPut<object, object>(route, request => 
                        {
                            // This is a placeholder - actual implementation would need HttpContext
                            return new object();
                        });
                        break;
                    case "DELETE":
                        builder.MapDelete(route, () => method.Invoke(instance, null));
                        break;
                }
            }
            else if (parameters.Length == 1)
            {
                // Single parameter (request body)
                var paramType = parameters[0].ParameterType;
                
                switch (httpMethod)
                {
                    case "POST":
                        var postDelegate = CreateDelegate(paramType, method, instance);
                        var postGenericMethod = typeof(RestApiBuilder).GetMethod("MapPost", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance)
                            .MakeGenericMethod(paramType, method.ReturnType);
                        postGenericMethod.Invoke(builder, new object[] { route, postDelegate });
                        break;
                    case "PUT":
                        var putDelegate = CreateDelegate(paramType, method, instance);
                        var putGenericMethod = typeof(RestApiBuilder).GetMethod("MapPut", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance)
                            .MakeGenericMethod(paramType, method.ReturnType);
                        putGenericMethod.Invoke(builder, new object[] { route, putDelegate });
                        break;
                }
            }
        }

        /// <summary>
        /// Creates delegate for method invocation
        /// </summary>
        private static Delegate CreateDelegate(Type paramType, MethodInfo method, object instance)
        {
            var genericMethod = typeof(RestApiGenerator).GetMethod(nameof(CreateGenericDelegate), BindingFlags.Static | BindingFlags.NonPublic)
                .MakeGenericMethod(paramType, method.ReturnType);
            
            return (Delegate)genericMethod.Invoke(null, new object[] { method, instance });
        }

        private static Func<TRequest, TResponse> CreateGenericDelegate<TRequest, TResponse>(MethodInfo method, object instance)
        {
            return request => (TResponse)method.Invoke(instance, new object[] { request });
        }
    }

    /// <summary>
    /// Utility class for OpenAPI/Swagger documentation generation
    /// </summary>
    public static class OpenApiGenerator
    {
        /// <summary>
        /// Generates basic OpenAPI specification for REST API
        /// </summary>
        public static object GenerateOpenApiSpec(string title, string version, List<RouteEndpoint> endpoints)
        {
            return new
            {
                openapi = "3.0.0",
                info = new
                {
                    title = title,
                    version = version
                },
                paths = GeneratePaths(endpoints)
            };
        }

        /// <summary>
        /// Generates paths object for OpenAPI spec
        /// </summary>
        private static object GeneratePaths(List<RouteEndpoint> endpoints)
        {
            var paths = new Dictionary<string, object>();
            
            foreach (var endpoint in endpoints)
            {
                if (!paths.ContainsKey(endpoint.Pattern))
                {
                    paths[endpoint.Pattern] = new Dictionary<string, object>();
                }
                
                var pathItem = (Dictionary<string, object>)paths[endpoint.Pattern];
                pathItem[endpoint.Method.ToLower()] = new
                {
                    summary = $"{endpoint.Method} {endpoint.Pattern}",
                    responses = new Dictionary<string, object>
                    {
                        ["200"] = new Dictionary<string, object>
                        {
                            ["description"] = "Successful response",
                            ["content"] = new Dictionary<string, object>
                            {
                                ["application/json"] = new Dictionary<string, object>
                                {
                                    ["schema"] = new { type = "object" }
                                }
                            }
                        }
                    }
                };
            }
            
            return paths;
        }
    }
}
