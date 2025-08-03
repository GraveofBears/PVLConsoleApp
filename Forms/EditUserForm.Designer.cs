#nullable enable
using System;
using System.Drawing;
using System.Windows.Forms;

namespace AuthServerTool.Forms
{
    partial class EditUserForm
    {
        private System.ComponentModel.IContainer components = null;
        private Label usernameLabel;
        private TextBox usernameInput;
        private Label firstNameLabel;
        private TextBox firstNameInput;
        private Label lastNameLabel;
        private TextBox lastNameInput;
        private Label emailLabel;
        private TextBox emailInput;
        private Label customerCodeLabel;
        private TextBox customerCodeInput;
        private Label companyLabel;
        private TextBox companyInput;
        private Label passwordLabel;
        private TextBox passwordInput;
        private Button saveButton;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();

            usernameLabel = new Label();
            usernameInput = new TextBox();
            firstNameLabel = new Label();
            firstNameInput = new TextBox();
            lastNameLabel = new Label();
            lastNameInput = new TextBox();
            emailLabel = new Label();
            emailInput = new TextBox();
            customerCodeLabel = new Label();
            customerCodeInput = new TextBox();
            companyLabel = new Label();
            companyInput = new TextBox();
            passwordLabel = new Label();
            passwordInput = new TextBox();
            saveButton = new Button();

            SuspendLayout();

            int x = 20, width = 250, labelHeight = 15, inputHeight = 23;
            int y = 20, padding = 5;

            void PlaceControl(Control label, Control input)
            {
                label.Location = new Point(x, y);
                label.Size = new Size(width, labelHeight);
                y += labelHeight + padding;
                input.Location = new Point(x, y);
                input.Size = new Size(width, inputHeight);
                y += inputHeight + padding * 2;
                Controls.Add(label);
                Controls.Add(input);
            }

            // Username
            usernameLabel.Text = "Username:";
            usernameInput.Name = "usernameInput";
            PlaceControl(usernameLabel, usernameInput);

            // First Name
            firstNameLabel.Text = "First Name:";
            firstNameInput.Name = "firstNameInput";
            PlaceControl(firstNameLabel, firstNameInput);

            // Last Name
            lastNameLabel.Text = "Last Name:";
            lastNameInput.Name = "lastNameInput";
            PlaceControl(lastNameLabel, lastNameInput);

            // Email
            emailLabel.Text = "Email:";
            emailInput.Name = "emailInput";
            PlaceControl(emailLabel, emailInput);

            // Customer Code
            customerCodeLabel.Text = "Customer Code:";
            customerCodeInput.Name = "customerCodeInput";
            PlaceControl(customerCodeLabel, customerCodeInput);

            // Company
            companyLabel.Text = "Company:";
            companyInput.Name = "companyInput";
            PlaceControl(companyLabel, companyInput);

            // Password
            passwordLabel.Text = "New Password (optional):";
            passwordInput.Name = "passwordInput";
            passwordInput.PasswordChar = '*';
            PlaceControl(passwordLabel, passwordInput);

            // Save Button
            saveButton.Text = "Save Changes";
            saveButton.Name = "saveButton";
            saveButton.Size = new Size(width, 30);
            saveButton.Location = new Point(x, y);
            saveButton.Click += new EventHandler(saveButton_Click);
            Controls.Add(saveButton);

            // Form
            ClientSize = new Size(300, saveButton.Bottom + 20);
            Name = "EditUserForm";
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
#nullable restore
