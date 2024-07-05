using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace GNDView.Library.Plugin
{
    /// <summary>
    /// 加载插件的进度信息委托,让你可以自定义如何显示加载插件的进度信息
    /// </summary>
    /// <param name="msg">加载插件时的过程信息</param>
    public delegate void ProgressInfoHandler(string msg, bool isLineBreak = true);

    /// <summary>
    /// 插件基类(抽象类)
    /// </summary>
    public abstract class Plugin
    {
        #region        变量 start
        ///// <summary>
        ///// 扩展点集合
        ///// </summary>
        //private ExtensionCollection extensions;
        ///// <summary>
        ///// 安全证书
        ///// </summary>
        //private License license;
        ///// <summary>
        ///// 是否启用
        ///// </summary>
        //private bool enable;
        #endregion 变量 end

        #region        属性 start
        /// <summary>
        /// 当前插件的版本
        /// </summary>
        public abstract Version PluginVersion { get; }

        /// <summary>
        /// 获取设置该插件的扩展点集合。
        /// </summary>
        public ExtensionCollection Extensions { get; set; }

        /// <summary>
        /// 获取设置该插件的安全证书
        /// </summary>
        public License License { get; set; }

        /// <summary>
        /// 是否启动
        /// </summary>
        public bool Enable { get; set; }
        #endregion 属性 end

        #region        公开方法 start
        /// <summary>
        /// 插件入口方法
        /// </summary>
        public abstract void Entry();

        /// <summary>
        /// 注册本插件的扩展点,重写此方法以注册插件的扩展点
        /// </summary>
        public virtual void ExtensionRegistry()
        {

        }

        /// <summary>
        /// 加载本插件的子插件,重写此方法以加载子插件
        /// </summary>
        public virtual void LoadChildPlugins()
        {

        }

        /// <summary>
        /// 加载,过滤和排序插件程序集
        /// </summary>
        /// <param name="pluginName">要加载的插件名称,可用通配符,如Agile.Plugins.*.dll</param>
        /// <param name="extensions">主应用程序扩展点</param>
        /// <returns>返回插件程序集</returns>
        public static AssemblyCollection LoadPlugins(string pluginName, ExtensionCollection extensions)
        {
            //设置参数默认值
            LicenseValidate licenseValidate = delegate (License license)
            {
                return true;
            };

            return LoadPlugins(pluginName, extensions, licenseValidate);
        }

        /// <summary>
        /// 加载,过滤和排序插件程序集
        /// </summary>
        /// <param name="pluginName">要加载的插件名称,可用通配符,如Agile.Plugins.*.dll</param>
        /// <param name="extensions">主应用程序扩展点</param>
        /// <param name="licenseValidate">插件证书</param>
        /// <returns>返回插件程序集</returns>
        public static AssemblyCollection LoadPlugins(string pluginName, ExtensionCollection extensions, LicenseValidate licenseValidate)
        {
            //设置参数默认值
            ProgressInfoHandler progressInfo = delegate (string msg, bool isLineBreak)
            {
                System.Diagnostics.Debug.WriteLine(msg);
            };

            return LoadPlugins(pluginName, extensions, licenseValidate, progressInfo);
        }

        /// <summary>
        /// 加载,过滤和排序插件程序集
        /// </summary>
        /// <param name="pluginName">要加载的插件名称,可用通配符,如Agile.Plugins.*.dll</param>
        /// <param name="extensions">主应用程序扩展点</param>
        /// <param name="licenseValidate">插件证书</param>
        /// <param name="progressInfo">加载插件进度信息</param>
        /// <returns>返回插件程序集</returns>
        public static AssemblyCollection LoadPlugins(string pluginName, ExtensionCollection extensions, LicenseValidate licenseValidate, ProgressInfoHandler progressInfo)
        {
            string[] pluginNames = new string[] { pluginName };
            return LoadPlugins(pluginNames, extensions, licenseValidate, progressInfo);
        }

        /// <summary>
        /// 加载,过滤和排序插件程序集
        /// </summary>
        /// <param name="pluginName">要加载的插件名称,可用通配符,如Agile.Plugins.*.dll</param>
        /// <param name="extensions">主应用程序扩展点</param>
        /// <param name="licenseValidate">插件证书</param>
        /// <param name="progressInfo">加载插件进度信息</param>
        /// <param name="pluginHashTable">插件与插件属性对应表</param>
        /// <returns>返回插件程序集</returns>
        public static AssemblyCollection LoadPlugins(string pluginName, ExtensionCollection extensions, LicenseValidate licenseValidate, ProgressInfoHandler progressInfo, out Hashtable pluginHashTable)
        {
            string[] pluginNames = new string[] { pluginName };
            return LoadPlugins(pluginNames, extensions, licenseValidate, progressInfo, out pluginHashTable);
        }

        /// <summary>
        /// 加载,过滤和排序插件程序集
        /// </summary>
        /// <param name="pluginNames">要加载的插件名称,一个字符串数组,数组元素可用通配符,如Agile.Plugins.*.dll</param>
        /// <param name="extensions">主应用程序扩展点</param>
        /// <returns>返回插件程序集</returns>
        public static AssemblyCollection LoadPlugins(string[] pluginNames, ExtensionCollection extensions)
        {
            //设置参数默认值
            LicenseValidate licenseValidate = delegate (License license)
            {
                return true;
            };

            return LoadPlugins(pluginNames, extensions, licenseValidate);
        }

        /// <summary>
        /// 加载,过滤和排序插件程序集
        /// </summary>
        /// <param name="pluginNames">要加载的插件名称,一个字符串数组,数组元素可用通配符,如Agile.Plugins.*.dll</param>
        /// <param name="extensions">主应用程序扩展点</param>
        /// <param name="licenseValidate">插件证书</param>
        /// <returns>返回插件程序集</returns>
        public static AssemblyCollection LoadPlugins(string[] pluginNames, ExtensionCollection extensions, LicenseValidate licenseValidate)
        {
            //设置参数默认值
            ProgressInfoHandler progressInfo = delegate (string msg, bool isLineBreak)
            {
                System.Diagnostics.Debug.WriteLine(msg);
            };

            return LoadPlugins(pluginNames, extensions, licenseValidate, progressInfo);
        }

        /// <summary>
        /// 加载,过滤和排序插件程序集
        /// </summary>
        /// <param name="pluginNames">要加载的插件名称,一个字符串数组,数组元素可用通配符,如Agile.Plugins.*.dll</param>
        /// <param name="extensions">主应用程序扩展点</param>
        /// <param name="licenseValidate">插件证书</param>
        /// <param name="progressInfo">加载插件进度信息</param>
        /// <returns>返回插件程序集</returns>
        public static AssemblyCollection LoadPlugins(string[] pluginNames, ExtensionCollection extensions, LicenseValidate licenseValidate, ProgressInfoHandler progressInfo)
        {
            Hashtable pluginHashTable;
            return LoadPlugins(pluginNames, extensions, licenseValidate, progressInfo, out pluginHashTable);
        }

        /// <summary>
        /// 加载,过滤和排序插件程序集
        /// </summary>
        /// <param name="pluginNames">要加载的插件名称,一个字符串数组,数组元素可用通配符,如Agile.Plugins.*.dll</param>
        /// <param name="extensions">主应用程序扩展点</param>
        /// <param name="licenseValidate">插件证书</param>
        /// <param name="progressInfo">加载插件进度信息</param>
        /// <param name="pluginHashTable">插件与插件属性对应表</param>
        /// <returns>返回插件程序集</returns>
        public static AssemblyCollection LoadPlugins(string[] pluginNames, ExtensionCollection extensions, LicenseValidate licenseValidate, ProgressInfoHandler progressInfo, out Hashtable pluginHashTable)
        {
            //获取和过滤插件程序集
            AssemblyCollection pluginAssemblys = new AssemblyCollection();
            ArrayList pluginAttributes = new ArrayList();

            pluginHashTable = new Hashtable();

            foreach (string pluginName in pluginNames)
            {
                string[] filenames = Directory.GetFiles(Application.StartupPath, pluginName, SearchOption.TopDirectoryOnly);
                for (int i = 0; i < filenames.Length; i++)
                {
                    Assembly assembly = Assembly.LoadFile(filenames[i]);

                    object[] attributes = assembly.GetCustomAttributes(typeof(PluginAttribute), false);
                    if (attributes.Length > 0)
                    {
                        pluginAssemblys.Add(assembly);
                        pluginAttributes.Add(attributes[0]);
                    }
                }
            }

            //排序插件程序集
            for (int i = 0; i < pluginAssemblys.Count; i++)
            {
                int k = i;
                for (int j = i + 1; j < pluginAssemblys.Count; j++)
                {
                    if ((pluginAttributes[k] as PluginAttribute).Index > (pluginAttributes[j] as PluginAttribute).Index)
                        k = j;
                }
                if (k != i)
                {
                    Assembly assembly = (Assembly)pluginAssemblys[i];
                    pluginAssemblys[i] = pluginAssemblys[k];
                    pluginAssemblys[k] = assembly;

                    PluginAttribute pluginAttribute = pluginAttributes[i] as PluginAttribute;
                    pluginAttributes[i] = pluginAttributes[k];
                    pluginAttributes[k] = pluginAttribute;
                }
            }

            if (pluginAttributes.Count > 0)
            {
                PluginVar.GlobalMaxPluginIndex = (pluginAttributes[pluginAttributes.Count - 1] as PluginAttribute).Index;
            }

            //开始加载插件
            for (int i = 0; i < pluginAssemblys.Count; i++)
            {
                PluginAttribute pluginAttribute = pluginAssemblys[i].GetCustomAttributes(typeof(PluginAttribute), false)[0] as PluginAttribute;

                //获得程序集中英文名的集合
                pluginHashTable.Add(pluginAssemblys[i], pluginAttribute);

                //加载插件进度信息
                progressInfo("初始化 " + pluginAttribute.Name + "插件...");

                try
                {
                    //获取插件入口类型并创建实例
                    if (pluginAttribute.EntryClass.BaseType != typeof(Plugin))
                    {
                        progressInfo("失败,入口类型不正确！");
                    }

                    //验证插件证书
                    License license = (License)pluginAttribute.EntryClass.GetProperty("License").GetValue(pluginAttribute.EntryObject, null);
                    if (licenseValidate(license))
                    {
                        //给插件入口类的属性赋值
                        pluginAttribute.EntryClass.GetProperty("Extensions").SetValue(pluginAttribute.EntryObject, extensions, null);

                        //调用插件入口函数
                        pluginAttribute.EntryClass.GetMethod("Entry").Invoke(pluginAttribute.EntryObject, null);

                        //注册插件的扩展点
                        pluginAttribute.EntryClass.GetMethod("ExtensionRegistry").Invoke(pluginAttribute.EntryObject, null);

                        //加载本插件的子插件
                        pluginAttribute.EntryClass.GetMethod("LoadChildPlugins").Invoke(pluginAttribute.EntryObject, null);

                        //设置插件为启动状态
                        pluginAttribute.EntryClass.GetProperty("Enable").SetValue(pluginAttribute.EntryObject, true, null);

                        progressInfo("成功\n");
                    }
                    else
                    {
                        progressInfo("无权\n");
                    }
                }
                catch (Exception ex)
                {
                    progressInfo("失败," + ex.Message + "\n");
                }
            }

            return pluginAssemblys;
        }
        #endregion 公开方法 end
    }
}