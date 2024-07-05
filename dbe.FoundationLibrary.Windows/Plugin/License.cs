using System;
using System.IO;

namespace GNDView.Library.Plugin
{
    /// <summary>
    /// 验证插件委托
    /// </summary>
    /// <param name="license">证书</param>
    /// <returns>返回是否通过</returns>
    public delegate bool LicenseValidate(License license);

    /// <summary>
    /// 插件使用的安全证书。
    /// </summary>
    public class License : System.ComponentModel.License
    {
        #region        变量 start
        //private Version version;
        private string licenseKey;
        //private string issuer;
        //private string user;
        //private string enterprise;
        //private object tag;
        #endregion 变量 end

        #region        构造与析构 start
        /// <summary>
        /// 创建插件证书的新实例
        /// </summary>
        /// <param name="version">当前主程序(插件容器)的版本</param>
        public License(Version version)
        {
            this.Version = version;
        }

        /// <summary>
        /// 创建插件证书的新实例
        /// </summary>
        /// <param name="version">当前主程序(插件容器)的版本</param>
        /// <param name="licenseKey">证书序列号</param>
        public License(Version version, string licenseKey)
            : this(version)
        {
            this.licenseKey = licenseKey;
        }

        /// <summary>
        /// 创建插件证书的新实例
        /// </summary>
        /// <param name="version">当前主程序(插件容器)的版本</param>
        /// <param name="licenseKey">证书序列号</param>
        /// <param name="issuer">证书的颁发者</param>
        public License(Version version, string licenseKey, string issuer)
            : this(version, licenseKey)
        {
            this.Issuer = issuer;
        }

        /// <summary>
        /// 创建插件证书的新实例
        /// </summary>
        /// <param name="version">当前主程序(插件容器)的版本</param>
        /// <param name="licenseKey">证书序列号</param>
        /// <param name="issuer">证书的颁发者</param>
        /// <param name="user">证书使用者</param>
        public License(Version version, string licenseKey, string issuer, string user)
            : this(version, licenseKey, issuer)
        {
            this.User = user;
        }

        /// <summary>
        /// 创建插件证书的新实例
        /// </summary>
        /// <param name="version">当前主程序(插件容器)的版本</param>
        /// <param name="licenseKey">证书序列号</param>
        /// <param name="issuer">证书的颁发者</param>
        /// <param name="user">证书使用者</param>
        /// <param name="enterprise">证书使用单位编号</param>
        public License(Version version, string licenseKey, string issuer, string user, string enterprise)
            : this(version, licenseKey, issuer, user)
        {
            this.Enterprise = enterprise;
        }
        #endregion 构造与析构 end

        #region        属性 start
        /// <summary>
        /// 当前主程序(插件容器)的版本。
        /// </summary>
        public Version Version { get; private set; }

        /// <summary>
        ///  证书序列号
        /// </summary>
        public override string LicenseKey
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        /// <summary>
        /// 证书的颁发者。
        /// </summary>
        public string Issuer { get; private set; }

        /// <summary>
        /// 证书使用者。
        /// </summary>
        public string User { get; private set; }

        /// <summary>
        /// 证书使用单位。
        /// </summary>
        public string Enterprise { get; private set; }

        /// <summary>
        /// 获取或设置一个对象 
        /// </summary>
        public object Tag { get; set; }
        #endregion 属性 end

        #region        公开方法 start
        /// <summary>
        /// 导入证书
        /// </summary>
        /// <param name="fileName">文件名</param>
        public void LoadLicense(string fileName)
        {

        }

        /// <summary>
        /// 导入证书
        /// </summary>
        /// <param name="stream">证书数据流</param>
        public void LoadLicense(Stream stream)
        {

        }

        /// <summary>
        /// 释放该许可证使用的资源。
        /// </summary>
        public override void Dispose()
        {
            throw new Exception("The method or operation is not implemented.");
        }
        #endregion 公开方法 end
    }
}