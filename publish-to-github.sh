#!/bin/bash

# Script de publication sur GitHub
# Usage: ./publish-to-github.sh [github-username]

set -e

GITHUB_USERNAME=${1:-"VOTRE_USERNAME"}

echo "🚀 Publication de KBA.CoreUtilities sur GitHub"
echo "================================================"
echo ""

# Vérifier si on est dans le bon répertoire
if [ ! -f "KBACoreUtilities.sln" ]; then
    echo "❌ Erreur: Ce script doit être exécuté depuis le répertoire racine du projet"
    exit 1
fi

# Vérifier si git est initialisé
if [ ! -d ".git" ]; then
    echo "📦 Initialisation de git..."
    git init
fi

# Vérifier si on a des changements
if [ -z "$(git status --porcelain)" ]; then
    echo "✅ Aucun changement à commiter"
else
    echo "📝 Ajout des fichiers..."
    git add .
    
    echo "💾 Commit des changements..."
    git commit -m "Release v1.3.1 - Complete enterprise toolkit with 500+ methods

Features:
- 20 modules with 500+ production-ready methods
- Multi-targeting: .NET 6.0, 7.0, 8.0
- TaskExtensions: Async patterns with retry, timeout, parallel processing
- ReflectionUtils: Dynamic property/method access, type inspection, object mapping
- PerformanceUtils: Profiling, caching, benchmarking, memory monitoring
- ConfigurationExtensions: Type-safe config access with validation
- StringExtensions: 50+ methods for validation, conversion, hashing
- CollectionExtensions: 40+ LINQ enhancements
- ObjectExtensions: 30+ null handling and conversion methods
- ValidationUtils: IBAN, credit cards, BIC/SWIFT, VAT validation
- CryptographyUtils: AES-256, RSA, PBKDF2, secure tokens
- EMV QR Codes: Payment QR codes with 170+ currencies
- International support: 200+ countries, phone validation
- Complete documentation with 120+ code examples
- CI/CD with GitHub Actions
- Automatic NuGet publication on tags"
fi

# Renommer la branche en main
echo "🔄 Renommage de la branche en 'main'..."
git branch -M main

# Vérifier si le remote existe
if git remote | grep -q "^origin$"; then
    echo "✅ Remote 'origin' existe déjà"
else
    echo "🔗 Ajout du remote GitHub..."
    git remote add origin "https://github.com/${GITHUB_USERNAME}/KBACoreUtilities.git"
fi

echo ""
echo "⚠️  ATTENTION: Vous allez pusher vers:"
echo "   https://github.com/${GITHUB_USERNAME}/KBACoreUtilities.git"
echo ""
read -p "Continuer? (y/n) " -n 1 -r
echo

if [[ $REPLY =~ ^[Yy]$ ]]; then
    echo "⬆️  Push vers GitHub..."
    git push -u origin main
    
    echo ""
    echo "🏷️  Création du tag v1.3.1..."
    git tag -a v1.3.1 -m "Release v1.3.1 - Complete documentation and GitHub setup"
    
    echo "⬆️  Push du tag..."
    git push origin v1.3.1
    
    echo ""
    echo "✅ SUCCÈS! Votre projet est maintenant sur GitHub!"
    echo ""
    echo "📋 Prochaines étapes:"
    echo "1. Ajoutez le secret NUGET_API_KEY dans GitHub:"
    echo "   https://github.com/${GITHUB_USERNAME}/KBACoreUtilities/settings/secrets/actions"
    echo ""
    echo "2. Vérifiez les GitHub Actions:"
    echo "   https://github.com/${GITHUB_USERNAME}/KBACoreUtilities/actions"
    echo ""
    echo "3. Ajoutez les topics au repository:"
    echo "   https://github.com/${GITHUB_USERNAME}/KBACoreUtilities/settings"
    echo ""
    echo "🎉 Le package sera automatiquement publié sur NuGet via le tag!"
else
    echo "❌ Publication annulée"
    exit 1
fi
