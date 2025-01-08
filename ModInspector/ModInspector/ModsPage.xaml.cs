using System.IO;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using System.Linq;

namespace ModInspector
{
    /// <summary>
    /// Логика взаимодействия для ModsPage.xaml
    /// </summary>
    public partial class ModsPage : Page
    {
        OpenFolderDialog openFolderDialog = new();
        public string? ModsPath { get; set; } = null;

        public ModsPage() => InitializeComponent();
        private void btnSelectAll_Click(object sender, RoutedEventArgs e) => modsListBox.SelectAll();
        private void btnRefreshListOfMods_Click(object sender, RoutedEventArgs e) => Refresh();

        private void btnSwitchState_Click(object sender, RoutedEventArgs e)
        {
            foreach (string item in modsListBox.SelectedItems.Cast<string>().ToList())
            {
                if (Path.GetExtension(item) == ".deactivated")
                    ChangeFileExtension(item, ".jar");
                else if (Path.GetExtension(item) == ".jar")
                    ChangeFileExtension(item, ".deactivated");
            }
            Refresh();
        }

        private void ChangeFileExtension(string fileNameWithoutExtension, string newExtension)
        {
            string oldFilePath = Path.Combine(ModsPath, fileNameWithoutExtension);
            string newFilePathWithExtension = Path.ChangeExtension(oldFilePath, newExtension.TrimStart('.'));

            if (File.Exists(oldFilePath))
            {
                try
                {
                    File.Move(oldFilePath, newFilePathWithExtension);
                    Refresh();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Maybe something is wrong?: {ex.Message}");
                }
            }
            else MessageBox.Show($"File {oldFilePath} not found.");
        }

        private void btnSelectFolder_Click(object sender, RoutedEventArgs e)
        {
            if (openFolderDialog.ShowDialog() == true)
                ModsPath = openFolderDialog.FolderName;
            Refresh();
        }

        void Refresh()
        {
            if (!string.IsNullOrEmpty(ModsPath))
            {
                modsListBox.Items.Clear();
                foreach (string item in Directory.GetFiles(ModsPath))
                    modsListBox.Items.Add(item.Substring(item.LastIndexOf('\\') + 1));
            }
        }
    }
}
