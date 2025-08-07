#nullable enable
using System;
using System.IO;
using System.Net.Mail;
using System.Text.Json;
using System.Windows.Forms;
using PVLConsoleApp.Models;
using PVLConsoleApp.Services;

namespace PVLConsoleApp.Forms
{
    public partial class UserRegistrationForm : Form
    {
        // 👤 Exposed property for MainForm to access after registration
        public string RegisteredUsername { get; private set; } = string.Empty;

        private const string ConfigPath = "config.json";

        private class Config
        {
            public string UserRootFolder { get; set; } = "Users";
        }

        private Config config = new();

        public UserRegistrationForm()
        {
            InitializeComponent();
            LoadConfig();
        }

        private void LoadConfig()
        {
            if (File.Exists(ConfigPath))
            {
                var json = File.ReadAllText(ConfigPath);
                config = JsonSerializer.Deserialize<Config>(json) ?? new Config();
            }
        }

        private void RegisterButton_Click(object sender, EventArgs e)
        {
            var username = usernameInput.Text.Trim();
            var password = passwordInput.Text;
            var email = emailInput.Text.Trim();
            var customerCode = customerCodeInput.Text.Trim();
            var firstName = firstNameInput.Text.Trim();
            var lastName = lastNameInput.Text.Trim();
            var company = companyInput.Text.Trim();

            // 🔒 Basic validation
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(customerCode) ||
                string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName) ||
                string.IsNullOrWhiteSpace(company))
            {
                MessageBox.Show("Please fill in all fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!MailAddress.TryCreate(email, out _))
            {
                MessageBox.Show("Invalid email format.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 👤 Create user object (without password hash yet)
            var newUser = new User
            {
                Username = username,
                CustomerCode = customerCode,
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Company = company,
                PasswordHash = string.Empty // will be set in service
            };

            // 🧠 Delegate password hashing and saving to service
            bool success = UserService.AddNewUser(newUser, password);

            if (success)
            {
                RegisteredUsername = username;

                // 🗂 Create user folder
                var folderPath = Path.Combine(config.UserRootFolder, username);
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                MessageBox.Show("User registered and folder created successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Username already exists. Choose a different one.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            usernameInput.Text = string.Empty;
            passwordInput.Text = string.Empty;
            emailInput.Text = string.Empty;
            customerCodeInput.Text = string.Empty;
            firstNameInput.Text = string.Empty;
            lastNameInput.Text = string.Empty;
            companyInput.Text = string.Empty;
        }
    }
}
#nullable restore
