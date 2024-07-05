using System.Runtime.InteropServices;

namespace dbe.FoundationLibrary.Core.Win32.Structures
{
    [StructLayout(LayoutKind.Sequential)]
    public struct SystemTime
    {
        public short wYear;
        public short wMonth;
        public short wDayOfWeek;
        public short wDay;
        public short wHour;
        public short wMinute;
        public short wSecond;
        public short wMilliseconds;
    }
}