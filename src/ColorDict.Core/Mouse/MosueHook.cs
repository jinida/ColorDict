﻿using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using static ColorDict.Core.Apis.Win32Apis;
using System.Windows.Input;

namespace ColorDict.Core.Mouse
{
    public class MosueHook
    {
        public delegate void MouseUpEventHandler(object sender, System.Drawing.Point p);

        internal class MouseHook
        {
            private const int WH_MOUSE_LL = 14;
            private const int WM_LBUTTONDOWN = 0x0201;
            private const int WM_LBUTTONUP = 0x0202;
            private const int WM_RBUTTONDOWN = 0x0204;
            private const int WM_RBUTTONUP = 0x0205;
            private const int WM_MOUSEWHEEL = 0x020A;
            private const int WHEEL_DELTA = 120;

            private nint _mouseHookHandle;
            private HookProc _mouseDelegate;

            private event MouseUpEventHandler LeftMouseDown;
            public event MouseUpEventHandler OnLeftMouseDown
            {
                add
                {
                    Subscribe();
                    LeftMouseDown += value;
                }
                remove
                {
                    LeftMouseDown -= value;
                    Unsubscribe();
                }
            }

            private event MouseUpEventHandler LeftMouseUp;
            public event MouseUpEventHandler OnLeftMouseUp
            {
                add
                {
                    Subscribe();
                    LeftMouseUp += value;
                }
                remove
                {
                    LeftMouseUp -= value;
                    Unsubscribe();
                }
            }

            private event MouseUpEventHandler RightMouseDown;
            public event MouseUpEventHandler OnRightMouseDown
            {
                add
                {
                    RightMouseDown += value;
                }
                remove
                {
                    RightMouseDown -= value;
                }
            }

            private event MouseWheelEventHandler MouseWheel;
            public event MouseWheelEventHandler OnMouseWheel
            {
                add
                {
                    Subscribe();
                    MouseWheel += value;
                }
                remove
                {
                    MouseWheel -= value;
                    Unsubscribe();
                }
            }

            private void Unsubscribe()
            {
                if (_mouseHookHandle != nint.Zero)
                {
                    var result = UnhookWindowsHookEx(_mouseHookHandle);
                    _mouseHookHandle = nint.Zero;
                    _mouseDelegate = null;
                    if (!result)
                    {
                        int errorCode = Marshal.GetLastWin32Error();
                        throw new Win32Exception(errorCode);
                    }
                }
            }

            private void Subscribe()
            {
                if (_mouseHookHandle == nint.Zero)
                {
                    _mouseDelegate = MouseHookProc;
                    _mouseHookHandle = SetWindowsHookEx(WH_MOUSE_LL,
                        _mouseDelegate,
                        GetModuleHandle(Process.GetCurrentProcess().MainModule.ModuleName),
                        0);
                    if (_mouseHookHandle == nint.Zero)
                    {
                        int errorCode = Marshal.GetLastWin32Error();
                        throw new Win32Exception(errorCode);
                    }
                }
            }

            private nint MouseHookProc(int nCode, nint wParam, nint lParam)
            {
                if (nCode >= 0)
                {
                    MSLLHOOKSTRUCT mouseHookStruct = (MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT));
                    if (wParam.ToInt32() == WM_LBUTTONDOWN)
                    {
                        LeftMouseDown?.Invoke(null, new System.Drawing.Point(mouseHookStruct.pt.x, mouseHookStruct.pt.y));
                        return new nint(-1);
                    }
                    if (wParam.ToInt32() == WM_LBUTTONUP)
                    {
                        LeftMouseUp?.Invoke(null, new System.Drawing.Point(mouseHookStruct.pt.x, mouseHookStruct.pt.y));
                        return new nint(-1);
                    }
                    if (wParam.ToInt32() == WM_RBUTTONDOWN)
                    {
                        return new nint(-1);
                    }
                    if (wParam.ToInt32() == WM_RBUTTONUP)
                    {
                        RightMouseDown?.Invoke(null, new System.Drawing.Point(mouseHookStruct.pt.x, mouseHookStruct.pt.y));
                        return new nint(-1);
                    }
                    if (wParam.ToInt32() == WM_MOUSEWHEEL)
                    {
                        if (MouseWheel != null)
                        {
                            MouseDevice mouseDev = InputManager.Current.PrimaryMouseDevice;
                            MouseWheel.Invoke(null, new MouseWheelEventArgs(mouseDev, Environment.TickCount, (int)mouseHookStruct.mouseData >> 16));
                        }
                    }
                }
                return CallNextHookEx(_mouseHookHandle, nCode, wParam, lParam);
            }
        }
    }
}
