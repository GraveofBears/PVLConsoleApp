#nullable enable
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Text;
using BCrypt.Net;
using AuthServerTool.Models;

namespace AuthServerTool.Services
{
    public static class UserService
    {
        // ✅ Replaced SHA256 with BCrypt password hashing
        private static string HashPassword(string input)
        {
            return BCrypt.Net.BCrypt.HashPassword(input);
        }

        private static bool UserExists(string username)
        {
            using var conn = DatabaseService.GetConnection();
            using var cmd = new SQLiteCommand("SELECT COUNT(*) FROM users WHERE username = @u;", conn);
            cmd.Parameters.AddWithValue("@u", username);
            return (long)cmd.ExecuteScalar() > 0;
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
                    Console.WriteLine($"📁 Created user folder: {userPath}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ Folder creation failed: {ex.Message}");
            }
        }

        public static bool AddNewUser(User user, string password)
        {
            if (UserExists(user.Username)) return false;

            user.PasswordHash = HashPassword(password); // ✅ bcrypt applied here
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

        // ✅ Updated login validation to use BCrypt.Verify
        public static bool ValidateUser(string username, string password)
        {
            using var conn = DatabaseService.GetConnection();
            using var cmd = new SQLiteCommand("SELECT passwordHash, isSuspended FROM users WHERE username = @u;", conn);
            cmd.Parameters.AddWithValue("@u", username);

            using var reader = cmd.ExecuteReader();
            if (!reader.Read()) return false;

            var storedHash = reader.GetString(0);
            var isSuspended = reader.GetInt32(1);

            return BCrypt.Net.BCrypt.Verify(password, storedHash) && isSuspended == 0;
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

        // ✅ Updated password update logic to use bcrypt
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
