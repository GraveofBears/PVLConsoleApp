using System;
using System.Data.SQLite;
using AuthServerTool.Models;



public class UserRepository
{
    private readonly string connectionString = "Data Source=users.db;Version=3;";

    public User GetUserByUsername(string username)
    {
        using (var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();
            using (var command = new SQLiteCommand("SELECT Username, PasswordHash, IsSuspended FROM Users WHERE Username = @Username", connection))
            {
                command.Parameters.AddWithValue("@Username", username);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new User
                        {
                            Username = reader.GetString(0),
                            PasswordHash = reader.GetString(1),
                            IsSuspended = reader.GetBoolean(3)
                        };
                    }
                }
            }
        }

        return null; // User not found
    }
}
