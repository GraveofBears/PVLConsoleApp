using System;
using System.Windows.Forms;
using AuthServerTool;
using AuthServerTool.Services;
using PVLConsoleApp.Forms;

namespace PVLConsoleApp
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            // 🔒 Windows Forms thread safety
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {
                // 🛠️ Initialize database and patch schema if needed
                DatabaseService.Initialize();

                // 🚀 Launch main application window inside guarded context
                Application.Run(new MainForm());
            }
            catch (Exception ex)
            {
                // 🧯 Last-resort crash handler: helpful during silent failures like exit code 0xFFFFFFFF
                MessageBox.Show(
                    $"Fatal error during startup:\n\n{ex.Message}",
                    "Application Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );

                // Optional: log error or save to crash report file if needed for diagnostics
            }
        }
    }
}
