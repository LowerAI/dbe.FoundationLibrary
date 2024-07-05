namespace dbe.FoundationLibrary.Communication.RTUComm
{
    /// <summary>
    /// 低功耗蓝牙设备
    /// </summary>
    public class BluetoothLEDevice
    {
        /// <summary>
        /// 唯一标识符
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 设备名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 唯一标识字符串
        /// </summary>
        public string DevStr
        {
            get
            {
                var devName = string.IsNullOrWhiteSpace(Name) ? "Unknown" : Name;
                return $"{Id}({devName})";
            }
        }
        /// <summary>
        /// 是否已配对
        /// </summary>
        public bool IsPaired { get; set; }
        /// <summary>
        /// 外观
        /// </summary>
        public ushort Appearance { get; set; }
        /// <summary>
        /// 发射功率
        /// </summary>
        public sbyte TxPower { get; set; }
        /// <summary>
        /// 信号强度
        /// </summary>
        public short Rssi { get; set; }

        public void Clone(BluetoothLEDevice device)
        {
            device.Id = Id;
            device.Name = Name;
            device.Appearance = Appearance;
            device.TxPower = TxPower;
            device.Rssi = Rssi;
        }

        public override bool Equals(object obj)
        {
            // 空引用过滤
            if (obj == null) return false;
            // 不为空则调用重写之后的 == 进行比较
            return this == (BluetoothLEDevice)obj;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        // 自定义的 == 重载，比较两个向量的对应坐标是否相等,若相等，返回 true，否则返回 false
        public static bool operator ==(BluetoothLEDevice left, BluetoothLEDevice right)
        {
            if (left is null && right is null) return true;
            if ((left is null && !(right is null)) || (!(left is null) && right is null)) return false;
            return left.Id == right.Id && left.Name == right.Name && left.IsPaired == right.IsPaired && left.Appearance == right.Appearance && left.TxPower == right.TxPower && left.Rssi == right.Rssi;
        }

        // 自定义的 != 重载，比较两个向量的对应坐标是否相等,若相等，返回 true，否则返回 false
        public static bool operator !=(BluetoothLEDevice left, BluetoothLEDevice right)
        {
            return !(left == right);
        }
    }
}