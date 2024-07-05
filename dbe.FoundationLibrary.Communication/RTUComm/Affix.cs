//using dbe.FoundationLibrary.Core.DataConvert;

using dbe.FoundationLibrary.Core.Extensions;

using System.Runtime.InteropServices;
using System.Text;

namespace dbe.FoundationLibrary.Communication.RTUComm
{
    [StructLayout(LayoutKind.Sequential, Size = 1)]
    public struct Affix
    {
        /// <summary>
        /// 附缀的Unicode字串形式(string类型)
        /// </summary>
        public string RawString;

        /// <summary>
        /// 附缀的Unicode字串长度
        /// </summary>
        public int RawStringLength;

        /// <summary>
        /// 附缀的字节流形式(byte[]类型)
        /// </summary>
        public byte[] Bytes;

        /// <summary>
        /// 附缀的字节流长度
        /// </summary>
        public int BytesLength;

        /// <summary>
        /// 附缀的16进制字串形式(string类型)
        /// </summary>
        public string HexString;

        /// <summary>
        /// 附缀的16进制字串长度
        /// </summary>
        public int HexStringLength;

        public Affix(string rawString)
        {
            RawString = rawString;
            RawStringLength = RawString.Length;
            Bytes = Encoding.Default.GetBytes(rawString);
            BytesLength = Bytes.Length;
            //HexString = StringLib.GetHexStringFromByteArray(Bytes);
            HexString = Bytes.ToHexString();
            HexStringLength = HexString.Length;
        }
    }
}