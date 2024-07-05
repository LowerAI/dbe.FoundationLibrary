using System;
using System.Collections;
using System.IO;

namespace dbe.FoundationLibrary.Core.Util
{
    /// <summary>
    /// 配置管理
    /// </summary>
    public class InfoSetting
    {
        #region        变量 start
        /// <summary>
        /// 配置目录
        /// </summary>
        private const string settingFoder = "AppConfig";
        /// <summary>
        /// 保存配置信息的hashtable对象
        /// </summary>
        private Hashtable settingHash = null;
        /// <summary>
        /// 配置名称
        /// </summary>
        private string settingFileName = string.Empty;
        #endregion 变量 end

        #region        构造与析构 start
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="settingFileName">配置名称</param>
        public InfoSetting(string settingFileName)
        {
            this.settingFileName = settingFileName;
            Initialize();
        }
        #endregion 构造与析构 end

        #region        属性 start
        /// <summary>
        /// 配置文件路径
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
        /// 配置文件所在文件夹
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
        /// 获取配置的hashtable
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
        #endregion 属性 end

        #region        索引 start
        /// <summary>
        /// 获取或设置 配置参数的值
        /// 特别说明,因为需要序列化过程,所以值只能用基本类型,如:string bool int 之类
        /// </summary>
        /// <param name="paraname">参数名称,paraname只能用string</param>
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
        #endregion 索引 end

        #region        公开方法 start
        /// <summary>
        /// 清空赋值
        /// </summary>
        public void Clear()
        {
            settingHash = new Hashtable();
        }

        /// <summary>
        /// 读取配置文件,初始化hashtable
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
        /// 保存配置文件
        /// </summary>
        public void Save()
        {
            Serialize();
        }

        /// <summary>
        /// 删除配置文件
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
        #endregion 公开方法 end

        #region        私有方法 start
        /// <summary>
        /// 解密
        /// </summary>
        /// <returns></returns>
        private Hashtable Deserialize()
        {
            return (Hashtable)FileHelper.Deserialize(this.SettingFilePath);
        }

        /// <summary>
        /// 加密
        /// </summary>
        private void Serialize()
        {
            if (!Directory.Exists(SettingFileFolder))
            {
                DirectoryInfo di = Directory.CreateDirectory(SettingFileFolder);
            }
            FileHelper.Serialize(this.settingHash, this.SettingFilePath);
        }
        #endregion 私有方法 end
    }
}