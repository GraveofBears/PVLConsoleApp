namespace AuthServerTool.Models
{
#nullable enable
    public class User
    {
        public int Id { get; set; }

        // Required identity fields
        public required string Username { get; set; }
        public required string CustomerCode { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string Company { get; set; }
        public required string AccessLevel { get; set; }

        // Lifecycle metadata
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // 🚫 Suspension flag (0 = active, 1 = suspended)
        public bool IsSuspended { get; set; } = false;

        public override string ToString()
        {
            return $"{CustomerCode} - {Username} ({Company})" + (IsSuspended ? " [Suspended]" : "");
        }

        public override bool Equals(object? obj)
        {
            return obj is User other && Username == other.Username;
        }

        public override int GetHashCode()
        {
            return Username.GetHashCode();
        }
    }
#nullable restore
}
