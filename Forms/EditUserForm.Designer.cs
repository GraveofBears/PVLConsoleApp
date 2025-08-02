namespace AuthServerTool.Forms
{
    partial class EditUserForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label usernameLabel;
        private System.Windows.Forms.TextBox usernameInput;
        private System.Windows.Forms.Label firstNameLabel;
        private System.Windows.Forms.TextBox firstNameInput;
        private System.Windows.Forms.Label lastNameLabel;
        private System.Windows.Forms.TextBox lastNameInput;
        private System.Windows.Forms.Label emailLabel;
        private System.Windows.Forms.TextBox emailInput;
        private System.Windows.Forms.Label customerCodeLabel;
        private System.Windows.Forms.TextBox customerCodeInput;
        private System.Windows.Forms.Label companyLabel;
        private System.Windows.Forms.TextBox companyInput;
        private System.Windows.Forms.Label accessLabel;
        private System.Windows.Forms.ComboBox accessDropdown;
        private System.Windows.Forms.Label passwordLabel;
        private System.Windows.Forms.TextBox passwordInput;
        private System.Windows.Forms.Button saveButton;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.usernameLabel = new Label();
            this.usernameInput = new TextBox();
            this.firstNameLabel = new Label();
            this.firstNameInput = new TextBox();
            this.lastNameLabel = new Label();
            this.lastNameInput = new TextBox();
            this.emailLabel = new Label();
            this.emailInput = new TextBox();
            this.customerCodeLabel = new Label();
            this.customerCodeInput = new TextBox();
            this.companyLabel = new Label();
            this.companyInput = new TextBox();
            this.accessLabel = new Label();
            this.accessDropdown = new ComboBox();
            this.passwordLabel = new Label();
            this.passwordInput = new TextBox();
            this.saveButton = new Button();
            this.SuspendLayout();

            int x = 20, width = 250, labelHeight = 15, inputHeight = 23;
            int y = 20, padding = 5;

            // Utility method for label & input positioning
            void PlaceControl(Control label, Control input)
            {
                label.Location = new Point(x, y);
                label.Size = new Size(width, labelHeight);
                y += labelHeight + padding;
                input.Location = new Point(x, y);
                input.Size = new Size(width, inputHeight);
                y += inputHeight + padding * 2;
                this.Controls.Add(label);
                this.Controls.Add(input);
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

            // Access Level
            accessLabel.Text = "Access Level:";
            accessDropdown.DropDownStyle = ComboBoxStyle.DropDownList;
            accessDropdown.Name = "accessDropdown";
            PlaceControl(accessLabel, accessDropdown);

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
            saveButton.Click += new EventHandler(this.saveButton_Click);
            this.Controls.Add(saveButton);

            // EditUserForm
            this.ClientSize = new Size(300, saveButton.Bottom + 20);
            this.Name = "EditUserForm";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
