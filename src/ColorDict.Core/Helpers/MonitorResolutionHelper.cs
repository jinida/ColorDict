﻿using System.Collections;
using System.Runtime.InteropServices;
using static ColorDict.Core.Apis.Win32Apis;
using System.Windows.Media;
using System.Windows;

namespace ColorDict.Core.Helpers
{
    public class MonitorResolutionHelper
    {
        public static readonly HandleRef NullHandleRef = new HandleRef(null, IntPtr.Zero);

        private MonitorResolutionHelper(IntPtr monitor, IntPtr hdc)
        {
            var info = new MonitorInfoEx();
            GetMonitorInfo(new HandleRef(null, monitor), info);
            Bounds = new System.Windows.Rect(
                info.rcMonitor.left,
                info.rcMonitor.top,
                info.rcMonitor.right - info.rcMonitor.left,
                info.rcMonitor.bottom - info.rcMonitor.top);
            WorkingArea = new System.Windows.Rect(
                info.rcWork.left,
                info.rcWork.top,
                info.rcWork.right - info.rcWork.left,
                info.rcWork.bottom - info.rcWork.top);
            IsPrimary = (info.dwFlags & MonitorinfofPrimary) != 0;
            Name = new string(info.szDevice).TrimEnd((char)0);
        }

        public static DpiScale GetCurrentMonitorDpi(string windowTag = null)
        {
            if (!string.IsNullOrEmpty(windowTag))
            {
                foreach (Window window in Application.Current.Windows)
                {

                    if (window.Tag?.ToString() == windowTag)
                    {
                        return VisualTreeHelper.GetDpi(window);
                    }
                }
            }
            return VisualTreeHelper.GetDpi(Application.Current.MainWindow);
        }

        public static IEnumerable<MonitorResolutionHelper> AllMonitors
        {
            get
            {
                var closure = new MonitorEnumCallback();
                var proc = new MonitorEnumProc(closure.Callback);
                EnumDisplayMonitors(NullHandleRef, IntPtr.Zero, proc, IntPtr.Zero);
                return closure.Monitors.Cast<MonitorResolutionHelper>();
            }
        }

        public System.Windows.Rect Bounds { get; private set; }

        public System.Windows.Rect WorkingArea { get; private set; }

        public string Name { get; private set; }

        public bool IsPrimary { get; private set; }

        public static bool HasMultipleMonitors()
        {
            return AllMonitors.Count() > 1;
        }

        private class MonitorEnumCallback
        {
            public MonitorEnumCallback()
            {
                Monitors = new ArrayList();
            }

            public ArrayList Monitors { get; private set; }

            public bool Callback(
                IntPtr monitor,
                IntPtr hdc,
                IntPtr lprcMonitor,
                IntPtr lparam)
            {
                Monitors.Add(new MonitorResolutionHelper(monitor, hdc));
                return true;
            }
        }
    }
}
