using System;
using System.Net.Mail;
using System.Windows.Forms;
using AuthServerTool.Models;
using AuthServerTool.Services;

namespace AuthServerTool.Forms
{
    public partial class UserRegistrationForm : Form
    {
        public UserRegistrationForm()
        {
            InitializeComponent();
        }

        private void RegisterButton_Click(object? sender, EventArgs e)
        {
            var username = usernameInput.Text.Trim();
            var password = passwordInput.Text;
            var email = emailInput.Text.Trim();
            var customerCode = customerCodeInput.Text.Trim();
            var firstName = firstNameInput.Text.Trim();
            var lastName = lastNameInput.Text.Trim();
            var company = companyInput.Text.Trim();
            var accessLevel = accessLevelDropdown.SelectedItem?.ToString() ?? "user";

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

            var newUser = new User
            {
                Username = username,
                CustomerCode = customerCode,
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Company = company,
                AccessLevel = accessLevel
            };

            bool success = UserService.AddNewUser(newUser, password);

            if (success)
            {
                MessageBox.Show("User registered successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show("Username already exists. Choose a different one.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearButton_Click(object? sender, EventArgs e)
        {
            usernameInput.Text = string.Empty;
            passwordInput.Text = string.Empty;
            emailInput.Text = string.Empty;
            customerCodeInput.Text = string.Empty;
            firstNameInput.Text = string.Empty;
            lastNameInput.Text = string.Empty;
            companyInput.Text = string.Empty;
            accessLevelDropdown.SelectedIndex = 0;
        }
    }
}
