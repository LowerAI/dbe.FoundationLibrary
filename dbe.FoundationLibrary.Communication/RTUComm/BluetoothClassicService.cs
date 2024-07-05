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
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace dbe.FoundationLibrary.Communication.RTUComm
{
    /// <summary>
    /// 经典蓝牙(Bluetooth Classic)通信服务类
    /// </summary>
    public class BluetoothClassicService : RTUCore
    {
        #region    字段 start
        //private readonly List<BluetoothDevice> BTDeviceList;// 经典蓝牙设备列表

        private readonly List<BluetoothDeviceInfo> DeviceInfoList;// 蓝牙设备信息列表
        private readonly ConcurrentQueue<byte> rqRawData;// 存储经典蓝牙接收数据的队列

        private bool isStopScanDevice;// 是否停止搜索经典蓝牙设备
        //private BluetoothLEScan bleScan;// ble扫描器
        private BluetoothDeviceInfo btDeviceInfo;// 已连接的BT设备

        private int initFrameLength;// 初始接收帧长度
        private Socket client;
        private NetworkStream stream;
        #endregion 字段 end

        #region    属性 start
        /// <summary>
        /// 通信模式(顶级分类)
        /// </summary>
        public CommunicationMode CommunicationMode => CommunicationMode.Bluetooth;
        /// <summary>
        /// 蓝牙连接方式(2级子类)
        /// </summary>
        public BluetoothCommunicationType ConnectionMode => BluetoothCommunicationType.BluetoothClassic;
        /// <summary>
        /// 蓝牙适配器是否可用
        /// </summary>
        public bool IsRadioReady { get; private set; }
        /// <summary>
        /// 经典蓝牙设备信息列表
        /// </summary>
        public List<BluetoothDeviceInfo> BTDeviceInfoList { get; set; } = new List<BluetoothDeviceInfo>();
        /// <summary>
        /// BT客户端信息列表
        ///     元素映射：
        ///         string - mac地址
        ///         int - 排序
        ///         bool - 是否已连接
        /// </summary>
        public List<(string, int, bool)> BTClientInfoList { get; set; } = new List<(string, int, bool)>();
        /// <summary>
        /// 本地终结点
        /// </summary>
        //public BluetoothEndPoint LocalEndpoint { get; private set; }
        /// <summary>
        /// 本地客户端
        /// </summary>
        public BluetoothClient LocalClient { get; private set; }
        /// <summary>
        /// 主蓝牙模块
        /// </summary>
        public BluetoothRadio PrimaryRadio { get; private set; }
        #endregion 属性 end

        #region    结构体 start

        #endregion 结构体 end

        #region        事件声明 start
        /// <summary>
        /// 检测到广播数据从收到变为未收到或者从未收到变为收到时
        /// </summary>
        /// <param name="isReceive">true表示从未收到变为收到/false表示收到变为未收到</param>
        public delegate void CommunicateStateChangedEvent(bool isReceive);
        /// <summary>
        /// 反馈(广播)通信状态的事件，非广播通信例如交互式通信的状态无法反馈
        /// </summary>
        public event CommunicateStateChangedEvent OnCommunicateStateChanged;

        /// <summary>
        /// 检测到经典蓝牙设备连接/断开时
        /// </summary>
        /// <param name="device">连接/断开的Ble设备</param>
        /// <param name="args">设备变更事件参数</param>
        public delegate void BTConnectChangedEvent(BluetoothDeviceInfo deviceInfo, DeviceChangedEventArgs args);
        /// <summary>
        /// 经典蓝牙设备连接/断开事件
        /// </summary>
        public event BTConnectChangedEvent OnConnectChanged;

        /// <summary>
        /// 检测到广播数据时开启处理
        /// </summary>
        /// <param name="deviceInfo"></param>
        public delegate void HandleAdvertisementDataEvent(BluetoothClassicService service);
        /// <summary>
        /// 处理广播数据的事件，非广播通信例如交互式通信时不可用
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
        /// 接收到广播数据时
        /// </summary>
        /// <param name="deviceInfo"></param>
        /// <param name="client"></param>
        /// <param name="stream"></param>
        public delegate void ReceivedEvent(BluetoothDeviceInfo deviceInfo, byte[] data);
        /// <summary>
        /// 接收到广播数据的事件
        /// </summary>
        public event ReceivedEvent OnReceived;

        /// <summary>
        /// 每一次轮询后更新蓝牙设备
        /// </summary>
        /// <param name="infoLst"></param>
        /// <param name="deviceInfoLst"></param>
        public delegate void UpdateBTDevciceEvent(List<(string, int, bool)> infoLst, List<BluetoothDeviceInfo> deviceInfoLst);
        /// <summary>
        /// 更新搜索到的经典蓝牙设备的事件
        /// </summary>
        public event UpdateBTDevciceEvent OnUpdateBTDevcice;
        #endregion 事件声明 start

        #region    构造与析构 start
        public BluetoothClassicService(InstructionFormat instructionFormat, int recvFrameLength = 11) : base(instructionFormat)
        {
            try
            {
                this.initFrameLength = recvFrameLength;
                PrimaryRadio = BluetoothRadio.Default;
                IsRadioReady = true;
            }
            catch (Exception)
            {
                IsRadioReady = false;
            }
            LocalClient = new BluetoothClient { InquiryLength = RTUDefault.InquiryLength };
            client = LocalClient.Client;// 获取socket对象用于收发数据

            rqRawData = new ConcurrentQueue<byte>();
            DeviceInfoList = new List<BluetoothDeviceInfo>();

            var osVersion = RTUDefault.GetOSVersion();
            if (osVersion == OSVersion.Windows11)
            {
                Bluetooth.AvailabilityChanged += Bluetooth_AvailabilityChanged;
            }

            //if (IsRadioReady)
            //{
            //    var btAddr = PrimaryRadio.LocalAddress;
            //    LocalEndpoint = new BluetoothEndPoint(btAddr, RTUDefault.ServiceGuid);
            //    //LocalEndpoint = new BluetoothEndPoint(btAddr, Guid);
            //}
        }
        #endregion 构造与析构 end

        #region    事件处理 start
        // 蓝牙适配器打开/关闭事件
        private async void Bluetooth_AvailabilityChanged(object sender, EventArgs e)
        {
            PrimaryRadio = BluetoothRadio.Default;
            IsRadioReady = await Bluetooth.GetAvailabilityAsync();
            //Trace.WriteLine(IsRadioReady ? "蓝牙已打开" : "蓝牙已关闭");
            OnRadioStateChanged.Invoke(IsRadioReady);
        }
        #endregion 事件处理 end

        #region    公开方法 start
        /// <summary>
        /// 开始搜索经典蓝牙设备
        /// </summary>
        public void StartScan()
        {
            Task.Factory.StartNew(() =>
            {
                isStopScanDevice = false;
                while (!isStopScanDevice)
                {
                    var deviceInfos = LocalClient.DiscoverDevices();
                    BTDeviceInfoList = deviceInfos.ToList();
                    Trace.WriteLine($"StartScan.BTClientInfoList.Count={BTDeviceInfoList.Count}");
                    OnUpdateBTDevcice?.Invoke(BTClientInfoList, BTDeviceInfoList);
                }
            });
        }

        /// <summary>
        /// 停止搜索经典蓝牙设备
        /// </summary>
        public void StopScan()
        {
            if (!isStopScanDevice)
            {// 如果正在扫描中
                isStopScanDevice = true;
                //UpdateClient();
            }
        }

        /// <summary>
        /// 连接经典蓝牙设备
        /// </summary>
        /// <param name="deviceId">经典蓝牙设备的Id</param>
        public bool Connect(int deviceId)
        {
            if (DeviceInfoList.Count == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(DeviceInfoList.Count));
            }
            var device = DeviceInfoList[deviceId];
            return Connect(device);
        }

        /// <summary>
        /// 连接指定地址的BT设备
        /// </summary>
        /// <param name="macAddr">指定地址</param>
        public bool Connect(BluetoothDeviceInfo bdi)
        {
            return Connect(bdi, RTUDefault.ServiceGuid);
        }

        /// <summary>
        /// 连接经典蓝牙设备
        /// </summary>
        /// <param name="bdi"></param>
        /// <param name="guid"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public bool Connect(BluetoothDeviceInfo bdi, Guid guid)
        {
            if (bdi.DeviceAddress == BluetoothAddress.None)
            {
                throw new ArgumentNullException(nameof(bdi.DeviceAddress));
            }
            var success = false;
            try
            {
                if (BluetoothSecurity.PairRequest(bdi.DeviceAddress, RTUDefault.CommonPins[0]))
                {
                    LocalClient.Connect(bdi.DeviceAddress, guid);
                    success = LocalClient.Connected;

                    if (success)
                    {
                        bdi.SetServiceState(guid, true);
                        //var serviceGuids = bdi.InstalledServices;
                        //foreach (var item in serviceGuids)
                        //{
                        //    var srb = new ServiceRecordBuilder();
                        //    srb.AddServiceClass(item);
                        //    var sr = srb.ServiceRecord;
                        //    foreach (var id in sr.AttributeIds)
                        //    {
                        //        var sa = sr.GetAttributeById(id);
                        //    }
                        //}
                        btDeviceInfo = bdi;

                        if (OnConnectChanged != null)
                        {
                            var dce = new DeviceChangedEventArgs(true, false);
                            OnConnectChanged.Invoke(bdi, dce);
                        }

                        ReceiveAdvertisementDataAsync();
                    }
                }
                else
                {
                    Trace.WriteLine($"认证失败！");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return success;
        }

        /// <summary>
        /// 断开经典蓝牙设备
        /// </summary>
        public bool Disconnect()
        {
            var success = false;
            if (LocalClient.Connected)
            {
                success = BluetoothSecurity.RemoveDevice(btDeviceInfo.DeviceAddress);
            }
            if (OnConnectChanged != null)
            {
                var dce = new DeviceChangedEventArgs(false, false);
                OnConnectChanged.Invoke(btDeviceInfo, dce);
            }
            return success;
        }

        /// <summary>
        /// 异步连接指定地址的BT设备
        /// </summary>
        /// <param name="macAddr">指定地址</param>
        /// <returns></returns>
        public async Task<bool> ConnectAsync(BluetoothDeviceInfo bdi)
        {
            return await ConnectAsync(bdi, RTUDefault.ServiceGuid);
        }

        /// <summary>
        /// 异步连接指定地址和服务的BT设备
        /// </summary>
        /// <param name="macAddr">指定地址</param>
        /// <param name="guid">指定服务的标识符</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<bool> ConnectAsync(BluetoothDeviceInfo bdi, Guid guid)
        {
            if (bdi.DeviceAddress == BluetoothAddress.None)
            {
                throw new ArgumentNullException(nameof(bdi.DeviceAddress));
            }

            var success = false;
            if (BluetoothSecurity.PairRequest(bdi.DeviceAddress, RTUDefault.CommonPins[0]))
            {
                await LocalClient.ConnectAsync(bdi.DeviceAddress, guid);
                success = LocalClient.Connected;

                if (success)
                {
                    bdi.SetServiceState(guid, true);
                    //var serviceGuids = bdi.InstalledServices;
                    //foreach (var item in serviceGuids)
                    //{
                    //    var srb = new ServiceRecordBuilder();
                    //    srb.AddServiceClass(item);
                    //    var sr = srb.ServiceRecord;
                    //    foreach (var id in sr.AttributeIds)
                    //    {
                    //        var sa = sr.GetAttributeById(id);
                    //    }
                    //}
                    btDeviceInfo = bdi;

                    if (OnConnectChanged != null)
                    {
                        var dce = new DeviceChangedEventArgs(true, false);
                        OnConnectChanged.Invoke(bdi, dce);
                    }

                    await ReceiveAdvertisementDataAsync();
                }
            }
            else
            {
                Trace.WriteLine($"认证失败！");
            }
            return success;
        }

        /// <summary>
        /// 更新本机的蓝牙客户端
        /// </summary>
        public void UpdateClient()
        {
            if (LocalClient != null)
            {
                LocalClient.Close();
                LocalClient = new BluetoothClient { InquiryLength = RTUDefault.InquiryLength };
            }
        }

        /// <summary>
        /// 串口数据接收
        /// </summary>
        /// <returns></returns>
        public override Task ReceiveAdvertisementDataAsync()
        {
            OnHandleAdvertisementData?.Invoke(this);
            return Task.Factory.StartNew((async () =>
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
                        if (!LocalClient.Connected)
                        {
                            continue;
                        }

                        var recvLen = client.Available;
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
                        if (recvLen > 0)
                        {
                            //Trace.WriteLine($"经典蓝牙数据接收=>len=【{dataLen}】");
                            byte[] data = new byte[recvLen];
                            stream = LocalClient.GetStream();
                            int dataLen = stream.Read(data, 0, recvLen);
                            //Trace.WriteLine($"经典蓝牙数据接收=>dataLen=【{dataLen}】");

                            //var hexStr = data.ToHexString();
                            //Trace.WriteLine(hexStr);

                            for (int i = 0; i < dataLen; i++)
                            {
                                rqRawData.Enqueue(data[i]);
                            }
                            OnReceived?.Invoke(btDeviceInfo, data);
                        }
                    }
                    catch (Exception ex)
                    {
                        string message = $"接收蓝牙数据发生异常：{ex.Message}如果是因为蓝牙客户端断开请重新连接！".Replace("\r\n", "");
                        //logger.Fatal(message, ex);
                    }
                    //timer.Stop();
                    //var timeSpan = sw.ElapsedMilliseconds;
                    //Trace.WriteLine($"串口数据接收=>本次接收耗时:{timeSpan}毫秒");
                }
            }), TaskCreationOptions.LongRunning);
        }

        /// <summary>
        /// 从数据队列中取出新一帧(已废弃)
        /// </summary>
        /// <returns>正确的单帧数据</returns>
        public async Task<byte[]> GetNewFrameAsync(byte funCode, bool isCancell = false)
        {
            FrameLength = RTUDefault.HCI_Instruction_FunCodeTab.Contains(funCode) ? RTUDefault.BluetoothFrameLength : initFrameLength;
            byte[] recvFrame = null;
            var iterationLength = FixedPrefix.BytesLength;
            List<byte> affixData = new List<byte>();// 缓存词缀(帧头/帧尾)专用
            List<byte> recvData = new List<byte>();
            while (!isCancell)
            {
                while (affixData.Count < iterationLength)// 这个while循环耗时十几ms到一百ms左右
                {// 接收数据长度不足接收数据帧长度则继续从队列中拉取
                    if (rqRawData.IsEmpty)
                    {
                        //Thread.Sleep(5);
                        await Task.Delay(1);
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
                //Trace.WriteLine($"GetNewFrameAsync:GetAckDataFromBuffer->code={code}");
                if (code == 0)
                {
                    recvFrame = ackData;
                    //Trace.WriteLine($"GetNewFrameAsync:取得正确帧->frameLen={recvFrame.Length}");
                }
                break;
            }
            return recvFrame;
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

        //        /// <summary>
        //        /// 写入值到寄存器
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
        //                //var rawHexStr = StringLib.GetHexStringFromByteArray(sendbuffer);
        //                var rawHexStr = sendBuffer.ToHexString();
        //#endif
        //                if (stream != null)
        //                {
        //                    //serialPort.DiscardOutBuffer();// 清空串口发送缓冲区的所有数据
        //                    await stream.WriteAsync(sendBuffer, 0, sendBuffer.Length);
        //#if DEBUG
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
        //        /// 写入值到寄存器然后接收应答
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
        //                var recvData = await SendDataRecAsync(sendBuffer);
        //                if (recvData != null)
        //                {
        //                    if (recvData.ToArray() == sendBuffer)
        //                    {
        //                        recBuffer = recvData.ToArray();
        //                    }
        //                    else
        //                    {
        //                        (int code, byte[] ackData) = GetAckDataFromBuffer(recvData, funCode);// 取出应答数据
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
        /// 写入值到寄存器
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
                //var rawHexStr = StringLib.GetHexStringFromByteArray(sendbuffer);
                var rawHexStr = sendBuffer.ToHexString();
#endif
                if (stream != null)
                {
                    //serialPort.DiscardOutBuffer();// 清空串口发送缓冲区的所有数据
                    await stream.WriteAsync(sendBuffer, 0, sendBuffer.Length);
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
            return ret;
        }

        /// <summary>
        /// 写入值到寄存器然后接收应答
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
            catch
            {
                throw;
            }
            return recBuffer;
        }

        /// <summary>
        /// 匹配指定的蓝牙设备
        /// </summary>
        /// <param name="device">指定的蓝牙设备</param>
        /// <returns>true表示匹配成功/false表示配对失败</returns>
        public bool PairDevice(BluetoothDeviceInfo device)
        {
            if (device.Authenticated) return true;
            // loop through common PIN numbers to see if they pair
            foreach (var devicePin in RTUDefault.CommonPins)
            {
                var isPaired = BluetoothSecurity.PairRequest(device.DeviceAddress, devicePin);
                if (isPaired) break;
            }

            //device.Update();
            device.Refresh();
            return device.Authenticated;
        }

        /// <summary>
        /// 自动配对蓝牙设备
        /// </summary>
        public void AutoPairDevices()
        {
            // get a list of all paired devices
            var paired = LocalClient.DiscoverDevices();
            // check every discovered device if it is already paired 
            foreach (var device in DeviceInfoList)
            {
                var isPaired = paired.Any(t => device.Equals(t));

                // if the device is not paired, try to pair it
                if (!isPaired)
                {
                    // loop through common PIN numbers to see if they pair
                    foreach (var devicePin in RTUDefault.CommonPins)
                    {
                        isPaired = BluetoothSecurity.PairRequest(device.DeviceAddress, devicePin);
                        if (isPaired) break;
                    }
                }
            }
        }
        #endregion 公开方法 end

        #region    私有方法 start
        /// <summary>
        /// 发送并且接收字节数组
        /// </summary>
        /// <param name="sendBuffer">要发送的byte[]</param>
        /// <returns>应答的List<byte></returns>
        private List<byte> SendDataRec(byte[] sendBuffer)
        {
            List<byte> total = new List<byte>();
            try
            {
                // 发送数据
                stream.Write(sendBuffer, 0, sendBuffer.Length);
#if DEBUG
                //var rawHexStr = StringLib.GetHexStringFromByteArray(sendBuffer);
                var rawHexStr = sendBuffer.ToHexString();
                logger.Info($"发送指令：{rawHexStr}");
#endif

                byte[] buffer = null;
                int counts = 0;
                DateTime startTime = DateTime.Now;
                while (true)
                {
                    int len = client.Available;
                    if (len > 0)
                    {
                        buffer = new byte[len];
                        counts = stream.Read(buffer, 0, len);
                        if (counts > 0)
                        {
                            total.AddRange(buffer);
                        }
                    }
                    else
                    {
                        Thread.Sleep(10);
                        if ((DateTime.Now - startTime).TotalMilliseconds > stream.ReadTimeout)
                        {// 超时
                            break;
                        }
                    }
                }
            }
#if DEBUG
            catch (Exception ex)
            {
                Trace.WriteLine($"WriteCommand异常=>{ex.Message}");
#else
            catch
            {
#endif
                throw;
            }
            return total;
        }

        /// <summary>
        /// 发送并且接收字节数组
        /// </summary>
        /// <param name="sendBuffer">要发送的byte[]</param>
        /// <returns>应答的List<byte></returns>
        private async Task<List<byte>> SendDataRecAsync(byte[] sendBuffer)
        {
            List<byte> total = new List<byte>();
            try
            {
                // 发送数据
                stream.Write(sendBuffer, 0, sendBuffer.Length);
#if DEBUG
                //var rawHexStr = StringLib.GetHexStringFromByteArray(sendBuffer);
                var rawHexStr = sendBuffer.ToHexString();
                logger.Info($"发送指令：{rawHexStr}");
#endif

                byte[] buffer = null;
                int counts = 0;
                DateTime startTime = DateTime.Now;
                while (true)
                {
                    int len = client.Available;
                    if (len > 0)
                    {
                        buffer = new byte[len];
                        counts = await stream.ReadAsync(buffer, 0, len);
                        if (counts > 0)
                        {
                            total.AddRange(buffer);
                        }
                    }
                    else
                    {
                        Thread.Sleep(10);
                        if ((DateTime.Now - startTime).TotalMilliseconds > stream.ReadTimeout)
                        {// 超时
                            break;
                        }
                    }
                }
            }
#if DEBUG
            catch (Exception ex)
            {
                Trace.WriteLine($"WriteCommand异常=>{ex.Message}");
#else
            catch
            {
#endif
                throw;
            }
            return total;
        }
        #endregion 私有方法 end
    }
}