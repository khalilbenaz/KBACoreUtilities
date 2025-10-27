# Contributing to KBA.CoreUtilities

First off, thank you for considering contributing to KBA.CoreUtilities! 🎉

## Code of Conduct

This project and everyone participating in it is governed by our Code of Conduct. By participating, you are expected to uphold this code.

## How Can I Contribute?

### Reporting Bugs

Before creating bug reports, please check the issue list as you might find out that you don't need to create one. When you are creating a bug report, please include as many details as possible:

- **Use a clear and descriptive title**
- **Describe the exact steps to reproduce the problem**
- **Provide specific examples to demonstrate the steps**
- **Describe the behavior you observed after following the steps**
- **Explain which behavior you expected to see instead and why**
- **Include code samples and stack traces**

### Suggesting Enhancements

Enhancement suggestions are tracked as GitHub issues. When creating an enhancement suggestion, please include:

- **Use a clear and descriptive title**
- **Provide a step-by-step description of the suggested enhancement**
- **Provide specific examples to demonstrate the steps**
- **Describe the current behavior and explain which behavior you expected to see instead**
- **Explain why this enhancement would be useful**

### Pull Requests

- Fill in the required template
- Follow the C# coding style (see below)
- Include appropriate test coverage
- Update documentation as needed
- End all files with a newline

## Development Setup

### Prerequisites

- .NET 6.0 SDK or higher
- Visual Studio 2022 / VS Code / Rider
- Git

### Getting Started

1. Fork the repository
2. Clone your fork:
   ```bash
   git clone https://github.com/YOUR_USERNAME/KBACoreUtilities.git
   cd KBACoreUtilities
   ```

3. Create a branch:
   ```bash
   git checkout -b feature/my-new-feature
   ```

4. Make your changes and commit:
   ```bash
   git add .
   git commit -m "Add my new feature"
   ```

5. Push to your fork:
   ```bash
   git push origin feature/my-new-feature
   ```

6. Create a Pull Request

### Building the Project

```bash
# Restore dependencies
dotnet restore

# Build
dotnet build

# Run tests
dotnet test

# Create NuGet package
dotnet pack -c Release
```

## Coding Style

### General Guidelines

- Follow standard C# naming conventions
- Use meaningful variable and method names
- Keep methods small and focused
- Write XML documentation comments for public APIs
- Use `var` for local variables when the type is obvious
- Prefer expression-bodied members when appropriate

### Example

```csharp
namespace KBA.CoreUtilities.Utilities
{
    /// <summary>
    /// Utilities for string manipulation and validation
    /// </summary>
    public static class StringUtils
    {
        /// <summary>
        /// Checks if a string is null or empty
        /// </summary>
        /// <param name="value">The string to check</param>
        /// <returns>True if null or empty, false otherwise</returns>
        public static bool IsNullOrEmpty(string value)
        {
            return string.IsNullOrEmpty(value);
        }
    }
}
```

### Naming Conventions

- **Classes/Interfaces**: PascalCase (e.g., `ValidationUtils`, `IService`)
- **Methods**: PascalCase (e.g., `IsValidEmail`, `GetCountryByCode`)
- **Properties**: PascalCase (e.g., `CountryName`, `CurrencyCode`)
- **Private fields**: _camelCase with underscore (e.g., `_cache`, `_logger`)
- **Parameters/Local variables**: camelCase (e.g., `userId`, `result`)
- **Constants**: PascalCase (e.g., `MaxRetries`, `DefaultTimeout`)

### File Organization

- One class per file
- File name should match class name
- Organize files in appropriate folders:
  - `Utilities/` - Static utility classes
  - `Extensions/` - Extension methods
  - `Models/` - Data models and DTOs

### Documentation

All public APIs must have XML documentation:

```csharp
/// <summary>
/// Validates an IBAN number using Mod 97 algorithm
/// </summary>
/// <param name="iban">The IBAN to validate</param>
/// <returns>True if valid, false otherwise</returns>
/// <example>
/// <code>
/// bool isValid = ValidationUtils.IsValidIBAN("FR1420041010050500013M02606");
/// </code>
/// </example>
public static bool IsValidIBAN(string iban)
{
    // Implementation
}
```

## Testing

### Writing Tests

- Write unit tests for all new functionality
- Use descriptive test names that explain what is being tested
- Follow the Arrange-Act-Assert pattern
- Test edge cases and error conditions

### Test Example

```csharp
[Fact]
public void IsValidEmail_WithValidEmail_ReturnsTrue()
{
    // Arrange
    var email = "test@example.com";
    
    // Act
    var result = ValidationUtils.IsValidEmail(email);
    
    // Assert
    Assert.True(result);
}
```

## Commit Messages

- Use the present tense ("Add feature" not "Added feature")
- Use the imperative mood ("Move cursor to..." not "Moves cursor to...")
- Limit the first line to 72 characters or less
- Reference issues and pull requests liberally after the first line

### Examples

```
Add credit card validation utility

- Implement Luhn algorithm
- Support Visa, Mastercard, Amex
- Add comprehensive tests
- Update documentation

Fixes #123
```

## Release Process

1. Update version in `.csproj` file
2. Update `CHANGELOG.md`
3. Create a git tag: `git tag -a v1.3.1 -m "Release v1.3.1"`
4. Push tag: `git push origin v1.3.1`
5. GitHub Actions will automatically build and publish to NuGet

## Module Structure

When adding a new utility module:

1. Create class in appropriate namespace
2. Add XML documentation
3. Write unit tests
4. Update README with examples
5. Update CHANGELOG

### New Utility Module Template

```csharp
using System;

namespace KBA.CoreUtilities.Utilities
{
    /// <summary>
    /// Utilities for [description]
    /// </summary>
    public static class YourUtils
    {
        /// <summary>
        /// [Method description]
        /// </summary>
        /// <param name="param">[Parameter description]</param>
        /// <returns>[Return value description]</returns>
        /// <example>
        /// <code>
        /// var result = YourUtils.MethodName(param);
        /// </code>
        /// </example>
        public static ReturnType MethodName(ParamType param)
        {
            // Validation
            if (param == null)
                throw new ArgumentNullException(nameof(param));
            
            // Implementation
            return result;
        }
    }
}
```

## Extension Method Guidelines

When adding extension methods:

```csharp
using System;

namespace KBA.CoreUtilities.Extensions
{
    /// <summary>
    /// Extension methods for [type]
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// [Method description]
        /// </summary>
        /// <param name="source">The source object</param>
        /// <returns>[Return description]</returns>
        public static ReturnType MethodName(this SourceType source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            
            // Implementation
            return result;
        }
    }
}
```

## Questions?

Feel free to open an issue with the `question` label if you have any questions about contributing.

## Recognition

Contributors will be recognized in:
- GitHub contributors page
- Release notes
- Project documentation

Thank you for contributing to KBA.CoreUtilities! 🚀
