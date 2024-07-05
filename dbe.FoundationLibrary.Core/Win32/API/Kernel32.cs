using dbe.FoundationLibrary.Core.Win32.Structures;

using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace dbe.FoundationLibrary.Core.Win32.API
{
    /// <summary>
    /// "kernel32.dll"中的Api
    /// </summary>
    public class Kernel32
    {
        [DllImport("kernel32.dll")]
        public static extern IntPtr _lopen(string lpPathName, int iReadWrite);

        /// <summary>
        /// 打开控制台
        /// </summary>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        public static extern bool AllocConsole();

        /// <summary>
        /// 控制蜂鸣器发声
        /// </summary>
        /// <param name="dwFreq">声音频率（从37Hz到32767Hz）,在windows95中忽略</param>
        /// <param name="dwDuration">声音的持续时间，以毫秒为单位</param>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        public static extern int Beep(int dwFreq, int dwDuration);

        /// <summary>
        /// 关闭程序句柄
        /// </summary>
        /// <param name="hObject"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        public static extern bool CloseHandle(IntPtr hObject);

        /// <summary>
        /// 释放控制台
        /// </summary>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        public static extern bool FreeConsole();

        /// <summary>
        /// 获取串口属性
        /// </summary>
        /// <param name="hFile"></param>
        /// <param name="lpCommProp"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool GetCommProperties(IntPtr hFile, ref COMMPROP lpCommProp);

        /// <summary>
        /// 获取系统时间
        /// </summary>
        /// <param name="st"></param>
        [DllImport("kernel32.dll", EntryPoint = "GetLocalTime", SetLastError = true)]
        public static extern void GetLocalTime(out SystemTime st);

        /// <summary>
        /// 获取操作系统版本信息
        /// </summary>
        /// <param name="osVersionInfo"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        public static extern bool GetVersionEx(ref OSVERSIONINFOEX osVersionInfo);

        /// <summary>
        /// 获取所有段落名称
        /// </summary>
        /// <param name="lpszReturnBuffer">接受数据内存缓冲区指针</param>
        /// <param name="nSize">数据内存缓冲区最大大小</param>
        /// <param name="lpFileName">ini文件的路径</param>
        /// <returns></returns>
        [SuppressUnmanagedCodeSecurity]
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern int GetPrivateProfileSectionNames(IntPtr lpszReturnBuffer, uint nSize, string lpFileName);

        /// <summary>
        /// 获取值(String)
        /// </summary>
        /// <param name="lpAppName">段落名称</param>
        /// <param name="lpKeyName">键名称</param>
        /// <param name="lpDefault">找不到时返回的值</param>
        /// <param name="lpReturnedString">接受数据字符串</param>
        /// <param name="nSize">数据内存缓冲区最大大小</param>
        /// <param name="lpFileName">ini文件的路径</param>
        /// <returns></returns>
        [SuppressUnmanagedCodeSecurity]
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern uint GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault, StringBuilder lpReturnedString, int nSize, string lpFileName);

        /// <summary>
        /// 获取值(String)
        /// </summary>
        /// <param name="lpAppName">段落名称</param>
        /// <param name="lpKeyName">键名称</param>
        /// <param name="lpDefault">找不到时返回的值</param>
        /// <param name="retVal">要读取的数据</param>
        /// <param name="nSize">数据内存缓冲区最大大小</param>
        /// <param name="lpFileName">ini文件的路径</param>
        /// <returns></returns>
        [SuppressUnmanagedCodeSecurity]
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern int GetPrivateProfileString(byte[] section, byte[] key, byte[] def, byte[] retVal, int size, string filePath);

        /// <summary>
        /// 获取值(char[])
        /// </summary>
        /// <param name="lpAppName">段落名称</param>
        /// <param name="lpKeyName">键名称</param>
        /// <param name="lpDefault">找不到时返回的值</param>
        /// <param name="lpReturnedString">接受数据字符串</param>
        /// <param name="nSize">数据内存缓冲区最大大小</param>
        /// <param name="lpFileName">ini文件的路径</param>
        /// <returns></returns>
        [SuppressUnmanagedCodeSecurity]
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern uint GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault, [In, Out] char[] lpReturnedString, int nSize, string lpFileName);

        /// <summary>
        /// 获取值(IntPtr)
        /// </summary>
        /// <param name="lpAppName">段落名称</param>
        /// <param name="lpKeyName">键名称</param>
        /// <param name="lpDefault">找不到时返回的值</param>
        /// <param name="lpReturnedString">接受数据字符串指针</param>
        /// <param name="nSize">数据内存缓冲区最大大小</param>
        /// <param name="lpFileName">ini文件的路径</param>
        /// <returns></returns>
        [SuppressUnmanagedCodeSecurity]
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern int GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault, IntPtr lpReturnedString, uint nSize, string lpFileName);

        /// <summary>
        /// 获取值(IntPtr)
        /// </summary>
        /// <param name="lpAppName">段落名称</param>
        /// <param name="lpKeyName">键名称</param>
        /// <param name="lpDefault">找不到时返回的值</param>
        /// <param name="lpReturnedString">接受数据字符串指针</param>
        /// <param name="nSize">数据内存缓冲区最大大小</param>
        /// <param name="lpFileName">ini文件的路径</param>
        /// <returns></returns>
        [SuppressUnmanagedCodeSecurity]
        [DllImport("kernel32.dll")]
        public static extern uint GetPrivateProfileStringA(string lpAppName, string lpKeyName, string lpDefault, Byte[] retVal, int nSize, string lpFileName);

        /// <summary>
        /// 获取值(Int)
        /// </summary>
        /// <param name="lpAppName">段落名称</param>
        /// <param name="lpKeyName">键名称</param>
        /// <param name="lpDefault">找不到时返回的值</param>
        /// <param name="lpFileName">ini文件的路径</param>
        /// <returns></returns>
        [SuppressUnmanagedCodeSecurity]
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern int GetPrivateProfileInt(string lpAppName, string lpKeyName, int lpDefault, string lpFileName);

        /// <summary>
        /// 获取指定段落
        /// </summary>
        /// <param name="lpAppName">段落名称</param>
        /// <param name="lpReturnedString">接受数据字符串指针</param>
        /// <param name="nSize">数据内存缓冲区最大大小</param>
        /// <param name="lpFileName">ini文件的路径</param>
        /// <returns></returns>
        [SuppressUnmanagedCodeSecurity]
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern int GetPrivateProfileSection(string lpAppName, IntPtr lpReturnedString, uint nSize, string lpFileName);

        [DllImport("kernel32.dll", EntryPoint = "GetSystemTime", SetLastError = true)]
        internal static extern void GetSystemTime(out SystemTime st);

        [DllImport("kernel32.dll")]
        public static extern bool IsWindows10OrGreater();

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool VerifyVersionInfo(ref OSVERSIONINFOEX lpVersionInformation, uint dwTypeMask, ulong dwlConditionMask);

        /// <summary>
        /// 写入失败，未找到原因
        /// </summary>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        [SuppressUnmanagedCodeSecurity]
        [DllImport("kernel32.dll")]
        public static extern bool WritePrivateProfileString(byte[] section, byte[] key, byte[] val, string filePath);

        /// <summary>
        /// 获取值(String)(WritePrivateProfileString通过SetLastError返回错误)
        /// </summary>
        /// <param name="lpAppName">段落名称</param>
        /// <param name="lpKeyName">键名称</param>
        /// <param name="lpString">键对应值</param>
        /// <param name="lpFileName">ini文件的路径</param>
        /// <returns></returns>
        [SuppressUnmanagedCodeSecurity]
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool WritePrivateProfileString(string lpAppName, string lpKeyName, string lpString, string lpFileName);
    }
}