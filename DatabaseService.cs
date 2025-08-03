#nullable enable
using System;
using System.Data.SQLite;
using System.IO;
using System.Collections.Generic;

namespace AuthServerTool
{
    public static class DatabaseService
    {
        private const string DatabaseFile = "auth.db";
        private const string ConnectionString = "Data Source=auth.db;Version=3;";

        public static void Initialize()
        {
            if (!File.Exists(DatabaseFile))
            {
                SQLiteConnection.CreateFile(DatabaseFile);
                Console.WriteLine("📂 Database file created.");
            }

            using var conn = new SQLiteConnection(ConnectionString);
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
            var conn = new SQLiteConnection(ConnectionString);
            conn.Open();

            // ⚙️ Recommended for consistency during read/write
            using var cmd = new SQLiteCommand("PRAGMA journal_mode=WAL;", conn);
            cmd.ExecuteNonQuery();

            return conn;
        }
    }
}
#nullable restore
