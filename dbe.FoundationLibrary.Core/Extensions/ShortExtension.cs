using dbe.FoundationLibrary.Core.Common;

using System;
using System.ComponentModel;
using System.Text;

namespace dbe.FoundationLibrary.Core.Extensions
{
    /// <summary>
    /// Short的扩展类
    /// </summary>
    public static class ShortExtension
    {
        /// <summary>
        /// 根据一个Short返回指定位
        /// </summary>
        /// <param name="source">short数值</param>
        /// <param name="offset">偏移位</param>
        /// <param name="isLittleEndian">大小端</param>
        /// <remarks>偏移位0-15有效，否则结果不正确</remarks>
        /// <returns>布尔结果</returns>
        [Description("根据一个Short返回指定位")]
        public static bool GetBit(this short source, int offset, bool isLittleEndian = true)
        {
            return BitConverter.GetBytes(source).GetBitFrom2BytesArray(offset, !isLittleEndian);
        }

        /// <summary>
        /// 设置16位整型某个位
        /// </summary>
        /// <param name="source">Short数据</param>
        /// <param name="offset">某个位</param>
        /// <param name="bitVal">True或者False</param>
        /// <param name="dataFormat">数据格式</param>
        /// <returns>返回Short结果</returns>
        [Description("设置16位整型某个位")]
        public static short SetBit(this short source, int offset, bool bitVal, Endianness dataFormat = Endianness.ABCD)
        {
            byte[] data = source.ToByteArray(dataFormat);

            return data.SetBitAndToShort(offset, bitVal, dataFormat);
        }

        /// <summary>
        /// 将Short类型数值转换成字节数组
        /// </summary>
        /// <param name="source">Short类型数值</param>
        /// <param name="dataFormat">字节顺序</param>
        /// <returns>字节数组</returns>
        [Description("将Short类型数值转换成字节数组")]
        public static byte[] ToByteArray(this short source, Endianness dataFormat = Endianness.ABCD)
        {
            byte[] resTemp = BitConverter.GetBytes(source);
            byte[] res = resTemp.Format(dataFormat);
            return res;
        }

        /// <summary>
        /// 将short数据转换成一个Ascii格式字节数组
        /// </summary>
        /// <param name="source">short数据</param>
        /// <returns>字节数组</returns>
        [Description("将short数据转换成一个Ascii格式字节数组")]
        public static byte[] ToAsciiByteArray(this short source)
        {
            return Encoding.ASCII.GetBytes(source.ToString("X4"));
        }
    }
}