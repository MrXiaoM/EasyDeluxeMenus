using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace EasyDeluxeMenus
{
    public class Win32
    {
        public const int HWND_TOP = 0;
        public const int HWND_BOTTOM = 1;
        public const int HWND_TOPMOST = -1;
        public const int HWND_NOTOPMOST = -2;
        private const int LOGPIXELSX = 88;
        private const int LOGPIXELSY = 90;
        [DllImport("gdi32.dll")]
        private static extern int GetDeviceCaps(IntPtr hdc, int index);
        [DllImport("user32.dll")]
        private static extern IntPtr GetDC(IntPtr hWnd);
        [DllImport("user32.dll")]
        private static extern int ReleaseDC(IntPtr hWnd, IntPtr hDc);

        [DllImport("user32.dll")]
        public static extern IntPtr SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, uint wFlags);

        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hWnd, out WindowRect lpRect);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool GetCursorPos(out POINT pt);

        public static void SetTopMost(Window window)
        {

            IntPtr hwnd = new WindowInteropHelper(window).Handle;
            SetTopMost(hwnd);
        }
        public static void SetTopMost(IntPtr hWnd)
        {
            WindowRect rect = new WindowRect();
            GetWindowRect(hWnd, out rect);
            SetWindowPos(hWnd, (IntPtr)HWND_TOPMOST, rect.Left, rect.Top, rect.Right - rect.Left, rect.Bottom - rect.Top, 0);
        }
        public static Dpi GetDpiByWin32()
        {
            Dpi dpi = new Dpi();
            var hDc = GetDC(IntPtr.Zero);
            dpi.DpiX = GetDeviceCaps(hDc, LOGPIXELSX);
            dpi.DpiY = GetDeviceCaps(hDc, LOGPIXELSY);
            ReleaseDC(IntPtr.Zero, hDc);
            return dpi;
        }
        public static POINT MousePos
        {
            get
            {
                POINT point = new POINT();
                GetCursorPos(out point);
                return point;
            }
        }

        public struct Dpi
        {
            public int DpiX;
            public int DpiY;
        }
        public struct WindowRect
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }
        public struct POINT
        {
            public int X;
            public int Y;
        }
    }
}
