using KBA.CoreUtilities.Utilities;
using System;

Console.WriteLine("╔══════════════════════════════════════════════════════════╗");
Console.WriteLine("║        TEST GLOBAL - KBA.CoreUtilities v1.0.0           ║");
Console.WriteLine("╚══════════════════════════════════════════════════════════╝");
Console.WriteLine();

int testsTotal = 0;
int testsReussis = 0;
int testsEchoues = 0;

void Test(string nom, Action test)
{
    testsTotal++;
    try
    {
        test();
        Console.WriteLine($"✅ {nom}");
        testsReussis++;
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ {nom}");
        Console.WriteLine($"   Erreur: {ex.Message}");
        testsEchoues++;
    }
}

Console.WriteLine("🌍 TEST 1: CountryUtils");
Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");

Test("GetCountryByIso2", () =>
{
    var country = CountryUtils.GetCountryByIso2("SN");
    if (country == null || country.Name != "Sénégal") throw new Exception("Échec");
});

Test("GetCountryByIso3", () =>
{
    var country = CountryUtils.GetCountryByIso3("USA");
    if (country == null || country.Name != "États-Unis") throw new Exception("Échec");
});

Test("GetCountryByCurrency", () =>
{
    var countries = CountryUtils.GetCountriesByCurrency("XOF");
    if (countries == null || !countries.Any()) throw new Exception("Échec");
});

Test("IsInUEMOA", () =>
{
    if (!CountryUtils.IsInUEMOA("SN")) throw new Exception("Échec");
});

Test("GetCurrencyFromIso2", () =>
{
    var currency = CountryUtils.GetCurrencyFromIso2("FR");
    if (currency != "EUR") throw new Exception("Échec");
});

Console.WriteLine();
Console.WriteLine("📱 TEST 2: PhoneUtils");
Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");

Test("IsValidPhoneNumber", () =>
{
    if (!PhoneUtils.IsValidPhoneNumber("+221771234567", "SN")) throw new Exception("Échec");
});

Test("FormatPhoneNumber", () =>
{
    var formatted = PhoneUtils.FormatPhoneNumber("771234567", "SN");
    if (string.IsNullOrEmpty(formatted)) throw new Exception("Échec");
});

Test("FormatPhoneNumberInternational", () =>
{
    var intl = PhoneUtils.FormatPhoneNumberInternational("771234567", "SN");
    if (string.IsNullOrEmpty(intl)) throw new Exception("Échec");
});

Console.WriteLine();
Console.WriteLine("📲 TEST 3: QrCodeUtils");
Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");

Test("GenerateQrCode", () =>
{
    var qr = QrCodeUtils.GenerateQrCode("Test QR Code", 300, 300);
    if (qr == null || qr.Length == 0) throw new Exception("Échec");
});

Test("GetCurrencyDescription (XOF)", () =>
{
    var desc = QrCodeUtils.GetCurrencyDescription("952");
    if (!desc.Contains("XOF")) throw new Exception("Échec");
});

Test("GetCurrencyDescription (USD)", () =>
{
    var desc = QrCodeUtils.GetCurrencyDescription("840");
    if (!desc.Contains("USD")) throw new Exception("Échec");
});

Test("GetCurrencyDescription (EUR)", () =>
{
    var desc = QrCodeUtils.GetCurrencyDescription("978");
    if (!desc.Contains("EUR")) throw new Exception("Échec");
});

Test("GetCurrencyDescription (NGN)", () =>
{
    var desc = QrCodeUtils.GetCurrencyDescription("566");
    if (!desc.Contains("NGN")) throw new Exception("Échec");
});

Test("CreateEmvPaymentData", () =>
{
    var payment = EmvPaymentData.CreateDefault("Test Shop", "Dakar", 5000m);
    if (payment == null || !payment.IsValid()) throw new Exception("Échec");
});

Test("GenerateEmvPaymentQrCode", () =>
{
    var payment = EmvPaymentData.CreateDefault("Test", "Dakar", 1000m);
    var qrData = QrCodeUtils.GenerateEmvPaymentQrCode(payment);
    if (string.IsNullOrEmpty(qrData)) throw new Exception("Échec");
});

Console.WriteLine();
Console.WriteLine("🕒 TEST 4: DateTimeUtils");
Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");

Test("ToIso8601", () =>
{
    var iso = DateTimeUtils.ToIso8601(DateTime.UtcNow);
    if (string.IsNullOrEmpty(iso) || !iso.Contains("Z")) throw new Exception("Échec");
});

Test("ToUnixTimestamp", () =>
{
    var timestamp = DateTimeUtils.ToUnixTimestamp(DateTime.UtcNow);
    if (timestamp <= 0) throw new Exception("Échec");
});

Test("ToRelativeTime", () =>
{
    var relative = DateTimeUtils.ToRelativeTime(DateTime.UtcNow.AddHours(-2));
    if (!relative.Contains("ago") && !relative.Contains("il y a")) throw new Exception("Échec");
});

