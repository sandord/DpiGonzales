using NLog;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace DpiGonzales
{
    public partial class AboutWindow : Window
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public AboutWindow()
        {
            InitializeComponent();

            VersionLabel.Content = $"v{GetType().Assembly.GetName().Version.ToString()}";
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SupportWebsiteHyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        private void LicenseButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var tempFileName = Path.GetTempFileName();
                var licenseTextFile = Path.ChangeExtension(tempFileName, ".txt");
                File.Move(tempFileName, licenseTextFile);
                File.WriteAllText(licenseTextFile, File.ReadAllText("LICENSE"));

                Process.Start(new ProcessStartInfo(licenseTextFile));
            }
            catch (Exception exception)
            {
                MessageBox.Show("There was a problem displaying the license details.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                Logger.Log(LogLevel.Error, exception);
            }

            e.Handled = true;
        }
    }
}
