using System.IO;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Coswalt.Config;

public static class Config
{
    private static readonly string ConfigPath =
        Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "config.json");

    public static string? GetValue(string key) {
        try {
            if (!File.Exists(ConfigPath)) {
                throw new FileNotFoundException($"Configuration file not found: {ConfigPath}.");
            }

            string jsonContent = File.ReadAllText(ConfigPath);
            var configData = JsonSerializer.Deserialize<JsonObject>(jsonContent);
            if (configData is null || !configData.ContainsKey(key)) {
                throw new KeyNotFoundException($"Key {key} not found in the configuration file.");
            }

            return configData[key].ToString();
        } catch (Exception ex) {
            Console.WriteLine($"Error reading config: {ex.Message}.");
            return null;
        }
    }
}