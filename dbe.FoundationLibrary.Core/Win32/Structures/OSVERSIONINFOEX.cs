using System.Runtime.InteropServices;

namespace dbe.FoundationLibrary.Core.Win32.Structures
{
    [StructLayout(LayoutKind.Sequential)]
    public struct OSVERSIONINFOEX
    {
        /// <summary>
        /// 此数据结构的大小（以字节为单位）
        /// </summary>
        public int dwOSVersionInfoSize;
        /// <summary>
        /// 操作系统的主版本号
        /// </summary>
        public int dwMajorVersion;
        /// <summary>
        /// 操作系统的次要版本号
        /// </summary>
        public int dwMinorVersion;
        /// <summary>
        /// 操作系统的内部版本号
        /// </summary>
        public int dwBuildNumber;
        /// <summary>
        /// 操作系统平台
        /// </summary>
        public int dwPlatformId;
        /// <summary>
        /// 以 null 结尾的字符串，例如“Service Pack 3”，指示系统上安装的最新 Service Pack。 如果未安装 Service Pack，则字符串为空。
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string szCSDVersion;
        ///// <summary>
        ///// 系统上安装的最新 Service Pack 的主版本号。 例如，对于 Service Pack 3，主版本号为 3。 如果未安装 Service Pack，则值为零。
        ///// </summary>
        //public int wServicePackMajor;
        ///// <summary>
        ///// 系统上安装的最新 Service Pack 的次要版本号。 例如，对于 Service Pack 3，次要版本号为 0。
        ///// </summary>
        //public int wServicePackMinor;
        ///// <summary>
        ///// 标识系统上可用的产品套件的位掩码。 此成员可以是WindowsVersion值的组合。
        ///// </summary>
        //public int wSuiteMask;
        ///// <summary>
        ///// 有关系统的类型。 此成员可以是WindowsVersion值之一。
        ///// </summary>
        //public byte wProductType;
        ///// <summary>
        ///// 保留供将来使用
        ///// </summary>
        //public byte wReserved;
    }
}