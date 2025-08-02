namespace PVLConsoleApp.Forms
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;
        private MenuStrip menuStrip;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem configurationToolStripMenuItem;
        private ToolStripMenuItem usersToolStripMenuItem;
        private ToolStripMenuItem addUserToolStripMenuItem;
        private DataGridView userGrid;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            this.menuStrip = new MenuStrip();
            this.fileToolStripMenuItem = new ToolStripMenuItem();
            this.configurationToolStripMenuItem = new ToolStripMenuItem();
            this.usersToolStripMenuItem = new ToolStripMenuItem();
            this.addUserToolStripMenuItem = new ToolStripMenuItem();
            this.userGrid = new DataGridView();

            // MenuStrip
            this.menuStrip.Items.AddRange(new ToolStripItem[] {
                this.fileToolStripMenuItem,
                this.usersToolStripMenuItem
            });
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(800, 24);

            // File Menu
            this.fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] {
                this.configurationToolStripMenuItem
            });
            this.fileToolStripMenuItem.Text = "File";

            // Configuration Menu Item
            this.configurationToolStripMenuItem.Text = "Configuration";
            this.configurationToolStripMenuItem.Click += new EventHandler(this.configurationToolStripMenuItem_Click);

            // Users Menu
            this.usersToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] {
                this.addUserToolStripMenuItem
            });
            this.usersToolStripMenuItem.Text = "Users";

            // Add User Menu Item
            this.addUserToolStripMenuItem.Text = "Add New User";
            this.addUserToolStripMenuItem.Click += new EventHandler(this.addUserToolStripMenuItem_Click);

            // User Grid
            this.userGrid.Location = new System.Drawing.Point(0, 24);
            this.userGrid.Name = "userGrid";
            this.userGrid.Size = new System.Drawing.Size(800, 576);
            this.userGrid.Dock = DockStyle.Fill;
            this.userGrid.ReadOnly = true;
            this.userGrid.AllowUserToAddRows = false;
            this.userGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.userGrid.Columns.Add("Username", "Username");
            this.userGrid.Columns.Add("AccessLevel", "Access Level");

            // MainForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.Controls.Add(this.userGrid);
            this.Controls.Add(this.menuStrip);
            this.MainMenuStrip = this.menuStrip;
            this.Name = "MainForm";
            this.Text = "PVL Server Tool";
        }
    }
}
