using InTheHand.Net;

using System.Text.RegularExpressions;

namespace dbe.FoundationLibrary.Communication.RTUComm
{
    /// <summary>
    /// 蓝牙mac地址扩展类
    /// </summary>
    public static class BluetoothAddressExtensions
    {
        /// <summary>
        /// 转换为带冒号分隔的字符串
        /// </summary>
        /// <param name="addr">mac地址</param>
        /// <param name="toLower">是否转为小写</param>
        /// <returns></returns>
        public static string ToStringSplitByColon(this BluetoothAddress addr, bool toLower = false)
        {
            if (addr == null || string.IsNullOrWhiteSpace(addr.ToString()))
            {
                return string.Empty;
            }

            var mac = addr.ToString();
            // 移除所有非十六进制字符
            mac = Regex.Replace(mac, "[^0-9A-Fa-f]", "");

            // 插入冒号分隔符
            if (mac.Length == 12)
            {
                mac = mac.Insert(2, ":").Insert(5, ":").Insert(8, ":").Insert(11, ":").Insert(14, ":");
            }
            if (toLower)
            {
                mac = mac.ToLower();
            }

            return mac;
        }

        /// <summary>
        /// 转换为带连字符分隔的字符串
        /// </summary>
        /// <param name="addr">mac地址</param>
        /// <param name="toLower">是否转为小写</param>
        /// <returns></returns>
        public static string ToStringSplitByHyphen(this BluetoothAddress addr, bool toLower = false)
        {
            if (addr == null || string.IsNullOrWhiteSpace(addr.ToString()))
            {
                return string.Empty;
            }

            var mac = addr.ToString();
            // 移除所有非十六进制字符
            mac = Regex.Replace(mac, "[^0-9A-Fa-f]", "");

            // 插入冒号分隔符
            if (mac.Length == 12)
            {
                mac = mac.Insert(2, "-").Insert(5, "-").Insert(8, "-").Insert(11, "-").Insert(14, "-");
            }
            if (toLower)
            {
                mac = mac.ToLower();
            }

            return mac;
        }
    }
}