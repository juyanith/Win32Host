using System;
using System.Text;
using System.Windows;

namespace Win32Host
{
    public static class User32
    {
        public const int
            LBN_SELCHANGE = 0x00000001,
            WM_COMMAND = 0x00000111,
            LB_GETCURSEL = 0x00000188,
            LB_GETTEXTLEN = 0x0000018A,
            LB_ADDSTRING = 0x00000180,
            LB_GETTEXT = 0x00000189,
            LB_DELETESTRING = 0x00000182,
            LB_GETCOUNT = 0x0000018B;

        public static ushort LOWORD(IntPtr dwValue) => (ushort)(((long)dwValue) & 0xffff);

        public static ushort LOWORD(uint dwValue) => (ushort)(dwValue & 0xffff);

        public static ushort HIWORD(IntPtr dwValue) => (ushort)((((long)dwValue) >> 0x10) & 0xffff);

        public static ushort HIWORD(uint dwValue) => (ushort)(dwValue >> 0x10);

        public static int GET_WHEEL_DELTA_WPARAM(IntPtr wParam) => (short)HIWORD(wParam);

        public static int GET_WHEEL_DELTA_WPARAM(uint wParam) => (short)HIWORD(wParam);

        /// <summary>
        /// Gets the mouse coordinates from lParam.
        /// </summary>
        /// <param name="lParam"></param>
        /// <returns>The coordinates.</returns>
        public static Point PointFromLParam(IntPtr lParam) 
            => new(LOWORD(lParam), HIWORD(lParam));

        public static IntPtr CreateWindowEx(
            int exStyle,
            string className,
            string windowName,
            int style,
            int x, int y,
            int width, int height,
            IntPtr hwndParent,
            IntPtr hMenu,
            IntPtr hInstance,
            object pvParam)
        {
            return NativeMethods.CreateWindowEx(
                exStyle,
                className,
                windowName,
                style,
                x, y,
                width, height,
                hwndParent,
                hMenu,
                hInstance,
                pvParam);
        }

        public static void DestroyWindow(IntPtr hwnd) => NativeMethods.DestroyWindow(hwnd);

        /// <summary>
        /// Place cursor at the specified screen coordinates.
        /// </summary>
        /// <param name="screen">Screen coordinates.</param>
        /// <returns>True if successfult; otherwise, false.</returns>
        public static bool SetCursorPos(Point screen)
            => SetCursorPos((int)screen.X, (int)screen.Y);

        /// <summary>
        /// Place cursor at the specified screen coordinates.
        /// </summary>
        /// <param name="x">Horizontal screen coordinates.</param>
        /// <param name="y">Vertical screen coordinates.</param>
        /// <returns>True if successfult; otherwise, false.</returns>
        public static bool SetCursorPos(int x, int y) => NativeMethods.SetCursorPos(x, y);

        /// <summary>
        /// Try to retrieve the position of the mouse cursor, in screen coordinates.
        /// </summary>
        /// <param name="point">The screen coordinates of the cursor.</param>
        /// <returns>True if successful; otherwise, false.</returns>
        public static bool TryGetCursorPos(out Point point)
        {
            var result = NativeMethods.GetCursorPos(out NativeMethods.POINT pt);
            point = result ? new(pt.X, pt.Y) : new();
            return result;
        }

        /// <summary>
        /// Sets the cursor shape.
        /// </summary>
        /// <param name="cursor">Shape to set.</param>
        /// <returns>The handle to the previous cursor, if there was one.</returns>
        public static IntPtr SetCursor(Cursors cursor)
        {
            var hCursor = NativeMethods.LoadCursor(IntPtr.Zero, (int)cursor);
            return NativeMethods.SetCursor(hCursor);
        }

        /// <summary>
        /// Sets the mouse capture to the specified window.
        /// </summary>
        /// <param name="hwnd">A handle to the window that is to capture the mouse.</param>
        /// <returns>A handle to the window that had previously captured the mouse.</returns>
        public static IntPtr SetCapture(IntPtr hwnd) => NativeMethods.SetCapture(hwnd);

        /// <summary>
        /// Sets the focus to the specified window.
        /// </summary>
        /// <param name="hWnd">A handle to the window that will receive the keyboard input. If this parameter is NULL, keystrokes are ignored.</param>
        /// <returns>A handle to the window that previously had the keyboard focus, or NULL if the hWnd parameter is invalid.</returns>
        public static IntPtr SetFocus(IntPtr hWnd) => NativeMethods.SetFocus(hWnd);

        /// <summary>
        /// Releases the mouse capture from a window in the current thread and restores 
        /// normal mouse input processing.
        /// </summary>
        /// <returns>True if the function succeeds; otherwise, false.</returns>
        public static bool ReleaseCapture() => NativeMethods.ReleaseCapture();

        /// <summary>
        /// Changes the size, position, and Z order of a child, pop-up, or top-level window.
        /// </summary>
        /// <param name="hwnd">A handle to the window.</param>
        /// <param name="hWndInsertAfter">A handle to the window to precede the positioned window in the Z order</param>
        /// <param name="x">The new position of the left side of the window, in client coordinates.</param>
        /// <param name="y">The new position of the top of the window, in client coordinates.</param>
        /// <param name="width">The new width of the window, in pixels.</param>
        /// <param name="height">The new height of the window, in pixels.</param>
        /// <param name="flags">The window sizing and positioning flags.</param>
        /// <returns>True if successful; otherwise, false.</returns>
        public static bool SetWindowPos(
            IntPtr hwnd, IntPtr hWndInsertAfter,
            int x, int y,
            int width, int height,
            uint flags)
        {
            return NativeMethods.SetWindowPos(hwnd, hWndInsertAfter, x, y, width, height, flags);
        }

        public static int SendMessage( 
            IntPtr hwnd,
            int msg,
            IntPtr wParam,
            IntPtr lParam)
        {
            return NativeMethods.SendMessage(hwnd, msg, wParam, lParam);
        }

        public static int SendMessage(
            IntPtr hwnd,
            int msg,
            int wParam,
            StringBuilder lParam)
        {
            return NativeMethods.SendMessage(hwnd, msg, wParam, lParam);
        }

        public static IntPtr SendMessage(
            IntPtr hwnd,
            int msg,
            IntPtr wParam,
            String lParam)
        {
            return NativeMethods.SendMessage(hwnd, msg, wParam, lParam);
        }
    }
}
