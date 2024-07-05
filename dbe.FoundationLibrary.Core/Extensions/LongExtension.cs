using dbe.FoundationLibrary.Core.Common;

using System;
using System.ComponentModel;

namespace dbe.FoundationLibrary.Core.Extensions
{
    /// <summary>
    /// long的扩展类
    /// </summary>
    public static class LongExtension
    {
        /// <summary>
        /// 将Long类型数值转换成字节数组
        /// </summary>
        /// <param name="source">Long类型数值</param>
        /// <param name="dataFormat">字节顺序</param>
        /// <returns>字节数组</returns>
        [Description("将Long类型数值转换成字节数组")]
        public static byte[] ToByteArray(this long source, Endianness dataFormat = Endianness.ABCD)
        {
            byte[] resTemp = BitConverter.GetBytes(source);
            byte[] res = resTemp.Format(dataFormat);
            return res;
        }
    }
}