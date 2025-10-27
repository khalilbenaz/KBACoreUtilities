# Guide de Publication sur NuGet.org

## 📦 Nom du Package Recommandé

**`KBA.CoreUtilities`**

Ce nom est :
- ✅ Professionnel et descriptif
- ✅ Facile à mémoriser
- ✅ Suit les conventions .NET
- ✅ Unique sur NuGet.org

### Alternatives possibles :
- `KBA.FinancialUtilities` (focus services financiers)
- `KBA.PaymentTools` (focus paiements)
- `KBA.MobileMoneySDK` (focus mobile money)
- `KBA.EMVTools` (focus EMV/QR codes)

---

## 🚀 Étapes de Publication

### 1. Prérequis

Avant de publier, assurez-vous d'avoir :

- [x] Un compte sur [NuGet.org](https://www.nuget.org/)
- [x] Une clé API NuGet (obtenue depuis votre compte)
- [x] .NET 8 SDK installé
- [x] Le projet compile sans erreurs

### 2. Obtenir une Clé API NuGet

1. Connectez-vous à [NuGet.org](https://www.nuget.org/)
2. Allez dans **Account Settings** → **API Keys**
3. Cliquez sur **Create** pour créer une nouvelle clé
4. Configurez :
   - **Key Name** : `KBA.CoreUtilities-Deploy`
   - **Package Owner** : Votre compte
   - **Scopes** : `Push new packages and package versions`
   - **Glob Pattern** : `KBA.CoreUtilities`
   - **Expires** : Choisissez une durée (recommandé : 1 an)
5. Copiez la clé générée (elle ne sera affichée qu'une seule fois !)

### 3. Vérification Avant Publication

Exécutez ces commandes pour vérifier que tout est prêt :

```bash
cd /Users/lilou/Downloads/KBACoreUtilities/KBA.CoreUtilities

# Nettoyer les builds précédents
dotnet clean

# Restaurer les dépendances
dotnet restore

# Compiler en mode Release
dotnet build -c Release

# Vérifier qu'il n'y a pas d'erreurs
dotnet test (si vous avez des tests)
```

### 4. Créer le Package NuGet

```bash
# Créer le package .nupkg
dotnet pack -c Release

# Le fichier sera créé dans :
# bin/Release/KBA.CoreUtilities.1.0.0.nupkg
```

### 5. Valider le Package Localement

Avant de publier, validez le contenu du package :

```bash
# Installer l'outil NuGet Package Explorer (optionnel mais recommandé)
dotnet tool install -g NuGetPackageExplorer

# Ou utilisez la commande pour lister le contenu
unzip -l bin/Release/KBA.CoreUtilities.1.0.0.nupkg
```

Vérifiez que le package contient :
- ✅ Tous les fichiers .dll
- ✅ Le fichier README.md
- ✅ Les fichiers XML de documentation
- ✅ Les métadonnées correctes (.nuspec)

### 6. Publier sur NuGet.org

**Option A : Publication via ligne de commande**

```bash
# Remplacez YOUR_API_KEY par votre clé API réelle
dotnet nuget push bin/Release/KBA.CoreUtilities.1.0.0.nupkg \
  --api-key YOUR_API_KEY \
  --source https://api.nuget.org/v3/index.json
```

**Option B : Publication via NuGet CLI**

```bash
nuget push bin/Release/KBA.CoreUtilities.1.0.0.nupkg \
  -ApiKey YOUR_API_KEY \
  -Source https://api.nuget.org/v3/index.json
```

### 7. Vérification Post-Publication

Après quelques minutes (5-15 min), votre package sera disponible sur NuGet.org :

1. Visitez : `https://www.nuget.org/packages/KBA.CoreUtilities`
2. Vérifiez que :
   - ✅ Le package apparaît dans les résultats de recherche
   - ✅ La description est correcte
   - ✅ Les tags sont visibles
   - ✅ Le README s'affiche correctement
   - ✅ La version est correcte

### 8. Tester l'Installation

Créez un nouveau projet de test pour vérifier l'installation :

```bash
# Créer un projet de test
mkdir test-nuget-install
cd test-nuget-install
dotnet new console

# Installer votre package
dotnet add package KBA.CoreUtilities

# Tester l'utilisation
```

Exemple de code de test (Program.cs) :

```csharp
using KBA.CoreUtilities.Utilities;

// Test Country Utils
var senegal = CountryUtils.GetCountryByIso2("SN");
Console.WriteLine($"Country: {senegal.Name}, Currency: {senegal.CurrencyCode}");

// Test Phone Utils
string phone = PhoneUtils.FormatPhoneNumber("771234567", "SN");
Console.WriteLine($"Formatted Phone: {phone}");

// Test DateTime Utils
string relative = DateTimeUtils.ToRelativeTime(DateTime.Now.AddHours(-2));
Console.WriteLine($"Time: {relative}");

// Test QR Code
var payment = EmvPaymentData.CreateDefault("Test Shop", "Dakar", 5000m);
byte[] qr = QrCodeUtils.GenerateEmvPaymentQrCodeImage(payment);
Console.WriteLine($"QR Code generated: {qr.Length} bytes");

Console.WriteLine("\n✅ All tests passed! Package works correctly.");
```

---

## 🔄 Publier une Nouvelle Version

Lorsque vous voulez publier une mise à jour :

### 1. Mettre à jour la version

Modifiez dans `KBA.CoreUtilities.csproj` :

```xml
<PackageVersion>1.0.1</PackageVersion>
<AssemblyVersion>1.0.1.0</AssemblyVersion>
<FileVersion>1.0.1.0</FileVersion>
```

### 2. Mettre à jour les Release Notes

```xml
<PackageReleaseNotes>
  Version 1.0.1:
  - Bug fixes...
  - New features...
  - Performance improvements...
</PackageReleaseNotes>
```

### 3. Republier

```bash
dotnet clean
dotnet pack -c Release
dotnet nuget push bin/Release/KBA.CoreUtilities.1.0.1.nupkg \
  --api-key YOUR_API_KEY \
  --source https://api.nuget.org/v3/index.json
```

---

## 📋 Versioning (Semantic Versioning)

Suivez le format **MAJOR.MINOR.PATCH** :

- **MAJOR** (1.x.x) : Changements incompatibles avec l'API précédente
- **MINOR** (x.1.x) : Nouvelles fonctionnalités rétrocompatibles
- **PATCH** (x.x.1) : Corrections de bugs rétrocompatibles

Exemples :
- `1.0.0` → Première version stable
- `1.0.1` → Corrections de bugs
- `1.1.0` → Nouvelles fonctionnalités
- `2.0.0` → Changements majeurs (breaking changes)

---

## 🔒 Sécurité de la Clé API

**⚠️ IMPORTANT : Protégez votre clé API !**

### Bonnes pratiques :

1. **Ne jamais commiter la clé dans Git**
   ```bash
   # Ajoutez à .gitignore
   echo "*.apikey" >> .gitignore
   echo "nuget.config" >> .gitignore
   ```

2. **Utiliser des variables d'environnement**
   ```bash
   # Linux/macOS
   export NUGET_API_KEY="your_key_here"
   
   # Windows
   set NUGET_API_KEY=your_key_here
   
   # Puis dans la commande :
   dotnet nuget push bin/Release/KBA.CoreUtilities.1.0.0.nupkg \
     --api-key $NUGET_API_KEY \
     --source https://api.nuget.org/v3/index.json
   ```

3. **Utiliser GitHub Secrets pour CI/CD**
   
   Dans GitHub Actions, stockez la clé dans **Settings** → **Secrets** → **Actions**

---

## 🔄 CI/CD avec GitHub Actions

Créez `.github/workflows/publish-nuget.yml` :

```yaml
name: Publish to NuGet

on:
  push:
    tags:
      - 'v*'

jobs:
  publish:
    runs-on: ubuntu-latest
    steps:
    - uses: actions/checkout@v3
    
    - name: Setup .NET
      uses: actions/setup-dotnet@v3
      with:
        dotnet-version: 8.0.x
    
    - name: Restore dependencies
      run: dotnet restore
      working-directory: KBA.CoreUtilities
    
    - name: Build
      run: dotnet build -c Release --no-restore
      working-directory: KBA.CoreUtilities
    
    - name: Pack
      run: dotnet pack -c Release --no-build
      working-directory: KBA.CoreUtilities
    
    - name: Publish to NuGet
      run: dotnet nuget push bin/Release/*.nupkg --api-key ${{ secrets.NUGET_API_KEY }} --source https://api.nuget.org/v3/index.json
      working-directory: KBA.CoreUtilities
```

Pour publier automatiquement :
```bash
git tag v1.0.0
git push origin v1.0.0
```

---

## 📊 Suivi Post-Publication

### Métriques à surveiller sur NuGet.org :

1. **Downloads** : Nombre de téléchargements
2. **Statistics** : Tendances d'utilisation
3. **Versions** : Adoption des différentes versions
4. **Dependencies** : Packages qui dépendent du vôtre

### Promouvoir votre package :

- 📝 Écrivez un article de blog
- 🐦 Annoncez sur Twitter/LinkedIn
- 💬 Partagez dans des communautés .NET
- 📚 Créez des tutoriels vidéo
- 🌟 Ajoutez un badge NuGet dans votre README GitHub

---

## 🆘 Résolution de Problèmes

### Erreur : "Package already exists"
- Vous ne pouvez pas remplacer une version déjà publiée
- Incrémentez la version et republiez

### Erreur : "Invalid API key"
- Vérifiez que la clé est correcte
- Vérifiez que la clé n'a pas expiré
- Vérifiez les permissions de la clé

### Package ne s'affiche pas après publication
- Attendez 5-15 minutes pour l'indexation
- Videz le cache NuGet local : `dotnet nuget locals all --clear`

### Erreur lors du build
```bash
# Nettoyer complètement
dotnet clean
rm -rf bin obj
dotnet restore
dotnet build -c Release
```

---

## ✅ Checklist Finale Avant Publication

- [ ] Le code compile sans erreurs ni warnings
- [ ] Les tests passent (si applicable)
- [ ] La version est correctement incrémentée
- [ ] Le README.md est à jour
- [ ] Les release notes sont complétées
- [ ] La licence est spécifiée (MIT)
- [ ] Le .gitignore exclut les fichiers sensibles
- [ ] Le package a été testé localement
- [ ] La clé API est sécurisée
- [ ] Vous avez fait un commit de toutes les modifications

---

## 🎉 Félicitations !

Une fois publié, votre package sera disponible mondialement via :

```bash
dotnet add package KBA.CoreUtilities
```

**Bonne chance avec votre publication ! 🚀**
