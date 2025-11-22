# Contributing to AppSettingsConverter

Thank you for your interest in contributing to AppSettingsConverter! This document provides guidelines and instructions for contributing.

## Code of Conduct

This project adheres to a code of conduct that all contributors are expected to follow. Please be respectful and constructive in all interactions.

## How to Contribute

### Reporting Bugs

If you find a bug, please open an issue with:

- A clear, descriptive title
- Steps to reproduce the issue
- Expected behavior
- Actual behavior
- Your environment (OS, .NET version, etc.)
- Any relevant error messages or logs

### Suggesting Features

Feature suggestions are welcome! Please open an issue with:

- A clear description of the feature
- Use cases and examples
- Any potential implementation ideas (optional)

### Pull Requests

1. **Fork the repository** and create a new branch from `main`
   ```bash
   git checkout -b feature/your-feature-name
   ```

2. **Make your changes** following the coding standards:
   - Follow the existing code style
   - Add comments for complex logic
   - Update documentation as needed
   - Add or update tests if applicable

3. **Test your changes**
   ```bash
   dotnet build
   dotnet test
   ```

4. **Commit your changes** with clear, descriptive commit messages
   ```bash
   git commit -m "Add feature: description of your changes"
   ```

5. **Push to your fork** and open a Pull Request
   - Provide a clear description of your changes
   - Reference any related issues
   - Ensure all checks pass

## Development Setup

1. Clone the repository:
   ```bash
   git clone https://github.com/yourusername/AppSettingsConverter.git
   cd AppSettingsConverter
   ```

2. Restore dependencies:
   ```bash
   dotnet restore
   ```

3. Build the project:
   ```bash
   dotnet build
   ```

4. Run tests:
   ```bash
   dotnet test
   ```

## Coding Standards

- Use C# 12 features where appropriate
- Follow the `.editorconfig` settings
- Use meaningful variable and method names
- Add XML documentation comments for public APIs
- Keep methods focused and single-purpose
- Handle errors gracefully with appropriate error messages

## Testing

- Write unit tests for new features
- Ensure all existing tests pass
- Aim for good test coverage
- Test edge cases and error conditions

## Documentation

- Update the README.md if you add new features or change behavior
- Add XML documentation comments for public APIs
- Update CHANGELOG.md with your changes

## Questions?

If you have questions, feel free to:
- Open an issue with the `question` label
- Check existing issues and discussions

Thank you for contributing! 🎉

