# KBA.CoreUtilities

[![NuGet](https://img.shields.io/nuget/v/KBA.CoreUtilities.svg)](https://www.nuget.org/packages/KBA.CoreUtilities/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![.NET](https://img.shields.io/badge/.NET-6.0%20%7C%207.0%20%7C%208.0-512BD4)](https://dotnet.microsoft.com/)
[![Downloads](https://img.shields.io/nuget/dt/KBA.CoreUtilities.svg)](https://www.nuget.org/packages/KBA.CoreUtilities/)

**The most comprehensive multi-target .NET utility library (.NET 6.0/7.0/8.0) for enterprise applications with 500+ production-ready methods.**

Perfect for FinTech, banking, mobile money, payment systems, microservices, and any modern .NET application.

## 🚀 Quick Start

```bash
dotnet add package KBA.CoreUtilities
```

```csharp
using KBA.CoreUtilities.Utilities;
using KBA.CoreUtilities.Extensions;

// Validate IBAN
bool isValid = ValidationUtils.IsValidIBAN("FR1420041010050500013M02606");

// Generate EMV QR Code for payment
var payment = EmvPaymentData.CreateMobileMoney(
    merchantName: "My Store",
    mobileNumber: "+221781234567",
    amount: 25000m,
    currency: "XOF"
);
var qrCode = QrCodeUtils.GenerateEmvQrCodeAsPng(payment);

// Async with retry and timeout
var result = await (() => FetchDataAsync())
    .Retry(maxRetries: 3)
    .WithTimeout(TimeSpan.FromSeconds(30));

// Performance monitoring
using (PerformanceUtils.Profile("API Call", LogPerformance))
{
    await ProcessDataAsync();
}
```

## 🌟 Features

### 💰 Financial Services
- **IBAN Validation**: Full Mod 97 validation for 80+ countries
- **Credit Card Validation**: Luhn algorithm, BIN detection, card type identification
- **BIC/SWIFT Validation**: Format and structure validation
- **VAT Number Validation**: European VAT number verification
- **EMV QR Codes**: EMVCo-compliant QR codes for payments (170+ currencies)
- **Currency Support**: All ISO 4217 currencies with symbols and formatting

### 🌍 International Support
- **200+ Countries**: Complete information (ISO codes, capitals, currencies, languages)
- **Phone Validation**: International phone number validation and formatting (E.164)
- **Multi-language**: Support for country and currency names in multiple languages

### 🔐 Cryptography & Security
- **AES-256 Encryption**: Symmetric encryption with secure key derivation
- **RSA-2048/4096**: Asymmetric encryption and digital signatures
- **Password Hashing**: PBKDF2 with configurable iterations (10K default)
- **Hash Functions**: SHA256, SHA512, MD5, HMAC
- **Secure Tokens**: Cryptographically secure token and OTP generation

### ⚡ Async & Performance
- **Task Extensions**: WithTimeout, Retry with exponential backoff, FireAndForget
- **Parallel Processing**: RunInParallel with degree limits
- **Performance Monitoring**: Profiling, benchmarking, memory tracking
- **In-Memory Caching**: Cache with expiration policies and invalidation
- **Rate Limiting**: Request throttling and quota management

### 🔍 Reflection & Dynamic Access
- **Property/Method Invocation**: Dynamic access to properties and methods
- **Type Inspection**: Interface checking, nullable detection, type discovery
- **Object Mapping**: Automatic mapping between types
- **Deep Cloning**: Deep copy with reflection
- **Attribute Handling**: Custom attribute discovery and processing

### ⚙️ Configuration Management
- **Type-Safe Access**: GetInt, GetBool, GetEnum, GetTimeSpan, etc.
- **Required Validation**: Validate required configuration keys
- **Environment Fallback**: Automatic fallback to environment variables
- **Section Binding**: Bind configuration sections to typed objects
- **Debug Helpers**: Configuration dumping with sensitive data masking

### 📝 Extension Methods (120+)
- **String Extensions** (50+): IsEmail, IsValidIPv4, ToSHA256, Truncate, Capitalize, ToInt/Decimal/Enum, RemoveDiacritics, Mask, and more
- **Collection Extensions** (40+): ForEach, DistinctBy, Chunk, Shuffle, Random, Page, WhereNotNull, MinBy/MaxBy, and more
- **Object Extensions** (30+): Null handling, type conversions, JSON operations, cloning, validation, and more

### 📁 File & I/O Operations
- **File Operations**: Read, write, copy, move with safety checks
- **Compression**: GZip and ZIP compression/decompression
- **MIME Detection**: Automatic MIME type detection
- **Directory Management**: Recursive operations, size calculation
- **Temporary Files**: Secure temporary file and directory creation

### 🕒 Date & Time
- **50+ Methods**: Age calculation, business days, ISO 8601, Unix timestamps
- **Formatting**: Multiple format options with localization
- **Calculations**: Add/subtract business days, get date ranges
- **Validation**: IsWeekend, IsBusinessDay, IsFutureDate, IsPastDate

### 📄 Serialization & APIs
- **JSON & XML**: Fast serialization/deserialization
- **REST APIs**: Helper methods for GET, POST, PUT, DELETE
- **SOAP Client**: SOAP web service consumption (.NET 8)
- **API Builder**: Fluent REST API builder (.NET 8)

### 📊 Logging & Monitoring
- **Structured Logging**: ILogger extensions with context enrichment
- **Correlation IDs**: Automatic correlation ID tracking
- **Exception Logging**: Enhanced exception logging with stack traces
- **Performance Logging**: Automatic performance metric logging

## 📦 What's Included

| Module | Description | Methods |
|--------|-------------|---------|
| **StringExtensions** | Email/IP validation, hashing, conversions, formatting | 50+ |
| **CollectionExtensions** | LINQ enhancements, batching, pagination | 40+ |
| **ObjectExtensions** | Null handling, JSON, type conversions | 30+ |
| **TaskExtensions** | Async patterns, retry, timeout, parallel processing | 15+ |
| **ConfigurationExtensions** | Type-safe config access, validation | 25+ |
| **ValidationUtils** | IBAN, credit cards, BIC, VAT, SSN, ISBN | 20+ |
| **CryptographyUtils** | AES, RSA, hashing, signatures, tokens | 30+ |
| **ReflectionUtils** | Dynamic access, mapping, type inspection | 30+ |
| **PerformanceUtils** | Profiling, caching, benchmarking | 20+ |
| **CountryUtils** | 200+ countries with ISO codes | 15+ |
| **PhoneUtils** | International phone validation | 10+ |
| **QrCodeUtils** | EMV QR codes, barcode generation | 15+ |
| **DateTimeUtils** | Date/time manipulation and formatting | 50+ |
| **FileUtils** | File operations, compression, MIME | 25+ |
| **SerializationUtils** | JSON/XML serialization | 10+ |
| **ApiUtils** | REST/SOAP API consumption | 15+ |
| **DecimalUtils** | Financial calculations | 10+ |
| **StringUtils** | String manipulation | 15+ |
| **LoggingUtils** | Structured logging | 10+ |
| **RestApiBuilder** | Fluent API builder (.NET 8 only) | 20+ |

**Total: 20 modules, 500+ methods**

## 📚 Documentation

### Quick Links
- 📖 [Full Documentation](./KBA.CoreUtilities/README.md) - Complete API reference with examples
- 🚀 [Quick Start Examples](./QUICK_START_EXAMPLES.md) - Ready-to-use code samples
- 📝 [Changelog](./CHANGELOG.md) - Version history and release notes
- 🤝 [Contributing](./CONTRIBUTING.md) - How to contribute

### Key Examples

#### Financial Validation
```csharp
// IBAN validation
bool validIban = ValidationUtils.IsValidIBAN("FR1420041010050500013M02606");

// Credit card validation
bool validCard = ValidationUtils.IsValidCreditCard("4532015112830366");
var cardType = ValidationUtils.GetCreditCardType("4532015112830366"); // Visa

// BIC/SWIFT validation
bool validBic = ValidationUtils.IsValidBIC("BNPAFRPP");
```

#### EMV QR Code Generation
```csharp
var payment = EmvPaymentData.CreateMobileMoney(
    merchantName: "Restaurant",
    mobileNumber: "+221781234567",
    amount: 50000m,
    currency: "XOF"
);

// Generate PNG QR code
byte[] qrCode = QrCodeUtils.GenerateEmvQrCodeAsPng(payment, 300);
File.WriteAllBytes("payment.png", qrCode);
```

#### Async Operations with Resilience
```csharp
// Retry with exponential backoff
var data = await (() => apiClient.GetDataAsync())
    .Retry(maxRetries: 3, initialDelay: TimeSpan.FromSeconds(1));

// Timeout
var result = await LongRunningTask()
    .WithTimeout(TimeSpan.FromSeconds(30));

// Fire and forget
SendNotificationAsync(user).FireAndForget(
    onException: ex => logger.LogError(ex, "Notification failed"));
```

#### Performance Monitoring
```csharp
// Profile operation
using (PerformanceUtils.Profile("Database Query", (name, time) => 
    logger.LogInformation($"{name}: {time.TotalMilliseconds}ms")))
{
    var users = await db.Users.ToListAsync();
}

// Cache with expiration
var config = PerformanceUtils.GetOrAdd(
    "app_config",
    () => LoadConfiguration(),
    expiration: TimeSpan.FromMinutes(10));

// Benchmark
var result = PerformanceUtils.Benchmark(() => Algorithm(), iterations: 1000);
Console.WriteLine(result); // Avg, Min, Max times
```

#### Configuration Management
```csharp
// Type-safe access
var timeout = configuration.GetInt("Timeout", 30);
var apiUrl = configuration.GetRequired("ApiUrl");
var logLevel = configuration.GetEnum<LogLevel>("LogLevel", LogLevel.Information);

// Environment fallback
var connString = configuration.GetConnectionStringOrEnv("Database");

// Validation
configuration.ValidateRequired("ApiKey", "DatabaseUrl");
```

## 🎯 Use Cases

- **FinTech Applications**: Payment processing, IBAN validation, EMV QR codes
- **Banking Systems**: Credit card validation, BIC/SWIFT verification, secure transactions
- **Mobile Money**: QR code generation, currency handling, phone validation
- **E-commerce**: Payment validation, international support, secure data handling
- **Microservices**: Async patterns, configuration management, performance monitoring
- **Enterprise Applications**: Reflection utilities, caching, logging, validation

## 🏗️ Multi-Targeting

Supports **.NET 6.0**, **.NET 7.0**, and **.NET 8.0** with framework-specific optimizations:

- Automatic selection based on your project's target framework
- Conditional dependencies per framework
- Framework-specific features (e.g., RestApiBuilder for .NET 8)

## 🔧 Requirements

- **.NET 6.0 SDK** or higher
- **C# 10** or higher
- Optional: ASP.NET Core (for RestApiBuilder in .NET 8)

## 📥 Installation

### Via .NET CLI
```bash
dotnet add package KBA.CoreUtilities
```

### Via Package Manager
```powershell
Install-Package KBA.CoreUtilities
```

### Via PackageReference
```xml
<PackageReference Include="KBA.CoreUtilities" Version="1.3.1" />
```

## 🌍 Supported Regions

- **Europe**: Full IBAN validation, SEPA support, VAT validation
- **Africa**: UEMOA/CEMAC currencies, mobile money formats
- **Americas**: North & South American payment systems
- **Asia-Pacific**: Asian currencies and payment formats
- **Middle East**: GCC currencies and formats

**200+ countries** | **170+ currencies** | **80+ IBAN countries**

## 📄 License

This project is licensed under the **MIT License** - see the [LICENSE](LICENSE) file for details.

## 🤝 Contributing

Contributions are welcome! Please read our [Contributing Guide](CONTRIBUTING.md) for details on:
- Code of conduct
- Development setup
- Pull request process
- Coding standards

## 📞 Support

- 📧 **Issues**: [GitHub Issues](https://github.com/khalilbenaz/KBACoreUtilities/issues)
- 📦 **NuGet**: [Package Page](https://www.nuget.org/packages/KBA.CoreUtilities/)
- 📖 **Documentation**: See [README](./KBA.CoreUtilities/README.md)

## 🔄 Changelog

See [CHANGELOG.md](CHANGELOG.md) for a detailed version history.

### Latest Version: 1.3.1 (Oct 2025)
- ✅ Complete README documentation for v1.3.0 features
- ✅ TaskExtensions, ReflectionUtils, PerformanceUtils, ConfigurationExtensions
- ✅ 500+ methods, 20 modules, multi-target support

## ⭐ Star History

If you find this library useful, please consider giving it a star ⭐

## 🎖️ Credits

Developed and maintained by **Khalil Benazzouz**

- GitHub: [@khalilbenaz](https://github.com/khalilbenaz)
- LinkedIn: [Khalil Benazzouz](https://www.linkedin.com/in/khalilbenazzouz/)
- Issues: [Report an issue](https://github.com/khalilbenaz/KBACoreUtilities/issues)

## 📊 Stats

- **500+ Methods**: Production-ready utilities
- **20 Modules**: Organized and documented
- **120+ Extensions**: String, Collection, Object, Task, Configuration
- **200+ Countries**: Complete international support
- **170+ Currencies**: ISO 4217 compliant
- **100% Documented**: Full XML documentation and examples

---

**Built with ❤️ for the .NET community**

[![NuGet](https://img.shields.io/nuget/v/KBA.CoreUtilities.svg)](https://www.nuget.org/packages/KBA.CoreUtilities/)
[![Downloads](https://img.shields.io/nuget/dt/KBA.CoreUtilities.svg)](https://www.nuget.org/packages/KBA.CoreUtilities/)
