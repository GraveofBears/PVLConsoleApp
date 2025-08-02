#nullable enable
using System;
using System.IO;
using System.Text.Json;
using AuthServerTool.Models;

namespace AuthServerTool
{
    public class SessionService
    {
        private readonly string _userFolder;
        private readonly string _sessionFile;

        public SessionService(string userFolder)
        {
            _userFolder = userFolder;
            _sessionFile = Path.Combine(_userFolder, "session.json");

            Directory.CreateDirectory(_userFolder); // Ensure path exists
        }

        public void CreateSession(User user)
        {
            var sessionData = new Session
            {
                Username = user.Username,
                LoginTime = DateTime.UtcNow
            };

            string json = JsonSerializer.Serialize(sessionData, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_sessionFile, json);

            Console.WriteLine($"🗂️ Session created at {_sessionFile}");
        }

        public Session? GetSession()
        {
            if (!File.Exists(_sessionFile))
                return null;

            try
            {
                string json = File.ReadAllText(_sessionFile);
                return JsonSerializer.Deserialize<Session>(json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ Failed to load session: {ex.Message}");
                return null;
            }
        }

        public void EndSession()
        {
            if (File.Exists(_sessionFile))
            {
                try
                {
                    File.Delete(_sessionFile);
                    Console.WriteLine("🧹 Session ended.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"⚠️ Failed to delete session: {ex.Message}");
                }
            }
        }
    }

    public class Session
    {
        public required string Username { get; set; }
        public DateTime LoginTime { get; set; }
    }
}
#nullable restore
