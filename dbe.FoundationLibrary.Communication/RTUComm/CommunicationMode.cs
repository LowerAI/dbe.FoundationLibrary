using System;
using System.ComponentModel;

namespace dbe.FoundationLibrary.Communication.RTUComm
{
    /// <summary>
    /// 通信方式
    /// </summary>
    [Flags]
    public enum CommunicationMode
    {
        /// <summary>
        /// 串口通信
        /// </summary>
        [Description("串口")]
        Serial,
        /// <summary>
        /// 文件数据读写
        /// </summary>
        [Description("文件数据")]
        File,
        /// <summary>
        /// 蓝牙通信
        /// </summary>
        [Description("蓝牙")]
        Bluetooth
    }
}