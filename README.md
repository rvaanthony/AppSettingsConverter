# AppSettingsConverter

[![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![GitHub](https://img.shields.io/github/license/yourusername/AppSettingsConverter)](LICENSE.txt)

A powerful .NET console application that flattens JSON configuration files (like `appsettings.json`) into Azure App Service configuration format. Perfect for converting nested JSON configurations into flat key-value pairs with customizable delimiters.

## Features

- ✅ Flattens nested JSON configurations into flat key-value pairs
- ✅ Supports custom delimiters (e.g., `__`, `:`, or any string)
- ✅ Handles arrays, objects, and all JSON value types
- ✅ Proper JSON escaping for values containing special characters
- ✅ Multiple output formats (formatted lines or JSON array)
- ✅ Output to file or stdout
- ✅ Comprehensive error handling with exit codes
- ✅ Cross-platform (.NET 10.0)

## Installation

### Prerequisites

- [.NET 10.0 SDK](https://dotnet.microsoft.com/download) or later

### Build from Source

```bash
git clone https://github.com/yourusername/AppSettingsConverter.git
cd AppSettingsConverter
dotnet build
```

### Run Directly

```bash
dotnet run -- <path_to_json_file> <delimiter>
```

## Usage

### Basic Usage

```bash
AppSettingsConverter <path_to_json_file> <delimiter>
```

**Arguments:**
- `<path_to_json_file>` - Path to the JSON configuration file to flatten
- `<delimiter>` - Delimiter to use for flattening (e.g., `__` or `:`)

**Options:**
- `--output <file>` - Output file path (default: stdout)
- `--format json` - Output as JSON array (default: formatted lines)
- `--help` - Show help message

### Examples

#### Example 1: Basic Flattening

Input file (`appsettings.json`):
```json
{
  "ApiUrl": "www.api.com",
  "Emails": {
    "Admin": "admin@admin.com"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

Command:
```bash
AppSettingsConverter appsettings.json __
```

Output:
```
  {
     "name": "ApiUrl",
     "value": "www.api.com",
     "slotSetting": false
  },
  {
     "name": "Emails__Admin",
     "value": "admin@admin.com",
     "slotSetting": false
  },
  {
     "name": "Logging__LogLevel__Default",
     "value": "Information",
     "slotSetting": false
  },
  {
     "name": "Logging__LogLevel__Microsoft.AspNetCore",
     "value": "Warning",
     "slotSetting": false
  }
```

#### Example 2: Output to File

```bash
AppSettingsConverter appsettings.json __ --output flattened.json
```

#### Example 3: JSON Array Output

```bash
AppSettingsConverter appsettings.json __ --format json
```

Output:
```json
[
  {
    "Name": "ApiUrl",
    "Value": "www.api.com",
    "SlotSetting": false
  },
  {
    "Name": "Emails__Admin",
    "Value": "admin@admin.com",
    "SlotSetting": false
  }
]
```

#### Example 4: Using Colon Delimiter

```bash
AppSettingsConverter appsettings.json :
```

## Exit Codes

The application returns the following exit codes:

- `0` - Success
- `1` - Invalid arguments
- `2` - File not found
- `3` - JSON parse error
- `4` - Invalid delimiter
- `5` - File read/write error

## Supported JSON Types

The converter handles all JSON value types:

- **Objects** - Flattened with delimiter-separated keys
- **Arrays** - Flattened with numeric indices (e.g., `Items__0`, `Items__1`)
- **Strings** - Preserved with proper escaping
- **Numbers** - Converted to string representation
- **Booleans** - Converted to `true`/`false` strings
- **Null** - Converted to empty string

## Use Cases

- **Azure App Service Configuration** - Convert `appsettings.json` to Azure App Service application settings format
- **Environment Variables** - Generate environment variable format from JSON configuration
- **Configuration Migration** - Migrate nested configurations to flat key-value stores
- **CI/CD Pipelines** - Automate configuration transformation in build pipelines

## Docker

You can also run AppSettingsConverter using Docker:

```bash
# Build the image
docker build -t appsettingsconverter .

# Run with a JSON file
docker run --rm -v $(pwd):/data appsettingsconverter /data/appsettings.json __

# Or with output to file
docker run --rm -v $(pwd):/data appsettingsconverter /data/appsettings.json __ --output /data/output.json
```

## Examples

Example JSON files are available in the [`examples`](examples/) directory:
- [`appsettings.json`](examples/appsettings.json) - Sample input file
- [`appsettings.expected.json`](examples/appsettings.expected.json) - Expected output format

## Contributing

Contributions are welcome! Please read our [Contributing Guide](CONTRIBUTING.md) for details on our code of conduct and the process for submitting pull requests.

## License

This project is licensed under the MIT License - see the [LICENSE.txt](LICENSE.txt) file for details.

## Acknowledgments

- Built with [.NET](https://dotnet.microsoft.com/)
- Uses [System.Text.Json](https://docs.microsoft.com/en-us/dotnet/api/system.text.json) for JSON parsing

---

**Made with ❤️ for the .NET community**
