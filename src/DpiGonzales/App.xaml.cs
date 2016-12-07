using System;
using System.Windows;
using System.Windows.Threading;
using DpiGonzales.Win32Helpers;
using Hardcodet.Wpf.TaskbarNotification;

namespace DpiGonzales
{
    public partial class App : Application
    {
        private const int RefreshRate = 60;
        private const float ReferenceDpi = 96f;
        private const int MinMouseSpeed = 0;
        private const int MaxMouseSpeed = 20;
        private const float DpiMouseSpeedFactor = 1 / 16f; // Increment mouse speed for every 16 dots above the reference dpi.

        private TaskbarIcon _notifyIcon;
        private int _systemMouseSpeed;
        private DispatcherTimer _dispatcherTimer;
        private IntPtr _currentDisplay = (IntPtr)0;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            _notifyIcon = (TaskbarIcon)FindResource("NotifyIcon");

            // By enabling DPI awareness after initially disabling it in AssemblyInfo, we can determine the per-display DPI.
            if (!DpiAwarenessHelper.SetPerMonitorDpiAware())
            {
                throw new InvalidOperationException("Failed to set per-monitor DPI awareness.");
            }

            // Get system mouse speed.
            _systemMouseSpeed = MouseHelper.GetMouseSpeed();

            SetUpTimer();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _notifyIcon.Dispose();

            StopTimer();

            // Restore system mouse speed.
            MouseHelper.SetMouseSpeed(_systemMouseSpeed);

            base.OnExit(e);
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            var mousePosition = MouseHelper.GetMousePosition();
            var displayHandle = DisplayHelper.GetDisplayHandleFromPoint(mousePosition);

            if (_currentDisplay != displayHandle)
            {
                _currentDisplay = displayHandle;

                var displayDpi = DisplayHelper.GetDpiForDisplay(displayHandle) ?? ReferenceDpi;
                var mouseSpeed = (int)Math.Floor(_systemMouseSpeed + (displayDpi - ReferenceDpi) * DpiMouseSpeedFactor);
                mouseSpeed = Math.Min(Math.Max(mouseSpeed, MinMouseSpeed), MaxMouseSpeed);

                // Use even values only, just like Windows Mouse Properties does.
                mouseSpeed = (mouseSpeed / 2) * 2;

                MouseHelper.SetMouseSpeed(mouseSpeed);
#if DEBUG
                System.Diagnostics.Debug.WriteLine(mouseSpeed);
#endif
            }
        }

        private void SetUpTimer()
        {
            _dispatcherTimer = new DispatcherTimer();
            _dispatcherTimer.Tick += DispatcherTimer_Tick;
            _dispatcherTimer.Interval = TimeSpan.FromSeconds(1f / RefreshRate);
            _dispatcherTimer.Start();
        }

        private void StopTimer()
        {
            _dispatcherTimer?.Stop();
        }
    }
}
