using System.Windows;

namespace ModInspector
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly internal MainPage mainPage = new();
        readonly internal BrowserPage browserPage = new();
        readonly internal ModsPage modsPage = new();

        public MainWindow()
        {
            InitializeComponent();
            mainFrame.Content = mainPage;
        }

        private void btnHome_Click(object sender, RoutedEventArgs e) => mainFrame.Content = mainPage;

        private void btnBrowser_Click(object sender, RoutedEventArgs e) => mainFrame.Content = browserPage;

        private void btnMods_Click(object sender, RoutedEventArgs e) => mainFrame.Content = modsPage;
    }
}