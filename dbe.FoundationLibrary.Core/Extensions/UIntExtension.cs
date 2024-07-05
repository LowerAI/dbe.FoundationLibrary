using dbe.FoundationLibrary.Core.Common;

using System;
using System.ComponentModel;

namespace dbe.FoundationLibrary.Core.Extensions
{
    /// <summary>
    /// uint的扩展类
    /// </summary>
    public static class UIntExtension
    {
        /// <summary>
        /// 将UInt类型数值转换成字节数组
        /// </summary>
        /// <param name="source">UInt类型数值</param>
        /// <param name="dataFormat">字节顺序</param>
        /// <returns>字节数组</returns>
        [Description("将UInt类型数值转换成字节数组")]
        public static byte[] ToByteArray(this uint source, Endianness dataFormat = Endianness.ABCD)
        {
            byte[] resTemp = BitConverter.GetBytes(source);
            byte[] res = resTemp.Format(dataFormat);
            return res;
        }
    }
}