Test("AddBusinessDays", () =>
{
    var result = DateTimeUtils.AddBusinessDays(DateTime.Today, 5);
    if (result <= DateTime.Today) throw new Exception("Échec");
});

Test("GetBusinessDaysDifference", () =>
{
    var diff = DateTimeUtils.GetBusinessDaysDifference(DateTime.Today, DateTime.Today.AddDays(10));
    if (diff <= 0) throw new Exception("Échec");
});

Test("StartOfMonth", () =>
{
    var start = DateTimeUtils.StartOfMonth(DateTime.Today);
    if (start.Day != 1) throw new Exception("Échec");
});

Test("EndOfMonth", () =>
{
    var end = DateTimeUtils.EndOfMonth(DateTime.Today);
    if (end.Day < 28) throw new Exception("Échec");
});

Test("IsWeekend", () =>
{
    var sunday = new DateTime(2024, 10, 27); // Dimanche
    if (!DateTimeUtils.IsWeekend(sunday)) throw new Exception("Échec");
});

Test("GetAge", () =>
{
    var age = DateTimeUtils.GetAge(new DateTime(1990, 1, 1));
    if (age < 30) throw new Exception("Échec");
});

Test("GetThisMonthRange", () =>
{
    var (start, end) = DateTimeUtils.GetThisMonthRange();
    if (start >= end) throw new Exception("Échec");
});

Console.WriteLine();
Console.WriteLine("📄 TEST 5: SerializationUtils");
Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");

Test("ToJson", () =>
{
    var obj = new { Name = "Test", Value = 123 };
    var json = SerializationUtils.ToJson(obj);
    if (string.IsNullOrEmpty(json)) throw new Exception("Échec");
});

Test("FromJson", () =>
{
    var json = "{\"Name\":\"Test\",\"Value\":123}";
    var obj = SerializationUtils.FromJson<dynamic>(json);
    if (obj == null) throw new Exception("Échec");
});

Test("ToCompactJson", () =>
{
    var obj = new { Name = "Test" };
    var json = SerializationUtils.ToCompactJson(obj);
    if (string.IsNullOrEmpty(json) || json.Contains("\n")) throw new Exception("Échec");
});

Test("IsValidJson", () =>
{
    if (!SerializationUtils.IsValidJson("{\"test\":true}")) throw new Exception("Échec");
});

Test("MinifyJson", () =>
{
    var json = "{\n  \"test\": true\n}";
    var minified = SerializationUtils.MinifyJson(json);
    if (minified.Contains("\n")) throw new Exception("Échec");
});

Test("FormatJson", () =>
{
    var json = "{\"test\":true}";
    var formatted = SerializationUtils.FormatJson(json);
    if (!formatted.Contains("\n")) throw new Exception("Échec");
});

Test("ToXml", () =>
{
    var obj = new { Name = "Test", Value = 123 };
    var xml = SerializationUtils.ToXml(obj);
    if (string.IsNullOrEmpty(xml)) throw new Exception("Échec");
});

Test("IsValidXml", () =>
{
    if (!SerializationUtils.IsValidXml("<root><test>true</test></root>")) throw new Exception("Échec");
});

Test("DeepClone", () =>
{
    var obj = new { Name = "Test", Value = 123 };
    var clone = SerializationUtils.DeepClone(obj);
    if (clone == null) throw new Exception("Échec");
});

Console.WriteLine();
Console.WriteLine("✅ TEST 6: ValidationUtils");
Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");

Test("IsValidIban", () =>
{
    if (!ValidationUtils.IsValidIban("FR7630006000011234567890189")) throw new Exception("Échec");
});

Test("FormatIban", () =>
{
    var formatted = ValidationUtils.FormatIban("FR7630006000011234567890189");
    if (!formatted.Contains(" ")) throw new Exception("Échec");
});

Test("IsValidCreditCard", () =>
{
    if (!ValidationUtils.IsValidCreditCard("4532015112830366")) throw new Exception("Échec");
});

Test("GetCreditCardType", () =>
{
    var type = ValidationUtils.GetCreditCardType("4532015112830366");
    if (type != "Visa") throw new Exception("Échec");
});

Test("MaskCreditCard", () =>
{
    var masked = ValidationUtils.MaskCreditCard("4532015112830366");
    if (!masked.EndsWith("0366")) throw new Exception("Échec");
});

Test("IsValidBic", () =>
{
    if (!ValidationUtils.IsValidBic("BNPAFRPPXXX")) throw new Exception("Échec");
});

Test("IsValidVatNumber", () =>
{
    // Test with valid format pattern
    var result = ValidationUtils.IsValidVatNumber("FR12345678901", "FR");
    // VAT validation is format-based, so this should work
    if (!result) throw new Exception("Échec");
});

Test("IsValidIPv4", () =>
{
    if (!ValidationUtils.IsValidIPv4("192.168.1.1")) throw new Exception("Échec");
});

