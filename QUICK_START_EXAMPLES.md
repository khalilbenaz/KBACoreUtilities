# 🚀 Quick Start Examples - KBA.CoreUtilities

This guide provides ready-to-use code snippets for common use cases.

## 📋 Table of Contents

1. [Mobile Money Payment QR](#1-mobile-money-payment-qr)
2. [Multi-Country Phone Validation](#2-multi-country-phone-validation)
3. [International Payment System](#3-international-payment-system)
4. [Date Range Reporting](#4-date-range-reporting)
5. [API Integration with Retry](#5-api-integration-with-retry)
6. [Data Serialization Pipeline](#6-data-serialization-pipeline)
7. [Country Information Display](#7-country-information-display)
8. [EMV QR Code Reader](#8-emv-qr-code-reader)
9. [Business Days Calculator](#9-business-days-calculator)
10. [Multi-Format Serialization](#10-multi-format-serialization)

---

## 1. Mobile Money Payment QR

Generate a QR code for mobile money payment in Senegal.

```csharp
using KBA.CoreUtilities.Utilities;
using System.IO;

// Create EMV payment data for mobile money
var payment = EmvPaymentData.CreateMobileMoney(
    merchantName: "Restaurant Teranga",
    merchantCity: "Dakar",
    amount: 15000m,
    phoneNumber: "771234567",
    provider: "Orange Money",
    countryCode: "SN",
    currencyCode: "952" // XOF - West African CFA Franc
);

// Generate QR code image
byte[] qrCodeImage = QrCodeUtils.GenerateEmvPaymentQrCodeImage(
    payment, 
    width: 400, 
    height: 400
);

// Save to file
File.WriteAllBytes("payment_qr.png", qrCodeImage);

// Display info
Console.WriteLine($"Merchant: {payment.MerchantName}");
Console.WriteLine($"Amount: {payment.Amount} XOF");
Console.WriteLine($"Currency: {QrCodeUtils.GetCurrencyDescription("952")}");
Console.WriteLine($"QR Code saved to payment_qr.png");
```

**Output:**
```
Merchant: Restaurant Teranga
Amount: 15000 XOF
Currency: West African CFA Franc (XOF)
QR Code saved to payment_qr.png
```

---

## 2. Multi-Country Phone Validation

Validate and format phone numbers from multiple countries.

```csharp
using KBA.CoreUtilities.Utilities;

var phoneNumbers = new[]
{
    (Phone: "+221771234567", Country: "SN", Name: "Senegal"),
    (Phone: "+33612345678", Country: "FR", Name: "France"),
    (Phone: "+2349012345678", Country: "NG", Name: "Nigeria"),
    (Phone: "+1-650-555-1234", Country: "US", Name: "USA"),
    (Phone: "+971501234567", Country: "AE", Name: "UAE")
};

Console.WriteLine("📱 International Phone Validation\n");
Console.WriteLine($"{"Country",-15} {"Original",-20} {"Valid?",-8} {"Formatted",-25} {"E.164",-20}");
Console.WriteLine(new string('-', 90));

foreach (var (phone, country, name) in phoneNumbers)
{
    bool isValid = PhoneUtils.IsValidPhoneNumber(phone, country);
    string formatted = isValid ? PhoneUtils.FormatPhoneNumber(phone, country) : "Invalid";
    string e164 = isValid ? PhoneUtils.FormatE164(phone, country) : "N/A";
    
    Console.WriteLine($"{name,-15} {phone,-20} {(isValid ? "✅" : "❌"),-8} {formatted,-25} {e164,-20}");
}
```

**Output:**
```
📱 International Phone Validation

Country         Original             Valid?   Formatted                 E.164               
------------------------------------------------------------------------------------------
Senegal         +221771234567        ✅       +221 77 123 45 67         +221771234567       
France          +33612345678         ✅       +33 6 12 34 56 78         +33612345678        
Nigeria         +2349012345678       ✅       +234 901 234 5678         +2349012345678      
USA             +1-650-555-1234      ✅       +1 650-555-1234           +16505551234        
UAE             +971501234567        ✅       +971 50 123 4567          +971501234567       
```

---

## 3. International Payment System

Generate payment QR codes for different countries.

```csharp
using KBA.CoreUtilities.Utilities;

var payments = new[]
{
    (Merchant: "Boutique Dakar", City: "Dakar", Amount: 5000m, Country: "SN", Currency: "952"), // XOF
    (Merchant: "Shop Paris", City: "Paris", Amount: 50m, Country: "FR", Currency: "978"),      // EUR
    (Merchant: "Market Lagos", City: "Lagos", Amount: 10000m, Country: "NG", Currency: "566"), // NGN
    (Merchant: "Store NY", City: "New York", Amount: 25m, Country: "US", Currency: "840"),     // USD
    (Merchant: "Mall Dubai", City: "Dubai", Amount: 200m, Country: "AE", Currency: "784")      // AED
};

Console.WriteLine("💰 International Payment QR Codes\n");

foreach (var (merchant, city, amount, country, currency) in payments)
{
    var payment = EmvPaymentData.CreateForCountry(
        merchantName: merchant,
        merchantCity: city,
        amount: amount,
        countryCode: country,
        currencyCode: currency
    );
    
    byte[] qr = QrCodeUtils.GenerateEmvPaymentQrCodeImage(payment, 300, 300);
    string filename = $"qr_{country.ToLower()}.png";
    File.WriteAllBytes(filename, qr);
    
    string currencyDesc = QrCodeUtils.GetCurrencyDescription(currency);
    var countryInfo = CountryUtils.GetCountryByIso2(country);
    
    Console.WriteLine($"🏪 {merchant} ({city})");
    Console.WriteLine($"   Country: {countryInfo.Name}");
    Console.WriteLine($"   Amount: {amount:N2} {currencyDesc}");
    Console.WriteLine($"   QR Code: {filename} ({qr.Length / 1024}KB)");
    Console.WriteLine();
}
```

---

## 4. Date Range Reporting

Generate reports with business day calculations.

```csharp
using KBA.CoreUtilities.Utilities;

// Get common date ranges
var ranges = new[]
{
    ("Today", DateTimeUtils.GetTodayRange()),
    ("This Week", DateTimeUtils.GetThisWeekRange()),
    ("This Month", DateTimeUtils.GetThisMonthRange()),
    ("Last 30 Days", DateTimeUtils.GetLastNDaysRange(30)),
    ("This Quarter", (DateTimeUtils.StartOfQuarter(DateTime.Today), DateTimeUtils.EndOfQuarter(DateTime.Today))),
    ("This Year", DateTimeUtils.GetThisYearRange())
};

Console.WriteLine("📊 Date Range Report\n");
Console.WriteLine($"Generated: {DateTime.Now:yyyy-MM-dd HH:mm:ss}\n");

foreach (var (name, (start, end)) in ranges)
{
    int totalDays = (int)(end - start).TotalDays + 1;
    int businessDays = DateTimeUtils.GetBusinessDaysDifference(start, end);
    int weekendDays = totalDays - businessDays;
    
    Console.WriteLine($"📅 {name}");
    Console.WriteLine($"   Period: {start:MMM dd} - {end:MMM dd, yyyy}");
    Console.WriteLine($"   Total Days: {totalDays}");
    Console.WriteLine($"   Business Days: {businessDays}");
    Console.WriteLine($"   Weekend Days: {weekendDays}");
    Console.WriteLine();
}

// Calculate project deadline
Console.WriteLine("🎯 Project Deadline Calculator");
DateTime projectStart = DateTime.Today;
int requiredBusinessDays = 20;
DateTime deadline = DateTimeUtils.AddBusinessDays(projectStart, requiredBusinessDays);

Console.WriteLine($"   Start Date: {projectStart:yyyy-MM-dd}");
Console.WriteLine($"   Business Days Required: {requiredBusinessDays}");
Console.WriteLine($"   Deadline: {deadline:yyyy-MM-dd} ({DateTimeUtils.ToRelativeTime(deadline)})");
```

---

## 5. API Integration with Retry

Consume an API with automatic retry on failure.

```csharp
using KBA.CoreUtilities.Utilities;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
}

// Create authenticated client
string apiToken = "your_bearer_token";
var client = ApiUtils.CreateAuthenticatedHttpClient(apiToken);

// Fetch data with retry policy
try
{
    var user = await ApiUtils.ExecuteWithRetryAsync(
        async () => await ApiUtils.GetJsonAsync<User>(
            "https://api.example.com/users/1", 
            client
        ),
        maxRetries: 3,
        delayMs: 1000
    );
    
    Console.WriteLine($"✅ User fetched: {user.Name} ({user.Email})");
    
    // Save to JSON file
    await SerializationUtils.ToJsonFileAsync(user, "user.json");
    Console.WriteLine("💾 Saved to user.json");
}
catch (Exception ex)
{
    Console.WriteLine($"❌ Failed after retries: {ex.Message}");
}

// POST example with custom headers
var headers = new Dictionary<string, string>
{
    ["X-API-Key"] = "your_api_key",
    ["X-Request-ID"] = Guid.NewGuid().ToString()
};

var customClient = ApiUtils.CreateHttpClient(headers, timeout: TimeSpan.FromSeconds(30));

var newUser = new User { Name = "John Doe", Email = "john@example.com" };
var response = await ApiUtils.PostJsonAsync<User, User>(
    "https://api.example.com/users",
    newUser,
    customClient
);

Console.WriteLine($"✅ User created with ID: {response.Id}");
```

---

## 6. Data Serialization Pipeline

Process data through multiple serialization formats.

```csharp
using KBA.CoreUtilities.Utilities;

public class Transaction
{
    public string Id { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; }
    public DateTime Date { get; set; }
    public string Description { get; set; }
}

var transaction = new Transaction
{
    Id = "TXN-001",
    Amount = 15000m,
    Currency = "XOF",
    Date = DateTime.Now,
    Description = "Mobile money transfer"
};

Console.WriteLine("🔄 Serialization Pipeline\n");

// 1. Serialize to JSON
string json = SerializationUtils.ToJson(transaction);
Console.WriteLine("1️⃣ JSON:");
Console.WriteLine(json);
Console.WriteLine();

// 2. Convert to XML
string xml = SerializationUtils.ToXml(transaction);
Console.WriteLine("2️⃣ XML:");
Console.WriteLine(xml);
Console.WriteLine();

// 3. Minify JSON
string minified = SerializationUtils.MinifyJson(json);
Console.WriteLine("3️⃣ Minified JSON:");
Console.WriteLine(minified);
Console.WriteLine($"   Size reduction: {json.Length} → {minified.Length} bytes");
Console.WriteLine();

// 4. Convert JSON to XML dynamically
string jsonToXml = SerializationUtils.JsonToXml(json, "Transaction");
Console.WriteLine("4️⃣ JSON → XML Conversion:");
Console.WriteLine(jsonToXml);
Console.WriteLine();

// 5. Deep clone
var clone = SerializationUtils.DeepClone(transaction);
clone.Amount = 20000m;
Console.WriteLine("5️⃣ Deep Clone:");
Console.WriteLine($"   Original: {transaction.Amount} XOF");
Console.WriteLine($"   Clone: {clone.Amount} XOF");
Console.WriteLine();

// 6. Validate and compare
bool isValidJson = SerializationUtils.IsValidJson(json);
bool areEqual = SerializationUtils.AreJsonEqual(json, SerializationUtils.ToJson(transaction));
Console.WriteLine("6️⃣ Validation:");
Console.WriteLine($"   Is Valid JSON: {(isValidJson ? "✅" : "❌")}");
Console.WriteLine($"   JSON Equality: {(areEqual ? "✅" : "❌")}");
```

---

## 7. Country Information Display

Display comprehensive country information.

```csharp
using KBA.CoreUtilities.Utilities;

var countries = new[] { "SN", "FR", "NG", "US", "MA", "AE", "BR", "CN" };

Console.WriteLine("🌍 Country Information Dashboard\n");
Console.WriteLine($"{"Code",-6} {"Country",-30} {"Capital",-20} {"Currency",-10} {"Phone",-8} {"Region",-20}");
Console.WriteLine(new string('-', 100));

foreach (var code in countries)
{
    var country = CountryUtils.GetCountryByIso2(code);
    string region = CountryUtils.GetRegion(code);
    
    Console.WriteLine($"{code,-6} {country.Name,-30} {country.Capital,-20} {country.CurrencyCode,-10} +{country.PhoneCode,-7} {region,-20}");
}

Console.WriteLine("\n📊 Regional Statistics\n");

// UEMOA countries
var uemoaCountries = CountryUtils.GetCountriesByCurrency("XOF");
Console.WriteLine($"UEMOA (XOF): {uemoaCountries.Count()} countries");
foreach (var c in uemoaCountries)
{
    Console.WriteLine($"  • {c.Name}");
}

Console.WriteLine();

// Time zones for selected countries
Console.WriteLine("🕐 Time Zones:");
foreach (var code in new[] { "SN", "FR", "US", "CN" })
{
    var country = CountryUtils.GetCountryByIso2(code);
    string tz = CountryUtils.GetTimeZone(code);
    Console.WriteLine($"  {country.Name}: {tz}");
}

Console.WriteLine();

// Language groups
Console.WriteLine("🗣️ Language Groups:");
Console.WriteLine($"  Francophone countries: {countries.Count(c => CountryUtils.IsFrancophone(c))}");
Console.WriteLine($"  Anglophone countries: {countries.Count(c => CountryUtils.IsAnglophone(c))}");
```

---

## 8. EMV QR Code Reader

Read and parse EMV QR codes.

```csharp
using KBA.CoreUtilities.Utilities;

// Generate a test QR code
var payment = EmvPaymentData.CreateDefault("Test Merchant", "Dakar", 5000m);
byte[] qrImage = QrCodeUtils.GenerateEmvPaymentQrCodeImage(payment);

// Save it
File.WriteAllBytes("test_qr.png", qrImage);

// Read it back
string qrContent = QrCodeUtils.ReadQrCodeFromFile("test_qr.png");

if (!string.IsNullOrEmpty(qrContent))
{
    Console.WriteLine("✅ QR Code Read Successfully\n");
    Console.WriteLine($"Raw Content: {qrContent.Substring(0, Math.Min(50, qrContent.Length))}...\n");
    
    // Validate EMV format
    bool isValid = QrCodeUtils.ValidateEmvQrCode(qrContent);
    Console.WriteLine($"EMV Valid: {(isValid ? "✅" : "❌")}\n");
    
    // Parse EMV data
    var parsed = QrCodeUtils.ParseEmvPaymentQrCode(qrContent);
    
    Console.WriteLine("📋 Parsed Payment Information:");
    Console.WriteLine($"  Merchant: {parsed.MerchantName}");
    Console.WriteLine($"  City: {parsed.MerchantCity}");
    Console.WriteLine($"  Amount: {parsed.Amount}");
    Console.WriteLine($"  Currency: {QrCodeUtils.GetCurrencyDescription(parsed.CurrencyCode)}");
    Console.WriteLine($"  Country: {parsed.CountryCode}");
    Console.WriteLine($"  Category: {QrCodeUtils.GetMerchantCategoryDescription(parsed.MerchantCategoryCode)}");
}
else
{
    Console.WriteLine("❌ Failed to read QR code");
}
```

---

## 9. Business Days Calculator

Calculate business days for project planning.

```csharp
using KBA.CoreUtilities.Utilities;

Console.WriteLine("📅 Business Days Calculator\n");

// Scenario 1: Calculate delivery date
DateTime orderDate = new DateTime(2024, 10, 27); // Sunday
int deliveryDays = 5;
DateTime deliveryDate = DateTimeUtils.AddBusinessDays(orderDate, deliveryDays);

Console.WriteLine("🚚 Delivery Date Calculation:");
Console.WriteLine($"  Order Date: {orderDate:ddd, MMM dd, yyyy}");
Console.WriteLine($"  Business Days: {deliveryDays}");
Console.WriteLine($"  Delivery Date: {deliveryDate:ddd, MMM dd, yyyy}");
Console.WriteLine($"  Time until delivery: {DateTimeUtils.ToRelativeTime(deliveryDate)}");
Console.WriteLine();

// Scenario 2: Calculate working days between dates
DateTime projectStart = new DateTime(2024, 11, 1);
DateTime projectEnd = new DateTime(2024, 11, 30);
int workingDays = DateTimeUtils.GetBusinessDaysDifference(projectStart, projectEnd);

Console.WriteLine("📊 Project Duration:");
Console.WriteLine($"  Start: {projectStart:MMM dd, yyyy}");
Console.WriteLine($"  End: {projectEnd:MMM dd, yyyy}");
Console.WriteLine($"  Total Working Days: {workingDays}");
Console.WriteLine();

// Scenario 3: List all business days in a period
Console.WriteLine("📆 Business Days this Week:");
var (weekStart, weekEnd) = DateTimeUtils.GetThisWeekRange();
var businessDays = DateTimeUtils.GetBusinessDaysBetween(weekStart, weekEnd).Take(5);

foreach (var day in businessDays)
{
    string marker = DateTimeUtils.IsToday(day) ? "👉" : "  ";
    Console.WriteLine($"{marker} {day:ddd, MMM dd}");
}
```

---

## 10. Multi-Format Serialization

Batch process and serialize multiple objects.

```csharp
using KBA.CoreUtilities.Utilities;

public class Product
{
    public string Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Currency { get; set; }
}

var products = new List<Product>
{
    new() { Id = "P001", Name = "Phone", Price = 250000m, Currency = "XOF" },
    new() { Id = "P002", Name = "Laptop", Price = 500000m, Currency = "XOF" },
    new() { Id = "P003", Name = "Tablet", Price = 150000m, Currency = "XOF" }
};

Console.WriteLine("💾 Multi-Format Serialization\n");

// 1. Batch serialize to JSON
await SerializationUtils.BatchSerializeToJsonAsync(products, "products.json");
Console.WriteLine("✅ Saved to products.json");

// 2. Load and stream (memory-efficient for large files)
Console.WriteLine("\n📖 Streaming products:");
foreach (var product in SerializationUtils.StreamJsonArray<Product>("products.json"))
{
    Console.WriteLine($"  • {product.Name}: {product.Price:N0} {product.Currency}");
}

// 3. Get file size comparison
foreach (var product in products)
{
    long jsonSize = SerializationUtils.GetJsonSize(product);
    byte[] xmlBytes = System.Text.Encoding.UTF8.GetBytes(SerializationUtils.ToXml(product));
    
    Console.WriteLine($"\n{product.Name}:");
    Console.WriteLine($"  JSON: {jsonSize} bytes");
    Console.WriteLine($"  XML: {xmlBytes.Length} bytes");
    Console.WriteLine($"  Difference: {xmlBytes.Length - jsonSize} bytes");
}
```

---

## 🎯 Complete Use Case: Payment Processing System

Here's a complete example combining multiple utilities:

```csharp
using KBA.CoreUtilities.Utilities;

public class PaymentProcessor
{
    public async Task ProcessInternationalPayment(
        string phoneNumber, 
        string countryCode, 
        decimal amount,
        string description)
    {
        Console.WriteLine("🔄 Processing International Payment\n");
        
        // 1. Validate phone number
        if (!PhoneUtils.IsValidPhoneNumber(phoneNumber, countryCode))
        {
            Console.WriteLine("❌ Invalid phone number");
            return;
        }
        
        string formattedPhone = PhoneUtils.FormatPhoneNumber(phoneNumber, countryCode);
        Console.WriteLine($"✅ Phone validated: {formattedPhone}");
        
        // 2. Get country and currency information
        var country = CountryUtils.GetCountryByIso2(countryCode);
        string currencyCode = country.CurrencyCode;
        
        // Get ISO 4217 numeric code
        var currencyNumeric = GetCurrencyNumericCode(currencyCode);
        
        Console.WriteLine($"✅ Country: {country.Name}");
        Console.WriteLine($"✅ Currency: {currencyCode}");
        
        // 3. Create payment record
        var payment = new
        {
            TransactionId = Guid.NewGuid().ToString(),
            Phone = formattedPhone,
            Country = country.Name,
            CountryCode = countryCode,
            Amount = amount,
            Currency = currencyCode,
            Description = description,
            Timestamp = DateTimeUtils.ToIso8601(DateTime.UtcNow),
            RelativeTime = DateTimeUtils.ToRelativeTime(DateTime.UtcNow)
        };
        
        // 4. Save transaction
        string json = SerializationUtils.ToJson(payment);
        await File.WriteAllTextAsync($"payment_{payment.TransactionId}.json", json);
        Console.WriteLine($"✅ Transaction saved: {payment.TransactionId}");
        
        // 5. Generate QR code
        var emvPayment = EmvPaymentData.CreateForCountry(
            merchantName: "International Payment",
            merchantCity: country.Capital,
            amount: amount,
            countryCode: countryCode,
            currencyCode: currencyNumeric
        );
        
        byte[] qr = QrCodeUtils.GenerateEmvPaymentQrCodeImage(emvPayment, 400, 400);
        await File.WriteAllBytesAsync($"qr_{payment.TransactionId}.png", qr);
        Console.WriteLine($"✅ QR Code generated ({qr.Length / 1024}KB)");
        
        Console.WriteLine("\n✅ Payment processed successfully!");
    }
    
    private string GetCurrencyNumericCode(string currencyCode)
    {
        // Map common currencies to ISO 4217 numeric codes
        return currencyCode switch
        {
            "XOF" => "952",
            "XAF" => "950",
            "EUR" => "978",
            "USD" => "840",
            "GBP" => "826",
            "NGN" => "566",
            _ => "952" // Default to XOF
        };
    }
}

// Usage
var processor = new PaymentProcessor();
await processor.ProcessInternationalPayment(
    phoneNumber: "+221771234567",
    countryCode: "SN",
    amount: 25000m,
    description: "Mobile money transfer"
);
```

---

## 📚 Additional Resources

- **Full Documentation**: See [README.md](README.md)
- **API Reference**: Check XML documentation in your IDE
- **Publishing Guide**: See [NUGET_PUBLICATION_GUIDE.md](NUGET_PUBLICATION_GUIDE.md)
- **Changelog**: See [CHANGELOG.md](CHANGELOG.md)

---

Made with ❤️ for developers worldwide
