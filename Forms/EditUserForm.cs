#nullable enable
using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using AuthServerTool.Services;

namespace AuthServerTool.Forms
{
    public partial class EditUserForm : Form
    {
        // Public accessor for the entered username
        public string EnteredUsername => usernameInput.Text.Trim();

        public EditUserForm(
            string username,
            string passwordHash,
            string firstName,
            string lastName,
            string email,
            string customerCode)
        {
            InitializeComponent();
            this.Text = $"Edit User: {username}";

            usernameInput.Text = username;
            emailInput.Text = email;
            customerCodeInput.Text = customerCode;
            companyInput.Text = string.Empty; // Optional: Remove this line if Company input was deleted
            firstNameInput.Text = firstName;
            lastNameInput.Text = lastName;
        }

        private void saveButton_Click(object? sender, EventArgs e)
        {
            var updatedUsername = usernameInput.Text.Trim();
            var updatedEmail = emailInput.Text.Trim();
            var updatedCustomerCode = customerCodeInput.Text.Trim();
            var updatedCompany = companyInput.Text.Trim(); // Optional: Remove if Company no longer used
            var updatedFirstName = firstNameInput.Text.Trim();
            var updatedLastName = lastNameInput.Text.Trim();
            var newPassword = passwordInput.Text;

            if (!IsValidEmail(updatedEmail))
            {
                MessageBox.Show("Please enter a valid email address.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            bool infoUpdated = UserService.EditUser(updatedUsername, updatedEmail, updatedCustomerCode, updatedCompany);
            bool nameUpdated = UserService.UpdateName(updatedUsername, updatedFirstName, updatedLastName);
            bool passwordUpdated = false;

            if (!string.IsNullOrWhiteSpace(newPassword))
            {
                passwordUpdated = UserService.UpdatePassword(updatedUsername, newPassword);
            }

            string result = (infoUpdated || nameUpdated || passwordUpdated)
                ? "User updated successfully."
                : "No changes were made.";

            MessageBox.Show(result, "Update", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }

        private bool IsValidEmail(string email)
        {
            return Regex.IsMatch(email,
                @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                RegexOptions.IgnoreCase);
        }
    }
}
#nullable restore
