using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace Win32Host
{
    /// <summary>
    /// Creates internal Hwnd to which content can be rendered.
    /// </summary>
    public abstract class Win32HwndHost : HwndHost
    {
        protected Win32HwndHost()
        {
            Loaded += FrameworkElement_Loaded;
            Unloaded += FrameworkElement_Unloaded;
        }

        /// <summary>
        /// The child HWND.
        /// </summary>
        public IntPtr Hwnd { get; private set; }

        /// <summary>
        /// The instance handle.
        /// </summary>
        public IntPtr Hinstance { get; private set; }

        /// <summary>
        /// Has hosted content been initialized?
        /// </summary>
        public bool IsContentInitialized { get; private set; }

        /// <summary>
        /// Ready hosted content.
        /// </summary>
        protected abstract void InitializeHostedContent();

        /// <summary>
        /// Cleanup hosted content.
        /// </summary>
        protected abstract void UninitializeHostedContent();

        /// <summary>
        /// Resize hosted content.
        /// </summary>
        protected abstract void ResizeHostedContent();

        /// <summary>
        /// Resize hosted content and raise RenderSizeChanged.
        /// </summary>
        /// <param name="sizeInfo"></param>
        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            UpdateWindowPos();

            base.OnRenderSizeChanged(sizeInfo);

            if (IsContentInitialized) ResizeHostedContent();
        }

        #region Utility

        public double GetDpiScale()
        {
            var source = PresentationSource.FromVisual(this);
            return source.CompositionTarget.TransformToDevice.M11;
        }

        public Size GetScaledWindowSize()
        {
            var scale = GetDpiScale();
            var width = ActualWidth <= 0 ? 0 : Math.Ceiling(ActualWidth * scale);
            var height = ActualHeight <= 0 ? 0 : Math.Ceiling(ActualHeight * scale);
            return new Size(width, height);
        }

        private const int SWP_NOMOVE = 0x0002;
        private const int SWP_NOZORDER = 0x0004;
        protected bool ResizeHwnd(int width, int height)
        {
            return NativeMethods.SetWindowPos(
                Hwnd, IntPtr.Zero, 0, 0,
                width, height,
                SWP_NOMOVE | SWP_NOZORDER);
        }

        #endregion

        #region HwndHost overrides

        /// <summary>
        /// Create child window in which to content will be rendered.
        /// </summary>
        /// <param name="hwndParent"></param>
        /// <returns></returns>
        protected override HandleRef BuildWindowCore(HandleRef hwndParent)
        {
            var className = GetType().Name;
            var wndClass = new NativeMethods.WndClassEx();
            wndClass.cbSize = (uint)Marshal.SizeOf(wndClass);
            wndClass.hInstance = NativeMethods.GetModuleHandle(null);
            wndClass.lpfnWndProc = NativeMethods.DefaultWindowProc;
            wndClass.lpszClassName = className;
            // If this is not null, the cursor is drawn whenever the mouse is moved.
            wndClass.hCursor = IntPtr.Zero;
            NativeMethods.RegisterClassEx(ref wndClass);

            Hwnd = NativeMethods.CreateWindowEx(
                0,
                className,
                "",
                NativeMethods.WS_CHILD | NativeMethods.WS_VISIBLE,
                0,
                0,
                (int)Width, (int)Height,
                hwndParent.Handle,
                IntPtr.Zero, IntPtr.Zero, 0);

            var mainModule = GetType().Module;
            Hinstance = Marshal.GetHINSTANCE(mainModule);

            return new HandleRef(this, Hwnd);
        }

        /// <summary>
        /// Clean up child window.
        /// </summary>
        /// <param name="hwnd"></param>
        protected override void DestroyWindowCore(HandleRef hwnd)
        {
            NativeMethods.DestroyWindow(hwnd.Handle);
            Hwnd = IntPtr.Zero;
            Hinstance = IntPtr.Zero;
        }

        #endregion

        #region Event handlers

        private void FrameworkElement_Loaded(object sender, RoutedEventArgs routedEventArgs)
        {
            InitializeHostedContent();
            IsContentInitialized = true;
        }

        private void FrameworkElement_Unloaded(object sender, RoutedEventArgs routedEventArgs)
        {
            UninitializeHostedContent();
            IsContentInitialized = false;
        }

        #endregion
    }
}
