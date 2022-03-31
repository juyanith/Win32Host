using System;
using System.Text;
using System.Windows;

using static Win32Host.User32;

namespace Win32Listbox
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void AppendText(object sender, EventArgs args)
        {
            if (!string.IsNullOrEmpty(txtAppend.Text))
            {
                SendMessage(listboxHost.ListboxHwnd, LB_ADDSTRING, IntPtr.Zero, txtAppend.Text);
            }
            itemCount = SendMessage(listboxHost.ListboxHwnd, LB_GETCOUNT, IntPtr.Zero, IntPtr.Zero);
            numItems.Text = "" + itemCount.ToString();
        }

        private void DeleteText(object sender, EventArgs args)
        {
            selectedItem = SendMessage(listboxHost.ListboxHwnd, LB_GETCURSEL, IntPtr.Zero, IntPtr.Zero);
            if (selectedItem != -1) //check for selected item
            {
                SendMessage(listboxHost.ListboxHwnd, LB_DELETESTRING, (IntPtr)selectedItem, IntPtr.Zero);
            }
            itemCount = SendMessage(listboxHost.ListboxHwnd, LB_GETCOUNT, IntPtr.Zero, IntPtr.Zero);
            numItems.Text = "" + itemCount.ToString();
        }

        int itemCount;
        int selectedItem;

        private IntPtr ControlMsgFilter(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            int textLength;

            handled = false;
            if (msg == WM_COMMAND)
            {
                switch ((uint)wParam.ToInt32() >> 16 & 0xFFFF) //extract the HIWORD
                {
                    case LBN_SELCHANGE: //Get the item text and display it
                        selectedItem = SendMessage(listboxHost.ListboxHwnd, LB_GETCURSEL, IntPtr.Zero, IntPtr.Zero);
                        textLength = SendMessage(listboxHost.ListboxHwnd, LB_GETTEXTLEN, IntPtr.Zero, IntPtr.Zero);
                        StringBuilder itemText = new StringBuilder();
                        SendMessage(listboxHost.ListboxHwnd, LB_GETTEXT, selectedItem, itemText);
                        selectedText.Text = itemText.ToString();
                        handled = true;
                        break;
                }
            }
            return IntPtr.Zero;
        }
    }
}
