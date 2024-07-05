using System;

namespace dbe.FoundationLibrary.Communication.RTUComm
{
    /// <summary>
    /// UART设备变更事件
    /// </summary>
    public class UartDeviceChangedEventArgs : EventArgs
    {
        private bool isInsert;
        /// <summary>
        /// 是否插入设备
        /// </summary>
        public bool IsInsert { get => isInsert; }

        private bool portNotChanged;
        /// <summary>
        /// 端口号是否未改变(插入设备是否之前拔出的串口或者拔出设备是否当前正在使用的串口)
        /// </summary>
        public bool PortNotChanged { get => portNotChanged; }

        public UartDeviceChangedEventArgs(bool isInsert = false, bool portNotChanged = false)
        {
            this.isInsert = isInsert;
            this.portNotChanged = portNotChanged;
        }
    }
}