﻿using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace ColorDict.Core.Apis
{
    public static class Win32Apis
    {
        public const int WH_KEYBOARD_LL = 13;
        public const int VkSnapshot = 0x2c;
        public const int KfAltdown = 0x2000;
        public const int LlkhfAltdown = (KfAltdown >> 8);
        public const int MonitorinfofPrimary = 0x00000001;

        public delegate bool MonitorEnumProc(
            IntPtr monitor, IntPtr hdc, IntPtr lprcMonitor, IntPtr lParam);

        public delegate IntPtr HookProc(int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        internal static extern IntPtr LoadLibrary(string lpFileName);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        internal static extern bool FreeLibrary(IntPtr hModule);


        [DllImport("kernel32.dll", CharSet = CharSet.Unicode,
             CallingConvention = CallingConvention.StdCall)]
        internal static extern IntPtr GetModuleHandle(string name);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        [ResourceExposure(ResourceScope.None)]
        internal static extern bool GetMonitorInfo(HandleRef hmonitor, [In, Out] MonitorInfoEx info);

        [DllImport("user32.dll", ExactSpelling = true)]
        [ResourceExposure(ResourceScope.None)]
        internal static extern bool EnumDisplayMonitors(HandleRef hdc, IntPtr rcClip, MonitorEnumProc lpfnEnum, IntPtr dwData);

        [DllImport("user32.dll")]
        internal static extern bool GetCursorPos(out PointInter lpPoint);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        internal static extern IntPtr SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hMod, int dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto,CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        internal static extern bool UnhookWindowsHookEx(IntPtr idHook);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        internal static extern IntPtr CallNextHookEx(IntPtr idHook, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", EntryPoint = "SystemParametersInfo")]
        internal static extern bool SystemParametersInfo(int uiAction, int uiParam, IntPtr pvParam, int fWinIni);

        [StructLayout(LayoutKind.Sequential)]
        internal struct POINT
        {
            public int x;
            public int y;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct MSLLHOOKSTRUCT
        {
            public POINT pt;
            public uint mouseData;
            public uint flags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct PointInter
        {
            public int X;
            public int Y;
            public static explicit operator System.Windows.Point(PointInter point) => new System.Windows.Point(point.X, point.Y);
        }


        [StructLayout(LayoutKind.Sequential)]
        internal struct Rect
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto, Pack = 4)]
        internal class MonitorInfoEx
        {
            internal int cbSize = Marshal.SizeOf(typeof(MonitorInfoEx));
            internal Rect rcMonitor = default;
            internal Rect rcWork = default;
            internal int dwFlags = 0;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            internal char[] szDevice = new char[32];
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct LowLevelKeyboardInputEvent
        {
            public int VirtualCode;
            public int HardwareScanCode;
            public int Flags;
            public int TimeStamp;
            public IntPtr AdditionalInformation;
        }

    }
}
