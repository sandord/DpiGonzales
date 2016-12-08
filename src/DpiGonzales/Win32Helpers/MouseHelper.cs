using System;
using System.Runtime.InteropServices;
using System.Windows;

namespace DpiGonzales.Win32Helpers
{
    public static class MouseHelper
    {
        public static Point GetMousePosition()
        {
            var w32Mouse = new Win32Point();
            GetCursorPos(ref w32Mouse);

            return new Point(w32Mouse.X, w32Mouse.Y);
        }

        public static int GetMouseSpeed()
        {
            uint mouseSpeed = 0;

            if (!SystemParametersInfo(SpiAction.GetMousespeed, 0, ref mouseSpeed, 0))
            {
                throw new InvalidOperationException("Failed to get system mouse speed.");
            }

            return (int)mouseSpeed;
        }

        public static void SetMouseSpeed(int mouseSpeed)
        {
            if (!SystemParametersInfo(SpiAction.SetMousespeed, 0, (uint)mouseSpeed, 0))
            {
                throw new InvalidOperationException("Failed to restore system mouse speed.");
            }
#if DEBUG
            System.Diagnostics.Debug.WriteLine($"Changed mouse speed to {mouseSpeed}");
#endif
        }

        [DllImport("User32.dll")]
        private static extern bool SystemParametersInfo(SpiAction uiAction, uint uiParam, uint pvParam, uint fWinIni);

        [DllImport("User32.dll")]
        private static extern bool SystemParametersInfo(SpiAction uiAction, uint uiParam, ref uint pvParam, uint fWinIni);

        [DllImport("User32.dll")]
        private static extern bool GetCursorPos(ref Win32Point pt);

        private enum SpiAction
        {
            GetMousespeed = 0x0070,
            SetMousespeed = 0x0071
        }
    }
}
