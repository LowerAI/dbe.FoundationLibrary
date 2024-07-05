using System;
using System.ComponentModel;

namespace GNDView.Library.DataConvert
{
    /// <summary>
    /// Byte类型(8位无符号整数，范围0 ~ 255)数据转换类
    /// </summary>
    [Description("Byte类型数据转换类")]
    public class ByteLib
    {
        /// <summary>
        /// 将字节中的某个位赋值✔
        /// </summary>
        /// <param name="value">原始字节</param>
        /// <param name="offset">位</param>
        /// <param name="bitValue">写入数值</param>
        /// <returns>返回字节</returns>
        [Description("将字节中的某个位赋值")]
        [Obsolete("该方法已弃用，byte.SetBit()取代它")]
        public static byte SetBitValue(byte value, int offset, bool bitValue)
        {
            return bitValue ? (byte)(value | (byte)Math.Pow(2, offset)) : (byte)(value & ~(byte)Math.Pow(2, offset));
        }

        /// <summary>
        /// 从字节数组中截取某个字节
        /// </summary>
        /// <param name="value">字节数组</param>
        /// <param name="index">开始索引</param>
        /// <returns>返回字节</returns>
        [Description("从字节数组中截取某个字节")]
        [Obsolete("该方法已弃用，byte[][]取代它")]
        public static byte GetByteFromByteArray(byte[] value, int index)
        {
            if (index > value.Length - 1) throw new ArgumentException("字节数组长度不够或开始索引太大");

            return value[index];
        }

        /// <summary>
        /// 将布尔数组转换成字节数组✔
        /// </summary>
        /// <param name="value">布尔数组</param>
        /// <returns>字节数组</returns>
        [Description("将布尔数组转换成字节数组")]
        [Obsolete("该方法已弃用，bool[].ToByte()取代它")]
        public static byte GetByteFromBoolArray(bool[] value)
        {
            if (value.Length != 8) throw new ArgumentNullException("检查数组长度是否为8");

            byte result = 0;

            //遍历当前字节的每个位赋值
            for (int i = 0; i < 8; i++)
            {
                result = SetBitValue(result, i, value[i]);
            }
            return result;
        }
    }
}
