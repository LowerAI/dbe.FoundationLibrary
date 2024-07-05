using dbe.FoundationLibrary.Core.Common;
using dbe.FoundationLibrary.Core.Extensions;
using dbe.FoundationLibrary.Communication.RTUComm;
using dbe.FoundationLibrary.Core.Util;
using dbe.FoundationLibrary.Windows.UI.CustomForms;

using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace dbe.FoundationLibrary.Windows.UI.CommonDialogs
{
    public partial class FirmwareUpdateDlg : BaseWithBorderless
    {
        private readonly RTUCore rtu;
        #region        字段 Start
        #endregion 字段 End

        #region        属性 Start
        #endregion 属性 End

        #region        构造与析构 Start
        public FirmwareUpdateDlg(RTUCore rtu)
        {
            InitializeComponent();
            this.rtu = rtu;
        }
        #endregion 构造与析构 End

        #region        事件声明 start
        public delegate string GetFirmwareVersionEvent();
        public event GetFirmwareVersionEvent OnGetFirmwareVersion;
        //public event Func<Task<string>> OnGetFirmwareVersion;
        #endregion 事件声明 start

        #region        事件处理 Start
        private void FirmwareUpdateDlg_Load(object sender, EventArgs e)
        {
            //lbl_CurrentFirmware.Text = GetCurrentFirmwareVersion();
        }

        // 读取当前固件版本号
        private void lbl_ReloadSN_Click(object sender, EventArgs e)
        {
            lbl_CurrentFirmware.Text = GetCurrentFirmwareVersion();
        }

        // 双击加载新固件
        private void lbl_NewFirmware_DoubleClick(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.Filter = "固件文件(*.bin)|*.bin|所有文件|*.*";
            ofd.Title = "请选择固件文件";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                lbl_NewFirmware.Text = Path.GetFileName(ofd.FileName);
                //lbl_NewFirmware.Text = GetNewFirmwareVersion(ofd.FileName);
                byte[] fileData = File.ReadAllBytes(ofd.FileName);
                var crcStr = CheckSum.GetCRC(CrcType.CRC32_MPEG2, fileData, Endianness.ABCD).ToHexString();
                lbl_Crc32.Text = $"0x{crcStr.Replace(" ", "").ToLower()}";
                btn_Update.Enabled = true;
            }
        }

        // 更新固件
        private void btn_Update_Click(object sender, EventArgs e)
        {

        }
        #endregion 事件处理 End

        #region        公开方法 Start
        #endregion 公开方法 End

        #region        私有方法 Start
        /// <summary>
        /// 获取当前固件的版本号
        /// </summary>
        /// <returns></returns>
        private string GetCurrentFirmwareVersion()
        {
            if (rtu == null)
                return string.Empty;

            var currentVersion = OnGetFirmwareVersion?.Invoke();

            return currentVersion;
        }

        /// <summary>
        /// 获取新固件的版本号
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private string GetNewFirmwareVersion(string filePath)
        {
            // 获取文件版本信息
            var fileVersionInfo = FileVersionInfo.GetVersionInfo(filePath);
            // 读取产品版本号
            string productVersion = fileVersionInfo.ProductVersion;
            return productVersion;
        }
        #endregion 私有方法 End
    }
}