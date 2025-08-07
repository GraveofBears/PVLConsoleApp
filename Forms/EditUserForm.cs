#nullable enable
using System;
using System.Windows.Forms;
using PVLConsoleApp.Models;
using PVLConsoleApp.Services;

namespace PVLConsoleApp.Forms
{
    public partial class EditUserForm : Form
    {
        private readonly string username;

        public EditUserForm(string username)
        {
            this.username = username;
            InitializeComponent();
            Load += EditUserForm_Load;
        }

        private void EditUserForm_Load(object? sender, EventArgs e)
        {
            SetupLayout();
            LoadUserData();
        }

        private void LoadUserData()
        {
            var user = UserService.GetUser(username);
            if (user == null)
            {
                MessageBox.Show("User not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
                return;
            }

            usernameInput!.Text = user.Username;
            firstNameInput!.Text = user.FirstName;
            lastNameInput!.Text = user.LastName;
            emailInput!.Text = user.Email;
            customerCodeInput!.Text = user.CustomerCode;
            companyInput!.Text = user.Company;
        }

        private void saveButton_Click(object? sender, EventArgs e)
        {
            var updatedUser = new User
            {
                Username = username,
                FirstName = firstNameInput!.Text,
                LastName = lastNameInput!.Text,
                Email = emailInput!.Text,
                CustomerCode = customerCodeInput!.Text,
                Company = companyInput!.Text
            };

            var newPassword = string.IsNullOrWhiteSpace(passwordInput!.Text) ? null : passwordInput.Text;

            var success = UserService.UpdateUser(updatedUser, newPassword);

            if (success)
            {
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
