using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using AuthServerTool.Services;

namespace AuthServerTool.Forms
{
    public partial class EditUserForm : Form
    {
        private readonly string _username;

        public EditUserForm(string username, string currentEmail, string currentAccess, string customerCode, string company)
        {
            InitializeComponent();
            _username = username;
            this.Text = $"Edit User: {username}";

            customerCodeInput.Text = customerCode;
            companyInput.Text = company;
            emailInput.Text = currentEmail;
            accessDropdown.SelectedItem = currentAccess;
        }

        private void saveButton_Click(object? sender, EventArgs e)
        {
            var updatedEmail = emailInput.Text.Trim();
            var updatedAccess = accessDropdown.SelectedItem?.ToString();
            var updatedCustomerCode = customerCodeInput.Text.Trim();
            var updatedCompany = companyInput.Text.Trim();
            var newPassword = passwordInput.Text;

            if (!IsValidEmail(updatedEmail))
            {
                MessageBox.Show("Please enter a valid email address.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            bool infoUpdated = UserService.EditUser(_username, updatedEmail, updatedAccess, updatedCustomerCode, updatedCompany);
            bool passwordUpdated = false;

            if (!string.IsNullOrWhiteSpace(newPassword))
            {
                passwordUpdated = UserService.UpdatePassword(_username, newPassword);
            }

            string result = (infoUpdated || passwordUpdated)
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
