using dbe.FoundationLibrary.Communication.RTUComm;
using dbe.FoundationLibrary.Core.Util;
using dbe.FoundationLibrary.Windows.UI.CustomForms;
using dbe.FoundationLibrary.Windows.Mvvm;
//using InTheHand.Net.Bluetooth;

using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace dbe.FoundationLibrary.Windows.UI.CommonDialogs
{
    /// <summary>
    /// 主机控制接口设置
    /// </summary>
    public partial class HCISettingsDlg1 : BaseWithBorderless
    {
        #region    字段 start
        private LoggerUtil logger = LoggerUtil.Instance;
        private SerialPortService uart;

        //private BleCore ble;
        private string _serviceGuid = "6e400001-b5a3-f393-e0a9-e50e24dcca9e";
        private string _writeCharacteristicGuid = "6e400002-b5a3-f393-e0a9-e50e24dcca9e";
        private string _notifyCharacteristicGuid = "6e400003-b5a3-f393-e0a9-e50e24dcca9e";

        private List<string> snLst;
        private CancellationTokenSource cts;// 取消线程的控制器
        private ManualResetEvent pauseHandleData = new ManualResetEvent(true);
        private bool isScanning = false;// 是否正在扫描
        private bool isCancell = false;// 是否取消扫描
        private string cnSN4Dgv = nameof(dgvc_SlaveSN);// 设备SN列的列名
        private string cnFrom4Dgv = nameof(dgvc_SlaveFrom);// 来源列的列名
        private string cnDel4Dgv = nameof(dgvc_BtnDelete);// 删除列的列名
        private string cnSN4DS = "SN";// 数据源SN列的列名
        private string cnDef4DS = "Default";// 数据源Default列的列名
        #endregion 字段 end

        #region    属性 start
        /// <summary>
        /// 串口
        /// </summary>
        public SerialPort SerialPort { get => uart.SerialPort; }

        /// <summary>
        /// 串口列表
        /// </summary>
        public List<KeyValuePair<string, string>> ComLst { get => RTUDefault.GetComList(); }

        /// <summary>
        /// 开关串口
        /// </summary>
        public Func<bool, Task<bool>> SwitchPort { set; get; }

        /// <summary>
        /// 保存设置
        /// </summary>
        public Action<string, int> SaveSettings { set; get; }

        /// <summary>
        /// 在开始扫描前
        /// </summary>
        public Action OnBeforeStartScan { set; get; }

        /// <summary>
        /// 在停止扫描后
        /// </summary>
        public Action OnAfterStopScan { set; get; }

        /// <summary>
        /// 改变了连接的从机后
        /// </summary>
        public Action<string> OnChangedConnect { set; get; }
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
        public HCISettingsDlg1(SerialPort serialPort, Func<bool, Task<bool>> switchPort)
        {
            CtorBase(serialPort, switchPort);
        }

        public HCISettingsDlg1(RTUCore rtu)
        {
            uart = rtu as SerialPortService;
            uart.OnDeviceChanged += Uart_OnDeviceChanged;
            CtorBase(uart.SerialPort, uart.SwitchPortAsync);
        }
        #endregion 构造与析构 end

        #region    事件处理 start
        // 设置对话框加载时
        private void SerialPortSettingsDlg_Load(object sender, EventArgs e)
        {
            InitSource();

            cbb_Ports.TwoWayBinding("SelectedValue", SerialPort, "PortName");
            cbb_BaudRates.TwoWayBinding("SelectedValue", SerialPort, "BaudRate", true);

            pauseHandleData.Reset();// 暂停解析SN的线程
            StartHandleSerialData();// 启动本类中的串口数据解析线程
        }

        // UART设备插拔事件
        private void Uart_OnDeviceChanged(object sender, DeviceChangedEventArgs e)
        {
            if (this.InvokeRequired)
            {
                while (!this.IsHandleCreated)
                {
                    //解决窗体关闭时出现“访问已释放句柄“的异常
                    if (this.Disposing || this.IsDisposed)
                        return;
                }
                var res = this.BeginInvoke(new Action<object, DeviceChangedEventArgs>(Uart_OnDeviceChanged), sender, e);
                this.EndInvoke(res);
            }
            else
            {
                if (e.IsConnect)
                {// 插入设备
                    UpdateComStatus("", SystemColors.Control);
                    btn_Refresh_Click(null, EventArgs.Empty);
                    if (e.DeviceNotChanged && ComLst.Exists(kvp => kvp.Key == uart.LastPortName))
                    {
                        cbb_Ports.SelectedValue = uart.LastPortName;
                        ClickToSwitch(e.IsConnect);
                    }
                }
                else
                {// 拔出设备
                    ClickToSwitch(e.IsConnect);
                    btn_Refresh_Click(null, EventArgs.Empty);
                }
            }
        }

        // 切换主机类型
        private void radbtn_BluetoothModule_CheckedChanged(object sender, EventArgs e)
        {
            RowStyle rs0 = tlp_Host.RowStyles[0];
            RowStyle rs1 = tlp_Host.RowStyles[1];
            var isShow = radbtn_UARTBluetoothModule.Checked;
            if (isShow)
            {
                rs0.Height = 100;
                rs1.Height = 0;
            }
            else
            {
                rs0.Height = 0;
                rs1.Height = 100;
            }
        }

        // 打开/关闭串口
        private void tglbtn_Switch_Click(object sender, EventArgs e)
        {
            if (cbb_Ports.SelectedValue == null)
            {
                UpdateComStatus("请先选定端口", Color.Red);
                tglbtn_Switch.Checked = false;
                return;
            }
            ClickToSwitch(!SerialPort.IsOpen);
        }

        // 刷新串口列表
        private void btn_Refresh_Click(object sender, EventArgs e)
        {
            cbb_Ports.DataSource = ComLst;
            cbb_Ports.Refresh();
        }

        // 扫描蓝牙从机
        private async void btn_Scan_Click(object sender, EventArgs e)
        {
            try
            {
                btn_Scan.Enabled = false;
                //BluetoothUtils.PrintAllDevices();
                if (isScanning)
                {// 暂停扫描
                    btn_Scan.Text = "扫描";
                    pauseHandleData.Reset();// 暂停解析SN的线程
                    UpdateScanStatus("扫描已停止", Color.Green);
                    isScanning = false;
                    OnAfterStopScan?.Invoke();
                }
                else
                {// 开始扫描
                    (bool success, string message) = await uart.StartScanAsync();
                    if (success)
                    {// 扫描指令发送成功
                        btn_Scan.Text = "停止";
                        isScanning = true;
                        OnBeforeStartScan?.Invoke();// 开始扫描前取消MFVM中的串口数据解析线程
                        await uart.SwitchRecvBtCfgDataAsync();// uart切换到接收蓝牙配置数据
                        pauseHandleData.Set();// 重启解析SN的线程

                        snLst.Clear();// 清空内存中的从机列表
                        RemoveAllNotNewRow();
                        OnChangedConnect?.Invoke("");// 清空主界面中的SN
                        UpdateScanStatus("扫描中...", Color.Red);
                    }
                    else
                    {// 扫描指令发送失败
                        UpdateScanStatus(message, Color.Red);
                    }
                }
            }
            catch (Exception ex)
            {
                var msg = ex.Message.Replace("\r\n", "");
                UpdateScanStatus(msg, Color.Red);
                System.Diagnostics.Trace.WriteLine(msg);
                logger.Fatal(msg, ex);
            }
            finally
            {
                btn_Scan.Enabled = true;
            }
        }

        // 设备SN列表 => 添加行之后新行被选中
        private void dgv_SlaveDevices_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            var dgvr = dgv_SlaveDevices.Rows[e.RowIndex == 0 ? e.RowCount : e.RowIndex];
            if (dgvr.IsNewRow)
            {
                dgvr.Selected = true;
            }
        }

        // 设备SN列表 => 点击删除按钮删除当前行
        private void dgv_SlaveDevices_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var curRow = dgv_SlaveDevices.Rows[e.RowIndex];
            if (curRow.HeaderCell.Style.BackColor == Color.LightSkyBlue)
            {
                return;
            }
            if (dgv_SlaveDevices.Columns[e.ColumnIndex].Name == cnDel4Dgv && !curRow.IsNewRow)
            {
                dgv_SlaveDevices.Rows.RemoveAt(e.RowIndex);
            }
        }

        // 设备SN列表 => 结束编辑时改变来源样式
        private void dgv_SlaveDevices_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            dgv_SlaveDevices.Rows[e.RowIndex].Cells[cnFrom4Dgv].Value = "手动添加";
            dgv_SlaveDevices.Rows[e.RowIndex].Cells[cnFrom4Dgv].Style.ForeColor = Color.BlueViolet;
        }

        // 设备SN列表 => 开始编辑SN时初始化其余两列的文本
        private void dgv_SlaveDevices_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            var dgvr = dgv_SlaveDevices.Rows[e.RowIndex];
            dgvr.HeaderCell.Value = "✦";
            dgvr.HeaderCell.ToolTipText = "双击切换至该设备";
            //dgvr.Cells[cnFrom4Dgv].Value = "◉";
            dgvr.Cells[cnDel4Dgv].Value = "✖";
            //if (dgvr.IsNewRow)
            //{
            //    //dgvr.HeaderCell.Selected = true;
            //    dgvr.Selected = true;
            //}
        }

        // 设备SN列表 => 双击当前行的行头切换到对应的从机
        private async void dgv_SlaveDevices_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                dgv_SlaveDevices.EndEdit();
                var snVal = dgv_SlaveDevices.Rows[e.RowIndex].Cells[cnSN4Dgv].Value;
                if (snVal == null) return;
                var sn = snVal.ToString();
                if (string.IsNullOrWhiteSpace(sn)) return;
                (bool success, string message) = await uart.ConnectAsync(sn);
                if (success)
                {
                    pauseHandleData.Reset();
                    btn_Scan.Text = "扫描";
                    isScanning = false;
                    UpdateScanStatus($"已连接到{sn}", Color.Green);
                    //cts.Cancel();
                    await uart.SwitchRecvNormalDataAsync();
                    ChangeConnectedRow(e.RowIndex);
                    OnChangedConnect?.Invoke(sn);
                }
                else
                {
                    //MessageBox.Show(message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    UpdateScanStatus(message, Color.Red);
                    logger.Error(message);
                }
            }
            catch (Exception ex)
            {
                var msg = $"切换传感器异常：{ex.Message.Replace("\r\n", "")}";
                //MessageBox.Show(msg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                UpdateScanStatus(msg, Color.Red);
                logger.Fatal(msg, ex);
                //UpdateMessage(Color.Red, msg);
            }
        }

        // 关闭时保存当前串口和波特率
        private void SerialPortSettingsDlg_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (pauseHandleData.Reset())
            {// 取消接收SN的线程
                isCancell = true;
                cts.Cancel();
            }

            SaveSettings?.Invoke(SerialPort.PortName, SerialPort.BaudRate);
        }
        #endregion 事件处理 end

        #region    私有方法 start
        /// <summary>
        /// 构造函数公用方法
        /// </summary>
        /// <param name="serialPort"></param>
        /// <param name="switchPort"></param>
        private void CtorBase(SerialPort serialPort, Func<bool, Task<bool>> switchPort)
        {
            InitializeComponent();
            cbb_Ports.DisplayMember = "Value";
            cbb_Ports.ValueMember = "Key";

            //ble = new BleCore(_serviceGuid, _writeCharacteristicGuid, _notifyCharacteristicGuid);
            ////ble.SelectDeviceAsync(string.Empty).GetAwaiter().GetResult();
            //ble.UpdateDevcice += UpdateDevcice;
            //ble.SelectDeviceFromIdAsync(string.Empty).GetAwaiter().GetResult();
            //ble.StartBleDeviceWatcher2();

            this.SwitchPort = switchPort;
            UpdateUIStatus(serialPort.IsOpen);
        }

        private void UpdateDevcice(string bleDevice)
        {
            Trace.WriteLine($"DeviceAddress:{bleDevice}");
        }

        /// <summary>
        /// 初始化MainForm中用到的数据源
        /// </summary>
        private void InitSource()
        {
            snLst = new List<string>();
            cts = new CancellationTokenSource();

            btn_Refresh_Click(null, EventArgs.Empty);

            //List<KeyValuePair<string, string>> comPairs = cbb_Ports.DataSource as List<KeyValuePair<string, string>>;
            if (ComLst.Exists(kvp => kvp.Key == SerialPort.PortName))
            {
                cbb_Ports.SelectedValue = SerialPort.PortName;
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
            int specifiedBaudRate = SerialPort.BaudRate;
            if (baudRates.Contains(specifiedBaudRate))
            {
                cbb_BaudRates.SelectedValue = specifiedBaudRate;
            }
        }

        /// <summary>
        /// 单击开关
        /// </summary>
        /// <param name="openOrClose">true表示打开/false表示关闭</param>
        private void ClickToSwitch(bool openOrClose)
        {
            try
            {
                tglbtn_Switch.Enabled = false;
                SwitchPort?.Invoke(openOrClose);
                UpdateUIStatus(openOrClose);
            }
            catch (Exception ex)
            {
                var msg = ex.Message.Replace("\r\n", "");
                UpdateComStatus(msg, Color.Red);
                logger.Fatal(msg, ex);
            }
            finally
            {
                tglbtn_Switch.Enabled = true;
                btn_Refresh.Enabled = !tglbtn_Switch.Checked;
            }
        }

        /// <summary>
        /// 更新UI状态
        /// </summary>
        /// <param name="isOpened">true表示已打开/false表示已关闭或未打开</param>
        private void UpdateUIStatus(bool isOpened)
        {
            cbb_Ports.Enabled = !isOpened;
            btn_Refresh.Enabled = !isOpened;
            tglbtn_Switch.Checked = isOpened;
            tglbtn_Switch.Text = isOpened ? "关" : "开";
            cbb_BaudRates.Enabled = !isOpened;
            dgv_SlaveDevices.Enabled = isOpened;
            btn_Scan.Enabled = isOpened;
        }

        /// <summary>
        /// 移除所有非NewRow数据
        /// </summary>
        private void RemoveAllNotNewRow()
        {
            for (int i = dgv_SlaveDevices.Rows.Count - 2; i >= 0; i--)
            {
                var row = dgv_SlaveDevices.Rows[i];
                if (!row.IsNewRow)
                {
                    dgv_SlaveDevices.Rows.Remove(row);
                }
            }
        }

        /// <summary>
        /// 切换已连接的行
        /// </summary>
        /// <param name="rowIndex"></param>
        private void ChangeConnectedRow(int rowIndex)
        {
            foreach (DataGridViewRow dgvr in dgv_SlaveDevices.Rows)
            {
                if (dgvr.Index == rowIndex)
                {
                    dgvr.ReadOnly = true;
                    //dgvr.HeaderCell.Style.ForeColor = Color.Red;
                    dgvr.HeaderCell.Style.BackColor = Color.LightSkyBlue;
                    dgvr.Cells[cnSN4Dgv].ToolTipText = "已连接";
                    dgvr.Cells[cnDel4Dgv].ToolTipText = "已连接的从机无法删除";
                    dgvr.Cells[cnFrom4Dgv].Style.ForeColor = Color.Green;
                }
                else
                {
                    dgvr.ReadOnly = false;
                    dgvr.HeaderCell.Style.BackColor = SystemColors.Control;
                    dgvr.Cells[cnSN4Dgv].ToolTipText = "";
                    dgvr.Cells[cnDel4Dgv].ToolTipText = "";
                }
            }
        }

        /// <summary>
        /// 添加SN到dgv_SNList
        /// </summary>
        /// <param name="sn"></param>
        /// <param name="isReadOnly"></param>
        private void AddToSlaveDgv(string sn, bool isReadOnly = true)
        {
            if (dgv_SlaveDevices.InvokeRequired)
            {
                while (!dgv_SlaveDevices.IsHandleCreated)
                {
                    //解决窗体关闭时出现“访问已释放句柄“的异常
                    if (dgv_SlaveDevices.Disposing || dgv_SlaveDevices.IsDisposed)
                        return;
                }
                //var res = dgv_SNList.BeginInvoke(new Action<string, bool>(AddToDgv), sn, isReadOnly);
                //dgv_SNList.EndInvoke(res);
                dgv_SlaveDevices.Invoke(new Action<string, bool>(AddToSlaveDgv), sn, isReadOnly);
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(sn) && !snLst.Contains(sn))
                {
                    snLst.Add(sn);
                    var rIndex = dgv_SlaveDevices.Rows.Add(sn, "扫描得到");
                    var dgvr = dgv_SlaveDevices.Rows[rIndex];
                    dgvr.ReadOnly = isReadOnly;
                    dgvr.Cells[cnFrom4Dgv].Style.ForeColor = Color.LightSkyBlue;
                }
            }
        }

        /// <summary>
        /// 更新COM口状态
        /// </summary>
        /// <param name="msg"></param>
        private void UpdateComStatus(string msg, Color color)
        {
            if (lbl_PortStatus.InvokeRequired)
            {
                while (!lbl_PortStatus.IsHandleCreated)
                {
                    //解决窗体关闭时出现“访问已释放句柄“的异常
                    if (lbl_PortStatus.Disposing || lbl_PortStatus.IsDisposed)
                        return;
                }
                var res = lbl_PortStatus.BeginInvoke(new Action<string, Color>(UpdateScanStatus), msg, color);
                lbl_PortStatus.EndInvoke(res);
            }
            else
            {
                lbl_PortStatus.ForeColor = color;
                lbl_PortStatus.Text = msg;
            }
        }

        /// <summary>
        /// 更新扫描状态
        /// </summary>
        /// <param name="msg"></param>
        private void UpdateScanStatus(string msg, Color color)
        {
            if (lbl_ScanStatus.InvokeRequired)
            {
                while (!lbl_ScanStatus.IsHandleCreated)
                {
                    //解决窗体关闭时出现“访问已释放句柄“的异常
                    if (lbl_ScanStatus.Disposing || lbl_ScanStatus.IsDisposed)
                        return;
                }
                var res = lbl_ScanStatus.BeginInvoke(new Action<string, Color>(UpdateScanStatus), msg, color);
                lbl_ScanStatus.EndInvoke(res);
            }
            else
            {
                lbl_ScanStatus.ForeColor = color;
                lbl_ScanStatus.Text = msg;
            }
        }

        /// <summary>
        /// 串口数据解析
        /// </summary>
        private void StartHandleSerialData()
        {
            Task.Factory.StartNew(DataHandleAsync, cts.Token);
        }

        /// <summary>
        /// 串口数据解析
        /// </summary>
        /// <returns></returns>r
        private async Task DataHandleAsync()
        {
            var funCode = HCIinstructions.ScanSlaveKey;
            while (!isCancell)
            {
                if (!pauseHandleData.SafeWaitHandle.IsClosed)
                {
                    pauseHandleData.WaitOne();
                }

                var dataFrame = await uart.GetNewFrameAsync(funCode, isCancell);
                if (dataFrame == null)
                {
                    continue;
                }
                var validLen = uart.GetValidDataLength(dataFrame.Length, 3, funCode);
                var ackVal = uart.ToValue<string>(dataFrame, 8, validLen);
                if (!string.IsNullOrWhiteSpace(ackVal))
                {
                    var sn = ackVal.Trim('?').Trim('\0');
                    AddToSlaveDgv(sn);
                }
            }
        }
        #endregion 私有方法 start

        #region    公开方法 start
        /// <summary>
        /// 应用数据源
        /// </summary>
        /// <param name="dataTable"></param>
        public void ApplyDataSource(DataTable dataTable)
        {
            if (dataTable == null)
            {
                return;
            }

            foreach (DataRow dr in dataTable.Rows)
            {
                var rIndex = dgv_SlaveDevices.Rows.Add(dr[cnSN4DS]);
                if (int.TryParse(dr[cnDef4DS].ToString(), out int def))
                {
                    if (def == 1)
                    {
                        ChangeConnectedRow(rIndex);
                    }
                }
            }
        }

        /// <summary>
        /// 应用当前SN
        /// </summary>
        /// <param name="sn"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public void ApplyCurrentSN(string sn)
        {
            if (!string.IsNullOrWhiteSpace(sn))
            {
                var rIndex = dgv_SlaveDevices.Rows.Add(sn);
                dgv_SlaveDevices.Rows[rIndex].Cells[cnFrom4Dgv].Value = "扫描得到";
                ChangeConnectedRow(rIndex);
            }
        }
        #endregion 公开方法 start
    }
}