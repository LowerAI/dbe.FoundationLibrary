using dbe.FoundationLibrary.Core.Common;
using dbe.FoundationLibrary.Core.Extensions;

using InTheHand.Bluetooth;
using InTheHand.Net;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace dbe.FoundationLibrary.Communication.RTUComm
{
    /// <summary>
    /// 低功耗蓝牙(Bluetooth Low Energy)通信服务类
    /// </summary>
    public class BluetoothLowEnergyService : RTUCore//, IStateObject                                       
    {
        #region    字段 start
        private readonly List<BluetoothDevice> BLEDeviceList;// 低功耗蓝牙设备列表
        private readonly List<BluetoothDeviceInfo> DeviceInfoList;// 蓝牙设备信息列表
        private ConcurrentQueue<byte[]> rqRawData;// 存储接收数据的队列
        private GattCharacteristic nusWriteChtt;// 写数据的特征;
        private GattCharacteristic nusReadChtt;// 读数据的特征;

        //private bool isStopScanBTDevice;// 是否停止搜索经典蓝牙设备
        private BluetoothLEScan bleScan;// ble扫描器

        private RemoteGattServer remoteGattServer;// 已连接的BLE设备
        private IReadOnlyList<GattCharacteristic> chttLst;// 通信特性的集合
        //private long ReceivedCount = 0;// 收到数据次数
        //private System.Timers.Timer timerSpeedTest;// 测试帧率的定时器
        #endregion 字段 end

        #region    属性 start
        /// <summary>
        /// 通信模式
        /// </summary>
        public CommunicationMode CommunicationMode => CommunicationMode.Bluetooth;
        /// <summary>
        /// 蓝牙连接方式
        /// </summary>
        public BluetoothCommunicationType ConnectionMode => BluetoothCommunicationType.BluetoothLowEnergy;

        /// <summary>
        /// 蓝牙适配器是否可用
        /// </summary>
        public bool IsRadioReady { get; private set; }

        /// <summary>
        /// 主蓝牙适配器
        /// </summary>
        public BluetoothRadio PrimaryRadio { get; private set; }

        /// <summary>
        /// 蓝牙客户端是否已连接
        /// </summary>
        public bool IsConnected { get; private set; } = false;

        /// <summary>
        /// BLE设备列表
        /// </summary>
        public List<BluetoothLEDevice> BleDeviceList { get; set; } = new List<BluetoothLEDevice>();

        /// <summary>
        /// BLE客户端信息列表
        /// </summary>
        //public List<(string, short, bool)> BleClientInfoList { get; set; } = new List<(string, short, bool)>();
        /// <summary>
        /// 本地终结点
        /// </summary>
        //public BluetoothEndPoint LocalEndpoint { get; private set; }

        /// <summary>
        /// 是否漏号重传
        /// </summary>
        public bool IsResendAfterMissed { get; set; }
        #endregion 属性 end

        #region    结构体 start
        public struct SerialPort
        {
            public string Port;
            public string DeviceId;
        }
        #endregion 结构体 end

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
        /// 检测到(BLE)设备连接/断开时
        /// </summary>
        /// <param name="device">连接/断开的Ble设备</param>
        /// <param name="args">设备变更事件参数</param>
        public delegate void BLEConnectChangedEvent(BluetoothDevice device, DeviceChangedEventArgs args);
        /// <summary>
        /// BLE设备拔插事件
        /// </summary>
        public event BLEConnectChangedEvent OnConnectChanged;

        /// <summary>
        /// 检测到广播数据时开启处理
        /// </summary>
        /// <param name="deviceInfo"></param>
        public delegate void HandleAdvertisementDataEvent(BluetoothLowEnergyService service);
        /// <summary>
        /// 处理广播数据的事件，交互式通信时不可用
        /// </summary>
        public event HandleAdvertisementDataEvent OnHandleAdvertisementData;

        /// <summary>
        /// 检测到蓝牙适配器打开或者关闭时
        /// </summary>
        /// <param name="isReady">true表示打开/false表示断开</param>
        public delegate void RadioStateChangedEvent(bool isReady);
        /// <summary>
        /// 反馈蓝牙适配器状态的事件
        /// </summary>
        public event RadioStateChangedEvent OnRadioStateChanged;

        /// <summary>
        /// 接收到数据时
        /// </summary>
        /// <param name="device"></param>
        public delegate void ReceivedEvent(GattCharacteristic nusReadChtt, byte[] data);
        /// <summary>
        /// 接收到数据的事件
        /// </summary>
        public event ReceivedEvent OnReceived;

        /// <summary>
        /// 每一次轮询后更新蓝牙设备
        /// </summary>
        /// <param name="infoLst"></param>
        /// <param name="deviceInfoLst"></param>
        public delegate void UpdateBLEDevciceEvent(List<BluetoothLEDevice> deviceLst, BluetoothLEDevice bleDevice);
        /// <summary>
        /// 更新搜索到的低功耗蓝牙设备的事件
        /// </summary>
        public event UpdateBLEDevciceEvent OnUpdateBleDevcice;
        #endregion 事件声明 start

        #region    构造与析构 start
        public BluetoothLowEnergyService(InstructionFormat instructionFormat) : base(instructionFormat)
        {
            rqRawData = new ConcurrentQueue<byte[]>();
            //timerSpeedTest = new System.Timers.Timer(5000);
            //timerSpeedTest.AutoReset = true;
            //timerSpeedTest.Enabled = true;
            //timerSpeedTest.Elapsed += TimerSpeedTest_Elapsed;

            try
            {
                PrimaryRadio = BluetoothRadio.Default;
                IsRadioReady = true;
            }
            catch (Exception)
            {
                IsRadioReady = false;
            }

            BLEDeviceList = new List<BluetoothDevice>();
            DeviceInfoList = new List<BluetoothDeviceInfo>();

            var osVersion = RTUDefault.GetOSVersion();
            if (osVersion == OSVersion.Windows11)
            {
                Bluetooth.AvailabilityChanged += Bluetooth_AvailabilityChanged;
            }

            Bluetooth.AdvertisementReceived += Bluetooth_AdvertisementReceived;

            //if (IsRadioReady)
            //{
            //    var btAddr = PrimaryRadio.LocalAddress;
            //    LocalEndpoint = new BluetoothEndPoint(btAddr, RTUDefault.ServiceGuid);
            //}
        }
        #endregion 构造与析构 end

        #region    事件处理 start
        // 蓝牙适配器打开/关闭事件
        private async void Bluetooth_AvailabilityChanged(object sender, EventArgs e)
        {
            PrimaryRadio = BluetoothRadio.Default;// 重新赋值防止切换到了新的适配器
            IsRadioReady = await Bluetooth.GetAvailabilityAsync();
            //Trace.WriteLine(IsRadioReady ? "蓝牙已打开" : "蓝牙已关闭");
            OnRadioStateChanged?.Invoke(IsRadioReady);
        }

        // 测试蓝牙速率
        private void TimerSpeedTest_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            var rate = rqRawData.Count / 5f;
            Trace.WriteLine($"当前速率：{rate}帧/秒");
            rqRawData = new ConcurrentQueue<byte[]>();
        }

        // 接收BLE广播客户端事件 => 获取BLE设备列表
        private void Bluetooth_AdvertisementReceived(object sender, BluetoothAdvertisingEvent e)
        {
            if (e.Device != null)
            {
                var dev = e.Device;
                //dev.GattServerDisconnected += Dev_GattServerDisconnected;
                if (BLEDeviceList.Count > 0)
                {
                    if (!BLEDeviceList.Exists(device => device.Id == e.Device.Id))
                    {
                        BLEDeviceList.Add(dev);
                    }
                }
                //var devName = string.IsNullOrWhiteSpace(dev.Name) ? "Unknown" : dev.Name;
                //var devStr = $"{dev.Id}({devName})";

                var bleDevice = new BluetoothLEDevice
                {
                    Id = dev.Id,
                    Name = dev.Name,
                    IsPaired = dev.IsPaired,
                    Appearance = e.Appearance,
                    TxPower = e.TxPower,
                    Rssi = e.Rssi
                };

                if (BleDeviceList.Count > 0)
                {
                    var item = BleDeviceList.Find(device => device?.DevStr == bleDevice.DevStr);
                    if (item == null)
                    {// DevStr不同时添加新项
                        BleDeviceList.Add(bleDevice);
                    }
                    else if (item != bleDevice)
                    {// DevStr相同时通过赋值更新
                        bleDevice.Clone(item);
                    }
                }
                else
                {
                    BleDeviceList.Add(bleDevice);
                }

                OnUpdateBleDevcice?.Invoke(BleDeviceList, bleDevice);
                //OnUpdateBleDevcice?.Invoke(e, bleDevice);
            }
        }

        // BLE设备断开连接事件
        private void Dev_GattServerDisconnected(object sender, EventArgs e)
        {
            IsConnected = false;
            Trace.WriteLine("Ble设备断开了");
            if (OnConnectChanged != null)
            {
                var dce = new DeviceChangedEventArgs(false, false);
                OnConnectChanged.Invoke(remoteGattServer?.Device, dce);
            }
        }

        // BLE特性值变更通知 => 获取BLE设备的特性值
        private void Chtt_CharacteristicValueChanged(object sender, GattCharacteristicValueChangedEventArgs e)
        {
            var data = e.Value;
            if (data?.Length > 0)
            {
                //var str = data.ToHexString();
                //Trace.WriteLine($"data(长度={data.Length})\r\n值：{str}");
                rqRawData.Enqueue(data);
            }
        }
        #endregion 事件处理 end

        #region    公开方法 start
        //public void EnterState(SystemStatus state)
        //{

        //}

        //public void ExitState(SystemStatus state)
        //{

        //}

        //public void UpdateState(SystemStatus state)
        //{

        //}

        /// <summary>
        /// 清空接收队列
        /// </summary> 
        public void ClearQueue()
        {
#if NET5_0_OR_GREATER
            rqRawData.Clear();
#else
            rqRawData = new ConcurrentQueue<byte[]>();
#endif
        }

        /// <summary>
        /// 开始搜索低功耗蓝牙设备(异步地)
        /// </summary>
        /// <returns></returns>
        public async Task StartScanAsync()
        {
            BleDeviceList.Clear();
            bleScan = await Bluetooth.RequestLEScanAsync(new BluetoothLEScanOptions { AcceptAllAdvertisements = true, KeepRepeatedDevices = true });
            //var device = await Bluetooth.RequestDeviceAsync(new RequestDeviceOptions { AcceptAllDevices = true });
            //Trace.WriteLine(device);
        }

        /// <summary>
        /// 停止搜索低功耗蓝牙设备
        /// </summary>
        public Task StopScanAsync()
        {
            return Task.Run(() =>
            {
                bleScan?.Stop();
            });
        }

        /// <summary>
        /// 连接指定的BLE设备(异步地)
        /// </summary>
        /// <param name="device">指定的BLE设备</param>
        /// <returns></returns>
        public async Task<bool> ConnectAsync(BluetoothDevice device)
        {
            return await ConnectAsync(device, RTUDefault.NusBaseService);
        }

        /// <summary>
        /// 连接指定的BLE设备(异步地)
        /// </summary>
        /// <param name="device">指定的BLE设备</param>
        /// <returns></returns>
        public async Task<bool> ConnectAsync(BluetoothDevice device, BluetoothUuid serviceUuid)
        {
            remoteGattServer = device.Gatt;
            await remoteGattServer.ConnectAsync();
            IsConnected = remoteGattServer.IsConnected;
            if (IsConnected)
            {
                device.GattServerDisconnected += Dev_GattServerDisconnected;
                await InitServiceAndCharacteristics(serviceUuid);

                if (OnConnectChanged != null)
                {
                    var dce = new DeviceChangedEventArgs(true, false);
                    OnConnectChanged.Invoke(device, dce);
                }
            }
            IsConnected = remoteGattServer.IsConnected;
            return IsConnected;
        }

        /// <summary>
        /// 断开当前连接的BLE设备
        /// </summary>
        public Task<bool> DisconnectAsync()
        {
            return Task.Run(() =>
            {
                nusReadChtt = null;
                nusWriteChtt = null;
                chttLst = new List<GattCharacteristic>();
                if (remoteGattServer == null)
                {
                    return true;
                }
                remoteGattServer.Disconnect();
                return remoteGattServer.IsConnected;
            });
        }

        /// <summary>
        /// 获取本机蓝牙设备地址
        /// </summary>
        /// <returns>本机蓝牙设备地址</returns>
        private BluetoothAddress LocalAddress()
        {
            if (PrimaryRadio == null)
            {
                Console.WriteLine("No radio hardware or unsupported software stack");
                return null;
            }
            // Note that LocalAddress is null if the radio is powered-off.
            //Console.WriteLine("* Radio, address: {0:C}", primaryRadio.LocalAddress);
            return PrimaryRadio.LocalAddress;
        }

        /// <summary>
        /// 接收低功耗蓝牙设备的广播数据
        /// </summary>
        /// <returns></returns>
        public override Task ReceiveAdvertisementDataAsync()
        {
            OnHandleAdvertisementData?.Invoke(this);
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
                    //Logger.Trace($"蓝牙数据接收=>len=【{len}】");
                    try
                    {
                        if (nusReadChtt == null)
                        {
                            await Task.Delay(1);
                            //Trace.WriteLine($"nusReadChtt为空");
                            continue;
                        }

                        var data = await nusReadChtt.ReadValueAsync();
                        if (data.Length == 0)
                        {
                            await Task.Delay(1);
                            if (ReceivedCount == 1000000)
                            {
                                IsRecived = false;
                                OnCommunicateStateChanged?.Invoke(IsRecived);
                                UpdateMessage?.Invoke(Color.Red, "未接收到数据");
                                await SwitchReceiveDataAsync(false);
                                await SwitchHandleDataAsync(false);
                                ReceivedCount++;
                            }
                            else if (ReceivedCount != 1000001)
                            {
                                ReceivedCount++;
                            }
                            continue;
                        }
                        if (ReceivedCount == 1000001)
                        {
                            //var message = $"设备已重新上电并打开，可能需要刷新数据";
                            UpdateMessage?.Invoke(Color.Green, "");
                            await SwitchReceiveDataAsync(true);
                            await SwitchHandleDataAsync(true);
                        }
                        ReceivedCount = 0;
                        IsRecived = true;
                        OnCommunicateStateChanged?.Invoke(IsRecived);
                        //Trace.WriteLine($"蓝牙数据接收=>len=【{dataRe.Length}】");

                        rqRawData.Enqueue(data);

                        OnReceived?.Invoke(nusReadChtt, data);
                    }
                    catch (Exception ex)
                    {
                        string message = $"接收蓝牙数据发生异常：{ex.Message}如果是因为蓝牙断开请重新插回！".Replace("\r\n", "");
                        //logger.Fatal(message, ex);
                        //UpdateComStatus?.Invoke(false, PortName, message);
                    }
                    //timer.Stop();
                    //var timeSpan = sw.ElapsedMilliseconds;
                    //Trace.WriteLine($"蓝牙数据接收=>本次接收耗时:{timeSpan}毫秒");
                }
            }, TaskCreationOptions.LongRunning);
        }

        /// <summary>
        /// 获取指定功能码指令的最新一帧(通常是广播数据)
        /// </summary>
        /// <param name="funCode">指定功能码</param>
        /// <returns>最新一帧数据</returns>
        public byte[] GetNewFrame(byte funCode)
        {
            byte[] recvFrame = null;
            while (rqRawData.TryDequeue(out byte[] result))
            {
                var recvData = result.ToList();
                (int code, byte[] ackData) = GetAckDataFromBuffer(recvData, funCode);// 取出应答数据
                if (code == 0)
                {
                    recvFrame = ackData;
                }
                break;
            }
            return recvFrame;
        }

        public async Task<byte[]> GetNewFrameAsync(byte funCode)
        {
            //return Task.Factory.StartNew(async () =>
            //{
            byte[] recvFrame = null;
            while (rqRawData.TryDequeue(out byte[] result))
            {
                var recvData = result.ToList();
                (int code, byte[] ackData) = GetAckDataFromBuffer(recvData, funCode);// 取出应答数据
                if (code == 0)
                {
                    await Task.Delay(1);
                    recvFrame = ackData;
                }
                break;
            }
            return recvFrame;
            //}).Result;
        }

        /// <summary>
        /// 返回蓝牙mac地址的“-”连接符形式
        /// </summary>
        /// <param name="bluetoothAdress">“:”连接符形式的蓝牙mac地址</param>
        /// <returns>“-”连接符形式的蓝牙mac地址</returns>
        public string StripBluetoothAdress(string bluetoothAdress)
        {
            var charsToRemove = new string[] { ":", "-" };
            return charsToRemove.Aggregate(bluetoothAdress, (current, c) => current.Replace(c, string.Empty));
        }

        /// <summary>
        /// 自动配对蓝牙设备
        /// </summary>
        public void AutoPairDevices()
        {
            // get a list of all paired devices
            //var paired = LocalClient.DiscoverDevices();
            //// check every discovered device if it is already paired 
            //foreach (var device in DeviceInfoList)
            //{
            //    var isPaired = paired.Any(t => device.Equals(t));

            //    // if the device is not paired, try to pair it
            //    if (!isPaired)
            //    {
            //        // loop through common PIN numbers to see if they pair
            //        foreach (var devicePin in RTUDefault.CommonPins)
            //        {
            //            isPaired = BluetoothSecurity.PairRequest(device.DeviceAddress, devicePin);
            //            if (isPaired) break;
            //        }
            //    }
            //}
        }

        //        /// <summary>
        //        /// 写入值到寄存器(异步地)
        //        /// </summary>
        //        /// <param name="funCode">控制模式</param>
        //        /// <param name="value">寄存器的值，文本内容请事先转换为byte数组以防止转码不通用的问题</param>
        //        /// <returns>true表示写入成功，false表示写入失败</returns>
        //        public override async Task<bool> WriteRegisterAsync<T>(byte? chNum, byte funCode, params T[] values)
        //        {
        //            //协议：帧头(集迦) + 指令长度 + 功能码 + 寄存器值 + crc16 + 帧尾(回车换行)
        //            //字节数：4       + 2       + 1     + ?       +  2    + 2
        //            bool ret = false;
        //            try
        //            {
        //                byte[] sendBuffer = GetSendContent(chNum, funCode, values);
        //#if DEBUG
        //                var rawHexStr = sendBuffer.ToHexString();
        //#endif
        //                if (nusWriteChtt != null)
        //                {
        //                    await nusWriteChtt.WriteValueWithoutResponseAsync(sendBuffer);
        //#if DEBUG
        //                    Trace.WriteLine($"发送指令：{rawHexStr}");
        //                    logger.Trace($"发送指令：{rawHexStr}");
        //#endif
        //                    ret = true;
        //                }
        //            }
        //            catch
        //            {
        //                throw;
        //            }
        //            return ret;
        //        }

        //        /// <summary>
        //        /// 写入值到寄存器然后接收应答(异步地)
        //        /// </summary>
        //        /// <param name="funCode">控制模式</param>
        //        /// <param name="value">寄存器的值，文本内容请事先转换为byte数组以防止转码不通用的问题</param>
        //        /// <returns>返回下位机响应的数据流</returns>
        //        public override async Task<byte[]> WriteRegisterThenRecvAckAsync<T>(byte? chNum, byte funCode, params T[] values)
        //        {
        //            //协议：帧头(集迦) + 指令长度 + 通道号/广播地址 + 功能码 + 寄存器值 + crc16 + 帧尾(回车换行)
        //            //字节数：4       + 2       + 0/1           + 1     + ?       +  2    + 2
        //            byte[] recBuffer = null;
        //            try
        //            {
        //                byte[] sendBuffer = GetSendContent(chNum, funCode, values);
        //                if (nusWriteChtt != null && nusReadChtt != null)
        //                {
        //                    await nusWriteChtt.WriteValueWithResponseAsync(sendBuffer);
        //                    var recvData = await nusReadChtt.ReadValueAsync();
        //                    if (recvData.ToArray() == sendBuffer)
        //                    {
        //                        recBuffer = recvData.ToArray();
        //                    }
        //                    else
        //                    {
        //                        (int code, byte[] ackData) = GetAckDataFromBuffer(recvData.ToList(), funCode);// 取出应答数据
        //                        if (code == 0)
        //                        {
        //                            recBuffer = ackData;
        //                        }
        //                    }
        //                }
        //            }
        //            catch
        //            {
        //                throw;
        //            }
        //            return recBuffer;
        //        }

        /// <summary>
        /// 写入值到寄存器(异步地)
        /// </summary>
        /// <param name="funCode">控制模式</param>
        /// <param name="value">寄存器的值，文本内容请事先转换为byte数组以防止转码不通用的问题</param>
        /// <returns>true表示写入成功，false表示写入失败</returns>
        public override async Task<bool> WriteRegisterAsync(byte? chNum, byte funCode, params dynamic[] values)
        {
            //协议：帧头(集迦) + 指令长度 + 功能码 + 寄存器值 + crc16 + 帧尾(回车换行)
            //字节数：4       + 2       + 1     + ?       +  2    + 2
            bool ret = false;
            try
            {
                byte[] sendBuffer = GetSendContent(funCode, values, chNum.Value);
#if DEBUG
                var rawHexStr = sendBuffer.ToHexString();
#endif
                if (nusWriteChtt != null)
                {
                    await nusWriteChtt.WriteValueWithoutResponseAsync(sendBuffer);
#if DEBUG
                    Trace.WriteLine($"发送指令：{rawHexStr}");
                    logger.Trace($"发送指令：{rawHexStr}");
#endif
                    ret = true;
                }
            }
            catch
            {
                throw;
            }
            return ret;
        }

        /// <summary>
        /// 写入值到寄存器然后接收应答(异步地)
        /// </summary>
        /// <param name="funCode">控制模式</param>
        /// <param name="value">寄存器的值，文本内容请事先转换为byte数组以防止转码不通用的问题</param>
        /// <returns>返回下位机响应的数据流</returns>
        public override async Task<byte[]> WriteRegisterThenRecvAckAsync(byte? chNum, byte funCode, params dynamic[] values)
        {
            //协议：帧头(集迦) + 指令长度 + 通道号/广播地址 + 功能码 + 寄存器值 + crc16 + 帧尾(回车换行)
            //字节数：4       + 2       + 0/1           + 1     + ?       +  2    + 2
            byte[] recBuffer = null;
            try
            {
                byte[] sendBuffer = GetSendContent(funCode, values, chNum.Value);
                if (nusWriteChtt != null && nusReadChtt != null)
                {
                    await nusWriteChtt.WriteValueWithResponseAsync(sendBuffer);
                    var recvData = await nusReadChtt.ReadValueAsync();
                    if (recvData.ToArray() == sendBuffer)
                    {
                        recBuffer = recvData.ToArray();
                    }
                    else
                    {
                        (int code, byte[] ackData) = GetAckDataFromBuffer(recvData.ToList(), funCode);// 取出应答数据
                        if (code == 0)
                        {
                            recBuffer = ackData;
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
            return recBuffer;
        }
        #endregion 公开方法 end

        #region    私有方法 start
        /// <summary>
        /// 初始化服务和特征
        /// </summary>
        /// <param name="bluetoothUuid">服务的Uuid</param>
        /// <returns></returns>
        private async Task InitServiceAndCharacteristics(BluetoothUuid? bluetoothUuid = null)
        {
            var gattServiceLst = await remoteGattServer.GetPrimaryServicesAsync(bluetoothUuid);
            foreach (var gattService in gattServiceLst)
            {
                chttLst = await gattService.GetCharacteristicsAsync();
                foreach (var chtt in chttLst)
                {
                    try
                    {
                        if (chtt.Uuid == RTUDefault.NusWriteChtt)
                        {
                            nusWriteChtt = chtt;
                        }
                        if (chtt.Uuid == RTUDefault.NusReadChtt)
                        {
                            nusReadChtt = chtt;
                        }
                        if (chtt.Properties.HasFlag(GattCharacteristicProperties.Broadcast) || chtt.Properties.HasFlag(GattCharacteristicProperties.Notify) || chtt.Properties.HasFlag(GattCharacteristicProperties.Indicate))
                        {
                            chtt.CharacteristicValueChanged += Chtt_CharacteristicValueChanged;
                            await chtt.StartNotificationsAsync();
                        }
                    }
                    catch (Exception ex)
                    {
                        Trace.WriteLine(ex.Message);
                    }
                }
            }
        }
        #endregion 私有方法 end
    }
}