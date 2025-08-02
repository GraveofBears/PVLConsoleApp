namespace ConsoleApp.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }

        public User() { }

        public User(int id, string username, string passwordHash)
        {
            Id = id;
            Username = username;
            PasswordHash = passwordHash;
        }
    }
}
