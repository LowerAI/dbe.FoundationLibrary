using dbe.FoundationLibrary.Core.Common;
using dbe.FoundationLibrary.Core.Util;
using dbe.FoundationLibrary.Windows.UI.CustomForms;

using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace dbe.FoundationLibrary.Windows.UI.CommonDialogs
{
    public partial class AboutDlg : BaseWithBorderless
    {
        #region    字段 start
        private bool isRegistered;// 是否已注册
        private readonly string asemblyName;// 程序集名
        private readonly string projectName;// 项目名
        private string version;// 版本号
        private readonly string compiledOn;// 编译时间
        private readonly string copyright;// 版权
        private readonly WinFormEnvironment deploymentEnvironment;// 发布环境
        #endregion 字段 end

        #region    构造与析构 start
        public AboutDlg(string asemblyName, string projectName, string version, string copyright, string compiledOn, WinFormEnvironment deploymentEnvironment = WinFormEnvironment.Production, bool isRegistered = false)
        {
            InitializeComponent();
            this.asemblyName = asemblyName;
            this.projectName = projectName;
            this.version = version;
            this.compiledOn = compiledOn;
            this.copyright = copyright;
            this.deploymentEnvironment = deploymentEnvironment;
            this.isRegistered = isRegistered;
        }

        public AboutDlg(AssemblyAttributeUtil aau, WinFormEnvironment deploymentEnvironment = WinFormEnvironment.Production, bool isRegistered = false) : this(aau.AssemblyProduct, aau.AssemblyTitle.TrimStart("xxView.".ToCharArray()), aau.AssemblyInformationalVersion, aau.AssemblyCopyright, aau.CompiledOn, deploymentEnvironment, isRegistered)
        {
        }
        #endregion 构造与析构 end

        #region    事件处理 start
        private void AboutVibeViewDlg_Load(object sender, System.EventArgs e)
        {
            //var aau = new AssemblyAttributeUtil();
            this.Text = string.Format(this.Text, asemblyName);
            if (deploymentEnvironment == WinFormEnvironment.Production)
            {
                var regFlag = isRegistered ? "已注册" : "未注册";
                this.Text = $"{this.Text}({regFlag})";
            }
            lbl_ProjectName.Text = projectName;
            //var regExp = new Regex(@"^(.+)\+", RegexOptions.IgnoreCase);
            //version = regExp.Match(version).Value.TrimEnd('+');
            lbl_Version.Text = $"v{version}({deploymentEnvironment})";
            lbl_CompiledOn.Text = $"{lbl_CompiledOn.Text}{compiledOn}";
            lbl_Copyright.Text = copyright;

            ToCenterChild(lbl_ProjectName);
            ToCenterChild(lbl_Version);
            ToCenterChild(lbl_CompiledOn);
            ToCenterChildToCompany(lnklbl_Website);
            ToCenterChildToCompany(lbl_Phone);
            ToCenterChild(lbl_Copyright);
        }

        // 打开公司链接
        private void lnklbl_Website_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            lnklbl_Website.LinkVisited = true;
            Process.Start(lnklbl_Website.Text);
        }

        // 打开公司邮箱
        private void lnklbl_Email_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                lnklbl_Email.LinkVisited = true;
                Process.Start($"mailto:{lnklbl_Email.Text}");
            }
            catch (Exception ex)
            {
                var msg = $"打开公司邮箱异常：{ex.Message.Replace("\r\n", "")}";
                Trace.WriteLine(msg);
            }
        }
        #endregion 事件处理 start

        #region    私有方法 start
        /// <summary>
        ///  将指定的子控件居中
        /// </summary>
        /// <param name="child"></param>
        private void ToCenterChild(Control child)
        {
            child.Left = (Width - child.Width) / 2;
        }

        /// <summary>
        /// 将指定的子控件以公司名控件为基准居中
        /// </summary>
        /// <param name="child">指定的子控件</param>
        private void ToCenterChildToCompany(Control child)
        {
            var childRight = lbl_Company.Left + child.Width;
            var companyRight = lbl_Company.Right;
            child.Left = lbl_Company.Left - (childRight - companyRight) / 2 + 1;
        }
        #endregion 私有方法 end
    }
}