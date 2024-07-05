using dbe.FoundationLibrary.Core.Common;
using dbe.FoundationLibrary.Core.Extensions;

using InTheHand.Bluetooth;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;

namespace dbe.FoundationLibrary.Communication.RTUComm
{
    /// <summary>
    /// RTU对象的常用默认值
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Size = 1)]
    public struct RTUDefault
    {
        #region    公共部分 start
        /// <summary>
        /// 默认通道号
        /// </summary>
        public const byte ChNum = 0x00;
        /// <summary>
        /// 读取缓冲区的默认大小
        /// </summary>
        public const int ReadBufferSize = 64;
        /// <summary>
        /// 默认读取超时时间(默认为1秒)
        /// </summary>
        public const int ReadTimeout = 1000;
        /// <summary>
        /// 默认写入超时时间(默认为1秒)
        /// </summary>
        public const int WriteTimeout = 1000;
        /// <summary>
        /// 默认接收超时时间(默认为2秒)
        /// </summary>
        public const int ReciveTimeout = 500;
        /// <summary>
        /// 认定为接收不到数据需要循环的次数
        /// </summary>
        public const int ThresholdTimes = 200;
        /// <summary>
        /// 无效字节
        /// </summary>
        public const byte InvalidBytes = 0xFF;
        /// <summary>
        /// 默认读取/写入一行的结束符
        /// </summary>
        public const string NewLine = "\r\n";
        /// <summary>
        /// 接收数据帧的频率(默认为500)
        /// </summary>
        public const int RecvFrameFrequency = 500;
        /// <summary>
        /// 每帧里面元素的数量(默认为4)
        /// </summary>
        public const int ElementsCountPreFrame = 4;
        /// <summary>
        /// 主数据帧的长度(默认为9)
        /// </summary>
        public const int FrameLength = 9;
        /// <summary>
        /// 蓝牙数据帧的长度(默认为9)
        /// </summary>
        public const int BluetoothFrameLength = 31;
        /// <summary>
        /// 校验和占用长度(默认为2)
        /// </summary>
        public const int CrcValueLength = 2;
        /// <summary>
        /// 串行数据默认的前缀部分(默认为集迦)
        /// </summary>
        public const string FixedPrefix = "集迦";
        /// <summary>
        /// 串行数据默认的后缀部分(默认为回车换行符)
        /// </summary>
        public const string FixedSuffix = "\r\n";
        /// <summary>
        /// 无效的SN
        /// </summary>
        public const string InvalidSN = "GND_000";
        /// <summary>
        /// WCS协议的默认通道号(对应指令格式3)，由于没参与运算仅占位所以是固定值
        /// </summary>
        public const byte ChNumDefault3 = 0x01;
        /// <summary>
        /// 无效的从机设备名
        /// </summary>
        public static readonly byte[] InvalidDeviceName = "GND_FFFFFFFFFFFF".ToByteArray();
        /// <summary>
        /// 特殊功能码校验字典
        /// 通常应答数据的功能码与发送指令的功能码一致，但是也有例外，比如蓝牙配置指令发送的E7预期的是E8
        /// </summary>
        public static readonly Dictionary<byte, byte> FunCodeVerificationDict = new Dictionary<byte, byte> { { 0xE7, 0xE8 } };
        #endregion 公共部分 end

        #region    串口部分 start
        /// <summary>
        /// 默认波特率(默认为230400)
        /// </summary>
        public const int BaudRate = 230400;
        /// <summary>
        /// 默认校验位
        /// </summary>
        public static Parity Parity = Parity.None;
        /// <summary>
        /// 默认数据位(默认为8)
        /// </summary>
        public const int DataBits = 8;
        /// <summary>
        /// 默认停止位
        /// </summary>
        public static StopBits StopBits = StopBits.One;
        /// <summary>
        /// 默认读取缓冲区大小(默认为64秒)
        /// </summary>
        #endregion 串口部分 end

        #region        HCI部分 start
        /// <summary>
        /// 蓝牙配置指令的默认通道号(对应指令格式2)，由于没参与运算仅占位所以是固定值
        /// </summary>
        public const byte ChNumDefault2 = 0x00;
        /// <summary>
        /// 蓝牙配置指令的功能码表
        /// </summary>
        public static readonly byte[] HCI_Instruction_FunCodeTab = new byte[] { 0xE0, 0xE1, 0xE2, 0xE3, 0xE4, 0xE5, 0xE6, 0xE7, 0xE8 };
        /// <summary>
        /// HCI指令保留字段的值
        /// </summary>
        public static readonly byte[] HCI_Instruction_Reserve = Enumerable.Repeat<byte>(0x00, 3).ToArray();
        /// <summary>
        /// 蓝牙配置协议中的状态值=开启(00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01)
        /// </summary>
        public static readonly byte[] StateOn = 1.ToByteArray().PadLeft(16);
        /// <summary>
        /// 蓝牙配置协议中的状态值=关闭(00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00)
        /// </summary>
        public static readonly byte[] StateOff = 0.ToByteArray().PadRight(16);
        #endregion HCI部分 end

        #region    蓝牙部分 start
        /// <summary>
        /// 请求扫描经典蓝牙设备的时间间隔
        /// </summary>
        public static readonly TimeSpan InquiryLength = TimeSpan.FromSeconds(3);
        /// <summary>
        /// 连接经典蓝牙设备的通用pin码
        /// </summary>
        public static readonly List<string> CommonPins = new List<string> { "0000", "1111", "1234" };
        /// <summary>
        /// 默认蓝牙服务的Guid
        /// </summary>
        public static readonly Guid ServiceGuid = InTheHand.Net.Bluetooth.BluetoothService.SerialPort;

        /// <summary>
        /// 集迦自定义的Ble数据传输uuid
        /// </summary>
        [BluetoothUti("集迦自定义的Ble数据传输uuid")]
        public static readonly BluetoothUuid NusBaseService = new Guid("6863bcaf-6544-4e47-6961-68676e616853");

        /// <summary>
        /// 集迦自定义的Ble数据传输服务写特征uuid
        /// </summary>
        [BluetoothUti("集迦自定义的Ble数据传输服务写特征uuid")]
        public static readonly BluetoothUuid NusWriteChtt = new Guid("68630002-6544-4e47-6961-68676e616853");

        /// <summary>
        /// 集迦自定义的Ble数据传输服务读特征uuid
        /// </summary>
        [BluetoothUti("集迦自定义的Ble数据传输服务读特征uuid")]
        public static readonly BluetoothUuid NusReadChtt = new Guid("68630003-6544-4e47-6961-68676e616853");
        #endregion 蓝牙部分 end

        #region    公开方法 start
        /// <summary>
        /// 返回指定数量的无效字节数组
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public static byte[] GetInvalidBytes(int count)
        {
            byte[] bytes = new byte[count];
            for (int i = 0; i < count; i++)
            {
                bytes[i] = InvalidBytes;
            }
            return bytes;
        }

        /// <summary>
        /// 检测指定的bytes中是否全是无效字节
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static bool IsInvalid(byte[] bytes)
        {
            bool bInvalid = false;
            for (int i = 0; i < bytes.Length; i++)
            {
                if (bytes[i] == InvalidBytes)
                {
                    bInvalid = true;
                }
            }
            return bInvalid;
        }

        /// <summary>
        /// 获取所有的串口设备描述和端口号
        /// </summary>
        /// <returns></returns>
        public static List<KeyValuePair<string, string>> GetComList()
        {
            List<KeyValuePair<string, string>> comPairs = new List<KeyValuePair<string, string>>();
            try
            {
                Thread thread = new Thread(() =>
                {// 此处的操作必须放在单独的线程中否则会发生InvalidCastException
                    using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * from Win32_PnPEntity"))
                    {
                        Regex regExp = new Regex("COM\\d+");
                        ManagementObjectCollection deviceInfos = searcher.Get();
                        foreach (ManagementObject deviceInfo in deviceInfos)
                        {
                            var nameVal = deviceInfo.Properties["Name"].Value;
                            var name = nameVal == null ? string.Empty : nameVal.ToString();
                            if (regExp.IsMatch(name))
                            {
                                string portName = regExp.Match(name).Value;
                                comPairs.Add(new KeyValuePair<string, string>(portName, name));
                            }
                        }
                        searcher.Dispose();
                    }
                });
                thread.Start();
                thread.Join();
            }
            catch (Exception ex)
            {
                if (ex is InvalidCastException ice)
                {
                    Trace.WriteLine($"ice.Message = {ice.Message}");
                }
            }
            return comPairs;
        }

        public static string GetFullOSName()
        {
            var caption = string.Empty;
            var version = string.Empty;
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem"))
            {
                ManagementObjectCollection deviceInfos = searcher.Get();
                foreach (ManagementObject deviceInfo in deviceInfos)
                {
                    caption = deviceInfo.GetPropertyValue("Caption")?.ToString();
                    version = deviceInfo.GetPropertyValue("Version")?.ToString();
                }
                searcher.Dispose();
            }
            return $"{caption} {version}";
        }

        /// <summary>
        /// 获取操作系统版本信息
        /// </summary>
        /// <returns></returns>
        public static OSVersion GetOSVersion()
        {
            var osVersion = OSVersion.Unkown;

            var caption = string.Empty;
            var version = string.Empty;
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem"))
            {
                ManagementObjectCollection deviceInfos = searcher.Get();
                foreach (ManagementObject deviceInfo in deviceInfos)
                {
                    caption = deviceInfo.GetPropertyValue("Caption")?.ToString();
                    version = deviceInfo.GetPropertyValue("Version")?.ToString();
                }
                searcher.Dispose();
            }

            if (caption.IsEmpty())
            {
                osVersion = OSVersion.Unkown;
            }
            else if (caption.StartsWith("Microsoft Windows Server 2022"))
            {
                osVersion = OSVersion.WindowsServer2022;
            }
            else if (caption.StartsWith("Microsoft Windows 11"))
            {
                osVersion = OSVersion.Windows11;
            }
            else if (caption.StartsWith("Microsoft Windows Server 2019"))
            {
                osVersion = OSVersion.WindowsServer2019;
            }
            else if (caption.StartsWith("Microsoft Windows 10"))
            {
                osVersion = OSVersion.Windows10;
            }
            else if (caption.StartsWith("Microsoft Windows Server 2016"))
            {
                osVersion = OSVersion.WindowsServer2016;
            }
            else if (caption.StartsWith("Microsoft Windows 8.1 Update 1"))
            {
                osVersion = OSVersion.Windows8_1Update1;
            }
            else if (caption.StartsWith("Microsoft Windows 8.1"))
            {
                osVersion = OSVersion.Windows8_1;
            }
            else if (caption.StartsWith("Microsoft Windows Server 2012 R2"))
            {
                osVersion = OSVersion.WindowsServer2012R2;
            }
            else if (caption.StartsWith("Microsoft Windows 8"))
            {
                osVersion = OSVersion.Windows8;
            }
            else if (caption.StartsWith("Microsoft Windows Server 2012"))
            {
                osVersion = OSVersion.WindowsServer2012;
            }
            else if (caption.StartsWith("Microsoft Windows Home Server 2011"))
            {
                osVersion = OSVersion.WindowsHomeServer2011;
            }
            else if (caption.StartsWith("Microsoft Windows Server 2008 R2  SP1"))
            {
                osVersion = OSVersion.WindowsServer2008R2SP1;
            }
            else if (caption.StartsWith("Microsoft Windows Server 2008 R2"))
            {
                osVersion = OSVersion.WindowsServer2008R2;
            }
            else if (caption.StartsWith("Microsoft Windows 7 Service Pack 1"))
            {
                osVersion = OSVersion.Windows7ServicePack1;
            }
            else if (caption.StartsWith("Microsoft Windows 7"))
            {
                osVersion = OSVersion.Windows7;
            }
            else if (caption.StartsWith("Microsoft Windows Server 2008 R2"))
            {
                osVersion = OSVersion.WindowsServer2008R2;
            }
            else if (caption.StartsWith("Microsoft Windows Server 2008"))
            {
                osVersion = OSVersion.WindowsServer2008;
            }
            else if (caption.StartsWith("Microsoft Windows Vista Service Pack 2"))
            {
                osVersion = OSVersion.WindowsVistaServicePack2;
            }
            else if (caption.StartsWith("Microsoft Windows Vista Service Pack 1"))
            {
                osVersion = OSVersion.WindowsVistaServicePack1;
            }
            else if (caption.StartsWith("Microsoft Windows Vista"))
            {
                osVersion = OSVersion.WindowsVista;
            }
            else if (caption.StartsWith("Microsoft Windows Home Server"))
            {
                osVersion = OSVersion.WindowsHomeServer;
            }
            else if (caption.StartsWith("Microsoft Windows Server 2003 R2"))
            {
                osVersion = OSVersion.WindowsServer2003R2;
            }
            else if (caption.StartsWith("Microsoft Windows Server 2003 Service Pack 1"))
            {
                osVersion = OSVersion.WindowsServer2003ServicePack1;
            }
            else if (caption.StartsWith("Microsoft Windows Server 2003"))
            {
                osVersion = OSVersion.WindowsServer2003;
            }
            else if (caption.StartsWith("Microsoft Windows XP 64"))
            {
                osVersion = OSVersion.WindowsXP64;
            }
            else if (caption.StartsWith("Microsoft Windows XP Service Pack 3"))
            {
                osVersion = OSVersion.WindowsXPServicePack3;
            }
            else if (caption.StartsWith("Microsoft Windows XP Service Pack 2"))
            {
                osVersion = OSVersion.WindowsXPServicePack2;
            }
            else if (caption.StartsWith("Microsoft Windows XP Service Pack 1"))
            {
                osVersion = OSVersion.WindowsXPServicePack1;
            }
            else if (caption.StartsWith("Microsoft Windows XP"))
            {
                osVersion = OSVersion.WindowsXP;
            }
            else if (caption.StartsWith("Microsoft Windows 2000"))
            {
                osVersion = OSVersion.Windows2000;
            }
            else if (caption.StartsWith("Microsoft Windows NT 4.00"))
            {
                osVersion = OSVersion.WindowsNT4_00;
            }
            else if (caption.StartsWith("Microsoft Windows NT 3.51"))
            {
                osVersion = OSVersion.WindowsNT3_51;
            }
            else if (caption.StartsWith("Microsoft Windows NT 3.5"))
            {
                osVersion = OSVersion.WindowsNT3_5;
            }
            else if (caption.StartsWith("Microsoft Windows NT 3.1"))
            {
                osVersion = OSVersion.WindowsNT3_1;
            }
            else if (caption.StartsWith("Microsoft Windows Millennium Edition"))
            {
                osVersion = OSVersion.WindowsMillenium;
            }
            else if (caption.StartsWith("Microsoft Windows 98 Second Edition"))
            {
                osVersion = OSVersion.Windows98SecondEdition;
            }
            else if (caption.StartsWith("Microsoft Windows 98"))
            {
                osVersion = OSVersion.Windows98;
            }
            else if (caption.StartsWith("Microsoft Windows 95 OEM Service Release 2.5 C"))
            {
                osVersion = OSVersion.Windows95OEMServiceRelease2_5C;
            }
            else if (caption.StartsWith("Microsoft Windows 95 OEM Service Release 2.1"))
            {
                osVersion = OSVersion.Windows95OEMServiceRelease2_1;
            }
            else if (caption.StartsWith("Microsoft Windows 95 OEM Service Release 2"))
            {
                osVersion = OSVersion.Windows95OEMServiceRelease2;
            }
            else if (caption.StartsWith("Microsoft Windows 95 OEM Service Release 1"))
            {
                osVersion = OSVersion.Windows95OEMServiceRelease1;
            }
            return osVersion;
        }
        #endregion 公开方法 end
    }
}