# 🎉 VERSION 1.2.0 - EXTENSIONS & MULTI-TARGETING PUBLIÉS!

## ✅ Publication Réussie sur NuGet.org

**Date:** 27 octobre 2025  
**Version:** 1.2.0  
**Statut:** ✅ **PUBLIÉ ET DISPONIBLE**

---

## 🚀 NOUVEAUTÉS MAJEURES v1.2.0

### 1. 🎯 MULTI-TARGETING (.NET 6.0, 7.0, 8.0)

Le package supporte maintenant **3 versions de .NET** pour une compatibilité maximale:

- ✅ **.NET 6.0** (LTS - Support jusqu'à novembre 2024)
- ✅ **.NET 7.0** (Standard)
- ✅ **.NET 8.0** (LTS - Support jusqu'à novembre 2026)

**Installation adaptative automatique** selon votre projet!

---

### 2. 📝 STRING EXTENSIONS (50+ Méthodes)

Nouveau namespace: `KBA.CoreUtilities.Extensions`

#### Validation
```csharp
using KBA.CoreUtilities.Extensions;

"user@example.com".IsEmail();           // true
"192.168.1.1".IsValidIPv4();            // true
"12345".IsNumeric();                     // true
"abc123".IsAlphanumeric();               // true
```

#### Manipulation
```csharp
"Hello World".Truncate(5);               // "Hello..."
"hello".Capitalize();                    // "Hello"
"prefix_text".RemovePrefix("prefix_");   // "text"
"text_suffix".RemoveSuffix("_suffix");   // "text"
"hello".Reverse();                       // "olleh"
"test test test".CountOccurrences("test"); // 3
```

#### Conversions
```csharp
"123".ToInt();                           // 123
"123.45".ToDecimal();                    // 123.45m
"true".ToBool();                         // true
"Active".ToEnum<Status>();               // Status.Active
"Hello".ToBase64();                      // "SGVsbG8="
```

#### Hashing
```csharp
"password".ToSHA256();                   // Hash SHA256
"data".ToSHA512();                       // Hash SHA512
"content".ToMD5();                       // Hash MD5
```

#### Formatage
```csharp
"hello-world".ToCamelCase();            // "helloWorld"
"hello world".ToPascalCase();           // "HelloWorld"
"HelloWorld".ToSnakeCase();             // "hello_world"
"HelloWorld".ToKebabCase();             // "hello-world"
"Hôtel".RemoveDiacritics();             // "Hotel"
"4532015112830366".Mask(4);             // "************0366"
```

#### Null Handling
```csharp
string? text = null;
text.GetEmptyStringIfNull();            // ""
text.GetDefaultIfEmpty("default");      // "default"
```

---

### 3. 📚 COLLECTION EXTENSIONS (40+ Méthodes)

#### Null & Empty Checks
```csharp
using KBA.CoreUtilities.Extensions;

List<int> list = null;
list.IsNullOrEmpty();                   // true
list.OrEmpty();                         // Empty collection
```

#### ForEach & Actions
```csharp
items.ForEach(item => Console.WriteLine(item));
items.ForEach((item, index) => Console.WriteLine($"{index}: {item}"));
```

#### Chunking & Batching
```csharp
var numbers = Enumerable.Range(1, 100);

// Découper en morceaux de 10
numbers.Chunk(10);                      // 10 chunks de 10 items

// Découper en 5 batches
numbers.Batch(5);                       // 5 batches de 20 items
```

#### Shuffle & Random
```csharp
var shuffled = items.Shuffle();         // Mélange aléatoire
var randomItem = items.Random();        // Item aléatoire
var randomItems = items.RandomElements(5); // 5 items aléatoires
```

#### String Operations
```csharp
var numbers = new[] { 1, 2, 3, 4, 5 };
numbers.JoinString(", ");               // "1, 2, 3, 4, 5"
```

#### Filtering
```csharp
items.WhereNotNull();                   // Filtre les nulls
items.DistinctBy(x => x.Id);           // Distinct par propriété
```

#### Min/Max
```csharp
people.MinBy(p => p.Age);              // Personne la plus jeune
people.MaxBy(p => p.Salary);           // Personne au salaire le plus élevé
```

#### Paging
```csharp
items.Page(pageNumber: 1, pageSize: 20); // Page 1, 20 items
```

#### Conversion
```csharp
items.ToHashSet();                      // Vers HashSet
items.ToQueue();                        // Vers Queue
items.ToStack();                        // Vers Stack
```

#### Contains
```csharp
items.ContainsAny(1, 2, 3);            // true si contient au moins un
items.ContainsAll(1, 2, 3);            // true si contient tous
```

---

### 4. 🎯 OBJECT EXTENSIONS

#### Null Handling
```csharp
using KBA.CoreUtilities.Extensions;

object obj = null;
obj.IsNull();                           // true
obj.IfNull("default");                  // "default"
obj.ThrowIfNull("paramName");           // ArgumentNullException

string result = obj.IfNotNull(o => o.ToString(), "default");
```

#### Type Conversions
```csharp
object value = "123";
value.ConvertTo<int>();                 // 123
value.As<string>();                     // "123" (cast safe)
value.Is<string>();                     // true
```

#### JSON Conversions
```csharp
var obj = new { Name = "Test", Age = 30 };
string json = obj.ToJson();             // Sérialisation
var restored = json.FromJson<MyType>(); // Désérialisation

var dict = json.JsonToDictionary();     // Vers Dictionary
```

#### Cloning
```csharp
var clone = obj.DeepClone();            // Clone profond
```

#### Range & Between
```csharp
5.IsBetween(1, 10);                     // true
15.Clamp(1, 10);                        // 10
```

#### In & NotIn
```csharp
value.In(1, 2, 3, 4, 5);               // true si dans la liste
value.NotIn(6, 7, 8);                  // true si pas dans la liste
```

#### Exception Handling
```csharp
var result = obj.Try(
    o => o.RiskyOperation(),
    onError: ex => Log.Error(ex)
);
```

#### Query String
```csharp
var obj = new { Name = "Test", Page = 1 };
string qs = obj.ToQueryString();        // "Name=Test&Page=1"

var dict = "?name=Test&page=1".QueryStringToDictionary();
```

---

## 📦 Package Multi-Target

### Support de Versions
```xml
<TargetFrameworks>net6.0;net7.0;net8.0</TargetFrameworks>
```

### Installation Automatique
Le package s'adapte automatiquement à votre projet:

```bash
# Pour projet .NET 6
dotnet add package KBA.CoreUtilities  # Utilise net6.0

# Pour projet .NET 7
dotnet add package KBA.CoreUtilities  # Utilise net7.0

# Pour projet .NET 8
dotnet add package KBA.CoreUtilities  # Utilise net8.0
```

### Dépendances Conditionnelles
Le package utilise des versions appropriées des dépendances pour chaque framework:

- **.NET 6**: `System.Text.Json 6.0.10`, `Microsoft.Extensions.* 6.0.0`
- **.NET 7**: `System.Text.Json 7.0.4`, `Microsoft.Extensions.* 7.0.0`
- **.NET 8**: `System.Text.Json 8.0.5`, `Microsoft.Extensions.* 8.0.0`

---

## 📊 Statistiques v1.2.0

### Nouveaux Modules
- ✅ **StringExtensions**: 50+ méthodes d'extension
- ✅ **CollectionExtensions**: 40+ méthodes d'extension
- ✅ **ObjectExtensions**: 30+ méthodes d'extension

### Total Extensions: **120+ méthodes**

### Multi-Targeting
- ✅ Support .NET 6.0, 7.0, 8.0
- ✅ Dépendances conditionnelles
- ✅ Compatibilité maximale

---

## 🎯 Exemples Concrets

### Exemple 1: Validation & Formatage de Données

```csharp
using KBA.CoreUtilities.Extensions;

// Validation email
if (email.IsEmail())
{
    // Nettoyer et formater
    var formatted = email.Trim().ToLowerInvariant();
    
    // Hasher pour stockage
    var hash = formatted.ToSHA256();
}

// Validation IP
if (ipAddress.IsValidIPv4())
{
    // Traiter l'adresse IP
}
```

### Exemple 2: Manipulation de Collections

```csharp
// Paginer des résultats
var users = database.GetAllUsers();
var page1 = users.Page(pageNumber: 1, pageSize: 20);

// Grouper par batches pour traitement
users.Chunk(100).ForEach(batch => 
{
    ProcessBatch(batch);
});

// Trouver l'utilisateur le plus âgé
var oldest = users.MaxBy(u => u.Age);
```

### Exemple 3: Conversions Sûres

```csharp
// Convertir avec valeur par défaut
string input = "123abc";
int number = input.ToInt(defaultValue: 0);  // 0

// Conversion d'enum
string status = "Active";
var statusEnum = status.ToEnum<UserStatus>(UserStatus.Unknown);

// JSON
var obj = jsonString.FromJson<MyObject>();
if (obj != null)
{
    // Traiter
}
```

### Exemple 4: Null Safety

```csharp
// Chaîner des opérations nullables
string result = user
    .IfNotNull(u => u.Profile)
    .IfNotNull(p => p.DisplayName)
    .GetDefaultIfEmpty("Anonymous");

// Validation avec exceptions
var config = configValue
    .ThrowIfNull("config")
    .ThrowIf(c => string.IsNullOrEmpty(c.ApiKey), "API key required");
```

---

## 📈 Croissance du Package

| Métrique | v1.1.0 | v1.2.0 | Évolution |
|----------|--------|--------|-----------|
| Modules | 13 | 16 | **+23%** |
| Méthodes | ~280 | ~400 | **+43%** |
| Frameworks | 1 (.NET 8) | 3 (.NET 6/7/8) | **+200%** |
| Extensions | 0 | 120+ | **NOUVEAU** |
| Compatibilité | .NET 8 | .NET 6/7/8 | **Étendue** |

---

## 🌟 Avantages v1.2.0

### Pour les Développeurs
- ✅ **Extensions fluides** - Code plus lisible et expressif
- ✅ **Null safety** - Gestion élégante des valeurs nulles
- ✅ **Conversions sûres** - Pas d'exceptions, valeurs par défaut
- ✅ **LINQ enrichi** - Opérations avancées sur collections

### Pour les Projets
- ✅ **Multi-targeting** - Support .NET 6, 7, 8
- ✅ **Rétrocompatibilité** - Mise à jour sans casse
- ✅ **Performance** - Extensions optimisées
- ✅ **Productivité** - Moins de code boilerplate

### Pour l'Équipe
- ✅ **Standards** - Extensions cohérentes dans toute l'équipe
- ✅ **Maintenance** - Code plus maintenable
- ✅ **Tests** - Méthodes testées et fiables
- ✅ **Documentation** - XML docs complets

---

## 🔄 Migration depuis v1.1.0

La migration est **100% rétrocompatible**:

```bash
# Mettre à jour simplement
dotnet add package KBA.CoreUtilities

# Ajouter le nouveau namespace pour les extensions
using KBA.CoreUtilities.Extensions;
```

**Aucun code existant n'est cassé!**

---

## 📚 Modules du Package v1.2.0

### Extensions (NOUVEAU)
1. **StringExtensions** - 50+ méthodes
2. **CollectionExtensions** - 40+ méthodes
3. **ObjectExtensions** - 30+ méthodes

### Utilitaires (v1.1.0)
4. **ValidationUtils** - IBAN, cartes, BIC, VAT
5. **CryptographyUtils** - AES, RSA, hashing
6. **FileUtils** - Compression, MIME

### Utilitaires Core (v1.0.0)
7. **CountryUtils** - 200+ pays
8. **PhoneUtils** - Validation internationale
9. **QrCodeUtils** - EMV, 170+ devises
10. **DateTimeUtils** - 50+ méthodes
11. **SerializationUtils** - JSON/XML
12. **ApiUtils** - REST/SOAP
13. **DecimalUtils** - Calculs financiers
14. **StringUtils** - Manipulation
15. **LoggingUtils** - Structured logging

### Spécial .NET 8
16. **RestApiBuilder** - API builder (.NET 8 only)

**Total: 16 modules | 400+ méthodes**

---

## 💡 Cas d'Usage

### API Web
```csharp
// Valider et convertir les paramètres
var pageNumber = Request.Query["page"].ToInt(1);
var pageSize = Request.Query["size"].ToInt(20).Clamp(1, 100);

var results = data.Page(pageNumber, pageSize);
```

### Traitement de Données
```csharp
// Nettoyer et valider
var validEmails = emails
    .WhereNotNull()
    .Where(e => e.IsEmail())
    .DistinctBy(e => e.ToLowerInvariant());

// Grouper par batches
validEmails.Chunk(1000).ForEach(batch => 
{
    SendBulkEmails(batch);
});
```

### Configuration
```csharp
// Null-safe configuration
var apiKey = config
    .GetValue("ApiKey")
    .GetDefaultIfEmpty(Environment.GetEnvironmentVariable("API_KEY"))
    .ThrowIfNull("API key not configured");
```

---

## 🎊 Conclusion

**KBA.CoreUtilities v1.2.0** apporte une **évolution majeure** avec:

- 🎯 **Multi-targeting** pour .NET 6, 7, 8
- 📝 **120+ extensions** pour développement moderne
- 🚀 **400+ méthodes** au total
- ✅ **100% rétrocompatible**

Le package est maintenant **plus puissant**, **plus flexible** et **plus accessible** que jamais!

---

## 📦 Installation

```bash
dotnet add package KBA.CoreUtilities
```

**URL:** https://www.nuget.org/packages/KBA.CoreUtilities/1.2.0

---

**🎉 Merci d'utiliser KBA.CoreUtilities! 🚀**

*Publié le 27 octobre 2025*  
*Version: 1.2.0*  
*Statut: ✅ PUBLIÉ SUR NUGET.ORG*  
*Frameworks: .NET 6.0 | 7.0 | 8.0*
