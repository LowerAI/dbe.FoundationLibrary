using System.Runtime.InteropServices;

namespace dbe.FoundationLibrary.Core.Win32.Structures
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct LPPOINT
    {
        public int x;
        public int y;
    }
}