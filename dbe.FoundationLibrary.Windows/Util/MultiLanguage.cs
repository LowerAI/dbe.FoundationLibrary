using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Threading;
using System.Windows.Forms;

namespace dbe.FoundationLibrary.Windows.Util
{
    public static class MultiLanguage
    {
        #region    变量 start
        //当前默认语言
        public static string DefaultLanguage = "zh-CN";

        private static Dictionary<string, Dictionary<string, string>> languageData;

        //自定义多语言资源文件名
        private const string _englishResourceBaseName = "CameraOnWafer.Properties.Language-en-US";
        private const string _chineseResourceBaseName = "CameraOnWafer.Properties.Language-zh-CN";

        //当前自定义多语言资源文件
        public static ResourceManager DefaultResourceManager;
        #endregion 变量 end

        #region    属性 start
        #endregion 属性 end

        #region    构造与析构 start
        static MultiLanguage()
        {
            languageData = new Dictionary<string, Dictionary<string, string>>();
            Assembly asm = Assembly.GetEntryAssembly();
            var resourceFileNames = asm.GetManifestResourceNames();

        }
        #endregion 构造与析构 end

        #region    公开方法 start
        /// <summary>
        /// 修改默认语言
        /// </summary>
        /// <param name="lang">待设置默认语言</param>
        public static void SetDefaultLanguage(string lang)
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(lang);
            DefaultLanguage = lang;
            //Properties.Settings.Default.DefaultLanguage = lang;
            //Properties.Settings.Default.Save();

            //自定义的语言资源文件
            Assembly asm = Assembly.GetExecutingAssembly();
            if (lang == "zh-CN")
            {
                DefaultResourceManager = new ResourceManager(_chineseResourceBaseName, asm);
            }
            else
            {
                DefaultResourceManager = new ResourceManager(_englishResourceBaseName, asm);
            }
        }

        /// <summary>
        /// 加载语言
        /// </summary>
        /// <param name="form">加载语言的窗口</param>
        /// <param name="formType">窗口的类型</param>
        public static void LoadLanguage(Form form, Type formType)
        {
            if (form != null)
            {
                ComponentResourceManager resources = new ComponentResourceManager(formType);
                resources.ApplyResources(form, "$this");
                Loading(form, resources);
            }
        }

        public static string returnString(string str)
        {
            string strReturn = "--";
            try
            {
                //if (MultiLanguage.DefaultResourceManager != null)
                //{
                //    strReturn = MultiLanguage.DefaultResourceManager.GetString(str);
                //}
                strReturn = DefaultResourceManager?.GetString(str);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return strReturn;
        }
        #endregion 公开方法 end

        #region    私有方法 start
        /// <summary>
        /// 加载语言
        /// </summary>
        /// <param name="control">控件</param>
        /// <param name="resources">语言资源</param>
        private static void Loading(Control control, ComponentResourceManager resources)
        {
            if (control is MenuStrip menuStrip)
            {
                //将资源与控件对应
                resources.ApplyResources(control, control.Name);
                //MenuStrip ms = (MenuStrip)control;
                if (menuStrip.Items.Count > 0)
                {
                    foreach (ToolStripMenuItem tsm in menuStrip.Items)
                    {//遍历菜单
                        Loading(tsm, resources);
                    }
                }
            }

            if (control is StatusStrip statusStrip)
            {
                //将资源与控件对应
                resources.ApplyResources(control, control.Name);
                //StatusStrip ts = (StatusStrip)control;

                foreach (ToolStripItem tsm in statusStrip.Items)
                {//遍历菜单
                    resources.ApplyResources(tsm, tsm.Name);
                }
            }

            foreach (Control childControl in control.Controls)
            {
                resources.ApplyResources(childControl, childControl.Name);
                Loading(childControl, resources);
            }
        }

        /// <summary>
        /// 遍历菜单
        /// </summary>
        /// <param name="item">菜单项</param>
        /// <param name="resources">语言资源</param>
        private static void Loading(ToolStripMenuItem item, ComponentResourceManager resources)
        {
            if (item is ToolStripMenuItem tsmi)
            {
                resources.ApplyResources(item, item.Name);
                if (tsmi.DropDownItems.Count > 0)
                {
                    foreach (ToolStripMenuItem tsmiChild in tsmi.DropDownItems)
                    {
                        Loading(tsmiChild, resources);
                    }
                }
            }
        }
        #endregion 私有方法 end
    }
}