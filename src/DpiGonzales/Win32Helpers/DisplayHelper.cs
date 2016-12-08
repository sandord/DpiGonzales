using System;
using System.Runtime.InteropServices;
using System.Windows;

namespace DpiGonzales.Win32Helpers
{
    public static class DisplayHelper
    {
        public static IntPtr GetDisplayHandleFromPoint(Point point)
        {
            return MonitorFromPoint(new Win32Point(point), MonitorOptions.DefaultToNull);
        }

        public static int? GetDpiForDisplay(IntPtr hmonitor)
        {
            uint newDpiX;
            uint newDpiY;

            if (GetDpiForMonitor(hmonitor, DpiType.Effective, out newDpiX, out newDpiY) != (IntPtr)0)
            {
                return null;
            }

            return (int)newDpiX;
        }

        [DllImport("User32.dll")]
        private static extern IntPtr MonitorFromPoint(Win32Point pt, MonitorOptions dwFlags);

        [DllImport("Shcore.dll")]
        private static extern IntPtr GetDpiForMonitor(IntPtr hmonitor, DpiType dpiType, out uint dpiX, out uint dpiY);
        
        [Flags]
        private enum MonitorOptions
        {
            DefaultToNull = 0,
            DefaultToPrimary = 1,
            DefaultToNearest = 2,
        }

        private enum DpiType
        {
            Effective = 0,
            Angular = 1,
            Raw = 2
        }
    }
}
