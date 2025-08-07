using BCrypt.Net;

namespace PVLConsoleApp.Utils
{
    public static class CryptoLib
    {
        // 🔐 Hash password using bcrypt
        public static string HashPassword(string plainPassword)
        {
            return BCrypt.Net.BCrypt.HashPassword(plainPassword);
        }

        // 🔍 Verify password against hash
        public static bool VerifyPassword(string plainPassword, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(plainPassword, hashedPassword);
        }
    }
}
