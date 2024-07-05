using System;
using System.Collections.Generic;
using System.Management;

namespace dbe.FoundationLibrary.Core.Util
{
    /// <summary>
    /// 返回当前PC的硬件信息
    /// 参考链接：
    /// 使用 C# 获取计算机硬件信息_c#获取电脑硬件信息_allway2的博客-CSDN博客 https://blog.csdn.net/allway2/article/details/123154410
    /// </summary>
    public class HardwareInfo
    {
        #region    属性 start
        /// <summary>
        /// 返回处理器Id
        /// </summary>
        public static string CPUId
        {
            get
            {
                return GetFromMC<string>("win32_processor", mo =>
                {
                    var Id = mo.Properties["processorID"].Value.ToString();
                    return Id;
                });
            }
        }

        /// <summary>
        /// 返回处理器的制造商
        /// </summary>
        public static string CPUManufacturer
        {
            get
            {
                return GetFromMC<string>("win32_processor", mo =>
                {
                    var maker = mo.Properties["Manufacturer"].Value.ToString();
                    return maker;
                });
            }
        }

        /// <summary>
        /// 返回处理器当前的时钟速度
        /// </summary>
        public static float CPUCurrentClockSpeed
        {
            get
            {
                return GetFromMC<float>("win32_processor", mo =>
                {
                    var speed = Convert.ToSingle(mo.Properties["CurrentClockSpeed"].Value);
                    return speed;
                });
            }
        }

        /// <summary>
        /// 返回处理器当前的时钟速度(以GHz为单位)
        /// </summary>
        public static float CPUSpeedInGHz
        {
            get
            {
                return 0.001F * CPUCurrentClockSpeed;
            }
        }

        /// <summary>
        /// 返回处理器信息
        /// </summary>
        public static string CPUInformation
        {
            get
            {
                return GetFromMC<string>("win32_processor", mo =>
                {
                    string name = (string)mo["Name"];
                    name = name.Replace("(TM)", "™").Replace("(tm)", "™").Replace("(R)", "®").Replace("(r)", "®").Replace("(C)", "©").Replace("(c)", "©").Replace("    ", " ").Replace("  ", " ");

                    var info = $"{name}, {mo["Caption"]}, {mo["SocketDesignation"]}";
                    return info;
                });
            }
        }

        /// <summary>
        /// 返回硬盘型号的集合
        /// </summary>
        public static List<string> HDDModels
        {
            get
            {
                var lst = new List<string>();
                return GetFromMC<List<string>>("Win32_LogicalDisk", mo =>
                {
                    var model = mo.Properties["Model"].Value.ToString();
                    lst.Add(model);
                    return lst;
                });
            }
        }

        /// <summary>
        /// 返回硬盘各卷序号的集合
        /// </summary>
        public static List<string> HDDSerialNos
        {
            get
            {
                var lst = new List<string>();
                return GetFromMC<List<string>>("Win32_LogicalDisk", mo =>
                {
                    var vsn = mo.Properties["VolumeSerialNumber"].Value.ToString();
                    lst.Add(vsn);
                    return lst;
                });
            }
        }

        /// <summary>
        /// 返回Mac地址
        /// </summary>
        public static string DefaultIPGateway
        {
            get
            {
                return GetFromMC<string>("Win32_NetworkAdapterConfiguration", mo =>
                {
                    var gateway = string.Empty;
                    if ((bool)mo["IPEnabled"])
                    {
                        gateway = mo.Properties["DefaultIPGateway"].Value.ToString();
                    }
                    return gateway;
                });
            }
        }

        /// <summary>
        /// 返回IP地址
        /// </summary>
        public static string IPAddress
        {
            get
            {
                return GetFromMC<string>("Win32_NetworkAdapterConfiguration", mo =>
                {
                    var ip = string.Empty;
                    if ((bool)mo["IPEnabled"])
                    {
                        ip = mo.Properties["IpAddress"].Value.ToString();
                    }
                    return ip;
                });
            }
        }

        /// <summary>
        /// 返回Mac地址
        /// </summary>
        public static string MACAddress
        {
            get
            {
                return GetFromMC<string>("Win32_NetworkAdapterConfiguration", mo =>
                {
                    var mac = string.Empty;
                    if ((bool)mo["IPEnabled"])
                    {
                        mac = mo.Properties["MacAddress"].Value.ToString();
                    }
                    return mac;
                });
            }
        }

        /// <summary>
        /// 返回主板制造商
        /// </summary>
        public static string BoardMaker
        {
            get
            {
                return GetFromMOS<string>("root\\CIMV2", "SELECT * FROM Win32_BaseBoard", mo =>
                {
                    var maker = mo.GetPropertyValue("Manufacturer").ToString();
                    return maker;
                });
            }
        }

        /// <summary>
        /// 返回主板产品标识符
        /// </summary>
        public static string BoardProductId
        {
            get
            {
                return GetFromMOS<string>("root\\CIMV2", "SELECT * FROM Win32_BaseBoard", mo =>
                {
                    var id = mo.GetPropertyValue("Product").ToString();
                    return id;
                });
            }
        }

        /// <summary>
        /// 返回光驱设备
        /// </summary>
        public static string CdRomDrive
        {
            get
            {
                return GetFromMOS<string>("root\\CIMV2", "SELECT * FROM Win32_CDROMDrive", mo =>
                {
                    var drive = mo.GetPropertyValue("Drive").ToString();
                    return drive;
                });
            }
        }

