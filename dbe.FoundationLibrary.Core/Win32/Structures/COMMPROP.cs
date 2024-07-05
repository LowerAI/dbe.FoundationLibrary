using System.Runtime.InteropServices;

namespace dbe.FoundationLibrary.Core.Win32.Structures
{
    /// <summary>
    /// 端口属性信息
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct COMMPROP
    {
        public ushort wPacketLength;
        public ushort wPacketVersion;
        public uint dwServiceMask;
        public uint dwReserved1;
        public uint dwMaxTxQueue;
        public uint dwMaxRxQueue;
        public uint dwMaxBaud;
        public uint dwProvSubType;
        public uint dwProvCapabilities;
        public uint dwSettableParams;
        public uint dwSettableBaud;
        public ushort wSettableData;
        public ushort wSettableStopParity;
        public uint dwCurrentTxQueue;
        public uint dwCurrentRxQueue;
        public uint dwProvSpec1;
        public uint dwProvSpec2;
        public char wcProvChar;
    }
}