using System;
using System.Collections;
using System.IO;

namespace dbe.FoundationLibrary.Core.Util
{
    /// <summary>
    /// ���ù���
    /// </summary>
    public class InfoSetting
    {
        #region        ���� start
        /// <summary>
        /// ����Ŀ¼
        /// </summary>
        private const string settingFoder = "AppConfig";
        /// <summary>
        /// ����������Ϣ��hashtable����
        /// </summary>
        private Hashtable settingHash = null;
        /// <summary>
        /// ��������
        /// </summary>
        private string settingFileName = string.Empty;
        #endregion ���� end

        #region        ���������� start
        /// <summary>
        /// ����
        /// </summary>
        /// <param name="settingFileName">��������</param>
        public InfoSetting(string settingFileName)
        {
            this.settingFileName = settingFileName;
            Initialize();
        }
        #endregion ���������� end

        #region        ���� start
        /// <summary>
        /// �����ļ�·��
        /// </summary>
        public string SettingFilePath
        {
            get
            {
                //return Application.StartupPath + "\\" + settingFoder + "\\" + settingFileName + ".xxpcs";
                return $@".\{settingFoder}\{settingFileName}.xxpcs";
            }
        }

        /// <summary>
        /// �����ļ������ļ���
        /// </summary>
        public string SettingFileFolder
        {
            get
            {
                //return Application.StartupPath + "\\" + settingFoder;
                return $@".\{settingFoder}";
            }
        }

        /// <summary>
        /// ��ȡ���õ�hashtable
        /// </summary>
        private Hashtable GetSetHash
        {
            get
            {
                if (settingHash == null)
                {
                    settingHash = Deserialize();
                    if (settingHash == null)
                    {
                        settingHash = new Hashtable();
                    }
                }
                return settingHash;
            }
        }
        #endregion ���� end

        #region        ���� start
        /// <summary>
        /// ��ȡ������ ���ò�����ֵ
        /// �ر�˵��,��Ϊ��Ҫ���л�����,����ֵֻ���û�������,��:string bool int ֮��
        /// </summary>
        /// <param name="paraname">��������,paranameֻ����string</param>
        public object this[string paraname]
        {
            get
            {
                object retvalue = null;
                try
                {
                    retvalue = this.GetSetHash[paraname];
                }
                catch
                {
                    retvalue = null;
                }
                return retvalue;
            }
            set
            {
                this.GetSetHash[paraname] = value;
            }
        }
        #endregion ���� end

        #region        �������� start
        /// <summary>
        /// ��ո�ֵ
        /// </summary>
        public void Clear()
        {
            settingHash = new Hashtable();
        }

        /// <summary>
        /// ��ȡ�����ļ�,��ʼ��hashtable
        /// </summary>
        public void Initialize()
        {
            settingHash = Deserialize();
            if (settingHash == null)
            {
                settingHash = new Hashtable();
            }
        }

        /// <summary>
        /// ���������ļ�
        /// </summary>
        public void Save()
        {
            Serialize();
        }

        /// <summary>
        /// ɾ�������ļ�
        /// </summary>
        public void Delete()
        {
            try
            {
                File.Delete(this.SettingFilePath);
            }
            catch (Exception ex)
            {
                string s = ex.Message;
            }
        }
        #endregion �������� end

        #region        ˽�з��� start
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        private Hashtable Deserialize()
        {
            return (Hashtable)FileHelper.Deserialize(this.SettingFilePath);
        }

        /// <summary>
        /// ����
        /// </summary>
        private void Serialize()
        {
            if (!Directory.Exists(SettingFileFolder))
            {
                DirectoryInfo di = Directory.CreateDirectory(SettingFileFolder);
            }
            FileHelper.Serialize(this.settingHash, this.SettingFilePath);
        }
        #endregion ˽�з��� end
    }
}