#nullable enable
using System;
using System.Drawing;
using System.Windows.Forms;
using AuthServerTool.Services;
using AuthServerTool.Forms;

namespace PVLConsoleApp.Forms
{
    public class MainForm : Form
    {
        private DataGridView? userGrid;
        private MenuStrip? menuStrip;
        private ToolStripMenuItem? fileMenu;
        private ToolStripMenuItem? editMenu;

        public MainForm()
        {
            InitializeUI();
            LoadUsers();
        }

        private void InitializeUI()
        {
            this.Text = "PVL Console App";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(800, 500);

            menuStrip = new MenuStrip();

            // 📁 File Menu
            fileMenu = new ToolStripMenuItem("File");
            var configurationItem = new ToolStripMenuItem("Configuration");
            configurationItem.Click += configurationToolStripMenuItem_Click;
            var exitItem = new ToolStripMenuItem("Exit Program");
            exitItem.Click += (_, _) => Application.Exit();
            fileMenu.DropDownItems.Add(configurationItem);
            fileMenu.DropDownItems.Add(exitItem);

            // 🧾 Edit Menu
            editMenu = new ToolStripMenuItem("Edit");
            var addUserItem = new ToolStripMenuItem("Add User");
            addUserItem.Click += addUserToolStripMenuItem_Click;
            var editUserItem = new ToolStripMenuItem("Edit User");
            editUserItem.Click += editUserToolStripMenuItem_Click;
            var suspendUserItem = new ToolStripMenuItem("Suspend User");
            suspendUserItem.Click += suspendUserToolStripMenuItem_Click;
            var unsuspendUserItem = new ToolStripMenuItem("Unsuspend User");
            unsuspendUserItem.Click += unsuspendUserToolStripMenuItem_Click;
            var deleteUserItem = new ToolStripMenuItem("Delete User");
            deleteUserItem.Click += deleteUserToolStripMenuItem_Click;
            editMenu.DropDownItems.Add(addUserItem);
            editMenu.DropDownItems.Add(editUserItem);
            editMenu.DropDownItems.Add(suspendUserItem);
            editMenu.DropDownItems.Add(unsuspendUserItem);
            editMenu.DropDownItems.Add(deleteUserItem);

            menuStrip.Items.Add(fileMenu);
            menuStrip.Items.Add(editMenu);
            this.MainMenuStrip = menuStrip;
            this.Controls.Add(menuStrip);

            // 📋 User Grid Setup
            userGrid = new DataGridView
            {
                Top = menuStrip.Height + 10,
                Left = 10,
                Width = this.ClientSize.Width - 20,
                Height = this.ClientSize.Height - menuStrip.Height - 20,
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                ReadOnly = true,
                AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            userGrid.Columns.Add("CustomerCode", "Customer Code");
            userGrid.Columns.Add("Username", "Username");
            userGrid.Columns.Add("Company", "Company");
            userGrid.Columns.Add("Status", "Active/Suspended");

            userGrid.SortCompare += (s, e) =>
            {
                e.SortResult = string.Compare(e.CellValue1?.ToString(), e.CellValue2?.ToString(), StringComparison.OrdinalIgnoreCase);
                e.Handled = true;
            };

            this.Controls.Add(userGrid);
        }

        private void LoadUsers()
        {
            if (userGrid is null) return;
            userGrid.Rows.Clear();

            var users = UserService.GetAllUsers();
            foreach (var user in users)
            {
                string status = user.IsSuspended ? "Suspended" : "Active";
                int rowIndex = userGrid.Rows.Add(user.CustomerCode, user.Username, user.Company, status);

                if (user.IsSuspended)
                {
                    userGrid.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                    userGrid.Rows[rowIndex].DefaultCellStyle.Font = new Font(userGrid.Font, FontStyle.Bold);
                }
            }
        }

        private void addUserToolStripMenuItem_Click(object? sender, EventArgs e)
        {
            var form = new UserRegistrationForm();
            form.ShowDialog();
            form.Dispose();
            LoadUsers();
        }

        private void configurationToolStripMenuItem_Click(object? sender, EventArgs e)
        {
            var form = new ConfigurationForm();
            form.ShowDialog();
            form.Dispose();
        }

        private void editUserToolStripMenuItem_Click(object? sender, EventArgs e)
        {
            if (userGrid is null || userGrid.SelectedRows.Count == 0)
            {
                MessageBox.Show("Select a user to edit.", "No User Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var username = userGrid.SelectedRows[0].Cells["Username"].Value?.ToString();
            if (string.IsNullOrWhiteSpace(username)) return;

            var user = UserService.GetAllUsers().Find(u => u.Username == username);
            if (user == null)
            {
                MessageBox.Show("User not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var form = new EditUserForm(user.Username, user.Email, user.AccessLevel, user.CustomerCode, user.Company);
            form.ShowDialog();
            form.Dispose();
            LoadUsers();
        }

        private void suspendUserToolStripMenuItem_Click(object? sender, EventArgs e)
        {
            if (userGrid is null || userGrid.SelectedRows.Count == 0)
            {
                MessageBox.Show("Select a user to suspend.", "No User Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var username = userGrid.SelectedRows[0].Cells["Username"].Value?.ToString();
            if (string.IsNullOrWhiteSpace(username)) return;

            UserService.SuspendUser(username);
            LoadUsers();
            MessageBox.Show($"User '{username}' has been suspended.", "Suspend User", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void unsuspendUserToolStripMenuItem_Click(object? sender, EventArgs e)
        {
            if (userGrid is null || userGrid.SelectedRows.Count == 0)
            {
                MessageBox.Show("Select a user to unsuspend.", "No User Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var username = userGrid.SelectedRows[0].Cells["Username"].Value?.ToString();
            if (string.IsNullOrWhiteSpace(username)) return;

            UserService.UnsuspendUser(username);
            LoadUsers();
            MessageBox.Show($"User '{username}' has been unsuspended.", "Unsuspend User", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void deleteUserToolStripMenuItem_Click(object? sender, EventArgs e)
        {
            if (userGrid is null || userGrid.SelectedRows.Count == 0)
            {
                MessageBox.Show("Select a user to delete.", "No User Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var username = userGrid.SelectedRows[0].Cells["Username"].Value?.ToString();
            if (string.IsNullOrWhiteSpace(username)) return;

            var confirm = MessageBox.Show($"Delete user '{username}' and their folder?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm == DialogResult.Yes)
            {
                UserService.DeleteUser(username);
                LoadUsers();
                MessageBox.Show($"User '{username}' deleted.", "Delete User", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
#nullable restore
