using System;
using System.IO;
using ConsoleApp.Models;

namespace AuthServerTool
{
    public class UserService
    {
        private readonly DatabaseService _db;
        private readonly string _baseFolder;

        public UserService(DatabaseService db, string baseFolder)
        {
            _db = db;
            _baseFolder = baseFolder;
            _db.Initialize();

            EnsureBaseFolderExists();
        }

        public void Register(string username, string password)
        {
            if (_db.UserExists(username))
            {
                Console.WriteLine($"⚠️ User '{username}' already exists.");
                return;
            }

            _db.CreateUser(username, password);
            string userFolder = GetUserFolderPath(username);
            Directory.CreateDirectory(userFolder);

            Console.WriteLine($"✅ Registered user '{username}' and created folder at: {userFolder}");
        }

        public User Login(string username, string password, string secretKey)
        {
            bool isValid = _db.ValidateUserWithSecret(username, password, secretKey);
            if (!isValid)
            {
                Console.WriteLine("❌ Invalid credentials.");
                return null;
            }

            string userFolder = GetUserFolderPath(username);
            if (!Directory.Exists(userFolder))
            {
                Directory.CreateDirectory(userFolder);
                Console.WriteLine($"📁 Folder created for user: {username}");
            }

            Console.WriteLine($"✅ Login successful for '{username}'");

            return new User
            {
                Username = username,
                PasswordHash = "N/A" // This can be skipped or replaced with actual hash if needed
            };
        }

        private void EnsureBaseFolderExists()
        {
            if (!Directory.Exists(_baseFolder))
            {
                Directory.CreateDirectory(_baseFolder);
                Console.WriteLine($"📁 Created base folder: {_baseFolder}");
            }
        }

        private string GetUserFolderPath(string username)
        {
            return Path.Combine(_baseFolder, username);
        }
    }
}
