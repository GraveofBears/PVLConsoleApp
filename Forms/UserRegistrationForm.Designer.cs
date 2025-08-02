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
            this.Text = "Register New User";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.ClientSize = new System.Drawing.Size(300, 520);

            int x = 20, width = 250, labelHeight = 15, inputHeight = 23;
            int y = 20, pad = 5;

            // Helper to position controls cleanly
            void Place(Control label, Control input)
            {
                label.Location = new System.Drawing.Point(x, y);
                label.Size = new System.Drawing.Size(width, labelHeight);
                y += labelHeight + pad;
                input.Location = new System.Drawing.Point(x, y);
                input.Size = new System.Drawing.Size(width, inputHeight);
                y += inputHeight + pad * 2;
                this.Controls.Add(label);
                this.Controls.Add(input);
            }

            usernameLabel = new Label { Text = "Username:" };
            usernameInput = new TextBox { Name = "usernameInput" };
            Place(usernameLabel, usernameInput);

            passwordLabel = new Label { Text = "Password:" };
            passwordInput = new TextBox { Name = "passwordInput", PasswordChar = '*' };
            Place(passwordLabel, passwordInput);

            emailLabel = new Label { Text = "Email:" };
            emailInput = new TextBox { Name = "emailInput" };
            Place(emailLabel, emailInput);

            customerCodeLabel = new Label { Text = "Customer Code:" };
            customerCodeInput = new TextBox { Name = "customerCodeInput" };
            Place(customerCodeLabel, customerCodeInput);

            firstNameLabel = new Label { Text = "First Name:" };
            firstNameInput = new TextBox { Name = "firstNameInput" };
            Place(firstNameLabel, firstNameInput);

            lastNameLabel = new Label { Text = "Last Name:" };
            lastNameInput = new TextBox { Name = "lastNameInput" };
            Place(lastNameLabel, lastNameInput);

            companyLabel = new Label { Text = "Company:" };
            companyInput = new TextBox { Name = "companyInput" };
            Place(companyLabel, companyInput);

            accessLevelLabel = new Label { Text = "Access Level:" };
            accessLevelDropdown = new ComboBox
            {
                Name = "accessLevelDropdown",
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            accessLevelDropdown.Items.AddRange(new string[] { "user", "admin", "moderator" });
            accessLevelDropdown.SelectedIndex = 0;
            Place(accessLevelLabel, accessLevelDropdown);

            clearButton = new Button
            {
                Text = "Clear",
                Width = 100,
                Location = new System.Drawing.Point(x, y)
            };
            clearButton.Click += ClearButton_Click;

            registerButton = new Button
            {
                Text = "Register",
                Width = 100,
                Location = new System.Drawing.Point(x + 150, y)
            };
            registerButton.Click += RegisterButton_Click;

            this.Controls.Add(clearButton);
            this.Controls.Add(registerButton);
        }
    }
}
