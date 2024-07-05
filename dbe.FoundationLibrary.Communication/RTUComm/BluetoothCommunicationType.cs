using System.ComponentModel;

namespace dbe.FoundationLibrary.Communication.RTUComm
{
    /// <summary>
    /// 蓝牙通信类型
    /// </summary>
    public enum BluetoothCommunicationType : int
    {
        /// <summary>
        /// 经典蓝牙通信
        /// </summary>
        [Description("经典蓝牙")]
        BluetoothClassic,
        /// <summary>
        /// 低功耗蓝牙通信
        /// </summary>
        [Description("低功耗蓝牙")]
        BluetoothLowEnergy
    }
}