using System;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;

namespace AuthServerTool.Services
{
    public static class ConfigService
    {
        private const string ConfigFile = "appsettings.json";

        // 🔑 Save encrypted API key
        public static void SaveKey(string encryptedKey)
        {
            var config = LoadConfig();
            config["ApiKey"] = encryptedKey;
            SaveConfig(config);
        }

        // 🔑 Load encrypted API key
        public static string LoadKey()
        {
            var config = LoadConfig();
            return config.TryGetValue("ApiKey", out var key) ? key : string.Empty;
        }

        // 📁 Save user root folder path
        public static void SaveFolderPath(string path)
        {
            var config = LoadConfig();
            config["UserFolderPath"] = path;
            SaveConfig(config);
        }

        // 📁 Load user root folder path
        public static string LoadFolderPath()
        {
            var config = LoadConfig();
            return config.TryGetValue("UserFolderPath", out var path) ? path : string.Empty;
        }

        // 💾 Save database path
        public static void SaveDatabasePath(string path)
        {
            var config = LoadConfig();
            config["DatabasePath"] = path;
            SaveConfig(config);
        }

        // 💾 Load database path
        public static string LoadDatabasePath()
        {
            var config = LoadConfig();
            return config.TryGetValue("DatabasePath", out var path) ? path : string.Empty;
        }

        // 🧠 Generic get
        public static string Get(string key)
        {
            var config = LoadConfig();
            return config.TryGetValue(key, out var value) ? value : string.Empty;
        }

        // 🧠 Generic set
        public static void Set(string key, string value)
        {
            var config = LoadConfig();
            config[key] = value;
            SaveConfig(config);
        }

        // 🛠 Load config file
        private static Dictionary<string, string> LoadConfig()
        {
            if (!File.Exists(ConfigFile))
                return new Dictionary<string, string>();

            var json = File.ReadAllText(ConfigFile);
            return JsonSerializer.Deserialize<Dictionary<string, string>>(json) ?? new Dictionary<string, string>();
        }

        // 🛠 Save config file
        private static void SaveConfig(Dictionary<string, string> config)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize(config, options);
            File.WriteAllText(ConfigFile, json);
        }
    }
}
