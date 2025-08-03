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
        // 🔐 Hash the password using SHA256
        private static string HashPassword(string input)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
            return Convert.ToBase64String(bytes);
        }

        // 🔍 Check if username already exists
        private static bool UserExists(string username)
        {
            using var conn = DatabaseService.GetConnection();
            using var cmd = new SQLiteCommand("SELECT COUNT(*) FROM users WHERE username = @u;", conn);
            cmd.Parameters.AddWithValue("@u", username);
            return (long)cmd.ExecuteScalar() > 0;
        }

        // 📁 Create user-specific folder if not already present
        private static void TryCreateUserFolder(string username)
        {
            var root = ConfigService.Get("UserDataRoot") ?? "UserData";
            var userPath = Path.Combine(root, username);

            try
            {
                if (!Directory.Exists(userPath))
                {
                    Directory.CreateDirectory(userPath);
                    Console.WriteLine($"📁 Created user folder: {userPath}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ Folder creation failed: {ex.Message}");
            }
        }

        // ➕ Register new user
        public static bool AddNewUser(User user, string password)
        {
            if (UserExists(user.Username)) return false;

            user.PasswordHash = HashPassword(password);
            user.CreatedAt = user.CreatedAt == default ? DateTime.UtcNow : user.CreatedAt;

            using var conn = DatabaseService.GetConnection();
            using var cmd = new SQLiteCommand(@"
                INSERT INTO users (
                    username, passwordHash, email,
                    customerCode, firstName, lastName, company,
                    createdAt, isSuspended
                ) VALUES (
                    @u, @p, @e,
                    @cc, @fn, @ln, @comp,
                    @created, 0
                );", conn);

            cmd.Parameters.AddWithValue("@u", user.Username);
            cmd.Parameters.AddWithValue("@p", user.PasswordHash);
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

        // ✅ Validate login
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

        // 🔎 Retrieve all users
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
                        Username = reader["username"].ToString() ?? "",
                        Email = reader["email"].ToString() ?? "",
                        CustomerCode = reader["customerCode"].ToString() ?? "",
                        FirstName = reader["firstName"].ToString() ?? "",
                        LastName = reader["lastName"].ToString() ?? "",
                        Company = reader["company"].ToString() ?? "",                        
                        PasswordHash = reader["passwordHash"].ToString() ?? "",
                        CreatedAt = DateTime.TryParse(reader["createdAt"].ToString(), out var dt) ? dt : DateTime.UtcNow,
                        IsSuspended = Convert.ToInt32(reader["isSuspended"]) == 1
                    };
                    users.Add(user);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"⚠️ Skipped row: {ex.Message}");
                }
            }

            return users;
        }

        // ✏️ Update user metadata
        public static bool EditUser(string username, string? newEmail = null, string? newCustomerCode = null, string? newCompany = null)
        {
            using var conn = DatabaseService.GetConnection();
            using var cmd = new SQLiteCommand(@"
                UPDATE users 
                SET 
                    email = COALESCE(@e, email),
                    customerCode = COALESCE(@cc, customerCode),
                    company = COALESCE(@comp, company)
                WHERE username = @u;", conn);

            cmd.Parameters.AddWithValue("@u", username);
            cmd.Parameters.AddWithValue("@e", (object?)newEmail ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@cc", (object?)newCustomerCode ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@comp", (object?)newCompany ?? DBNull.Value);

            return cmd.ExecuteNonQuery() > 0;
        }

        // ✏️ Update user's full name
        public static bool UpdateName(string username, string firstName, string lastName)
        {
            using var conn = DatabaseService.GetConnection();
            using var cmd = new SQLiteCommand(@"
                UPDATE users 
                SET firstName = @fn, lastName = @ln 
                WHERE username = @u;", conn);

            cmd.Parameters.AddWithValue("@u", username);
            cmd.Parameters.AddWithValue("@fn", firstName);
            cmd.Parameters.AddWithValue("@ln", lastName);

            return cmd.ExecuteNonQuery() > 0;
        }

        // 🔑 Change password
        public static bool UpdatePassword(string username, string newPassword)
        {
            if (!UserExists(username)) return false;

            var hash = HashPassword(newPassword);
            using var conn = DatabaseService.GetConnection();
            using var cmd = new SQLiteCommand("UPDATE users SET passwordHash = @p WHERE username = @u;", conn);

            cmd.Parameters.AddWithValue("@u", username);
            cmd.Parameters.AddWithValue("@p", hash);

            return cmd.ExecuteNonQuery() > 0;
        }

        // 🚫 Suspend a user
        public static bool SuspendUser(string username)
        {
            using var conn = DatabaseService.GetConnection();
            using var cmd = new SQLiteCommand("UPDATE users SET isSuspended = 1 WHERE username = @u;", conn);
            cmd.Parameters.AddWithValue("@u", username);
            return cmd.ExecuteNonQuery() > 0;
        }

        // ✅ Unsuspend a user
        public static bool UnsuspendUser(string username)
        {
            using var conn = DatabaseService.GetConnection();
            using var cmd = new SQLiteCommand("UPDATE users SET isSuspended = 0 WHERE username = @u;", conn);
            cmd.Parameters.AddWithValue("@u", username);
            return cmd.ExecuteNonQuery() > 0;
        }

        // 🧹 Delete user and folder
        public static bool DeleteUser(string username)
        {
            using var conn = DatabaseService.GetConnection();
            using var cmd = new SQLiteCommand("DELETE FROM users WHERE username = @u;", conn);
            cmd.Parameters.AddWithValue("@u", username);
            var rows = cmd.ExecuteNonQuery();

            var root = ConfigService.Get("UserDataRoot") ?? "UserData";
            var userPath = Path.Combine(root, username);

            try
            {
                if (Directory.Exists(userPath))
                {
                    Directory.Delete(userPath, true);
                    Console.WriteLine($"🧹 Deleted folder: {userPath}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ Folder deletion failed: {ex.Message}");
            }

            return rows > 0;
        }
    }
}
#nullable restore
