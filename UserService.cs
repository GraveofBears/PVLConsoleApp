#nullable enable
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using PVLConsoleApp.Models;
using PVLConsoleApp.Utils;

namespace PVLConsoleApp.Services
{
    public static class UserService
    {
        public static List<User> GetAllUsers()
        {
            var users = new List<User>();
            using var conn = DatabaseService.GetConnection();
            using var cmd = new SQLiteCommand("SELECT * FROM users", conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                users.Add(new User
                {
                    Username = reader["username"].ToString() ?? "",
                    FirstName = reader["firstName"].ToString() ?? "",
                    LastName = reader["lastName"].ToString() ?? "",
                    Email = reader["email"].ToString() ?? "",
                    CustomerCode = reader["customerCode"].ToString() ?? "",
                    Company = reader["company"].ToString() ?? "",
                    IsSuspended = Convert.ToBoolean(reader["isSuspended"])
                });
            }

            return users;
        }

        public static User? GetUser(string username)
        {
            using var conn = DatabaseService.GetConnection();
            using var cmd = new SQLiteCommand("SELECT * FROM users WHERE username = @username", conn);
            cmd.Parameters.AddWithValue("@username", username);
            using var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                return new User
                {
                    Username = reader["username"].ToString() ?? "",
                    FirstName = reader["firstName"].ToString() ?? "",
                    LastName = reader["lastName"].ToString() ?? "",
                    Email = reader["email"].ToString() ?? "",
                    CustomerCode = reader["customerCode"].ToString() ?? "",
                    Company = reader["company"].ToString() ?? "",
                    IsSuspended = Convert.ToBoolean(reader["isSuspended"])
                };
            }

            return null;
        }

        public static bool AddNewUser(User user, string plainPassword)
        {
            var hashedPassword = CryptoLib.HashPassword(plainPassword);
            using var conn = DatabaseService.GetConnection();
            using var cmd = new SQLiteCommand(@"
                INSERT INTO users (username, firstName, lastName, email, customerCode, company, passwordHash, isSuspended)
                VALUES (@username, @firstName, @lastName, @email, @customerCode, @company, @passwordHash, 0)", conn);

            cmd.Parameters.AddWithValue("@username", user.Username);
            cmd.Parameters.AddWithValue("@firstName", user.FirstName);
            cmd.Parameters.AddWithValue("@lastName", user.LastName);
            cmd.Parameters.AddWithValue("@email", user.Email);
            cmd.Parameters.AddWithValue("@customerCode", user.CustomerCode);
            cmd.Parameters.AddWithValue("@company", user.Company);
            cmd.Parameters.AddWithValue("@passwordHash", hashedPassword);

            return cmd.ExecuteNonQuery() > 0;
        }

        public static bool UpdateUser(User user, string? newPassword = null)
        {
            using var conn = DatabaseService.GetConnection();
            var sql = @"
                UPDATE users SET
                    firstName = @firstName,
                    lastName = @lastName,
                    email = @email,
                    customerCode = @customerCode,
                    company = @company";

            if (!string.IsNullOrWhiteSpace(newPassword))
            {
                sql += ", passwordHash = @passwordHash";
            }

            sql += " WHERE username = @username";

            using var cmd = new SQLiteCommand(sql, conn);
            cmd.Parameters.AddWithValue("@username", user.Username);
            cmd.Parameters.AddWithValue("@firstName", user.FirstName);
            cmd.Parameters.AddWithValue("@lastName", user.LastName);
            cmd.Parameters.AddWithValue("@email", user.Email);
            cmd.Parameters.AddWithValue("@customerCode", user.CustomerCode);
            cmd.Parameters.AddWithValue("@company", user.Company);

            if (!string.IsNullOrWhiteSpace(newPassword))
            {
                var hashed = CryptoLib.HashPassword(newPassword);
                cmd.Parameters.AddWithValue("@passwordHash", hashed);
            }

            return cmd.ExecuteNonQuery() > 0;
        }

        public static bool SuspendUser(string username)
        {
            using var conn = DatabaseService.GetConnection();
            using var cmd = new SQLiteCommand("UPDATE users SET isSuspended = 1 WHERE username = @username", conn);
            cmd.Parameters.AddWithValue("@username", username);
            return cmd.ExecuteNonQuery() > 0;
        }

        public static bool UnsuspendUser(string username)
        {
            using var conn = DatabaseService.GetConnection();
            using var cmd = new SQLiteCommand("UPDATE users SET isSuspended = 0 WHERE username = @username", conn);
            cmd.Parameters.AddWithValue("@username", username);
            return cmd.ExecuteNonQuery() > 0;
        }

        public static bool DeleteUser(string username)
        {
            using var conn = DatabaseService.GetConnection();
            using var cmd = new SQLiteCommand("DELETE FROM users WHERE username = @username", conn);
            cmd.Parameters.AddWithValue("@username", username);
            return cmd.ExecuteNonQuery() > 0;
        }
    }
}
#nullable restore
