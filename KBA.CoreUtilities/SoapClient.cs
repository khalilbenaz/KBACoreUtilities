using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace KBA.CoreUtilities.Utilities
{
    /// <summary>
    /// Utility class for WSDL/SOAP web service consumption
    /// </summary>
    public class SoapClient : IDisposable
    {
        public readonly HttpClient _httpClient;
        private readonly string _endpointUrl;
        private readonly string _soapAction;
        private readonly Dictionary<string, string> _namespaces;

        public SoapClient(string wsdlUrl, string soapAction = null, Dictionary<string, string> namespaces = null)
        {
            _endpointUrl = wsdlUrl.Replace("?wsdl", "");
            _soapAction = soapAction;
            _namespaces = namespaces ?? new Dictionary<string, string>
            {
                { "soap", "http://schemas.xmlsoap.org/soap/envelope/" },
                { "xsd", "http://www.w3.org/2001/XMLSchema" },
                { "xsi", "http://www.w3.org/2001/XMLSchema-instance" }
            };
            
            _httpClient = new HttpClient();
        }

        /// <summary>
        /// Calls a SOAP web service method
        /// </summary>
        public async Task<string> CallMethodAsync(string methodName, Dictionary<string, object> parameters = null)
        {
            var soapEnvelope = BuildSoapEnvelope(methodName, parameters);
            
            using var content = new StringContent(soapEnvelope, Encoding.UTF8, "text/xml");
            if (!string.IsNullOrEmpty(_soapAction))
            {
                content.Headers.Add("SOAPAction", _soapAction + methodName);
            }
            
            var response = await _httpClient.PostAsync(_endpointUrl, content);
            response.EnsureSuccessStatusCode();
            
            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// Calls a SOAP web service method and returns deserialized response
        /// </summary>
        public async Task<T> CallMethodAsync<T>(string methodName, Dictionary<string, object> parameters = null)
        {
            var response = await CallMethodAsync(methodName, parameters);
            return ParseSoapResponse<T>(response);
        }

        /// <summary>
        /// Calls a SOAP web service method with custom SOAP action
        /// </summary>
        public async Task<string> CallMethodAsync(string methodName, string soapAction, Dictionary<string, object> parameters = null)
        {
            var soapEnvelope = BuildSoapEnvelope(methodName, parameters);
            
            using var content = new StringContent(soapEnvelope, Encoding.UTF8, "text/xml");
            content.Headers.Add("SOAPAction", soapAction);
            
            var response = await _httpClient.PostAsync(_endpointUrl, content);
            response.EnsureSuccessStatusCode();
            
            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// Builds SOAP envelope for method call
        /// </summary>
        private string BuildSoapEnvelope(string methodName, Dictionary<string, object> parameters)
        {
            var envelope = new XDocument(
                new XElement(_namespaces["soap"] + "Envelope",
                    new XAttribute(XNamespace.Xmlns + "soap", _namespaces["soap"]),
                    new XElement(_namespaces["soap"] + "Body",
                        new XElement(methodName,
                            parameters?.Select(p => new XElement(p.Key, p.Value))
                        )
                    )
                )
            );
            
            return envelope.ToString();
        }

        /// <summary>
        /// Parses SOAP response and extracts specified type
        /// </summary>
        private T ParseSoapResponse<T>(string soapResponse)
        {
            var doc = XDocument.Parse(soapResponse);
            var body = doc.Root.Element(_namespaces["soap"] + "Body");
            var responseElement = body.Elements().First();
            
            return SerializationUtils.FromXml<T>(responseElement.ToString());
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }

    /// <summary>
    /// Utility class for WSDL operations
    /// </summary>
    public static class WsdlUtils
    {
        /// <summary>
        /// Downloads WSDL document from URL
        /// </summary>
        public static async Task<string> DownloadWsdlAsync(string wsdlUrl)
        {
            using var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(wsdlUrl);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// Parses WSDL and extracts available methods
        /// </summary>
        public static List<string> ExtractMethods(string wsdlContent)
        {
            var methods = new List<string>();
            var doc = XDocument.Parse(wsdlContent);
            
            // Look for operations in different possible namespaces
            var operationElements = doc.Descendants()
                .Where(e => e.Name.LocalName == "operation")
                .ToList();
            
            foreach (var operation in operationElements)
            {
                var operationName = operation.Attribute("name")?.Value;
                if (!string.IsNullOrEmpty(operationName))
                {
                    methods.Add(operationName);
                }
            }
            
            return methods.Distinct().ToList();
        }

        /// <summary>
        /// Parses WSDL and extracts service endpoints
        /// </summary>
        public static List<ServiceEndpoint> ExtractEndpoints(string wsdlContent)
        {
            var endpoints = new List<ServiceEndpoint>();
            var doc = XDocument.Parse(wsdlContent);
            
            // Look for service ports
            var portElements = doc.Descendants()
                .Where(e => e.Name.LocalName == "port")
                .ToList();
            
            foreach (var port in portElements)
            {
                var portName = port.Attribute("name")?.Value;
                var addressElement = port.Descendants().FirstOrDefault(e => e.Name.LocalName == "address");
                var location = addressElement?.Attribute("location")?.Value;
                
                if (!string.IsNullOrEmpty(portName) && !string.IsNullOrEmpty(location))
                {
                    endpoints.Add(new ServiceEndpoint
                    {
                        Name = portName,
                        Url = location
                    });
                }
            }
            
            return endpoints;
        }

        /// <summary>
        /// Creates SOAP client from WSDL URL
        /// </summary>
        public static async Task<SoapClient> CreateClientFromWsdlAsync(string wsdlUrl, string serviceName = null)
        {
            var wsdlContent = await DownloadWsdlAsync(wsdlUrl);
            var endpoints = ExtractEndpoints(wsdlContent);
            
            var endpointUrl = endpoints.FirstOrDefault()?.Url ?? wsdlUrl.Replace("?wsdl", "");
            var soapAction = serviceName != null ? $"{serviceName}/" : null;
            
            return new SoapClient(endpointUrl, soapAction);
        }

        /// <summary>
        /// Validates WSDL document
        /// </summary>
        public static bool IsValidWsdl(string wsdlContent)
        {
            try
            {
                var doc = XDocument.Parse(wsdlContent);
                var definitions = doc.Root;
                
                // Check if it's a valid WSDL by looking for key elements
                return definitions.Name.LocalName == "definitions" &&
                       (definitions.Attribute("name") != null || 
                        definitions.Descendants().Any(e => e.Name.LocalName == "service"));
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Extracts SOAP action for a specific method
        /// </summary>
        public static string ExtractSoapAction(string wsdlContent, string methodName)
        {
            try
            {
                var doc = XDocument.Parse(wsdlContent);
                var operation = doc.Descendants()
                    .FirstOrDefault(e => e.Name.LocalName == "operation" && 
                                        e.Attribute("name")?.Value == methodName);
                
                if (operation != null)
                {
                    var soapOperation = operation.Descendants()
                        .FirstOrDefault(e => e.Name.LocalName == "operation" && 
                                            e.Name.Namespace?.ToString().Contains("soap") == true);
                    
                    return soapOperation?.Attribute("soapAction")?.Value;
                }
            }
            catch
            {
                // Return null if parsing fails
            }
            
            return null;
        }
    }

    /// <summary>
    /// Represents a WSDL service endpoint
    /// </summary>
    public class ServiceEndpoint
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string Binding { get; set; }
        public string Port { get; set; }
    }

    /// <summary>
    /// Utility class for advanced SOAP operations
    /// </summary>
    public static class AdvancedSoapUtils
    {
        /// <summary>
        /// Creates SOAP client with custom headers
        /// </summary>
        public static SoapClient CreateClientWithHeaders(string wsdlUrl, Dictionary<string, string> headers)
        {
            var client = new SoapClient(wsdlUrl);
            
            // Add custom headers to the HTTP client
            foreach (var header in headers)
            {
                client._httpClient.DefaultRequestHeaders.TryAddWithoutValidation(header.Key, header.Value);
            }
            
            return client;
        }

        /// <summary>
        /// Creates SOAP client with authentication
        /// </summary>
        public static SoapClient CreateAuthenticatedClient(string wsdlUrl, string username, string password)
        {
            var client = new SoapClient(wsdlUrl);
            
            var credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{password}"));
            client._httpClient.DefaultRequestHeaders.Authorization = 
                new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", credentials);
            
            return client;
        }

        /// <summary>
        /// Calls SOAP method with WS-Security headers
        /// </summary>
        public static async Task<string> CallWithWSSecurityAsync(string wsdlUrl, string methodName, 
            Dictionary<string, object> parameters, string username, string password)
        {
            var soapEnvelope = BuildWSSecurityEnvelope(methodName, parameters, username, password);
            
            using var httpClient = new HttpClient();
            using var content = new StringContent(soapEnvelope, Encoding.UTF8, "text/xml");
            
            var endpointUrl = wsdlUrl.Replace("?wsdl", "");
            var response = await httpClient.PostAsync(endpointUrl, content);
            response.EnsureSuccessStatusCode();
            
            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// Builds SOAP envelope with WS-Security headers
        /// </summary>
        private static string BuildWSSecurityEnvelope(string methodName, Dictionary<string, object> parameters, 
            string username, string password)
        {
            var wsseNs = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd";
            var soapNs = "http://schemas.xmlsoap.org/soap/envelope/";
            
            var envelope = new XDocument(
                new XElement(soapNs + "Envelope",
                    new XAttribute(XNamespace.Xmlns + "soap", soapNs),
                    new XAttribute(XNamespace.Xmlns + "wsse", wsseNs),
                    new XElement(soapNs + "Header",
                        new XElement(wsseNs + "Security",
                            new XElement(wsseNs + "UsernameToken",
                                new XElement(wsseNs + "Username", username),
                                new XElement(wsseNs + "Password", password)
                            )
                        )
                    ),
                    new XElement(soapNs + "Body",
                        new XElement(methodName,
                            parameters?.Select(p => new XElement(p.Key, p.Value))
                        )
                    )
                )
            );
            
            return envelope.ToString();
        }

        /// <summary>
        /// Parses SOAP fault from response
        /// </summary>
        public static SoapFault ParseSoapFault(string soapResponse)
        {
            try
            {
                var doc = XDocument.Parse(soapResponse);
                var faultElement = doc.Descendants()
                    .FirstOrDefault(e => e.Name.LocalName == "Fault");
                
                if (faultElement != null)
                {
                    return new SoapFault
                    {
                        FaultCode = faultElement.Element("faultcode")?.Value,
                        FaultString = faultElement.Element("faultstring")?.Value,
                        FaultActor = faultElement.Element("faultactor")?.Value,
                        Detail = faultElement.Element("detail")?.Value
                    };
                }
            }
            catch
            {
                // Return null if parsing fails
            }
            
            return null;
        }
    }

    /// <summary>
    /// Represents a SOAP fault
    /// </summary>
    public class SoapFault
    {
        public string FaultCode { get; set; }
        public string FaultString { get; set; }
        public string FaultActor { get; set; }
        public string Detail { get; set; }
    }
}
