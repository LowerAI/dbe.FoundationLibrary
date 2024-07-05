using System;
using System.Runtime.InteropServices;

namespace dbe.FoundationLibrary.Core.Win32.Structures
{
    [StructLayout(LayoutKind.Sequential)]
    public struct CWPRETSTRUCT
    {
        public IntPtr lResult;
        public IntPtr lParam;
        public IntPtr wParam;
        public uint message;
        public IntPtr hwnd;
    };
}