using dbe.FoundationLibrary.Core.Common;

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace dbe.FoundationLibrary.Core.Extensions
{
    /// <summary>
    /// int的扩展类
    /// </summary>
    public static class IntExtension
    {
        /// <summary>
        /// 源自微信群友的一张截图，直接用await int;的来实现await Task.Delay(ms);的效果
        /// 使用方式举例:
        ///     Console.WriteLine(DateTime.Now);
        ///     await 5000;
        ///     Console.WriteLine(DateTime.Now);
        ///  返回效果：
        ///     2024/1/4 10:56:59
        ///     2024/1/4 10:57:04
        /// </summary>
        /// <param name="delay">延迟时间(int型毫秒数)</param>
        /// <returns></returns>
        public static TaskAwaiter GetTaskAwaiter(this int delay)
        {
            return Task.Delay(delay).GetAwaiter();
        }

        /// <summary>
        /// 判断整数source是否为2的幂
        /// </summary>
        /// <param name="source">源整数</param>
        /// <returns>true表示是/false表示否</returns>
        public static bool IsPowerOf2(this int source)
        {
            if (source == 1)
                return true;
            if (source < 1)
                return false;
            return IsPowerOf2(source / 2);
        }

        /// <summary>
        /// 通过布尔长度取整数
        /// </summary>
        /// <param name="boolLength">布尔长度</param>
        /// <returns>整数</returns>
        [Description("通过布尔长度取整数")]
        public static T RoundByteLength<T>(this int boolLength)
        {
            dynamic val = default(T);
            var type = typeof(T);
            var typeCode = Type.GetTypeCode(type);
            switch (typeCode)
            {
                //case TypeCode.Empty:
                //    break;
                //case TypeCode.Object:
                //    break;
                //case TypeCode.DBNull:
                //    break;
                //case TypeCode.Boolean:
                //    break;
                //case TypeCode.Char:
                //    break;
                //case TypeCode.SByte:
                //    break;
                //case TypeCode.Byte:
                //    break;
                case TypeCode.Int16:
                    val = boolLength % 8 == 0 ? (short)(boolLength / 8) : (short)(boolLength / 8 + 1);
                    break;
                case TypeCode.UInt16:
                    val = boolLength % 8 == 0 ? (ushort)(boolLength / 8) : (ushort)(boolLength / 8 + 1);
                    break;
                case TypeCode.Int32:
                    val = boolLength % 8 == 0 ? boolLength / 8 : boolLength / 8 + 1;
                    break;
                case TypeCode.UInt32:
                    val = boolLength % 8 == 0 ? (uint)(boolLength / 8) : (uint)(boolLength / 8 + 1);
                    break;
                //case TypeCode.Int64:
                //    break;
                //case TypeCode.UInt64:
                //    break;
                //case TypeCode.Single:
                //    break;
                //case TypeCode.Double:
                //    break;
                //case TypeCode.Decimal:
                //    break;
                //case TypeCode.DateTime:
                //    break;
                //case TypeCode.String:
                //    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(T), "不支持转换到的类型!");
            }
            return val;
        }


        /// <summary>
        /// 将Int类型数值转换成字节数组
        /// </summary>
        /// <param name="source">Int类型数值</param>
        /// <param name="dataFormat">字节顺序</param>
        /// <returns>字节数组</returns>
        [Description("将Int类型数值转换成字节数组")]
        public static byte[] ToByteArray(this int source, Endianness dataFormat = Endianness.ABCD)
        {
            byte[] resTemp = BitConverter.GetBytes(source);
            byte[] res = resTemp.Format(dataFormat);
            return res;
        }
    }
}