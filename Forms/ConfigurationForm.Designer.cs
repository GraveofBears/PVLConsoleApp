using System.Windows.Forms;
using System.Drawing;

namespace AuthServerTool.Forms
{
    partial class ConfigurationForm
    {
        private Label folderPathLabel;
        private TextBox folderPathInput;
        private Button browseButton;
        private Button saveButton;

        private void InitializeComponent()
        {
            this.folderPathLabel = new Label();
            this.folderPathInput = new TextBox();
            this.browseButton = new Button();
            this.saveButton = new Button();

            // 
            // folderPathLabel
            // 
            this.folderPathLabel.AutoSize = true;
            this.folderPathLabel.Location = new Point(20, 20);
            this.folderPathLabel.Text = "User Folder Root Path:";

            // 
            // folderPathInput
            // 
            this.folderPathInput.Location = new Point(20, 45);
            this.folderPathInput.Width = 300;
            this.folderPathInput.ReadOnly = true;

            // 
            // browseButton
            // 
            this.browseButton.Text = "Browse...";
            this.browseButton.Location = new Point(330, 43);
            this.browseButton.Click += new System.EventHandler(this.browseButton_Click);

            // 
            // saveButton
            // 
            this.saveButton.Text = "Save";
            this.saveButton.Location = new Point(280, 90);
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);

            // 
            // ConfigurationForm
            // 
            this.ClientSize = new Size(420, 140);
            this.Controls.Add(this.folderPathLabel);
            this.Controls.Add(this.folderPathInput);
            this.Controls.Add(this.browseButton);
            this.Controls.Add(this.saveButton);
        }
    }
}
