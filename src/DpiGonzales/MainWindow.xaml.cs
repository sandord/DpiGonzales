using System.Windows;

namespace DpiGonzales
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            VersionLabel.Content = $"v{GetType().Assembly.GetName().Version.ToString()}";
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
