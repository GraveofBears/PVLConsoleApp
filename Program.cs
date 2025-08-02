using System;
using System.IO;
using Sodium;
using Microsoft.Extensions.Configuration;
using AuthServerTool;
using AuthServerTool.Utils;
using ConsoleApp.Models;

namespace AuthServerTool
{
    class Program
    {
        static void Main(string[] args)
        {
            // Load configuration
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            string baseFolder = config["StorageRoot"];
            string secretKey = config["SecretKey"];

            if (string.IsNullOrWhiteSpace(baseFolder) || string.IsNullOrWhiteSpace(secretKey))
            {
                Console.WriteLine("⚠️ Missing configuration values in appsettings.json.");
                return;
            }

            var db = new DatabaseService();
            var userService = new UserService(db, baseFolder);

            Console.WriteLine("🔐 Local Auth Server Tool Started");

            // Ensure base folder exists
            if (!Directory.Exists(baseFolder))
            {
                Directory.CreateDirectory(baseFolder);
                Console.WriteLine($"📁 Created base storage folder: {baseFolder}");
            }

            while (true)
            {
                Console.WriteLine("\nSelect an option:");
                Console.WriteLine("1. Register a new user");
                Console.WriteLine("2. Login");
                Console.WriteLine("3. Exit");
                Console.Write("Choice: ");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.Write("Enter username: ");
                        var newUsername = Console.ReadLine();
                        Console.Write("Enter password: ");
                        var newPassword = ReadSecureLine();

                        userService.Register(newUsername, newPassword);
                        break;

                    case "2":
                        Console.Write("Enter username: ");
                        var loginUsername = Console.ReadLine();
                        Console.Write("Enter password: ");
                        var loginPassword = ReadSecureLine();

                        var user = userService.Login(loginUsername, loginPassword, secretKey);
                        if (user is not null)
                        {
                            string userFolder = Path.Combine(baseFolder, user.Username);
                            if (!Directory.Exists(userFolder))
                            {
                                Directory.CreateDirectory(userFolder);
                                Console.WriteLine($"📁 Folder created for user: {user.Username}");
                            }
                            else
                            {
                                Console.WriteLine($"📁 Folder already exists for user: {user.Username}");
                            }

                            // 💾 Create session
                            var sessionService = new SessionService(userFolder);
                            sessionService.CreateSession(user);

                            Console.WriteLine("🔐 Session stored. Files will route to your folder.");
                        }
                        break;

                    case "3":
                        Console.WriteLine("👋 Exiting...");
                        return;

                    default:
                        Console.WriteLine("❓ Invalid selection.");
                        break;
                }
            }
        }

        static string ReadSecureLine()
        {
            string input = "";
            ConsoleKeyInfo key;
            do
            {
                key = Console.ReadKey(true);
                if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                {
                    input += key.KeyChar;
                    Console.Write("*");
                }
                else if (key.Key == ConsoleKey.Backspace && input.Length > 0)
                {
                    input = input[..^1];
                    Console.Write("\b \b");
                }
            } while (key.Key != ConsoleKey.Enter);
            Console.WriteLine();
            return input;
        }
    }
}
