# KBA.CoreUtilities

[![NuGet](https://img.shields.io/nuget/v/KBA.CoreUtilities.svg)](https://www.nuget.org/packages/KBA.CoreUtilities/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

**The most comprehensive multi-target .NET utility library (.NET 6.0/7.0/8.0) for enterprise applications with 500+ methods.**

## 📦 Installation

```bash
dotnet add package KBA.CoreUtilities
```

Or via NuGet Package Manager:

```
Install-Package KBA.CoreUtilities
```

## 🌟 Features

### Core Utilities
- **🌍 Country Utilities**: Complete country information (200+ countries) with ISO codes, phone codes, currencies, capitals, languages
- **📱 Phone Utilities**: International phone validation and formatting for 200+ countries
- **💰 Currency Utilities**: Support for all ISO 4217 currencies with EMV QR code generation
- **📲 QR Code Generation**: EMVCo-compliant QR codes for worldwide payment systems
- **🕒 DateTime Utilities**: Comprehensive date/time manipulation, formatting, and calculations
- **📄 Serialization**: Optimized JSON & XML serialization/deserialization
- **🌐 API Consumption**: REST and SOAP API utilities with authentication
- **🔢 Decimal Utilities**: Financial calculations with precision
- **📝 String Utilities**: String manipulation and validation
- **📊 Logging Utilities**: Structured logging helpers

### Advanced Utilities
- **✅ Validation Utilities**: IBAN, credit cards, BIC/SWIFT, VAT, SSN, ISBN validation
- **🔐 Cryptography Utilities**: Encryption (AES, RSA), hashing, digital signatures, secure tokens
- **📁 File Utilities**: File operations, compression, MIME type detection, directory management
- **🔍 Reflection Utilities**: Dynamic property/method access, type inspection, object mapping
- **⚡ Performance Utilities**: Profiling, caching, benchmarking, memory monitoring

### Extension Methods (120+ methods)
- **📝 String Extensions**: IsEmail, ToSHA256, Truncate, Capitalize, ToInt/Decimal/Enum, and 40+ more
- **📚 Collection Extensions**: ForEach, DistinctBy, Chunk, Shuffle, Random, Page, and 30+ more
- **🎯 Object Extensions**: Null handling, type conversions, JSON, cloning, validation, and 20+ more
- **⚡ Task Extensions**: WithTimeout, Retry, FireAndForget, RunInParallel, and 10+ more
- **⚙️ Configuration Extensions**: Typed access, validation, environment fallback, and 20+ more

## 📚 Table of Contents

### Core Utilities
- [Country Utilities](#-country-utilities)
- [Phone Utilities](#-phone-utilities)
- [QR Code Utilities](#-qr-code-utilities)
- [DateTime Utilities](#-datetime-utilities)
- [Serialization Utilities](#-serialization-utilities)
- [API Utilities](#-api-utilities)
- [Decimal Utilities](#-decimal-utilities)
- [String Utilities](#-string-utilities)

### Advanced Utilities
- [Validation Utilities](#-validation-utilities)
- [Cryptography Utilities](#-cryptography-utilities)
- [File Utilities](#-file-utilities)
- [Reflection Utilities](#-reflection-utilities)
- [Performance Utilities](#-performance-utilities)

### Extension Methods
- [String Extensions](#-string-extensions)
- [Collection Extensions](#-collection-extensions)
- [Object Extensions](#-object-extensions)
- [Task Extensions](#-task-extensions)
- [Configuration Extensions](#-configuration-extensions)

---

## 🌍 Country Utilities

### Get Country Information

```csharp
using KBA.CoreUtilities.Utilities;

// By ISO2 code
var country = CountryUtils.GetCountryByIso2("SN");
Console.WriteLine($"{country.Name} - Capital: {country.Capital}"); // Sénégal - Capital: Dakar

// By ISO3 code
var usa = CountryUtils.GetCountryByIso3("USA");

// By phone code
var france = CountryUtils.GetCountryByPhoneCode("33");

// By currency code
var countries = CountryUtils.GetCountriesByCurrency("XOF");
foreach (var c in countries)
{
    Console.WriteLine(c.Name); // Burkina Faso, Bénin, Côte d'Ivoire, etc.
}

// By name
var morocco = CountryUtils.GetCountryByName("Maroc");
```

### Regional Queries

```csharp
// Check if country is in UEMOA (West African Economic and Monetary Union)
bool isUemoa = CountryUtils.IsInUEMOA("SN"); // true

// Check if country is in CEMAC (Central African Economic and Monetary Community)
bool isCemac = CountryUtils.IsInCEMAC("CM"); // true

// Get all countries in a region
var westAfricanCountries = CountryUtils.GetCountriesByRegion("Afrique de l'Ouest");

// Language checks
bool isFrancophone = CountryUtils.IsFrancophone("SN"); // true
bool isAnglophone = CountryUtils.IsAnglophone("NG"); // true
```

### Currency and Formatting

```csharp
// Get currency code
string currency = CountryUtils.GetCurrencyFromIso2("FR"); // EUR

// Format currency
decimal amount = 1000.50m;
string formatted = CountryUtils.FormatCurrency(amount, "FR"); // 1 000,50 €

// Format date
DateTime date = DateTime.Now;
string formattedDate = CountryUtils.FormatDate(date, "FR"); // French date format

// Get time zone
string timezone = CountryUtils.GetTimeZone("SN"); // UTC+0
```

---

## 📱 Phone Utilities

### Validate Phone Numbers

```csharp
using KBA.CoreUtilities.Utilities;

// Validate phone number
bool isValid = PhoneUtils.IsValidPhoneNumber("+221771234567", "SN"); // true

// Validate mobile number (checks if it's a mobile line)
bool isMobile = PhoneUtils.IsValidMobileNumber("771234567", "SN"); // true

// Validate international format
bool isInternational = PhoneUtils.IsValidInternationalPhone("+221771234567"); // true
```

### Format Phone Numbers

```csharp
// Format for display
string formatted = PhoneUtils.FormatPhoneNumber("771234567", "SN");
Console.WriteLine(formatted); // +221 77 123 45 67

// Format international
string international = PhoneUtils.FormatInternational("771234567", "SN");
Console.WriteLine(international); // +221771234567

// Format national
string national = PhoneUtils.FormatNational("771234567", "SN");
Console.WriteLine(national); // 77 123 45 67

// Format E.164 (for APIs)
string e164 = PhoneUtils.FormatE164("771234567", "SN");
Console.WriteLine(e164); // +221771234567
```

### Parse and Extract

```csharp
// Parse phone number
var phoneInfo = PhoneUtils.ParsePhoneNumber("+221771234567");
Console.WriteLine($"Country: {phoneInfo.CountryCode}"); // SN
Console.WriteLine($"Number: {phoneInfo.NationalNumber}"); // 771234567

// Extract country code
string countryCode = PhoneUtils.GetCountryCodeFromPhone("+221771234567"); // SN

// Get phone code for country
string phoneCode = PhoneUtils.GetPhoneCodeForCountry("SN"); // 221
```

---

## 📲 QR Code Utilities

### EMV Payment QR Codes (Worldwide Support)

```csharp
using KBA.CoreUtilities.Utilities;

// Generate EMV QR code for Senegal (XOF)
var paymentData = EmvPaymentData.CreateForCountry(
    merchantName: "Boutique Dakar",
    merchantCity: "Dakar",
    amount: 5000m,
    countryCode: "SN",
    currencyCode: "952" // XOF
);

byte[] qrCodeBytes = QrCodeUtils.GenerateEmvPaymentQrCodeImage(paymentData);
File.WriteAllBytes("payment_qr.png", qrCodeBytes);

// Generate for any country - Examples:

// USA (USD)
var usPayment = EmvPaymentData.CreateForCountry("Shop NY", "New York", 50.00m, "US", "840");

// France (EUR)
var euPayment = EmvPaymentData.CreateForCountry("Boutique Paris", "Paris", 25.00m, "FR", "978");

// Nigeria (NGN)
var ngPayment = EmvPaymentData.CreateForCountry("Shop Lagos", "Lagos", 10000m, "NG", "566");

// Morocco (MAD)
var maPayment = EmvPaymentData.CreateForCountry("Marché Casablanca", "Casablanca", 200m, "MA", "504");
```

### Mobile Money QR Codes

```csharp
// Generate mobile money QR code (Orange Money, Wave, Free Money, etc.)
byte[] mobileMoneyQr = QrCodeUtils.GenerateMobileMoneyQrCode(
    merchantName: "Restaurant Teranga",
    merchantCity: "Dakar",
    amount: 15000m,
    phoneNumber: "771234567",
    provider: "Orange Money",
    width: 300,
    height: 300
);
```

### Specialized QR Codes

```csharp
// vCard (business card)
string vCardData = QrCodeUtils.GenerateVCardData(
    name: "John Doe",
    phone: "+221771234567",
    email: "john@example.com",
    organization: "KBA Corp",
    website: "https://example.com"
);
byte[] vCardQr = QrCodeUtils.GenerateQrCode(vCardData, 300, 300);

// WiFi QR code
string wifiData = QrCodeUtils.GenerateWifiData(
    ssid: "MyWiFi",
    password: "SecurePass123",
    encryption: WifiEncryptionType.WPA
);
byte[] wifiQr = QrCodeUtils.GenerateQrCode(wifiData, 300, 300);

// SMS QR code
string smsData = QrCodeUtils.GenerateSmsData("+221771234567", "Hello from QR!");
byte[] smsQr = QrCodeUtils.GenerateQrCode(smsData, 300, 300);

// Email QR code
string emailData = QrCodeUtils.GenerateEmailData(
    email: "contact@example.com",
    subject: "Inquiry",
    body: "I would like to know more..."
);
byte[] emailQr = QrCodeUtils.GenerateQrCode(emailData, 300, 300);

// Location QR code
string locationData = QrCodeUtils.GenerateLocationData(
    latitude: 14.6937m,
    longitude: -17.4441m
);
byte[] locationQr = QrCodeUtils.GenerateQrCode(locationData, 300, 300);
```

### Read QR Codes

```csharp
// Read QR code from image
string content = QrCodeUtils.ReadQrCodeFromFile("qr_code.png");
Console.WriteLine(content);

// Read from byte array
byte[] qrBytes = File.ReadAllBytes("qr_code.png");
string decodedText = QrCodeUtils.ReadQrCode(qrBytes);

// Parse EMV QR code
string emvQrData = "000201010211..."; // EMV QR code data
var parsed = QrCodeUtils.ParseEmvPaymentQrCode(emvQrData);
Console.WriteLine($"Merchant: {parsed.MerchantName}");
Console.WriteLine($"Amount: {parsed.Amount}");
Console.WriteLine($"Currency: {QrCodeUtils.GetCurrencyDescription(parsed.CurrencyCode)}");
```

### Currency Description (All World Currencies)

```csharp
// Get description for any currency (ISO 4217)
string xof = QrCodeUtils.GetCurrencyDescription("952"); // West African CFA Franc (XOF)
string usd = QrCodeUtils.GetCurrencyDescription("840"); // US Dollar (USD)
string eur = QrCodeUtils.GetCurrencyDescription("978"); // Euro (EUR)
string ngn = QrCodeUtils.GetCurrencyDescription("566"); // Nigerian Naira (NGN)
string mad = QrCodeUtils.GetCurrencyDescription("504"); // Moroccan Dirham (MAD)
string cny = QrCodeUtils.GetCurrencyDescription("156"); // Chinese Yuan (CNY)
string jpy = QrCodeUtils.GetCurrencyDescription("392"); // Japanese Yen (JPY)
```

---

## 🕒 DateTime Utilities

### Formatting

```csharp
using KBA.CoreUtilities.Utilities;

DateTime now = DateTime.Now;

// ISO 8601 format
string iso = DateTimeUtils.ToIso8601(now); // 2024-10-27T19:12:00Z

// Unix timestamp
long timestamp = DateTimeUtils.ToUnixTimestamp(now);
DateTime fromTimestamp = DateTimeUtils.FromUnixTimestamp(timestamp);

// Relative time
string relative = DateTimeUtils.ToRelativeTime(DateTime.Now.AddHours(-2)); // 2 hours ago
string future = DateTimeUtils.ToRelativeTime(DateTime.Now.AddDays(3)); // in 3 days

// Locale-aware formatting
string shortDate = DateTimeUtils.ToShortDate(now); // 27/10/2024 (depends on culture)
string longDate = DateTimeUtils.ToLongDate(now); // Sunday, October 27, 2024
```

### Date Calculations

```csharp
// Add business days (skip weekends)
DateTime result = DateTimeUtils.AddBusinessDays(DateTime.Today, 5);

// Get business days between dates
int businessDays = DateTimeUtils.GetBusinessDaysDifference(startDate, endDate);

// Start and end of periods
DateTime startOfDay = DateTimeUtils.StartOfDay(DateTime.Now);
DateTime endOfDay = DateTimeUtils.EndOfDay(DateTime.Now);
DateTime startOfWeek = DateTimeUtils.StartOfWeek(DateTime.Now);
DateTime endOfMonth = DateTimeUtils.EndOfMonth(DateTime.Now);
DateTime startOfYear = DateTimeUtils.StartOfYear(DateTime.Now);
DateTime endOfQuarter = DateTimeUtils.EndOfQuarter(DateTime.Now);

// Age calculation
int age = DateTimeUtils.GetAge(new DateTime(1990, 5, 15)); // Calculate age from birthdate

// Days in month
int daysInCurrentMonth = DateTimeUtils.GetDaysInCurrentMonth();
```

### Validation

```csharp
// Check day types
bool isWeekend = DateTimeUtils.IsWeekend(DateTime.Now);
bool isWeekday = DateTimeUtils.IsWeekday(DateTime.Now);

// Check time periods
bool isToday = DateTimeUtils.IsToday(someDate);
bool isYesterday = DateTimeUtils.IsYesterday(someDate);
bool isTomorrow = DateTimeUtils.IsTomorrow(someDate);
bool isInPast = DateTimeUtils.IsInPast(someDate);
bool isInFuture = DateTimeUtils.IsInFuture(someDate);

// Range validation
bool inRange = DateTimeUtils.IsInRange(date, startDate, endDate);

// Leap year
bool isLeap = DateTimeUtils.IsLeapYear(2024); // true
```

### Date Ranges

```csharp
// Common date ranges
var today = DateTimeUtils.GetTodayRange();
var yesterday = DateTimeUtils.GetYesterdayRange();
var thisWeek = DateTimeUtils.GetThisWeekRange();
var lastWeek = DateTimeUtils.GetLastWeekRange();
var thisMonth = DateTimeUtils.GetThisMonthRange();
var lastMonth = DateTimeUtils.GetLastMonthRange();
var thisYear = DateTimeUtils.GetThisYearRange();
var last30Days = DateTimeUtils.GetLastNDaysRange(30);
var next7Days = DateTimeUtils.GetNextNDaysRange(7);

// Usage example
var (start, end) = DateTimeUtils.GetThisMonthRange();
Console.WriteLine($"This month: {start:d} to {end:d}");
```

### TimeSpan Utilities

```csharp
// Format duration
TimeSpan duration = TimeSpan.FromMinutes(125);
string formatted = DateTimeUtils.FormatDuration(duration); // 2h 5m 0s

// Parse duration
TimeSpan? parsed = DateTimeUtils.ParseDuration("2h 30m"); // 2.5 hours
```

### Time Zone Operations

```csharp
// Convert between time zones
TimeZoneInfo parisZone = DateTimeUtils.GetTimeZoneById("Europe/Paris");
TimeZoneInfo nyZone = DateTimeUtils.GetTimeZoneById("America/New_York");

DateTime parisTime = DateTime.Now;
DateTime nyTime = DateTimeUtils.ConvertTimeZone(parisTime, parisZone, nyZone);

// Convert to UTC
DateTime utc = DateTimeUtils.ToUtc(DateTime.Now);

// Convert from UTC
DateTime local = DateTimeUtils.FromUtc(DateTime.UtcNow);

// Get all time zones
var allZones = DateTimeUtils.GetAllTimeZones();
```

---

## 📄 Serialization Utilities

### JSON Serialization

```csharp
using KBA.CoreUtilities.Utilities;

// Simple serialization
var person = new Person { Name = "John", Age = 30 };
string json = SerializationUtils.ToJson(person);

// Compact JSON (no indentation)
string compact = SerializationUtils.ToCompactJson(person);

// Deserialization
Person deserializedPerson = SerializationUtils.FromJson<Person>(json);

// Deep clone
Person clone = SerializationUtils.DeepClone(person);
```

### JSON File Operations

```csharp
// Save to file
SerializationUtils.ToJsonFile(person, "person.json");

// Load from file
Person loadedPerson = SerializationUtils.FromJsonFile<Person>("person.json");

// Async operations
await SerializationUtils.ToJsonFileAsync(person, "person.json");
Person asyncLoaded = await SerializationUtils.FromJsonFileAsync<Person>("person.json");
```

### Advanced JSON Operations

```csharp
// Validate JSON
bool isValid = SerializationUtils.IsValidJson(jsonString);

// Minify JSON (remove whitespace)
string minified = SerializationUtils.MinifyJson(jsonString);

// Format/Pretty print JSON
string formatted = SerializationUtils.FormatJson(compactJson);

// Get JSON property by path
string value = SerializationUtils.GetJsonPropertyValue(json, "user.address.city");

// Merge JSON objects
string merged = SerializationUtils.MergeJson(json1, json2);

// Compare JSON
bool areEqual = SerializationUtils.AreJsonEqual(json1, json2);

// Get JSON size
long sizeInBytes = SerializationUtils.GetJsonSize(person);
```

### XML Serialization

```csharp
// Serialize to XML
string xml = SerializationUtils.ToXml(person);

// Deserialize from XML
Person personFromXml = SerializationUtils.FromXml<Person>(xml);

// XML file operations
SerializationUtils.ToXmlFile(person, "person.xml");
Person loadedFromXml = SerializationUtils.FromXmlFile<Person>("person.xml");

// XML stream operations
using (var stream = File.OpenWrite("output.xml"))
{
    SerializationUtils.ToXmlStream(person, stream);
}
```

### Advanced XML Operations

```csharp
// Validate XML
bool isValidXml = SerializationUtils.IsValidXml(xmlString);

// Minify XML
string minifiedXml = SerializationUtils.MinifyXml(xmlString);

// Format XML
string formattedXml = SerializationUtils.FormatXml(xmlString);

// Convert between JSON and XML
string xml = SerializationUtils.JsonToXml(jsonString, "root");
string json = SerializationUtils.XmlToJson(xmlString);

// Validate XML against schema
bool isValidAgainstSchema = SerializationUtils.ValidateXmlAgainstSchema(xml, xsdSchema);
```

### Performance & Optimization

```csharp
// Create optimized options for high-performance scenarios
var optimizedOptions = SerializationUtils.CreateOptimizedJsonOptions();
string fastJson = SerializationUtils.ToJson(largeObject, optimizedOptions);

// Stream large JSON arrays (memory-efficient)
foreach (var item in SerializationUtils.StreamJsonArray<Person>("large_array.json"))
{
    // Process each item without loading entire file into memory
    Console.WriteLine(item.Name);
}

// Batch operations
var people = new List<Person> { person1, person2, person3 };
await SerializationUtils.BatchSerializeToJsonAsync(people, "people.json");
List<Person> loadedPeople = await SerializationUtils.BatchDeserializeFromJsonAsync<Person>("people.json");
```

---

## 🌐 API Utilities

### REST API Consumption

```csharp
using KBA.CoreUtilities.Utilities;

// GET request
var user = await ApiUtils.GetJsonAsync<User>("https://api.example.com/users/1");

// POST request
var newUser = new User { Name = "John", Email = "john@example.com" };
var response = await ApiUtils.PostJsonAsync<User, ApiResponse>(
    "https://api.example.com/users",
    newUser
);

// PUT request
var updatedUser = await ApiUtils.PutJsonAsync<User, User>(
    "https://api.example.com/users/1",
    user
);

// DELETE request
var result = await ApiUtils.DeleteStringAsync("https://api.example.com/users/1");
```

### Authentication

```csharp
// Bearer token authentication
var client = ApiUtils.CreateAuthenticatedHttpClient("your_bearer_token");
var data = await ApiUtils.GetJsonAsync<Data>("https://api.example.com/data", client);

// Basic authentication
var basicClient = ApiUtils.CreateBasicAuthHttpClient("username", "password");

// Custom headers
var headers = new Dictionary<string, string>
{
    ["X-API-Key"] = "your_api_key",
    ["X-Custom-Header"] = "value"
};
var customClient = ApiUtils.CreateHttpClient(headers);
```

### Advanced Features

```csharp
// Query string builder
var parameters = new Dictionary<string, object>
{
    ["page"] = 1,
    ["limit"] = 10,
    ["search"] = "john"
};
string url = ApiUtils.BuildQueryString("https://api.example.com/users", parameters);
// Result: https://api.example.com/users?page=1&limit=10&search=john

// Retry policy
var data = await ApiUtils.ExecuteWithRetryAsync(
    async () => await ApiUtils.GetJsonAsync<Data>("https://api.example.com/data"),
    maxRetries: 3,
    delayMs: 1000
);

// Download file
await ApiUtils.DownloadFileAsync("https://example.com/file.pdf", "downloaded.pdf");

// Upload file
string uploadResult = await ApiUtils.UploadFileAsync(
    "https://api.example.com/upload",
    "document.pdf",
    parameterName: "file"
);
```

### SOAP Services

```csharp
// Create SOAP client
var soapClient = new SoapClient("https://api.example.com/service.asmx");

// Call SOAP method
var soapResult = await soapClient.CallAsync<SoapResponse>(
    "GetUserData",
    new Dictionary<string, object>
    {
        ["userId"] = 123
    }
);
```

---

## 🔢 Decimal Utilities

### Financial Calculations

```csharp
using KBA.CoreUtilities.Utilities;

// Round to decimal places
decimal amount = 123.456m;
decimal rounded = DecimalUtils.Round(amount, 2); // 123.46

// Calculate percentage
decimal total = 1000m;
decimal percentage = DecimalUtils.CalculatePercentage(250m, total); // 25%

// Apply percentage
decimal result = DecimalUtils.ApplyPercentage(1000m, 10); // 1100 (1000 + 10%)

// Discount calculation
decimal discounted = DecimalUtils.ApplyDiscount(1000m, 15); // 850 (1000 - 15%)

// Compare decimals with tolerance
bool areEqual = DecimalUtils.AreEqual(0.1m + 0.2m, 0.3m, tolerance: 0.001m);
```

---

## 📝 String Utilities

### String Manipulation

```csharp
using KBA.CoreUtilities.Utilities;

// Truncate string
string text = "This is a long text that needs to be truncated";
string truncated = StringUtils.Truncate(text, 20); // "This is a long te..."

// Remove accents/diacritics
string withAccents = "Hôtel à Paris près de l'Élysée";
string normalized = StringUtils.RemoveAccents(withAccents); // "Hotel a Paris pres de l'Elysee"

// Generate slug
string slug = StringUtils.GenerateSlug("Hello World! This is a Test"); // "hello-world-this-is-a-test"

// Capitalize
string capitalized = StringUtils.Capitalize("hello world"); // "Hello world"

// Title case
string title = StringUtils.ToTitleCase("hello world"); // "Hello World"

// Camel case
string camel = StringUtils.ToCamelCase("hello-world-test"); // "helloWorldTest"

// Pascal case
string pascal = StringUtils.ToPascalCase("hello-world-test"); // "HelloWorldTest"

// Snake case
string snake = StringUtils.ToSnakeCase("HelloWorldTest"); // "hello_world_test"

// Kebab case
string kebab = StringUtils.ToKebabCase("HelloWorldTest"); // "hello-world-test"
```

### String Validation

```csharp
// Email validation
bool isEmail = StringUtils.IsValidEmail("user@example.com");

// URL validation
bool isUrl = StringUtils.IsValidUrl("https://www.example.com");

// Numeric validation
bool isNumeric = StringUtils.IsNumeric("12345");

// Contains only letters
bool isAlpha = StringUtils.IsAlpha("HelloWorld");

// Contains only alphanumeric
bool isAlphanumeric = StringUtils.IsAlphanumeric("Hello123");
```

---

## 📊 Logging Utilities

```csharp
using KBA.CoreUtilities.Utilities;
using Microsoft.Extensions.Logging;

// Create logger
ILogger logger = LoggingUtils.CreateLogger<MyClass>();

// Log with context
logger.LogInformation("User {UserId} performed {Action}", userId, action);

// Structured logging
logger.LogError(exception, "Error processing order {OrderId}", orderId);
```

---

## ✅ Validation Utilities

### IBAN Validation

```csharp
using KBA.CoreUtilities.Utilities;

// Validate IBAN
bool isValid = ValidationUtils.IsValidIban("FR7630006000011234567890189");
Console.WriteLine($"Valid: {isValid}"); // true

// Format IBAN with spaces
string formatted = ValidationUtils.FormatIban("FR7630006000011234567890189");
Console.WriteLine(formatted); // FR76 3000 6000 0112 3456 7890 189

// Get country code
string country = ValidationUtils.GetIbanCountryCode("FR7630006000011234567890189");
Console.WriteLine($"Country: {country}"); // FR
```

### Credit Card Validation

```csharp
// Validate credit card
bool isValid = ValidationUtils.IsValidCreditCard("4532015112830366");
Console.WriteLine($"Valid: {isValid}"); // true

// Get card type
string cardType = ValidationUtils.GetCreditCardType("4532015112830366");
Console.WriteLine($"Type: {cardType}"); // Visa

// Mask card number
string masked = ValidationUtils.MaskCreditCard("4532015112830366");
Console.WriteLine(masked); // ************0366
```

### BIC/SWIFT Validation

```csharp
// Validate BIC/SWIFT code
bool isValid = ValidationUtils.IsValidBic("BNPAFRPPXXX");
Console.WriteLine($"Valid: {isValid}"); // true

// Get country from BIC
string country = ValidationUtils.GetBicCountryCode("BNPAFRPPXXX");
Console.WriteLine($"Country: {country}"); // FR
```

### VAT Number Validation

```csharp
// Validate European VAT number
bool isValid = ValidationUtils.IsValidVatNumber("FR12345678901", "FR");
Console.WriteLine($"Valid: {isValid}"); // true

// Validate with auto-detection of country code
bool isValid2 = ValidationUtils.IsValidVatNumber("DE123456789");
Console.WriteLine($"Valid: {isValid2}");
```

### Additional Validations

```csharp
// US SSN validation
bool validSSN = ValidationUtils.IsValidUsSSN("123-45-6789");

// US EIN validation
bool validEIN = ValidationUtils.IsValidUsEIN("12-3456789");

// ISBN validation
bool validISBN = ValidationUtils.IsValidIsbn("978-3-16-148410-0");

// MAC address validation
bool validMAC = ValidationUtils.IsValidMacAddress("00:1B:63:84:45:E6");

// IP address validation
bool validIP4 = ValidationUtils.IsValidIPv4("192.168.1.1");
bool validIP6 = ValidationUtils.IsValidIPv6("2001:0db8:85a3::8a2e:0370:7334");

// Hex color validation
bool validColor = ValidationUtils.IsValidHexColor("#FF5733");

// Strong password validation
bool strongPassword = ValidationUtils.IsStrongPassword("MyP@ssw0rd123");
```

---

## 🔐 Cryptography Utilities

### Hashing

```csharp
using KBA.CoreUtilities.Utilities;

// SHA256 hash
string hash = CryptographyUtils.HashSHA256("Hello World");
Console.WriteLine(hash); // a591a6d40bf420404a011733cfb7b190d62c65bf0bcda32b57b277d9ad9f146e

// SHA512 hash
string hash512 = CryptographyUtils.HashSHA512("Hello World");

// MD5 hash (for checksums only)
string md5 = CryptographyUtils.HashMD5("Hello World");

// Password hashing with PBKDF2 (recommended for passwords)
string hashedPassword = CryptographyUtils.HashPassword("MySecurePassword");

// Verify password
bool isCorrect = CryptographyUtils.VerifyPassword("MySecurePassword", hashedPassword);
Console.WriteLine($"Password correct: {isCorrect}");
```

### AES Encryption

```csharp
// Encrypt with AES-256
string plainText = "Sensitive data to encrypt";
string key = "MySecureEncryptionKey123";

string encrypted = CryptographyUtils.EncryptAES(plainText, key);
Console.WriteLine($"Encrypted: {encrypted}");

// Decrypt
string decrypted = CryptographyUtils.DecryptAES(encrypted, key);
Console.WriteLine($"Decrypted: {decrypted}"); // Sensitive data to encrypt
```

### RSA Encryption & Digital Signatures

```csharp
// Generate RSA key pair
var (publicKey, privateKey) = CryptographyUtils.GenerateRSAKeyPair(2048);

// Encrypt with public key
string encrypted = CryptographyUtils.EncryptRSA("Secret message", publicKey);

// Decrypt with private key
string decrypted = CryptographyUtils.DecryptRSA(encrypted, privateKey);

// Sign data
string signature = CryptographyUtils.SignData("Important document", privateKey);

// Verify signature
bool isValid = CryptographyUtils.VerifySignature("Important document", signature, publicKey);
Console.WriteLine($"Signature valid: {isValid}");
```

### Secure Token Generation

```csharp
// Generate secure random string
string randomString = CryptographyUtils.GenerateRandomString(32);

// Generate secure token
string token = CryptographyUtils.GenerateSecureToken(32);

// Generate OTP
string otp = CryptographyUtils.GenerateOTP(6);
Console.WriteLine($"OTP: {otp}"); // e.g., 742831

// Generate GUID
string guid = CryptographyUtils.GenerateGuid();
```

### HMAC

```csharp
// Generate HMAC signature
string message = "API request data";
string secretKey = "MySecretKey";
string hmac = CryptographyUtils.GenerateHMAC(message, secretKey);

// Verify HMAC
bool isValid = CryptographyUtils.VerifyHMAC(message, hmac, secretKey);
Console.WriteLine($"HMAC valid: {isValid}");
```

### File Hashing

```csharp
// Compute file hash for integrity check
string fileHash = CryptographyUtils.ComputeFileHash("document.pdf", "SHA256");
Console.WriteLine($"File hash: {fileHash}");
```

---

## 📁 File Utilities

### File Operations

```csharp
using KBA.CoreUtilities.Utilities;

// Read file asynchronously
string content = await FileUtils.ReadAllTextAsync("file.txt");

// Write file asynchronously
await FileUtils.WriteAllTextAsync("output.txt", "Hello World");

// Append to file
await FileUtils.AppendTextAsync("log.txt", "New log entry\n");

// Read all lines
string[] lines = FileUtils.ReadAllLines("data.txt");

// Read large file lazily (memory efficient)
foreach (var line in FileUtils.ReadLinesLazy("large_file.txt"))
{
    Console.WriteLine(line);
}
```

### File Information

```csharp
// Get file size
long bytes = FileUtils.GetFileSize("document.pdf");

// Get formatted size
string sizeStr = FileUtils.GetFileSizeFormatted("document.pdf");
Console.WriteLine(sizeStr); // e.g., "2.5 MB"

// Format bytes
string formatted = FileUtils.FormatFileSize(1024 * 1024 * 5);
Console.WriteLine(formatted); // 5 MB

// Get file extension
string ext = FileUtils.GetFileExtension("document.pdf"); // pdf

// Get file name without extension
string name = FileUtils.GetFileNameWithoutExtension("document.pdf"); // document

// Get file times
DateTime created = FileUtils.GetFileCreationTime("file.txt");
DateTime modified = FileUtils.GetFileLastModifiedTime("file.txt");
```

### MIME Type Detection

```csharp
// Get MIME type
string mimeType = FileUtils.GetMimeType("image.jpg");
Console.WriteLine(mimeType); // image/jpeg

// Check file types
bool isImage = FileUtils.IsImageFile("photo.png"); // true
bool isDocument = FileUtils.IsDocumentFile("report.pdf"); // true
bool isVideo = FileUtils.IsVideoFile("movie.mp4"); // true
bool isAudio = FileUtils.IsAudioFile("song.mp3"); // true
```

### Compression

```csharp
// Compress file with GZip
await FileUtils.CompressFileAsync("large_file.txt", "large_file.txt.gz");

// Decompress file
await FileUtils.DecompressFileAsync("large_file.txt.gz", "decompressed.txt");

// Create ZIP archive from multiple files
string[] files = { "file1.txt", "file2.txt", "file3.txt" };
FileUtils.CreateZipArchive(files, "archive.zip");

// Extract ZIP archive
FileUtils.ExtractZipArchive("archive.zip", "extracted_folder");

// Compress entire directory
FileUtils.CompressDirectory("my_folder", "my_folder.zip");
```

### Directory Operations

```csharp
// Get all files recursively
string[] allFiles = FileUtils.GetAllFiles("/path/to/directory", "*.txt");

// Get directory size
long totalSize = FileUtils.GetDirectorySize("/path/to/directory");
string formattedSize = FileUtils.FormatFileSize(totalSize);

// Copy directory recursively
FileUtils.CopyDirectory("source_dir", "destination_dir", recursive: true);

// Delete directory
FileUtils.DeleteDirectory("temp_dir", recursive: true);

// Ensure directory exists
FileUtils.EnsureDirectoryExists("/path/to/new/directory");
```

### Path Operations

```csharp
// Combine paths
string fullPath = FileUtils.CombinePaths("Users", "Documents", "file.txt");

// Get relative path
string relativePath = FileUtils.GetRelativePath("/Users/home", "/Users/home/Documents/file.txt");
Console.WriteLine(relativePath); // Documents/file.txt

// Sanitize file name
string safe = FileUtils.SanitizeFileName("my*file?name.txt");
Console.WriteLine(safe); // my_file_name.txt
```

### Temporary Files

```csharp
// Create temporary file
string tempFile = FileUtils.CreateTempFile(".txt");
Console.WriteLine($"Temp file: {tempFile}");

// Create temporary directory
string tempDir = FileUtils.CreateTempDirectory();
Console.WriteLine($"Temp directory: {tempDir}");
```

### File Comparison

```csharp
// Compare two files byte by byte
bool areEqual = FileUtils.AreFilesEqual("file1.txt", "file2.txt");
Console.WriteLine($"Files are equal: {areEqual}");
```

---

## 🔍 Reflection Utilities

### Property Operations

```csharp
using KBA.CoreUtilities.Utilities;

// Get property value dynamically
var email = ReflectionUtils.GetPropertyValue(user, "Email");

// Set property value
ReflectionUtils.SetPropertyValue(user, "Status", "Active");

// Get properties with specific attribute
var props = ReflectionUtils.GetPropertiesWithAttribute<RequiredAttribute>(typeof(User));
```

### Method Invocation

```csharp
// Invoke method dynamically
var result = ReflectionUtils.InvokeMethod(service, "ProcessOrder", orderId, userId);

// Invoke static method
var config = ReflectionUtils.InvokeStaticMethod(typeof(AppConfig), "Load");

// Invoke generic method
var data = ReflectionUtils.InvokeGenericMethod(
    repository, 
    "GetById", 
    new[] { typeof(User) }, 
    userId);
```

### Type Inspection

```csharp
// Check if type implements interface
bool implements = ReflectionUtils.ImplementsInterface<IService>(typeof(MyService));

// Get all types implementing interface
var services = ReflectionUtils.GetTypesImplementingInterface<IPlugin>(assembly);

// Check if nullable
bool isNullable = ReflectionUtils.IsNullable(typeof(int?)); // true
```

### Object Mapping

```csharp
// Map object to another type
var userDto = ReflectionUtils.MapTo<User, UserDto>(user);

// Deep copy
var clone = ReflectionUtils.DeepCopy(originalObject);
```

### Instance Creation

```csharp
// Create instance with parameters
var service = ReflectionUtils.CreateInstance<MyService>(dependency1, dependency2);

// Create instance of type
var instance = ReflectionUtils.CreateInstance(type, args);
```

---

## ⚡ Performance Utilities

### Execution Time Measurement

```csharp
using KBA.CoreUtilities.Utilities;

// Measure execution time
var elapsed = PerformanceUtils.Measure(() => {
    // Heavy operation
    ProcessData();
});
Console.WriteLine($"Took {elapsed.TotalMilliseconds}ms");

// Measure with result
var (result, time) = PerformanceUtils.Measure(() => ComputeValue());
```

### Async Measurements

```csharp
// Measure async operations
var elapsed = await PerformanceUtils.MeasureAsync(async () => {
    await FetchDataAsync();
});

// Measure async with result
var (data, time) = await PerformanceUtils.MeasureAsync(async () => {
    return await LoadDataAsync();
});
```

### Profiling

```csharp
// Profile with using statement
using (PerformanceUtils.Profile("Database Query", (name, elapsed) => {
    Logger.LogInformation($"{name} completed in {elapsed.TotalMilliseconds}ms");
}))
{
    var data = database.Query();
}
```

### Memory Monitoring

```csharp
// Get current memory usage
var memoryBytes = PerformanceUtils.GetMemoryUsage();
var memoryFormatted = PerformanceUtils.GetMemoryUsageFormatted(); // "125.5 MB"

// Measure memory used by operation
var (result, memUsed) = PerformanceUtils.MeasureMemory(() => {
    return LoadLargeDataSet();
});
Console.WriteLine($"Used {PerformanceUtils.FormatBytes(memUsed)}");
```

### In-Memory Caching

```csharp
// Get or add to cache with expiration
var users = PerformanceUtils.GetOrAdd(
    "all_users", 
    () => database.GetAllUsers(),
    expiration: TimeSpan.FromMinutes(5));

// Async cache
var data = await PerformanceUtils.GetOrAddAsync(
    "api_data",
    async () => await apiClient.FetchDataAsync(),
    expiration: TimeSpan.FromMinutes(10));

// Direct cache operations
PerformanceUtils.SetCache("key", value, TimeSpan.FromHours(1));
var cached = PerformanceUtils.GetFromCache<MyType>("key");
PerformanceUtils.RemoveFromCache("key");
PerformanceUtils.ClearCache();
```

### Rate Limiting

```csharp
// Check if rate limited
if (PerformanceUtils.IsRateLimited("api_call", maxCalls: 100, TimeSpan.FromMinutes(1)))
{
    throw new TooManyRequestsException();
}

// Process request
ProcessApiCall();
```

### Benchmarking

```csharp
// Benchmark operation
var result = PerformanceUtils.Benchmark(() => Algorithm(), iterations: 1000);
Console.WriteLine(result);
// Output: Iterations: 1000, Total: 523.45ms, Avg: 0.52ms, Min: 0.48ms, Max: 2.31ms
```

---

## ⚡ Task Extensions

### Timeout Operations

```csharp
using KBA.CoreUtilities.Extensions;

// Add timeout to task
var result = await LongRunningTask()
    .WithTimeout(TimeSpan.FromSeconds(30));

// With cancellation token
var data = await FetchDataAsync()
    .WithTimeout(TimeSpan.FromSeconds(10), cancellationToken);
```

### Retry with Exponential Backoff

```csharp
// Retry on failure
var result = await (() => UnreliableOperationAsync())
    .Retry(
        maxRetries: 3,
        initialDelay: TimeSpan.FromSeconds(1),
        backoffMultiplier: 2.0);

// Sync retry
(() => UnreliableOperation()).Retry(maxRetries: 5);
```

### Exception Handling

```csharp
// Handle exceptions gracefully
var result = await RiskyOperationAsync()
    .HandleException(ex => {
        Logger.LogError(ex, "Operation failed");
        return defaultValue;
    });

// Return default on error
var data = await FetchDataAsync().OrDefault(new DataObject());
```

### Fire and Forget

```csharp
// Execute without waiting
SendEmailAsync(user).FireAndForget(onException: ex => {
    Logger.LogError(ex, "Email sending failed");
});
```

### Parallel Processing

```csharp
// Run tasks in parallel with degree limit
var urls = new[] { "url1", "url2", "url3", /*...*/ };
var tasks = urls.Select(url => DownloadAsync(url)).ToArray();

var results = await tasks.RunInParallel(maxDegreeOfParallelism: 5);
```

### Task Chaining

```csharp
// Chain tasks
var result = await FetchDataAsync()
    .Then(data => ProcessDataAsync(data))
    .Then(processed => SaveResultAsync(processed));
```

### WaitAll with Cancellation

```csharp
// Wait for all tasks with cancellation
var tasks = new[] { task1, task2, task3 };
await tasks.WaitAllAsync(cancellationToken);

// Wait with timeout
await tasks.WaitAllAsync(TimeSpan.FromSeconds(30));
```

---

## ⚙️ Configuration Extensions

### Typed Value Retrieval

```csharp
using KBA.CoreUtilities.Extensions;
using Microsoft.Extensions.Configuration;

// Get typed values with defaults
var timeout = configuration.GetInt("Timeout", defaultValue: 30);
var retries = configuration.GetInt("MaxRetries", 3);
var enabled = configuration.GetBool("FeatureEnabled", false);
var factor = configuration.GetDecimal("MultiplierFactor", 1.5m);
var mode = configuration.GetEnum<LogLevel>("LogLevel", LogLevel.Information);
var delay = configuration.GetTimeSpan("RetryDelay", TimeSpan.FromSeconds(5));
```

### Required Values

```csharp
// Get required value (throws if missing)
var apiKey = configuration.GetRequired("ApiKey");
var connectionString = configuration.GetRequired("DatabaseConnection");

// Get required with type conversion
var port = configuration.GetRequired<int>("Port");
```

### Environment Variable Fallback

```csharp
// Try config first, then environment variable
var apiUrl = configuration.GetFromConfigOrEnv("ApiUrl", "API_URL");

// Connection string with env fallback
var connString = configuration.GetConnectionStringOrEnv("Database");
```

### Section Operations

```csharp
// Bind section to typed object
var appSettings = configuration.GetSection<AppSettings>("App");

// Check if section exists
if (configuration.SectionExists("Features"))
{
    var features = configuration.GetSection<FeatureFlags>("Features");
}

// Get section as dictionary
var dict = configuration.GetDictionary("CustomSettings");
```

### Array and List Values

```csharp
// Get array from comma-separated value
var allowedHosts = configuration.GetArray("AllowedHosts");

// Get list of objects
var endpoints = configuration.GetList<EndpointConfig>("Endpoints");
```

### Validation

```csharp
// Validate required keys exist
configuration.ValidateRequired("ApiKey", "DatabaseUrl", "CacheTimeout");

// Validate section exists
configuration.ValidateSection("Logging");
```

### Debug Helpers

```csharp
// Dump all configuration (masks sensitive keys)
var configDump = configuration.DumpConfiguration(includeSensitive: false);
Console.WriteLine(configDump);

// Get all settings as dictionary
var allSettings = configuration.GetAllSettings();
```

---

## 🎯 Real-World Examples

### Example 1: Mobile Money Payment with QR Code

```csharp
using KBA.CoreUtilities.Utilities;

// Create EMV payment for mobile money
var payment = EmvPaymentData.CreateMobileMoney(
    merchantName: "Restaurant Teranga",
    merchantCity: "Dakar",
    amount: 15000m,
    phoneNumber: "771234567",
    provider: "Orange Money",
    countryCode: "SN",
    currencyCode: "952" // XOF
);

// Generate QR code
byte[] qrCode = QrCodeUtils.GenerateEmvPaymentQrCodeImage(payment, 400, 400);

// Save QR code
File.WriteAllBytes("payment_qr.png", qrCode);

// Validate the payment data
if (payment.IsValid())
{
    Console.WriteLine("Payment QR code generated successfully!");
    Console.WriteLine($"Currency: {QrCodeUtils.GetCurrencyDescription(payment.CurrencyCode)}");
}
```

### Example 2: International Phone Validation & Formatting

```csharp
// Validate and format phones from multiple countries
var phones = new[]
{
    ("+221771234567", "SN"),  // Senegal
    ("+33612345678", "FR"),   // France
    ("+2349012345678", "NG"), // Nigeria
    ("+16505551234", "US")    // USA
};

foreach (var (phone, country) in phones)
{
    if (PhoneUtils.IsValidPhoneNumber(phone, country))
    {
        string formatted = PhoneUtils.FormatPhoneNumber(phone, country);
        string e164 = PhoneUtils.FormatE164(phone, country);
        
        Console.WriteLine($"{country}: {formatted} (E.164: {e164})");
    }
}
```

### Example 3: Date Range Reporting

```csharp
// Generate monthly report
var (startDate, endDate) = DateTimeUtils.GetThisMonthRange();

Console.WriteLine($"Report Period: {DateTimeUtils.ToShortDate(startDate)} - {DateTimeUtils.ToShortDate(endDate)}");
Console.WriteLine($"Business Days: {DateTimeUtils.GetBusinessDaysDifference(startDate, endDate)}");

// Get all business days in range
var businessDays = DateTimeUtils.GetBusinessDaysBetween(startDate, endDate);
Console.WriteLine($"Working days: {string.Join(", ", businessDays.Select(d => d.ToString("dd/MM")))}");
```

### Example 4: API Integration with Retry

```csharp
// Fetch data from API with automatic retry on failure
var userData = await ApiUtils.ExecuteWithRetryAsync(async () =>
{
    var client = ApiUtils.CreateAuthenticatedHttpClient(apiToken);
    return await ApiUtils.GetJsonAsync<UserData>(apiUrl, client);
}, maxRetries: 3, delayMs: 2000);

// Serialize to JSON and save
await SerializationUtils.ToJsonFileAsync(userData, "user_data.json");
```

---

## 📋 Requirements

- **.NET 8.0** or higher
- C# 10.0 or higher

### Dependencies

- `System.Text.Json` (>= 8.0.5)
- `System.ComponentModel.Annotations` (>= 5.0.0)
- `Microsoft.Extensions.Logging.Abstractions` (>= 8.0.0)
- `System.IdentityModel.Tokens.Jwt` (>= 7.1.2)
- `QRCoder` (>= 1.6.0)
- `ZXing.Net` (>= 0.16.9)
- `System.Drawing.Common` (>= 8.0.0)

---

## 🌟 Highlights

- ✅ **Production-Ready**: Battle-tested in financial services
- ✅ **EMV Compliant**: Full EMVCo QR code specification support
- ✅ **200+ Countries**: Complete country, phone, and currency data
- ✅ **Worldwide Support**: All ISO 4217 currencies supported
- ✅ **Well-Documented**: Comprehensive examples and API docs
- ✅ **High Performance**: Optimized for speed and memory efficiency
- ✅ **Type-Safe**: Strongly typed with full IntelliSense support
- ✅ **Async-First**: Modern async/await patterns throughout

---

Made with ❤️ for the African FinTech community and worldwide payment systems.
