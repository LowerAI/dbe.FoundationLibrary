using System;
using System.ComponentModel;

namespace GNDView.Library.DataConvert
{
    /// <summary>
    /// SByte类型(8位有符号整数，范围-128 ~ 127)数据转换类
    /// </summary>
    [Description("SByte类型数据转换类")]
    public class SByteLib
    {
        /// <summary>
        /// 将字节中的某个位赋值✔
        /// </summary>
        /// <param name="value">原始字节</param>
        /// <param name="offset">位</param>
        /// <param name="bitValue">写入数值</param>
        /// <returns>返回字节</returns>
        [Description("将字节中的某个位赋值")]
        [Obsolete("该方法已弃用，sbyte.SetBit()取代它")]
        public static sbyte SetBitValue(sbyte value, int offset, bool bitValue)
        {
            return bitValue ? (sbyte)(value | (sbyte)Math.Pow(2, offset)) : (sbyte)(value & ~(sbyte)Math.Pow(2, offset));
        }

        /// <summary>
        /// 从字节数组中截取某个字节✔
        /// </summary>
        /// <param name="value">字节数组</param>
        /// <param name="index">开始索引</param>
        /// <returns>返回字节</returns>
        [Description("从字节数组中截取某个字节")]
        [Obsolete("该方法已弃用，byte[].ToSByte()取代它")]
        public static sbyte GetByteFromByteArray(byte[] value, int index)
        {
            if (index > value.Length - 1) throw new ArgumentException("字节数组长度不够或开始索引太大");

            return (sbyte)value[index];
        }

        /// <summary>
        /// 将布尔数组转换成字节数组✔
        /// </summary>
        /// <param name="value">布尔数组</param>
        /// <returns>字节数组</returns>
        [Description("将布尔数组转换成字节数组")]
        [Obsolete("该方法已弃用，bool[].ToSByte()取代它")]
        public static sbyte GetByteFromBoolArray(bool[] value)
        {
            if (value.Length != 8) throw new ArgumentNullException("检查数组长度是否为8");

            sbyte result = 0;

            //遍历当前字节的每个位赋值
            for (int i = 0; i < 8; i++)
            {
                result = SetBitValue(result, i, value[i]);
            }
            return result;
        }
    }
}
