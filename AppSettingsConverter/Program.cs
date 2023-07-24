using System.Text.Json;

namespace AppSettingsConverter
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Please provide the path to the JSON file and the delimiter as command-line arguments.");
                return;
            }

            var filePath = args[0];
            var delimiter = args[1];

            if (!File.Exists(filePath))
            {
                Console.WriteLine($"File not found: {filePath}");
                return;
            }

            var jsonConfig = File.ReadAllText(filePath);

            var flattenedConfig = FlattenConfig(jsonConfig, delimiter);

            foreach (var item in flattenedConfig)
            {
                Console.WriteLine("  {");
                Console.WriteLine($"     \"name\": \"{item.Name}\",");
                Console.WriteLine($"     \"value\": \"{item.Value}\",");
                Console.WriteLine($"     \"slotSetting\": {item.SlotSetting.ToString().ToLower()}");
                Console.WriteLine("  },");
            }
        }

        private static List<ConfigItem> FlattenConfig(string jsonConfig, string delimiter)
        {
            var flattenedConfig = new List<ConfigItem>();
            try
            {
                var doc = JsonDocument.Parse(jsonConfig);
                FlattenJsonObject(doc.RootElement, string.Empty, flattenedConfig, delimiter);
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error parsing JSON: {ex.Message}");
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
                        var currentKey = string.IsNullOrEmpty(parentKey) ? property.Name : $"{parentKey}{delimiter}{property.Name}";
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
                        Value = jsonElement.GetString(),
                        SlotSetting = false // Set this to true/false based on your requirements
                    });
                    break;
                case JsonValueKind.Number:
                case JsonValueKind.True:
                case JsonValueKind.False:
                    flattenedConfig.Add(new ConfigItem
                    {
                        Name = parentKey,
                        Value = jsonElement.ToString(),
                        SlotSetting = false // Set this to true/false based on your requirements
                    });
                    break;
                default:
                    // Ignore other value types for this
                    break;
            }
        }
    }

    public class ConfigItem
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public bool SlotSetting { get; set; }
    }
}
