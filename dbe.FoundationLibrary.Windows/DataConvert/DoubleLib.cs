using GNDView.Library.Common;

using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace GNDView.Library.DataConvert
{
    /// <summary>
    /// Double类型(64位单精度浮点数，范围(+/-)5.0x10E-324 ~ (+/-)1.7x10E308)数据转换类
    /// </summary>
    [Description("Double类型数据转换类")]
    public class DoubleLib
    {
        /// <summary>
        /// 将字节数组中某8个字节转换成Double类型✔
        /// </summary>
        /// <param name="value">字节数组</param>
        /// <param name="startIndex">开始位置</param>
        /// <param name="dataFormat">字节顺序</param>
        /// <returns>Double类型数值</returns>
        [Description("将字节数组中某8个字节转换成Double类型")]
        [Obsolete("该方法已弃用，byte[].ToDouble()取代它")]
        public static double GetDoubleFromByteArray(byte[] value, int startIndex = 0, Endianness dataFormat = Endianness.ABCD)
        {
            byte[] data = ByteArrayLib.Get8BytesFromByteArray(value, startIndex, dataFormat);
            return BitConverter.ToDouble(data, 0);
        }

        /// <summary>
        /// 将字节数组转换成Double数组✔
        /// </summary>
        /// <param name="value">字节数组</param>
        /// <param name="dataFormat">字节顺序</param>
        /// <returns>Double数组</returns>
        [Description("将字节数组转换成Double数组")]
        [Obsolete("该方法已弃用，byte[].ToDoubleArray()取代它")]
        public static double[] GetDoubleArrayFromByteArray(byte[] value, Endianness dataFormat = Endianness.ABCD)
        {
            if (value == null) throw new ArgumentNullException("检查数组长度是否为空");

            if (value.Length % 8 != 0) throw new ArgumentNullException("检查数组长度是否为8的倍数");

            double[] values = new double[value.Length / 8];

            for (int i = 0; i < value.Length / 8; i++)
            {
                values[i] = GetDoubleFromByteArray(value, 8 * i, dataFormat);
            }

            return values;
        }

        /// <summary>
        /// 将Double字符串转换成双精度浮点型数组✔
        /// </summary>
        /// <param name="value">Double字符串</param>
        /// <param name="spilt">分割符</param>
        /// <returns>双精度浮点型数组</returns>
        [Description("将Double字符串转换成双精度浮点型数组")]
        [Obsolete("该方法已弃用，string.ToDoubleArray()取代它")]
        public static double[] GetDoubleArrayFromString(string value, string spilt = " ")
        {
            value = value.Trim();
            List<double> result = new List<double>();
            try
            {
                if (value.Contains(spilt))
                {
                    string[] str = value.Split(new string[] { spilt }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (var item in str)
                    {
                        result.Add(Convert.ToDouble(item.Trim()));
                    }
                }
                else
                {
                    result.Add(Convert.ToDouble(value.Trim()));
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentNullException("数据转换失败：" + ex.Message);
            }
            return result.ToArray();
        }
    }
}