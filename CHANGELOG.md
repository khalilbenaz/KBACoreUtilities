# Changelog

All notable changes to KBA.CoreUtilities will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [1.0.0] - 2025-10-27

### ✨ Initial Release

#### 🌍 Country Utilities
- Complete database of 200+ countries with ISO2, ISO3, phone codes
- Currency codes, capitals, and official languages for each country
- Regional groupings (UEMOA, CEMAC, Maghreb, etc.)
- Currency and date formatting per country
- Time zone information and conversion
- Language classification (Francophone, Anglophone, Lusophone, Arabophone)

#### 📱 Phone Utilities
- International phone number validation for 200+ countries
- Phone number formatting (E.164, international, national)
- Mobile number detection and validation
- Country code extraction from phone numbers
- Support for all international dialing codes

#### 💰 QR Code Utilities
- **EMVCo-compliant QR code generation** for worldwide payment systems
- Support for **all ISO 4217 currencies** (170+ currencies)
- GetCurrencyDescription with complete worldwide currency database
- Mobile money QR code generation (Orange Money, Wave, Free Money, etc.)
- Specialized QR codes (vCard, WiFi, SMS, Email, Location, Events)
- QR code reading and parsing
- EMV payment data validation
- Industry-specific payment templates (retail, restaurant, transportation, e-commerce)

#### 🕒 DateTime Utilities
- Comprehensive date/time formatting (ISO 8601, RFC 3339, Unix timestamps)
- Relative time formatting ("2 hours ago", "in 3 days")
- Business days calculations (add, count, list)
- Start/end of periods (day, week, month, quarter, year)
- Date range generators (today, this week, last month, etc.)
- Age calculations
- Weekend/weekday validation
- Time zone conversions
- Week number and quarter calculations
- Duration formatting and parsing
- Date rounding and truncation

#### 📄 Serialization Utilities
- Optimized JSON serialization/deserialization
- XML serialization/deserialization
- JSON ↔ XML conversion
- JSON validation, minification, formatting, and pretty-printing
- XML validation against XSD schemas
- File operations (async/sync)
- Stream operations for large files
- Batch serialization
- Deep cloning
- JSON merging and comparison
- Property extraction by path

#### 🌐 API Utilities
- REST API consumption (GET, POST, PUT, DELETE)
- GraphQL support
- Bearer token authentication
- Basic authentication
- Custom headers management
- Query string builder
- Retry policies with exponential backoff
- File download and upload
- Multipart form data support
- SOAP web service client
- WSDL service builder

#### 🔢 Decimal Utilities
- Precise rounding for financial calculations
- Percentage calculations
- Discount applications
- Decimal comparison with tolerance
- Currency-aware operations

#### 📝 String Utilities
- String truncation with ellipsis
- Accent/diacritic removal
- Slug generation
- Case conversions (camelCase, PascalCase, snake_case, kebab-case)
- Capitalization and title case
- Validation (email, URL, numeric, alpha, alphanumeric)

#### 📊 Logging Utilities
- Structured logging helpers
- Logger factory methods
- Context-aware logging

### 🎯 Key Features

- ✅ **Production-ready**: Battle-tested in financial services
- ✅ **EMV Compliant**: Full EMVCo QR code specification support
- ✅ **Worldwide Support**: All countries, currencies, and payment systems
- ✅ **Type-safe**: Strongly typed with full IntelliSense support
- ✅ **Async-first**: Modern async/await patterns throughout
- ✅ **Well-documented**: Comprehensive XML documentation and examples
- ✅ **High Performance**: Optimized for speed and memory efficiency
- ✅ **Zero breaking changes**: Stable public API

### 📦 Dependencies

- .NET 8.0+
- System.Text.Json 8.0.5
- System.ComponentModel.Annotations 5.0.0
- Microsoft.Extensions.Logging.Abstractions 8.0.0
- System.IdentityModel.Tokens.Jwt 7.1.2
- QRCoder 1.6.0
- ZXing.Net 0.16.9
- System.Drawing.Common 8.0.0
- System.ServiceModel.Http 8.0.0

### 🌟 Highlights for African FinTech

- Full support for UEMOA (XOF) and CEMAC (XAF) currencies
- Mobile money QR code generation for all African operators
- Phone validation for all African countries
- Regional grouping and filtering
- Time zone support for African regions
- Francophone, Anglophone, and Lusophone country classification

---

## [Unreleased]

### Planned Features for Future Versions

#### 1.1.0 (Planned)
- [ ] Cryptocurrency support in QR codes
- [ ] Enhanced phone number parsing with carrier detection
- [ ] Additional date/time localization options
- [ ] Performance profiling and optimization tools
- [ ] Extended SOAP authentication methods

#### 1.2.0 (Planned)
- [ ] Bank account validation (IBAN, SWIFT)
- [ ] Card number validation (Luhn algorithm)
- [ ] CVV validation
- [ ] BIN/IIN lookup for card identification
- [ ] Additional barcode formats (Code128, EAN13, etc.)

#### 2.0.0 (Future Major Release)
- [ ] .NET 9 support
- [ ] Source generators for improved performance
- [ ] Minimal API extensions
- [ ] Blazor components for QR code display
- [ ] Real-time exchange rate integration

---

## Version History

| Version | Release Date | Description |
|---------|--------------|-------------|
| 1.0.0   | 2025-10-27   | Initial release with complete utilities suite |

---

## Migration Guide

### From No Previous Version (New Installation)

Simply install the package:

```bash
dotnet add package KBA.CoreUtilities
```

And start using:

```csharp
using KBA.CoreUtilities.Utilities;
```

---

## Breaking Changes

None in version 1.0.0 (initial release).

Future breaking changes will be clearly documented here with migration paths.

---

## Deprecations

None in version 1.0.0 (initial release).

---

## Contributors

- KBA Team - Initial development and maintenance

---

## Support

For issues, questions, or contributions:
- GitHub Issues: https://github.com/kba/core-utilities/issues
- Email: support@kba.com
- Documentation: https://github.com/kba/core-utilities/wiki

---

Made with ❤️ for the FinTech community
