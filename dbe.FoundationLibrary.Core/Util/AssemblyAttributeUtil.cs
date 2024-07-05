using dbe.FoundationLibrary.Core.Common;
using dbe.FoundationLibrary.Core.Extensions;

using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace dbe.FoundationLibrary.Core.Util
{
    /// <summary>
    /// 程序集特性访问类
    /// </summary>
    public class AssemblyAttributeUtil
    {
        #region    字段 start
        private readonly Assembly assembly;
        #endregion 字段 end

        #region    属性 start
        // <summary>
        /// 程序集-标题(通常是项目的英文名)
        /// </summary>
        public string AssemblyName
        {
            get { return assembly.GetName().Name; }
        }

        /// <summary>
        /// 程序集-标题(通常是项目的中文名)
        /// </summary>
        public string AssemblyTitle
        {
            get
            {
                var titleAttr = GetAttributeFromAssembly<AssemblyTitleAttribute>()?.Title;
                titleAttr = titleAttr ?? Path.GetFileNameWithoutExtension(assembly.CodeBase);
                return titleAttr;
            }
        }

        /// <summary>
        /// 程序集-描述
        /// </summary>
        public string AssemblyDescription
        {
            get
            {
                return GetAttributeFromAssembly<AssemblyDescriptionAttribute>()?.Description;
            }
        }

        /// <summary>
        /// 程序集-生成配置 <=> WinFormEnvironment
        /// </summary>
        public string AssemblyConfiguration
        {
            get
            {
                return GetAttributeFromAssembly<AssemblyConfigurationAttribute>()?.Configuration;
            }
        }

        /// <summary>
        /// 程序集-发布环境
        /// </summary>
        public WinFormEnvironment DeploymentEnvironment
        {
            get
            {
                return AssemblyConfiguration.ToEnum<WinFormEnvironment>();
            }
        }

        /// <summary>
        /// 程序集-公司名
        /// </summary>
        public string AssemblyCompany
        {
            get
            {
                return GetAttributeFromAssembly<AssemblyCompanyAttribute>()?.Company;
            }
        }

        /// <summary>
        /// 程序集-产品名
        /// </summary>
        public string AssemblyProduct
        {
            get
            {
                return GetAttributeFromAssembly<AssemblyProductAttribute>()?.Product;
            }
        }

        /// <summary>
        /// 程序集-版权
        /// </summary>
        public string AssemblyCopyright
        {
            get
            {
                return GetAttributeFromAssembly<AssemblyCopyrightAttribute>()?.Copyright;
            }
        }

        /// <summary>
        /// 程序集-商标
        /// </summary>
        public string AssemblyTrademark
        {
            get
            {
                return GetAttributeFromAssembly<AssemblyTrademarkAttribute>()?.Trademark;
            }
        }

        /// <summary>
        /// 程序集-文件版本
        /// </summary>
        public string AssemblyVersion
        {
            get
            {
                var verAttr = assembly.GetName().Version.ToString();
                return verAttr;
            }
        }

        /// <summary>
        /// 程序集-产品版本
        /// </summary>
        public string AssemblyInformationalVersion
        {
            get
            {
                var version = GetAttributeFromAssembly<AssemblyInformationalVersionAttribute>()?.InformationalVersion;
                if (version.Contains('+'))
                {
                    var regExp = new Regex(@"^(.+)\+", RegexOptions.IgnoreCase);
                    version = regExp.Match(version).Value.TrimEnd('+');
                }
                return version;
            }
        }

        /// <summary>
        /// 编译时间
        /// </summary>
        public string CompiledOn
        {
            get
            {
                var dt = File.GetLastWriteTime(assembly.Location);
                return string.Format("{0:u}", dt);
            }
        }
        #endregion 属性 end

        #region    构造与析构 start
        public AssemblyAttributeUtil(Assembly assembly = null)
        {
            this.assembly = assembly ?? Assembly.GetEntryAssembly();
        }
        #endregion 构造与析构 end

        #region    公开方法 start

        #endregion 公开方法 end

        #region    私有方法 start
        /// <summary>
        /// 获取程序集中指定类型得特性值
        /// </summary>
        /// <typeparam name="T">指定类型</typeparam>
        /// <returns>特性值</returns>
        private T GetAttributeFromAssembly<T>() where T : Attribute
        {
            //object[] attributes = assembly.GetCustomAttributes(typeof(T), false);
            //if (attributes.Length == 0)
            //{
            //    return null;
            //}
            //return (T)attributes[0];

            return assembly.GetCustomAttributes(typeof(T), false).FirstOrDefault() as T;
        }
        #endregion 私有方法 end
    }
}