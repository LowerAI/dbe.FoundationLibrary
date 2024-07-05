using dbe.FoundationLibrary.Core.Dencrypt;
using dbe.FoundationLibrary.Core.Util;

using System;
using System.Windows.Forms;

namespace dbe.FoundationLibrary.Windows.UI.CommonDialogs
{
    /// <summary>
    /// 软件注册对话框
    /// </summary>
    public partial class SoftwareRegistrationDlg : Form
    {
        #region    字段 start
        /// <summary>
        /// 程序集名称
        /// </summary>
        private readonly string assemblyName;
        /// <summary>
        /// 程序集产品名
        /// </summary>
        private readonly string assemblyProduct;
        /// <summary>
        /// 产品版本号
        /// </summary>
        private readonly string productVersion;
        #endregion 字段 end

        #region    属性 start
        /// <summary>
        /// 机器码
        /// </summary>
        public string MachineCode
        {
            get
            {
                string str = $"jiCpuID:{HardwareInfo.CPUId}jiadianzi{assemblyProduct}";//由于插入U盘也会影响DiskID，所以计算机器码的时候不加入DiskID了
                var machineCode = MD5Helper.Encrypt16Bit(str);//计算机器码
                return machineCode;
            }
        }

        /// <summary>
        /// 注册码
        /// </summary>
        public string RegisterCode
        {
            get
            {
                var pwsStr = $"GND{MachineCode}{productVersion}";
                var registerCode = MD5Helper.Encrypt16Bit(pwsStr);//计算注册码
                return registerCode;
            }
        }
        #endregion 属性 end

        #region    构造函数 start
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="assemblyName">程序集名称</param>
        /// <param name="assemblyProduct">所属产品名</param>
        /// <param name="productVersion">产品版本号</param>
        /// <exception cref="ArgumentException"></exception>
        public SoftwareRegistrationDlg(string assemblyName, string assemblyProduct, string productVersion)
        {
            InitializeComponent();

            if (string.IsNullOrWhiteSpace(assemblyName))
            {
                throw new ArgumentException($"“{nameof(assemblyName)}”不能为 null 或空白。", nameof(assemblyName));
            }

            if (string.IsNullOrWhiteSpace(productVersion))
            {
                throw new ArgumentException($"“{nameof(productVersion)}”不能为 null 或空白。", nameof(productVersion));
            }

            this.assemblyName = assemblyName;
            this.assemblyProduct = assemblyProduct;
            this.productVersion = productVersion;
        }
        #endregion 构造函数 end

        #region    事件处理 start
        private void SoftwareRegistrationDlg_Load(object sender, EventArgs e)
        {
            this.Text = $"{Text}{assemblyName} {productVersion}";
            txt_MachineCode.Text = MachineCode;
            txt_RegisterCode.Focus();
        }

        private void Txt_RegisterCode_TextChanged(object sender, EventArgs e)
        {
            if (sender is TextBox)
            {
                TextBox textBox = (TextBox)sender;
                textBox.Text = textBox.Text.ToUpper();// 输入注册码小写自动变大写
                textBox.SelectionStart = textBox.Text.Length; // 将光标移动到文本末尾
            }
        }

        private void btn_OK_Click(object sender, EventArgs e)
        {
            if (RegisterCode.Trim().ToUpper() == txt_RegisterCode.Text.Trim().ToUpper())
            {
                MessageBox.Show("软件注册成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("注册码不正确，请重试！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
        #endregion 事件处理 end

        #region    私有方法 start
        #endregion 私有方法 start
    }
}