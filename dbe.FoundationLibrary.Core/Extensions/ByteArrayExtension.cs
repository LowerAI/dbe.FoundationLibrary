using dbe.FoundationLibrary.Core.Common;
using dbe.FoundationLibrary.Core.Util;

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace dbe.FoundationLibrary.Core.Extensions
{
    /// <summary>
    /// Bytes数组的扩展类
    /// </summary>
    public static class ByteArrayExtension
    {
        #region        byte[] => bool start
        /// <summary>
        /// 与buffer比较长度是否一致且元素值是否全部相等 ByteArrayLib.Equals
        /// </summary>
        /// <param name="source">数组1</param>
        /// <param name="buffer">数组2</param>
        /// <returns>true:相等，false:不相等</returns>
        public static bool IsEqual(this byte[] source, byte[] buffer)
        {
            var len1 = source.Length;
            var len2 = buffer.Length;
            if (len1 != len2)
            {
                return false;
            }
            for (var i = 0; i < len1; i++)
            {
                if (source[i] != buffer[i])
                    return false;
            }
            return true;
        }

        /// <summary>
        /// 返回字节数组中某个字节的指定位
        /// </summary>
        /// <param name="source">字节数组</param>
        /// <param name="start">字节索引</param>
        /// <param name="offset">偏移位</param>
        /// <remarks>偏移位0-7有效，否则结果不正确</remarks> 
        /// <returns>布尔结果</returns>
        [Description("返回字节数组中某个字节的指定位")]
        public static bool GetBitToBool(this byte[] source, int start, int offset)
        {
            if (start > source.Length - 1) throw new ArgumentException("数组长度不够或开始索引太大");

            return source[start].GetBitToBool(offset);
        }

        /// <summary>
        /// 获取高低字节的指定位
        /// </summary>
        /// <param name="high">高位字节</param>
        /// <param name="low">低位字节</param>
        /// <param name="offset">偏移位</param>
        /// <remarks>偏移位0-15有效，否则结果不正确</remarks>
        /// <returns>布尔结果</returns>
        [Description("获取高低字节的指定位")]
        private static bool GetBitFrom2Bytes(byte high, byte low, int offset)
        {
            if (offset >= 0 && offset <= 7)
            {
                //return GetBitFromByte(low, offset);
                return low.GetBitToBool(offset);
            }
            else
            {
                //return GetBitFromByte(high, offset - 8);
                return high.GetBitToBool(offset - 8);
            }
        }

        /// <summary>
        /// 获取字节数组(长度为2)中的指定位
        /// </summary>
        /// <param name="source">字节数组</param>
        /// <param name="offset">偏移位</param>
        /// <param name="isLittleEndian">大小端：true表示小端/false表示大端</param>
        /// <remarks>偏移位0-15有效，否则结果不正确</remarks>
        /// <returns>布尔结果</returns>
        [Description("获取字节数组(长度为2)中的指定位")]
        public static bool GetBitFrom2BytesArray(this byte[] source, int offset, bool isLittleEndian = true)

        {
            if (source.Length < 2) throw new ArgumentException("数组长度小于2");

            if (isLittleEndian)
            {
                return GetBitFrom2Bytes(source[1], source[0], offset);
            }
            else
            {
                return GetBitFrom2Bytes(source[0], source[1], offset);
            }
        }

        /// <summary>
        /// 返回字节数组中某2个字节的指定位
        /// </summary>
        /// <param name="source">字节数组</param>
        /// <param name="start">字节索引</param>
        /// <param name="offset">偏移位</param>
        /// <param name="isLittleEndian">大小端</param>
        /// <remarks>偏移位0-15有效，否则结果不正确</remarks> 
        /// <returns>布尔结果</returns>
        [Description("返回字节数组中某2个字节的指定位")]
        public static bool GetBitFrom2BytesArray(this byte[] source, int start, int offset, bool isLittleEndian = true)
        {
            if (start > source.Length - 2) throw new ArgumentException("数组长度不够或开始索引太大");

            byte[] array = new byte[] { source[start], source[start + 1] };

            return array.GetBitFrom2BytesArray(offset, isLittleEndian);
        }
        #endregion byte[] => bool end

        #region        byte[] => bool[] start
        /// <summary>
        /// 将字节数组转换成布尔数组 => BitLib.GetBitArrayFromByteArray
        /// </summary>
        /// <param name="source">字节数组</param>
        /// <param name="length">布尔数组长度</param>
        /// <returns>布尔数组</returns>
        [Description("将字节数组转换成布尔数组")]
        public static bool[] ToBitArray(this byte[] source, int length)
        {
            return ToBitArray(source, 0, length);
        }

        /// <summary>
        /// 将字节数组转换成布尔数组 => BitLib.GetBitArrayFromByteArray
        /// </summary>
        /// <param name="source">字节数组</param>
        /// <param name="start">开始索引</param>
        /// <param name="length">布尔数组长度</param>
        /// <returns>布尔数组</returns>
        [Description("将字节数组转换成布尔数组")]
        public static bool[] ToBitArray(this byte[] source, int start, int length)
        {
            if (length <= 0) throw new ArgumentException("长度必须为正数");

            if (start < 0) throw new ArgumentException("开始索引必须为非负数");

            if (start + length > source.Length * 8) throw new ArgumentException("数组长度不够或长度太大");

            var bitArr = new BitArray(source);

            var bools = new bool[length];

            for (var i = 0; i < length; i++)
            {
                bools[i] = bitArr[i + start];
            }
            return bools;
        }

        /// <summary>
        /// 将字节数组转换成布尔数组 => BitLib.GetBitArrayFromByteArray
        /// </summary>
        /// <param name="value">字节数组</param>
        /// <returns>布尔数组</returns>
        [Description("将字节数组转换成布尔数组")]
        public static bool[] ToBitArray(this byte[] value)
        {
            return value.ToBitArray(value.Length * 8);
        }
        #endregion byte[] => bool[] end

        #region        byte[] => byte start

        #endregion byte[] => byte end

        #region        byte[] => byte[] start
        private static readonly List<int> LenLst = new List<int> { 2, 4, 8 };
        /// <summary>
        /// 返回源数组的拷贝
        /// </summary>
        /// <param name="source">源数组</param>
        /// <returns>源数组的拷贝</returns>
        [Description("返回源数组的拷贝")]
        public static byte[] Copy(this byte[] source)
        {
            var dest = new byte[source.Length];
            Array.Copy(source, 0, dest, 0, source.Length);
            //Buffer.BlockCopy(source, 0, dest, 0, length);
            return dest;
        }

        /// <summary>
        /// 根据指定的字节顺序格式化源数组
        /// </summary>
        /// <param name="source">源数组</param>
        /// <param name="format">字节顺序</param>
        /// <param name="dataFormat">字节顺序，默认为大端(ABCD)</param>
        /// <returns>格式化之后的字节数组</returns>
        /// <exception cref="ArgumentException"></exception>
        [Description("根据指定的字节顺序格式化源数组")]
        public static byte[] Format0(this byte[] source, Endianness endianness)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            var dataLen = source.Length;
            var result = new byte[dataLen];
            switch (dataLen)
            {
                case 2:
                    switch (endianness)
                    {
                        case Endianness.ABCD:
                        case Endianness.CDAB:
                            result[0] = source[1];
                            result[1] = source[0];
                            break;
                        case Endianness.BADC:
                        case Endianness.DCBA:
                            result = source;
                            break;
                        default:
                            break;
                    }
                    break;
                case 4:
                    switch (endianness)
                    {
                        case Endianness.ABCD:
                            result[0] = source[3];
                            result[1] = source[2];
                            result[2] = source[1];
                            result[3] = source[0];
                            break;
                        case Endianness.CDAB:
                            result[0] = source[1];
                            result[1] = source[0];
                            result[2] = source[3];
                            result[3] = source[2];
                            break;
                        case Endianness.BADC:
                            result[0] = source[2];
                            result[1] = source[3];
                            result[2] = source[0];
                            result[3] = source[1];
                            break;
                        case Endianness.DCBA:
                            result = source;
                            break;
                    }
                    break;
                case 8:
                    switch (endianness)
                    {
                        case Endianness.ABCD:
                            result[0] = source[7];
                            result[1] = source[6];
                            result[2] = source[5];
                            result[3] = source[4];
                            result[4] = source[3];
                            result[5] = source[2];
                            result[6] = source[1];
                            result[7] = source[0];
                            break;
                        case Endianness.CDAB:
                            result[0] = source[1];
                            result[1] = source[0];
                            result[2] = source[3];
                            result[3] = source[2];
                            result[4] = source[5];
                            result[5] = source[4];
                            result[6] = source[7];
                            result[7] = source[6];
                            break;
                        case Endianness.BADC:
                            result[0] = source[6];
                            result[1] = source[7];
                            result[2] = source[4];
                            result[3] = source[5];
                            result[4] = source[2];
                            result[5] = source[3];
                            result[6] = source[0];
                            result[7] = source[1];
                            break;
                        case Endianness.DCBA:
                            result = source;
                            break;
                    }
                    break;
                default:
                    throw new ArgumentException("参数长度不符合要求", nameof(source));
            }

            return result;
        }

        /// <summary>
        /// 根据指定的字节顺序格式化源数组
        /// </summary>
        /// <param name="source">源数组</param>
        /// <param name="format">字节顺序</param>
        /// <param name="dataFormat">字节顺序，默认为大端(ABCD)</param>
        /// <returns>格式化之后的字节数组</returns>
        /// <exception cref="ArgumentException"></exception>
        [Description("根据指定的字节顺序格式化源数组")]
        public static byte[] Format(this byte[] source, Endianness endianness)
        {
            if (source == null || source.Length == 0 || !source.Length.IsPowerOf2())
            {
                throw new ArgumentException("源数组不能为空，且长度必须是2的幂");
            }

            var dataLen = source.Length;
            var result = new byte[dataLen];

            switch (endianness)
            {
                case Endianness.ABCD:
                    Array.Copy(source, result, dataLen);
                    Array.Reverse(result);
                    break;
                case Endianness.CDAB:
                    Array.Copy(source, result, dataLen);
                    for (int i = 0; i < source.Length; i += 2)
                    {
                        Array.Reverse(result, i, 2);
                    }
                    break;
                case Endianness.BADC:
                    int offset = 0;
                    for (int i = 0; i < dataLen; i += 2)
                    {
                        offset = offset == 0 ? dataLen - 2 : offset - 2;
                        result[i] = source[offset];
                        result[i + 1] = source[offset + 1];
                    }
                    break;
                case Endianness.DCBA:
                    // 不需要调整字节顺序
                    result = source;
                    break;
                default:
                    throw new ArgumentException("未知的字节顺序");
            }

            return result;
        }

        /// <summary>
        /// 根据起始地址和长度自定义截取字节数组 ByteArrayLib.GetByteArrayFromByteArray
        /// </summary>
        /// <param name="source">源字节数组</param>
        /// <param name="start">起始索引，为正表示正向截取/为负表示反向截取</param>
        /// <param name="length">截取长度</param>
        /// <param name="dataFormat">字节顺序，默认为小端(DCBA)</param>
        /// <returns>截取的字节数组</returns>
        [Description("根据起始地址和长度自定义截取字节数组")]
        public static byte[] Subbytes(this byte[] source, int start, int length, Endianness endianness = Endianness.DCBA)
        {
            //if (start < 0) throw new ArgumentException("开始索引不能为负数");

            if (length <= 0) throw new ArgumentException("长度必须为正数");

            if (start >= 0)
            {
                if (source.Length < (start + length)) throw new ArgumentException("字节数组长度不够或开始索引太大");
            }
            else
            {
                if (source.Length < (Math.Abs(1 + start) + length)) throw new ArgumentException("字节数组长度不够或开始索引太大");
                start = source.Length + start - length + 1;
            }

            byte[] result = new byte[length];
            Array.Copy(source, start, result, 0, length);
            //Buffer.BlockCopy(data, start, result, 0, length); 

            //if (LenLst.Contains(length))
            if (length.IsPowerOf2())
            {
                result = result.Format(endianness);
            }

            return result;
        }

        /// <summary>
        /// 根据起始地址自定义截取字节数组 ByteArrayLib.GetByteArrayFromByteArray
        /// </summary>
        /// <param name="source">源字节数组</param>
        /// <param name="start">起始索引，为正表示正向截取/为负表示反向截取</param>
        /// <param name="dataFormat">字节顺序，默认为大端(ABCD)</param>
        /// <returns>截取的字节数组</returns>
        [Description("根据起始地址自定义截取字节数组")]
        public static byte[] Subbytes(this byte[] source, int start, Endianness endianness = Endianness.DCBA)
        {
            return Subbytes(source, start, source.Length - start, endianness);
        }

        /// <summary>
        /// 从字节数组中截取2个字节,并按指定字节序返回 ByteArrayLib.Get2BytesFromByteArray
        /// </summary>
        /// <param name="source">源字节数组</param>
        /// <param name="start">起始索引，为正表示正向截取/为负表示反向截取</param>
        /// <param name="dataFormat">字节顺序，默认为大端(ABCD)</param>
        /// <returns>截取的字节数组</returns> 
        [Description("从字节数组中截取2个字节,并按指定字节序返回")]
        public static byte[] Sub2bytes(this byte[] source, int start = 0, Endianness endianness = Endianness.DCBA)
        {
            return source.Subbytes(start, 2, endianness);
        }

        /// <summary>
        /// 从字节数组中截取4个字节,并按指定字节序返回 ByteArrayLib.Get4BytesFromByteArray
        /// </summary>
        /// <param name="source">源字节数组</param>
        /// <param name="start">起始索引，为正表示正向截取/为负表示反向截取</param>
        /// <param name="dataFormat">字节顺序，默认为大端(ABCD)</param>
        /// <returns>截取的字节数组</returns>
        [Description("从字节数组中截取4个字节,并按指定字节序返回")]
        public static byte[] Sub4bytes(this byte[] source, int start = 0, Endianness endianness = Endianness.DCBA)
        {
            return Subbytes(source, start, 4, endianness);
        }

        /// <summary>
        /// 从字节数组中截取8个字节,并按指定字节序返回 ByteArrayLib.Get8BytesFromByteArray
        /// </summary>
        /// <param name="source">源字节数组</param>
        /// <param name="start">起始索引，为正表示正向截取/为负表示反向截取</param>
        /// <param name="dataFormat">字节顺序，默认为大端(ABCD)</param>
        /// <returns>截取的字节数组</returns>
        [Description("从字节数组中截取8个字节,并按指定字节序返回")]
        public static byte[] Sub8bytes(this byte[] source, int start = 0, Endianness endianness = Endianness.DCBA)
        {
            return Subbytes(source, start, 8, endianness);
        }

        /// <summary>
        /// 将字节数组中的某个数据修改
        /// </summary>
        /// <param name="source">字节数组</param>
        /// <param name="value">数据，确定好类型</param>
        /// <param name="start">开始索引</param>
        /// <param name="offset">偏移，布尔及字符串才起作用</param>
        /// <returns>返回字节数组</returns>
        [Description("将字节数组中的某个数据修改")]
        public static byte[] SetByte(this byte[] source, object value, int start, int offset)
        {
            byte[] b = null;
            string name = value.GetType().Name;
            switch (name.ToLower())
            {
                case "boolean":
                    //Array.Copy(ByteArrayLib.GetByteArrayFromByte(ByteLib.SetBitValue(source[start], offset, Convert.ToBoolean(value))), 0, source, start, 1);
                    Array.Copy(source[start].SetBit(offset, Convert.ToBoolean(value)).ToByteArray(), 0, source, start, 1);
                    break;
                case "byte":
                    //Array.Copy(ByteArrayLib.GetByteArrayFromByte(Convert.ToByte(value)), 0, source, start, 1);
                    Array.Copy(Convert.ToByte(value).ToByteArray(), 0, source, start, 1);
                    break;
                case "int16":
                    //Array.Copy(ByteArrayLib.GetByteArrayFromShort(Convert.ToInt16(value)), 0, source, start, 2);
                    Array.Copy(Convert.ToInt16(value).ToByteArray(), 0, source, start, 2);
                    break;
                case "uint16":
                    //Array.Copy(ByteArrayLib.GetByteArrayFromUShort(Convert.ToUInt16(value)), 0, source, start, 2);
                    Array.Copy(Convert.ToUInt16(value).ToByteArray(), 0, source, start, 2);
                    break;
                case "int32":
                    //Array.Copy(ByteArrayLib.GetByteArrayFromInt(Convert.ToInt32(value)), 0, source, start, 4);
                    Array.Copy(Convert.ToInt32(value).ToByteArray(), 0, source, start, 4);
                    break;
                case "uint32":
                    //Array.Copy(ByteArrayLib.GetByteArrayFromUInt(Convert.ToUInt32(value)), 0, source, start, 4);
                    Array.Copy(Convert.ToUInt32(value).ToByteArray(), 0, source, start, 4);
                    break;
                case "single":
                    //Array.Copy(ByteArrayLib.GetByteArrayFromFloat(Convert.ToSingle(value)), 0, source, start, 4);
                    Array.Copy(Convert.ToSingle(value).ToByteArray(), 0, source, start, 4);
                    break;
                case "double":
                    //Array.Copy(ByteArrayLib.GetByteArrayFromDouble(Convert.ToDouble(value)), 0, source, start, 8);
                    Array.Copy(Convert.ToDouble(value).ToByteArray(), 0, source, start, 8);
                    break;
                case "int64":
                    //Array.Copy(ByteArrayLib.GetByteArrayFromLong(Convert.ToInt64(value)), 0, source, start, 8);
                    Array.Copy(Convert.ToInt64(value).ToByteArray(), 0, source, start, 8);
                    break;
                case "uint64":
                    //Array.Copy(ByteArrayLib.GetByteArrayFromULong(Convert.ToUInt64(value)), 0, source, start, 8);
                    Array.Copy(Convert.ToUInt64(value).ToByteArray(), 0, source, start, 8);
                    break;
                case "byte[]":
                    //b = ByteArrayLib.GetByteArrayFromHexString(value.ToString());
                    b = value.ToString().ToByteArrayFromHexString();
                    Array.Copy(b, 0, source, start, b.Length);
                    break;
                case "int16[]":
                    //b = GetByteArrayFromShortArray(ShortLib.GetShortArrayFromString(value.ToString()));
                    Array.Copy(b, 0, source, start, b.Length);
                    break;
                case "uint16[]":
                    //b = GetByteArrayFromUShortArray(UShortLib.GetUShortArrayFromString(value.ToString()));
                    Array.Copy(b, 0, source, start, b.Length);
                    break;
                case "int32[]":
                    //b = GetByteArrayFromIntArray(IntLib.GetIntArrayFromString(value.ToString()));
                    Array.Copy(b, 0, source, start, b.Length);
                    break;
                case "uint32[]":
                    //b = GetByteArrayFromUIntArray(UIntLib.GetUIntArrayFromString(value.ToString()));
                    Array.Copy(b, 0, source, start, b.Length);
                    break;
                case "single[]":
                    //b = ByteArrayLib.GetByteArrayFromFloatArray(FloatLib.GetFloatArrayFromString(value.ToString()));
                    b = value.ToString().ToFloatArray().ToByteArray();
                    Array.Copy(b, 0, source, start, b.Length);
                    break;
                case "double[]":
                    //b = GetByteArrayFromDoubleArray(DoubleLib.GetDoubleArrayFromString(value.ToString()));
                    Array.Copy(b, 0, source, start, b.Length);
                    break;
                case "int64[]":
                    //b = GetByteArrayFromLongArray(LongLib.GetLongArrayFromString(value.ToString()));
                    Array.Copy(b, 0, source, start, b.Length);
                    break;
                case "uint64[]":
                    //b = GetByteArrayFromULongArray(ULongLib.GetULongArrayFromString(value.ToString()));
                    Array.Copy(b, 0, source, start, b.Length);
                    break;
                default:
                    break;
            }

            return source;
        }

        /// <summary>
        /// 扩展为偶数长度字节数组
        /// </summary>
        /// <param name="source">原始字节数据</param>
        /// <returns>返回字节数组</returns>
        [Description("扩展为偶数长度字节数组")]
        public static byte[] ExtendAsEvenByteArray(this byte[] source)
        {
            if (source == null) return new byte[0];

            if (source.Length % 2 != 0)
                return source.ToFixedLengthByteArray(source.Length + 1);
            else
                return source;
        }

        /// <summary>
        /// 扩展或压缩字节数组到指定数量
        /// </summary>
        /// <param name="source">原始字节数据</param>
        /// <param name="length">指定长度</param>
        /// <returns>返回字节数组</returns>
        [Description("扩展或压缩字节数组到指定数量")]
        public static byte[] ToFixedLengthByteArray(this byte[] source, int length)
        {
            if (source == null) return new byte[length];

            if (source.Length == length) return source;

            byte[] buffer = new byte[length];

            Array.Copy(source, buffer, Math.Min(source.Length, buffer.Length));

            return buffer;
        }

        /// <summary>
        /// 将字节数组转换成Ascii字节数组
        /// </summary>
        /// <param name="source">字节数组</param>
        /// <param name="segment">分隔符</param>
        /// <returns>ASCII字节数组</returns>
        [Description("将字节数组转换成Ascii字节数组")]
        public static byte[] ToAsciiByteArray(this byte[] source, string segment = "")
        {
            return Encoding.ASCII.GetBytes(source.ToHexString(segment));
        }

        ///// <summary>
        ///// 将Ascii字节数组转换成字节数组
        ///// </summary>
        ///// <param name="value">ASCII字节数组</param>
        ///// <returns>字节数组</returns>
        //[Description("将Ascii字节数组转换成字节数组")]
        //public static byte[] ToByteArray(byte[] value)
        //{
        //    return Encoding.ASCII.GetString(value).ToByteArrayFromHexStringWithoutSpilt();
        //}

        /// <summary>
        /// 返回格式化后的字节数组
        /// </summary>
        /// <param name="source">源字节数组</param>
        /// <param name="endianness">格式化枚举值</param>
        /// <returns>格式化后的字节数组</returns>
        [Description("返回格式化后的字节数组")]
        public static byte[] ToByteArray(this byte[] source, Endianness endianness = Endianness.DCBA)
        {
            return source.Format(endianness);
        }

        /// <summary>
        /// 获取对buffer进行指定类型的crc之后的校验值
        /// </summary>
        /// <param name="source">要校验的数据块</param>
        /// <param name="type">指定的crc类型</param>
        /// <returns>校验值</returns>
        public static byte[] GetCRC(this byte[] source, CrcType type, Endianness endianness = Endianness.DCBA)
        {
            return CheckSum.GetCRC(type, source, endianness);
        }

        /// <summary>
        /// 获取对buffer进行modebus类型的crc16之后的校验值
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static byte[] GetCRC16(this byte[] source, Endianness endianness = Endianness.DCBA)
        {
            return CheckSum.GetCRC(CrcType.CRC16_MODBUS, source, endianness);
        }

        /// <summary>
        /// 获取对buffer进行mpeg2类型的crc32之后的校验值
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static byte[] GetCRC32(this byte[] source, Endianness endianness = Endianness.DCBA)
        {
            return CheckSum.GetCRC(CrcType.CRC32_MPEG2, source, endianness);
        }

        /// <summary>
        /// 查短表法计算CRC-16/MODBUS
        /// </summary>
        /// <param name="source">要校验的数据块</param>
        /// <returns>双字节校验值</returns>
        public static byte[] CRC16ForModbus(this byte[] source)
        {
            return CheckSum.CRC16ForModbus(source, 0, source.Length);
        }

        /// <summary>
        /// 将2个字节数组进行合并 => ByteArrayLib.GetByteArrayFromTwoByteArray
        /// </summary>
        /// <param name="source">源数组</param>
        /// <param name="bytes2">字节数组2</param>
        /// <returns>返回字节数组</returns>
        [Description("将2个字节数组进行合并")]
        public static byte[] Union(this byte[] source, byte[] bytes2)
        {
            if (source == null && bytes2 == null) return null;
            if (source == null) return bytes2;
            if (bytes2 == null) return source;

            byte[] buffer = new byte[source.Length + bytes2.Length];
            source.CopyTo(buffer, 0);
            bytes2.CopyTo(buffer, source.Length);
            return buffer;
        }

        /// <summary>
        /// 将3个字节数组进行合并 => ByteArrayLib.GetByteArrayFromThreeByteArray
        /// </summary>
        /// <param name="source">源数组</param>
        /// <param name="bytes2">字节数组2</param>
        /// <param name="bytes3">字节数组3</param>
        /// <returns>返回字节数组</returns>
        [Description("将3个字节数组进行合并")]
        public static byte[] Union(this byte[] source, byte[] bytes2, byte[] bytes3)
        {
            return source.Union(bytes2).Union(bytes3);
        }
        #endregion byte[] => byte[] end

        #region        byte[] => double start
        /// <summary>
        /// 将字节数组中某8个字节转换成Double类型
        /// </summary>
        /// <param name="source">字节数组</param>
        /// <param name="startIndex">开始位置</param>
        /// <param name="dataFormat">字节顺序</param>
        /// <returns>Double类型数值</returns>
        [Description("将字节数组中某8个字节转换成Double类型")]
        public static double ToDouble(this byte[] source, int startIndex = 0, Endianness dataFormat = Endianness.ABCD)
        {
            byte[] data = source.Sub8bytes(startIndex, dataFormat);
            return BitConverter.ToDouble(data, 0);
        }
        #endregion byte[] => double end

        #region        byte[] => double[] start
        /// <summary>
        /// 将字节数组转换成Double数组
        /// </summary>
        /// <param name="source">字节数组</param>
        /// <param name="dataFormat">字节顺序</param>
        /// <returns>Double数组</returns>
        [Description("将字节数组转换成Double数组")]
        public static double[] ToDoubleArray(this byte[] source, Endianness dataFormat = Endianness.ABCD)
        {
            if (source == null) throw new ArgumentNullException("检查数组长度是否为空");

            if (source.Length % 8 != 0) throw new ArgumentNullException("检查数组长度是否为8的倍数");

            double[] values = new double[source.Length / 8];

            for (int i = 0; i < source.Length / 8; i++)
            {
                values[i] = source.ToDouble(8 * i, dataFormat);
            }

            return values;
        }
        #endregion byte[] => double[] end

        #region        byte[] => float start
        /// <summary>
        /// 将字节数组中某4个字节转换成Float类型
        /// </summary>
        /// <param name="source">字节数组</param>
        /// <param name="startIndex">开始索引</param>
        /// <param name="dataFormat">数据格式</param>
        /// <returns>返回一个浮点数</returns>
        [Description("将字节数组中某4个字节转换成Float类型")]
        public static float ToFloat(this byte[] source, int startIndex = 0, Endianness dataFormat = Endianness.ABCD)
        {
            byte[] b = source.Sub4bytes(startIndex, dataFormat);
            return BitConverter.ToSingle(b, 0);
        }
        #endregion byte[] => float end

        #region        byte[] => float[] start
        /// <summary>
        /// 将字节数组转换成Float数组
        /// </summary>
        /// <param name="source">字节数组</param>
        /// <param name="dataFormat">数据格式</param>
        /// <returns>返回浮点数组</returns>
        [Description("将字节数组转换成Float数组")]
        public static float[] ToFloatArray(this byte[] source, Endianness dataFormat = Endianness.ABCD)
        {
            if (source == null) throw new ArgumentNullException("检查数组长度是否为空");

            if (source.Length % 4 != 0) throw new ArgumentNullException("检查数组长度是否为4的倍数");

            float[] values = new float[source.Length / 4];

            for (int i = 0; i < source.Length / 4; i++)
            {
                values[i] = source.ToFloat(4 * i, dataFormat);
            }

            return values;
        }
        #endregion byte[] => float[] end

        #region        byte[] => int start
        /// <summary>
        /// 字节数组中截取转成32位整型
        /// </summary>
        /// <param name="source">字节数组</param>
        /// <param name="startIndex">开始索引</param>
        /// <param name="dataFormat">数据格式</param>
        /// <returns>返回int类型</returns>
        [Description("字节数组中截取转成32位整型")]
        public static int ToInt(this byte[] source, int startIndex = 0, Endianness dataFormat = Endianness.ABCD)
        {
            byte[] data = source.Sub4bytes(startIndex, dataFormat);
            return BitConverter.ToInt32(data, 0);
        }
        #endregion byte[] => int end

        #region        byte[] => int[] start
        /// <summary>
        /// 将字节数组中截取转成32位整型数组
        /// </summary>
        /// <param name="source">字节数组</param>
        /// <param name="dataFormat">数据格式</param>
        /// <returns>返回int数组</returns>
        [Description("将字节数组中截取转成32位整型数组")]
        public static int[] ToIntArray(this byte[] source, Endianness dataFormat = Endianness.ABCD)
        {
            if (source == null) throw new ArgumentNullException("检查数组长度是否为空");

            if (source.Length % 4 != 0) throw new ArgumentNullException("检查数组长度是否为4的倍数");

            int[] values = new int[source.Length / 4];

            for (int i = 0; i < source.Length / 4; i++)
            {
                values[i] = source.ToInt(4 * i, dataFormat);
            }

            return values;
        }
        #endregion byte[] => int[] end

        #region        byte[] => long start
        /// <summary>
        /// 字节数组中截取转成64位整型
        /// </summary>
        /// <param name="source">字节数组</param>
        /// <param name="startIndex">开始索引</param>
        /// <param name="dataFormat">数据格式</param>
        /// <returns>返回一个Long类型</returns>
        [Description("字节数组中截取转成64位整型")]
        public static long ToLong(this byte[] source, int startIndex = 0, Endianness dataFormat = Endianness.ABCD)
        {
            byte[] data = source.Sub8bytes(startIndex, dataFormat);
            return BitConverter.ToInt64(data, 0);
        }
        #endregion byte[] => long end

        #region        byte[] => long[] start
        /// <summary>
        /// 将字节数组中截取转成64位整型数组
        /// </summary>
        /// <param name="source">字节数组</param>
        /// <param name="dataFormat">数据格式</param>
        /// <returns>返回Long数组</returns>
        [Description("将字节数组中截取转成64位整型数组")]
        public static long[] ToLongArray(this byte[] source, Endianness dataFormat = Endianness.ABCD)
        {
            if (source == null) throw new ArgumentNullException("检查数组长度是否为空");

            if (source.Length % 8 != 0) throw new ArgumentNullException("检查数组长度是否为8的倍数");

            long[] values = new long[source.Length / 8];

            for (int i = 0; i < source.Length / 8; i++)
            {
                values[i] = source.ToLong(8 * i, dataFormat);
            }
            return values;
        }
        #endregion byte[] => long[] end

        #region        byte[] => object
        /// <summary>
        /// 反序列化回对象
        /// </summary>
        /// <param name="source">byte[]字节序列</param>
        /// <returns>对象object</returns>
        public static object Deserialize(this byte[] source)
        {
            object data = null;
            try
            {
                MemoryStream streamMemory;
                BinaryFormatter formatter = new BinaryFormatter();
                streamMemory = new MemoryStream(source);
                data = formatter.Deserialize(streamMemory);
            }
            catch
            {
                data = null;
            }
            return data;
        }
        #endregion byte[] => object end

        #region        byte[] => sbyte start
        /// <summary>
        /// 从字节数组中截取某个字节
        /// </summary>
        /// <param name="source">字节数组</param>
        /// <param name="index">开始索引</param>
        /// <returns>返回字节</returns>
        [Description("从字节数组中截取某个字节")]
        public static sbyte ToSByte(this byte[] source, int index)
        {
            if (index > source.Length - 1) throw new ArgumentException("字节数组长度不够或开始索引太大");

            return (sbyte)source[index];
        }
        #endregion byte[] => sbyte end

        #region        byte[] => short start
        /// <summary>
        /// 字节数组中截取转成16位整型
        /// </summary>
        /// <param name="source">字节数组</param>
        /// <param name="startIndex">开始索引</param>
        /// <param name="dataFormat">数据格式</param>
        /// <returns>返回Short结果</returns>
        [Description("字节数组中截取转成16位整型")]
        public static short ToShort(this byte[] source, int startIndex = 0, Endianness dataFormat = Endianness.ABCD)
        {
            byte[] data = source.Sub2bytes(startIndex, dataFormat);
            return BitConverter.ToInt16(data, 0);
        }

        /// <summary>
        /// 设置字节数组某个位 => ShortLib.SetBitValueFrom2ByteArray
        /// </summary>
        /// <param name="source">字节数组</param>
        /// <param name="offset">某个位</param>
        /// <param name="bitVal">True或者False</param>
        /// <param name="dataFormat">数据格式</param>
        /// <returns>返回short结果</returns>
        [Description("设置字节数组某个位")]
        public static short SetBitAndToShort(this byte[] source, int offset, bool bitVal, Endianness dataFormat = Endianness.ABCD)
        {
            if (offset >= 0 && offset <= 7)
            {
                source[1] = source[1].SetBit(offset, bitVal);
            }
            else
            {
                source[0] = source[0].SetBit(offset - 8, bitVal);
            }
            return source.ToShort(0, dataFormat);
        }
        #endregion byte[] => short end

        #region       byte[] => short[] start
        /// <summary>
        /// 将字节数组中截取转成16位整型数组
        /// </summary>
        /// <param name="source">字节数组</param>
        /// <param name="type">数据格式</param>
        /// <returns>返回Short数组</returns>
        [Description("将字节数组中截取转成16位整型数组")]
        public static short[] ToShortArray(this byte[] source, Endianness type = Endianness.ABCD)
        {
            if (source == null) throw new ArgumentNullException("检查数组长度是否为空");

            if (source.Length % 2 != 0) throw new ArgumentNullException("检查数组长度是否为偶数");

            short[] result = new short[source.Length / 2];

            for (int i = 0; i < result.Length; i++)
            {
                result[i] = source.ToShort(i * 2, type);
            }
            return result;
        }
        #endregion byte[] => short[] end

        #region       byte[] => string start
        /// <summary>
        /// 将字节数组转换成字符串
        /// </summary>
        /// <param name="source">字节数组</param>
        /// <param name="start">开始索引</param>
        /// <param name="count">数量</param>
        /// <returns>16进制格式的字符串</returns>
        [Description("将字节数组转换成字符串")]
        public static string ToStringByBitConvert(this byte[] source, int start, int count)
        {
            return BitConverter.ToString(source, start, count);
        }

        /// <summary>
        /// 将字节数组转换成字符串
        /// </summary>
        /// <param name="source">字节数组</param>
        /// <returns>16进制格式的字符串</returns>
        [Description("将字节数组转换成字符串")]
        public static string ToStringByBitConvert(this byte[] source)
        {
            return source.ToStringByBitConvert(0, source.Length);
        }

        /// <summary>
        /// 将字节数组转换成带编码格式字符串
        /// </summary>
        /// <param name="source">字节数组</param>
        /// <param name="start">开始索引</param>
        /// <param name="count">数量</param>
        /// <param name="encoding">编码格式</param>
        /// <returns>指定编码格式的字符串</returns>
        [Description("将字节数组转换成带编码格式字符串")]
        public static string ToStringByEncoding(this byte[] source, int start, int count, Encoding encoding, bool isReverse = false)
        {
            var dest = source.Subbytes(start, count).Copy();
            if (isReverse)
            {
                dest = dest.Reverse().ToArray();
            }
            return encoding.GetString(dest);
        }

        /// <summary>
        /// 将字节数组转换成带编码格式字符串
        /// </summary>
        /// <param name="source">字节数组</param>
        /// <param name="start">开始索引</param>
        /// <param name="count">数量</param>
        /// <returns>指定编码格式的字符串</returns>
        [Description("将字节数组转换成带编码格式字符串")]
        public static string ToStringByEncoding(this byte[] source, int start, int count, bool isReverse = false)
        {
            return ToStringByEncoding(source, start, count, Encoding.Default, isReverse);
        }

        /// <summary>
        /// 将字节数组转换成带编码格式字符串
        /// </summary>
        /// <param name="source">字节数组</param>
        /// <param name="encoding">编码格式</param>
        /// <returns>指定编码格式的字符串</returns>
        [Description("将字节数组转换成带编码格式字符串")]
        public static string ToStringByEncoding(this byte[] source, Encoding encoding, bool isReverse = false)
        {
            var dest = source.Copy();
            if (isReverse)
            {
                dest = dest.Reverse().ToArray();
            }
            return encoding.GetString(dest);
        }

        /// <summary>
        /// 将字节数组转换成带编码格式字符串
        /// </summary>
        /// <param name="source">字节数组</param>
        /// <returns>指定编码格式的字符串</returns>
        [Description("将字节数组转换成带编码格式字符串")]
        public static string ToStringByEncoding(this byte[] source, bool isReverse = false)
        {
            return ToStringByEncoding(source, Encoding.Default, isReverse);
        }

        /// <summary>
        /// 根据起始地址和长度将字节数组转换成带16进制字符串
        /// </summary>
        /// <param name="value">字节数组</param>
        /// <param name="start">开始索引</param>
        /// <param name="count">数量</param>
        /// <param name="segment">连接符</param>
        /// <returns>转换结果：正常时应该是16进制字符串</returns>
        [Description("根据起始地址和长度将字节数组转换成带16进制字符串")]
        public static string ToHexString(this byte[] source, int start, int count, string segment = " ")
        {
            if (source == null || source.Length == 0)
            {
                return string.Empty;
            }
            var result = BitConverter.ToString(source, start, count);
            result = result.Replace("-", segment);
            return result;
        }

        /// <summary>
        /// 将整个字节数组转换成带16进制字符串
        /// </summary>
        /// <param name="source">字节数组</param>
        /// <param name="segment">连接符</param>
        /// <returns>转换结果</returns>
        [Description("将整个字节数组转换成带16进制字符串")]
        public static string ToHexString(this byte[] source, string segment = " ")
        {
            return source.ToHexString(0, source.Length, segment);
        }

        /// <summary>
        /// 将字节数组转换成西门子字符串
        /// </summary>
        /// <param name="source">字节数组</param>
        /// <param name="start">开始索引</param>
        /// <param name="length">长度</param>
        /// <param name="emptyStr">如果为空，显示什么内容</param>
        /// <returns>转换结果</returns>
        [Description("将字节数组转换成西门子字符串")]
        public static string ToSiemensString(this byte[] source, int start, int length, string emptyStr = "empty")
        {
            byte[] data = source.Subbytes(start, length + 2);

            int valid = data[1];

            if (valid > 0)
            {
                return Encoding.GetEncoding("GBK").GetString(source.Subbytes(2, valid));
            }
            else
            {
                return emptyStr;
            }
        }

        #endregion byte[] => string end

        #region        byte[] => uint start
        /// <summary>
        /// 字节数组中截取转成32位无符号整型
        /// </summary>
        /// <param name="value">字节数组</param>
        /// <param name="startIndex">开始索引</param>
        /// <param name="dataFormat">数据格式</param>
        /// <returns>返回UInt类型</returns>
        [Description("字节数组中截取转成32位无符号整型")]
        public static uint ToUInt(this byte[] source, int startIndex = 0, Endianness dataFormat = Endianness.ABCD)
        {
            byte[] data = source.Sub4bytes(startIndex, dataFormat);
            return BitConverter.ToUInt32(data, 0);
        }
        #endregion byte[] => uint end

        #region        byte[] => uint[] start
        /// <summary>
        /// 将字节数组中截取转成32位无符号整型数组
        /// </summary>
        /// <param name="source">字节数组</param>
        /// <param name="dataFormat">数据格式</param>
        /// <returns>返回int数组</returns>
        [Description("将字节数组中截取转成32位无符号整型数组")]
        public static uint[] ToUIntArray(this byte[] source, Endianness dataFormat = Endianness.ABCD)
        {
            if (source == null) throw new ArgumentNullException("检查数组长度是否为空");

            if (source.Length % 4 != 0) throw new ArgumentNullException("检查数组长度是否为4的倍数");

            uint[] values = new uint[source.Length / 4];

            for (int i = 0; i < source.Length / 4; i++)
            {
                values[i] = source.ToUInt(4 * i, dataFormat);
            }

            return values;
        }
        #endregion byte[] => uint[] end

        #region        byte[] => ulong start
        /// <summary>
        /// 字节数组中截取转成64位无符号整型
        /// </summary>
        /// <param name="source">字节数组</param>
        /// <param name="startIndex">开始索引</param>
        /// <param name="dataFormat">数据格式</param>
        /// <returns>返回一个ULong类型</returns>
        [Description("字节数组中截取转成64位无符号整型")]
        public static ulong ToULong(this byte[] source, int startIndex = 0, Endianness dataFormat = Endianness.ABCD)
        {
            byte[] data = source.Sub8bytes(startIndex, dataFormat);
            return BitConverter.ToUInt64(data, 0);
        }
        #endregion byte[] => ulong end

        #region        byte[] => ulong[] start
        /// <summary>
        /// 将字节数组中截取转成64位无符号整型数组
        /// </summary>
        /// <param name="source">字节数组</param>
        /// <param name="dataFormat">数据格式</param>
        /// <returns>返回Long数组</returns>
        [Description("将字节数组中截取转成64位无符号整型数组")]
        public static ulong[] ToULongArray(this byte[] source, Endianness dataFormat = Endianness.ABCD)
        {
            if (source == null) throw new ArgumentNullException("检查数组长度是否为空");

            if (source.Length % 8 != 0) throw new ArgumentNullException("检查数组长度是否为8的倍数");

            ulong[] values = new ulong[source.Length / 8];

            for (int i = 0; i < source.Length / 8; i++)
            {
                values[i] = source.ToULong(8 * i, dataFormat);
            }
            return values;
        }
        #endregion byte[] => ulong[] end

        #region        byte[] => ushort start
        /// <summary>
        /// 字节数组中截取转成16位无符号整型
        /// </summary>
        /// <param name="source">字节数组</param>
        /// <param name="startIndex">开始索引</param>
        /// <param name="dataFormat">数据格式</param>
        /// <returns>返回UShort结果</returns>
        [Description("字节数组中截取转成16位无符号整型")]
        public static ushort ToUShort(this byte[] source, int startIndex = 0, Endianness dataFormat = Endianness.ABCD)
        {
            byte[] data = source.Sub2bytes(startIndex, dataFormat);
            return BitConverter.ToUInt16(data, 0);
        }

        /// <summary>
        /// 设置字节数组某个位 => UShortLib.SetBitValueFrom2ByteArray
        /// </summary>
        /// <param name="source">字节数组</param>
        /// <param name="offset">偏移位</param>
        /// <param name="bitVal">True或者False</param>
        /// <param name="dataFormat">数据格式</param>
        /// <returns>返回UShort结果</returns>
        [Description("设置字节数组某个位")]
        public static ushort SetBitAndToUShort(this byte[] source, int offset, bool bitVal, Endianness dataFormat = Endianness.ABCD)
        {
            if (offset >= 0 && offset <= 7)
            {
                source[1] = source[1].SetBit(offset, bitVal);
            }
            else
            {
                source[0] = source[0].SetBit(offset - 8, bitVal);
            }
            return source.ToUShort(0, dataFormat);
        }
        #endregion byte[] => ushort end

        #region        byte[] => ushort[] start
        /// <summary>
        /// 将字节数组中截取转成16位无符号整型数组
        /// </summary>
        /// <param name="source">字节数组</param>
        /// <param name="type">数据格式</param>
        /// <returns>返回UShort数组</returns>
        [Description("将字节数组中截取转成16位无符号整型数组")]
        public static ushort[] ToUShortArray(this byte[] source, Endianness type = Endianness.ABCD)
        {
            if (source == null) throw new ArgumentNullException("检查数组长度是否为空");

            if (source.Length % 2 != 0) throw new ArgumentNullException("检查数组长度是否为偶数");

            ushort[] result = new ushort[source.Length / 2];

            for (int i = 0; i < result.Length; i++)
            {
                result[i] = source.ToUShort(i * 2, type);
            }
            return result;
        }
        #endregion byte[] => ushort[] end

        #region        byte[] => 其他 start
        [Description("转换为常用数据类型")]
        public static T ToValue<T>(this byte[] source, int index = 0, int? length = null, Endianness dataFormat = Endianness.ABCD)
        {
            dynamic val = default(T);
            try
            {
                if (source == null || source.Length == 0)
                {
                    throw new ArgumentNullException(nameof(source));
                }

                var type = typeof(T);
                if (!type.IsValueType && !length.HasValue)
                {
                    throw new ArgumentException($"引用类型{nameof(T)}必须指定长度");
                }
                var byteNum = type.IsValueType ? Marshal.SizeOf<T>() : -1;
                var amout = length.HasValue ? length.Value : byteNum;
                byte[] dest = new byte[amout];
                if (length.HasValue)
                {
                    Array.Copy(source, index, dest, 0, amout);
                    //Buffer.BlockCopy(source, index, dest, 0, amout);
                }
                else
                {
                    dest = source;
                }

                var typeCode = Type.GetTypeCode(type);
                switch (typeCode)
                {
                    case TypeCode.SByte:
                        val = dest.ToSByte(0);
                        break;
                    case TypeCode.Byte:
                        val = dest.Length > 0 ? dest[0] : default;
                        break;
                    case TypeCode.Boolean:
                        val = dest.GetBitToBool(0, dest.Length);
                        break;
                    case TypeCode.Char:
                        var chars = Encoding.Default.GetChars(dest);
                        val = chars?[0];
                        break;
                    case TypeCode.String:
                        val = dest.ToStringByEncoding(0, dest.Length);
                        break;
                    case TypeCode.UInt16:
                        val = dest.ToUShort(0, dataFormat);
                        break;
                    case TypeCode.Int16:
                        val = dest.ToShort(0, dataFormat);
                        break;
                    case TypeCode.UInt32:
                        val = dest.ToUInt(0, dataFormat);
                        break;
                    case TypeCode.Int32:
                        val = dest.ToInt(0, dataFormat);
                        break;
                    case TypeCode.UInt64:
                        val = dest.ToULong(0, dataFormat);
                        break;
                    case TypeCode.Int64:
                        val = dest.ToLong(0, dataFormat);
                        break;
                    case TypeCode.Single:
                        val = dest.ToFloat(0, dataFormat);
                        break;
                    case TypeCode.Double:
                        val = dest.ToDouble(0, dataFormat);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(T), "不支持转换到的类型!");
                }
                // C# 泛型 无法将类型xx隐式转换为“T”-C#/.net框架-少有人走的路 http://www.skcircle.com/?id=158
            }
            catch
            {
                throw;
            }
            return val;
        }

        /// <summary>
        /// 接收到的数据转换为指定的常用类型的数据
        /// </summary>
        /// <typeparam name="T">指定的常用类型的数据</typeparam>
        /// <param name="source">接收到的数据</param>
        /// <param name="inArgCounts">参数个数</param>
        /// <param name="srcOffset">源数组中取值的起始位置</param>
        /// <param name="count">源数组中有效数据的长度</param>
        /// <returns></returns>
        public static List<T> ToValues<T>(this byte[] source, int inArgCounts, int srcOffset = 7, int count = 2)
        {
            List<T> values = new List<T>();
            try
            {
                if (source == null)
                {
                    throw new ArgumentNullException(nameof(source));
                }
                else if (source.Length < 11)
                {
                    throw new ArgumentException($"{nameof(source)}长度不能小于11");
                }
                //var data = new byte[count];
                var stepOffset = count / inArgCounts;// 每个参数取值的偏移长度
                var data = new byte[stepOffset];

                var startOffset = srcOffset;
                for (int i = 0; i < inArgCounts; i++)
                {
                    Array.Clear(data, 0, data.Length);
                    Buffer.BlockCopy(source, startOffset, data, 0, stepOffset);
                    //T val = ByteArrayLib.ToValue<T>(data);
                    T val = data.ToValue<T>();
                    values.Add(val);

                    startOffset += stepOffset;
                }
            }
            catch
            {
                throw;
            }
            return values;
        }

        /// <summary>
        /// 返回totalWidth个paddingByte填充在source左边的byte数组
        /// </summary>
        /// <param name="source"></param>
        /// <param name="totalWidth">填充长度</param>
        /// <param name="paddingByte">填充的byte</param>
        /// <returns>填充后的byte数组</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static byte[] PadLeft(this byte[] source, int totalWidth, byte paddingByte = 0x00)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (totalWidth <= 0) throw new ArgumentOutOfRangeException(nameof(totalWidth));
            var bytes = Enumerable.Repeat(paddingByte, totalWidth - source.Length).ToArray();
            var result = bytes.Union(source);
            return result.ToArray();
        }

        /// <summary>
        /// 返回totalWidth个paddingByte填充在source右边的byte数组
        /// </summary>
        /// <param name="source">字节流</param>
        /// <param name="totalWidth">填充长度</param>
        /// <param name="paddingByte">填充的byte</param>
        /// <returns>填充后的byte数组</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static byte[] PadRight(this byte[] source, int totalWidth, byte paddingByte = 0x00)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (totalWidth <= 0) throw new ArgumentOutOfRangeException(nameof(totalWidth));
            var bytes = Enumerable.Repeat(paddingByte, totalWidth - source.Length).ToArray();
            var result = source.Union(bytes);
            return result.ToArray();
        }
        #endregion byte[] => 其他 end
    }
}