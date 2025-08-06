using System;
using System.Data.SQLite;
using AuthServerTool.Models;

public class UserRepository
{
    private readonly string connectionString = "Data Source=users.db;Version=3;";

    public bool UserExists(string username)
    {
        using var connection = new SQLiteConnection(connectionString);
        connection.Open();

        using var command = new SQLiteCommand("SELECT COUNT(*) FROM Users WHERE Username = @Username", connection);
        command.Parameters.AddWithValue("@Username", username);

        var count = Convert.ToInt32(command.ExecuteScalar());
        return count > 0;
    }

    public void Save(User user)
    {
        using var connection = new SQLiteConnection(connectionString);
        connection.Open();

        using var command = new SQLiteCommand(@"
            INSERT INTO Users (Username, PasswordHash, Email, CustomerCode, FirstName, LastName, Company, IsSuspended)
            VALUES (@Username, @PasswordHash, @Email, @CustomerCode, @FirstName, @LastName, @Company, @IsSuspended)", connection);

        command.Parameters.AddWithValue("@Username", user.Username);
        command.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
        command.Parameters.AddWithValue("@Email", user.Email);
        command.Parameters.AddWithValue("@CustomerCode", user.CustomerCode);
        command.Parameters.AddWithValue("@FirstName", user.FirstName);
        command.Parameters.AddWithValue("@LastName", user.LastName);
        command.Parameters.AddWithValue("@Company", user.Company);
        command.Parameters.AddWithValue("@IsSuspended", user.IsSuspended);

        command.ExecuteNonQuery();
    }

    public User? GetUserByUsername(string username)
    {
        using var connection = new SQLiteConnection(connectionString);
        connection.Open();

        using var command = new SQLiteCommand(@"
            SELECT Username, PasswordHash, Email, CustomerCode, FirstName, LastName, Company, IsSuspended
            FROM Users WHERE Username = @Username", connection);

        command.Parameters.AddWithValue("@Username", username);

        using var reader = command.ExecuteReader();
        if (reader.Read())
        {
            return new User
            {
                Username = reader.GetString(0),
                PasswordHash = reader.GetString(1),
                Email = reader.GetString(2),
                CustomerCode = reader.GetString(3),
                FirstName = reader.GetString(4),
                LastName = reader.GetString(5),
                Company = reader.GetString(6),
                IsSuspended = reader.GetBoolean(7)
            };
        }

        return null;
    }

    public void UpdatePassword(string username, string newPasswordHash)
    {
        using var connection = new SQLiteConnection(connectionString);
        connection.Open();

        using var command = new SQLiteCommand("UPDATE Users SET PasswordHash = @PasswordHash WHERE Username = @Username", connection);
        command.Parameters.AddWithValue("@PasswordHash", newPasswordHash);
        command.Parameters.AddWithValue("@Username", username);

        command.ExecuteNonQuery();
    }
}
