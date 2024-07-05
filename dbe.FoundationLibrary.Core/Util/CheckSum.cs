using dbe.FoundationLibrary.Core.Common;
using dbe.FoundationLibrary.Core.Extensions;

using System;
using System.ComponentModel;

namespace dbe.FoundationLibrary.Core.Util
{
    /// <summary>
    /// 数据校验工具类
    /// </summary>
    [Description("数据校验工具类")]
    public class CheckSum
    {
        /// <summary>
        /// crc16短表
        /// </summary>
        private static readonly ushort[] crcTlb = new ushort[23] { 0x7E, 0x01, 0x00, 0x00, 0x01, 0x10, 0x02, 0x00, 0x27, 0x00, 0x0C, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x50, 0x00, 0x3C, 0x00, 0x03 };

        /// <summary>
        /// 查短表法计算CRC-16/MODBUS
        /// </summary>
        /// <param name="buffer">要校验的字节流</param>
        /// <param name="startPos">起始位置</param>
        /// <param name="count">字节数量</param>
        /// <returns>双字节校验值</returns>
        public static byte[] CRC16ForModbus(byte[] buffer, int startPos, int count)
        {
            byte[] crcHL = new byte[2];
            byte i = 0;
            int crc = 0xFFFF;

            while (--count >= 0)
            {
                crc ^= (int)buffer[startPos];
                for (i = 0; i < 8; i++)
                {
                    if ((crc & 1) == 1)
                    {
                        crc >>= 1;
                        crc ^= 0xA001;
                    }
                    else
                    {
                        crc >>= 1;
                    }
                }
                startPos++;
            }

            crcHL[0] = (byte)(crc & 0xFF);
            crcHL[1] = (byte)(crc >> 8);

            return crcHL;
        }

        /// <summary>
        /// Crc信息格式
        /// </summary>
        private struct CrcInfo
        {
            public string Name;
            public byte Width; //宽度，即CRC比特数。
            public uint Poly; //生成多项式的简写，以16进制表示。例如：CRC-32即是0x04C11DB7，忽略了最高位的"1"，即完整的生成项是0x104C11DB7。
            public uint CrcInit; //初始值,这是算法开始时寄存器（crc）的初始化预置值，十六进制表示。
            public uint XorOut; //计算结果与此参数异或后得到最终的CRC值。
            public bool RefIn; //待测数据的每个字节是否按位反转，true或false。
            public bool RefOut; //在计算后之后，异或输出之前，整个数据是否按位反转，true或false。
        }

