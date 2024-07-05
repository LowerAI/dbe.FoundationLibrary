using System;
using System.Reflection;
using System.Windows.Forms;

namespace GNDView.Library.Plugin
{
    /// <summary>
    /// 容器窗体 
    /// </summary>
    public class PluginFillContainer
    {
        /// <summary>
        /// AddForm参数枚举
        /// </summary>
        public enum PluginAddFormPara
        {
            /// <summary>
            /// AddForm的展现模式ShowDialog
            /// </summary>
            ShowDialog,
            /// <summary>
            /// AddForm的展现模式Show
            /// </summary>
            Show,
            /// <summary>
            /// 普通的Show
            /// </summary>
            JustShow
        }

        #region        构造与析构 start
        public PluginFillContainer(Form form)
        {
            this.Container = form;
        }
        #endregion 构造与析构 end

        #region        属性 start
        /// <summary>
        /// 容器窗体
        /// </summary>
        public Form Container { get; set; }
        #endregion 属性 end

        #region        公开方法 start
        /// <summary>
        /// 添加TabPage的泛型重载,如此页面不存在,就创建新页面,否则选择该页面
        /// </summary>
        /// <typeparam name="TForm">业务类型</typeparam>
        public TForm AddTabPage<TForm>() where TForm : Form, new()
        {
            return this.AddTabPage<TForm>(null, null, PluginAddFormPara.Show);
        }

        /// <summary>
        /// 添加TabPage的泛型重载,如此页面不存在,就创建新页面,否则选择该页面
        /// </summary>
        /// <typeparam name="TForm">业务类型</typeparam>
        /// <param name="showpara">窗体打开样式</param>
        public TForm AddTabPage<TForm>(PluginAddFormPara showpara) where TForm : Form, new()
        {
            return this.AddTabPage<TForm>(null, null, showpara);
        }

        /// <summary>
        /// 添加TabPage的泛型重载,如此页面不存在,就创建新页面,否则选择该页面
        /// </summary>
        /// <typeparam name="TForm">业务类型</typeparam>
        /// <param name="args">参数列表</param>
        public TForm AddTabPage<TForm>(object[] args) where TForm : Form, new()
        {
            return this.AddTabPage<TForm>(args, null, PluginAddFormPara.Show);
        }

        /// <summary>
        /// 添加TabPage的泛型重载,如此页面不存在,就创建新页面,否则选择该页面
        /// </summary>
        /// <typeparam name="TForm">业务类型</typeparam>
        /// <param name="args">参数列表</param>
        /// <param name="sender">打开业务的对象</param>
        /// <param name="showpara">窗体打开样式</param>
        public TForm AddTabPage<TForm>(object[] args, Control sender, PluginAddFormPara showpara) where TForm : Form, new()
        {
            #region 老代码
            ////判断是否已经打开此页面
            //foreach (Form f in this.Container.MdiChildren)
            //{
            //    if (f.GetType().Equals(typeof(TForm)))
            //    {
            //        if (sender != null)
            //        {
            //            if (sender.Tag.Equals(f))
            //            {
            //                f.Select();
            //                f.Update();
            //                return f as TForm;
            //            }
            //        }
            //        else
            //        {
            //            f.Select();
            //            f.Update();
            //            return f as TForm;
            //        }
            //    }
            //}

            ////如果未打开就创建新实例

            //TForm form = typeof(TForm).InvokeMember("new", BindingFlags.CreateInstance, null, null, args) as TForm;

            //if (para == PluginAddFormPara.ShowDialog)
            //{
            //    form.ShowDialog();
            //}
            //else
            //{

            //    form.WindowState = FormWindowState.Maximized;
            //    form.MdiParent = this.Container;

            //    try
            //    {
            //        form.Show();
            //    }
            //    catch
            //    {
            //        IntPtr handle = form.Handle;   //让窗体自动拥有句柄,一遍可以show出来    
            //        form.Show();
            //    }
            //    form.Update();

            //    //记录当前按钮按下的业务
            //    if (sender != null)
            //    {
            //        sender.Tag = form;
            //    }
            //}

            //return form;
            #endregion 老代码

            return AddTabPage(typeof(TForm), args, sender, showpara) as TForm;
        }

