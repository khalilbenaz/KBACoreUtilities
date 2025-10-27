using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace KBA.CoreUtilities.Utilities
{
    /// <summary>
    /// Utility class for JSON and XML serialization operations
    /// </summary>
    public static class SerializationUtils
    {
        private static readonly JsonSerializerOptions DefaultJsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            Converters = { new JsonStringEnumConverter() }
        };

        private static readonly JsonSerializerOptions CompactJsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            Converters = { new JsonStringEnumConverter() }
        };

        #region JSON Serialization

        /// <summary>
        /// Serializes an object to JSON string with default options
        /// </summary>
        public static string ToJson<T>(T obj)
        {
            if (obj == null) return null;
            return JsonSerializer.Serialize(obj, DefaultJsonOptions);
        }

        /// <summary>
        /// Serializes an object to JSON string with custom options
        /// </summary>
        public static string ToJson<T>(T obj, JsonSerializerOptions options)
        {
            if (obj == null) return null;
            return JsonSerializer.Serialize(obj, options);
        }

        /// <summary>
        /// Serializes an object to compact JSON string (no indentation)
        /// </summary>
        public static string ToCompactJson<T>(T obj)
        {
            if (obj == null) return null;
            return JsonSerializer.Serialize(obj, CompactJsonOptions);
        }

        /// <summary>
        /// Deserializes JSON string to object
        /// </summary>
        public static T FromJson<T>(string json)
        {
            if (string.IsNullOrWhiteSpace(json)) return default;
            return JsonSerializer.Deserialize<T>(json, DefaultJsonOptions);
        }

        /// <summary>
        /// Deserializes JSON string to object with custom options
        /// </summary>
        public static T FromJson<T>(string json, JsonSerializerOptions options)
        {
            if (string.IsNullOrWhiteSpace(json)) return default;
            return JsonSerializer.Deserialize<T>(json, options);
        }

        /// <summary>
        /// Serializes an object to JSON stream asynchronously
        /// </summary>
        public static async Task ToJsonStreamAsync<T>(T obj, Stream stream)
        {
            if (obj == null) return;
            await JsonSerializer.SerializeAsync(stream, obj, DefaultJsonOptions);
        }

        /// <summary>
        /// Serializes an object to JSON stream asynchronously with custom options
        /// </summary>
        public static async Task ToJsonStreamAsync<T>(T obj, Stream stream, JsonSerializerOptions options)
        {
            if (obj == null) return;
            await JsonSerializer.SerializeAsync(stream, obj, options);
        }

        /// <summary>
        /// Deserializes object from JSON stream asynchronously
        /// </summary>
        public static async Task<T> FromJsonStreamAsync<T>(Stream stream)
        {
            return await JsonSerializer.DeserializeAsync<T>(stream, DefaultJsonOptions);
        }

        /// <summary>
        /// Deserializes object from JSON stream asynchronously with custom options
        /// </summary>
        public static async Task<T> FromJsonStreamAsync<T>(Stream stream, JsonSerializerOptions options)
        {
            return await JsonSerializer.DeserializeAsync<T>(stream, options);
        }

        /// <summary>
        /// Converts object to another type using JSON serialization
        /// </summary>
        public static TTarget Convert<TSource, TTarget>(TSource source)
        {
            if (source == null) return default;
            var json = ToJson(source);
            return FromJson<TTarget>(json);
        }

        /// <summary>
        /// Deep clones an object using JSON serialization
        /// </summary>
        public static T DeepClone<T>(T obj)
        {
            if (obj == null) return default;
            var json = ToJson(obj);
            return FromJson<T>(json);
        }

        #endregion

        #region XML Serialization

        /// <summary>
        /// Serializes an object to XML string
        /// </summary>
        public static string ToXml<T>(T obj)
        {
            if (obj == null) return null;
            
            var serializer = new XmlSerializer(typeof(T));
            using var stringWriter = new StringWriter();
            serializer.Serialize(stringWriter, obj);
            return stringWriter.ToString();
        }

        /// <summary>
        /// Serializes an object to XML string with custom namespaces
        /// </summary>
        public static string ToXml<T>(T obj, XmlSerializerNamespaces namespaces)
        {
            if (obj == null) return null;
            
            var serializer = new XmlSerializer(typeof(T));
            using var stringWriter = new StringWriter();
            serializer.Serialize(stringWriter, obj, namespaces);
            return stringWriter.ToString();
        }

        /// <summary>
        /// Deserializes XML string to object
        /// </summary>
        public static T FromXml<T>(string xml)
        {
            if (string.IsNullOrWhiteSpace(xml)) return default;
            
            var serializer = new XmlSerializer(typeof(T));
            using var stringReader = new StringReader(xml);
            return (T)serializer.Deserialize(stringReader);
        }

        /// <summary>
        /// Serializes an object to XML file
        /// </summary>
        public static void ToXmlFile<T>(T obj, string filePath)
        {
            if (obj == null) return;
            
            var serializer = new XmlSerializer(typeof(T));
            using var fileStream = new FileStream(filePath, FileMode.Create);
            serializer.Serialize(fileStream, obj);
        }

        /// <summary>
        /// Deserializes object from XML file
        /// </summary>
        public static T FromXmlFile<T>(string filePath)
        {
            if (!File.Exists(filePath)) return default;
            
            var serializer = new XmlSerializer(typeof(T));
            using var fileStream = new FileStream(filePath, FileMode.Open);
            return (T)serializer.Deserialize(fileStream);
        }

        /// <summary>
        /// Serializes an object to XML stream
        /// </summary>
        public static void ToXmlStream<T>(T obj, Stream stream)
        {
            if (obj == null) return;
            
            var serializer = new XmlSerializer(typeof(T));
            serializer.Serialize(stream, obj);
        }

        /// <summary>
        /// Deserializes object from XML stream
        /// </summary>
        public static T FromXmlStream<T>(Stream stream)
        {
            var serializer = new XmlSerializer(typeof(T));
            return (T)serializer.Deserialize(stream);
        }

        #endregion

        #region Utility Methods

        /// <summary>
        /// Creates custom JSON serializer options
        /// </summary>
        public static JsonSerializerOptions CreateJsonOptions(
            bool writeIndented = true,
            bool camelCase = true,
            bool ignoreNullValues = true,
            bool includeEnumStringConverter = true)
        {
            var options = new JsonSerializerOptions();
            
            if (camelCase)
                options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            
            options.WriteIndented = writeIndented;
            
            if (ignoreNullValues)
                options.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            
            if (includeEnumStringConverter)
                options.Converters.Add(new JsonStringEnumConverter());
            
            return options;
        }

        /// <summary>
        /// Validates JSON string format
        /// </summary>
        public static bool IsValidJson(string json)
        {
            if (string.IsNullOrWhiteSpace(json)) return false;
            
            try
            {
                using var document = JsonDocument.Parse(json);
                return true;
            }
            catch (JsonException)
            {
                return false;
            }
        }

        /// <summary>
        /// Validates XML string format
        /// </summary>
        public static bool IsValidXml(string xml)
        {
            if (string.IsNullOrWhiteSpace(xml)) return false;
            
            try
            {
                var xmlDoc = new System.Xml.XmlDocument();
                xmlDoc.LoadXml(xml);
                return true;
            }
            catch (System.Xml.XmlException)
            {
                return false;
            }
        }

        /// <summary>
        /// Minifies JSON string (removes whitespace)
        /// </summary>
        public static string MinifyJson(string json)
        {
            if (string.IsNullOrWhiteSpace(json)) return json;
            
            try
            {
                using var document = JsonDocument.Parse(json);
                return JsonSerializer.Serialize(document, CompactJsonOptions);
            }
            catch (JsonException)
            {
                return json;
            }
        }

        /// <summary>
        /// Formats JSON string with proper indentation
        /// </summary>
        public static string FormatJson(string json)
        {
            if (string.IsNullOrWhiteSpace(json)) return json;
            
            try
            {
                using var document = JsonDocument.Parse(json);
                return JsonSerializer.Serialize(document, DefaultJsonOptions);
            }
            catch (JsonException)
            {
                return json;
            }
        }

        /// <summary>
        /// Gets JSON property value by path (e.g., "user.address.city")
        /// </summary>
        public static string GetJsonPropertyValue(string json, string propertyPath)
        {
            if (string.IsNullOrWhiteSpace(json) || string.IsNullOrWhiteSpace(propertyPath)) return null;
            
            try
            {
                using var document = JsonDocument.Parse(json);
                var pathParts = propertyPath.Split('.');
                var element = document.RootElement;
                
                foreach (var part in pathParts)
                {
                    if (element.TryGetProperty(part, out var nextElement))
                        element = nextElement;
                    else
                        return null;
                }
                
                return element.ValueKind switch
                {
                    JsonValueKind.String => element.GetString(),
                    JsonValueKind.Number => element.GetDecimal().ToString(),
                    JsonValueKind.True => "true",
                    JsonValueKind.False => "false",
                    _ => element.GetRawText()
                };
            }
            catch (JsonException)
            {
                return null;
            }
        }

        #endregion

        #region File Operations

        /// <summary>
        /// Serializes an object to JSON file
        /// </summary>
        public static void ToJsonFile<T>(T obj, string filePath)
        {
            if (obj == null) return;
            var json = ToJson(obj);
            File.WriteAllText(filePath, json);
        }

        /// <summary>
        /// Serializes an object to JSON file asynchronously
        /// </summary>
        public static async Task ToJsonFileAsync<T>(T obj, string filePath)
        {
            if (obj == null) return;
            var json = ToJson(obj);
            await File.WriteAllTextAsync(filePath, json);
        }

        /// <summary>
        /// Deserializes object from JSON file
        /// </summary>
        public static T FromJsonFile<T>(string filePath)
        {
            if (!File.Exists(filePath)) return default;
            var json = File.ReadAllText(filePath);
            return FromJson<T>(json);
        }

        /// <summary>
        /// Deserializes object from JSON file asynchronously
        /// </summary>
        public static async Task<T> FromJsonFileAsync<T>(string filePath)
        {
            if (!File.Exists(filePath)) return default;
            var json = await File.ReadAllTextAsync(filePath);
            return FromJson<T>(json);
        }

        #endregion

        #region Advanced JSON Operations

        /// <summary>
        /// Serializes object to JSON bytes (UTF8)
        /// </summary>
        public static byte[] ToJsonBytes<T>(T obj)
        {
            if (obj == null) return null;
            return System.Text.Encoding.UTF8.GetBytes(ToJson(obj));
        }

        /// <summary>
        /// Deserializes object from JSON bytes
        /// </summary>
        public static T FromJsonBytes<T>(byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0) return default;
            var json = System.Text.Encoding.UTF8.GetString(bytes);
            return FromJson<T>(json);
        }

        /// <summary>
        /// Merges two JSON objects (second overwrites first)
        /// </summary>
        public static string MergeJson(string json1, string json2)
        {
            if (string.IsNullOrWhiteSpace(json1)) return json2;
            if (string.IsNullOrWhiteSpace(json2)) return json1;

            try
            {
                using var doc1 = JsonDocument.Parse(json1);
                using var doc2 = JsonDocument.Parse(json2);

                var merged = new Dictionary<string, object>();

                // Add all properties from first JSON
                foreach (var property in doc1.RootElement.EnumerateObject())
                {
                    merged[property.Name] = property.Value.GetRawText();
                }

                // Overwrite/add properties from second JSON
                foreach (var property in doc2.RootElement.EnumerateObject())
                {
                    merged[property.Name] = property.Value.GetRawText();
                }

                return ToJson(merged);
            }
            catch (JsonException)
            {
                return json2; // If merge fails, return second JSON
            }
        }

        /// <summary>
        /// Compares two JSON strings for equality
        /// </summary>
        public static bool AreJsonEqual(string json1, string json2)
        {
            if (json1 == json2) return true;
            if (string.IsNullOrWhiteSpace(json1) || string.IsNullOrWhiteSpace(json2)) return false;

            try
            {
                using var doc1 = JsonDocument.Parse(json1);
                using var doc2 = JsonDocument.Parse(json2);
                return doc1.RootElement.ToString() == doc2.RootElement.ToString();
            }
            catch (JsonException)
            {
                return false;
            }
        }

        /// <summary>
        /// Converts JSON to XML
        /// </summary>
        public static string JsonToXml(string json, string rootElementName = "root")
        {
            if (string.IsNullOrWhiteSpace(json)) return null;

            try
            {
                using var doc = JsonDocument.Parse(json);
                var xmlDoc = new System.Xml.XmlDocument();
                var root = xmlDoc.CreateElement(rootElementName);
                xmlDoc.AppendChild(root);

                void ProcessElement(System.Xml.XmlNode parent, JsonElement element)
                {
                    switch (element.ValueKind)
                    {
                        case JsonValueKind.Object:
                            foreach (var property in element.EnumerateObject())
                            {
                                var childNode = xmlDoc.CreateElement(property.Name);
                                parent.AppendChild(childNode);
                                ProcessElement(childNode, property.Value);
                            }
                            break;
                        case JsonValueKind.Array:
                            foreach (var item in element.EnumerateArray())
                            {
                                var itemNode = xmlDoc.CreateElement("item");
                                parent.AppendChild(itemNode);
                                ProcessElement(itemNode, item);
                            }
                            break;
                        default:
                            parent.InnerText = element.ToString();
                            break;
                    }
                }

                ProcessElement(root, doc.RootElement);

                using var stringWriter = new StringWriter();
                using var xmlWriter = System.Xml.XmlWriter.Create(stringWriter, new System.Xml.XmlWriterSettings { Indent = true });
                xmlDoc.Save(xmlWriter);
                return stringWriter.ToString();
            }
            catch (JsonException)
            {
                return null;
            }
        }

        /// <summary>
        /// Gets size of serialized JSON in bytes
        /// </summary>
        public static long GetJsonSize<T>(T obj)
        {
            if (obj == null) return 0;
            var bytes = ToJsonBytes(obj);
            return bytes?.Length ?? 0;
        }

        /// <summary>
        /// Pretty prints JSON with custom indentation
        /// </summary>
        public static string PrettyPrintJson(string json, int indentSize = 2)
        {
            if (string.IsNullOrWhiteSpace(json)) return json;

            try
            {
                using var doc = JsonDocument.Parse(json);
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                };
                return JsonSerializer.Serialize(doc, options);
            }
            catch (JsonException)
            {
                return json;
            }
        }

        #endregion

        #region Performance and Optimization

        /// <summary>
        /// Creates optimized JSON options for high-performance scenarios
        /// </summary>
        public static JsonSerializerOptions CreateOptimizedJsonOptions()
        {
            return new JsonSerializerOptions
            {
                PropertyNamingPolicy = null, // No conversion for performance
                WriteIndented = false,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                PropertyNameCaseInsensitive = true,
                NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString
            };
        }

        /// <summary>
        /// Streams large JSON array for memory-efficient processing
        /// </summary>
        public static IEnumerable<T> StreamJsonArray<T>(string filePath)
        {
            using var fileStream = File.OpenRead(filePath);
            using var document = JsonDocument.Parse(fileStream);
            
            if (document.RootElement.ValueKind == JsonValueKind.Array)
            {
                foreach (var element in document.RootElement.EnumerateArray())
                {
                    var json = element.GetRawText();
                    yield return FromJson<T>(json);
                }
            }
        }

        /// <summary>
        /// Batch serializes multiple objects to JSON file
        /// </summary>
        public static async Task BatchSerializeToJsonAsync<T>(IEnumerable<T> items, string filePath)
        {
            using var fileStream = new FileStream(filePath, FileMode.Create);
            await JsonSerializer.SerializeAsync(fileStream, items, DefaultJsonOptions);
        }

        /// <summary>
        /// Batch deserializes multiple objects from JSON file
        /// </summary>
        public static async Task<List<T>> BatchDeserializeFromJsonAsync<T>(string filePath)
        {
            using var fileStream = File.OpenRead(filePath);
            return await JsonSerializer.DeserializeAsync<List<T>>(fileStream, DefaultJsonOptions);
        }

        #endregion

        #region XML Advanced Operations

        /// <summary>
        /// Converts XML to JSON
        /// </summary>
        public static string XmlToJson(string xml)
        {
            if (string.IsNullOrWhiteSpace(xml)) return null;

            try
            {
                var xmlDoc = new System.Xml.XmlDocument();
                xmlDoc.LoadXml(xml);

                object ConvertNode(System.Xml.XmlNode node)
                {
                    if (node.ChildNodes.Count == 0)
                        return node.InnerText;

                    if (node.ChildNodes.Count == 1 && node.FirstChild.NodeType == System.Xml.XmlNodeType.Text)
                        return node.InnerText;

                    var dict = new Dictionary<string, object>();
                    foreach (System.Xml.XmlNode child in node.ChildNodes)
                    {
                        if (child.NodeType == System.Xml.XmlNodeType.Element)
                        {
                            if (dict.ContainsKey(child.Name))
                            {
                                if (dict[child.Name] is List<object> list)
                                    list.Add(ConvertNode(child));
                                else
                                {
                                    var newList = new List<object> { dict[child.Name], ConvertNode(child) };
                                    dict[child.Name] = newList;
                                }
                            }
                            else
                            {
                                dict[child.Name] = ConvertNode(child);
                            }
                        }
                    }
                    return dict;
                }

                var result = ConvertNode(xmlDoc.DocumentElement);
                return ToJson(new Dictionary<string, object> { [xmlDoc.DocumentElement.Name] = result });
            }
            catch (System.Xml.XmlException)
            {
                return null;
            }
        }

        /// <summary>
        /// Validates XML against XSD schema
        /// </summary>
        public static bool ValidateXmlAgainstSchema(string xml, string xsdSchema)
        {
            try
            {
                var schemas = new System.Xml.Schema.XmlSchemaSet();
                using var xsdReader = new StringReader(xsdSchema);
                schemas.Add("", System.Xml.XmlReader.Create(xsdReader));

                var xmlDoc = new System.Xml.XmlDocument();
                xmlDoc.LoadXml(xml);
                xmlDoc.Schemas = schemas;

                var isValid = true;
                xmlDoc.Validate((sender, args) => { isValid = false; });
                return isValid;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Minifies XML (removes whitespace and formatting)
        /// </summary>
        public static string MinifyXml(string xml)
        {
            if (string.IsNullOrWhiteSpace(xml)) return xml;

            try
            {
                var xmlDoc = new System.Xml.XmlDocument();
                xmlDoc.LoadXml(xml);

                using var stringWriter = new StringWriter();
                using var xmlWriter = System.Xml.XmlWriter.Create(stringWriter, new System.Xml.XmlWriterSettings 
                { 
                    Indent = false,
                    OmitXmlDeclaration = true
                });
                xmlDoc.Save(xmlWriter);
                return stringWriter.ToString();
            }
            catch (System.Xml.XmlException)
            {
                return xml;
            }
        }

        /// <summary>
        /// Formats XML with proper indentation
        /// </summary>
        public static string FormatXml(string xml)
        {
            if (string.IsNullOrWhiteSpace(xml)) return xml;

            try
            {
                var xmlDoc = new System.Xml.XmlDocument();
                xmlDoc.LoadXml(xml);

                using var stringWriter = new StringWriter();
                using var xmlWriter = System.Xml.XmlWriter.Create(stringWriter, new System.Xml.XmlWriterSettings
                {
                    Indent = true,
                    IndentChars = "  ",
                    OmitXmlDeclaration = false
                });
                xmlDoc.Save(xmlWriter);
                return stringWriter.ToString();
            }
            catch (System.Xml.XmlException)
            {
                return xml;
            }
        }

        #endregion
    }
}
