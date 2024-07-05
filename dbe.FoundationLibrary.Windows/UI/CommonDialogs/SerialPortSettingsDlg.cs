using dbe.FoundationLibrary.Communication.RTUComm;
using dbe.FoundationLibrary.Windows.Mvvm;
using dbe.FoundationLibrary.Windows.UI.CustomForms;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Management;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace dbe.FoundationLibrary.Windows.UI.CommonDialogs
{
    public partial class SerialPortSettingsDlg : BaseWithBorderless
    {
        #region    字段 start
        private SerialPort serialPort;
        #endregion 字段 end

        #region    属性 start
        /// <summary>
        /// 开关串口
        /// </summary>
        public Func<bool, Task<bool>> SwitchPort { set; get; }

        /// <summary>
        /// 保存设置
        /// </summary>
        public Action<string, int> SaveSettings { set; get; }
        #endregion 属性 end

        #region    委托声明 start
        //private Func<bool, bool> SwitchPort;
        #endregion 委托声明 end

        #region    构造与析构 start
        /// <summary>
        /// 串口设置构造
        /// </summary>
        /// <param name="serialPort">全局唯一的串口对象</param>
        /// <param name="switchPort">开关串口的函数</param>
        //public SerialPortSettingsDlg(SerialPort serialPort, Func<bool, Task<bool>> switchPort)
        //{
        //    CtorBase(serialPort, switchPort);
        //}
        public SerialPortSettingsDlg(SerialPort serialPort, Func<bool, Task<bool>> switchPort)
        {
            this.serialPort = serialPort;
            this.SwitchPort = switchPort;
            UpdateStatus(serialPort.IsOpen);
        }

        //public SerialPortSettingsDlg(RTUCore rtu)
        //{
        //    var uart = rtu as SerialPortService;
        //    CtorBase(uart.SerialPort, uart.SwitchPortAsync);
        //}
        public SerialPortSettingsDlg(RTUCore rtu) : this((rtu as SerialPortService).SerialPort, (rtu as SerialPortService).SwitchPortAsync)
        {
        }
        #endregion 构造与析构 end

        #region    事件处理 start
        // 设置对话框加载时
        private void SerialPortSettingsDlg_Load(object sender, EventArgs e)
        {
            InitSource();
            cbb_Ports.TwoWayBinding("SelectedValue", serialPort, "PortName");
            cbb_BaudRates.TwoWayBinding("SelectedValue", serialPort, "BaudRate", true);
        }

        // 打开/关闭串口
        private void tglbtn_Switch_Click(object sender, EventArgs e)
        {
            try
            {
                tglbtn_Switch.Enabled = false;
                SwitchPort?.Invoke(!serialPort.IsOpen);
                UpdateStatus(serialPort.IsOpen);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                tglbtn_Switch.Enabled = true;
                btn_Refresh.Enabled = !tglbtn_Switch.Checked;
            }
        }

        // 刷新串口列表
        private void btn_Refresh_Click(object sender, EventArgs e)
        {
            cbb_Ports.DataSource = GetComList();
            cbb_Ports.DisplayMember = "Value";
            cbb_Ports.ValueMember = "Key";
        }

        // 关闭时保存当前串口和波特率
        private void SerialPortSettingsDlg_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveSettings?.Invoke(serialPort.PortName, serialPort.BaudRate);
        }
        #endregion 事件处理 end

        #region    私有方法 start
        ///// <summary>
        ///// 构造函数公用方法
        ///// </summary>
        ///// <param name="serialPort"></param>
        ///// <param name="switchPort"></param>
        //private void CtorBase(SerialPort serialPort, Func<bool, Task<bool>> switchPort)
        //{
        //    InitializeComponent();
        //    this.serialPort = serialPort;
        //    this.SwitchPort = switchPort;
        //    UpdateStatus(serialPort.IsOpen);
        //}

        /// <summary>
        /// 初始化MainForm中用到的数据源
        /// </summary>
        private void InitSource()
        {
            btn_Refresh.PerformClick();
            btn_Refresh.Enabled = !tglbtn_Switch.Checked;
            List<KeyValuePair<string, string>> comPairs = cbb_Ports.DataSource as List<KeyValuePair<string, string>>;
            if (comPairs.Exists(kvp => kvp.Key == serialPort.PortName))
            {
                cbb_Ports.SelectedValue = serialPort.PortName;
            }

            var baudRates = new int[] {
                110,300,600,1200,2400,4800,9600,14400,19200,38400,43000,57600,76800,115200,128000,230400,25600,460800,921600,1000000,2000000,3000000
            };
            var baudRateLst = new List<KeyValuePair<int, int>>();
            foreach (var item in baudRates)
            {
                baudRateLst.Add(new KeyValuePair<int, int>(item, item));
            }
            cbb_BaudRates.DataSource = baudRateLst;
            cbb_BaudRates.DisplayMember = "Key";
            cbb_BaudRates.ValueMember = "Value";
            //int specifiedBaudRate = 230400;
            int specifiedBaudRate = this.serialPort.BaudRate;
            if (baudRates.Contains(specifiedBaudRate))
            {
                cbb_BaudRates.SelectedValue = specifiedBaudRate;
            }
        }

        /// <summary>
        /// 更新UI状态
        /// </summary>
        /// <param name="isOpened">true表示已打开/false表示已关闭或未打开</param>
        private void UpdateStatus(bool isOpened)
        {
            tglbtn_Switch.Checked = isOpened;
            if (isOpened)
            {
                tglbtn_Switch.Text = "关";
                cbb_Ports.Enabled = false;
                cbb_BaudRates.Enabled = false;
            }
            else
            {
                tglbtn_Switch.Text = "开";
                cbb_Ports.Enabled = true;
                cbb_BaudRates.Enabled = true;
            }
        }
        #endregion 私有方法 start

        #region    公开方法 start
        /// <summary>
        /// 获取所有的串口设备描述和端口号
        /// </summary>
        /// <returns></returns>
        public List<KeyValuePair<string, string>> GetComList()
        {
            List<KeyValuePair<string, string>> comPairs = new List<KeyValuePair<string, string>>();
            try
            {
                Thread thread = new Thread(() =>
                {// 此处的操作必须放在单独的线程中否则会发生InvalidCastException
                    using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * from Win32_PnPEntity"))
                    {
                        Regex regExp = new Regex("COM\\d+");
                        ManagementObjectCollection deviceInfos = searcher.Get();
                        foreach (ManagementObject deviceInfo in deviceInfos)
                        {
                            var nameVal = deviceInfo.Properties["Name"].Value;
                            var name = nameVal == null ? string.Empty : nameVal.ToString();
                            if (regExp.IsMatch(name))
                            {
                                string portName = regExp.Match(name).Value;
                                comPairs.Add(new KeyValuePair<string, string>(portName, name));
                            }
                        }
                        searcher.Dispose();
                    }
                });
                thread.Start();
                thread.Join();
            }
            catch (Exception ex)
            {
                if (ex is InvalidCastException ice)
                {
                    Trace.WriteLine($"ice.Message = {ice.Message}");
                }
            }
            return comPairs;
        }
        #endregion 公开方法 start
    }
}