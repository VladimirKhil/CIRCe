using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace IRCProviders
{
    public static class Win32
    {
        public static readonly UInt32 WM_NCACTIVATE = 0x0086;
        public static readonly UInt32 WM_SYSCOMMAND = 0x0112;
        public static readonly UInt32 WM_MDIACTIVATE = 0x0222;
        public static readonly UInt32 WM_STYLECHANGING = 0x007C;
        public static readonly UInt32 WM_STYLECHANGED = 0x007D;
        public static readonly UInt32 WM_LBUTTONDOWN = 0x201;
        public static readonly int WM_MOUSEACTIVATE = 0x0021;
        public static readonly uint WM_PAINT = 0x000F;

        public static readonly int GWL_STYLE = -16;
        public static readonly int GWL_EXSTYLE = -20;

        public static readonly UInt32 SC_MAXIMIZE = 0xF030;

        public static readonly UInt32 WS_SYSMENU = 0x00080000;
        public static readonly UInt32 WS_SIZEBOX = 0x00040000;
        public static readonly UInt32 WS_MINIMIZEBOX = 0x00020000;
        public static readonly UInt32 WS_MAXIMIZEBOX = 0x00010000;
        public static readonly UInt32 WS_CAPTION      = 0x00C00000;
        public static readonly UInt32 WS_BORDER       = 0x00800000;
        public static readonly UInt32 WS_CLIPSIBLINGS = 0x04000000;
        public static readonly UInt32 WS_CLIPCHILDREN = 0x02000000;
        public static readonly UInt32 WS_MAXIMIZE     = 0x01000000;
        public static readonly UInt32 WS_POPUP        = 0x80000000;
        public static readonly UInt32 WS_CHILD        = 0x40000000;
        public static readonly UInt32 WS_MINIMIZE     = 0x20000000;
        public static readonly UInt32 WS_VISIBLE      = 0x10000000;

        public static readonly UInt32 WS_EX_MDICHILD = 0x40;

        public struct STYLESTRUCT
        {
            public UInt32 styleOld;
            public UInt32 styleNew;
        }

        public const int SB_HORZ = 0;
        public const int SB_VERT = 1;
        public const int SB_CTL = 2;

        public const int SIF_RANGE = 0x1;
        public const int SIF_PAGE = 0x2;
        public const int SIF_POS = 0x4;
        public const int SIF_DISABLENOSCROLL = 0x8;
        public const int SIF_TRACKPOS = 0x10;
        public const int SIF_ALL = (SIF_RANGE | SIF_PAGE | SIF_POS | SIF_TRACKPOS);

        [StructLayout(LayoutKind.Sequential)]
        public struct SCROLLINFO
        {
            public int cbSize;
            public int fMask;
            public int nMin;
            public int nMax;
            public int nPage;
            public int nPos;
            public int nTrackPos;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        [Flags]
        public enum ScrollWindowExFlags
        {
            SW_NONE = 0x0000,
            SW_SCROLLCHILDREN = 0x0001,
            SW_INVALIDATE = 0x0002,
            SW_ERASE = 0x0004,
            SW_SMOOTHSCROLL = 0x0010
        }

        public const int SB_THUMBTRACK = 5;

        public const int SWP_NOMOVE = 0x0002;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public extern static bool DestroyIcon(IntPtr handle);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint GetWindowLongPtr(IntPtr hWnd, int nIndex);

        public static uint GetWindowLongFunc(IntPtr hWnd, int nIndex)
        {
            return Environment.Is64BitProcess ? GetWindowLongPtr(hWnd, nIndex) : GetWindowLong(hWnd, nIndex);
        }

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, uint dwNewLong);

        [DllImport("user32.dll")]
        private static extern int SetWindowLongPtr(IntPtr hWnd, int nIndex, uint dwNewLong);

        public static int SetWindowLongFunc(IntPtr hWnd, int nIndex, uint dwNewLong)
        {
            return Environment.Is64BitProcess ? SetWindowLongPtr(hWnd, nIndex, dwNewLong) : SetWindowLong(hWnd, nIndex, dwNewLong);
        }

        [DllImport("user32.dll")]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr PostMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);

        /// <summary>
        /// Получить сокращённую запись пути
        /// </summary>
        /// <param name="path"></param>
        /// <param name="shortPath"></param>
        /// <param name="shortPathLength"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern int GetShortPathName([MarshalAs(UnmanagedType.LPTStr)] string path, [MarshalAs(UnmanagedType.LPTStr)] StringBuilder shortPath, int shortPathLength);

        [DllImport("user32.dll")]
        public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern IntPtr SetFocus(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern int GetScrollInfo(IntPtr hwnd, int n, ref SCROLLINFO lpScrollInfo);

        [DllImport("user32.dll")]
        public static extern int GetScrollPos(IntPtr hwnd, int n);

        [DllImport("user32.dll")]
        public static extern int SetScrollInfo(IntPtr hwnd, int n, ref SCROLLINFO lpScrollInfo, bool fRedraw);

        [DllImport("user32.dll")]
        public static extern int ScrollWindowEx(IntPtr hwnd, int xAmount, int yAmount, IntPtr lpRect, IntPtr lpClipRect, IntPtr hrgn, ref RECT updateRect, ScrollWindowExFlags flags);

        [DllImport("user32.dll")]
        public static extern int UpdateWindow(IntPtr hwnd);

        [DllImport("shell32.dll", CharSet=CharSet.Auto)]
        public static extern ulong FindExecutable(string lpFile, string lpDirectory, StringBuilder lpResult);

        public const int SW_HIDE = 0;
        public const int SW_SHOWNORMAL = 1;
        public const int SW_MAXIMIZE = 3;
        public const int SW_SHOW = 5;

        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        public static extern IntPtr GetParent(IntPtr hWnd);

        public enum TileMode
        {
            MDITILE_VERTICAL = 0,
            MDITILE_HORIZONTAL = 1 
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool TileWindows(IntPtr hwndParent, TileMode wHow, IntPtr lpRect, uint cKids, IntPtr lpKids);

        [Flags]
        public enum CascadeMode
        {
            MDITILE_SKIPDISABLED = 2,
            MDITILE_ZORDER = 4
        }

        [DllImport("user32.dll")]
        public static extern int CascadeWindows(IntPtr hwndParent, CascadeMode wHow, IntPtr lpRect, uint cKids, IntPtr lpKids);
    }
}
