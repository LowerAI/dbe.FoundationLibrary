using dbe.FoundationLibrary.Core.Common;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace dbe.FoundationLibrary.Core.Extensions
{
    public static class StringExtension
    {
        /// <summary>
        /// 字符串转Enum
        /// C# 字符串（String）转枚举（Enum）_c# 字符串转枚举_末零的博客-CSDN博客 https://blog.csdn.net/n_moling/article/details/124426548
        /// </summary>
        /// <typeparam name="T">枚举</typeparam>
        /// <param name="str">字符串</param>
        /// <returns>转换的枚举</returns>
        public static T ToEnum<T>(this string str)
        {
            T ret = default(T);
            try
            {
                ret = (T)Enum.Parse(typeof(T), str);
            }
            catch
            {
            }
            return ret;
        }

        /// <summary>
        /// 判断字符串是否为空
        /// </summary>
        /// <param name="str"></param>
        /// <returns>true表示为空/false表示不为空</returns>
        public static bool IsEmpty(this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }

        /// <summary>
        /// 判断字符串是否为布尔
        /// </summary>
        /// <param name="value">布尔字符串</param>
        /// <returns>布尔结果</returns>
        [Description("判断是否为布尔")]
        public static bool ToBoolean(this string value)
        {
            return value == "1" || value.ToLower() == "true";
        }

        /// <summary>
        /// 将字符串按照指定的分隔符转换成布尔数组
        /// </summary>
        /// <param name="source">待转换字符串</param>
        /// <param name="spilt">分割符</param>
        /// <returns>返回布尔数组</returns>
        [Description("将字符串按照指定的分隔符转换成布尔数组")]
        public static bool[] ToBitArray(this string source, string spilt = " ")
        {
            source = source.Trim();

            List<bool> result = new List<bool>();

            if (source.Contains(spilt))
            {// 字符串中存在分隔符时，例如“true false true false”或“1 0 0 1 0”
                string[] strings = source.Split(new string[] { spilt }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string item in strings)
                {
                    result.Add(item.ToBoolean());
                }
            }
            else
            {// 字符串中不存在分隔符时，例如“101101101”
                result.Add(source.ToBoolean());
            }

            return result.ToArray();
        }

        /// <summary>
        /// 将字符串转转成16位整型数组
        /// </summary>
        /// <param name="source">带转换字符串</param>
        /// <param name="spilt">分割符</param>
        /// <returns>返回Short数组</returns>
        [Description("将字符串转转成16位整型数组")]
        public static short[] ToShortArray(this string source, string spilt = " ")
        {
            source = source.Trim();
            List<short> result = new List<short>();
            try
            {
                if (source.Contains(spilt))
                {
                    string[] str = source.Split(new string[] { spilt }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (var item in str)
                    {
                        result.Add(Convert.ToInt16(item.Trim()));
                    }
                }
                else
                {
                    result.Add(Convert.ToInt16(source.Trim()));
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentNullException("数据转换失败：" + ex.Message);
            }
            return result.ToArray();
        }

        /// <summary>
        /// 将字符串转转成16位无符号整型数组
        /// </summary>
        /// <param name="source">带转换字符串</param>
        /// <param name="spilt">分割符</param>
        /// <returns>返回Short数组</returns>
        [Description("将字符串转转成16位无符号整型数组")]
        public static ushort[] ToUShortArray(this string source, string spilt = " ")
        {
            source = source.Trim();
            List<ushort> result = new List<ushort>();
            try
            {
                if (source.Contains(spilt))
                {
                    string[] str = source.Split(new string[] { spilt }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (var item in str)
                    {
                        result.Add(Convert.ToUInt16(item.Trim()));
                    }
                }
                else
                {
                    result.Add(Convert.ToUInt16(source.Trim()));
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentNullException("数据转换失败：" + ex.Message);
            }
            return result.ToArray();
        }

        /// <summary>
        /// 将字符串转转成32位整型数组
        /// </summary>
        /// <param name="source">字节数组</param>
        /// <param name="spilt">分隔符</param>
        /// <returns>返回int数组</returns>
        [Description("将字符串转转成32位整型数组")]
        public static int[] ToIntArray(this string source, string spilt = " ")
        {
            source = source.Trim();
            List<int> result = new List<int>();
            try
            {
                if (source.Contains(spilt))
                {
                    string[] str = source.Split(new string[] { spilt }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (var item in str)
                    {
                        result.Add(Convert.ToInt32(item.Trim()));
                    }
                }
                else
                {
                    result.Add(Convert.ToInt32(source.Trim()));
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentNullException("数据转换失败：" + ex.Message);
            }
            return result.ToArray();
        }

        /// <summary>
        /// 将字符串转转成32位无符号整型数组
        /// </summary>
        /// <param name="source">字节数组</param>
        /// <param name="spilt">分隔符</param>
        /// <returns>返回UInt数组</returns>
        [Description("将字符串转转成32位无符号整型数组")]
        public static uint[] ToUIntArray(this string source, string spilt = " ")
        {
            source = source.Trim();
            List<uint> result = new List<uint>();
            try
            {
                if (source.Contains(spilt))
                {
                    string[] str = source.Split(new string[] { spilt }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (var item in str)
                    {
                        result.Add(Convert.ToUInt32(item.Trim()));
                    }
                }
                else
                {
                    result.Add(Convert.ToUInt32(source.Trim()));
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentNullException("数据转换失败：" + ex.Message);
            }
            return result.ToArray();
        }

        /// <summary>
        /// 将字符串转转成64位整型数组
        /// </summary>
        /// <param name="source">字节数组</param>
        /// <param name="spilt">分隔符</param>
        /// <returns>返回Long数组</returns>
        [Description("将字符串转转成64位整型数组")]
        public static long[] ToLongArray(this string source, string spilt = " ")
        {
            source = source.Trim();
            List<long> result = new List<long>();
            try
            {
                if (source.Contains(spilt))
                {
                    string[] str = source.Split(new string[] { spilt }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (var item in str)
                    {
                        result.Add(Convert.ToInt64(item.Trim()));
                    }
                }
                else
                {
                    result.Add(Convert.ToInt64(source.Trim()));
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentNullException("数据转换失败：" + ex.Message);
            }
            return result.ToArray();
        }

        /// <summary>
        /// 将字符串转转成64位无符号整型数组
        /// </summary>
        /// <param name="source">字节数组</param>
        /// <param name="spilt">分隔符</param>
        /// <returns>返回ULong数组</returns>
        [Description("将字符串转转成64位无符号整型数组")]
        public static ulong[] ToULongArray(this string source, string spilt = " ")
        {
            source = source.Trim();
            List<ulong> result = new List<ulong>();
            try
            {
                if (source.Contains(spilt))
                {
                    string[] str = source.Split(new string[] { spilt }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (var item in str)
                    {
                        result.Add(Convert.ToUInt64(item.Trim()));
                    }
                }
                else
                {
                    result.Add(Convert.ToUInt64(source.Trim()));
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentNullException("数据转换失败：" + ex.Message);
            }
            return result.ToArray();
        }

        /// <summary>
        /// 将Float字符串转换成单精度浮点型数组
        /// </summary>
        /// <param name="source">Float字符串</param>
        /// <param name="spilt">分隔符</param>
        /// <returns>单精度浮点型数组</returns>
        [Description("将Float字符串转换成单精度浮点型数组")]
        public static float[] ToFloatArray(this string source, string spilt = " ")
        {
            source = source.Trim();

            List<float> result = new List<float>();

            try
            {
                if (source.Contains(spilt))
                {
                    string[] str = source.Split(new string[] { spilt }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var item in str)
                    {
                        result.Add(Convert.ToSingle(item.Trim()));
                    }
                }
                else
                {
                    result.Add(Convert.ToSingle(source.Trim()));
                }
                return result.ToArray();
            }
            catch (Exception ex)
            {
                throw new ArgumentNullException("数据转换失败：" + ex.Message);
            }
        }

        /// <summary>
        /// 将Double字符串转换成双精度浮点型数组
        /// </summary>
        /// <param name="source">Double字符串</param>
        /// <param name="spilt">分割符</param>
        /// <returns>双精度浮点型数组</returns>
        [Description("将Double字符串转换成双精度浮点型数组")]
        public static double[] ToDoubleArray(this string source, string spilt = " ")
        {
            source = source.Trim();
            List<double> result = new List<double>();
            try
            {
                if (source.Contains(spilt))
                {
                    string[] str = source.Split(new string[] { spilt }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (var item in str)
                    {
                        result.Add(Convert.ToDouble(item.Trim()));
                    }
                }
                else
                {
                    result.Add(Convert.ToDouble(source.Trim()));
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentNullException("数据转换失败：" + ex.Message);
            }
            return result.ToArray();
        }

        /// <summary>
        /// 将指定编码格式的字符串转换成字节数组
        /// </summary>
        /// <param name="value">字符串</param>
        /// <param name="encoding">编码格式</param>
        /// <returns>字节数组</returns>
        [Description("将指定编码格式的字符串转换成字节数组")]
        public static byte[] ToByteArray(this string source, Encoding encoding)
        {
            return encoding.GetBytes(source);
        }

        /// <summary>
        /// 将string数据转换成一个Ascii格式字节数组
        /// </summary>
        /// <param name="source">string数据</param>
        /// <returns>字节数组</returns>
        [Description("将string数据转换成一个Ascii格式字节数组")]
        public static byte[] ToByteArray(this string source, Endianness dataFormat = Endianness.DCBA)
        {
            return source.ToByteArray(Encoding.ASCII).Format(dataFormat);
        }

        /// <summary>
        /// 将16进制字符串按照空格分隔成字节数组
        /// </summary>
        /// <param name="source">16进制字符串</param>
        /// <param name="spilt">分隔符</param>
        /// <returns>字节数组</returns>
        [Description("将16进制字符串按照空格分隔成字节数组")]
        public static byte[] ToByteArrayFromHexString(this string source, string spilt = " ")
        {
            source = source.Trim();//去除空格

            List<byte> result = new List<byte>();
            try
            {
                if (source.Contains(spilt))
                {
                    string[] str = source.Split(new string[] { spilt }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (var item in str)
                    {
                        result.Add(Convert.ToByte(item.Trim(), 16));
                    }
                }
                else
                {
                    result.Add(Convert.ToByte(source.Trim(), 16));
                }
                return result.ToArray();
            }
            catch (Exception ex)
            {
                throw new ArgumentNullException("数据转换失败：" + ex.Message);
            }
        }

        /// <summary>
        /// 将16进制字符串不用分隔符转换成字节数组（每2个字符为1个字节）
        /// </summary>
        /// <param name="source">16进制字符串</param>
        /// <returns>字节数组</returns>
        [Description("将16进制字符串不用分隔符转换成字节数组（每2个字符为1个字节）")]
        public static byte[] ToByteArrayFromHexStringWithoutSpilt(this string source)
        {
            if (source.Length % 2 != 0) throw new ArgumentNullException("检查字符串长度是否为偶数");

            List<byte> result = new List<byte>();
            try
            {
                for (int i = 0; i < source.Length; i += 2)
                {
                    string temp = source.Substring(i, 2);

                    result.Add(Convert.ToByte(temp, 16));
                }
                return result.ToArray();
            }
            catch (Exception ex)
            {
                throw new ArgumentNullException("数据转换失败：" + ex.Message);
            }
        }

        /// <summary>
        /// 将西门子字符串转换成字节数组
        /// </summary>
        /// <param name="source">西门子字符串</param>
        /// <returns>字节数组</returns>
        [Description("将西门子字符串转换成字节数组")]
        public static byte[] ToByteArrayFromSiemensString(this string source)
        {
            byte[] data = source.ToByteArray(Encoding.GetEncoding("GBK"));
            byte[] result = new byte[data.Length + 2];
            result[0] = (byte)(data.Length + 2);
            result[1] = (byte)data.Length;
            Array.Copy(data, 0, result, 2, data.Length);
            return result;
        }

        /// <summary>
        /// 将欧姆龙CIP字符串转换成字节数组
        /// </summary>
        /// <param name="source">西门子字符串</param>
        /// <returns>字节数组</returns>
        [Description("将欧姆龙CIP字符串转换成字节数组")]
        public static byte[] ToByteArrayFromOmronCIPString(this string source)
        {
            byte[] buffer = source.ToByteArray(Encoding.ASCII);

            byte[] res = buffer.ExtendAsEvenByteArray();

            byte[] array = new byte[res.Length + 2];
            array[0] = BitConverter.GetBytes(array.Length - 2)[0];
            array[1] = BitConverter.GetBytes(array.Length - 2)[1];
            Array.Copy(res, 0, array, 2, res.Length);
            return array;
        }
    }
}