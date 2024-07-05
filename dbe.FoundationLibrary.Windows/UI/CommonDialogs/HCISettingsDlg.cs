using dbe.FoundationLibrary.Communication.RTUComm;
using dbe.FoundationLibrary.Core.Util;
using dbe.FoundationLibrary.Windows.Mvvm;
using dbe.FoundationLibrary.Windows.UI.CustomForms;

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

using static dbe.FoundationLibrary.Communication.RTUComm.SerialPortService;

namespace dbe.FoundationLibrary.Windows.UI.CommonDialogs
{
    public partial class HCISettingsDlg : BaseWithBorderless
    {
        #region    字段 start
        private LoggerUtil logger = LoggerUtil.Instance;
        private SerialPortService uart;
        private int rawHeight;
        private List<string> snLst;
        private CancellationTokenSource ctsScan0;// 取消扫描蓝牙从机名线程的控制器
        private CancellationTokenSource ctsScan1;// 取消扫描ESB从机信道线程的控制器
        private ManualResetEvent pauseHandleData0 = new ManualResetEvent(true);
        private ManualResetEvent pauseHandleData1 = new ManualResetEvent(true);
        private bool isScanning0 = false;// 是否正在扫描蓝牙从机
        private bool isCancell = false;// 是否取消扫描
        private string cnSN4Dgv = nameof(dgvc_TxtSN);// 设备SN列的列名
        private string cnFrom4Dgv = nameof(dgvc_TxtFrom);// 来源列的列名
        private string cnDel4Dgv = nameof(dgvc_BtnDelete);// 删除列的列名
        private string cnSN4DS = "SN";// 数据源SN列的列名
        private string cnDef4DS = "Default";// 数据源Default列的列名
        private bool isScanning1 = false;// 是否正在扫描ESB从机
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
        public HCISettingsDlg(SerialPort serialPort, Func<bool, Task<bool>> switchPort)
        {
            //CtorBase(serialPort, switchPort);
            InitializeComponent();
            cbb_Ports.DisplayMember = "Value";
            cbb_Ports.ValueMember = "Key";

            this.rawHeight = this.Height;
            this.SwitchPort = switchPort;
            UpdateUIStatus(serialPort.IsOpen);

        }

        public HCISettingsDlg(RTUCore rtu) : this((rtu as SerialPortService).SerialPort, (rtu as SerialPortService).SwitchPortAsync)
        {
            uart = rtu as SerialPortService;
            if (uart.ToSerialType == ToSerialType.ESBToSerial)
            {
                nud_CurrentCommunicationChannel.Value = uart.CurrentCommunicationChannel;
            }
            uart.OnDeviceChanged += Uart_OnDeviceChanged;
            ChangeLayout();
            //CtorBase(uart.SerialPort, uart.SwitchPortAsync);
        }
        #endregion 构造与析构 end

        #region    事件处理 start
        // 设置对话框加载时
        private void SerialPortSettingsDlg_Load(object sender, EventArgs e)
        {
            InitSource();

            cbb_Ports.TwoWayBinding("SelectedValue", SerialPort, "PortName");
            cbb_BaudRates.TwoWayBinding("SelectedValue", SerialPort, "BaudRate", true);

            switch (uart.ToSerialType)
            {
                case ToSerialType.DirectConnection:
                    break;
                case ToSerialType.BluetoothToSerial:
                    pauseHandleData0.Reset();// 暂停解析SN的线程
                    StartHandleSerialData();// 启动本类中的串口数据解析线程
                    break;
                case ToSerialType.ESBToSerial:
                    break;
            }
        }

