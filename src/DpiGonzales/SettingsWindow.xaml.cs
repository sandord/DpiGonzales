using System;
using System.Globalization;
using System.Windows;
using DpiGonzales.Properties;

namespace DpiGonzales
{
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshRateSlider.Minimum = App.MinRefreshRate;
            RefreshRateSlider.Maximum = App.MaxRefreshRate;
            RefreshRateSlider.Value = Settings.Default.RefreshRate;

            MouseSpeedToDisplayDpiRatioSlider.Minimum = App.MinMouseSpeedToDisplayDpiRatio;
            MouseSpeedToDisplayDpiRatioSlider.Maximum = App.MaxMouseSpeedToDisplayDpiRatio;
            MouseSpeedToDisplayDpiRatioSlider.Value = Settings.Default.MouseSpeedToDisplayDpiRatio;

            UpdateRefreshRateLabel();
            UpdateMouseSpeedToDisplayDpiRatioLabel();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            App.RefreshRate = (int)RefreshRateSlider.Value;
            App.MouseSpeedToDisplayDpiRatio = (float)MouseSpeedToDisplayDpiRatioSlider.Value;

            Settings.Default.RefreshRate = App.RefreshRate;
            Settings.Default.MouseSpeedToDisplayDpiRatio = (float)MouseSpeedToDisplayDpiRatioSlider.Value;

            Settings.Default.Save();

            Close();
        }

        private void RefreshRateSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (RefreshRateValue != null)
            {
                UpdateRefreshRateLabel();
            }
        }

        private void MouseSpeedToDisplayDpiRatioSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (MouseSpeedToDisplayDpiRatioValue != null)
            {
                UpdateMouseSpeedToDisplayDpiRatioLabel();
            }
        }

        private void UpdateRefreshRateLabel()
        {
            RefreshRateValue.Content = RefreshRateSlider.Value.ToString("#", CultureInfo.CurrentCulture);
        }

        private void UpdateMouseSpeedToDisplayDpiRatioLabel()
        {
            var normalizedValue = App.MaxMouseSpeedToDisplayDpiRatio - MouseSpeedToDisplayDpiRatioSlider.Value;

            normalizedValue =
                normalizedValue < 3 ? 0 :
                normalizedValue < 15 ? 1 :
                normalizedValue < 18 ? 2 :
                3;

            MouseSpeedToDisplayDpiRatioValue.Content = normalizedValue.ToString(CultureInfo.CurrentCulture);
        }
    }
}
