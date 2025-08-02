using System;
using System.Windows.Forms;
using AuthServerTool.Services;

namespace AuthServerTool.Forms
{
#nullable disable
    public partial class ConfigurationForm : Form
    {
        public ConfigurationForm()
        {
            InitializeComponent();
            this.Text = "User Folder Configuration";
            this.StartPosition = FormStartPosition.CenterScreen;

            // Load previously saved path if available
            var currentPath = ConfigService.LoadFolderPath();
            if (!string.IsNullOrWhiteSpace(currentPath))
            {
                folderPathInput.Text = currentPath;
            }
        }

        private void browseButton_Click(object sender, EventArgs e)
        {
            using var folderDialog = new FolderBrowserDialog();
            folderDialog.Description = "Select root directory for user folders";

            if (folderDialog.ShowDialog() == DialogResult.OK)
            {
                folderPathInput.Text = folderDialog.SelectedPath;
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            var selectedPath = folderPathInput.Text;

            if (string.IsNullOrWhiteSpace(selectedPath) || !System.IO.Directory.Exists(selectedPath))
            {
                MessageBox.Show("Please select a valid directory.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            ConfigService.SaveFolderPath(selectedPath);
            MessageBox.Show("User folder directory saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }
    }
#nullable restore
}
