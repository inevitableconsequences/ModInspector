using System.IO;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;

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
                File.Move(oldFilePath, newFilePathWithExtension);
                Refresh();
            }
        }

        private void DeleteFileExtension(string fileNameWithoutExtension)
        {
            string filePath = Path.Combine(ModsPath, fileNameWithoutExtension);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                Refresh();
            }
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

        private void tbSearchMod_TextChanged(object sender, TextChangedEventArgs e)
        {
            string search = tbSearchMod.Text.ToLower();
            List<string> mb = modsListBox.Items.Cast<string>().ToList().Where(x => Path.GetFileName(x).ToLower().Contains(search)).Select(item => Path.GetFileName(item)).ToList();
            modsListBox.Items.Clear();
            foreach (string item in mb)
                modsListBox.Items.Add(item);
        }

        private void btnDeleteSelected_Click(object sender, RoutedEventArgs e)
        {
            List<string> sp = modsListBox.SelectedItems.Cast<string>().ToList();
            foreach (var item in sp)
                DeleteFileExtension(item);
            //foreach (var item in sp)
            //    modsListBox.Items.Add(item);
            Refresh();
        }
    }
}
