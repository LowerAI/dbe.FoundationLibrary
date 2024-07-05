using System;

namespace dbe.FoundationLibrary.Communication.RTUComm
{
    /// <summary>
    /// 可选波特率
    /// </summary>
    [Flags]
    public enum BaudRates : int
    {
        BAUD_075 = 75,
        BAUD_110 = 110,
        BAUD_150 = 150,
        BAUD_300 = 300,
        BAUD_600 = 600,
        BAUD_1200 = 1200,
        BAUD_1800 = 1800,
        BAUD_2400 = 2400,
        BAUD_4800 = 4800,
        BAUD_7200 = 7200,
        BAUD_9600 = 9600,
        BAUD_14400 = 14400,
        BAUD_19200 = 19200,
        BAUD_38400 = 38400,
        BAUD_43000 = 43000,
        BAUD_56k = 56000,
        BAUD_57600 = 57600,
        BAUD_115200 = 115200,
        BAUD_128K = 128000,
        BAUD_230400 = 230400,
        BAUD_256K = 256000,
        BAUD_460800 = 460800,
        BAUD_921600 = 921600
    }
}