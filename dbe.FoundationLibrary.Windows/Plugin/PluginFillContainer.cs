using System;
using System.Reflection;
using System.Windows.Forms;

namespace GNDView.Library.Plugin
{
    /// <summary>
    /// �������� 
    /// </summary>
    public class PluginFillContainer
    {
        /// <summary>
        /// AddForm����ö��
        /// </summary>
        public enum PluginAddFormPara
        {
            /// <summary>
            /// AddForm��չ��ģʽShowDialog
            /// </summary>
            ShowDialog,
            /// <summary>
            /// AddForm��չ��ģʽShow
            /// </summary>
            Show,
            /// <summary>
            /// ��ͨ��Show
            /// </summary>
            JustShow
        }

        #region        ���������� start
        public PluginFillContainer(Form form)
        {
            this.Container = form;
        }
        #endregion ���������� end

        #region        ���� start
        /// <summary>
        /// ��������
        /// </summary>
        public Form Container { get; set; }
        #endregion ���� end

        #region        �������� start
        /// <summary>
        /// ���TabPage�ķ�������,���ҳ�治����,�ʹ�����ҳ��,����ѡ���ҳ��
        /// </summary>
        /// <typeparam name="TForm">ҵ������</typeparam>
        public TForm AddTabPage<TForm>() where TForm : Form, new()
        {
            return this.AddTabPage<TForm>(null, null, PluginAddFormPara.Show);
        }

        /// <summary>
        /// ���TabPage�ķ�������,���ҳ�治����,�ʹ�����ҳ��,����ѡ���ҳ��
        /// </summary>
        /// <typeparam name="TForm">ҵ������</typeparam>
        /// <param name="showpara">�������ʽ</param>
        public TForm AddTabPage<TForm>(PluginAddFormPara showpara) where TForm : Form, new()
        {
            return this.AddTabPage<TForm>(null, null, showpara);
        }

        /// <summary>
        /// ���TabPage�ķ�������,���ҳ�治����,�ʹ�����ҳ��,����ѡ���ҳ��
        /// </summary>
        /// <typeparam name="TForm">ҵ������</typeparam>
        /// <param name="args">�����б�</param>
        public TForm AddTabPage<TForm>(object[] args) where TForm : Form, new()
        {
            return this.AddTabPage<TForm>(args, null, PluginAddFormPara.Show);
        }

        /// <summary>
        /// ���TabPage�ķ�������,���ҳ�治����,�ʹ�����ҳ��,����ѡ���ҳ��
        /// </summary>
        /// <typeparam name="TForm">ҵ������</typeparam>
        /// <param name="args">�����б�</param>
        /// <param name="sender">��ҵ��Ķ���</param>
        /// <param name="showpara">�������ʽ</param>
        public TForm AddTabPage<TForm>(object[] args, Control sender, PluginAddFormPara showpara) where TForm : Form, new()
        {
            #region �ϴ���
            ////�ж��Ƿ��Ѿ��򿪴�ҳ��
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

            ////���δ�򿪾ʹ�����ʵ��

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
            //        IntPtr handle = form.Handle;   //�ô����Զ�ӵ�о��,һ�����show����    
            //        form.Show();
            //    }
            //    form.Update();

            //    //��¼��ǰ��ť���µ�ҵ��
            //    if (sender != null)
            //    {
            //        sender.Tag = form;
            //    }
            //}

            //return form;
            #endregion �ϴ���

            return AddTabPage(typeof(TForm), args, sender, showpara) as TForm;
        }

        /// <summary>
        /// ���TabPage�ķ�������,���ҳ�治����,�ʹ�����ҳ��,����ѡ���ҳ��
        /// </summary>
        /// <param name="type">��������</param>
        /// <returns></returns>
        public Form AddTabPage(Type type)
        {
            return AddTabPage(type, null, null, PluginAddFormPara.Show);
        }

        /// <summary>
        /// ���TabPage�ķ�������,���ҳ�治����,�ʹ�����ҳ��,����ѡ���ҳ��
        /// </summary>
        /// <param name="type">��������</param>
        /// <param name="showpara">�������ʽ</param>
        /// <returns></returns>
        public Form AddTabPage(Type type, PluginAddFormPara showpara)
        {
            return AddTabPage(type, null, null, showpara);
        }

        /// <summary>
        ///  ���TabPage�ķ�������,���ҳ�治����,�ʹ�����ҳ��,����ѡ���ҳ��
        /// </summary>
        /// <param name="type">��������</param>
        /// <param name="args">����ʵ���������б�</param>
        /// <param name="sender">��ҵ��Ķ���</param>
        /// <param name="showpara">�������ʽ</param>
        /// <returns></returns>
        //public Form AddTabPage(Type type, object[] args, BaseItem sender, PluginAddFormPara showpara)
        public Form AddTabPage(Type type, object[] args, Control sender, PluginAddFormPara showpara)
        {
            if (showpara == PluginAddFormPara.Show)
            {
                //��С����������
                foreach (Form f in this.Container.OwnedForms)
                {
                    f.Hide();
                }

                //�ж��Ƿ��Ѿ��򿪴�ҳ��
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
                //�ж��Ƿ��Ѿ��򿪴�ҳ��
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

            //���δ�򿪾ʹ�����ʵ��
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
                    IntPtr handle = form.Handle;   //�ô����Զ�ӵ�о��,һ�����show����    
                    form.Show();

                }
                finally
                {
                    form.Select();
                    form.Update();
                }
                //��¼��ǰ��ť���µ�ҵ��
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
        #endregion �������� end
    }
}