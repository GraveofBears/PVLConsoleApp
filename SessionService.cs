using System;
using System.IO;
using System.Text.Json;
using ConsoleApp.Models;

namespace AuthServerTool
{
    public class SessionService
    {
        private readonly string _userFolder;

        public SessionService(string userFolder)
        {
            _userFolder = userFolder;
        }

        public void CreateSession(User user)
        {
            string sessionPath = GetSessionFilePath();
            var sessionData = new Session
            {
                Username = user.Username,
                LoginTime = DateTime.UtcNow
            };

            string json = JsonSerializer.Serialize(sessionData, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(sessionPath, json);

            Console.WriteLine($"🗂️ Session created at {sessionPath}");
        }

        public Session GetSession()
        {
            string sessionPath = GetSessionFilePath();
            if (!File.Exists(sessionPath))
                return null;

            string json = File.ReadAllText(sessionPath);
            return JsonSerializer.Deserialize<Session>(json);
        }

        public void EndSession()
        {
            string sessionPath = GetSessionFilePath();
            if (File.Exists(sessionPath))
            {
                File.Delete(sessionPath);
                Console.WriteLine("🧹 Session ended.");
            }
        }

        private string GetSessionFilePath()
        {
            return Path.Combine(_userFolder, "session.json");
        }
    }

    public class Session
    {
        public string Username { get; set; }
        public DateTime LoginTime { get; set; }
    }
}