        // UART设备插拔事件
        private void Uart_OnDeviceChanged(string devicePort, DeviceChangedEventArgs e)
        {
            if (this.InvokeRequired)
            {
                while (!this.IsHandleCreated)
                {
                    //解决窗体关闭时出现“访问已释放句柄“的异常
                    if (this.Disposing || this.IsDisposed)
                        return;
                }
                var res = this.BeginInvoke(new USBSerialPortChanged(Uart_OnDeviceChanged), devicePort, e);
                this.EndInvoke(res);
            }
            else
            {
                if (e.IsConnect)
                {// 插入设备
                    if (btn_Refresh.Enabled)
                    {
                        btn_Refresh_Click(null, EventArgs.Empty);
                    }
                    if (e.DeviceNotChanged)
                    {
                        UpdateComStatus($"{devicePort}已重新插入", Color.Green);
                        ClickToSwitch(true);
                    }
                    else
                    {
                        UpdateComStatus($"{devicePort}已插入", Color.Goldenrod);
                    }
                }
                else
                {// 拔出设备
                    if (e.DeviceNotChanged)
                    {
                        ClickToSwitch(false);
                        btn_Refresh_Click(null, EventArgs.Empty);
                    }
                    UpdateComStatus($"{devicePort}已拔出", Color.Goldenrod);
                }
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

        // 蓝牙转串口-刷新串口列表
        private void btn_Refresh_Click(object sender, EventArgs e)
        {
            cbb_Ports.DataSource = ComLst;
            cbb_Ports.Refresh();
        }

        // 蓝牙转串口-扫描蓝牙从机
        private async void btn_Scan0_Click(object sender, EventArgs e)
        {
            try
            {
                tglbtn_Switch.Enabled = false;
                btn_Scan0.Enabled = false;
                //BluetoothUtils.PrintAllDevices();
                if (isScanning0)
                {// 暂停扫描
                    (bool success, string message) = await uart.StopScanAsync();
                    if (success)
                    {// 停止扫描指令发送成功
                        btn_Scan0.Text = "扫描";
                        pauseHandleData0.Reset();// 暂停解析SN的线程
                        UpdateScanStatus0("扫描已停止", Color.Green);
                        isScanning0 = false;
                        OnAfterStopScan?.Invoke();
                    }
                    else
                    {// 停止扫描指令发送失败
                        UpdateScanStatus0(message, Color.Red);
                    }
                }
                else
                {// 开始扫描
                    (bool success, string message) = await uart.StartScanAsync();
                    if (success)
                    {// 扫描指令发送成功
                        btn_Scan0.Text = "停止";
                        isScanning0 = true;
                        OnBeforeStartScan?.Invoke();// 开始扫描前取消MFVM中的串口数据解析线程
                        await uart.SwitchRecvBtCfgDataAsync();// uart切换到接收蓝牙配置数据
                        pauseHandleData0.Set();// 重启解析SN的线程

                        snLst.Clear();// 清空内存中的从机列表
                        RemoveAllNotNewRow();
                        OnChangedConnect?.Invoke("");// 清空主界面中的SN
                        UpdateScanStatus0("扫描中...", Color.Red);
                    }
                    else
                    {// 扫描指令发送失败
                        UpdateScanStatus0(message, Color.Red);
                    }
                }
            }
            catch (Exception ex)
            {
                var msg = ex.Message.Replace("\r\n", "");
                UpdateScanStatus0(msg, Color.Red);
                Trace.WriteLine(msg);
                logger.Fatal(msg, ex);
            }
            finally
            {
                btn_Scan0.Enabled = true;
                tglbtn_Switch.Enabled = true;
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

                UpdateScanStatus0($"正在连接[{sn}]...", Color.Green);
                (bool success, string message) = await uart.ConnectAsync(sn);
                if (success)
                {
                    pauseHandleData0.Reset();
                    btn_Scan0.Text = "扫描";
                    isScanning0 = false;
                    UpdateScanStatus0($"已连接到[{sn}]", Color.Green);

                    await uart.SwitchRecvNormalDataAsync();
                    ChangeConnectedRow(e.RowIndex);
                    OnChangedConnect?.Invoke(sn);
                }
                else
                {
                    //MessageBox.Show(message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    UpdateScanStatus0($"连接[{sn}]失败", Color.Red);
                    logger.Error(message);
                }
            }
            catch (Exception ex)
            {
                var msg = $"切换传感器异常：{ex.Message.Replace("\r\n", "")}";
                //MessageBox.Show(msg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                UpdateScanStatus0(msg, Color.Red);
                logger.Fatal(msg, ex);
                //UpdateMessage(Color.Red, msg);
            }
        }

        // ESB转串口-修改主机信道以匹配从机(匹配后就会自动连接)
        private void btn_Scan1_Click(object sender, EventArgs e)
        {
            try
            {
                btn_Scan1.Enabled = false;
                if (isScanning1)
                {// 暂停扫描
                    btn_Scan1.Text = "扫描";
                    pauseHandleData1.Reset();// 暂停解析SN的线程
                    UpdateStatusAfterPauseScan1(true);
                    UpdateScanStatus1("扫描已停止", Color.Green);
                    isScanning1 = false;
                    OnAfterStopScan?.Invoke();
                }
                else
                {// 开始扫描
                    isScanning1 = true;
                    UpdateStatusAfterPauseScan1(false);
                    var chanNoStart = (int)nud_CurrentCommunicationChannel.Value;
                    var rangeChannels = Enumerable.Range(40, 61).ToList();
                    pauseHandleData1.Set();
                    Task.Factory.StartNew(async () =>
                    {
                        var count4RetryModifyHost = 0;
                        while (true)
                        {
                            if (!pauseHandleData1.SafeWaitHandle.IsClosed)
                            {
                                pauseHandleData1.WaitOne();
                            }

                            var isRetry = false;
                            var chanNo = (int)nud_CurrentCommunicationChannel.Value;// 信道号从decimal类型变为int方便判断和剔除
                            var currentIndex = rangeChannels.FindIndex(m => m == chanNo);
                        RetryModifyHost:
                            (bool success, string msg) = await uart.ChangeHostChannelNo(chanNo);
                            if (success)
                            {// 修改成功可认为已连接
                                (success, msg) = await uart.ChangeBothChannelNo(chanNo);
                                if (success)
                                {// 修改成功可认为已连接
                                    SetCommunicationChannel(chanNo);// 应同步信道号到主从机通信信道控件
                                    UpdateScanStatus1($"通信信道{chanNo}匹配成功", Color.Green);
                                    uart.CurrentCommunicationChannel = chanNo;
                                    break;
                                }
                                else
                                {// 如果是信道不匹配则应该修改主机信道后再试
                                    isRetry = true;
                                }
                            }
                            else
                            {// 如果是修改主机信道失败则应该延迟500ms后再试2次
                                UpdateScanStatus1($"修改主机当前信道{chanNo}失败", Color.Red);
                                if (count4RetryModifyHost < 3)
                                {
                                    await Task.Delay(500);
                                    count4RetryModifyHost++;
                                    goto RetryModifyHost;
                                }
                                count4RetryModifyHost = 0;
                            }
                            // uart.IsRecived不是实时的，延迟会导致判断错误
                            //if (uart.IsRecived)
                            //{// 如果已开始接收数据那么忽略可能出现的任何通信信道匹配失败
                            //    UpdateActionStatus($"接收到数据，通信信道{chanNo}匹配成功", Color.Green);
                            //    break;
                            //}
                            if (isRetry)
                            {
                                UpdateScanStatus1($"通信信道{chanNo}匹配失败", Color.Red);
                                await Task.Delay(500);
                                currentIndex++;
                                if (currentIndex == rangeChannels.Count)
                                {
                                    currentIndex = 0;
                                }
                                var chanNoNew = rangeChannels[currentIndex];// 更换新的信道号
                                if (chanNoNew == chanNoStart)
                                {
                                    break;
                                }
                                SetCommunicationChannel(chanNoNew);
                                continue;
                            }
                        }
                        UpdateStatusAfterPauseScan1(true);
                    }, ctsScan1.Token);
                    UpdateScanStatus1("扫描中...", Color.Red);
                }
            }
            catch (Exception ex)
            {
                var msg = ex.Message.Replace("\r\n", "");
                UpdateScanStatus0(msg, Color.Red);
                Trace.WriteLine(msg);
                logger.Fatal(msg, ex);
            }
            finally
            {
                btn_Scan1.Enabled = true;
            }
        }

        // ESB转串口-修改主机信道以匹配从机(匹配后就会自动连接)
        private async void btn_Scan1_Click1(object sender, EventArgs e)
        {
            try
            {
                tglbtn_Switch.Enabled = false;
                btn_Scan1.Enabled = false;
                btn_Modify.Enabled = false;
                nud_CurrentCommunicationChannel.Enabled = false;
                var chanNoStart = (int)nud_CurrentCommunicationChannel.Value;
                var rangeChannels = Enumerable.Range(40, 61).ToList();

                while (true)
                {
                    var isRetry = false;
                    var chanNo = (int)nud_CurrentCommunicationChannel.Value;// 信道号从decimal类型变为int方便判断和剔除
                    var currentIndex = rangeChannels.FindIndex(m => m == chanNo);
                    (bool success, string msg) = await uart.ChangeHostChannelNo(chanNo);
                    if (success)
                    {// 修改成功可认为已连接
                        (success, msg) = await uart.ChangeBothChannelNo(chanNo);
                        if (success)
                        {// 修改成功可认为已连接
                            nud_CurrentCommunicationChannel.Value = chanNo; // 应同步信道号到主从机通信信道控件
                            UpdateScanStatus1($"通信信道{chanNo}匹配成功", Color.Green);
                            uart.CurrentCommunicationChannel = chanNo;
                            break;
                        }
                        else
                        {// 如果是信道不匹配则应该修改主机信道后再试
                            isRetry = true;
                        }
                    }
                    else
                    {// 如果是信道不匹配则应该修改主机信道后再试
                        UpdateScanStatus1($"修改主机当前信道{chanNo}失败", Color.Red);
                        isRetry = true;
                    }
                    // uart.IsRecived不是实时的，延迟会导致判断错误
                    //if (uart.IsRecived)
                    //{// 如果已开始接收数据那么忽略可能出现的任何通信信道匹配失败
                    //    UpdateActionStatus($"接收到数据，通信信道{chanNo}匹配成功", Color.Green);
                    //    break;
                    //}
                    if (isRetry)
                    {
                        UpdateScanStatus1($"通信信道{chanNo}匹配失败", Color.Red);
                        await Task.Delay(500);
                        currentIndex++;
                        if (currentIndex == rangeChannels.Count)
                        {
                            currentIndex = 0;
                        }
                        var chanNoNew = rangeChannels[currentIndex];// 更换新的信道号
                        if (chanNoNew == chanNoStart)
                        {
                            break;
                        }
                        nud_CurrentCommunicationChannel.Value = chanNoNew;
                        continue;
                    }
                }
            }
            catch (Exception ex)
            {
                var msg = ex.Message.Replace("\r\n", "");
                UpdateScanStatus0(msg, Color.Red);
                Trace.WriteLine(msg);
                logger.Fatal(msg, ex);
            }
            finally
            {
                nud_CurrentCommunicationChannel.Enabled = true;
                btn_Scan1.Enabled = true;
                btn_Modify.Enabled = true;
                tglbtn_Switch.Enabled = true;
            }
        }

        // ESB转串口-修改已连接的通信信道
        private async void btn_Modify_Click(object sender, EventArgs e)
        {
            try
            {
                tglbtn_Switch.Enabled = false;
                btn_Scan1.Enabled = false;
                btn_Modify.Enabled = false;
                nud_CurrentCommunicationChannel.Enabled = false;

                var chanNo = (int)nud_CurrentCommunicationChannel.Value;// 信道号从decimal类型变为int方便判断和剔除
                (bool success, string msg) = await uart.ChangeBothChannelNo(chanNo);
                if (success)
                {// 修改成功可认为已连接
                    nud_CurrentCommunicationChannel.Value = nud_CurrentCommunicationChannel.Value; // 主机信道可能已修改因此应同步信道号到主机当前信道控件
                    UpdateScanStatus1($"信道{chanNo}连接成功", Color.Green);
                    uart.CurrentCommunicationChannel = chanNo;
                }
                else
                {// 如果是信道不匹配则应该修改主机信道后再试
                    nud_CurrentCommunicationChannel.Value = uart.CurrentCommunicationChannel;
                    UpdateScanStatus1($"信道{chanNo}连接失败，信道号已回退", Color.Red);
                }
            }
            catch (Exception ex)
            {
                var msg = ex.Message.Replace("\r\n", "");
                UpdateScanStatus0(msg, Color.Red);
                Trace.WriteLine(msg);
                logger.Fatal(msg, ex);
            }
            finally
            {
                nud_CurrentCommunicationChannel.Enabled = true;
                btn_Scan1.Enabled = true;
                btn_Modify.Enabled = true;
                tglbtn_Switch.Enabled = true;
            }
        }

        // 关闭时保存当前串口和波特率
        private void SerialPortSettingsDlg_FormClosing(object sender, FormClosingEventArgs e)
        {
            switch (uart.ToSerialType)
            {
                case ToSerialType.BluetoothToSerial:
                    if (pauseHandleData0.Reset())
                    {// 取消接收SN的线程
                        isCancell = true;
                        ctsScan0.Cancel();
                    }
                    break;
                case ToSerialType.ESBToSerial:
                    if (pauseHandleData1.Reset())
                    {// 取消匹配信道的线程
                        ctsScan1.Cancel();
                    }
                    break;
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
            nud_CurrentCommunicationChannel.Value = uart.CurrentCommunicationChannel;
            cbb_Ports.DisplayMember = "Value";
            cbb_Ports.ValueMember = "Key";

            this.rawHeight = this.Height;

            this.SwitchPort = switchPort;
            UpdateUIStatus(serialPort.IsOpen);
            ChangeLayout();
        }

        /// <summary>
        /// 初始化MainForm中用到的数据源
        /// </summary>
        private void InitSource()
        {
            snLst = new List<string>();
            ctsScan0 = new CancellationTokenSource();
            ctsScan1 = new CancellationTokenSource();

            btn_Refresh_Click(null, EventArgs.Empty);

            if (ComLst.Exists(kvp => kvp.Key == SerialPort.PortName))
            {
                cbb_Ports.SelectedValue = SerialPort.PortName;
            }

            var baudRateLst = EnumHelper.GetEnumValuesToNumbers<int, BaudRates>();
            cbb_BaudRates.DataSource = baudRateLst;
            cbb_BaudRates.DisplayMember = "Value";
            cbb_BaudRates.ValueMember = "Value";
            //int specifiedBaudRate = 230400;
            int specifiedBaudRate = SerialPort.BaudRate;
            //if (baudRates.Contains(specifiedBaudRate))
            //{
            //    cbb_BaudRates.SelectedValue = specifiedBaudRate;
            //}

            if (baudRateLst.Exists(kvp => kvp.Key == specifiedBaudRate))
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
            btn_Scan0.Enabled = isOpened;
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
        private void AddToDgv(string sn, bool isReadOnly = true)
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
                dgv_SlaveDevices.Invoke(new Action<string, bool>(AddToDgv), sn, isReadOnly);
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
                var res = lbl_PortStatus.BeginInvoke(new Action<string, Color>(UpdateScanStatus0), msg, color);
                lbl_PortStatus.EndInvoke(res);
            }
            else
            {
                lbl_PortStatus.ForeColor = color;
                lbl_PortStatus.Text = msg;
            }
        }

        /// <summary>
        /// 蓝牙转串口-更新扫描状态
        /// </summary>
        /// <param name="msg">消息文本</param>
        /// <param name="color">文本颜色</param>
        private void UpdateScanStatus0(string msg, Color color)
        {
            if (lbl_ScanStatus0.InvokeRequired)
            {
                while (!lbl_ScanStatus0.IsHandleCreated)
                {
                    //解决窗体关闭时出现“访问已释放句柄“的异常
                    if (lbl_ScanStatus0.Disposing || lbl_ScanStatus0.IsDisposed)
                        return;
                }
                var res = lbl_ScanStatus0.BeginInvoke(new Action<string, Color>(UpdateScanStatus0), msg, color);
                lbl_ScanStatus0.EndInvoke(res);
            }
            else
            {
                lbl_ScanStatus0.ForeColor = color;
                lbl_ScanStatus0.Text = msg;
            }
        }

        /// <summary>
        /// ESB转串口-更新操作状态
        /// </summary>
        /// <param name="msg">消息文本</param>
        /// <param name="color">文本颜色</param>
        private void UpdateScanStatus1(string msg, Color color)
        {
            if (lbl_ScanStatus1.InvokeRequired)
            {
                while (!lbl_ScanStatus1.IsHandleCreated)
                {
                    //解决窗体关闭时出现“访问已释放句柄“的异常
                    if (lbl_ScanStatus1.Disposing || lbl_ScanStatus1.IsDisposed)
                        return;
                }
                var res = lbl_ScanStatus1.BeginInvoke(new Action<string, Color>(UpdateScanStatus1), msg, color);
                lbl_ScanStatus1.EndInvoke(res);
            }
            else
            {
                lbl_ScanStatus1.ForeColor = color;
                lbl_ScanStatus1.Text = msg;
            }
        }

        /// <summary>
        /// 根据连接和通信方式改变表格布局
        /// </summary>
        private void ChangeLayout()
        {
            this.radbtn_WirelessSerialPort.Checked = uart.ConnectionMode == SerialConnectionMode.WirelessSerial;
            this.radbtn_WiredSerialPort.Checked = uart.ConnectionMode == SerialConnectionMode.WiredSerial;
            switch (uart.ToSerialType)
            {
                case ToSerialType.BluetoothToSerial:
                    {
                        radbtn_BluetoothToSerial.Checked = true;
                        tlp_Global.RowStyles[1].Height = 280f;
                        gp_Slave0.Visible = true;
                        tlp_Global.RowStyles[2].Height = 0f;
                        gp_Slave1.Visible = false;
                        this.Height = this.rawHeight - 160;
                    }
                    break;
                case ToSerialType.ESBToSerial:
                    {
                        radbtn_ESBToSerial.Checked = true;
                        tlp_Global.RowStyles[1].Height = 0f;
                        gp_Slave0.Visible = false;
                        tlp_Global.RowStyles[2].Height = 108f;
                        gp_Slave1.Visible = true;
                        this.Height = this.rawHeight - 284;
                    }
                    break;
                case ToSerialType.DirectConnection:
                    {
                        radbtn_DirectConnection.Checked = true;
                        tlp_Global.RowStyles[1].Height = 0f;
                        gp_Slave0.Visible = false;
                        tlp_Global.RowStyles[2].Height = 0f;
                        gp_Slave1.Visible = false;
                        this.Height = this.rawHeight - 396;
                    }
                    break;
            }
        }

        /// <summary>
        /// 多线程环境设置信道控件的值
        /// </summary>
        /// <param name="channel"></param>
        private void SetCommunicationChannel(int channel)
        {
            if (nud_CurrentCommunicationChannel.InvokeRequired)
            {
                while (!nud_CurrentCommunicationChannel.IsHandleCreated)
                {
                    //解决窗体关闭时出现“访问已释放句柄“的异常
                    if (nud_CurrentCommunicationChannel.Disposing || nud_CurrentCommunicationChannel.IsDisposed)
                        return;
                }
                var res = nud_CurrentCommunicationChannel.BeginInvoke(new Action<int>(SetCommunicationChannel), channel);
                nud_CurrentCommunicationChannel.EndInvoke(res);
            }
            else
            {
                nud_CurrentCommunicationChannel.Value = channel;
            }
        }

        /// <summary>
        /// 暂停ESB扫描后更新控件状态
        /// </summary>
        private void UpdateStatusAfterPauseScan1(bool isStop = false)
        {
            if (gp_Slave1.InvokeRequired)
            {
                while (!gp_Slave1.IsHandleCreated)
                {
                    //解决窗体关闭时出现“访问已释放句柄“的异常
                    if (gp_Slave1.Disposing || gp_Slave1.IsDisposed)
                        return;
                }
                var res = gp_Slave1.BeginInvoke(new Action<bool>(UpdateStatusAfterPauseScan1), isStop);
                gp_Slave1.EndInvoke(res);
            }
            else
            {
                btn_Scan1.Text = isStop ? "扫描" : "停止";
                nud_CurrentCommunicationChannel.Enabled = isStop;
                btn_Scan1.Enabled = isStop;
                btn_Modify.Enabled = isStop;
                tglbtn_Switch.Enabled = isStop;
            }
        }

        /// <summary>
        /// 串口数据解析-启动接收广播蓝牙名的线程接收并解析应答数据为设备名
        /// </summary>
        private void StartHandleSerialData()
        {
            Task.Factory.StartNew(DataHandleAsync, ctsScan0.Token);
        }

        /// <summary>
        /// 串口数据解析(扫到的蓝牙设备名)
        /// </summary>
        /// <returns></returns>r
        private async Task DataHandleAsync()
        {
            var funCode = HCIinstructions.ScanSlaveKey;
            while (!isCancell)
            {
                if (!pauseHandleData0.SafeWaitHandle.IsClosed)
                {
                    pauseHandleData0.WaitOne();
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
                    AddToDgv(sn);
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