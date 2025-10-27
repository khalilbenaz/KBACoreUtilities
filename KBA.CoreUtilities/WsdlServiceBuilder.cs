using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace KBA.CoreUtilities.Utilities
{
    /// <summary>
    /// Simple WSDL service builder for minimal configuration
    /// </summary>
    public class WsdlServiceBuilder
    {
        private readonly List<ServiceContract> _contracts = new();
        private readonly string _serviceName;
        private readonly string _serviceNamespace;
        private readonly string _serviceUrl;

        public WsdlServiceBuilder(string serviceName = "MyService", string serviceNamespace = "http://tempuri.org/", string serviceUrl = "http://localhost:5000/Service.svc")
        {
            _serviceName = serviceName;
            _serviceNamespace = serviceNamespace;
            _serviceUrl = serviceUrl;
        }

        /// <summary>
        /// Adds a service contract interface
        /// </summary>
        public WsdlServiceBuilder AddService<T>() where T : class
        {
            var contractType = typeof(T);
            var contract = new ServiceContract
            {
                Name = contractType.Name,
                Namespace = _serviceNamespace,
                Operations = ExtractOperations(contractType)
            };
            
            _contracts.Add(contract);
            return this;
        }

        /// <summary>
        /// Adds a service contract with custom implementation
        /// </summary>
        public WsdlServiceBuilder AddService<TInterface, TImplementation>() 
            where TInterface : class 
            where TImplementation : class, TInterface
        {
            var contractType = typeof(TInterface);
            var implementationType = typeof(TImplementation);
            var contract = new ServiceContract
            {
                Name = contractType.Name,
                Namespace = _serviceNamespace,
                Implementation = implementationType,
                Operations = ExtractOperations(contractType)
            };
            
            _contracts.Add(contract);
            return this;
        }

        /// <summary>
        /// Builds the WSDL service
        /// </summary>
        public WsdlService Build()
        {
            var service = new WsdlService
            {
                Name = _serviceName,
                Namespace = _serviceNamespace,
                Url = _serviceUrl,
                Contracts = _contracts
            };
            
            return service;
        }

        /// <summary>
        /// Generates WSDL document
        /// </summary>
        public string GenerateWsdl()
        {
            var wsdl = new XDocument(
                new XElement(XName.Get("definitions", "http://schemas.xmlsoap.org/wsdl/"),
                    new XAttribute("name", _serviceName),
                    new XAttribute("targetNamespace", _serviceNamespace),
                    new XAttribute(XNamespace.Xmlns + "xsd", "http://www.w3.org/2001/XMLSchema"),
                    new XAttribute(XNamespace.Xmlns + "soap", "http://schemas.xmlsoap.org/wsdl/soap/"),
                    new XAttribute(XNamespace.Xmlns + "tns", _serviceNamespace),
                    
                    // Types section
                    new XElement(XName.Get("types", "http://schemas.xmlsoap.org/wsdl/"),
                        _contracts.SelectMany(c => c.Operations).SelectMany(op => GenerateSchemaTypes(op))
                    ),
                    
                    // Messages section
                    new XElement(XName.Get("message", "http://schemas.xmlsoap.org/wsdl/"),
                        _contracts.SelectMany(c => c.Operations).SelectMany(op => GenerateMessages(op))
                    ),
                    
                    // PortType section
                    new XElement(XName.Get("portType", "http://schemas.xmlsoap.org/wsdl/"),
                        _contracts.Select(c => GeneratePortType(c))
                    ),
                    
                    // Binding section
                    new XElement(XName.Get("binding", "http://schemas.xmlsoap.org/wsdl/"),
                        _contracts.Select(c => GenerateBinding(c))
                    ),
                    
                    // Service section
                    new XElement(XName.Get("service", "http://schemas.xmlsoap.org/wsdl/"),
                        new XAttribute("name", _serviceName),
                        _contracts.Select(c => GenerateServicePort(c))
                    )
                )
            );
            
            return wsdl.ToString();
        }

        #region Private Methods

        private static List<ServiceOperation> ExtractOperations(Type contractType)
        {
            var operations = new List<ServiceOperation>();
            var methods = contractType.GetMethods(BindingFlags.Public | BindingFlags.Instance);
            
            foreach (var method in methods)
            {
                var operation = new ServiceOperation
                {
                    Name = method.Name,
                    InputParameters = method.GetParameters().Select(p => new ServiceParameter
                    {
                        Name = p.Name,
                        Type = p.ParameterType,
                        IsRequired = !p.IsOptional
                    }).ToList(),
                    ReturnType = method.ReturnType,
                    IsAsync = typeof(Task).IsAssignableFrom(method.ReturnType)
                };
                
                operations.Add(operation);
            }
            
            return operations;
        }

        private static IEnumerable<XElement> GenerateSchemaTypes(ServiceOperation operation)
        {
            var xsdNamespace = "http://www.w3.org/2001/XMLSchema";
            
            // Input type
            yield return new XElement(XName.Get("complexType", xsdNamespace),
                new XAttribute("name", $"{operation.Name}Input"),
                new XElement(XName.Get("sequence", xsdNamespace),
                    operation.InputParameters.Select(p => new XElement(XName.Get("element", xsdNamespace),
                        new XAttribute("name", p.Name),
                        new XAttribute("type", GetXsdType(p.Type))
                    ))
                )
            );
            
            // Output type
            yield return new XElement(XName.Get("complexType", xsdNamespace),
                new XAttribute("name", $"{operation.Name}Output"),
                new XElement(XName.Get("sequence", xsdNamespace),
                    new XElement(XName.Get("element", xsdNamespace),
                        new XAttribute("name", $"{operation.Name}Result"),
                        new XAttribute("type", GetXsdType(operation.ReturnType))
                    )
                )
            );
        }

        private static IEnumerable<XElement> GenerateMessages(ServiceOperation operation)
        {
            var wsdlNamespace = "http://schemas.xmlsoap.org/wsdl/";
            
            yield return new XElement(XName.Get("message", wsdlNamespace),
                new XAttribute("name", $"{operation.Name}InputMessage"),
                new XElement(XName.Get("part", wsdlNamespace),
                    new XAttribute("name", "parameters"),
                    new XAttribute("element", $"tns:{operation.Name}Input")
                )
            );
            
            yield return new XElement(XName.Get("message", wsdlNamespace),
                new XAttribute("name", $"{operation.Name}OutputMessage"),
                new XElement(XName.Get("part", wsdlNamespace),
                    new XAttribute("name", "parameters"),
                    new XAttribute("element", $"tns:{operation.Name}Output")
                )
            );
        }

        private static XElement GeneratePortType(ServiceContract contract)
        {
            var wsdlNamespace = "http://schemas.xmlsoap.org/wsdl/";
            
            return new XElement(XName.Get("portType", wsdlNamespace),
                new XAttribute("name", $"{contract.Name}Interface"),
                contract.Operations.Select(op => new XElement(XName.Get("operation", wsdlNamespace),
                    new XAttribute("name", op.Name),
                    new XElement(XName.Get("input", wsdlNamespace),
                        new XAttribute("message", $"tns:{op.Name}InputMessage")
                    ),
                    new XElement(XName.Get("output", wsdlNamespace),
                        new XAttribute("message", $"tns:{op.Name}OutputMessage")
                    )
                ))
            );
        }

        private static XElement GenerateBinding(ServiceContract contract)
        {
            var wsdlNamespace = "http://schemas.xmlsoap.org/wsdl/";
            var soapNamespace = "http://schemas.xmlsoap.org/wsdl/soap/";
            
            return new XElement(XName.Get("binding", wsdlNamespace),
                new XAttribute("name", $"{contract.Name}Binding"),
                new XAttribute("type", $"tns:{contract.Name}Interface"),
                new XElement(XName.Get("binding", soapNamespace),
                    new XAttribute("transport", "http://schemas.xmlsoap.org/soap/http"),
                    new XAttribute("style", "document")
                ),
                contract.Operations.Select(op => new XElement(XName.Get("operation", wsdlNamespace),
                    new XAttribute("name", op.Name),
                    new XElement(XName.Get("operation", soapNamespace),
                        new XAttribute("soapAction", $"{contract.Namespace}{op.Name}")
                    ),
                    new XElement(XName.Get("input", wsdlNamespace),
                        new XElement(XName.Get("body", soapNamespace),
                            new XAttribute("use", "literal")
                        )
                    ),
                    new XElement(XName.Get("output", wsdlNamespace),
                        new XElement(XName.Get("body", soapNamespace),
                            new XAttribute("use", "literal")
                        )
                    )
                ))
            );
        }

        private static XElement GenerateServicePort(ServiceContract contract)
        {
            var wsdlNamespace = "http://schemas.xmlsoap.org/wsdl/";
            var soapNamespace = "http://schemas.xmlsoap.org/wsdl/soap/";
            
            return new XElement(XName.Get("port", wsdlNamespace),
                new XAttribute("name", $"{contract.Name}Port"),
                new XAttribute("binding", $"tns:{contract.Name}Binding"),
                new XElement(XName.Get("address", soapNamespace),
                    new XAttribute("location", $"{contract.Url}/{contract.Name}")
                )
            );
        }

        private static string GetXsdType(Type type)
        {
            if (type == typeof(string)) return "xsd:string";
            if (type == typeof(int) || type == typeof(int?)) return "xsd:int";
            if (type == typeof(long) || type == typeof(long?)) return "xsd:long";
            if (type == typeof(decimal) || type == typeof(decimal?)) return "xsd:decimal";
            if (type == typeof(double) || type == typeof(double?)) return "xsd:double";
            if (type == typeof(float) || type == typeof(float?)) return "xsd:float";
            if (type == typeof(bool) || type == typeof(bool?)) return "xsd:boolean";
            if (type == typeof(DateTime) || type == typeof(DateTime?)) return "xsd:dateTime";
            if (type == typeof(Guid) || type == typeof(Guid?)) return "xsd:string";
            
            return "xsd:string"; // Default for complex types
        }

        #endregion
    }

    /// <summary>
    /// Represents a WSDL service
    /// </summary>
    public class WsdlService
    {
        public string Name { get; set; }
        public string Namespace { get; set; }
        public string Url { get; set; }
        public List<ServiceContract> Contracts { get; set; } = new();

        /// <summary>
        /// Generates WSDL document
        /// </summary>
        public string GenerateWsdl()
        {
            var builder = new WsdlServiceBuilder(Name, Namespace, Url);
            foreach (var contract in Contracts)
            {
                // Add contract logic here
            }
            return builder.GenerateWsdl();
        }

        /// <summary>
        /// Saves WSDL to file
        /// </summary>
        public void SaveWsdl(string filePath)
        {
            var wsdl = GenerateWsdl();
            File.WriteAllText(filePath, wsdl);
        }
    }

    /// <summary>
    /// Represents a service contract
    /// </summary>
    public class ServiceContract
    {
        public string Name { get; set; }
        public string Namespace { get; set; }
        public Type Implementation { get; set; }
        public List<ServiceOperation> Operations { get; set; } = new();
        public string Url { get; set; }
    }

    /// <summary>
    /// Represents a service operation
    /// </summary>
    public class ServiceOperation
    {
        public string Name { get; set; }
        public List<ServiceParameter> InputParameters { get; set; } = new();
        public Type ReturnType { get; set; }
        public bool IsAsync { get; set; }
    }

    /// <summary>
    /// Represents a service parameter
    /// </summary>
    public class ServiceParameter
    {
        public string Name { get; set; }
        public Type Type { get; set; }
        public bool IsRequired { get; set; }
    }

    /// <summary>
    /// Utility class for WSDL generation and validation
    /// </summary>
    public static class WsdlGenerator
    {
        /// <summary>
        /// Generates WSDL from interface type
        /// </summary>
        public static string GenerateFromInterface<T>(string serviceName = null, string serviceNamespace = "http://tempuri.org/", string serviceUrl = null) where T : class
        {
            serviceName ??= typeof(T).Name.Replace("I", "") + "Service";
            serviceUrl ??= "http://localhost:5000/" + serviceName + ".svc";
            
            var builder = new WsdlServiceBuilder(serviceName, serviceNamespace, serviceUrl);
            builder.AddService<T>();
            
            return builder.GenerateWsdl();
        }

        /// <summary>
        /// Generates WSDL from interface and implementation types
        /// </summary>
        public static string GenerateFromTypes<TInterface, TImplementation>(string serviceName = null, string serviceNamespace = "http://tempuri.org/", string serviceUrl = null) 
            where TInterface : class 
            where TImplementation : class, TInterface
        {
            serviceName ??= typeof(TInterface).Name.Replace("I", "") + "Service";
            serviceUrl ??= "http://localhost:5000/" + serviceName + ".svc";
            
            var builder = new WsdlServiceBuilder(serviceName, serviceNamespace, serviceUrl);
            builder.AddService<TInterface, TImplementation>();
            
            return builder.GenerateWsdl();
        }

        /// <summary>
        /// Validates WSDL document against XSD schema
        /// </summary>
        public static bool ValidateWsdl(string wsdlContent, out List<string> errors)
        {
            errors = new List<string>();
            
            try
            {
                var settings = new XmlReaderSettings
                {
                    ValidationType = ValidationType.Schema,
                    ValidationFlags = XmlSchemaValidationFlags.ReportValidationWarnings
                };
                
                var localErrors = errors;
                settings.ValidationEventHandler += (sender, e) =>
                {
                    localErrors.Add(e.Message);
                };
                
                using var stringReader = new StringReader(wsdlContent);
                using var xmlReader = XmlReader.Create(stringReader, settings);
                
                while (xmlReader.Read()) { }
                
                return errors.Count == 0;
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Extracts service information from WSDL
        /// </summary>
        public static WsdlServiceInfo ExtractServiceInfo(string wsdlContent)
        {
            var doc = XDocument.Parse(wsdlContent);
            var definitions = doc.Root;
            
            var serviceInfo = new WsdlServiceInfo
            {
                Name = definitions.Attribute("name")?.Value,
                TargetNamespace = definitions.Attribute("targetNamespace")?.Value,
                Services = new List<ServiceInfo>()
            };
            
            var serviceElements = definitions.Elements(XName.Get("service", definitions.Name.NamespaceName));
            foreach (var serviceElement in serviceElements)
            {
                var service = new ServiceInfo
                {
                    Name = serviceElement.Attribute("name")?.Value,
                    Ports = new List<PortInfo>()
                };
                
                var portElements = serviceElement.Elements(XName.Get("port", definitions.Name.NamespaceName));
                foreach (var portElement in portElements)
                {
                    var addressElement = portElement.Elements().FirstOrDefault(e => e.Name.LocalName == "address");
                    var port = new PortInfo
                    {
                        Name = portElement.Attribute("name")?.Value,
                        Binding = portElement.Attribute("binding")?.Value,
                        Address = addressElement?.Attribute("location")?.Value
                    };
                    
                    service.Ports.Add(port);
                }
                
                serviceInfo.Services.Add(service);
            }
            
            return serviceInfo;
        }
    }

    /// <summary>
    /// Represents WSDL service information
    /// </summary>
    public class WsdlServiceInfo
    {
        public string Name { get; set; }
        public string TargetNamespace { get; set; }
        public List<ServiceInfo> Services { get; set; }
    }

    /// <summary>
    /// Represents service information
    /// </summary>
    public class ServiceInfo
    {
        public string Name { get; set; }
        public List<PortInfo> Ports { get; set; }
    }

    /// <summary>
    /// Represents port information
    /// </summary>
    public class PortInfo
    {
        public string Name { get; set; }
        public string Binding { get; set; }
        public string Address { get; set; }
    }
}
