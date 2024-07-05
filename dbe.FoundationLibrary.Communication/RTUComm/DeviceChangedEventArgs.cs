using System;

namespace dbe.FoundationLibrary.Communication.RTUComm
{
    /// <summary>
    /// 设备变更事件
    /// </summary>
    public class DeviceChangedEventArgs : EventArgs
    {
        private bool isConnect;
        /// <summary>
        /// 是否插入/连接设备
        /// </summary>
        public bool IsConnect { get => isConnect; }

        private bool deviceNotChanged;
        /// <summary>
        /// 客户端是否未改变(已连接设备是否之前断开的客户端)
        /// </summary>
        public bool DeviceNotChanged { get => deviceNotChanged; }

        public DeviceChangedEventArgs(bool isConnect = false, bool deviceNotChanged = false)
        {
            this.isConnect = isConnect;
            this.deviceNotChanged = deviceNotChanged;
        }
    }
}