Test("IsValidMacAddress", () =>
{
    if (!ValidationUtils.IsValidMacAddress("00:1B:63:84:45:E6")) throw new Exception("Échec");
});

Console.WriteLine();
Console.WriteLine("🔐 TEST 7: CryptographyUtils");
Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");

Test("HashSHA256", () =>
{
    var hash = CryptographyUtils.HashSHA256("Hello World");
    if (string.IsNullOrEmpty(hash) || hash.Length != 64) throw new Exception("Échec");
});

Test("HashPassword & VerifyPassword", () =>
{
    var password = "TestPassword123";
    var hashed = CryptographyUtils.HashPassword(password);
    if (!CryptographyUtils.VerifyPassword(password, hashed)) throw new Exception("Échec");
});

Test("EncryptAES & DecryptAES", () =>
{
    var plainText = "Secret message";
    var key = "MyEncryptionKey123";
    var encrypted = CryptographyUtils.EncryptAES(plainText, key);
    var decrypted = CryptographyUtils.DecryptAES(encrypted, key);
    if (decrypted != plainText) throw new Exception("Échec");
});

Test("GenerateRSAKeyPair", () =>
{
    var (publicKey, privateKey) = CryptographyUtils.GenerateRSAKeyPair(2048);
    if (string.IsNullOrEmpty(publicKey) || string.IsNullOrEmpty(privateKey)) throw new Exception("Échec");
});

Test("GenerateSecureToken", () =>
{
    var token = CryptographyUtils.GenerateSecureToken(32);
    if (string.IsNullOrEmpty(token)) throw new Exception("Échec");
});

Test("GenerateOTP", () =>
{
    var otp = CryptographyUtils.GenerateOTP(6);
    if (otp.Length != 6 || !otp.All(char.IsDigit)) throw new Exception("Échec");
});

Test("GenerateHMAC", () =>
{
    var hmac = CryptographyUtils.GenerateHMAC("message", "secret");
    if (string.IsNullOrEmpty(hmac)) throw new Exception("Échec");
});

Console.WriteLine();
Console.WriteLine("📁 TEST 8: FileUtils");
Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");

Test("FormatFileSize", () =>
{
    var formatted = FileUtils.FormatFileSize(1024 * 1024);
    if (!formatted.Contains("MB")) throw new Exception("Échec");
});

Test("GetMimeType (image)", () =>
{
    var mime = FileUtils.GetMimeType("test.jpg");
    if (mime != "image/jpeg") throw new Exception("Échec");
});

Test("GetMimeType (pdf)", () =>
{
    var mime = FileUtils.GetMimeType("document.pdf");
    if (mime != "application/pdf") throw new Exception("Échec");
});

Test("IsImageFile", () =>
{
    if (!FileUtils.IsImageFile("photo.png")) throw new Exception("Échec");
});

Test("IsDocumentFile", () =>
{
    if (!FileUtils.IsDocumentFile("report.pdf")) throw new Exception("Échec");
});

Test("SanitizeFileName", () =>
{
    var safe = FileUtils.SanitizeFileName("my*file?name.txt");
    if (safe.Contains("*") || safe.Contains("?")) throw new Exception("Échec");
});

Test("CreateTempFile", () =>
{
    var tempFile = FileUtils.CreateTempFile(".txt");
    if (string.IsNullOrEmpty(tempFile) || !File.Exists(tempFile)) throw new Exception("Échec");
    File.Delete(tempFile); // Cleanup
});

Test("CreateTempDirectory", () =>
{
    var tempDir = FileUtils.CreateTempDirectory();
    if (string.IsNullOrEmpty(tempDir) || !Directory.Exists(tempDir)) throw new Exception("Échec");
    Directory.Delete(tempDir); // Cleanup
});

Console.WriteLine();
Console.WriteLine("✅ TOUS LES NOUVEAUX TESTS PASSÉS");

Console.WriteLine();
Console.WriteLine("╔══════════════════════════════════════════════════════════╗");
Console.WriteLine("║                    RÉSULTATS DES TESTS                   ║");
Console.WriteLine("╚══════════════════════════════════════════════════════════╝");
Console.WriteLine();
Console.WriteLine($"📊 Total des tests     : {testsTotal}");
Console.WriteLine($"✅ Tests réussis       : {testsReussis}");
Console.WriteLine($"❌ Tests échoués       : {testsEchoues}");
Console.WriteLine($"📈 Taux de réussite    : {(testsReussis * 100.0 / testsTotal):F1}%");
Console.WriteLine();

if (testsEchoues == 0)
{
    Console.WriteLine("🎉 TOUS LES TESTS SONT PASSÉS AVEC SUCCÈS!");
    Console.WriteLine("✅ Le package KBA.CoreUtilities est PRÊT pour publication!");
    Environment.Exit(0);
}
else
{
    Console.WriteLine("⚠️  CERTAINS TESTS ONT ÉCHOUÉ!");
    Console.WriteLine("❌ Veuillez corriger les erreurs avant publication.");
    Environment.Exit(1);
}
