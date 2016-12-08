using System.Runtime.InteropServices;
using System.Windows;

namespace DpiGonzales.Win32Helpers
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Win32Point
    {
        public int X;
        public int Y;

        public Win32Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Win32Point(Point pt) : this((int)pt.X, (int)pt.Y) { }

        public static implicit operator Point(Win32Point p)
        {
            return new Point(p.X, p.Y);
        }

        public static implicit operator Win32Point(Point p)
        {
            return new Win32Point((int)p.X, (int)p.Y);
        }
    }
}