        /// <summary>
        /// 返回BIOS制造商
        /// </summary>
        public static string BIOSmaker
        {
            get
            {
                return GetFromMOS<string>("root\\CIMV2", "SELECT * FROM Win32_BIOS", mo =>
                {
                    var maker = mo.GetPropertyValue("Manufacturer").ToString();
                    return maker;
                });
            }
        }

        /// <summary>
        /// 返回BIOS界面的语言
        /// </summary>
        public static string BIOSUILanguage
        {
            get
            {
                return GetFromMOS<string>("root\\CIMV2", "SELECT * FROM Win32_BIOS", mo =>
                {
                    var lang = mo.GetPropertyValue("CurrentLanguage").ToString();
                    return lang;
                });
            }
        }

        /// <summary>
        /// 返回BIOS序号
        /// </summary>
        public static string BIOSSerialNo
        {
            get
            {
                return GetFromMOS<string>("root\\CIMV2", "SELECT * FROM Win32_BIOS", mo =>
                {
                    var number = mo.GetPropertyValue("SerialNumber").ToString();
                    return number;
                });
            }
        }

        /// <summary>
        /// 返回BIOS标题
        /// </summary>
        public static string BIOSCaption
        {
            get
            {
                return GetFromMOS<string>("root\\CIMV2", "SELECT * FROM Win32_BIOS", mo =>
                {
                    var caption = mo.GetPropertyValue("Caption").ToString();
                    return caption;
                });
            }
        }

        /// <summary>
        /// 返回系统帐户名
        /// </summary>
        public static string AccountName
        {
            get
            {
                return GetFromMOS<string>("root\\CIMV2", "SELECT * FROM Win32_UserAccount", mo =>
                {
                    var name = mo.GetPropertyValue("Name").ToString();
                    return name;
                });
            }
        }

        /// <summary>
        /// 返回操作系统的登录用户名
        /// </summary>
        public static string UserName
        {
            get
            {
                return GetFromMC<string>("Win32_ComputerSystem", mo =>
                {
                    var name = mo.Properties["UserName"].Value.ToString();
                    return name;
                });
            }
        }

        /// <summary>
        /// 返回物理内存容量
        /// </summary>
        public static string PhysicalMemory
        {
            get
            {
                long MemSize = 0;
                long mCap = 0;
                GetFromMOS<long>("root\\CIMV2", "SELECT * FROM Win32_PhysicalMemory", mo =>
                {
                    mCap = Convert.ToInt64(mo["Capacity"]);
                    MemSize += mCap;
                    return MemSize;
                });
                MemSize = (MemSize / 1024) / 1024;
                return MemSize.ToString() + "MB";
            }
        }

        /// <summary>
        /// 返回主板上内存插槽的数量
        /// </summary>
        public static int NoRamSlots
        {
            get
            {
                return GetFromMOS<int>("root\\CIMV2", "SELECT * FROM Win32_PhysicalMemoryArray", mo =>
                {
                    var number = Convert.ToInt32(mo["MemoryDevices"]);
                    return number;
                });
            }
        }

        /// <summary>
        /// 返回系统信息
        /// </summary>
        public static string OSInformation
        {
            get
            {
                return GetFromMOS<string>("root\\CIMV2", "SELECT * FROM Win32_OperatingSystem", mo =>
                {
                    var info = $"{mo["Caption"]},{mo["Version"]},{mo["OSArchitecture"]}";
                    return info;
                });
            }
        }

        /// <summary>
        /// 返回计算机名
        /// </summary>
        public static string ComputerName
        {
            get
            {
                return GetFromMC<string>("Win32_ComputerSystem", mo =>
                {
                    var name = mo.Properties["Name"].Value.ToString();
                    return name;
                });
                //return System.Environment.GetEnvironmentVariable("ComputerName");
            }
        }

        /// <summary>
        /// 返回PC类型 
        /// </summary>
        public static string SystemType
        {
            get
            {
                return GetFromMC<string>("Win32_ComputerSystem", mo =>
                {
                    var type = mo.Properties["SystemType"].Value.ToString();
                    return type;
                });
            }
        }
        #endregion 属性 end

        #region    构造函数 start

        #endregion 构造函数 end

        #region    公开方法 start

        #endregion 公开方法 end

        #region    私有方法 start
        /// <summary>
        /// 返回ManagementClass实例中指定键名的键值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        private static T GetFromMC<T>(string path, Func<ManagementObject, T> func)
        {
            T t = default(T);
            ManagementClass mc = new ManagementClass(path);
            ManagementObjectCollection moc = mc.GetInstances();
            foreach (ManagementObject mo in moc)
            {
                t = func(mo);
            }
            return t;
        }

        /// <summary>
        /// 返回ManagementObjectSearcher实例中指定键名的键值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="scope"></param>
        /// <param name="queryString"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        private static T GetFromMOS<T>(string scope, string queryString, Func<ManagementObject, T> func)
        {
            T t = default(T);

            ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, queryString);
            ManagementObjectCollection moc = searcher.Get();
            foreach (ManagementObject mo in moc)
            {
                t = func(mo);
            }
            return t;
        }
        #endregion 私有方法 end
    }
}