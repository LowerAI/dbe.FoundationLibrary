using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace GNDView.Library.Plugin
{
    /// <summary>
    /// 功能部件
    /// </summary>
    public abstract class Feature
    {
        #region        变量 start
        //private ExtensionCollection extensions;
        //private License license;
        //private bool enable;
        #endregion 变量 end

        #region        属性 start
        /// <summary>
        /// 当前功能部件的版本
        /// </summary>
        public abstract Version FeatureVersion { get; }

        /// <summary>
        /// 功能部件的扩展点集合。
        /// </summary>
        public ExtensionCollection Extensions { get; set; }

        /// <summary>
        /// 功能部件的安全证书
        /// </summary>
        public License License { get; set; }

        /// <summary>
        /// 是否启动
        /// </summary>
        public bool Enable { get; set; }
        #endregion 属性 end

        #region        公开方法 start
        /// <summary>
        /// 功能部件入口函数
        /// </summary>
        public abstract void Entry();

        /// <summary>
        /// 加载功能部件
        /// </summary>
        /// <param name="featureName">功能部件名称</param>
        /// <param name="extensions">主应用程序扩展点</param>
        /// <returns>返回功能部件程序集</returns>
        public static Assembly LoadFeature(string featureName, ExtensionCollection extensions)
        {
            //设置参数默认值
            LicenseValidate licenseValidate = delegate(License license)
            {
                return true;
            };

            return LoadFeature(featureName, extensions, licenseValidate);
        }

        /// <summary>
        /// 加载功能部件
        /// </summary>
        /// <param name="featureName">功能部件名称</param>
        /// <param name="extensions">主应用程序扩展点</param>
        /// <param name="licenseValidate">证书</param>
        /// <returns>返回功能部件程序集</returns>
        public static Assembly LoadFeature(string featureName, ExtensionCollection extensions, LicenseValidate licenseValidate)
        {
            var assemblyFullPath = $"{Application.StartupPath}\\{featureName}";
            //如果程序集不存在,则返回空
            if (!File.Exists(assemblyFullPath)) return null;

            //设置参数默认值
            if (licenseValidate == null)
            {
                licenseValidate = delegate(License license)
                {
                    return true;
                };
            }

            //返回的功能部件程序集
            Assembly featureAssembly = Assembly.LoadFile(assemblyFullPath);
            FeatureAttribute featureAttribute = featureAssembly.GetCustomAttributes(typeof(FeatureAttribute), false)[0] as FeatureAttribute;

            try
            {
                //获取功能部件入口类型并创建实例
                if (featureAttribute.EntryClass.BaseType != typeof(Feature))
                {
                    throw new Exception("入口类型不正确！");
                }

                //验证功能部件证书
                License license = (License)featureAttribute.EntryClass.GetProperty("License").GetValue(featureAttribute.EntryObject, null);
                if (licenseValidate(license))
                {
                    //给功能部件入口类的属性赋值
                    featureAttribute.EntryClass.GetProperty("Extensions").SetValue(featureAttribute.EntryObject, extensions, null);

                    //调用功能部件入口函数
                    featureAttribute.EntryClass.GetMethod("Entry").Invoke(featureAttribute.EntryObject, null);

                    //设置插件为启动状态
                    featureAttribute.EntryClass.GetProperty("Enable").SetValue(featureAttribute.EntryObject, true, null);
                }
                else
                {
                    throw new Exception("功能部件证书无效");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return featureAssembly;
        }
    }
    #endregion 公开方法 end
}