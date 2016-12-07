using System;
using System.Runtime.InteropServices;
using System.Windows;

namespace DpiGonzales.Win32Helpers
{
    public static class DisplayHelper
    {
        public static IntPtr GetDisplayHandleFromPoint(Point point)
        {
            return MonitorFromPoint(new PointStruct(point), MonitorOptions.DefaultToNull);
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
        private static extern IntPtr MonitorFromPoint(PointStruct pt, MonitorOptions dwFlags);

        [DllImport("Shcore.dll")]
        private static extern IntPtr GetDpiForMonitor([In]IntPtr hmonitor, [In]DpiType dpiType, [Out]out uint dpiX, [Out]out uint dpiY);
        
        [Flags]
        private enum MonitorOptions
        {
            DefaultToNull = 0,
            DefaultToPrimary = 1,
            DefaultToNearest = 2,
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct PointStruct
        {
            public int X;
            public int Y;

            public PointStruct(int x, int y)
            {
                X = x;
                Y = y;
            }

            public PointStruct(Point pt) : this((int)pt.X, (int)pt.Y) { }

            public static implicit operator Point(PointStruct p)
            {
                return new Point(p.X, p.Y);
            }

            public static implicit operator PointStruct(Point p)
            {
                return new PointStruct((int)p.X, (int)p.Y);
            }
        }

        private enum DpiType
        {
            Effective = 0,
            Angular = 1,
            Raw = 2
        }
    }
}
