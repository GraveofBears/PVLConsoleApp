#nullable enable
using System;
using System.Drawing;
using System.Windows.Forms;

namespace PVLConsoleApp.Forms
{
    public partial class EditUserForm
    {
        private Label? usernameLabel;
        private TextBox? usernameInput;
        private Label? firstNameLabel;
        private TextBox? firstNameInput;
        private Label? lastNameLabel;
        private TextBox? lastNameInput;
        private Label? emailLabel;
        private TextBox? emailInput;
        private Label? customerCodeLabel;
        private TextBox? customerCodeInput;
        private Label? companyLabel;
        private TextBox? companyInput;
        private Label? passwordLabel;
        private TextBox? passwordInput;
        private Button? saveButton;

        private void InitializeComponent()
        {
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

            saveButton.Text = "Save Changes";
            saveButton.Name = "saveButton";
            saveButton.Size = new Size(250, 30);
            saveButton.Click += saveButton_Click;

            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(300, 500);
            Name = "EditUserForm";
            Text = "Edit User";

            ResumeLayout(false);
            PerformLayout();
        }

        private void SetupLayout()
        {
            int x = 20, width = 250, labelHeight = 15, inputHeight = 23;
            int y = 20, pad = 5;

            void Place(Control label, Control input)
            {
                label.Location = new Point(x, y);
                label.Size = new Size(width, labelHeight);
                Controls.Add(label);
                y += labelHeight + pad;

                input.Location = new Point(x, y);
                input.Size = new Size(width, inputHeight);
                Controls.Add(input);
                y += inputHeight + pad * 2;
            }

            usernameLabel!.Text = "Username:";
            usernameInput!.Name = "usernameInput";
            usernameInput.ReadOnly = true;
            Place(usernameLabel, usernameInput);

            firstNameLabel!.Text = "First Name:";
            firstNameInput!.Name = "firstNameInput";
            Place(firstNameLabel, firstNameInput);

            lastNameLabel!.Text = "Last Name:";
            lastNameInput!.Name = "lastNameInput";
            Place(lastNameLabel, lastNameInput);

            emailLabel!.Text = "Email:";
            emailInput!.Name = "emailInput";
            Place(emailLabel, emailInput);

            customerCodeLabel!.Text = "Customer Code:";
            customerCodeInput!.Name = "customerCodeInput";
            Place(customerCodeLabel, customerCodeInput);

            companyLabel!.Text = "Company:";
            companyInput!.Name = "companyInput";
            Place(companyLabel, companyInput);

            passwordLabel!.Text = "New Password (optional):";
            passwordInput!.Name = "passwordInput";
            passwordInput.PasswordChar = '*';
            Place(passwordLabel, passwordInput);

            saveButton!.Location = new Point(x, y);
            Controls.Add(saveButton);

            ClientSize = new Size(300, saveButton.Bottom + 20);
        }
    }
}
#nullable restore
