using System;
using System.Runtime.InteropServices;
using System.Text;

namespace dbe.FoundationLibrary.Core.Win32.API
{
    /// <summary>
    /// "shell32.dll"中的Api
    /// </summary>
    public class Shell32
    {
        /// <summary>
        /// 调用Windows命令程序
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="lpszOp"></param>
        /// <param name="lpszFile"></param>
        /// <param name="lpszParams"></param>
        /// <param name="lpszDir"></param>
        /// <param name="FsShowCmd"></param>
        /// <returns></returns>
        [DllImport("shell32.dll")]
        public static extern int ShellExecute(IntPtr hwnd, StringBuilder lpszOp, StringBuilder lpszFile, StringBuilder lpszParams, StringBuilder lpszDir, int FsShowCmd);
    }
}