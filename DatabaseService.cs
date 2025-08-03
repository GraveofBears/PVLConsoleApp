#nullable enable
using System;
using System.Data.SQLite;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;

namespace AuthServerTool
{
    public static class DatabaseService
    {
        private static string _databasePath = "auth.db";
        private static string _connectionString = $"Data Source={_databasePath};Version=3;";
        public static Action<string>? OnNotify;

        static DatabaseService()
        {
            var configPath = Path.Combine(AppContext.BaseDirectory, "config.json");
            if (File.Exists(configPath))
            {
                try
                {
                    var json = File.ReadAllText(configPath);
                    var config = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
                    if (config != null && config.TryGetValue("DatabasePath", out string? path) && !string.IsNullOrWhiteSpace(path))
                    {
                        SetDatabasePath(path);
                        Initialize();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"⚠️ Failed to load config.json: {ex.Message}");
                }
            }
        }

        public static void Initialize()
        {
            if (!File.Exists(_databasePath))
            {
                SQLiteConnection.CreateFile(_databasePath);
                Console.WriteLine("📂 Database file created.");
            }

            using var conn = new SQLiteConnection(_connectionString);
            conn.Open();
            using var cmd = new SQLiteCommand(conn);

            // 👤 Create 'users' table
            cmd.CommandText = @"
                CREATE TABLE IF NOT EXISTS users (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    username TEXT NOT NULL UNIQUE,
                    passwordHash TEXT NOT NULL,
                    email TEXT NOT NULL,
                    isSuspended INTEGER DEFAULT 0,
                    createdAt TEXT DEFAULT CURRENT_TIMESTAMP
                );";
            cmd.ExecuteNonQuery();

            // 📦 Create 'sessions' table
            cmd.CommandText = @"
                CREATE TABLE IF NOT EXISTS sessions (
                    sessionId TEXT PRIMARY KEY,
                    username TEXT NOT NULL,
                    timestamp TEXT NOT NULL,
                    token TEXT NOT NULL
                );";
            cmd.ExecuteNonQuery();

            // 🔍 Patch missing columns in 'users'
            var requiredColumns = new[] { "customerCode", "company", "firstName", "lastName", "isSuspended" };
            cmd.CommandText = "PRAGMA table_info(users);";
            using var reader = cmd.ExecuteReader();
            var existingColumns = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            while (reader.Read())
            {
                if (reader["name"] is string name)
                    existingColumns.Add(name);
            }

            reader.Close();

            foreach (var column in requiredColumns)
            {
                if (!existingColumns.Contains(column))
                {
                    string columnType = column == "isSuspended" ? "INTEGER DEFAULT 0" : "TEXT";
                    cmd.CommandText = $"ALTER TABLE users ADD COLUMN {column} {columnType};";
                    cmd.ExecuteNonQuery();
                    Console.WriteLine($"🔧 Added missing column: {column}");
                }
            }
        }

        public static SQLiteConnection GetConnection()
        {
            var conn = new SQLiteConnection(_connectionString);
            conn.Open();

            using var cmd = new SQLiteCommand("PRAGMA journal_mode=WAL;", conn);
            cmd.ExecuteNonQuery();

            return conn;
        }

        public static void SetDatabasePath(string newPath)
        {
            if (Directory.Exists(newPath))
            {
                newPath = Path.Combine(newPath, "auth.db");
            }
            else if (!File.Exists(newPath))
            {
                var dir = Path.GetDirectoryName(newPath);
                if (!string.IsNullOrWhiteSpace(dir) && !Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
            }

            _databasePath = newPath;
            _connectionString = $"Data Source={_databasePath};Version=3;";

            // 🔄 Create DB if missing
            if (!File.Exists(_databasePath))
            {
                SQLiteConnection.CreateFile(_databasePath);
                Console.WriteLine($"📂 Created blank DB at: {_databasePath}");
                OnNotify?.Invoke($"Blank database created at:\n{_databasePath}");
            }
        }

        public static string GetDatabasePath() => _databasePath;
    }
}
#nullable restore
