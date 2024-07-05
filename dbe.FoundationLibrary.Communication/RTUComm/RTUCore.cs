using dbe.FoundationLibrary.Core.Extensions;
using dbe.FoundationLibrary.Core.Util;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace dbe.FoundationLibrary.Communication.RTUComm
{
    /// <summary>
    /// RTU(远程终端设备)通信基类
    /// </summary>
    public abstract class RTUCore
    {
        #region    字段 start
        internal LoggerUtil logger = LoggerUtil.Instance;
        internal static string rawPrefix = RTUDefault.FixedPrefix;
        internal static string rawSuffix = RTUDefault.FixedSuffix;
        public readonly Affix FixedPrefix = new Affix(rawPrefix);
        public readonly Affix FixedSuffix = new Affix(rawSuffix);
        internal readonly object SyncRoot = new object();// 锁线程专用

        internal List<string> missData = new List<string>();// 因校验失败而丢弃的数据
        //private ConcurrentDictionary<string, int> missDataDict = new ConcurrentDictionary<string, int>();//丢弃的数据 

        internal ManualResetEvent mreReceiveData = new ManualResetEvent(true);
        internal ManualResetEvent mreHandleData = new ManualResetEvent(true);
        //private InstructionFormat itrtFmt;
        #endregion 字段 end

        #region    属性 start
        /// <summary>
        /// 指令格式
        /// </summary>
        public InstructionFormat ItrtFmt { get; set; }

        /// <summary>
        /// 数据读写器
        /// </summary>
        public CsvRW DataSaver { get; set; }

        /// <summary>
        /// 接收广播数据的线程是否阻塞
        /// true表示已阻塞/false表示未阻塞
        /// </summary>
        internal bool IsReceiveDataBlock { get; set; }

        /// <summary>
        /// 处理广播数据的线程是否阻塞
        /// true表示已阻塞/false表示未阻塞
        /// </summary>
        internal bool IsHandleDataBlock { get; set; }

        /// <summary>
        /// 是否收到数据
        /// </summary>
        public bool IsRecived { get; protected set; }

        /// <summary>
        /// 收不到数据时开始的计数次数
        /// </summary>
        public int ReceivedCount { get; protected set; }

        /// <summary>
        /// 是否蓝牙转串口
        /// </summary>
        public bool IsBluetoothToSerial { get; protected set; }

        /// <summary>
        /// 控制接收广播数据的Task暂停/继续
        /// </summary>
        public ManualResetEvent PauseReceiveData
        {
            get => mreReceiveData;
        }

        /// <summary>
        /// 控制处理广播数据以获取数据帧的Task暂停/继续
        /// </summary>
        public ManualResetEvent PauseHandleData
        {
            get => mreHandleData;
        }

        /// <summary>
        /// 最小指令长度
        /// </summary>
        public int MinimumInstructionLength { get; set; }

        /// <summary>
        /// 帧数据长度
        /// </summary>
        public int FrameLength { get; set; }

        /// <summary>
        /// 更新状态栏中的消息
        /// </summary>
        public Action<Color, string> UpdateMessage { set; get; }
        #endregion 属性 end

        #region        事件声明start
        //public delegate bool SwitchReceiveDataEvent(bool isContinue);
        /// <summary>
        /// 在继续接收广播数据线程之前
        /// </summary>
        //public event SwitchReceiveDataEvent OnSwitchReceiveDataBefore;
        public Func<bool, Task<bool>> OnSwitchReceiveDataBefore;
        /// <summary>
        /// 在停止接收广播数据线程之后
        /// </summary>
        //public event SwitchReceiveDataEvent OnSwitchReceiveDataAfter;
        public Func<bool, Task<bool>> OnSwitchReceiveDataAfter;
        #endregion 事件声明end

        #region        构造与析构 start
        protected RTUCore(InstructionFormat instructionFormat)
        {
            //itrtFmt = instructionFormat;
            ItrtFmt = instructionFormat;
        }
        #endregion 构造与析构 end

        #region        公开方法 start
        /// <summary>
        /// 在接收广播数据线程中切换继续/暂停
        /// </summary>
        /// <param name="isContinue">true表示继续/false表示暂停</param>
        /// <returns>true表示切换成功/false表示切换失败</returns>
        public Task<bool> SwitchReceiveDataAsync(bool isContinue)
        {
            return Task.Run(() =>
            {
                bool success = false;
                try
                {
                    if (isContinue)
                    {
                        OnSwitchReceiveDataBefore?.Invoke(isContinue);
                        success = IsReceiveDataBlock ? mreReceiveData.Set() : true;
                        IsReceiveDataBlock = !success;
                        Trace.WriteLine("接收广播数据线程继续");
                    }
                    else
                    {
                        success = IsReceiveDataBlock ? true : mreReceiveData.Reset();
                        IsReceiveDataBlock = success;
                        OnSwitchReceiveDataAfter?.Invoke(isContinue);
                        Trace.WriteLine("接收广播数据线程暂停");
                    }
                }
                catch (Exception ex)
                {
                    logger.Fatal("ReciveData暂停/继续接收数据失败", ex);
                }
                return success;
            });
        }

        /// <summary>
        /// 在接收广播数据线程中通过收发指令切换继续/暂停
        /// </summary>
        /// <param name="isContinue">true表示继续/false表示暂停</param>
        /// <param name="funCode">true表示读取广播数据的功能码/false表示停止读取广播数据的功能码</param>
        /// <returns>true表示切换成功/false表示切换失败</returns>
        public async Task<bool> SwitchReceiveDataByInstructionAsync(bool isContinue, byte funCode)
        {
            var ret = false;
            if (isContinue)
            {
                // 发送指令：BC AF E5 C8 00 0B 0A 8A 15 0D 0A  --- 这是示例的指令代码
                Trace.WriteLine("发送【读取振动数据】指令");
                ret = await WriteRegisterAsync(null, funCode);
                // 应答数据：无
                await SwitchReceiveDataAsync(true);
            }
            else
            {
                await SwitchReceiveDataAsync(false);
                int counts = 0;
            repeat:
                while (true)
                {
                    // 发送指令：BC AF E5 C8 00 0B 11 CA 1E 0D 0A  --- 这是示例的指令代码
                    var bytes = await WriteRegisterThenRecvAckAsync(null, funCode);
                    // 应答数据：BC AF E5 C8 00 0C 11 00 2F 96 0D 0A  --- 这是示例的指令代码
                    if (bytes == null)
                    {
                        counts++;
                        if (counts < 3)
                        {
                            await Task.Delay(10);
                            goto repeat;
                        }
                        throw new Exception($"【停止读取振动数据】异常，应答结果{nameof(bytes)}为空");
                    }
                    var dataLen = bytes.ToUShort(4);
                    if (dataLen != 12)
                    {
                        counts++;
                        if (counts < 3)
                        {
                            goto repeat;
                        }
                        throw new Exception($"【停止读取振动数据】异常，应答数据长度{dataLen}与要求长度12不匹配！");
                    }
                    break;
                }
                ret = true;
            }
            return ret;
        }

        /// <summary>
        /// 在处理广播数据线程中切换继续/暂停
        /// </summary>
        /// <param name="isContinue">true表示继续/false表示暂停</param>
        /// <returns>true表示切换成功/false表示切换失败</returns>
        public Task<bool> SwitchHandleDataAsync(bool isContinue)
        {
            return Task.Run(() =>
            {
                bool success = false;
                try
                {
                    if (isContinue)
                    {
                        success = IsHandleDataBlock ? mreHandleData.Set() : true;
                        IsHandleDataBlock = !success;
                        Trace.WriteLine("处理广播数据线程继续");
                    }
                    else
                    {
                        success = IsHandleDataBlock ? true : mreHandleData.Reset();
                        IsHandleDataBlock = success;
                        Trace.WriteLine("处理广播数据线程暂停");
                    }
                }
                catch (Exception ex)
                {
                    logger.Fatal("HandleData暂停/继续处理数据失败", ex);
                }
                return success;
            });
        }

        /// <summary>
        /// 接收广播数据的线程
        /// </summary>
        /// <returns></returns>
        public abstract Task ReceiveAdvertisementDataAsync();

        /// <summary>
        /// 获取指令中的有效数据
        /// </summary>
        /// <param name="dataFrame">正确的单帧指令</param>
        /// <param name="funCode">需要特殊处理时的功能码</param>
        /// <returns>单帧指令中的有效数据</returns>
        public byte[] GetValidData(byte[] dataFrame, byte funCode = 0x00)
        {
            // funCode是针对像WLVS这样在常规的串口协议中还夹杂着蓝牙控制指令(同样存在广播数据收发)时做特殊处理，其他情况不需要
            var validDataLen = dataFrame.Length - ItrtFmt.ValidData_Offset - ItrtFmt.Crc_Len - ItrtFmt.Tail_Len;
            var validData = dataFrame.Subbytes(ItrtFmt.ValidData_Offset, validDataLen);
            return validData;
        }

        ///// <summary>
        ///// 写入值到寄存器(异步地)
        ///// </summary>
        ///// <param name="funCode">控制模式</param>
        ///// <param name="value">寄存器的值，文本内容请事先转换为byte数组以防止转码不通用的问题</param>
        ///// <returns>true表示写入成功，false表示写入失败</returns>
        //public abstract Task<bool> WriteRegisterAsync<T>(byte? chNum, byte funCode, params T[] values);

        ///// <summary>
        ///// 写入值到寄存器然后接收应答(异步地)
        ///// </summary>
        ///// <param name="funCode">控制模式</param>
        ///// <param name="value">寄存器的值，文本内容请事先转换为byte数组以防止转码不通用的问题</param>
        ///// <returns>返回下位机响应的数据流</returns>
        //public abstract Task<byte[]> WriteRegisterThenRecvAckAsync<T>(byte? chNum, byte funCode, params T[] values);

        /// <summary>
        /// 写入值到寄存器(异步地)
        /// </summary>
        /// <param name="funCode">控制模式</param>
        /// <param name="value">寄存器的值，文本内容请事先转换为byte数组以防止转码不通用的问题</param>
        /// <returns>true表示写入成功，false表示写入失败</returns>
        public abstract Task<bool> WriteRegisterAsync(byte? chNum, byte funCode, params dynamic[] values);

        /// <summary>
        /// 写入值到寄存器然后接收应答(异步地)
        /// </summary>
        /// <param name="funCode">控制模式</param>
        /// <param name="value">寄存器的值，文本内容请事先转换为byte数组以防止转码不通用的问题</param>
        /// <returns>返回下位机响应的数据流</returns>
        public abstract Task<byte[]> WriteRegisterThenRecvAckAsync(byte? chNum, byte funCode, params dynamic[] values);

        /// <summary>
        /// 接收到的数据转换为指定的常用类型的数据
        /// </summary>
        /// <typeparam name="T">指定的常用类型的数据</typeparam>
        /// <param name="bytes">接收到的数据</param>
        /// <param name="inArgCounts">参数个数</param>
        /// <param name="srcOffset">源数组中取值的起始位置</param>
        /// <param name="count">源数组中有效数据的长度</param>
        /// <returns></returns>
        public List<T> ToValues<T>(byte[] bytes, int inArgCounts, int srcOffset = 7, int count = 2)
        {
            List<T> values = new List<T>();
            try
            {
                if (bytes == null)
                {
                    throw new ArgumentNullException(nameof(bytes));
                }
                else if (bytes.Length < 11)
                {
                    throw new ArgumentException($"{nameof(bytes)}长度不能小于11");
                }
                //var data = new byte[count];
                var stepOffset = count / inArgCounts;// 每个参数取值的偏移长度
                var data = new byte[stepOffset];

                var startOffset = srcOffset;
                for (int i = 0; i < inArgCounts; i++)
                {
                    Array.Clear(data, 0, data.Length);
                    Buffer.BlockCopy(bytes, startOffset, data, 0, stepOffset);
                    //T val = ByteArrayLib.ToValue<T>(data);
                    T val = data.ToValue<T>();
                    values.Add(val);

                    startOffset += stepOffset;
                }
            }
            catch
            {
                throw;
            }
            return values;
        }
        #endregion 公开方法 end

        #region    私有方法 start
        ///// <summary>
        ///// 获取发送数据流
        ///// </summary>
        ///// <typeparam name="T">参数类型</typeparam>
        ///// <param name="funCode">功能码</param>
        ///// <param name="values">参数值</param>
        ///// <returns>组装好的帧数据</returns>
        //internal byte[] GetSendContent<T>(byte? chNum, byte funCode, T[] values)
        //{
        //    List<byte> bytes = new List<byte>();
        //    try
        //    {
        //        if (FixedPrefix.BytesLength != ItrtFmt.Head_Len)
        //        {
        //            var errMsg = $"当前帧头长度{FixedPrefix.BytesLength}与协议规定的帧头长度{ItrtFmt.Head_Len}不一致!";
        //            Trace.WriteLine(errMsg);
        //            throw new Exception(errMsg);
        //        }
        //        if (FixedSuffix.BytesLength != ItrtFmt.Tail_Len)
        //        {
        //            var errMsg = $"当前帧尾长度{FixedSuffix.BytesLength}与协议规定的帧尾长度{ItrtFmt.Tail_Len}不一致!";
        //            Trace.WriteLine(errMsg);
        //            throw new Exception(errMsg);
        //        }
        //        bytes.AddRange(FixedPrefix.Bytes);// 指令头-固定前缀
        //        if (ItrtFmt.ChNum_Len > 0 && chNum != null)
        //        {
        //            bytes.Add(chNum.Value);
        //        }
        //        bytes.Add(funCode);// 控制模式

        //        foreach (var value in values)
        //        {
        //            byte[] val;
        //            switch (value)
        //            {
        //                case sbyte sbyteValue:
        //                    val = new byte[] { (byte)sbyteValue };
        //                    break;
        //                case byte byteValue:
        //                    val = new byte[] { byteValue };
        //                    break;
        //                case short shortValue:
        //                    val = BitConverter.GetBytes(shortValue);
        //                    break;
        //                case ushort ushortValue:
        //                    val = BitConverter.GetBytes(ushortValue);
        //                    break;
        //                case int intValue:
        //                    val = BitConverter.GetBytes(intValue);
        //                    break;
        //                case uint uintValue:
        //                    val = BitConverter.GetBytes(uintValue);
        //                    break;
        //                case float floatValue:
        //                    val = BitConverter.GetBytes(floatValue);
        //                    break;
        //                case double doubleValue:
        //                    val = BitConverter.GetBytes(doubleValue);
        //                    break;
        //                case byte[] byteArrayValue:
        //                    val = byteArrayValue;
        //                    break;
        //                default:
        //                    throw new ArgumentOutOfRangeException(nameof(value));
        //            }

        //            Array.Reverse(val);
        //            bytes.AddRange(val);// 寄存器值
        //        }

        //        var fullLength = bytes.Count + ItrtFmt.Total_Len + ItrtFmt.Crc_Len + FixedSuffix.Bytes.Length;
        //        var fullLen = Convert.ToUInt16(fullLength).ToByteArray();// [指令全长]字段值
        //        if (ItrtFmt.Total_Len == 4)
        //        {
        //            fullLen = Convert.ToUInt32(fullLength).ToByteArray();
        //        }
        //        var insPos = FixedPrefix.BytesLength;// [指令全长]字段的偏移地址(即在指令中的起始位置)
        //        bytes.InsertRange(insPos, fullLen);

        //        //var crc = bytes.ToArray().GetCRC16();
        //        var crc = bytes.ToArray().GetCRC(ItrtFmt.CrcType);
        //        if (crc.Length != ItrtFmt.Crc_Len)
        //        {
        //            var errMsg = $"实时CRC长度{crc.Length}与协议规定的CRC长度{ItrtFmt.Crc_Len}不一致!";
        //            Trace.WriteLine(errMsg);
        //            throw new Exception(errMsg);
        //        }
        //        bytes.AddRange(crc);// crc16

        //        bytes.AddRange(FixedSuffix.Bytes);// 固定后缀
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //    return bytes.ToArray();
        //}

        ///// <summary>
        ///// 获取发送数据流
        ///// </summary>
        ///// <typeparam name="T">值类型</typeparam>
        ///// <param name="values"></param>
        ///// <returns>组装好的帧数据</returns>
        //internal byte[] GetSendContent(byte? chNum, byte funCode, object[] values)
        //{
        //    List<byte> bytes = new List<byte>();
        //    try
        //    {
        //        bytes.AddRange(FixedPrefix.Bytes);// 指令头-固定前缀
        //        if (chNum != null)
        //        {
        //            bytes.Add(chNum.Value);
        //        }
        //        bytes.Add(funCode);// 控制模式

        //        foreach (var value in values)
        //        {
        //            byte[] val;
        //            switch (value)
        //            {
        //                case sbyte sbyteValue:
        //                    val = new byte[] { (byte)sbyteValue };
        //                    break;
        //                case byte byteValue:
        //                    val = new byte[] { byteValue };
        //                    break;
        //                case short shortValue:
        //                    val = BitConverter.GetBytes(shortValue);
        //                    break;
        //                case ushort ushortValue:
        //                    val = BitConverter.GetBytes(ushortValue);
        //                    break;
        //                case int intValue:
        //                    val = BitConverter.GetBytes(intValue);
        //                    break;
        //                case uint uintValue:
        //                    val = BitConverter.GetBytes(uintValue);
        //                    break;
        //                case float floatValue:
        //                    val = BitConverter.GetBytes(floatValue);
        //                    break;
        //                case double doubleValue:
        //                    val = BitConverter.GetBytes(doubleValue);
        //                    break;
        //                case byte[] byteArrayValue:
        //                    val = byteArrayValue;
        //                    break;
        //                default:
        //                    throw new ArgumentOutOfRangeException(nameof(value));
        //            }

        //            Array.Reverse(val);
        //            bytes.AddRange(val);// 寄存器值
        //        }

        //        var fullLength = bytes.Count + ItrtFmt.Total_Len + ItrtFmt.Crc_Len + FixedSuffix.Bytes.Length;
        //        var fullLen = Convert.ToUInt16(fullLength).ToByteArray();// [指令全长]字段值
        //        var insPos = FixedPrefix.BytesLength;// [指令全长]字段的偏移地址(即在指令中的起始位置)
        //        bytes.InsertRange(insPos, fullLen);

        //        var crc = bytes.ToArray().GetCRC16();
        //        bytes.AddRange(crc);// crc16

        //        bytes.AddRange(FixedSuffix.Bytes);// 固定后缀
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //    return bytes.ToArray();
        //}

        /// <summary>
        /// 获取发送数据流
        /// </summary>
        /// <typeparam name="T">参数类型</typeparam>
        /// <param name="funCode">功能码</param>
        /// <param name="values">参数值</param>
        /// <returns>组装好的帧数据</returns>
        internal byte[] GetSendContent(byte funCode, dynamic[] values, byte chNum = 0x00)
        {
            List<byte> bytes = new List<byte>();
            try
            {
                if (FixedPrefix.BytesLength != ItrtFmt.Head_Len)
                {
                    var errMsg = $"当前帧头长度{FixedPrefix.BytesLength}与协议规定的帧头长度{ItrtFmt.Head_Len}不一致!";
                    Trace.WriteLine(errMsg);
                    throw new Exception(errMsg);
                }
                if (FixedSuffix.BytesLength != ItrtFmt.Tail_Len)
                {
                    var errMsg = $"当前帧尾长度{FixedSuffix.BytesLength}与协议规定的帧尾长度{ItrtFmt.Tail_Len}不一致!";
                    Trace.WriteLine(errMsg);
                    throw new Exception(errMsg);
                }
                bytes.AddRange(FixedPrefix.Bytes);// 指令头-固定前缀
                if (ItrtFmt.ChNum_Len > 0)
                {
                    bytes.Add(chNum);
                }
                bytes.Add(funCode);// 控制模式

                foreach (var value in values)
                {
                    //if (value is byte[] byteArr)
                    //{
                    //    bytes.AddRange(byteArr);
                    //}
                    //else
                    //{
                    //    byte[] val = value.ToByteArray<dynamic>();
                    //    bytes.AddRange(val);// 寄存器值
                    //}
                    //byte[] val = value.ToByteArray<dynamic>();
                    byte[] val = ConvertUtil.ToByteArray(value);
                    bytes.AddRange(val);// 寄存器值
                }

                var fullLength = bytes.Count + ItrtFmt.TotalLength_Len + ItrtFmt.Crc_Len + FixedSuffix.Bytes.Length;
                var fullLen = Convert.ToUInt16(fullLength).ToByteArray();// [指令全长]字段值
                if (ItrtFmt.TotalLength_Len == 4)
                {
                    fullLen = Convert.ToUInt32(fullLength).ToByteArray();
                }
                var insPos = FixedPrefix.BytesLength;// [指令全长]字段的偏移地址(即在指令中的起始位置)
                bytes.InsertRange(insPos, fullLen);

                var crc = bytes.ToArray().GetCRC(ItrtFmt.CrcType, ItrtFmt.CrcEndian);
                if (crc.Length != ItrtFmt.Crc_Len)
                {
                    var errMsg = $"实时CRC长度{crc.Length}与协议规定的CRC长度{ItrtFmt.Crc_Len}不一致!";
                    Trace.WriteLine(errMsg);
                    throw new Exception(errMsg);
                }
                bytes.AddRange(crc);// crc16

                bytes.AddRange(FixedSuffix.Bytes);// 固定后缀
            }
            catch
            {
                throw;
            }
            return bytes.ToArray();
        }

        /// <summary>
        /// 从缓存数组中取出正确的数据帧
        /// </summary>
        /// <param name="recvData">原始串口数据</param>
        /// <param name="funCode">发送指令的功能码</param>
        /// <returns>正确的数据帧</returns>
        internal (int, byte[]) GetAckDataFromBuffer(List<byte> recvData, byte funCode = 0xFF)
        {
            var code = 0;
            var rawData = recvData;// 备份原始数据
        restart:
            if (rawData.Count < MinimumInstructionLength)
            {
                var rawHexStr = rawData.ToArray().ToHexString();
                logger.Trace($"本次缓存数据长度{rawData.Count}小于指令最短长度{MinimumInstructionLength}：{rawHexStr}");
                return (1, null);
            }

            var rawPrefix = FixedPrefix.Bytes;
            var rawSuffix = FixedSuffix.Bytes;
            var dpLen = rawPrefix.Length;
            var dsLen = rawSuffix.Length;
            var prefixIndex = -1;
            for (int i = 0; i + dpLen <= rawData.Count; i++)
            {// 每次取4个字节判断是否匹配固定帧头
                var rawHead = rawData.GetRange(i, dpLen).ToArray();
                if (rawHead.IsEqual(rawPrefix))
                {// 找到匹配固定头帧的数据头时
                    prefixIndex = i;
                    break;
                }
            }

            if (prefixIndex >= 0)
            {// 找到了数据头则抛弃数据头之前的部分
                rawData.RemoveRange(0, prefixIndex);// 移除数据头之前的部分
            }
            else
            {// 找不到数据头那么找数据尾
                var tailIndex = -1;
                for (int i = 0; i + dsLen <= rawData.Count; i++)
                {// 每次取2个字节判断是否匹配固定帧尾
                    var rawEnd = rawData.GetRange(i, dsLen).ToArray();
                    if (rawEnd.IsEqual(rawSuffix))
                    {// 找到匹配固定尾帧的数据尾时
                        tailIndex = i;
                        break;
                    }
                }

                var rawHexStr = rawData.ToArray().ToHexString();
                if (tailIndex >= 0)
                {// 找不到数据头但找到了数据尾则抛弃(含)数据尾之前的部分
                    logger.Trace($"找不到数据头但找到了数据尾则抛弃(含)数据尾之前的部分：{rawHexStr}");
                    rawData.RemoveRange(0, tailIndex + dsLen);// 移除数据头之前的部分
                    return (2, null);
                }
                else
                {// 头尾都找不到则全部抛弃
                    logger.Trace($"头尾都找不到则全部抛弃：{rawHexStr}");
                    rawData.Clear();
                    return (3, null);
                }
            }

            if (rawData.Count < MinimumInstructionLength)
            {// rawData引用的是recvData，所以数据头之前的部分已被移除
                var rawHexStr = rawData.ToArray().ToHexString();
                logger.Trace($"消除头部无效数据后整体长度{rawData.Count}小于指令最短长度{MinimumInstructionLength}：{rawHexStr}");
                return (4, null);
            }

            if (funCode != 0xFF)
            {// 非特定功能码时需要校验功能码是否匹配
                var expectedFunCode = funCode;
                if (RTUDefault.FunCodeVerificationDict.TryGetValue(funCode, out byte vFunCode))
                {
                    expectedFunCode = vFunCode;
                }
                var offset = RTUDefault.HCI_Instruction_FunCodeTab.Contains(funCode) ? 7 : ItrtFmt.FunCode_Offset;
                var fc = rawData.ToArray().ToValue<byte>(offset, 1);
                //if ((retFunCode == 0xFF && fc != funCode) || (retFunCode != 0xFF && fc != retFunCode))
                if (fc != expectedFunCode)
                {
                    rawData.RemoveRange(0, dpLen);// 移除此次记录的数据头防止下一次循环仍然取这条数据 
                    goto restart;
                }
            }

            int dataLen = rawData.ToArray().ToUShort(dpLen);// 取数据长度
            if (ItrtFmt.TotalLength_Len > 2)
            {
                dataLen = rawData.ToArray().ToInt(dpLen);
            }
            if (dataLen > 0)
            {
                //itrtFormat.Total_Len = dataLen;
                FrameLength = dataLen;// 重设帧长度
            }
            else
            {// 应答数据长度不正确
                var rawHexStr = rawData.ToArray().ToHexString();
                logger.Trace($"应答数据的帧长度{dataLen}不正确：{rawHexStr}");
                return (5, null);
            }
            var diff = dataLen - rawData.Count;
            if (diff > 0)
            {// 已收数据长度小于应答数据长度
                var rawHexStr = rawData.ToArray().ToHexString();
                logger.Trace($"消除头部无效数据后应答数据的整体长度{rawData.Count}小于应答数据的帧长度{dataLen}：{rawHexStr}");
                return (6, null);
            }

            var suffixIndex = dataLen - dsLen;// 当前帧的帧尾应有的起始索引
            var rawTail = rawData.GetRange(suffixIndex, dsLen).ToArray();// 当前帧的帧尾
            if (!rawTail.IsEqual(rawSuffix))
            {// 数据尾不匹配固定尾帧时
                var rawHexStr = rawData.ToArray().ToHexString();
                logger.Trace($"数据尾不匹配固定帧尾所以移除已匹配的数据头：{rawHexStr}");
                rawData.RemoveRange(0, dpLen);// 移除此次记录的数据头防止下一次循环仍然取这条数据
                // 移除数据头方便循环后继续找下一个数据头，排除下一个数据头包含在本次缓存数据中的情况
                return (7, null);
            }

            var crc_offset = dataLen - (ItrtFmt.Crc_Len + dsLen);// 当前帧中校验和的偏移地址
            var crc_ack = rawData.GetRange(crc_offset, ItrtFmt.Crc_Len).ToArray();// 当前帧中校验和的值
            var validData = rawData.GetRange(0, crc_offset).ToArray();// 当前帧的待校验数据
            byte[] crc_calc = validData.GetCRC(ItrtFmt.CrcType, ItrtFmt.CrcEndian);
            if (!crc_ack.IsEqual(crc_calc))
            {
                var rawHexStr = rawData.ToArray().ToHexString();
                logger.Trace($"本次应答数据的校验和{crc_calc.ToHexString()}与原始校验和{crc_ack.ToHexString()}不匹配：{rawHexStr}");
                rawData.RemoveRange(0, dataLen);// 移除此记录防止下一次循环仍然取这条数据
                return (8, null);
            }

            // 数据头/数据尾/长度全都匹配后
            var ackData = rawData.Count > dataLen ? rawData.GetRange(0, dataLen).ToArray() : rawData.ToArray();// 取出应答数据
            return (code, ackData);
        }
        #endregion 私有方法 end
    }
}