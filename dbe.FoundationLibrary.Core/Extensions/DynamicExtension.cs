using GNDView.Library.Common;

using System;

namespace GNDView.Library.Extensions
{
    public static class DynamicExtension
    {
        /// <summary>
        /// 返回动态对象转换为byte[]的值
        /// </summary>
        /// <typeparam name="Dynamic">动态类型，运行时才可知</typeparam>
        /// <param name="source">动态对象</param>
        /// <param name="dataFormat">字节序，默认大端</param>
        /// <returns>转换后的byte[]</returns>
        public static byte[] ToByteArray<Dynamic>(this Dynamic source, Endianness dataFormat = Endianness.ABCD)
        {
            byte[] ret = null;
            try
            {
                byte[] val = null;
                switch (source)
                {
                    case bool boolValue:
                        val = BitConverter.GetBytes(boolValue);
                        break;
                    case char charValue:
                        val = BitConverter.GetBytes(charValue);
                        break;
                    case string stringValue:
                        val = stringValue.ToByteArray();
                        break;
                    case sbyte i8Value:
                        val = new byte[] { (byte)(i8Value & 0xFF) };
                        break;
                    case byte u8Value:
                        val = new byte[] { u8Value };
                        break;
                    case short i16Value:
                        val = BitConverter.GetBytes(i16Value);
                        break;
                    case ushort u16Value:
                        val = BitConverter.GetBytes(u16Value);
                        break;
                    case int i32Value:
                        val = BitConverter.GetBytes(i32Value);
                        break;
                    case uint u32Value:
                        val = BitConverter.GetBytes(u32Value);
                        break;
                    case long i64Value:
                        val = BitConverter.GetBytes(i64Value);
                        break;
                    case ulong u64Value:
                        val = BitConverter.GetBytes(u64Value);
                        break;
                    case float floatValue:
                        val = BitConverter.GetBytes(floatValue);
                        break;
                    case double doubleValue:
                        val = BitConverter.GetBytes(doubleValue);
                        break;
                    case Array arrayValue:
                        val = arrayValue.ToByteArray();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(source));
                }
                ret = val.Format(dataFormat);
            }
            catch
            {
                //val = null;
                throw;
            }
            return ret;
        }
    }
}