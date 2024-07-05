namespace dbe.FoundationLibrary.Communication.RTUComm
{
    /// <summary>
    /// 指令类型
    /// </summary>
    public enum InstructionType
    {
        /// <summary>
        /// 第1种串口
        /// </summary>
        Serial1 = 0,
        /// <summary>
        /// 主从机配置
        /// </summary>
        HCIConfig,
        /// <summary>
        /// 第1种经典蓝牙
        /// </summary>
        BT1,
        /// <summary>
        /// 第1种低功耗蓝牙
        /// </summary>
        BLE1
    }
}