using dbe.FoundationLibrary.Core.Extensions;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Management;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace dbe.FoundationLibrary.Communication.RTUComm
{
    /// <summary>
    /// 串口通信服务类
    /// </summary>
    public class SerialPortService : RTUCore
    {
        #region    字段 start
        private bool isBluetoothToSerial;// 是否蓝牙转串口
        private SerialPort serialPort;// PC选定串口
        private string portName4Set;// 配置文件设定的端口号
        private int baudRate;// 波特率
        private int initFrameLength;// 初始接收帧长度
        private int minimumInstructionLength;// 最小指令长度

        private int frameLength = RTUDefault.FrameLength;// 接收数据帧的(固定)长度

        private ConcurrentQueue<byte> rqRawData = new ConcurrentQueue<byte>();// 串口缓冲区的原始数据(任意长度)
        private ConcurrentDictionary<byte, ConcurrentQueue<byte[]>> dictAckData;// <功能码,对应的数据队列>
        private ConcurrentQueue<byte> rqVibration = new ConcurrentQueue<byte>();// 振动数据(233byte)
        private ConcurrentQueue<byte[]> rqRegularAck = new ConcurrentQueue<byte[]>();// 通用应答数据(12byte)

        private ManagementEventWatcher insertWatcher = null;// USB插入事件监视
        private ManagementEventWatcher removeWatcher = null;// USB拔出事件监视

        //private readonly Hourglass timer = new Hourglass();
        //private long ReceivedCount = 0;// 收到数据次数

        //private System.Timers.Timer timerSpeedTest;// 测速定时器
        public CancellationTokenSource ctsReceive = new CancellationTokenSource();
        public CancellationTokenSource ctsHandle = new CancellationTokenSource();
        #endregion 字段 end

        #region    属性 start
        /// <summary>
        /// 获取当前通信服务的通信模式(顶级分类)
        /// </summary>
        public CommunicationMode CommunicationMode => CommunicationMode.Serial;
        /// <summary>
        /// 获取当前通信服务的连接方式(2级子类)
        /// 支持无线串口/有线串口
        /// </summary>
        public SerialConnectionMode ConnectionMode
        {
            get
            {
                var scm = SerialConnectionMode.WiredSerial;
                if (ToSerialType != ToSerialType.DirectConnection)
                {
                    scm = SerialConnectionMode.WirelessSerial;
                }
                return scm;
            }
        }
        /// <summary>
        /// 获取或设置当前通信服务的转串口类型(3级子类)
        /// 无线串口时分为 蓝牙转串口/ESB转串口
        /// 有线串口时值为 直连
        /// </summary>
        public ToSerialType ToSerialType { get; set; }

        /// <summary>
        /// 主数据帧长度
        /// </summary>
        public new int FrameLength
        {
            get => frameLength;
            set => frameLength = value;
        }

        /// <summary>
        /// 读取超时(ms)
        /// </summary>
        public int ReadTimeout
        {
            get { return serialPort.ReadTimeout; }
            set { serialPort.ReadTimeout = value; }
        }

        /// <summary>
        /// 写入超时(ms)
        /// </summary>
        public int WriteTimeout
        {
            get { return serialPort.WriteTimeout; }
            set { serialPort.WriteTimeout = value; }
        }

        /// <summary>
        /// 是否漏号重传
        /// </summary>
        public bool IsResendAfterMissed { get; set; }

        /// <summary>
        /// 全局串口
        /// </summary>
        public SerialPort SerialPort
        {
            get => serialPort;
            set => serialPort = value;
        }

        /// <summary>
        /// 端口号
        /// </summary>
        public string PortName => serialPort.PortName;

        /// <summary>
        /// 最近一次打开过的端口名
        /// </summary>
        public string LastPortName { get; set; }

        /// <summary>
        /// 波特率
        /// </summary>
        public int BaudRate
        {
            get => baudRate;
            set => baudRate = value;
        }

        /// <summary>
        /// ESB转串口-当前通信信道
        /// </summary>
        public int CurrentCommunicationChannel { get; set; } = 40;

        /// <summary>
        /// 指令最短长度(11byte)
        /// </summary>
        public new int MinimumInstructionLength
        {
            get
            {
                if (minimumInstructionLength == 0)
                {
                    // 固定头所占字节数 + 数据长度所占字节数 + 功能码所占字节数 + CRC所占字节数 + 固定尾所占字节数
                    minimumInstructionLength = FixedPrefix.BytesLength + 2 + 1 + 2 + FixedSuffix.BytesLength;
                }
                return minimumInstructionLength;
            }
        }

        public ConcurrentDictionary<byte, ConcurrentQueue<byte[]>> AckDataDict { get => dictAckData; set => dictAckData = value; }

        /// <summary>
        /// 更新串口状态
        /// </summary>
        public Action<bool, string, string> UpdateComStatus { set; get; }
        #endregion 属性 end

        #region    构造与析构 start
        public SerialPortService(InstructionFormat instructionFormat, ToSerialType toSerialType = ToSerialType.BluetoothToSerial, string portName = "COM3", int recvFrameLength = 11, int baudRate = 115200) : base(instructionFormat)
        {
            this.initFrameLength = recvFrameLength;
            this.ToSerialType = toSerialType;
            base.IsBluetoothToSerial = toSerialType == ToSerialType.BluetoothToSerial;
            this.frameLength = recvFrameLength;
            this.baudRate = baudRate;

            //timerSpeedTest = new System.Timers.Timer(5000);
            //timerSpeedTest.AutoReset = true;
            //timerSpeedTest.Enabled = true;
            //timerSpeedTest.Elapsed += TimerSpeedTest_Elapsed;
            //var kvpLst = EnumHelper.GetEnumValuesWithNumber<int, BaudRates>();

            this.portName4Set = portName;
            serialPort = new SerialPort(portName, baudRate);
            serialPort.ReadBufferSize = FrameLength * 4;// 每次最多读4行数据
            serialPort.NewLine = RTUDefault.NewLine; // 设置ReadLine()换行标识符

            USBEventWatcher(USBEventHandler, USBEventHandler, TimeSpan.FromMilliseconds(1));
        }

        ~SerialPortService()
        {
            RemoveUSBEventWatcher();
        }
        #endregion 构造与析构 end

        #region        事件声明 start
        /// <summary>
        /// 检测到广播数据从收到变为未收到或者从未收到变为收到时
        /// </summary>
        /// <param name="isReceive">true表示从未收到变为收到/false表示收到变为未收到</param>
        public delegate void CommunicateStateChangedEvent(bool isReceive);
        /// <summary>
        /// 反馈(广播)通信状态的事件，非广播的通信例如交互式通信的状态无法反馈
        /// </summary>
        public event CommunicateStateChangedEvent OnCommunicateStateChanged;

        /// <summary>
        /// 检测到广播数据时开启处理
        /// </summary>
        /// <param name="deviceInfo"></param>
        public delegate void HandleAdvertisementDataEvent(SerialPort serialPort);
        /// <summary>
        /// 处理广播数据的事件，交互式通信时不可用
        /// </summary>
        public event HandleAdvertisementDataEvent OnHandleAdvertisementData;

        /// <summary>
        /// 串口接收到数据时
        /// </summary>
        /// <param name="serialPort">当前串口</param>
        /// <param name="data">接收的数据</param>
        public delegate void ReceivedEvent(SerialPort serialPort, byte[] data);
        /// <summary>
        /// 接收到数据的事件
        /// </summary>
        public event ReceivedEvent OnReceived;

        /// <summary>
        /// 检测到(串口)设备连接/断开时
        /// </summary>
        /// <param name="device">连接/断开的串口设备</param>
        /// <param name="args">设备变更事件参数</param>
        public delegate void SPConnectChangedEvent(SerialPort serialPort, DeviceChangedEventArgs args);
        /// <summary>
        /// 串口连接/断开事件
        /// </summary>
        public event SPConnectChangedEvent OnConnectChanged;

        ///// <summary>
        ///// 串口设备列表被更新时
        ///// </summary>
        ///// <param name="infoLst"></param>
        ///// <param name="deviceInfoLst"></param>
        //public delegate void UpdateSPDevciceEvent(List<(string, int, bool)> infoLst, List<BluetoothDeviceInfo> deviceInfoLst);
        ///// <summary>
        ///// 更新搜索到的串口设备的事件
        ///// </summary>
        //public event UpdateSPDevciceEvent OnUpdateBTDevcice;

        /// <summary>
        /// 检测到插拔(UART)设备时
        /// </summary>
        /// <param name="devicePort">插入或拔出的USB串口设备端口号</param>
        /// <param name="args">设备变更事件参数</param>
        public delegate void USBSerialPortChanged(string devicePort, DeviceChangedEventArgs args);
        /// <summary>
        /// USB串口设备拔插事件
        /// </summary>
        public event USBSerialPortChanged OnDeviceChanged;
        #endregion 事件声明 end

        #region        事件处理 Start
        private void TimerSpeedTest_Elapsed(object sender, ElapsedEventArgs e)
        {
            var rate = rqRawData.Count / 233 / 5f;
            Trace.WriteLine($"当前速率：{rate}帧/秒");
            rqRawData = new ConcurrentQueue<byte>();
        }
        #endregion 事件处理 end

        #region    公开方法 start
        /// <summary>
        /// 串口清理，一般用于程序退出之前
        /// </summary>
        public async Task SerialCleanAsync()
        {
            await SwitchHandleDataAsync(false);
            await SwitchPortAsync(false);
        }

        /// <summary>
        /// 返回指定串口对象的句柄，必须至少打开一次端口才能​​获取此数据
        /// 获得串口句柄，供 Win32 API 使用
        /// </summary>
        /// <param name="sp"></param>
        /// <returns></returns>
        public static IntPtr GetCommHandle(SerialPort sp)
        {
            IntPtr hComm = IntPtr.Zero;
            if (sp != null)
            {
                object stream = typeof(SerialPort).GetField("internalSerialStream", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(sp);
                var handle = (Microsoft.Win32.SafeHandles.SafeFileHandle)stream.GetType().GetField("_handle", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(stream);
                hComm = handle.DangerousGetHandle();
            }
            return hComm;
        }

        /// <summary>
        /// HCI配置：开启/关闭蓝牙串口透传
        /// </summary>
        /// <returns></returns>
        public async Task<(bool, string)> SwitchPassThroughAsync(bool isStart)
        {
            await SwitchReceiveDataAsync(false);
            var fc = HCIinstructions.SwitchPassThroughForSerial;
            var prefix = isStart ? "开启" : "关闭";
            var state = isStart ? RTUDefault.StateOn : RTUDefault.StateOff;
            byte[] bytes = await WriteRegisterThenRecvAckAsync(RTUDefault.ChNumDefault2, fc, state, RTUDefault.HCI_Instruction_Reserve);
            if (bytes == null)
            {
                await SwitchReceiveDataAsync(true);
                return (false, $"【{prefix}蓝牙串口透传】异常，应答结果{nameof(bytes)}为空");
            }
            var dataLen = ToValue<ushort>(bytes, 4, 2);
            if (dataLen != RTUDefault.BluetoothFrameLength)
            {
                await SwitchReceiveDataAsync(true);
                return (false, $"【{prefix}蓝牙串口透传】异常，应答数据长度{dataLen}与要求长度31不匹配！");
            }
            var funCode = ToValue<byte>(bytes, 7);
            Trace.WriteLine($"【{prefix}蓝牙串口透传】成功");
            await SwitchReceiveDataAsync(true);
            return (funCode == fc, null);
        }

        /// <summary>
        /// 开关串口
        /// </summary>
        /// <param name="openOrClose">true表示开/false表示关</param>
        /// <returns></returns>
        public async Task<bool> SwitchPortAsync(bool openOrClose)
        {
            bool isOpened = false;
            try
            {
                if (openOrClose)
                {// 打开
                    if (!serialPort.IsOpen)
                    {
                        serialPort.Open();
                    }
                    else
                    {
                        Trace.WriteLine($"打开[{PortName}]失败!");
                    }
                    if (serialPort.IsOpen)
                    {
                        await ClearSerialBufferAsync();
                        await SwitchReceiveDataAsync(true);
                        LastPortName = PortName;// 记住打开的端口名
                        IsRecived = true;
                        if (isBluetoothToSerial)
                        {
                            await ForceSwitchPassThroughAsync(true);
                        }
                    }
                }
                else
                {// 关闭
                    await SwitchReceiveDataAsync(false);
                    //if (isBluetoothToSerial)
                    //{
                    //    SwitchPassThrough(false);
                    //}
                    if (serialPort.IsOpen)
                    {
                        serialPort.Close();
                    }
                    else
                    {
                        Trace.WriteLine($"关闭[{PortName}]失败!");
                    }
                    if (!serialPort.IsOpen)
                    {
                        IsRecived = false;
                    }
                }
                UpdateComStatus?.Invoke(serialPort.IsOpen, PortName, null);
                isOpened = true;
            }
            catch (Exception ex)
            {// 此处应写入错误日志
                UpdateComStatus?.Invoke(false, PortName, $"打开{PortName}失败");
                UpdateMessage?.Invoke(Color.Red, $"切换串口失败：{ex.Message}");
            }
            return isOpened;
        }

        /// <summary>
        /// 查找第一个可用串口然后打开它
        /// </summary>
        /// <returns></returns>
        public async Task<bool> FindFirstPortThenOpenAsync()
        {
            bool isOpened = await SwitchPortAsync(true);
            if (isOpened)
            {
                portName4Set = PortName;
                //var pointer = GetCommHandle(serialPort);
                //COMMPROP commProp = new COMMPROP();
                //Kernel32.GetCommProperties(pointer, ref commProp);
            }
            else
            {
                string[] ports = SerialPort.GetPortNames();
                foreach (var port in ports)
                {
                    if (port == portName4Set)
                    {
                        continue;
                    }
                    SerialPort.PortName = port;
                    isOpened = await SwitchPortAsync(true);
                    if (isOpened)
                    {
                        UpdateMessage.Invoke(Color.Goldenrod, $"注意当前端口{PortName}并非设定端口{portName4Set}，可能需要重新设置串口");
                        break;
                    }
                }
            }
            return isOpened;
        }

        /// <summary>
        /// HCI配置：扫描从机(让传感器的蓝牙适配器广播自己的名称然后接收它们)
        /// 无线串口(蓝牙转串口)时可用
        /// </summary>
        /// <returns>响应格式(success,message)</returns>
        public async Task<(bool, string)> StartScanAsync()
        {
            await DisconnectAsync(); // 必须先断开当前已有的连接才能发扫描指令
            await SwitchReceiveDataAsync(false);
            var fc = HCIinstructions.ScanSlaveKey;
            // 发送指令：BC AF E5 C8 00 1F 00 E4 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 2B 20 0D 0A
            byte[] bytes = await WriteRegisterThenRecvAckAsync(RTUDefault.ChNumDefault2, fc, RTUDefault.StateOn, RTUDefault.HCI_Instruction_Reserve);
            // 应答数据：BC AF E5 C8 00 1F 00 E4 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 2B 20 0D 0A
            if (bytes == null)
            {
                // 如果收不到应答很可能已开始扫描，此时需要停止扫描重复流程否则会导致流程异常
                await WriteRegisterThenRecvAckAsync(RTUDefault.ChNumDefault2, fc, RTUDefault.StateOff, RTUDefault.HCI_Instruction_Reserve);
                await SwitchReceiveDataAsync(true);
                return (false, $"【扫描从机】异常，应答结果{nameof(bytes)}为空");
            }
            var dataLen = ToValue<ushort>(bytes, 4, 2);
            if (dataLen != RTUDefault.BluetoothFrameLength)
            {
                await SwitchReceiveDataAsync(true);
                return (false, $"【扫描从机】异常，应答数据长度{dataLen}与要求长度31不匹配！");
            }
            var funCode = ToValue<byte>(bytes, 7);
            await SwitchReceiveDataAsync(true);
            return (funCode == fc, null);
        }

        /// <summary>
        /// HCI配置：停止扫描从机
        /// 无线串口(蓝牙转串口)时可用
        /// </summary>
        /// <returns>响应格式(success,message)</returns>
        public async Task<(bool, string)> StopScanAsync()
        {
            await SwitchReceiveDataAsync(false);
            var fc = HCIinstructions.ScanSlaveKey;
            // 发送指令：BC AF E5 C8 00 1F 00 E4 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 16 DC 0D 0A
            byte[] bytes = await WriteRegisterThenRecvAckAsync(RTUDefault.ChNumDefault2, fc, RTUDefault.StateOff, RTUDefault.HCI_Instruction_Reserve);
            // 应答数据：BC AF E5 C8 00 1F 00 E4 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 16 DC 0D 0A 
            if (bytes == null)
            {
                await SwitchReceiveDataAsync(true);
                return (false, $"【停止扫描从机】异常，应答结果{nameof(bytes)}为空");
            }
            var dataLen = ToValue<ushort>(bytes, 4, 2);
            if (dataLen != RTUDefault.BluetoothFrameLength)
            {
                await SwitchReceiveDataAsync(true);
                return (false, $"【停止扫描从机】异常，应答数据长度{dataLen}与要求长度31不匹配！");
            }
            var funCode = ToValue<byte>(bytes, 7);
            await SwitchReceiveDataAsync(true);
            return (funCode == fc, null);
        }

        /// <summary>
        /// HCI配置：连接从机(在传感器上)，当SN=0xFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF时将断开当前连接并启用扫描模式
        /// 无线串口(蓝牙转串口)时可用
        /// </summary>
        /// <param name="uart"></param>
        /// <param name="sn">传感器的SN</param>
        /// <returns>响应格式(success,message)</returns>
        public Task<(bool, string)> ConnectAsync(string sn)
        {
            var input = Encoding.Default.GetBytes(sn).PadRight(16);
            //input = input.Reverse().ToArray();// 在拼凑发送指令的地方会反转无需此处提前反转
            return ConnectAsync(input);
        }

        /// <summary>
        /// HCI配置：连接从机(在传感器上)，当SN=0xFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF时将断开当前连接并启用扫描模式
        /// 无线串口(蓝牙转串口)时可用
        /// </summary>
        /// <param name="sn">传感器的SN</param>
        /// <returns>响应格式(success,message)</returns>
        public async Task<(bool, string)> ConnectAsync(byte[] sn)
        {
            var isConnect = sn.Equals(RTUDefault.InvalidDeviceName);
            var stateText = isConnect ? "连接" : "断开";
            await SwitchReceiveDataAsync(false);
            var fc = HCIinstructions.PerformSwitchSlave;
            byte[] bytes = await WriteRegisterThenRecvAckAsync(RTUDefault.ChNumDefault2, fc, sn, RTUDefault.HCI_Instruction_Reserve);
            if (bytes == null)
            {
                await SwitchReceiveDataAsync(true);
                return (false, $"【{stateText}从机】异常，应答结果{nameof(bytes)}为空");
            }
            var dataLen = ToValue<ushort>(bytes, 4, 2);
            if (dataLen != RTUDefault.BluetoothFrameLength)
            {
                await SwitchReceiveDataAsync(true);
                return (false, $"【{stateText}从机】异常，应答数据长度{dataLen}与要求长度31不匹配！");
            }
            var funCode = ToValue<byte>(bytes, 7);
            var success = funCode == fc;
            if (success)
            {
                if (OnConnectChanged != null)
                {
                    var dce = new DeviceChangedEventArgs(isConnect, false);
                    OnConnectChanged.Invoke(SerialPort, dce);
                }

                _ = ReceiveAdvertisementDataAsync().ConfigureAwait(false);
            }
            else
            {
                return (false, $"【{stateText}从机】异常，接收数据的功能码{funCode}与发送数据的功能码{fc}不匹配！");
            }
            await SwitchReceiveDataAsync(true);
            return (success, null);
        }

        /// <summary>
        /// HCI配置：断开对从机(在传感器上)的连接
        /// 无线串口(蓝牙转串口)时可用
        /// </summary>
        /// <returns></returns>
        public async Task<(bool, string)> DisconnectAsync()
        {
            return await ConnectAsync(RTUDefault.InvalidDeviceName);
        }

        /// <summary>
        /// HCI配置：读取当前调平晶圆的SN
        /// </summary>
        /// <param name="uart"></param>
        /// <returns>响应格式(success, message/sn)</returns>
        public async Task<(bool, string)> GetSlaveSNAsync()
        {
            await SwitchReceiveDataAsync(false);
            var fc = HCIinstructions.ReadSlaveKey;
            byte[] bytes = await WriteRegisterThenRecvAckAsync(RTUDefault.ChNumDefault2, fc, RTUDefault.StateOff, RTUDefault.HCI_Instruction_Reserve);
            if (bytes == null)
            {
                await SwitchReceiveDataAsync(true);
                return (false, $"【读从机SN】异常，应答结果{nameof(bytes)}为空");
            }
            //var dataLen = ToValue<ushort>(bytes, 4, 2);
            //if (dataLen != RTUDefault.BluetoothFrameLength)
            //{
            //    SwitchReceiveData(true);
            //    return (false, $"【读从机SN】异常，应答数据长度{dataLen}与要求长度31不匹配！", null);
            //}
            var funCode = ToValue<byte>(bytes, 7);
            //var ackVal = ToValue<string>(bytes, 8, 16);
            var ackVal = bytes.ToStringByEncoding(8, 16);
            if (!string.IsNullOrWhiteSpace(ackVal))
            {
                ackVal = ackVal.Trim('\0');
            }
            await SwitchReceiveDataAsync(true);
            return (funCode == fc, ackVal);
        }

        /// <summary>
        /// HCI配置：修改主从机通信的信道号
        /// </summary>
        /// <param name="channelNo">新的信道号</param>
        /// <returns></returns>
        public async Task<(bool, string)> ChangeBothChannelNo(int channelNo)
        {
            await SwitchReceiveDataAsync(false);
            var fc = HCIinstructions.ChangeHostAndSlaveChannelNo;
            var chanNum = Convert.ToByte(channelNo);
            byte[] bytes = await WriteRegisterThenRecvAckAsync(RTUDefault.ChNumDefault2, fc, chanNum);
            if (bytes == null)
            {
                await SwitchReceiveDataAsync(true);
                return (false, $"【修改主从机的信道号】异常，应答结果{nameof(bytes)}为空");
            }
            var dataLen = bytes.ToUShort(4);
            if (dataLen < RTUDefault.FrameLength)
            {
                await SwitchReceiveDataAsync(true);
                return (false, $"【修改主机的信道号】异常，应答数据长度{dataLen}小于最短帧长度{RTUDefault.FrameLength}！");
            }
            var funCode = bytes[7];
            if (funCode != HCIinstructions.ChangeHostChannelNo)
            {
                await SwitchReceiveDataAsync(true);
                return (false, $"【修改主从机的信道号】异常，应答结果功能码{funCode}不是预期值");
            }
            var chanNo = bytes[8];
            if (chanNo < 1 || chanNo > 255)
            {
                await SwitchReceiveDataAsync(true);
                return (false, $"【修改主从机的信道号】异常，应答结果{nameof(chanNo)}超出范围");
            }
            await SwitchReceiveDataAsync(true);
            return (true, chanNo.ToString());
        }

        /// <summary>
        /// HCI配置：修改主机的信道号
        /// </summary>
        /// <param name="channelNo">新的信道号</param>
        /// <returns></returns>
        public async Task<(bool, string)> ChangeHostChannelNo(int channelNo)
        {
            await SwitchReceiveDataAsync(false);
            var fc = HCIinstructions.ChangeHostChannelNo;
            var chanNum = Convert.ToByte(channelNo);
            byte[] bytes = await WriteRegisterThenRecvAckAsync(RTUDefault.ChNumDefault2, fc, chanNum, RTUDefault.HCI_Instruction_Reserve);
            if (bytes == null)
            {
                await SwitchReceiveDataAsync(true);
                return (false, $"【修改主机的信道号】异常，应答结果{nameof(bytes)}为空");
            }
            var dataLen = bytes.ToUShort(4);
            if (dataLen < RTUDefault.FrameLength)
            {
                await SwitchReceiveDataAsync(true);
                return (false, $"【修改主机的信道号】异常，应答数据长度{dataLen}小于最短帧长度{RTUDefault.FrameLength}！");
            }
            var funCode = bytes[7];
            if (funCode != HCIinstructions.ChangeHostChannelNo)
            {
                await SwitchReceiveDataAsync(true);
                return (false, $"【修改主机的信道号】异常，应答结果功能码{funCode}不是预期值");
            }
            var chanNo = bytes[8];
            if (chanNo < 1 || chanNo > 255)
            {
                await SwitchReceiveDataAsync(true);
                return (false, $"【修改主机的信道号】异常，应答结果{nameof(chanNo)}超出范围");
            }

            await SwitchReceiveDataAsync(true);
            return (true, chanNo.ToString());
        }

        /// <summary>
        /// 清空接收队列
        /// </summary>
        public Task ClearQueueAsync()
        {
            return Task.Run(() =>
            {
#if NET5_0_OR_GREATER
                rqRawData.Clear();
#else
                rqRawData = new ConcurrentQueue<byte>();
#endif
            });
        }

        /// <summary>
        /// 清空窗口缓冲区
        /// </summary>
        public async Task ClearSerialBufferAsync()
        {
            await ClearQueueAsync();
            if (serialPort.IsOpen)
            {
                serialPort.DiscardInBuffer();
                serialPort.DiscardOutBuffer();
            }
        }

        /// <summary>
        /// 切换到接收蓝牙配置指令
        /// </summary>
        public async Task SwitchRecvBtCfgDataAsync()
        {
            frameLength = RTUDefault.BluetoothFrameLength;
            await SwitchHandleDataAsync(false);// 暂停广播数据解析线程
            await ClearSerialBufferAsync();
        }

        /// <summary>
        /// 切换到接收广播数据
        /// </summary>
        public async Task SwitchRecvNormalDataAsync()
        {
            if (isBluetoothToSerial)
            {
                await ForceSwitchPassThroughAsync(true);
            }

            frameLength = initFrameLength;
            await ClearSerialBufferAsync();
            await SwitchHandleDataAsync(true);// 开始广播数据解析线程
        }

        /// <summary>
        /// 接收串口广播数据
        /// </summary>
        /// <returns></returns>
        public override Task ReceiveAdvertisementDataAsync()
        {
            OnHandleAdvertisementData?.Invoke(SerialPort);
            return Task.Factory.StartNew(async () =>
            {
                while (true)
                {
                    if (!PauseReceiveData.SafeWaitHandle.IsClosed)
                    {
                        if (PauseReceiveData.WaitOne())
                        {
                            //ClearSerialBuffer();
                        }
                    }

                    //timer.Restart();
                    //Logger.Trace($"串口数据接收=>len=【{len}】");
                    try
                    {
                        if (!serialPort.IsOpen)
                        {
                            IsRecived = false;
                            continue;
                        }

                        int recvLen = serialPort.BytesToRead;
                        if (recvLen == 0)
                        {
                            if (ReceivedCount == RTUDefault.ThresholdTimes)
                            {
                                IsRecived = false;
                                OnCommunicateStateChanged?.Invoke(IsRecived);
                                UpdateMessage?.Invoke(Color.Red, "未接收到数据");
                                //await SwitchReceiveDataAsync(false);
                                //await SwitchHandleDataAsync(false);
                                ReceivedCount++;
                            }
                            else if (ReceivedCount != RTUDefault.ThresholdTimes + 1)
                            {
                                ReceivedCount++;
                            }
                            await Task.Delay(5);
                            continue;
                        }
                        if (ReceivedCount == RTUDefault.ThresholdTimes + 1)
                        {
                            //var message = $"设备已重新上电并打开，可能需要刷新数据";
                            UpdateMessage?.Invoke(Color.Green, "");
                            //await SwitchReceiveDataAsync(true);
                            //await SwitchHandleDataAsync(true);
                        }
                        ReceivedCount = 0;
                        IsRecived = true;
                        OnCommunicateStateChanged?.Invoke(IsRecived);
                        //Trace.WriteLine($"串口数据接收=>len=【{len}】");
                        var dataRe = new byte[recvLen];
                        int dataLen = serialPort.Read(dataRe, 0, recvLen);

                        //Trace.WriteLine($"串口数据接收=>dataLen=【{dataLen}】");

                        for (int i = 0; i < dataLen; i++)
                        {
                            rqRawData.Enqueue(dataRe[i]);
                        }
                        //Trace.WriteLine($"串口数据接收=>单次接收结束");
                        OnReceived?.Invoke(SerialPort, dataRe);
                    }
                    catch (Exception ex)
                    {
                        string message = $"接收串口数据发生异常：{ex.Message}如果是因为串口被拔出请重新插回！".Replace("\r\n", "");
                        logger.Fatal(message, ex);
                        //UpdateComStatus?.Invoke(false, PortName, message);
                    }
                    //timer.Stop();
                    //var timeSpan = sw.ElapsedMilliseconds;
                    //Trace.WriteLine($"串口数据接收=>本次接收耗时:{timeSpan}毫秒");
                }
            }, TaskCreationOptions.LongRunning);
        }

        /// <summary>
        /// 从数据队列中取出新一帧
        /// </summary>
        /// <returns>正确的单帧数据</returns>
        public byte[] GetNewFrame(byte funCode, bool isCancell = false)
        {
            Monitor.Enter(SyncRoot);
            FrameLength = RTUDefault.HCI_Instruction_FunCodeTab.Contains(funCode) ? RTUDefault.BluetoothFrameLength : initFrameLength;

            byte[] recvFrame = null;
            var iterationLength = FixedPrefix.BytesLength;
            List<byte> affixData = new List<byte>();// 缓存词缀(固定帧头/帧尾)专用
            List<byte> recvData = new List<byte>();
            while (!isCancell)
            {
                while (affixData.Count < iterationLength)// 这个while循环耗时十几ms到一百ms左右
                {// 接收数据长度不足接收数据帧长度则继续从队列中拉取
                    if (rqRawData.IsEmpty)
                    {
                        Thread.Sleep(5);
                        //Trace.WriteLine($"串口数据处理=>0-recvDataQueue.Count=【{recvDataQueue.Count}】");
                        continue;
                    }

                    if (rqRawData.TryDequeue(out byte item))
                    {
                        affixData.Add(item);
                        recvData.Add(item);
                    }
                }

                if (affixData.ToArray().IsEqual(FixedPrefix.Bytes))
                {// 词缀为固定帧头时
                    affixData.Clear();
                    iterationLength = FixedSuffix.BytesLength;// 重设迭代长度找固定帧尾
                    var removeCount = recvData.Count - FixedPrefix.BytesLength;// 计算recvData中需在固定帧头前移除的数据量
                    recvData.RemoveRange(0, removeCount);// 移除recvData中固定帧头前的数据
                    continue;
                }
                else if (affixData.ToArray().IsEqual(FixedSuffix.Bytes))
                {// 词缀为固定帧尾时
                    affixData.Clear();
                    iterationLength = FixedPrefix.BytesLength;// 重设迭代长度找固定帧头
                }
                else
                {// 不是词缀时
                    affixData.RemoveAt(0);
                    continue;
                }

                (int code, byte[] ackData) = GetAckDataFromBuffer(recvData, funCode);// 取出应答数据
                if (code == 0)
                {
                    recvFrame = ackData;
                }
                break;
            }
            Monitor.Exit(SyncRoot);
            return recvFrame;
        }

        /// <summary>
        /// 从数据队列中取出新一帧
        /// </summary>
        /// <returns>正确的单帧数据</returns>
        public async Task<byte[]> GetNewFrameAsync(byte funCode, bool isCancell = false)
        {
            FrameLength = RTUDefault.HCI_Instruction_FunCodeTab.Contains(funCode) ? RTUDefault.BluetoothFrameLength : initFrameLength;
            byte[] recvFrame = null;
            var iterationLength = FixedPrefix.BytesLength;
            List<byte> affixData = new List<byte>();// 缓存词缀(固定帧头/帧尾)专用
            List<byte> recvData = new List<byte>();
            while (!isCancell)
            {
                while (affixData.Count < iterationLength)// 这个while循环耗时十几ms到一百ms左右
                {// 接收数据长度不足接收数据帧长度则继续从队列中拉取
                    if (rqRawData.IsEmpty)
                    {
                        //Thread.Sleep(5);
                        await Task.Delay(5);
                        //Trace.WriteLine($"串口数据处理=>0-recvDataQueue.Count=【{recvDataQueue.Count}】");
                        continue;
                    }

                    if (rqRawData.TryDequeue(out byte item))
                    {
                        affixData.Add(item);
                        recvData.Add(item);
                    }
                }

                if (affixData.ToArray().IsEqual(FixedPrefix.Bytes))
                {// 词缀为固定帧头时
                    affixData.Clear();
                    iterationLength = FixedSuffix.BytesLength;// 重设迭代长度找固定帧尾
                    var removeCount = recvData.Count - FixedPrefix.BytesLength;// 计算recvData中需在固定帧头前移除的数据量
                    recvData.RemoveRange(0, removeCount);// 移除recvData中固定帧头前的数据
                    continue;
                }
                else if (affixData.ToArray().IsEqual(FixedSuffix.Bytes))
                {// 词缀为固定帧尾时
                    affixData.Clear();
                    iterationLength = FixedPrefix.BytesLength;// 重设迭代长度找固定帧头
                }
                else
                {// 不是词缀时
                    affixData.RemoveAt(0);
                    continue;
                }

                (int code, byte[] ackData) = GetAckDataFromBuffer(recvData, funCode);// 取出应答数据
                if (code == 0)
                {
                    recvFrame = ackData;
                }
                break;
            }
            return recvFrame;
        }

        ///// <summary>
        ///// 发送无需应答的指令(异步地)
        ///// </summary>
        ///// <param name="chNum">通道号，可为空</param>
        ///// <param name="funCode">功能码</param>
        ///// <param name="value">发送值，文本内容请事先转换为byte数组以防止转码不通用的问题</param>
        ///// <returns>true表示发送成功，false表示发送失败</returns>
        //public override Task<bool> WriteRegisterAsync<T>(byte? chNum, byte funCode, params T[] values)
        //{
        //    return Task.Run(() =>
        //    {
        //        //协议：帧头(集迦) + 指令长度 + 功能码 + 寄存器值 + crc16 + 帧尾(回车换行)
        //        //字节数：4       + 2       + 1     + ?       +  2    + 2
        //        bool ret = false;
        //        try
        //        {
        //            byte[] sendBuffer = GetSendContent(chNum, funCode, values);

        //            if (serialPort.IsOpen)
        //            {
        //                var rawHexStr = sendBuffer.ToHexString();
        //                Trace.WriteLine($"【WriteRegister】发送指令：{rawHexStr}");
        //                //serialPort.DiscardOutBuffer();// 清空串口发送缓冲区的所有数据
        //                serialPort.Write(sendBuffer, 0, sendBuffer.Length);
        //                ret = true;
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            Trace.WriteLine($"执行【WriteRegisterAsync】发生异常：{ex.Message}");
        //        }
        //        return ret;
        //    });
        //}

        ///// <summary>
        ///// 发送需应答的指令(异步地)
        ///// </summary>
        ///// <param name="chNum">通道号，可为空</param>
        ///// <param name="funCode">功能码</param>
        ///// <param name="value">发送值，文本内容请事先转换为byte数组以防止转码不通用的问题</param>
        ///// <returns>返回下位机响应的数据流</returns>
        //public override async Task<byte[]> WriteRegisterThenRecvAckAsync<T>(byte? chNum, byte funCode, params T[] values)
        //{
        //    //协议：帧头(集迦) + 指令长度 + 功能码 + 寄存器值 + crc16 + 帧尾(回车换行)
        //    //字节数：4       + 2       + 1     + ?       +  2    + 2
        //    byte[] recBuffer = null;
        //    try
        //    {
        //        byte[] sendBuffer = GetSendContent(chNum, funCode, values);
        //        if (serialPort.IsOpen)
        //        {
        //            var recvData = await SendDataRecAsync(sendBuffer);
        //            if (recvData != null)
        //            {
        //                if (recvData.ToArray() == sendBuffer)
        //                {
        //                    recBuffer = recvData.ToArray();
        //                }
        //                else
        //                {
        //                    (int code, byte[] ackData) = GetAckDataFromBuffer(recvData, funCode);// 取出应答数据
        //                    if (code == 0)
        //                    {
        //                        recBuffer = ackData;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch
        //    {
        //        Trace.WriteLine($"执行WriteRegisterThenRecvAck<T>异常");
        //        throw;
        //    }
        //    return recBuffer;
        //}

        /// <summary>
        /// 发送无需应答的指令(异步地)
        /// </summary>
        /// <param name="chNum">通道号，可为空</param>
        /// <param name="funCode">功能码</param>
        /// <param name="value">发送值，文本内容请事先转换为byte数组以防止转码不通用的问题</param>
        /// <returns>true表示发送成功，false表示发送失败</returns>
        public override Task<bool> WriteRegisterAsync(byte? chNum, byte funCode, params dynamic[] values)
        {
            //协议：帧头(集迦) + 指令长度 + 功能码 + 寄存器值 + crc16 + 帧尾(回车换行)
            //字节数：4       + 2       + 1     + ?       +  2    + 2
            bool ret = false;
            try
            {
                byte channelNo = chNum.HasValue ? chNum.Value : RTUDefault.ChNum;
                byte[] sendBuffer = GetSendContent(funCode, values, channelNo);
#if DEBUG
                //var rawHexStr = StringLib.GetHexStringFromByteArray(sendbuffer);
                var rawHexStr = sendBuffer.ToHexString();
#endif
                if (serialPort.IsOpen)
                {
                    //serialPort.DiscardOutBuffer();// 清空串口发送缓冲区的所有数据
                    serialPort.Write(sendBuffer, 0, sendBuffer.Length);
#if DEBUG
                    logger.Trace($"发送指令：{rawHexStr}");
#endif
                    ret = true;
                }
            }
            catch
            {
                throw;
            }
            return Task.FromResult(ret);
        }

        /// <summary>
        /// 发送需应答的指令(异步地)
        /// </summary>
        /// <param name="chNum">通道号，可为空</param>
        /// <param name="funCode">功能码</param>
        /// <param name="value">发送值，文本内容请事先转换为byte数组以防止转码不通用的问题</param>
        /// <returns>返回下位机响应的数据流</returns>
        public override async Task<byte[]> WriteRegisterThenRecvAckAsync(byte? chNum, byte funCode, params dynamic[] values)
        {
            //协议：帧头(集迦) + 指令长度 + 功能码 + 寄存器值 + crc16 + 帧尾(回车换行)
            //字节数：4       + 2       + 1     + ?       +  2    + 2
            byte[] recBuffer = null;
            try
            {
                byte channelNo = chNum.HasValue ? chNum.Value : RTUDefault.ChNum;
                byte[] sendBuffer = GetSendContent(funCode, values, channelNo);
                if (serialPort.IsOpen)
                {
                    var recvData = await SendDataRecAsync(sendBuffer);
                    if (recvData != null)
                    {
                        if (recvData.ToArray() == sendBuffer)
                        {
                            recBuffer = recvData.ToArray();
                        }
                        else
                        {
                            (int code, byte[] ackData) = GetAckDataFromBuffer(recvData, funCode);// 取出应答数据
                            if (code == 0)
                            {
                                recBuffer = ackData;
                            }
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                await ClearSerialBufferAsync();
            }
            return recBuffer;
        }

        /// <summary>
        /// 接收到的数据转换为指定的常用类型的数据
        /// </summary>
        /// <typeparam name="T">指定的常用类型的数据</typeparam>
        /// <param name="bytes">接收到的数据</param>
        /// <returns></returns>
        public T ToValue<T>(byte[] bytes, int srcOffset = 7, int count = 1)
        {
            dynamic val = default(T);
            try
            {
                if (bytes == null)
                {
                    throw new ArgumentNullException(nameof(bytes));
                }
                else if (bytes.Length < MinimumInstructionLength)
                {
                    throw new ArgumentException($"{nameof(bytes)}长度不能小于{MinimumInstructionLength}");
                }
                var data = new byte[count];
                Buffer.BlockCopy(bytes, srcOffset, data, 0, count);
                val = data.ToValue<T>(0, count);
            }
            catch
            {
                throw;
            }
            return val;
        }

        /// <summary>
        /// 获取当前帧数据中的有效值长度
        /// </summary>
        /// <param name="frameLen">当前帧数据长度</param>
        /// <param name="cropLen">额外裁剪长度</param>
        /// <param name="funCode">功能码</param>
        /// <returns></returns>
        public int GetValidDataLength(int frameLen, int cropLen, byte funCode = 0xFF)
        {
            if (frameLen == 0)
            {
                return 0;
            }
            // 应该减去的长度：固定帧头(4字节) + 帧长度(2字节) + 功能码(1字节) + CRC校验值(2字节) + 固定帧尾(2字节) + 额外裁剪长度(cropLen)
            var len = frameLen - (4 + 2 + 1 + 2 + 2 + cropLen);
            if (RTUDefault.HCI_Instruction_FunCodeTab.Contains(funCode))
            {
                len -= 1;// 广播地址(1字节)
            }
            return len;
        }
        #endregion 公开方法 end

        #region    私有方法 start
        /// <summary>
        /// 异步发送并且接收字节数组
        /// </summary>
        /// <param name="sendbuffer"></param>
        /// <param name="recbuffer"></param>
        /// <returns></returns>
        public async Task<List<byte>> SendDataRecAsync(byte[] sendBuffer)
        {
            //return Task.Run(async () =>
            //{
            var total = new List<byte>();
            try
            {
                // 发送数据
                serialPort.Write(sendBuffer, 0, sendBuffer.Length);

                var rawHexStr = sendBuffer.ToHexString();
                Trace.WriteLine($"【SendDataRecAsync】发送指令：{rawHexStr}");

                //await Task.Delay(10);
                byte[] buffer = null;
                int counts = 0;
                DateTime startTime = DateTime.Now;
                while (true)
                {
                    if (serialPort.BytesToRead > 0)
                    {
                        int len = serialPort.BytesToRead;
                        buffer = new byte[len];
                        counts = serialPort.Read(buffer, 0, len);
                        if (counts > 0)
                        {
                            total.AddRange(buffer);
                        }
                    }
                    else
                    {
                        await Task.Delay(1);
                        if ((DateTime.Now - startTime).TotalMilliseconds > RTUDefault.ReciveTimeout)
                        {
                            if (counts == 0)
                            {// 超时
                                Trace.WriteLine("【SendDataRecAsync】接收应答超时");
                            }
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"执行【SendDataRecAsync】发生异常：{ex.Message}");
            }

            if (total.Count > 0)
            {
                var rawHexStr = total.ToArray().ToHexString();
                logger.Trace($"【SendDataRecAsync】接收的应答数据：{rawHexStr}");
            }
            return total;
            //});
        }

        /// <summary>
        /// 强制开启透传，否则无法接收串口数据
        /// </summary>
        private async Task ForceSwitchPassThroughAsync(bool isStart)
        {
        restart:
            (bool success, string msg) = await SwitchPassThroughAsync(isStart);
            if (!success)
            {
                //Trace.WriteLine(msg);
                logger.Error(msg);
                //Thread.Sleep(500);
                await Task.Delay(500);
                goto restart;
            }
        }

        /// <summary>
        /// 添加USB事件监视器
        /// </summary>
        /// <param name="usbInsertHandler">USB插入事件处理器</param>
        /// <param name="usbRemoveHandler">USB拔出事件处理器</param>
        /// <param name="withinInterval">发送通知允许的滞后时间</param>
        public bool USBEventWatcher(EventArrivedEventHandler usbInsertHandler, EventArrivedEventHandler usbRemoveHandler, TimeSpan withinInterval)
        {
            try
            {
                ManagementScope Scope = new ManagementScope("root\\CIMV2");
                Scope.Options.EnablePrivileges = true;

                // USB插入监视
                if (usbInsertHandler != null)
                {//Win32_USBControllerDevice
                    var InsertQuery = new WqlEventQuery("__InstanceCreationEvent", withinInterval, "TargetInstance isa 'Win32_PnPEntity'");

                    insertWatcher = new ManagementEventWatcher(Scope, InsertQuery);
                    insertWatcher.EventArrived += usbInsertHandler;
                    insertWatcher.Start();
                }
                // USB拔出监视
                if (usbRemoveHandler != null)
                {
                    var RemoveQuery = new WqlEventQuery("__InstanceDeletionEvent", withinInterval, "TargetInstance isa 'Win32_PnPEntity'");

                    removeWatcher = new ManagementEventWatcher(Scope, RemoveQuery);
                    removeWatcher.EventArrived += usbRemoveHandler;
                    removeWatcher.Start();
                }
                return true;
            }
            catch (Exception)
            {
                RemoveUSBEventWatcher();
                return false;
            }
        }

        /// <summary>
        /// 移去USB事件监视器
        /// </summary>
        public void RemoveUSBEventWatcher()
        {
            if (insertWatcher != null)
            {
                insertWatcher.Stop();
                insertWatcher = null;
            }

            if (removeWatcher != null)
            {
                removeWatcher.Stop();
                removeWatcher = null;
            }
        }

        /// <summary>
        /// USB设备插拔时处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void USBEventHandler(object sender, EventArrivedEventArgs e)
        {
            ManagementBaseObject targetInstance = (ManagementBaseObject)e.NewEvent["TargetInstance"];
            var caption = targetInstance.GetPropertyValue("Caption")?.ToString();
            var regExp = new Regex("Serial.*\\(COM\\d+\\)", RegexOptions.IgnoreCase);
            if (!regExp.IsMatch(caption))
            {// 非USB串口设备不处理
                return;
            }

            var watcher = sender as ManagementEventWatcher;
            watcher.Stop();

            regExp = new Regex("COM\\d+", RegexOptions.IgnoreCase);
            var devPort = regExp.Match(caption).Value;
            var portNotChanged = false;
            if (e.NewEvent.ClassPath.ClassName == "__InstanceCreationEvent")
            {
                if (devPort == PortName)
                {// 插入设备是之前拔出的串口
                    serialPort.Open();
                    //var message = $"设备({PortName})已重新连接并打开，可能需要刷新数据";
                    portNotChanged = true;
                }

                var dce = new DeviceChangedEventArgs(true, portNotChanged);
                OnDeviceChanged?.Invoke(devPort, dce);
            }
            else if (e.NewEvent.ClassPath.ClassName == "__InstanceDeletionEvent")
            {
                if (devPort == PortName)
                {// 拔出设备是当前正在使用的串口
                 //var message = $"当前设备({PortName})已被拔出，等待重新连接";
                    portNotChanged = true;
                }

                var dce = new DeviceChangedEventArgs(false, portNotChanged);
                OnDeviceChanged?.Invoke(devPort, dce);
            }

            // 业务代码，逻辑耗时尽量不要太长，以免影响事件的监听
            watcher.Start();
        }
        #endregion 私有方法 end
    }
}