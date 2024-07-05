using dbe.FoundationLibrary.Core.Win32.API;

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace dbe.FoundationLibrary.Core.Util
{
    /// <summary>
    /// INI文件读写工具类
    /// </summary>  
    public sealed class IniRW
    {
        /// <summary>
        /// ini文件中一个段落(section)的最大大小。此属性定义缓冲区的最大大小，用于从ini文件中检索数据。这个值是win32函数GetPrivateProfileSectionNames()或GetPrivateProfileString ()允许的最大值。
        /// </summary>
        private const int MaxSectionSize = 16384;//32767;

        private string fullPath;
        /// <summary>
        /// 返回ini文件完整路径
        /// </summary>
        public string FullPath { get => fullPath; }

        /// <summary>
        /// 构造函数。  
        /// </summary>
        /// <param name="fileName">ini文件名,全路径</param> 
        public IniRW(string fileName)
        {
            //必须是完全路径，不能是相对路径
            fullPath = fileName;
        }

        #region    读写数据 start
        /// <summary>
        /// 从ini文件中读取指定类型T的值。
        /// 当读取失败时返回缺省值
        /// </summary>
        /// <param name="Section">块</param>
        /// <param name="key">键</param>
        /// <param name="Default">缺省值</param>
        /// <returns>string型返回值</returns>
        public T Read<T>(string section, string key, string encodingName = "unicode")
        {
            if (section == null)
                throw new ArgumentNullException(nameof(section));

            if (key == null)
                throw new ArgumentNullException(nameof(key));

            byte[] buffer = new byte[MaxSectionSize];
            int bufLen = Kernel32.GetPrivateProfileString(GetBytes(section, encodingName), GetBytes(key, encodingName), GetBytes(default(T), encodingName), buffer, MaxSectionSize, FullPath);
            //必须设定0（系统默认的代码页）的编码方式，否则无法支持中文
            string s = Encoding.GetEncoding(encodingName).GetString(buffer);

            s = s.Substring(0, (int)bufLen);
            T value = (T)Convert.ChangeType(s, typeof(T));
            return value;
        }

        /// <summary>
        /// 将指定类型T的值写入ini
        /// </summary>
        /// <param name="Section">块</param>
        /// <param name="key">键</param>
        /// <param name="Value">T类型值</param>
        public bool Write<T>(string section, string key, T value, string encodingName = "unicode")
        {
            if (section == null)
                throw new ArgumentNullException(nameof(section));

            if (key == null)
                throw new ArgumentNullException(nameof(key));

            //return WritePrivateProfileString(GetBytes(section, encodingName), GetBytes(key, encodingName), GetBytes<T>(value, encodingName), FullPath);
            return Kernel32.WritePrivateProfileString(section, key, value?.ToString(), FullPath);
        }
        #endregion 读写数据 end

        #region 公开方法
        /// <summary>
        /// 从Ini文件中，读取所有Section的名称
        /// </summary>
        /// <returns>Section名字串集合</returns>
        public StringCollection ReadSectionNames()
        {
            StringCollection sectionList = new StringCollection();
            //Note:必须得用Bytes来实现，StringBuilder只能取到第一个Section
            byte[] buffer = new byte[MaxSectionSize];
            int bufLen = 0;
            bufLen = Kernel32.GetPrivateProfileString(null, null, null, buffer, MaxSectionSize, FullPath);
            sectionList.Clear();
            if (bufLen != 0)
            {
                int start = 0;
                for (int i = 0; i < bufLen; i++)
                {
                    if ((buffer[i] == 0) && ((i - start) > 0))
                    {
                        String s = Encoding.GetEncoding("gb2312").GetString(buffer, start, i - start);
                        sectionList.Add(s);
                        start = i + 1;
                    }
                }
            }

            return sectionList;
        }

        /// <summary>      
        /// 从Ini文件中，将指定的Section名称中的所有key名取出
        /// </summary>
        /// <param name="section">块名</param>
        /// <returns>key名字串集合</returns>
        public StringCollection ReadSectionKeys(string section, string encodingName = "unicode")
        {
            if (section == null)
                throw new ArgumentNullException(nameof(section));

            byte[] buffer = new byte[MaxSectionSize];

            StringCollection retkeys = new StringCollection();

            int bufLen = Kernel32.GetPrivateProfileString(GetBytes(section, encodingName), null, null, buffer, MaxSectionSize, FullPath);

            if (bufLen != 0)
            {
                int start = 0;
                for (int i = 0; i < bufLen; i++)
                {
                    if ((buffer[i] == 0) && ((i - start) > 0))
                    {
                        string s = Encoding.GetEncoding("gb2312").GetString(buffer, start, i - start);
                        retkeys.Add(s);
                        start = i + 1;
                    }
                }
            }
            return retkeys;
        }

        //读取字段下的所有键名
        public StringCollection ReadSectionKeys(string section)
        {
            if (section == null)
                throw new ArgumentNullException(nameof(section));

            StringCollection retkeys = new StringCollection();
            //List<string> list = new List<string>();
            byte[] buf = new byte[65536];
            uint len = Kernel32.GetPrivateProfileStringA(section, null, null, buf, buf.Length, FullPath);
            int j = 0;
            for (int i = 0; i < len; i++)
            {
                if (buf[i] == 0)
                {
                    //list.Add(Encoding.Default.GetString(buf, j, i - j));
                    retkeys.Add(Encoding.Default.GetString(buf, j, i - j));
                    j = i + 1;
                }
            }
            //return list.ToArray();
            return retkeys;
        }

        /// <summary>
        ///  读取指定的Section的所有键值对到列表中
        /// </summary>
        /// <param name="section">块名</param>
        /// <returns></returns>
        public Dictionary<string, T> ReadSectionValues<T>(string section)
        {
            if (section == null)
                throw new ArgumentNullException(nameof(section));

            Dictionary<string, T> dict = new Dictionary<string, T>();

            StringCollection KeyList = ReadSectionKeys(section);

            foreach (string key in KeyList)
            {
                dict.Add(key, Read<T>(section, key));
            }

            return dict;
        }

        /// <summary>
        /// 检查某个Section是否存在。
        /// </summary>
        /// <param name="section">块</param>
        /// <returns>存在返回true，否则为false。</returns>
        public bool SectionIsExist(string section)
        {
            if (section == null)
                throw new ArgumentNullException(nameof(section));

            StringCollection Sections = this.ReadSectionNames();
            return Sections.IndexOf(section) > -1;
        }

        /// <summary>
        /// 检查某个Section的某个键是否存在。
        /// </summary>
        /// <param name="section">块</param>
        /// <param name="key">键</param>
        /// <returns>存在返回true，否则为false。</returns>
        public bool KeyIsExist(string section, string key)
        {
            if (section == null)
                throw new ArgumentNullException(nameof(section));

            if (key == null)
                throw new ArgumentNullException(nameof(key));

            StringCollection keys = this.ReadSectionKeys(section);
            return keys.IndexOf(key) > -1;
        }

        /// <summary>
        /// 删除某个Section
        /// </summary>
        /// <param name="section"></param>
        public void DeleteSection(string section)
        {
            if (section == null)
                throw new ArgumentNullException(nameof(section));

            if (!Kernel32.WritePrivateProfileString(section, null, null, FullPath))
            {
                throw new Exception("无法清除配置文件中的" + section + "!");
            }
        }

        /// <summary>
        /// 删除某个Section下的某个key
        /// </summary>
        /// <param name="section">块</param>
        /// <param name="key">键</param>
        public void DeleteKey(string section, string key)
        {
            if (section == null)
                throw new ArgumentNullException(nameof(section));

            if (key == null)
                throw new ArgumentNullException(nameof(key));

            if (!Kernel32.WritePrivateProfileString(section, key, null, FullPath))
            {
                throw new Exception("无法清除配置文件中" + section + "的" + key + "!");
            }
        }

        /// <summary>
        /// 对于Win9X来说需要实现UpdateFile方法将缓冲中的数据写入文件
        /// 在Win NT, 2000和XP上，都是直接写文件，没有缓冲，所以，无须实现UpdateFile
        /// 执行完对Ini文件的修改之后，应该调用本方法更新缓冲区。
        /// 暂时不用此方法
        /// </summary>
        public void UpdateFile()
        {
            //WritePrivateProfileString(null, null, null, FileName);
        }
        #endregion 公开方法

        #region    私有方法 start
        /// <summary>
        /// 与ini交互必须统一编码格式
        /// </summary>
        /// <param name="s"></param>
        /// <param name="encodingName"></param>
        /// <returns></returns>
        private static byte[] GetBytes<T>(T value, string encodingName)
        {
            return null == value ? null : Encoding.GetEncoding(encodingName).GetBytes(value.ToString());
        }
        #endregion 私有方法 start
    }
}