        /// <summary>
        /// 添加TabPage的泛型重载,如此页面不存在,就创建新页面,否则选择该页面
        /// </summary>
        /// <param name="type">窗体类型</param>
        /// <returns></returns>
        public Form AddTabPage(Type type)
        {
            return AddTabPage(type, null, null, PluginAddFormPara.Show);
        }

        /// <summary>
        /// 添加TabPage的泛型重载,如此页面不存在,就创建新页面,否则选择该页面
        /// </summary>
        /// <param name="type">窗体类型</param>
        /// <param name="showpara">窗体打开样式</param>
        /// <returns></returns>
        public Form AddTabPage(Type type, PluginAddFormPara showpara)
        {
            return AddTabPage(type, null, null, showpara);
        }

        /// <summary>
        ///  添加TabPage的泛型重载,如此页面不存在,就创建新页面,否则选择该页面
        /// </summary>
        /// <param name="type">窗体类型</param>
        /// <param name="args">类型实例化参数列表</param>
        /// <param name="sender">打开业务的对象</param>
        /// <param name="showpara">窗体打开样式</param>
        /// <returns></returns>
        //public Form AddTabPage(Type type, object[] args, BaseItem sender, PluginAddFormPara showpara)
        public Form AddTabPage(Type type, object[] args, Control sender, PluginAddFormPara showpara)
        {
            if (showpara == PluginAddFormPara.Show)
            {
                //最小化悬浮窗口
                foreach (Form f in this.Container.OwnedForms)
                {
                    f.Hide();
                }

                //判断是否已经打开此页面
                foreach (Form f in this.Container.MdiChildren)
                {
                    if (f.GetType().Equals(type))
                    {
                        if (sender != null)
                        {
                            if (sender.Tag.Equals(f))
                            {
                                f.Select();
                                f.Update();
                                return f as Form;
                            }
                        }
                        else
                        {
                            f.Select();
                            f.Update();
                            return f as Form;
                        }
                    }
                }
            }
            else if (showpara == PluginAddFormPara.JustShow)
            {
                //判断是否已经打开此页面
                foreach (Form f in this.Container.OwnedForms)
                {
                    if (f.GetType().Equals(type))
                    {
                        //f.Select();
                        //f.Update();
                        //f.ShowInTaskbar = false;                 
                        //f.StartPosition = FormStartPosition.CenterScreen;
                        f.Show();
                        return f as Form;
                    }
                }
            }

            //如果未打开就创建新实例
            Form form = type.InvokeMember("new", BindingFlags.CreateInstance, null, null, args) as Form;

            if (showpara == PluginAddFormPara.ShowDialog)
            {
                form.StartPosition = FormStartPosition.CenterScreen;
                form.ShowInTaskbar = false;
                form.ShowDialog();
            }
            else if (showpara == PluginAddFormPara.Show)
            {

                form.WindowState = FormWindowState.Maximized;
                form.MdiParent = this.Container;

                try
                {
                    form.Show();
                }
                catch
                {
                    IntPtr handle = form.Handle;   //让窗体自动拥有句柄,一遍可以show出来    
                    form.Show();

                }
                finally
                {
                    form.Select();
                    form.Update();
                }
                //记录当前按钮按下的业务
                if (sender != null)
                {
                    sender.Tag = form;
                }
            }
            else if (showpara == PluginAddFormPara.JustShow)
            {
                form.ShowInTaskbar = false;
                form.StartPosition = FormStartPosition.CenterScreen;
                form.WindowState = FormWindowState.Normal;
                form.Show(this.Container);
            }
            return form;
        }
        #endregion 公开方法 end
    }
}