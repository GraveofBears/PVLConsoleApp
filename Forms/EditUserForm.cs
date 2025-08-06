#nullable enable
using System;
using System.Net.Mail;
using System.Windows.Forms;
using AuthServerTool.Services;

namespace AuthServerTool.Forms
{
    public partial class EditUserForm : Form
    {
        private readonly string originalUsername;

        public EditUserForm(string username)
        {
            originalUsername = username;
            InitializeComponent();
            LoadUserData();
        }

        private void LoadUserData()
        {
            var user = UserService.GetAllUsers().Find(u => u.Username == originalUsername);
            if (user == null)
            {
                MessageBox.Show("User not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
                return;
            }

            usernameInput.Text = user.Username;
            usernameInput.Enabled = false;

            firstNameInput.Text = user.FirstName;
            lastNameInput.Text = user.LastName;
            emailInput.Text = user.Email;
            customerCodeInput.Text = user.CustomerCode;
            companyInput.Text = user.Company;
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            var firstName = firstNameInput.Text.Trim();
            var lastName = lastNameInput.Text.Trim();
            var email = emailInput.Text.Trim();
            var customerCode = customerCodeInput.Text.Trim();
            var company = companyInput.Text.Trim();
            var newPassword = passwordInput.Text;

            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName) ||
                string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(customerCode) ||
                string.IsNullOrWhiteSpace(company))
            {
                MessageBox.Show("Please fill in all fields except password.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!MailAddress.TryCreate(email, out _))
            {
                MessageBox.Show("Invalid email format.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            bool updated = true;

            updated &= UserService.UpdateName(originalUsername, firstName, lastName);
            updated &= UserService.EditUser(originalUsername, email, customerCode, company);

            if (!string.IsNullOrWhiteSpace(newPassword))
            {
                updated &= UserService.UpdatePassword(originalUsername, newPassword);
            }

            if (updated)
            {
                MessageBox.Show("User updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                MessageBox.Show("Failed to update user.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
#nullable restore
