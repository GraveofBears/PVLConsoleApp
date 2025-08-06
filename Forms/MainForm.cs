#nullable enable
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using AuthServerTool.Models;
using AuthServerTool.Services;
using AuthServerTool.Forms;

namespace PVLConsoleApp.Forms
{
    public class MainForm : Form
    {
        private readonly ListView userListView = new();
        private readonly MenuStrip menuStrip = new();

        public MainForm()
        {
            Text = "User Manager";
            Size = new Size(800, 600);
            StartPosition = FormStartPosition.CenterScreen;

            InitializeMenu();
            InitializeControls();

            MainMenuStrip = menuStrip;
            Load += MainForm_Load;
        }

        private void InitializeMenu()
        {
            var fileMenu = new ToolStripMenuItem("File");

            var configureItem = new ToolStripMenuItem("Configure");
            configureItem.Click += ConfigureItem_Click;

            var exitItem = new ToolStripMenuItem("Exit");
            exitItem.Click += (_, _) => Close();

            fileMenu.DropDownItems.Add(configureItem);
            fileMenu.DropDownItems.Add(exitItem);

            var editMenu = new ToolStripMenuItem("Edit");

            var addItem = new ToolStripMenuItem("Add New User");
            addItem.Click += AddUser_Click;

            var editItem = new ToolStripMenuItem("Edit Selected User");
            editItem.Click += EditUser_Click;

            var suspendItem = new ToolStripMenuItem("Suspend Selected User");
            suspendItem.Click += SuspendUser_Click;

            var unsuspendItem = new ToolStripMenuItem("Unsuspend Selected User");
            unsuspendItem.Click += UnsuspendUser_Click;

            var deleteItem = new ToolStripMenuItem("Delete Selected User");
            deleteItem.Click += DeleteUser_Click;

            editMenu.DropDownItems.Add(addItem);
            editMenu.DropDownItems.Add(editItem);
            editMenu.DropDownItems.Add(suspendItem);
            editMenu.DropDownItems.Add(unsuspendItem);
            editMenu.DropDownItems.Add(deleteItem);

            menuStrip.Items.Add(fileMenu);
            menuStrip.Items.Add(editMenu);
            Controls.Add(menuStrip);
        }

        private void InitializeControls()
        {
            userListView.Location = new Point(20, menuStrip.Height + 20);
            userListView.Size = new Size(740, 500);
            userListView.View = View.Details;
            userListView.FullRowSelect = true;
            userListView.GridLines = true;

            userListView.Columns.Add("Customer Code", 120);
            userListView.Columns.Add("Username", 120);
            userListView.Columns.Add("Company", 180);
            userListView.Columns.Add("Status", 100);

            Controls.Add(userListView);
        }

        private void MainForm_Load(object? sender, EventArgs e)
        {
            RefreshUserList();
        }

        private void RefreshUserList()
        {
            try
            {
                var users = UserService.GetAllUsers();
                userListView.Items.Clear();

                foreach (var user in users)
                {
                    var item = new ListViewItem(user.CustomerCode);
                    item.SubItems.Add(user.Username);
                    item.SubItems.Add(user.Company);
                    item.SubItems.Add(user.IsSuspended ? "Suspended" : "Active");
                    item.Tag = user;
                    userListView.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load users:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigureItem_Click(object? sender, EventArgs e)
        {
            using var configDialog = new ConfigurationForm();
            configDialog.ShowDialog();
            RefreshUserList();
        }

        private void AddUser_Click(object? sender, EventArgs e)
        {
            using var dialog = new UserRegistrationForm();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                if (!string.IsNullOrWhiteSpace(dialog.RegisteredUsername))
                {
                    var folderPath = Path.Combine(ConfigService.LoadFolderPath(), dialog.RegisteredUsername);
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }
                }

                RefreshUserList();
                MessageBox.Show("User registered and folder created.", "Success");
            }
        }

        private void EditUser_Click(object? sender, EventArgs e)
        {
            if (!TryGetSelectedUser(out var user)) return;

            using var dialog = new EditUserForm(user.Username);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                RefreshUserList();
                MessageBox.Show("User updated.");
            }
        }

        private void SuspendUser_Click(object? sender, EventArgs e)
        {
            if (!TryGetSelectedUser(out var user)) return;
            if (user.IsSuspended) return;

            if (UserService.SuspendUser(user.Username))
            {
                RefreshUserList();
                MessageBox.Show("User suspended.");
            }
        }

        private void UnsuspendUser_Click(object? sender, EventArgs e)
        {
            if (!TryGetSelectedUser(out var user)) return;
            if (!user.IsSuspended) return;

            if (UserService.UnsuspendUser(user.Username))
            {
                RefreshUserList();
                MessageBox.Show("User unsuspended.");
            }
        }

        private void DeleteUser_Click(object? sender, EventArgs e)
        {
            if (!TryGetSelectedUser(out var user)) return;

            var confirm = MessageBox.Show($"Delete user '{user.Username}' and their folder?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm != DialogResult.Yes) return;

            if (UserService.DeleteUser(user.Username))
            {
                var folder = Path.Combine(ConfigService.LoadFolderPath(), user.Username);
                if (Directory.Exists(folder)) Directory.Delete(folder, true);

                RefreshUserList();
                MessageBox.Show("User and folder deleted.");
            }
            else
            {
                MessageBox.Show("Deletion failed.");
            }
        }

        private bool TryGetSelectedUser(out User user)
        {
            user = null!;
            if (userListView.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select a user first.", "Info");
                return false;
            }

            user = userListView.SelectedItems[0].Tag as User;
            return user != null;
        }
    }
}
#nullable restore
