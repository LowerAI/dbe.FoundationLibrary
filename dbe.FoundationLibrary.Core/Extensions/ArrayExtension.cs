using dbe.FoundationLibrary.Core.Common;

using System;
using System.ComponentModel;

namespace dbe.FoundationLibrary.Core.Extensions
{
    /// <summary>
    /// int[]的扩展类
    /// </summary>
    public static class ArrayExtension
    {
        /// <summary>
        /// 将Array数组转换成字节数组
        /// </summary>
        /// <param name="source">Array类型数组</param>
        /// <param name="dataFormat">字节顺序</param>
        /// <returns>字节数组</returns>
        [Description("将Array数组转换成字节数组")]
        public static byte[] ToByteArray(this Array source, Endianness dataFormat = Endianness.DCBA)
        {
            byte[] ret = null;
            try
            {
                var val = new byte[source.Length];
                switch (source)
                {
                    case bool[] boolArray:
                        val = boolArray.ToByteArray(dataFormat);
                        break;
                    case char[] charArray:
                        val = charArray.ToByteArray(dataFormat);
                        break;
                    case string[] stringArray:
                        val = stringArray.ToByteArray(dataFormat);
                        break;
                    case sbyte[] i8Array:
                        val = i8Array.ToByteArray(dataFormat);
                        break;
                    case byte[] u8Array:
                        val = u8Array.ToByteArray(dataFormat);
                        break;
                    case short[] i16Array:
                        val = i16Array.ToByteArray(dataFormat);
                        break;
                    case ushort[] u16Array:
                        val = u16Array.ToByteArray(dataFormat);
                        break;
                    case int[] i32Array:
                        val = i32Array.ToByteArray(dataFormat);
                        break;
                    case uint[] u32Array:
                        val = u32Array.ToByteArray(dataFormat);
                        break;
                    case long[] i64Array:
                        val = i64Array.ToByteArray(dataFormat);
                        break;
                    case ulong[] u64Array:
                        val = u64Array.ToByteArray(dataFormat);
                        break;
                    case float[] floatArray:
                        val = floatArray.ToByteArray(dataFormat);
                        break;
                    case double[] doubleArray:
                        val = doubleArray.ToByteArray(dataFormat);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(source));
                }
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