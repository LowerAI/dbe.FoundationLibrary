using dbe.FoundationLibrary.Core.Win32.API;

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace dbe.FoundationLibrary.Core.Util
{
    /// <summary>
    /// INI�ļ���д������
    /// </summary>  
    public sealed class IniRW
    {
        /// <summary>
        /// ini�ļ���һ������(section)������С�������Զ��建����������С�����ڴ�ini�ļ��м������ݡ����ֵ��win32����GetPrivateProfileSectionNames()��GetPrivateProfileString ()��������ֵ��
        /// </summary>
        private const int MaxSectionSize = 16384;//32767;

        private string fullPath;
        /// <summary>
        /// ����ini�ļ�����·��
        /// </summary>
        public string FullPath { get => fullPath; }

        /// <summary>
        /// ���캯����  
        /// </summary>
        /// <param name="fileName">ini�ļ���,ȫ·��</param> 
        public IniRW(string fileName)
        {
            //��������ȫ·�������������·��
            fullPath = fileName;
        }

        #region    ��д���� start
        /// <summary>
        /// ��ini�ļ��ж�ȡָ������T��ֵ��
        /// ����ȡʧ��ʱ����ȱʡֵ
        /// </summary>
        /// <param name="Section">��</param>
        /// <param name="key">��</param>
        /// <param name="Default">ȱʡֵ</param>
        /// <returns>string�ͷ���ֵ</returns>
        public T Read<T>(string section, string key, string encodingName = "unicode")
        {
            if (section == null)
                throw new ArgumentNullException(nameof(section));

            if (key == null)
                throw new ArgumentNullException(nameof(key));

            byte[] buffer = new byte[MaxSectionSize];
            int bufLen = Kernel32.GetPrivateProfileString(GetBytes(section, encodingName), GetBytes(key, encodingName), GetBytes(default(T), encodingName), buffer, MaxSectionSize, FullPath);
            //�����趨0��ϵͳĬ�ϵĴ���ҳ���ı��뷽ʽ�������޷�֧������
            string s = Encoding.GetEncoding(encodingName).GetString(buffer);

            s = s.Substring(0, (int)bufLen);
            T value = (T)Convert.ChangeType(s, typeof(T));
            return value;
        }

        /// <summary>
        /// ��ָ������T��ֵд��ini
        /// </summary>
        /// <param name="Section">��</param>
        /// <param name="key">��</param>
        /// <param name="Value">T����ֵ</param>
        public bool Write<T>(string section, string key, T value, string encodingName = "unicode")
        {
            if (section == null)
                throw new ArgumentNullException(nameof(section));

            if (key == null)
                throw new ArgumentNullException(nameof(key));

            //return WritePrivateProfileString(GetBytes(section, encodingName), GetBytes(key, encodingName), GetBytes<T>(value, encodingName), FullPath);
            return Kernel32.WritePrivateProfileString(section, key, value?.ToString(), FullPath);
        }
        #endregion ��д���� end

        #region ��������
        /// <summary>
        /// ��Ini�ļ��У���ȡ����Section������
        /// </summary>
        /// <returns>Section���ִ�����</returns>
        public StringCollection ReadSectionNames()
        {
            StringCollection sectionList = new StringCollection();
            //Note:�������Bytes��ʵ�֣�StringBuilderֻ��ȡ����һ��Section
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
        /// ��Ini�ļ��У���ָ����Section�����е�����key��ȡ��
        /// </summary>
        /// <param name="section">����</param>
        /// <returns>key���ִ�����</returns>
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

        //��ȡ�ֶ��µ����м���
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
        ///  ��ȡָ����Section�����м�ֵ�Ե��б���
        /// </summary>
        /// <param name="section">����</param>
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
        /// ���ĳ��Section�Ƿ���ڡ�
        /// </summary>
        /// <param name="section">��</param>
        /// <returns>���ڷ���true������Ϊfalse��</returns>
        public bool SectionIsExist(string section)
        {
            if (section == null)
                throw new ArgumentNullException(nameof(section));

            StringCollection Sections = this.ReadSectionNames();
            return Sections.IndexOf(section) > -1;
        }

        /// <summary>
        /// ���ĳ��Section��ĳ�����Ƿ���ڡ�
        /// </summary>
        /// <param name="section">��</param>
        /// <param name="key">��</param>
        /// <returns>���ڷ���true������Ϊfalse��</returns>
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
        /// ɾ��ĳ��Section
        /// </summary>
        /// <param name="section"></param>
        public void DeleteSection(string section)
        {
            if (section == null)
                throw new ArgumentNullException(nameof(section));

            if (!Kernel32.WritePrivateProfileString(section, null, null, FullPath))
            {
                throw new Exception("�޷���������ļ��е�" + section + "!");
            }
        }

        /// <summary>
        /// ɾ��ĳ��Section�µ�ĳ��key
        /// </summary>
        /// <param name="section">��</param>
        /// <param name="key">��</param>
        public void DeleteKey(string section, string key)
        {
            if (section == null)
                throw new ArgumentNullException(nameof(section));

            if (key == null)
                throw new ArgumentNullException(nameof(key));

            if (!Kernel32.WritePrivateProfileString(section, key, null, FullPath))
            {
                throw new Exception("�޷���������ļ���" + section + "��" + key + "!");
            }
        }

        /// <summary>
        /// ����Win9X��˵��Ҫʵ��UpdateFile�����������е�����д���ļ�
        /// ��Win NT, 2000��XP�ϣ�����ֱ��д�ļ���û�л��壬���ԣ�����ʵ��UpdateFile
        /// ִ�����Ini�ļ����޸�֮��Ӧ�õ��ñ��������»�������
        /// ��ʱ���ô˷���
        /// </summary>
        public void UpdateFile()
        {
            //WritePrivateProfileString(null, null, null, FileName);
        }
        #endregion ��������

        #region    ˽�з��� start
        /// <summary>
        /// ��ini��������ͳһ�����ʽ
        /// </summary>
        /// <param name="s"></param>
        /// <param name="encodingName"></param>
        /// <returns></returns>
        private static byte[] GetBytes<T>(T value, string encodingName)
        {
            return null == value ? null : Encoding.GetEncoding(encodingName).GetBytes(value.ToString());
        }
        #endregion ˽�з��� start
    }
}