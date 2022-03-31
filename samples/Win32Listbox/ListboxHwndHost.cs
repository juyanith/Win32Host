using System;
using Win32Host;

namespace Win32Listbox
{
    /// <summary>
    /// Hosts a WIN32 Listbox
    /// </summary>
    public class ListboxHwndHost : Win32HwndHost
    {
        internal const int
            WS_CHILD = 0x40000000,
            WS_VISIBLE = 0x10000000,
            LBS_NOTIFY = 0x00000001,
            HOST_ID = 0x00000002,
            LISTBOX_ID = 0x00000001,
            WS_VSCROLL = 0x00200000,
            WS_BORDER = 0x00800000;

        public IntPtr ListboxHwnd { get; private set; }

        protected override void InitializeHostedContent()
        {
            var size = GetScaledWindowSize();
            ListboxHwnd = User32.CreateWindowEx(
                0, "listbox", "",
                WS_CHILD | WS_VISIBLE | LBS_NOTIFY | WS_VSCROLL | WS_BORDER,
                0, 0,
                (int)size.Width, (int)size.Height,
                Hwnd,
                (IntPtr)LISTBOX_ID,
                IntPtr.Zero,
                0);
        }

        protected override void ResizeHostedContent()
        {
            var size = this.GetScaledWindowSize();
            ResizeHwnd((int)size.Width, (int)size.Height);
        }

        protected override void UninitializeHostedContent()
        {
            User32.DestroyWindow(ListboxHwnd);
            ListboxHwnd = IntPtr.Zero;
        }

        protected override IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            handled = false;
            return IntPtr.Zero;
        }
    }
}
