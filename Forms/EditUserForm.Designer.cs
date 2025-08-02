namespace AuthServerTool.Forms
{
    partial class EditUserForm
    {
        private System.ComponentModel.IContainer components = null;

        private Label customerCodeLabel;
        private TextBox customerCodeInput;
        private Label companyLabel;
        private TextBox companyInput;
        private Label emailLabel;
        private TextBox emailInput;
        private Label accessLabel;
        private ComboBox accessDropdown;
        private Label passwordLabel;
        private TextBox passwordInput;
        private Button saveButton;
        private Button cancelButton;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();

            customerCodeLabel = new Label
            {
                Text = "Customer Code:",
                Top = 20,
                Left = 20,
                AutoSize = true
            };

            customerCodeInput = new TextBox
            {
                Top = 45,
                Left = 20,
                Width = 160
            };

            companyLabel = new Label
            {
                Text = "Company:",
                Top = 85,
                Left = 20,
                AutoSize = true
            };

            companyInput = new TextBox
            {
                Top = 110,
                Left = 20,
                Width = 340
            };

            emailLabel = new Label
            {
                Text = "Email:",
                Top = 150,
                Left = 20,
                AutoSize = true
            };

            emailInput = new TextBox
            {
                Top = 175,
                Left = 20,
                Width = 340
            };

            accessLabel = new Label
            {
                Text = "Access Level:",
                Top = 215,
                Left = 20,
                AutoSize = true
            };

            accessDropdown = new ComboBox
            {
                Top = 240,
                Left = 20,
                Width = 200,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            accessDropdown.Items.AddRange(new[] { "user", "admin", "supervisor" });

            passwordLabel = new Label
            {
                Text = "New Password (optional):",
                Top = 280,
                Left = 20,
                AutoSize = true
            };

            passwordInput = new TextBox
            {
                Top = 305,
                Left = 20,
                Width = 340,
                UseSystemPasswordChar = true
            };

            saveButton = new Button
            {
                Text = "Save",
                Top = 350,
                Left = 160,
                Width = 80
            };
            saveButton.Click += saveButton_Click;

            cancelButton = new Button
            {
                Text = "Cancel",
                Top = 350,
                Left = 250,
                Width = 80
            };
            cancelButton.Click += (_, _) => this.Close();

            this.Controls.AddRange(new Control[]
            {
                customerCodeLabel, customerCodeInput,
                companyLabel, companyInput,
                emailLabel, emailInput,
                accessLabel, accessDropdown,
                passwordLabel, passwordInput,
                saveButton, cancelButton
            });

            this.ClientSize = new System.Drawing.Size(400, 400);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.AcceptButton = saveButton;
        }
    }
}
