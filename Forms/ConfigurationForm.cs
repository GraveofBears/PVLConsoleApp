#nullable enable
using PVLConsoleApp;
using PVLConsoleApp.Services;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace PVLConsoleApp.Forms
{
    public class ConfigurationForm : Form
    {
        private Label folderPathLabel;
        private TextBox folderPathBox;
        private Button userFolderButton;

        private Label dbPathLabel;
        private TextBox dbPathBox;
        private Button dbPathButton;

        private Button saveButton;

        public ConfigurationForm()
        {
            InitializeComponent();

            folderPathBox.Text = ConfigService.LoadFolderPath();
            dbPathBox.Text = ConfigService.LoadDatabasePath();
        }

        private void InitializeComponent()
        {
            this.folderPathLabel = new Label();
            this.folderPathBox = new TextBox();
            this.userFolderButton = new Button();
            this.dbPathLabel = new Label();
            this.dbPathBox = new TextBox();
            this.dbPathButton = new Button();
            this.saveButton = new Button();

            // folderPathLabel
            folderPathLabel.AutoSize = true;
            folderPathLabel.Location = new Point(20, 20);
            folderPathLabel.Text = "User Folder Root Path:";

            // folderPathBox
            folderPathBox.Location = new Point(20, 45);
            folderPathBox.Width = 300;
            folderPathBox.ReadOnly = true;

            // userFolderButton
            userFolderButton.Text = "Browse...";
            userFolderButton.Location = new Point(330, 43);
            userFolderButton.Click += UserFolderButton_Click;

            // dbPathLabel
            dbPathLabel.AutoSize = true;
            dbPathLabel.Location = new Point(20, 85);
            dbPathLabel.Text = "Database Path (.db):";

            // dbPathBox
            dbPathBox.Location = new Point(20, 110);
            dbPathBox.Width = 300;

            // dbPathButton
            dbPathButton.Text = "Browse...";
            dbPathButton.Location = new Point(330, 108);
            dbPathButton.Click += DbPathButton_Click;

            // saveButton
            saveButton.Text = "Save";
            saveButton.Location = new Point(280, 160);
            saveButton.Click += SaveButton_Click;

            // Form setup
            this.ClientSize = new Size(420, 220);
            this.Controls.Add(folderPathLabel);
            this.Controls.Add(folderPathBox);
            this.Controls.Add(userFolderButton);
            this.Controls.Add(dbPathLabel);
            this.Controls.Add(dbPathBox);
            this.Controls.Add(dbPathButton);
            this.Controls.Add(saveButton);
            this.Text = "Configuration";
        }

        private void UserFolderButton_Click(object? sender, EventArgs e)
        {
            using var dialog = new FolderBrowserDialog
            {
                Description = "Select folder for storing user directories",
                SelectedPath = folderPathBox.Text
            };

            if (dialog.ShowDialog() == DialogResult.OK)
                folderPathBox.Text = dialog.SelectedPath;
        }

        private void DbPathButton_Click(object? sender, EventArgs e)
        {
            using var dialog = new OpenFileDialog
            {
                Title = "Select database file",
                Filter = "SQLite DB (*.db)|*.db|All Files (*.*)|*.*",
                FileName = dbPathBox.Text
            };

            if (dialog.ShowDialog() == DialogResult.OK)
                dbPathBox.Text = dialog.FileName;
        }

        private void SaveButton_Click(object? sender, EventArgs e)
        {
            var userFolderPath = folderPathBox.Text;
            var dbPath = dbPathBox.Text;

            if (string.IsNullOrWhiteSpace(userFolderPath) || string.IsNullOrWhiteSpace(dbPath))
            {
                MessageBox.Show("Please provide both folder and database paths.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            ConfigService.SaveFolderPath(userFolderPath);
            ConfigService.SaveDatabasePath(dbPath);

            DatabaseService.OnNotify = message =>
                MessageBox.Show(message, "Database Created", MessageBoxButtons.OK, MessageBoxIcon.Information);

            DatabaseService.SetDatabasePath(dbPath);
            DatabaseService.Initialize(); // 👈 This ensures 'users' table exists


            MessageBox.Show("Configuration saved successfully.", "Configured", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Close();
        }
    }
}
#nullable restore
