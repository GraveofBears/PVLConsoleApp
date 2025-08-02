namespace AuthServerTool.Utils
{
    public static class CryptoLib
    {
        public static string HashPassword(string input)
        {
            using var sha256 = System.Security.Cryptography.SHA256.Create();
            var bytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(input));
            return Convert.ToBase64String(bytes);
        }

        public static string Encrypt(string input)
        {
            return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(input));
        }
    }
}
