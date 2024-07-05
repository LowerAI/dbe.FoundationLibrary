namespace dbe.FoundationLibrary.Communication.RTUComm
{
    /// <summary>
    /// 转串口类型
    /// </summary>
    public enum ToSerialType : int
    {
        /// <summary>
        /// 串口直连
        /// </summary>
        DirectConnection = -1,
        /// <summary>
        /// 蓝牙转串口
        /// </summary>
        BluetoothToSerial,
        /// <summary>
        /// 2.4G ESB转串口
        /// </summary>
        ESBToSerial
    }
}