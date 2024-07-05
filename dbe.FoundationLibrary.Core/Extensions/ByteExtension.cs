using System;
using System.ComponentModel;
using System.Text;

namespace dbe.FoundationLibrary.Core.Extensions
{
    /// <summary>
    /// Byte的扩展类
    /// </summary>
    public static class ByteExtension
    {
        /// <summary>
        /// 清0
        /// </summary>
        /// <param name="byt"></param>
        /// <param name="index"></param>
        public static byte ClearBit(this byte byt, int index)
        {
            return byt &= (byte)(~(1 << index));
        }

        /// <summary>
        /// 清除字节的某些位
        /// </summary>
        /// <param name="byt">字节数组</param>
        /// <param name="index">起始位置</param>
        /// <param name="len">长度</param>
        public static byte ClearBit(this byte byt, int index, int length)
        {
            byte mask = GetMask(index, length);
            return byt &= (byte)(~mask);
        }

        /// <summary>
        /// 返回某个字节的指定位
        /// </summary>
        /// <param name="source">字节</param>
        /// <param name="offset">偏移位</param>
        /// <remarks>偏移位0-7有效，否则结果不正确</remarks>
        /// <returns>布尔结果</returns>
        [Description("返回某个字节的指定位")]
        public static bool GetBitToBool(this byte source, int offset)
        {
            return (source & (1 << offset)) != 0;

            //return (data & (byte)Math.Pow(2, offset)) != 0;
        }

        /// <summary>
        /// 获取某个位
        /// </summary>
        /// <param name="byt"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static byte GetBitToByte(this byte byt, int index)
        {
            return (byte)((byt >> index) & 1);
        }

        /// <summary>
        /// 获取字节的某些位
        /// </summary>
        /// <param name="byt">字节数组</param>
        /// <param name="index">起始位置</param>
        /// <param name="len">长度</param>
        /// <returns></returns>
        public static byte GetBitToByte(this byte byt, int index, int length)
        {
            byte mask = GetMask(length);
            return (byte)((byt >> index) & mask);
        }

        /// <summary>
        /// 获取屏蔽位
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static byte GetMask(int length)
        {
            return (byte)((1 << length) - 1);
        }

        /// <summary>
        /// 获取屏蔽位
        /// </summary>
        /// <param name="index"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static byte GetMask(int index, int length)
        {
            byte mask = GetMask(length);
            return (byte)(mask << index);
        }

        /// <summary>
        /// 取反
        /// </summary>
        /// <param name="byt"></param>
        /// <param name="index"></param>
        /// <param name="length"></param>
        public static byte ReverseBit(this byte byt, int index, int length)
        {
            byte mask = GetMask(index, length);
            return byt ^= mask;
        }

        /// <summary>
        /// 某位取反
        /// </summary>
        /// <param name="byt"></param>
        /// <param name="index"></param>
        public static byte ReverseBit(this byte byt, int index)
        {
            return byt ^= (byte)(1 << index);
        }

        /// <summary>
        /// 将字节中的某个位赋值
        /// </summary>
        /// <param name="source">原始字节</param>
        /// <param name="offset">位</param>
        /// <param name="bitValue">写入数值</param>
        /// <returns>返回字节</returns>
        [Description("将字节中的某个位赋值")]
        public static byte SetBit(this byte source, int offset, bool bitValue)
        {
            return bitValue ? (byte)(source | (byte)Math.Pow(2, offset)) : (byte)(source & ~(byte)Math.Pow(2, offset));
        }

        /// <summary>
        /// 某位置1
        /// </summary>
        /// <param name="byt"></param>
        /// <param name="index"></param>
        public static byte SetBit(this byte byt, int index)
        {
            return byt |= (byte)(1 << index);
        }

        /// <summary>
        /// 某些位置1
        /// </summary>
        /// <param name="byt"></param>
        /// <param name="index"></param>
        /// <param name="length"></param>
        public static byte SetBit(this byte byt, int index, int length)
        {
            byte mask = GetMask(index, length);
            return byt |= (byte)mask;
        }

        /// <summary>
        /// 设置字节的某些位
        /// </summary>
        /// <param name="byt">字节数组</param>
        /// <param name="index">起始位置</param>
        /// <param name="len">长度</param>
        /// <param name="value">设置值</param>
        public static byte SetBit(this byte byt, int index, int length, int value)
        {
            int mask = GetMask(length);
            byt = byt.ClearBit(index, length);
            return byt |= (byte)((value & mask) << index);
        }

        /// <summary>
        /// 将byte数据转换成一个Asii格式字节数组
        /// </summary>
        /// <param name="source">byte数据</param>
        /// <returns>字节数组</returns>
        [Description("将byte数据转换成一个Asii格式字节数组")]
        public static byte[] ToAsciiByteArray(this byte source)
        {
            return Encoding.ASCII.GetBytes(source.ToString("X2"));
        }

        /// <summary>
        /// 将一个字节转换成布尔数组
        /// </summary>
        /// <param name="value">字节</param>
        /// <returns>布尔数组</returns>
        [Description("将一个字节转换成布尔数组")]
        public static bool[] ToBitArray(this byte value)
        {
            return new byte[] { value }.ToBitArray();
        }

        /// <summary>
        /// 将单个字节转换成字节数组 ByteArrayLib.GetByteArrayFromByte
        /// </summary>
        /// <param name="source">单个字节</param>
        /// <returns>字节数组</returns>
        [Description("将单个字节转换成字节数组")]
        public static byte[] ToByteArray(this byte source)
        {
            return new byte[] { source };
        }
    }
}