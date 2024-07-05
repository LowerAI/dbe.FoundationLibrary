using System;
using System.ComponentModel;

namespace dbe.FoundationLibrary.Core.Extensions
{
    /// <summary>
    /// sbyte的扩展类
    /// </summary>
    public static class SByteExtension
    {
        /// <summary>
        /// 将字节中的某个位赋值
        /// </summary>
        /// <param name="source">源字节</param>
        /// <param name="offset">位</param>
        /// <param name="bitValue">写入数值</param>
        /// <returns>返回字节</returns>
        [Description("将字节中的某个位赋值")]
        public static sbyte SetBit(this sbyte source, int offset, bool bitValue)
        {
            return bitValue ? (sbyte)(source | (sbyte)Math.Pow(2, offset)) : (sbyte)(source & ~(sbyte)Math.Pow(2, offset));
        }

        /// <summary>
        /// 返回sbyte值对应的byte值
        /// </summary>
        /// <param name="source">源字节</param>
        /// <returns>byte值</returns>
        [Description("返回对应的byte值")]
        public static byte ToByte(this sbyte source)
        {
            return (byte)(source & 0xFF);
        }
    }
}