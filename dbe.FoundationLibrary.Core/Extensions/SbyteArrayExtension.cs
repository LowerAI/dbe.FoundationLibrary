using dbe.FoundationLibrary.Core.Common;

using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace dbe.FoundationLibrary.Core.Extensions
{
    /// <summary>
    /// sbyte[]的扩展类
    /// </summary>
    public static class SbyteArrayExtension
    {
        /// <summary>
        /// 将sbyte数组转换成字节数组
        /// </summary>
        /// <param name="source">sbyte数组</param>
        /// <param name="dataFormat">字节顺序</param>
        /// <returns>字节数组</returns>
        [Description("将UShort数组转换成字节数组")]
        public static byte[] ToByteArray(this sbyte[] source, Endianness dataFormat = Endianness.ABCD)
        {
            List<byte> list = new List<byte>();
            foreach (var item in source)
            {
                //list.Add(item.ToByte());
                list.AddRange(BitConverter.GetBytes(item));
            }
            var ret = list.ToArray();
            return ret.Format(dataFormat);
        }
    }
}