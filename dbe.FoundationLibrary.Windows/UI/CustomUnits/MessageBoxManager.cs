#pragma warning disable 0618
using dbe.FoundationLibrary.Core.Win32.API;
using dbe.FoundationLibrary.Core.Win32.Structures;

using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;

[assembly: SecurityPermission(SecurityAction.RequestMinimum, UnmanagedCode = true)]
namespace dbe.FoundationLibrary.Windows.UI.CustomUnits
{
    /// <summary>
    /// 消息框个性化扩展
    /// 原文链接：Localizing System MessageBox - CodeProject https://www.codeproject.com/Articles/18399/Localizing-System-MessageBox
    /// </summary>
    public class MessageBoxManager
    {
        private static HookProc hookProc;
        private static EnumChildProc enumProc;
        [ThreadStatic]
        private static IntPtr hHook;
        [ThreadStatic]
        private static int nButton;

        /// <summary>
        /// OK text
        /// </summary>
        public static string OK = "&OK";
        /// <summary>
        /// Cancel text
        /// </summary>
        public static string Cancel = "&Cancel";
        /// <summary>
        /// Abort text
        /// </summary>
        public static string Abort = "&Abort";
        /// <summary>
        /// Retry text
        /// </summary>
        public static string Retry = "&Retry";
        /// <summary>
        /// Ignore text
        /// </summary>
        public static string Ignore = "&Ignore";
        /// <summary>
        /// Yes text
        /// </summary>
        public static string Yes = "&Yes";
        /// <summary>
        /// No text
        /// </summary>
        public static string No = "&No";

        static MessageBoxManager()
        {
            hookProc = new HookProc(MessageBoxHookProc);
            enumProc = new EnumChildProc(MessageBoxEnumProc);
            hHook = IntPtr.Zero;
        }

        /// <summary>
        /// Enables MessageBoxManager functionality
        /// </summary>
        /// <remarks>
        /// MessageBoxManager functionality is enabled on current thread only.
        /// Each thread that needs MessageBoxManager functionality has to call this method.
        /// </remarks>
        public static void Register()
        {
            if (hHook != IntPtr.Zero)
                throw new NotSupportedException("One hook per thread allowed.");
            hHook = User32.SetWindowsHookEx(Const.WH_CALLWNDPROCRET, hookProc, IntPtr.Zero, AppDomain.GetCurrentThreadId());
        }

        /// <summary>
        /// Disables MessageBoxManager functionality
        /// </summary>
        /// <remarks>
        /// Disables MessageBoxManager functionality on current thread only.
        /// </remarks>
        public static void Unregister()
        {
            if (hHook != IntPtr.Zero)
            {
                User32.UnhookWindowsHookEx(hHook);
                hHook = IntPtr.Zero;
            }
        }

        private static IntPtr MessageBoxHookProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode < 0)
                return User32.CallNextHookEx(hHook, nCode, wParam, lParam);

            CWPRETSTRUCT msg = (CWPRETSTRUCT)Marshal.PtrToStructure(lParam, typeof(CWPRETSTRUCT));
            IntPtr hook = hHook;

            if (msg.message == Const.WM_INITDIALOG)
            {
                int nLength = User32.GetWindowTextLength(msg.hwnd);
                StringBuilder className = new StringBuilder(10);
                User32.GetClassName(msg.hwnd, className, className.Capacity);
                if (className.ToString() == "#32770")
                {
                    nButton = 0;
                    User32.EnumChildWindows(msg.hwnd, enumProc, IntPtr.Zero);
                    if (nButton == 1)
                    {
                        IntPtr hButton = User32.GetDlgItem(msg.hwnd, Const.MB_Cancel);
                        if (hButton != IntPtr.Zero)
                            User32.SetWindowText(hButton, OK);
                    }
                }
            }

            return User32.CallNextHookEx(hook, nCode, wParam, lParam);
        }

        private static bool MessageBoxEnumProc(IntPtr hWnd, IntPtr lParam)
        {
            StringBuilder className = new StringBuilder(10);
            User32.GetClassName(hWnd, className, className.Capacity);
            if (className.ToString() == "Button")
            {
                int ctlId = User32.GetDlgCtrlID(hWnd);
                switch (ctlId)
                {
                    case Const.MB_OK:
                        User32.SetWindowText(hWnd, OK);
                        break;
                    case Const.MB_Cancel:
                        User32.SetWindowText(hWnd, Cancel);
                        break;
                    case Const.MB_Abort:
                        User32.SetWindowText(hWnd, Abort);
                        break;
                    case Const.MB_Retry:
                        User32.SetWindowText(hWnd, Retry);
                        break;
                    case Const.MB_Ignore:
                        User32.SetWindowText(hWnd, Ignore);
                        break;
                    case Const.MB_Yes:
                        User32.SetWindowText(hWnd, Yes);
                        break;
                    case Const.MB_No:
                        User32.SetWindowText(hWnd, No);
                        break;
                }
                nButton++;
            }

            return true;
        }
    }
}