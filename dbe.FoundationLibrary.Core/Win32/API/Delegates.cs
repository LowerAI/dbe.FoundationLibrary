using System;

namespace dbe.FoundationLibrary.Core.Win32.API
{
    public delegate IntPtr HookProc(int nCode, IntPtr wParam, IntPtr lParam);


    public delegate bool EnumChildProc(IntPtr hWnd, IntPtr lParam);
}