using System.Windows.Forms;

namespace AuthServerTool.Forms
{
    partial class UserRegistrationForm
    {
        private Label usernameLabel;
        private TextBox usernameInput;
        private Label passwordLabel;
        private TextBox passwordInput;
        private Label emailLabel;
        private TextBox emailInput;
        private Label customerCodeLabel;
        private TextBox customerCodeInput;
        private Label firstNameLabel;
        private TextBox firstNameInput;
        private Label lastNameLabel;
        private TextBox lastNameInput;
        private Label companyLabel;
        private TextBox companyInput;
        private Label accessLevelLabel;
        private ComboBox accessLevelDropdown;
        private Button registerButton;
        private Button clearButton;

        private void InitializeComponent()
        {
            // 💡 Form setup
            this.Text = "Register New User";
            this.Width = 440;
            this.Height = 520;
            this.StartPosition = FormStartPosition.CenterScreen;

            int left = 20;
            int width = 380;
            int spacing = 35;
            int top = 20;

            // 👤 Username
            usernameLabel = new Label { Text = "Username:", Top = top, Left = left, AutoSize = true };
            top += 20;
            usernameInput = new TextBox { Top = top, Left = left, Width = width };
            top += spacing;

            // 🔐 Password
            passwordLabel = new Label { Text = "Password:", Top = top, Left = left, AutoSize = true };
            top += 20;
            passwordInput = new TextBox { Top = top, Left = left, Width = width, UseSystemPasswordChar = true };
            top += spacing;

            // 📧 Email
            emailLabel = new Label { Text = "Email:", Top = top, Left = left, AutoSize = true };
            top += 20;
            emailInput = new TextBox { Top = top, Left = left, Width = width };
            top += spacing;

            // 🧾 Customer Code
            customerCodeLabel = new Label { Text = "Customer Code:", Top = top, Left = left, AutoSize = true };
            top += 20;
            customerCodeInput = new TextBox { Top = top, Left = left, Width = width };
            top += spacing;

            // 🧍 First Name
            firstNameLabel = new Label { Text = "First Name:", Top = top, Left = left, AutoSize = true };
            top += 20;
            firstNameInput = new TextBox { Top = top, Left = left, Width = width };
            top += spacing;

            // 🧍 Last Name
            lastNameLabel = new Label { Text = "Last Name:", Top = top, Left = left, AutoSize = true };
            top += 20;
            lastNameInput = new TextBox { Top = top, Left = left, Width = width };
            top += spacing;

            // 🏢 Company
            companyLabel = new Label { Text = "Company:", Top = top, Left = left, AutoSize = true };
            top += 20;
            companyInput = new TextBox { Top = top, Left = left, Width = width };
            top += spacing;

            // 🧭 Access Level
            accessLevelLabel = new Label { Text = "Access Level:", Top = top, Left = left, AutoSize = true };
            top += 20;
            accessLevelDropdown = new ComboBox
            {
                Top = top,
                Left = left,
                Width = width,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            accessLevelDropdown.Items.AddRange(new string[] { "user", "admin", "moderator" });
            accessLevelDropdown.SelectedIndex = 0;
            top += spacing;

            // 🧹 Clear
            clearButton = new Button { Text = "Clear", Top = top, Left = left, Width = 100 };
            clearButton.Click += ClearButton_Click;

            // ✅ Register
            registerButton = new Button { Text = "Register", Top = top, Left = left + 280, Width = 100 };
            registerButton.Click += RegisterButton_Click;

            // 📦 Add to form
            this.Controls.Add(usernameLabel);
            this.Controls.Add(usernameInput);
            this.Controls.Add(passwordLabel);
            this.Controls.Add(passwordInput);
            this.Controls.Add(emailLabel);
            this.Controls.Add(emailInput);
            this.Controls.Add(customerCodeLabel);
            this.Controls.Add(customerCodeInput);
            this.Controls.Add(firstNameLabel);
            this.Controls.Add(firstNameInput);
            this.Controls.Add(lastNameLabel);
            this.Controls.Add(lastNameInput);
            this.Controls.Add(companyLabel);
            this.Controls.Add(companyInput);
            this.Controls.Add(accessLevelLabel);
            this.Controls.Add(accessLevelDropdown);
            this.Controls.Add(clearButton);
            this.Controls.Add(registerButton);
        }
    }
}
