#nullable enable
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using AuthServerTool.Models;

namespace AuthServerTool.Services
{
    public static class UserService
    {
        public static bool AddNewUser(User user, string password)
        {
            if (UserExists(user.Username)) return false;

            var hashed = HashPassword(password);
            using var conn = DatabaseService.GetConnection();
            using var cmd = new SQLiteCommand(@"
                INSERT INTO users (
                    username, passwordHash, accessLevel, email,
                    customerCode, firstName, lastName, company,
                    createdAt, isSuspended
                ) VALUES (
                    @u, @p, @a, @e,
                    @cc, @fn, @ln, @comp,
                    @created, 0
                );", conn);

            cmd.Parameters.AddWithValue("@u", user.Username);
            cmd.Parameters.AddWithValue("@p", hashed);
            cmd.Parameters.AddWithValue("@a", user.AccessLevel);
            cmd.Parameters.AddWithValue("@e", user.Email);
            cmd.Parameters.AddWithValue("@cc", user.CustomerCode);
            cmd.Parameters.AddWithValue("@fn", user.FirstName);
            cmd.Parameters.AddWithValue("@ln", user.LastName);
            cmd.Parameters.AddWithValue("@comp", user.Company);
            cmd.Parameters.AddWithValue("@created", user.CreatedAt);

            cmd.ExecuteNonQuery();
            TryCreateUserFolder(user.Username);
            return true;
        }

        public static bool UserExists(string username)
        {
            using var conn = DatabaseService.GetConnection();
            using var cmd = new SQLiteCommand("SELECT COUNT(*) FROM users WHERE username = @u;", conn);
            cmd.Parameters.AddWithValue("@u", username);
            return (long)cmd.ExecuteScalar() > 0;
        }

        public static bool ValidateUser(string username, string password)
        {
            using var conn = DatabaseService.GetConnection();
            using var cmd = new SQLiteCommand("SELECT passwordHash, isSuspended FROM users WHERE username = @u;", conn);
            cmd.Parameters.AddWithValue("@u", username);

            using var reader = cmd.ExecuteReader();
            if (!reader.Read()) return false;

            var storedHash = reader.GetString(0);
            var isSuspended = reader.GetInt32(1);
            return storedHash == HashPassword(password) && isSuspended == 0;
        }

        public static List<User> GetAllUsers()
        {
            var users = new List<User>();
            using var conn = DatabaseService.GetConnection();
            using var cmd = new SQLiteCommand("SELECT * FROM users;", conn);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                try
                {
                    var user = new User
                    {
                        Username = reader["username"].ToString() ?? string.Empty,
                        AccessLevel = reader["accessLevel"].ToString() ?? "user",
                        Email = reader["email"].ToString() ?? string.Empty,
                        CustomerCode = reader["customerCode"].ToString() ?? string.Empty,
                        FirstName = reader["firstName"].ToString() ?? string.Empty,
                        LastName = reader["lastName"].ToString() ?? string.Empty,
                        Company = reader["company"].ToString() ?? string.Empty,
                        CreatedAt = DateTime.TryParse(reader["createdAt"].ToString(), out var created) ? created : DateTime.UtcNow,
                        IsSuspended = Convert.ToInt32(reader["isSuspended"]) == 1
                    };
                    users.Add(user);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"⚠️ Skipped user row due to parse error: {ex.Message}");
                }
            }

            Console.WriteLine($"✅ Loaded {users.Count} users from DB.");
            return users;
        }

        public static bool EditUser(string username, string? newEmail = null, string? newAccessLevel = null, string? newCustomerCode = null, string? newCompany = null)
        {
            using var conn = DatabaseService.GetConnection();
            using var cmd = new SQLiteCommand(@"
                UPDATE users 
                SET 
                    email = COALESCE(@e, email), 
                    accessLevel = COALESCE(@a, accessLevel),
                    customerCode = COALESCE(@cc, customerCode),
                    company = COALESCE(@comp, company)
                WHERE username = @u;", conn);

            cmd.Parameters.AddWithValue("@u", username);
            cmd.Parameters.AddWithValue("@e", (object?)newEmail ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@a", (object?)newAccessLevel ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@cc", (object?)newCustomerCode ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@comp", (object?)newCompany ?? DBNull.Value);
            return cmd.ExecuteNonQuery() > 0;
        }

        public static bool UpdatePassword(string username, string newPassword)
        {
            if (!UserExists(username)) return false;

            var hashed = HashPassword(newPassword);
            using var conn = DatabaseService.GetConnection();
            using var cmd = new SQLiteCommand("UPDATE users SET passwordHash = @p WHERE username = @u;", conn);
            cmd.Parameters.AddWithValue("@u", username);
            cmd.Parameters.AddWithValue("@p", hashed);
            return cmd.ExecuteNonQuery() > 0;
        }

        public static bool SuspendUser(string username)
        {
            using var conn = DatabaseService.GetConnection();
            using var cmd = new SQLiteCommand("UPDATE users SET isSuspended = 1 WHERE username = @u;", conn);
            cmd.Parameters.AddWithValue("@u", username);
            return cmd.ExecuteNonQuery() > 0;
        }

        public static bool UnsuspendUser(string username)
        {
            using var conn = DatabaseService.GetConnection();
            using var cmd = new SQLiteCommand("UPDATE users SET isSuspended = 0 WHERE username = @u;", conn);
            cmd.Parameters.AddWithValue("@u", username);
            return cmd.ExecuteNonQuery() > 0;
        }

        public static bool DeleteUser(string username)
        {
            using var conn = DatabaseService.GetConnection();
            using var cmd = new SQLiteCommand("DELETE FROM users WHERE username = @u;", conn);
            cmd.Parameters.AddWithValue("@u", username);
            var rows = cmd.ExecuteNonQuery();

            var userDir = Path.Combine(ConfigService.Get("UserDataRoot") ?? "UserData", username);
            if (Directory.Exists(userDir))
            {
                Directory.Delete(userDir, true);
                Console.WriteLine($"🧹 Deleted user folder: {userDir}");
            }

            return rows > 0;
        }

        private static string HashPassword(string input)
        {
            using var sha256 = SHA256.Create();
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
            return Convert.ToBase64String(bytes);
        }

        private static void TryCreateUserFolder(string username)
        {
            var root = ConfigService.Get("UserDataRoot") ?? "UserData";
            var userPath = Path.Combine(root, username);

            try
            {
                if (!Directory.Exists(userPath))
                {
                    Directory.CreateDirectory(userPath);
                    Console.WriteLine($"📁 Created user folder at: {userPath}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ Could not create user folder: {ex.Message}");
            }
        }
    }
}
#nullable restore
