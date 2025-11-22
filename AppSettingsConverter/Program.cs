// <copyright file="Program.cs" company="AppSettingsConverter">
// Copyright (c) 2024 AppSettingsConverter Contributors
// </copyright>

namespace AppSettingsConverter;

using System.Text;
using System.Text.Json;

/// <summary>
/// Main entry point for the AppSettingsConverter application.
/// </summary>
internal class Program
{
    private const int ExitCodeSuccess = 0;
    private const int ExitCodeInvalidArguments = 1;
    private const int ExitCodeFileNotFound = 2;
    private const int ExitCodeJsonParseError = 3;
    private const int ExitCodeInvalidDelimiter = 4;
    private const int ExitCodeFileReadError = 5;

    private static int Main(string[] args)
    {
        // Check for help flag first, before validating argument count
        if (args.Length > 0 && (args[0] == "--help" || args[0] == "-h" || args[0] == "/?"))
        {
            ShowHelp();
            return ExitCodeSuccess;
        }

        if (args.Length < 2)
        {
            Console.Error.WriteLine("Error: Missing required arguments.");
            Console.WriteLine();
            Console.WriteLine("Usage: AppSettingsConverter <path_to_json_file> <delimiter> [options]");
            Console.WriteLine();
            Console.WriteLine("Arguments:");
            Console.WriteLine("  <path_to_json_file>  Path to the JSON configuration file to flatten");
            Console.WriteLine("  <delimiter>          Delimiter to use for flattening (e.g., '__' or ':')");
            Console.WriteLine();
            Console.WriteLine("Options:");
            Console.WriteLine("  --output <file>      Output file path (default: stdout)");
            Console.WriteLine("  --format json        Output as JSON array (default: formatted lines)");
            Console.WriteLine("  --help               Show this help message");
            return ExitCodeInvalidArguments;
        }

        var filePath = args[0];
        var delimiter = args[1];

        // Validate delimiter
        if (string.IsNullOrWhiteSpace(delimiter))
        {
            Console.Error.WriteLine($"Error: Delimiter cannot be empty.");
            return ExitCodeInvalidDelimiter;
        }

        // Parse optional arguments
        string? outputFile = null;
        bool outputAsJson = false;
        for (int i = 2; i < args.Length; i++)
        {
            if (args[i] == "--output" && i + 1 < args.Length)
            {
                outputFile = args[++i];
            }
            else if (args[i] == "--format" && i + 1 < args.Length && args[i + 1] == "json")
            {
                outputAsJson = true;
                i++;
            }
        }

        if (!File.Exists(filePath))
        {
            Console.Error.WriteLine($"Error: File not found: {filePath}");
            return ExitCodeFileNotFound;
        }

        string jsonConfig;
        try
        {
            jsonConfig = File.ReadAllText(filePath);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error reading file: {ex.Message}");
            return ExitCodeFileReadError;
        }

        var flattenedConfig = FlattenConfig(jsonConfig, delimiter);
        if (flattenedConfig == null)
        {
            return ExitCodeJsonParseError;
        }

        var output = GenerateOutput(flattenedConfig, outputAsJson);

        if (!string.IsNullOrEmpty(outputFile))
        {
            try
            {
                File.WriteAllText(outputFile, output);
                Console.WriteLine($"Output written to: {outputFile}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error writing output file: {ex.Message}");
                return ExitCodeFileReadError;
            }
        }
        else
        {
            Console.Write(output);
        }

        return ExitCodeSuccess;
    }

    private static void ShowHelp()
    {
        Console.WriteLine("AppSettingsConverter - Flatten JSON configuration files");
        Console.WriteLine();
        Console.WriteLine("Usage: AppSettingsConverter <path_to_json_file> <delimiter> [options]");
        Console.WriteLine();
        Console.WriteLine("Arguments:");
        Console.WriteLine("  <path_to_json_file>  Path to the JSON configuration file to flatten");
        Console.WriteLine("  <delimiter>          Delimiter to use for flattening (e.g., '__' or ':')");
        Console.WriteLine();
        Console.WriteLine("Options:");
        Console.WriteLine("  --output <file>      Output file path (default: stdout)");
        Console.WriteLine("  --format json        Output as JSON array (default: formatted lines)");
        Console.WriteLine("  --help               Show this help message");
        Console.WriteLine();
        Console.WriteLine("Examples:");
        Console.WriteLine("  AppSettingsConverter appsettings.json __");
        Console.WriteLine("  AppSettingsConverter appsettings.json : --output output.json");
        Console.WriteLine("  AppSettingsConverter appsettings.json __ --format json");
    }

    private static List<ConfigItem>? FlattenConfig(string jsonConfig, string delimiter)
    {
        var flattenedConfig = new List<ConfigItem>();
        try
        {
            using var doc = JsonDocument.Parse(jsonConfig);
            FlattenJsonObject(doc.RootElement, string.Empty, flattenedConfig, delimiter);
        }
        catch (JsonException ex)
        {
            Console.Error.WriteLine($"Error parsing JSON: {ex.Message}");
            return null;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            return null;
        }

        return flattenedConfig;
    }

    private static void FlattenJsonObject(JsonElement jsonElement, string parentKey, List<ConfigItem> flattenedConfig, string delimiter)
    {
        switch (jsonElement.ValueKind)
        {
            case JsonValueKind.Object:
                foreach (var property in jsonElement.EnumerateObject())
                {
                    var currentKey = string.IsNullOrEmpty(parentKey)
                        ? property.Name
                        : $"{parentKey}{delimiter}{property.Name}";
                    FlattenJsonObject(property.Value, currentKey, flattenedConfig, delimiter);
                }
                break;

            case JsonValueKind.Array:
                var index = 0;
                foreach (var arrayElement in jsonElement.EnumerateArray())
                {
                    var currentKey = $"{parentKey}{delimiter}{index}";
                    FlattenJsonObject(arrayElement, currentKey, flattenedConfig, delimiter);
                    index++;
                }
                break;

            case JsonValueKind.String:
                flattenedConfig.Add(new ConfigItem
                {
                    Name = parentKey,
                    Value = jsonElement.GetString() ?? string.Empty,
                    SlotSetting = false
                });
                break;

            case JsonValueKind.Number:
            case JsonValueKind.True:
            case JsonValueKind.False:
                flattenedConfig.Add(new ConfigItem
                {
                    Name = parentKey,
                    Value = jsonElement.ToString(),
                    SlotSetting = false
                });
                break;

            case JsonValueKind.Null:
                flattenedConfig.Add(new ConfigItem
                {
                    Name = parentKey,
                    Value = string.Empty,
                    SlotSetting = false
                });
                break;

            default:
                // Ignore other value types
                break;
        }
    }

    private static string GenerateOutput(List<ConfigItem> items, bool asJson)
    {
        if (asJson)
        {
            var jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            return JsonSerializer.Serialize(items, jsonOptions);
        }

        var sb = new StringBuilder();
        for (int i = 0; i < items.Count; i++)
        {
            var item = items[i];
            var isLast = i == items.Count - 1;

            sb.AppendLine("  {");
            sb.AppendLine($"     \"name\": {JsonSerializer.Serialize(item.Name)},");
            sb.AppendLine($"     \"value\": {JsonSerializer.Serialize(item.Value)},");
            sb.AppendLine($"     \"slotSetting\": {item.SlotSetting.ToString().ToLowerInvariant()}");
            sb.Append("  }");
            if (!isLast)
            {
                sb.Append(",");
            }
            sb.AppendLine();
        }

        return sb.ToString();
    }
}
