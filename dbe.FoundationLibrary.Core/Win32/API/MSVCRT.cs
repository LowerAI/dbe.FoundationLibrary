using System.Runtime.InteropServices;

namespace dbe.FoundationLibrary.Core.Win32.API
{
    /// <summary>
    /// "msvcrt.dll"中的Api
    /// </summary>
    public class MSVCRT
    {
        [DllImport("msvcrt.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int ByteEquals(byte[] b1, byte[] b2, long count);
    }
}