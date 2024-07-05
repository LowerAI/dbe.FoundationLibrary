using System.Runtime.InteropServices;

namespace dbe.FoundationLibrary.Communication.RTUComm
{
    /// <summary>
    /// Host-Controller接口(HCI)指定主机与无线电控制器之间的所有交互。
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Size = 1)]
    public struct HCIinstructions
    {
        /// <summary>
        /// 执行切换从机
        /// </summary>
        public const byte PerformSwitchSlave = 0xE0;
        /// <summary>
        /// 修改从机的标识符
        /// </summary>
        public const byte ChangeSlaveKey = 0xE1;
        /// <summary>
        /// 读取连接状态
        /// </summary>
        public const byte ReadConnectionStatus = 0xE2;
        /// <summary>
        /// 读取从机的标识符
        /// </summary>
        public const byte ReadSlaveKey = 0xE3;
        /// <summary>
        /// 扫描从机的标识符
        /// </summary>
        public const byte ScanSlaveKey = 0xE4;
        /// <summary>
        /// 开启/关闭串口透传功能
        /// </summary>
        public const byte SwitchPassThroughForSerial = 0xE6;
        /// <summary>
        /// 修改无线2.4G(类似于无线鼠标的射频通信)主从机信道号
        /// </summary>
        public const byte ChangeHostAndSlaveChannelNo = 0xE7;
        /// <summary>
        /// 修改无线2.4G(类似于无线鼠标的射频通信)主机信道号
        /// </summary>
        public const byte ChangeHostChannelNo = 0xE8;
    }
}
