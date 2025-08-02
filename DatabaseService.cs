using System;
using System.Data.SQLite;
using System.Security.Cryptography;
using System.Text;

namespace AuthServerTool
{
    public class DatabaseService
    {
        private const string ConnectionString = "Data Source=auth.db;Version=3;";
        private SQLiteConnection _connection;

        public void Initialize()
        {
            _connection = new SQLiteConnection(ConnectionString);
            _connection.Open();

            using var command = new SQLiteCommand(_connection);
            command.CommandText = @"
                CREATE TABLE IF NOT EXISTS users (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    username TEXT NOT NULL UNIQUE,
                    passwordHash TEXT NOT NULL
                )";
            command.ExecuteNonQuery();
        }

        public bool UserExists(string username)
        {
            using var command = new SQLiteCommand("SELECT COUNT(*) FROM users WHERE username = @username", _connection);
            command.Parameters.AddWithValue("@username", username);
            long count = (long)command.ExecuteScalar();
            return count > 0;
        }

        public void CreateUser(string username, string password)
        {
            string hash = HashPassword(password);
            using var command = new SQLiteCommand("INSERT INTO users (username, passwordHash) VALUES (@username, @passwordHash)", _connection);
            command.Parameters.AddWithValue("@username", username);
            command.Parameters.AddWithValue("@passwordHash", hash);
            command.ExecuteNonQuery();
        }

        public bool ValidateUserWithSecret(string username, string password, string secretKey)
        {
            using var command = new SQLiteCommand("SELECT passwordHash FROM users WHERE username = @username", _connection);
            command.Parameters.AddWithValue("@username", username);
            using var reader = command.ExecuteReader();

            if (!reader.Read()) return false;

            string storedHash = reader.GetString(0);
            string providedHash = HashPassword(password + secretKey);
            return storedHash == providedHash;
        }

        private string HashPassword(string input)
        {
            using var sha256 = SHA256.Create();
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
            return Convert.ToBase64String(bytes);
        }
    }
}
