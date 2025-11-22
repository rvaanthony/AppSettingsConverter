using System.Text.Json;
using Xunit;

namespace AppSettingsConverter.Tests;

public class FlattenConfigTests
{
    [Fact]
    public void Flatten_SimpleObject_ReturnsFlattenedConfig()
    {
        // Arrange
        var json = """{"ApiUrl": "www.api.com"}""";
        var delimiter = "__";

        // Act
        var result = FlattenJson(json, delimiter);

        // Assert
        Assert.Single(result);
        Assert.Equal("ApiUrl", result[0].Name);
        Assert.Equal("www.api.com", result[0].Value);
        Assert.False(result[0].SlotSetting);
    }

    [Fact]
    public void Flatten_NestedObject_ReturnsFlattenedConfig()
    {
        // Arrange
        var json = """{"Emails": {"Admin": "admin@admin.com"}}""";
        var delimiter = "__";

        // Act
        var result = FlattenJson(json, delimiter);

        // Assert
        Assert.Single(result);
        Assert.Equal("Emails__Admin", result[0].Name);
        Assert.Equal("admin@admin.com", result[0].Value);
    }

    [Fact]
    public void Flatten_DeeplyNestedObject_ReturnsFlattenedConfig()
    {
        // Arrange
        var json = """{"Logging": {"LogLevel": {"Default": "Information"}}}""";
        var delimiter = "__";

        // Act
        var result = FlattenJson(json, delimiter);

        // Assert
        Assert.Single(result);
        Assert.Equal("Logging__LogLevel__Default", result[0].Name);
        Assert.Equal("Information", result[0].Value);
    }

    [Fact]
    public void Flatten_Array_ReturnsFlattenedConfigWithIndices()
    {
        // Arrange
        var json = """{"Items": ["item1", "item2"]}""";
        var delimiter = "__";

        // Act
        var result = FlattenJson(json, delimiter);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal("Items__0", result[0].Name);
        Assert.Equal("item1", result[0].Value);
        Assert.Equal("Items__1", result[1].Name);
        Assert.Equal("item2", result[1].Value);
    }

    [Fact]
    public void Flatten_NumberValue_ReturnsStringRepresentation()
    {
        // Arrange
        var json = """{"Port": 8080}""";
        var delimiter = "__";

        // Act
        var result = FlattenJson(json, delimiter);

        // Assert
        Assert.Single(result);
        Assert.Equal("Port", result[0].Name);
        Assert.Equal("8080", result[0].Value);
    }

    [Fact]
    public void Flatten_BooleanValue_ReturnsStringRepresentation()
    {
        // Arrange
        var json = """{"Enabled": true}""";
        var delimiter = "__";

        // Act
        var result = FlattenJson(json, delimiter);

        // Assert
        Assert.Single(result);
        Assert.Equal("Enabled", result[0].Name);
        Assert.Equal("True", result[0].Value);
    }

    [Fact]
    public void Flatten_NullValue_ReturnsEmptyString()
    {
        // Arrange
        var json = """{"Value": null}""";
        var delimiter = "__";

        // Act
        var result = FlattenJson(json, delimiter);

        // Assert
        Assert.Single(result);
        Assert.Equal("Value", result[0].Name);
        Assert.Equal(string.Empty, result[0].Value);
    }

    [Fact]
    public void Flatten_WithColonDelimiter_ReturnsFlattenedConfig()
    {
        // Arrange
        var json = """{"Emails": {"Admin": "admin@admin.com"}}""";
        var delimiter = ":";

        // Act
        var result = FlattenJson(json, delimiter);

        // Assert
        Assert.Single(result);
        Assert.Equal("Emails:Admin", result[0].Name);
        Assert.Equal("admin@admin.com", result[0].Value);
    }

    [Fact]
    public void Flatten_ComplexObject_ReturnsAllFlattenedConfigs()
    {
        // Arrange
        var json = """
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
        """;
        var delimiter = "__";

        // Act
        var result = FlattenJson(json, delimiter);

        // Assert
        Assert.Equal(4, result.Count);
        Assert.Contains(result, r => r.Name == "ApiUrl" && r.Value == "www.api.com");
        Assert.Contains(result, r => r.Name == "Emails__Admin" && r.Value == "admin@admin.com");
        Assert.Contains(result, r => r.Name == "Logging__LogLevel__Default" && r.Value == "Information");
        Assert.Contains(result, r => r.Name == "Logging__LogLevel__Microsoft.AspNetCore" && r.Value == "Warning");
    }

    [Fact]
    public void Flatten_ValueWithSpecialCharacters_EscapesCorrectly()
    {
        // Arrange
        var json = """{"Message": "Hello \"World\""}""";
        var delimiter = "__";

        // Act
        var result = FlattenJson(json, delimiter);

        // Assert
        Assert.Single(result);
        Assert.Equal("Message", result[0].Name);
        Assert.Equal("Hello \"World\"", result[0].Value);
    }

    [Fact]
    public void Flatten_EmptyObject_ReturnsEmptyList()
    {
        // Arrange
        var json = """{}""";
        var delimiter = "__";

        // Act
        var result = FlattenJson(json, delimiter);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void Flatten_EmptyArray_ReturnsEmptyList()
    {
        // Arrange
        var json = """{"Items": []}""";
        var delimiter = "__";

        // Act
        var result = FlattenJson(json, delimiter);

        // Assert
        Assert.Empty(result);
    }

    private static List<ConfigItem> FlattenJson(string jsonConfig, string delimiter)
    {
        var flattenedConfig = new List<ConfigItem>();
        using var doc = JsonDocument.Parse(jsonConfig);
        FlattenJsonObject(doc.RootElement, string.Empty, flattenedConfig, delimiter);
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
                break;
        }
    }
}

