namespace dbe.FoundationLibrary.Core.Common
{
    /// <summary>
    /// Crc类型
    /// </summary>
    public enum CrcType : byte
    {
        /// <summary>
        /// x4+x+1
        /// </summary>
        CRC4_ITU = 0x00,
        /// <summary>
        /// x5+x3+1
        /// </summary>
        CRC5_EPC,
        /// <summary>
        /// x5+x4+x2+1
        /// </summary>
        CRC5_ITU,
        /// <summary>
        /// x5+x2+1
        /// </summary>
        CRC5_USB,
        /// <summary>
        /// x6+x+1
        /// </summary>
        CRC6_ITU,
        /// <summary>
        /// x7+x3+1
        /// </summary>
        CRC7_MMC,
        /// <summary>
        /// x8+x2+x+1
        /// </summary>
        CRC8,
        /// <summary>
        /// x8+x2+x+1
        /// </summary>
        CRC8_ITU,
        /// <summary>
        /// x8+x2+x+1
        /// </summary>
        CRC8_ROHC,
        /// <summary>
        /// x8+x5+x4+1
        /// </summary>
        CRC8_MAXIM,
        /// <summary>
        /// x16+x15+x2+1
        /// </summary>
        CRC16_IBM,
        /// <summary>
        /// x16+x15+x2+1
        /// </summary>
        CRC16_MAXIM,
        /// <summary>
        /// x16+x15+x2+1
        /// </summary>
        CRC16_USB,
        /// <summary>
        /// x16+x15+x2+1
        /// </summary>
        CRC16_MODBUS,
        /// <summary>
        /// x16+x12+x5+1
        /// </summary>
        CRC16_CCITT,
        /// <summary>
        /// x16+x12+x5+1
        /// </summary>
        CRC16_CCITT_FALSE,
        /// <summary>
        /// x16+x12+x5+1
        /// </summary>
        CRC16_X25,
        /// <summary>
        /// x16+x12+x5+1
        /// </summary>
        CRC16_XMODEM,
        /// <summary>
        /// x16+x13+x12+x11+x10+x8+x6+x5+x2+1
        /// </summary>
        CRC16_DNP,
        /// <summary>
        /// x32+x26+x23+x22+x16+x12+x11+x10+x8+x7+x5+x4+x2+x+1
        /// </summary>
        CRC32,
        /// <summary>
        /// x32+x26+x23+x22+x16+x12+x11+x10+x8+x7+x5+x4+x2+x+1
        /// </summary>
        CRC32_MPEG2
    }
}