        /// <summary>
        /// Crc信息表
        /// </summary>
        private static CrcInfo[] CrcInfoTab = new CrcInfo[]
        {
            //            CRC算法名称			 宽度        多项式        初始值          结果异或值       输入反转       输出反转
            new CrcInfo { Name = "CRC4_ITU", Width = 4, Poly = 0x03, CrcInit = 0x00, XorOut = 0x00, RefIn = true, RefOut = true},
            new CrcInfo { Name = "CRC5_EPC", Width = 5, Poly = 0x09, CrcInit = 0x09, XorOut = 0x00, RefIn = false, RefOut = false},
            new CrcInfo { Name = "CRC5_ITU", Width = 5, Poly = 0x15, CrcInit = 0x00, XorOut = 0x00, RefIn = true, RefOut = true},
            new CrcInfo { Name = "CRC5_USB", Width = 5, Poly = 0x05, CrcInit = 0x1F, XorOut = 0x1F, RefIn = true, RefOut = true},
            new CrcInfo { Name = "CRC6_ITU", Width = 6, Poly = 0x03, CrcInit = 0x00, XorOut = 0x00, RefIn = true, RefOut = true},
            new CrcInfo { Name = "CRC7_MMC", Width = 7, Poly = 0x09, CrcInit = 0x00, XorOut = 0x00, RefIn = false, RefOut = false},
            new CrcInfo { Name = "CRC8", Width = 8, Poly = 0x07, CrcInit = 0x00, XorOut = 0x00, RefIn = false, RefOut = false},
            new CrcInfo { Name = "CRC8_ITU", Width = 8, Poly = 0x07, CrcInit = 0x00, XorOut = 0x55, RefIn = false, RefOut = false},
            new CrcInfo { Name = "CRC8_ROHC", Width = 8, Poly = 0x07, CrcInit = 0xFF, XorOut = 0x00, RefIn = true, RefOut = true},
            new CrcInfo { Name = "CRC8_MAXIM", Width = 8, Poly = 0x31, CrcInit = 0x00, XorOut = 0x00, RefIn = true, RefOut = true},
            new CrcInfo { Name = "CRC16_IBM", Width = 16, Poly = 0x8005, CrcInit = 0x0000, XorOut = 0x0000, RefIn = true, RefOut = true},
            new CrcInfo { Name = "CRC16_MAXIM", Width = 16, Poly = 0x8005, CrcInit = 0x0000, XorOut = 0xFFFF, RefIn = true, RefOut = true},
            new CrcInfo { Name = "CRC16_USB", Width = 16, Poly = 0x8005, CrcInit = 0xFFFF, XorOut = 0xFFFF, RefIn = true, RefOut = true},
            new CrcInfo { Name = "CRC16_MODBUS", Width = 16, Poly = 0x8005, CrcInit = 0xFFFF, XorOut = 0x0000, RefIn = true, RefOut = true},
            new CrcInfo { Name = "CRC16_CCITT", Width = 16, Poly = 0x1021, CrcInit = 0x0000, XorOut = 0x0000, RefIn = true, RefOut = true},
            new CrcInfo { Name = "CRC16_CCITT_FALSE", Width = 16, Poly = 0x1021, CrcInit = 0xFFFF, XorOut = 0x0000, RefIn = false, RefOut = false},
            new CrcInfo { Name = "CRC16_X25", Width = 16, Poly = 0x1021, CrcInit = 0xFFFF, XorOut = 0xFFFF, RefIn = true, RefOut = true},
            new CrcInfo { Name = "CRC16_XMODEM", Width = 16, Poly = 0x1021, CrcInit = 0x0000, XorOut = 0x0000, RefIn = false, RefOut = false},
            new CrcInfo { Name = "CRC16_DNP", Width = 16, Poly = 0x3D65, CrcInit = 0x0000, XorOut = 0xFFFF, RefIn = true, RefOut = true},
            new CrcInfo { Name = "CRC32", Width = 32, Poly = 0x04C11DB7, CrcInit = 0xFFFFFFFF, XorOut = 0xFFFFFFFF, RefIn = true, RefOut = true},
            new CrcInfo { Name = "CRC32_MPEG2", Width = 32, Poly = 0x04C11DB7, CrcInit = 0xFFFFFFFF, XorOut = 0x00000000, RefIn = false, RefOut = false}
        };

        /// <summary>
        /// 位反转:就是将高位与低位数据顺序反过来
        /// </summary>
        /// <param name="inVal">反转前数据</param>
        /// <param name="bits">反转位数</param>
        /// <returns>反转后数据</returns>
        private static uint BitsReverse(uint inVal, byte bits)
        {
            uint outVal = 0;

            for (byte i = 0; i < bits; i++)
            {
                if ((inVal & (1u << i)) != 0)
                    outVal |= 1u << (bits - 1 - i);
            }

            return outVal;
        }

        /// <summary>
        /// 获取指定类型的CRC校验值
        /// </summary>
        /// <param name="type">指定的CRC类型</param>
        /// <param name="buffer">待校验的数据块</param>
        /// <returns>校验码</returns>
        public static byte[] GetCRC(CrcType type, byte[] buffer, Endianness endianness = Endianness.DCBA)
        {
            return GetCRC((int)type, buffer, 0, buffer.Length, endianness);
        }

