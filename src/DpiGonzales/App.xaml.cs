using System;
using System.Windows;
using System.Windows.Threading;
using DpiGonzales.Win32Helpers;
using Hardcodet.Wpf.TaskbarNotification;

namespace DpiGonzales
{
    public partial class App : Application
    {
        private const float WindowsReferenceDpi = 96f;
        private const int WindowsMinMouseSpeed = 0;
        private const int WindowsMaxMouseSpeed = 20;

        // TODO: make these configurable.
        private int RefreshRate = 60;
        private float MouseSpeedToDisplayDpiRatio = 1 / 16f; // Increment mouse speed for every 16 dots above the reference dpi.

        private TaskbarIcon _notifyIcon;
        private int _systemMouseSpeed;
        private DispatcherTimer _dispatcherTimer;
        private IntPtr _currentDisplay = (IntPtr)0;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            _notifyIcon = (TaskbarIcon)FindResource("NotifyIcon");

            // By enabling DPI awareness after initially disabling it in AssemblyInfo, we can determine the per-display DPI.
            try
            {
                if (!DpiAwarenessHelper.SetPerMonitorDpiAware())
                {
                    throw new InvalidOperationException("Failed to set per-monitor DPI awareness.");
                }
            }
            catch (Exception exception)
            {
                //TODO: log/display exception as warning. Catch a custom exception type because currently we're ignoring exceptions to broadly.
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

                var displayDpi = DisplayHelper.GetDpiForDisplay(displayHandle) ?? WindowsReferenceDpi;
                var mouseSpeed = (int)Math.Floor(_systemMouseSpeed + (displayDpi - WindowsReferenceDpi) * MouseSpeedToDisplayDpiRatio);
                mouseSpeed = Math.Min(Math.Max(mouseSpeed, WindowsMinMouseSpeed), WindowsMaxMouseSpeed);

                // Use even values only, just like Windows Mouse Properties does.
                mouseSpeed = (mouseSpeed / 2) * 2;

                MouseHelper.SetMouseSpeed(mouseSpeed);
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
