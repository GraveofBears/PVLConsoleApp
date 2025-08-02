using System.IO;
using System.Text.Json;
using System.Collections.Generic;

namespace AuthServerTool.Services
{
    public static class ConfigService
    {
        private const string ConfigFile = "appsettings.json";

        // ✅ Save encrypted API key to config
        public static void SaveKey(string encryptedKey)
        {
            var config = LoadConfig();
            config["ApiKey"] = encryptedKey;
            SaveConfig(config);
        }

        // ✅ Retrieve encrypted API key
        public static string LoadKey()
        {
            var config = LoadConfig();
            return config.TryGetValue("ApiKey", out var key) ? key : string.Empty;
        }

        // 📂 Save root folder path for user directories
        public static void SaveFolderPath(string path)
        {
            var config = LoadConfig();
            config["UserFolderPath"] = path;
            SaveConfig(config);
        }

        // 📂 Load root folder path
        public static string LoadFolderPath()
        {
            var config = LoadConfig();
            return config.TryGetValue("UserFolderPath", out var path) ? path : string.Empty;
        }

        // 🧠 NEW: Generic config getter
        public static string Get(string key)
        {
            var config = LoadConfig();
            return config.TryGetValue(key, out var value) ? value : string.Empty;
        }

        // 🧠 Internal helpers
        private static Dictionary<string, string> LoadConfig()
        {
            if (!File.Exists(ConfigFile))
                return new Dictionary<string, string>();

            var json = File.ReadAllText(ConfigFile);
            return JsonSerializer.Deserialize<Dictionary<string, string>>(json) ?? new();
        }

        private static void SaveConfig(Dictionary<string, string> config)
        {
            var json = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(ConfigFile, json);
        }
    }
}
