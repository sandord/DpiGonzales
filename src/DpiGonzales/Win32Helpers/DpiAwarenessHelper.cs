using System;
using System.Runtime.InteropServices;

namespace DpiGonzales.Win32Helpers
{
    public class DpiAwarenessHelper
    {
        public static bool SetPerMonitorDpiAware()
        {
            switch (SetProcessDpiAwareness(ProcessDpiAwareness.ProcessPerMonitorDpiAware))
            {
                case EAccessDenied:
                    // TODO: throw a custom exception type so the caller can specifically catch it.
                    throw new InvalidOperationException("Access denied.");
                case SOk:
                    return true;
                default:
                    throw new InvalidOperationException("Failed to set per monitor DPI awareness.");
            }
        }

        private const int SOk = 0x00000000;
        private const int EAccessDenied = unchecked((int)0x80070005);

        [DllImport("Shcore")]
        private static extern int SetProcessDpiAwareness(ProcessDpiAwareness awareness);

        private enum ProcessDpiAwareness
        {
            ProcessDpiUnaware = 0,
            ProcessSystemDpiAware = 1,
            ProcessPerMonitorDpiAware = 2,
        }
    }
}
