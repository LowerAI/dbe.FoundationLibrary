using dbe.FoundationLibrary.Core.Common;

using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace dbe.FoundationLibrary.Core.Extensions
{
    /// <summary>
    /// bool[]的扩展类
    /// </summary>
    public static class BoolArrayExtension
    {
        /// <summary>
        /// 将布尔数组转换成字节数组
        /// </summary>
        /// <param name="source">布尔数组</param>
        /// <returns>字节数组</returns>
        [Description("将布尔数组转换成字节数组")]
        public static byte[] GetByteArrayFromBoolArray(this bool[] source)
        {

            if (source == null || source.Length == 0) throw new ArgumentNullException("检查数组长度是否正确"); ;

            byte[] result = new byte[source.Length % 8 != 0 ? source.Length / 8 + 1 : source.Length / 8];

            //遍历每个字节
            for (int i = 0; i < result.Length; i++)
            {
                int total = source.Length < 8 * (i + 1) ? source.Length - 8 * i : 8;

                //遍历当前字节的每个位赋值
                for (int j = 0; j < total; j++)
                {
                    result[i] = result[i].SetBit(j, source[8 * i + j]);
                }
            }
            return result;
        }

        /// <summary>
        /// 根据位开始和长度截取布尔数组
        /// </summary>
        /// <param name="source">布尔数组</param>
        /// <param name="start">开始索引</param>
        /// <param name="length">长度</param>
        /// <returns>返回布尔数组</returns>
        [Description("根据位开始和长度截取布尔数组")]
        public static bool[] SubBitArray(this bool[] source, int start, int length)
        {
            if (start < 0) throw new ArgumentException("开始索引不能为负数");

            if (length <= 0) throw new ArgumentException("长度必须为正数");

            if (source.Length < (start + length)) throw new ArgumentException("数组长度不够或开始索引太大");

            bool[] result = new bool[length];
            Array.Copy(source, start, result, 0, length);
            //bool[] result = value.Skip(start).Take(length).ToArray();
            return result;
        }

        /// <summary>
        /// 将布尔数组转换成字节
        /// </summary>
        /// <param name="source">布尔数组</param>
        /// <returns>字节数组</returns>
        [Description("将布尔数组转换成字节")]
        public static byte ToByte(this bool[] source)
        {
            if (source.Length != 8) throw new ArgumentNullException("检查数组长度是否为8");

            byte result = 0;

            //遍历当前字节的每个位赋值
            for (int i = 0; i < 8; i++)
            {
                result = result.SetBit(i, source[i]);
            }
            return result;
        }

        /// <summary>
        /// 将char数组转换成字节数组
        /// </summary>
        /// <param name="source">char数组</param>
        /// <param name="dataFormat">字节顺序</param>
        /// <returns>字节数组</returns>
        [Description("将UShort数组转换成字节数组")]
        public static byte[] ToByteArray(this bool[] source, Endianness dataFormat = Endianness.ABCD)
        {
            List<byte> list = new List<byte>();
            foreach (var item in source)
            {
                list.AddRange(BitConverter.GetBytes(item));
            }
            var ret = list.ToArray();
            return ret.Format(dataFormat);
        }

        /// <summary>
        /// 将布尔数组转换成字节
        /// </summary>
        /// <param name="value">布尔数组</param>
        /// <returns>字节数组</returns>
        [Description("将布尔数组转换成字节")]
        public static sbyte ToSByte(this bool[] source)
        {
            if (source.Length != 8) throw new ArgumentNullException("检查数组长度是否为8");

            sbyte result = 0;

            //遍历当前字节的每个位赋值
            for (int i = 0; i < 8; i++)
            {
                result = result.SetBit(i, source[i]);
            }
            return result;
        }
    }
}