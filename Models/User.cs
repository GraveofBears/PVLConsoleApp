#nullable enable
using System;

namespace AuthServerTool.Models
{
    public class User
    {
        // Required properties
        public string Username { get; set; } = string.Empty;
        public string CustomerCode { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Company { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsSuspended { get; set; } = false;

        // Optional convenience constructor
        public User(
            string username,
            string customerCode,
            string firstName,
            string lastName,
            string email,
            string company)
        {
            Username = username;
            CustomerCode = customerCode;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Company = company;
        }

        public User() { }
    }
}
#nullable restore
