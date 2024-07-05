using dbe.FoundationLibrary.Core.Common;

using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace dbe.FoundationLibrary.Core.Extensions
{
    /// <summary>
    /// short[]的扩展类
    /// </summary>
    public static class ShortArrayExtension
    {
        /// <summary>
        /// 将Short数组转换成字节数组
        /// </summary>
        /// <param name="source">Short数组</param>
        /// <param name="dataFormat">字节顺序</param>
        /// <returns>字节数组</returns>
        [Description("将Short数组转换成字节数组")]
        public static byte[] ToByteArray(this short[] source, Endianness dataFormat = Endianness.ABCD)
        {
            List<byte> list = new List<byte>();
            foreach (var item in source)
            {
                list.AddRange(BitConverter.GetBytes(item));
            }
            var ret = list.ToArray();
            return ret.Format(dataFormat);
        }
    }
}