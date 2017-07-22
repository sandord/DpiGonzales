using System;
using System.Windows;
using System.Windows.Threading;
using DpiGonzales.Properties;
using DpiGonzales.Win32Helpers;
using Hardcodet.Wpf.TaskbarNotification;
using NLog;

namespace DpiGonzales
{
    public partial class App : Application
    {
        private const float WindowsReferenceDpi = 96f;
        private const int WindowsMinMouseSpeed = 0;
        private const int WindowsMaxMouseSpeed = 20;

        internal const int MinRefreshRate = 1;
        internal const int MaxRefreshRate = 120;
        private static int _refreshRate = 60;

        internal const float MinMouseSpeedToDisplayDpiRatio = 7f;
        internal const float MaxMouseSpeedToDisplayDpiRatio = 26f;
        private static float _mouseSpeedToDisplayDpiRatio = 16f;
        
        internal static SettingsWindow SettingsWindow;

        /// <summary>
        /// Adjusts mouse speed n times per second.
        /// </summary>
        internal static int RefreshRate
        {
            get => _refreshRate;
            set => _refreshRate = value >= MinRefreshRate && value <= MaxRefreshRate ? value : _refreshRate;
        }

        /// <summary>
        /// Increments mouse speed for every n dots above the reference dpi.
        /// </summary>
        internal static float MouseSpeedToDisplayDpiRatio
        {
            get => _mouseSpeedToDisplayDpiRatio;
            set => _mouseSpeedToDisplayDpiRatio = value >= MinMouseSpeedToDisplayDpiRatio && value <= MaxMouseSpeedToDisplayDpiRatio ? value : _mouseSpeedToDisplayDpiRatio;
        }

        private TaskbarIcon _notifyIcon;
        private int _systemMouseSpeed;
        private DispatcherTimer _dispatcherTimer;
        private IntPtr _currentDisplay = (IntPtr)0;

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public App()
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledExceptionHandler;

            RefreshRate = Settings.Default.RefreshRate;
            MouseSpeedToDisplayDpiRatio = Settings.Default.MouseSpeedToDisplayDpiRatio;
        }

        private void CurrentDomain_UnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs e)
        {
            Logger.Log(e.IsTerminating ? LogLevel.Fatal : LogLevel.Error, e.ExceptionObject);
        }

        private void App_OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            Logger.Error(e.Exception);
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            Logger.Info("Application starting");
            
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
                Logger.Warn(exception.Message);
            }

            // Get system mouse speed.
            _systemMouseSpeed = MouseHelper.GetMouseSpeed();

            SetUpTimer();

            Logger.Info("Application started");
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
                var mouseSpeed = (int)Math.Floor(_systemMouseSpeed + (displayDpi - WindowsReferenceDpi) / MouseSpeedToDisplayDpiRatio);
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