        /// <summary>
        /// 获取指定类型的CRC校验值
        /// </summary>
        /// <param name="type">指定的CRC类型</param>
        /// <param name="buffer">待校验的数据块</param>
        /// <returns>校验码</returns>
        public static byte[] GetCRC(int type, byte[] buffer, Endianness endianness = Endianness.DCBA)
        {
            return GetCRC(type, buffer, 0, buffer.Length, endianness);
        }

        /// <summary>
        /// 获取指定类型的CRC校验值
        /// </summary>
        /// <param name="type">指定的CRC类型</param>
        /// <param name="buffer">待校验的数据块</param>
        /// <param name="startIndex">待校验的起始索引</param>
        /// <param name="length">待校验的数据长度(需小于或等于数据块长度)</param>
        /// <returns>校验码</returns>
        /// <exception cref="ArgumentNullException">当length大于buffer的长度时引发此异常</exception>
        /// <exception cref="ArgumentOutOfRangeException">当startIndex小于0或者大于等于length时引发此异常</exception>
        public static byte[] GetCRC(int type, byte[] buffer, int startIndex, int length, Endianness endianness = Endianness.DCBA)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException(nameof(buffer));
            }
            if (length > buffer.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(length));
            }
            if (startIndex < 0 || startIndex >= length)
            {
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            }

            var crcInfo = CrcInfoTab[type];
            byte width = crcInfo.Width; //宽度，即CRC比特数。
            uint poly = crcInfo.Poly;//生成多项式的简写，以16进制表示。例如：CRC-32即是0x04C11DB7，忽略了最高位的"1"，即完整的生成项是0x104C11DB7。
            uint crc = crcInfo.CrcInit;//初始值,这是算法开始时寄存器（crc）的初始化预置值，十六进制表示。
            uint xorout = crcInfo.XorOut;//计算结果与此参数异或后得到最终的CRC值。
            bool refin = crcInfo.RefIn;//待测数据的每个字节是否按位反转，E_TRUE或E_FALSE。
            bool refout = crcInfo.RefOut;//在计算后之后，异或输出之前，整个数据是否按位反转，E_TRUE或E_FALSE。
            byte n;
            uint bits;
            uint data;
            byte i;

            n = (byte)((width < 8) ? 0 : (width - 8));
            crc = (width < 8) ? (crc << (8 - width)) : crc;//CRC校验宽度小于8位,CRC初值移到高位计算
            bits = (width < 8) ? 0x80u : (uint)(1 << (width - 1));
            poly = (width < 8) ? (poly << (8 - width)) : poly;//CRC校验宽度小于8位,CRC多项式移到高位计算

            for (int idx = startIndex; idx < length; idx++)
            {
                data = buffer[idx];
                if (refin)//数据输入是否反转
                    data = BitsReverse(data, 8);
                crc ^= (data << n);

                for (i = 0; i < 8; i++)
                {
                    if ((crc & bits) != 0)
                    {
                        crc = (crc << 1) ^ poly;
                    }
                    else
                    {
                        crc = crc << 1;
                    }
                }
            }

            crc = (width < 8) ? (crc >> (8 - width)) : crc;//CRC校验宽度小于8位,CRC值计算完成后移到原位
            if (refout)//CRC输出是否反转
                crc = BitsReverse(crc, width);
            crc ^= xorout;//CRC结果异或
            crc = (crc & ((2u << (width - 1)) - 1));//获取有效位（也就是宽度位）

            var valLen = (int)width;
            var retp = BitConverter.GetBytes(crc);
            if (width % 8 == 0)
            {
                valLen = width / 8;
            }
            else
            {
                valLen = width / 8 + 1;
            }
            var ret = new byte[valLen];
            Array.Copy(retp, ret, valLen);
            //Array.Reverse(ret);
            ret = ret.Format(endianness);

            return ret;
        }
    }
}