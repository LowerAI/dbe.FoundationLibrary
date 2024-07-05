using dbe.FoundationLibrary.Communication.RTUComm;
using dbe.FoundationLibrary.Core.Common;
using dbe.FoundationLibrary.Core.Extensions;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace dbe.FoundationLibrary.Communication.CustComm.XCD_EDGE_Motor
{
    /// <summary>
    /// NanoMotion公司的XCD EDGE电机的通信控制类
    /// </summary>
    public class MotorControl
    {
        #region    字段 start
        private SerialPort serialPort;// PC选定串口
        private ushort destAddr = 0x00;// 目标地址
        private byte[] prefixBytes;// 串行数据前缀的字节形式
        private object lockObj = new object();
        #endregion 字段 end

        #region    属性 start
        /// <summary>
        /// 全局串口
        /// </summary>
        public SerialPort SerialPort
        {
            get => serialPort;
            set => serialPort = value;
        }

        /// <summary>
        /// 目标地址
        /// </summary>
        public ushort DestAddr
        {
            get => destAddr;
            set => destAddr = value;
        }

        /// <summary>
        /// 数据帧固定前缀的字节数组形式
        /// </summary>
        private byte[] PrefixBytes
        {
            get
            {
                if (prefixBytes == null)
                {
                    prefixBytes = new byte[] { 0xE4, 0xA5 };
                }
                return prefixBytes;
            }
        }
        #endregion 属性 end

        #region    构造函数 start
        public MotorControl(SerialPort serialPort)
        {
            this.serialPort = serialPort;
        }
        #endregion 构造函数 end

        #region    公有方法 start
        /// <summary>
        /// 移动到指定位置
        /// </summary>
        /// <param name="position" type=Real(4)>指定位置(单位mm)</param>
        /// <returns></returns>
        public bool MoveTo(float position)
        {
            return ExecuteCommand(XMSCommandID.MOVE, "移动到指定位置", position);
        }

        /// <summary>
        /// 给Int16(4)变量赋值
        /// </summary>
        /// <param name="varId">变量ID</param>
        /// <param name="value">分配的值</param>
        /// <returns></returns>
        public bool AssignInt16(ushort varId, short value)
        {
            return ExecuteCommand(XMSCommandID.ASSIGN_INT16, "赋值", varId, value);
        }

        /// <summary>
        /// 给Bool变量赋值
        /// </summary>
        /// <param name="varId">变量ID</param>
        /// <param name="value">分配的值</param>
        /// <returns></returns>
        public bool AssignBoolean(ushort varId, bool value)
        {
            var val = Convert.ToInt16(value);
            return AssignInt16(varId, val);
        }

        /// <summary>
        /// 给Real(4)变量赋值
        /// ID和参数在同一个ASSIGN命令中最多可以重复8次。
        /// </summary>
        /// <param name="varId">变量ID</param>
        /// <param name="value">分配的值</param>
        /// <returns></returns>
        public bool Assign(ushort varId, float value)
        {
            return ExecuteCommand(XMSCommandID.ASSIGN, "赋值", varId, value);
        }

        /// <summary>
        /// 给特殊变量赋值
        /// 仅限于给FPOS、TIME、S_IND这3个变量赋值
        /// </summary>
        /// <param name="varId">变量ID</param>
        /// <param name="value">分配的值</param>
        /// <returns></returns>
        public bool Set(ushort varId, float value)
        {
            if (varId != XMSVariableID.FPOS && varId != XMSVariableID.TIME && varId != XMSVariableID.S_IND)
            {
                throw new ArgumentOutOfRangeException(nameof(varId), "ID必须是FPOS、TIME、S_IND三者之一！");
            }
            return ExecuteCommand(XMSCommandID.SET, "设置特殊值", varId, value);
        }

        /// <summary>
        /// 给Bool变量赋值
        /// </summary>
        /// <param name="varId">变量ID</param>
        /// <param name="value">分配的值</param>
        /// <returns></returns>
        public bool SetBoolean(ushort varId, bool value)
        {
            var val = Convert.ToSingle(value);
            return Set(varId, val);
        }

        /// <summary>
        /// 执行复位
        /// </summary>
        /// <param name="shm" type=Int8(1)>选择标准复位方法<cref=StandardHomingMethods>之一</param>
        /// <param name="origin" type=Real(4)>定义原点的位置。如果没有值输入，系统假定零为原点。</param>
        /// <param name="velocity1" type=Real(4)>定义第一级速度</param>
        /// <param name="velocity2" type=Real(4)>定义第二级速度。如果省略，系统采用velocity1值。</param>
        /// <returns></returns>
        public bool Home(StandardHomingMethods shm = StandardHomingMethods.NegativeHardStop, float origin = 0, float velocity1 = 10, float velocity2 = float.NaN)
        {
            return ExecuteCommand(XMSCommandID.HOME, "复位", (byte)shm, origin, velocity1, velocity2);
        }

        /// <summary>
        /// 执行速度环控制
        /// 速度参数定义了平台所需的速度（每秒线性或旋转单位）。
        /// </summary>
        /// <returns></returns>
        public bool VelocityLoop(float value)
        {
            return ExecuteCommand(XMSCommandID.VELOCITY_LOOP, "执行速度环控制", value);
        }

        /// <summary>
        /// 执行开环控制
        /// 参数命令以百分比定义命令值，从‑100到+100。
        /// </summary>
        /// <returns></returns>
        public bool OpenLoop(float value)
        {
            return ExecuteCommand(XMSCommandID.OPEN_LOOP, "执行开环控制", value);
        }

        /// <summary>
        /// 将参数值保存到闪存
        /// 在下一次启动时，控制器从闪存中读取参数并以存储的参数而不是默认值启动。
        /// </summary>
        /// <param name="addr" type=Int8(1)>指定控制器的通讯地址</param>
        /// <returns></returns>
        public bool SaveParameters(sbyte addr)
        {
            return ExecuteCommand(XMSCommandID.SAVE_FLASH, "保存参数", addr, 0x5A);
        }

        /// <summary>
        /// 更改通讯地址
        /// 前两个参数是必需的，以防止意外使用该命令。
        /// </summary>
        /// <param name="addr" type=Int8(1)>指定控制器的当前通讯地址</param>
        /// (0x5A)  ‑  常数  90
        /// <param name="newaddr" type=Int8(1)>指定控制器的新通信地址</param>
        /// <returns></returns>
        public bool Address(sbyte addr, sbyte newaddr)
        {
            return ExecuteCommand(XMSCommandID.SET_ADDRESS, "更改通讯地址", addr, 0x5A, newaddr);
        }

        /// <summary>
        /// 启用伺服回路
        /// </summary>
        /// <returns></returns>
        public bool Enable()
        {
            return ExecuteCommand(XMSCommandID.ENABLE, "启用伺服回路");
        }

        /// <summary>
        /// 禁用伺服回路
        /// </summary>
        /// <returns></returns>
        public bool Disable()
        {
            return ExecuteCommand(XMSCommandID.DISABLE, "禁用伺服回路");
        }

        /// <summary>
        /// 读取电机版本信息
        /// </summary>
        /// <returns></returns>
        public string ReadVersion()
        {
            var info = string.Empty;
            try
            {
                var rev = ExecuteQuery(XMSCommandID.READ_VERSION, "读取版本信息");

                var controllerVersion = $"Controller Version: {rev[9]}.{rev[8]}";
                var build = $"Build: {rev[7]}.{rev[6]}";
                var controllerSN = $"{rev[13]}.{rev[12]}.{rev[11]}.{rev[10]}";
                controllerSN = controllerSN == "0.0.0.0" ? "0" : controllerSN;
                controllerSN = $"Controller SN: {controllerSN}";
                var code = $"{rev[15]}.{rev[14]}";
                code = code == "0.0.0.0" ? "0" : code;
                code = $"Code: {code}";
                info = $"{controllerVersion} {build} {controllerSN} {code}";
#if DEBUG
                Trace.WriteLine($"版本信息：{info}");
#endif
            }
            catch
            {
                throw;
            }
            return info;
        }

        /// <summary>
        /// 监控变量
        /// 要停止监视，请应用参数中全为零的监视器地址(21)。
        /// </summary>
        /// <param name="variable">变量</param>
        /// <param name="channel">通道</param>
        /// <param name="scale">变量比例</param>
        /// <returns></returns>
        public bool Monitor(sbyte variable, ushort channel, float scale)
        {
            return ExecuteCommand(XMSCommandID.MONITOR, "监控变量", variable, channel, scale);
        }

        /// <summary>
        /// 监控地址
        /// 要停止监视，请应用参数中全为零的监视器地址(21)。
        /// </summary>
        /// <param name="channel">通道</param>
        /// <param name="scale">变量比例</param>
        /// <returns></returns>
        public bool Monitor(ushort channel, float scale)
        {
            return ExecuteCommand(XMSCommandID.MONITOR_ADDRESS, "监控地址", channel, scale);
        }

        /// <summary>
        /// 以KDEC速率导致运动减速。此命令不会禁用电机。
        /// </summary>
        /// <returns></returns>
        public bool Kill()
        {
            return ExecuteCommand(XMSCommandID.KILL, "刹车");
        }

        /// <summary>
        /// 获取类型为Int16的变量值
        /// </summary>
        /// <param name="paras" type=Int16(2)>变量Id，数量范围1~10</param>
        /// <returns></returns>
        public List<short> ReportInt16(params object[] paras)
        {
            if (paras.Length < 1)
            {
                throw new ArgumentNullException(nameof(paras), "至少需要1个参数！");
            }
            else if (paras.Length > 10)
            {
                throw new ArgumentNullException(nameof(paras), "最多接受10个参数！");
            }
            byte[] bytes = ExecuteQuery(XMSCommandID.REPORT_INT16, "获取变量值", paras);
            var ret = ToValues<short>(bytes, paras.Length);
            return ret;
        }

        /// <summary>
        /// 获取类型为Int16的变量值
        /// </summary>
        /// <param name="varId" type=Int16(2)>变量Id</param>
        /// <returns></returns>
        public short ReportInt16(ushort varId)
        {
            byte[] bytes = ExecuteQuery(XMSCommandID.REPORT_INT16, "获取变量值", varId);
            var ret = ToValue<short>(bytes);
            return ret;
        }

        /// <summary>
        /// 获取类型为ID的变量值
        /// </summary>
        /// <param name="paras" type=ID(2)>变量Id，数量范围1~10</param>
        /// <returns></returns>
        public List<T> Report<T>(params object[] paras)
        {
            if (paras.Length < 1)
            {
                throw new ArgumentNullException(nameof(paras), "至少需要1个参数！");
            }
            else if (paras.Length > 10)
            {
                throw new ArgumentNullException(nameof(paras), "最多接受10个参数！");
            }
            byte[] bytes = ExecuteQuery(XMSCommandID.REPORT, "获取变量值", paras);
            List<T> values = ToValues<T>(bytes, paras.Length);
            return values;
        }

        /// <summary>
        /// 获取类型为ID的变量值
        /// </summary>
        /// <param name="varId" type=ID(2)>变量Id</param>
        /// <returns></returns>
        public T Report<T>(ushort varId)
        {
            byte[] bytes = ExecuteQuery(XMSCommandID.REPORT, "获取变量值", varId);
            var ret = ToValue<T>(bytes);
            return ret;
        }

        /// <summary>
        /// 配置控制器
        /// </summary>
        /// <param name="varId">变量的ID</param>
        /// <param name="value">变量的值</param>
        /// <returns></returns>
        public bool Configure(float varId, float value)
        {
            return ExecuteCommand(XMSCommandID.CONFIG, "配置控制器", varId, value);
        }

        /// <summary>
        /// 开始位置比较脉冲生成（增量）
        /// </summary>
        /// <param name="start" type=Real(4)>指定第一个位置，一旦电机反馈比较到 t 星，控制器产生第一个脉冲。</param>
        /// <param name="incremental" type=Real(4)>以毫米为单位指定脉冲之间的距离。下一个比较的位置是前一个位置加上增量。该值为正或负。</param>
        /// <param name="count" type=Int32(4)>指定脉冲数。如果计数是一，单脉冲在起点产生。</param>
        /// <returns></returns>
        public bool PositionPluseIncremental(float start, float incremental, int count)
        {
            return ExecuteCommand(XMSCommandID.PPI, "生成脉冲", start, incremental, count);
        }

        /// <summary>
        /// 控制器返回配置文件中定义的修订字符串(最多30个ASCII字符)。
        /// </summary>
        /// <param name="addr" type=Int8(1)>Var1选项是：配置、部件号、序列号</param>
        /// <returns></returns>
        public string GetDescriptor(sbyte var)
        {
            byte[] bytes = ExecuteQuery(XMSCommandID.GET_DESCRIPTOR, "获取修订描述", var);
            var ret = ToValue<string>(bytes);
            return ret;
        }

        /// <summary>
        /// 设置超高分辨率
        /// UHR功能在运动曲线期间提供较慢的电机运动，这种较慢的移动提高了载物台移动的准确性。
        /// </summary>
        /// <param name="value" type=Real(4)></param>
        /// <returns></returns>
        public bool UltraHighResolution(float value)
        {
            return ExecuteCommand(XMSCommandID.UHR, "设置超高分辨率", value);
        }

        /// <summary>
        /// 激活校准
        /// </summary>
        /// <param name="pwmValue" type=Real(4)>以最大值的百分比指定PWM电平。如果省略，则使用默认值50%。</param>
        /// <param name="pwmWidth" type=Real(4)>以毫秒为单位指定PWM脉冲的时间。在PWM脉冲之间，暂停是PWMwidth/2。如果省略，默认值为10毫秒。</param>
        /// <param name="threshold" type=Real(4)>毫米为单位指定最小运动：如果任何频率的最大运动超过阈值，则认为操作成功；否则报错。如果省略，默认值为 0.01 毫米。</param>
        /// <returns></returns>
        public bool ActivateCalibration(float pwmValue, float pwmWidth, float threshold)
        {
            return ExecuteCommand(XMSCommandID.CONFIG, "激活校准", 80F, pwmValue, pwmWidth, threshold);
        }

        /// <summary>
        /// 获取变量信息
        /// 返回有关由ID参数标识的变量的信息。
        /// </summary>
        /// <param name="Id">通道</param>
        /// <param name="bit">变量比例</param>
        /// <returns></returns>
        public T GetVar<T>(ushort Id, float bit = float.NaN)
        {
            byte[] bytes = ExecuteQuery(XMSCommandID.GETVAR, "获取变量信息", Id, bit);
            var ret = ToValue<T>(bytes);
            return ret;
        }
        #endregion 公有方法 start

        #region    私有方法 start
        /// <summary>
        /// 发送并且接收字节数组
        /// </summary>
        /// <param name="sendbuffer"></param>
        /// <param name="recbuffer"></param>
        /// <returns></returns>
        private byte[] SendDataRec(byte[] sendbuffer)
        {
            List<byte> total = new List<byte>();
            try
            {
                // 发送数据
                serialPort.Write(sendbuffer, 0, sendbuffer.Length);

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
                        Thread.Sleep(1);
                        if ((DateTime.Now - startTime).TotalMilliseconds > RTUDefault.ReciveTimeout)
                        {// 超时
                            return null;
                        }
                        else if (total.Count >= 5 && serialPort.BytesToRead == 0)
                        {// 接收完了
                         //Trace.WriteLine($"serialPort.BytesToRead：{serialPort.BytesToRead}");
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
            return total.ToArray();
        }

        /// <summary>
        /// 异步发送并且接收字节数组
        /// </summary>
        /// <param name="sendbuffer"></param>
        /// <param name="recbuffer"></param>
        /// <returns></returns>
        private Task<byte[]> SendDataRecAsync(byte[] sendbuffer)
        {
            return Task.Factory.StartNew<byte[]>(() =>
            {
                List<byte> total = new List<byte>();
                try
                {
                    // 发送数据
                    serialPort.Write(sendbuffer, 0, sendbuffer.Length);

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
                            Thread.Sleep(1);
                            if ((DateTime.Now - startTime).TotalMilliseconds > RTUDefault.ReciveTimeout)
                            {// 超时
                                return null;
                            }
                            else if (total.Count >= 5 && serialPort.BytesToRead == 0)
                            {// 接收完了
                             //Trace.WriteLine($"serialPort.BytesToRead：{serialPort.BytesToRead}");
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
                return total.ToArray();
            });
        }

        /// <summary>
        /// 发指令
        /// </summary>
        /// <param name="xmsCmd">XMS命令</param>
        /// <param name="varId">变量的ID</param>
        /// <param name="value">指令值</param>
        /// <returns></returns>
        private byte[] WriteCommand(ushort xmsCmd, ushort? varId, float value = float.NaN)
        {
            //协议：  E4 A5 + 目标地址 + 命令体长度 + XMS命令 + 变量的ID + 指令值
            //字节数：  2   + 1       + 1         + 1     +  2       + 4
            byte[] rev = null;
            try
            {
                List<byte> bytes = new List<byte>();

                bytes.Add((byte)xmsCmd);

                if (varId.HasValue)
                {
                    bytes.Add((byte)varId.Value);
                }

                if (!(value is float.NaN))
                {
                    //byte[] valArr = ByteArrayLib.GetByteArrayFromFloat(value, Endianness.DCBA);
                    byte[] valArr = value.ToByteArray(Endianness.DCBA);
                    bytes.AddRange(valArr);
                }

                int bodyLen = bytes.Count;
                bytes.Insert(0, (byte)bodyLen);// 命令体长度

                bytes.Insert(0, (byte)DestAddr);// 目标地址
                bytes.InsertRange(0, PrefixBytes);// 固定前缀

                byte[] sendbuffer = bytes.ToArray();
                if (serialPort.IsOpen)
                {
                    serialPort.DiscardInBuffer();
                    serialPort.DiscardOutBuffer();
                    rev = SendDataRec(sendbuffer);
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"WriteCommand异常=>{ex.Message}");
                throw;
            }
            return rev;
        }

        /// <summary>
        /// 发指令
        /// </summary>
        /// <param name="xmsCmd">XMS命令</param>
        /// <param name="paras">多种类型的指令值</param>
        /// <returns></returns>
        private byte[] WriteCommand(ushort xmsCmd, params object[] paras)
        {
            //协议：  E4 A5 + 目标地址 + 命令体长度 + XMS命令 + 多个指令值(最多10个)
            //字节数：  2   + 1       + 1         + 1     +  n
            byte[] rev = null;
            try
            {
                List<byte> bytes = new List<byte>();

                bytes.Add((byte)xmsCmd);

                if (paras != null)
                {
                    foreach (var param in paras)
                    {
                        var type = param.GetType();
                        var byteNum = Marshal.SizeOf(param);
                        switch (byteNum)
                        {
                            case 1:
                                bytes.Add((byte)param);
                                break;
                            case 2:
                                byte[] valArr2 = null;
                                if (type == typeof(char))
                                {
                                    valArr2 = Encoding.Default.GetBytes(new char[] { (char)param });
                                }
                                else if (type == typeof(ushort))
                                {
                                    //valArr2 = ByteArrayLib.GetByteArrayFromUShort((ushort)param, Endianness.DCBA);
                                    valArr2 = ((ushort)param).ToByteArray(Endianness.DCBA);
                                }
                                else if (type == typeof(short))
                                {
                                    //valArr2 = ByteArrayLib.GetByteArrayFromShort((short)param, Endianness.DCBA);
                                    valArr2 = ((short)param).ToByteArray(Endianness.DCBA);
                                }
                                if (valArr2 != null)
                                {
                                    bytes.AddRange(valArr2);
                                }
                                break;
                            case 4:
                                byte[] valArr4 = null;
                                if (type == typeof(uint))
                                {
                                    //valArr4 = ByteArrayLib.GetByteArrayFromUInt((uint)param, Endianness.DCBA);
                                    valArr4 = ((uint)param).ToByteArray(Endianness.DCBA);
                                }
                                else if (type == typeof(int))
                                {
                                    //valArr4 = ByteArrayLib.GetByteArrayFromInt((int)param, Endianness.DCBA);
                                    valArr4 = ((int)param).ToByteArray(Endianness.DCBA);
                                }
                                else if (type == typeof(float) && !(param is float.NaN))
                                {
                                    //valArr4 = ByteArrayLib.GetByteArrayFromFloat((float)param, Endianness.DCBA);
                                    valArr4 = ((float)param).ToByteArray(Endianness.DCBA);
                                }
                                if (valArr4 != null)
                                {
                                    bytes.AddRange(valArr4);
                                }
                                break;
                            case 8:
                                byte[] valArr8 = null;
                                if (type == typeof(ulong))
                                {
                                    //valArr8 = ByteArrayLib.GetByteArrayFromULong((ulong)param, Endianness.DCBA);
                                    valArr8 = ((ulong)param).ToByteArray(Endianness.DCBA);
                                }
                                else if (type == typeof(long))
                                {
                                    //valArr8 = ByteArrayLib.GetByteArrayFromLong((long)param, Endianness.DCBA);
                                    valArr8 = ((long)param).ToByteArray(Endianness.DCBA);
                                }
                                else if (type == typeof(double))
                                {
                                    //valArr8 = ByteArrayLib.GetByteArrayFromDouble((double)param, Endianness.DCBA);
                                    valArr8 = ((double)param).ToByteArray(Endianness.DCBA);
                                }
                                if (valArr8 != null)
                                {
                                    bytes.AddRange(valArr8);
                                }
                                break;
                        }
                    }
                }

                int bodyLen = bytes.Count;
                bytes.Insert(0, (byte)bodyLen);// 命令体长度

                bytes.Insert(0, (byte)DestAddr);// 目标地址
                bytes.InsertRange(0, PrefixBytes);// 固定前缀

                byte[] sendbuffer = bytes.ToArray();
                if (serialPort.IsOpen)
                {
                    serialPort.DiscardInBuffer();
                    serialPort.DiscardOutBuffer();
                    //#if DEBUG
                    //                    string hexString = StringLib.GetHexStringFromByteArray(sendbuffer);
                    //                    Trace.WriteLine($"WriteCommand=>sendbuffer={hexString}");
                    //#endif
                    rev = SendDataRec(sendbuffer);
                    //#if DEBUG
                    //                    hexString = StringLib.GetHexStringFromByteArray(rev);
                    //                    Trace.WriteLine($"WriteCommand=>rev={hexString}");
                    //#endif
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
            return rev;
        }

        /// <summary>
        /// 异步发指令
        /// </summary>
        /// <param name="xmsCmd">XMS命令</param>
        /// <param name="paras">多种类型的指令值</param>
        /// <returns></returns>
        private async Task<byte[]> WriteCommandAsync(ushort xmsCmd, params object[] paras)
        {
            //协议：  E4 A5 + 目标地址 + 命令体长度 + XMS命令 + 多个指令值(最多10个)
            //字节数：  2   + 1       + 1         + 1     +  n
            byte[] rev = null;
            try
            {
                List<byte> bytes = new List<byte>();

                bytes.Add((byte)xmsCmd);

                if (paras != null)
                {
                    foreach (var param in paras)
                    {
                        var type = param.GetType();
                        var byteNum = Marshal.SizeOf(param);
                        switch (byteNum)
                        {
                            case 1:
                                bytes.Add((byte)param);
                                break;
                            case 2:
                                byte[] valArr2 = null;
                                if (type == typeof(char))
                                {
                                    valArr2 = Encoding.Default.GetBytes(new char[] { (char)param });
                                }
                                else if (type == typeof(ushort))
                                {
                                    //valArr2 = ByteArrayLib.GetByteArrayFromUShort((ushort)param, Endianness.DCBA);
                                    valArr2 = ((ushort)param).ToByteArray(Endianness.DCBA);
                                }
                                else if (type == typeof(short))
                                {
                                    //valArr2 = ByteArrayLib.GetByteArrayFromShort((short)param, Endianness.DCBA);
                                    valArr2 = ((short)param).ToByteArray(Endianness.DCBA);
                                }
                                if (valArr2 != null)
                                {
                                    bytes.AddRange(valArr2);
                                }
                                break;
                            case 4:
                                byte[] valArr4 = null;
                                if (type == typeof(uint))
                                {
                                    //valArr4 = ByteArrayLib.GetByteArrayFromUInt((uint)param, Endianness.DCBA);
                                    valArr4 = ((uint)param).ToByteArray(Endianness.DCBA);
                                }
                                else if (type == typeof(int))
                                {
                                    //valArr4 = ByteArrayLib.GetByteArrayFromInt((int)param, Endianness.DCBA);
                                    valArr4 = ((int)param).ToByteArray(Endianness.DCBA);
                                }
                                else if (type == typeof(float) && !(param is float.NaN))
                                {
                                    //valArr4 = ByteArrayLib.GetByteArrayFromFloat((float)param, Endianness.DCBA);
                                    valArr4 = ((float)param).ToByteArray(Endianness.DCBA);
                                }
                                if (valArr4 != null)
                                {
                                    bytes.AddRange(valArr4);
                                }
                                break;
                            case 8:
                                byte[] valArr8 = null;
                                if (type == typeof(ulong))
                                {
                                    //valArr8 = ByteArrayLib.GetByteArrayFromULong((ulong)param, Endianness.DCBA);
                                    valArr8 = ((ulong)param).ToByteArray(Endianness.DCBA);
                                }
                                else if (type == typeof(long))
                                {
                                    //valArr8 = ByteArrayLib.GetByteArrayFromLong((long)param, Endianness.DCBA);
                                    valArr8 = ((long)param).ToByteArray(Endianness.DCBA);
                                }
                                else if (type == typeof(double))
                                {
                                    //valArr8 = ByteArrayLib.GetByteArrayFromDouble((double)param, Endianness.DCBA);
                                    valArr8 = ((double)param).ToByteArray(Endianness.DCBA);
                                }
                                if (valArr8 != null)
                                {
                                    bytes.AddRange(valArr8);
                                }
                                break;
                        }
                    }
                }

                int bodyLen = bytes.Count;
                bytes.Insert(0, (byte)bodyLen);// 命令体长度

                bytes.Insert(0, (byte)DestAddr);// 目标地址
                bytes.InsertRange(0, PrefixBytes);// 固定前缀

                byte[] sendbuffer = bytes.ToArray();
                if (serialPort.IsOpen)
                {
                    serialPort.DiscardInBuffer();
                    serialPort.DiscardOutBuffer();
                    //#if DEBUG
                    //                    string hexString = StringLib.GetHexStringFromByteArray(sendbuffer);
                    //                    Trace.WriteLine($"WriteCommand=>sendbuffer={hexString}");
                    //#endif
                    rev = await SendDataRecAsync(sendbuffer);
                    //#if DEBUG
                    //                    hexString = StringLib.GetHexStringFromByteArray(rev);
                    //                    Trace.WriteLine($"WriteCommand=>rev={hexString}");
                    //#endif
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
            return rev;
        }

        /// <summary>
        /// 执行命令并返回是否成功
        /// </summary>
        /// <param name="commandId">命令ID</param>
        /// <param name="msgPrefix">消息前缀</param>
        /// <param name="paras">命令参数</param>
        /// <returns>true表示命令执行成功，false表示失败</returns>
        private bool ExecuteCommand(ushort commandId, string msgPrefix, params object[] paras)
        {
            lock (lockObj)
            {
                bool ret = false;
                var msg = string.Empty;
                var rev = WriteCommand(commandId, paras);
                if (rev == null || rev.Length < 6)
                {
                    msg = $"ExecuteCommand=>{msgPrefix}失败：接收数据为空或者数据长度有误！";
#if DEBUG
                    Trace.WriteLine(msg);
#endif
                    throw new Exception(msg);
                }

                if (rev[3] != 2)
                {
                    msg = $"ExecuteCommand=>{msgPrefix}失败：接收数据长度有误！";
#if DEBUG
                    Trace.WriteLine(msg);
#endif
                    throw new Exception(msg);
                }

                if (rev[4] != commandId)
                {
                    msg = $"ExecuteCommand=>{msgPrefix}失败：接收数据不是预期的响应！";
#if DEBUG
                    Trace.WriteLine(msg);
#endif
                    throw new Exception(msg);
                }

                ret = rev[5] == 1;
                //var msg = ret ? $"{prefix}成功" : $"{prefix}失败";
                return ret;
            }
        }

        /// <summary>
        /// 异步执行命令并返回是否成功
        /// </summary>
        /// <param name="commandId">命令ID</param>
        /// <param name="msgPrefix">消息前缀</param>
        /// <param name="paras">命令参数</param>
        /// <returns>true表示命令执行成功，false表示失败</returns>
        private async Task<bool> ExecuteCommandAsync(ushort commandId, string msgPrefix, params object[] paras)
        {
            bool ret = false;
            var msg = string.Empty;
            var rev = await WriteCommandAsync(commandId, paras);
            if (rev == null || rev.Length < 6)
            {
                msg = $"ExecuteCommand=>{msgPrefix}失败：接收数据为空或者数据长度有误！";
#if DEBUG
                Trace.WriteLine(msg);
#endif
                throw new Exception(msg);
            }

            if (rev[3] != 2)
            {
                msg = $"ExecuteCommand=>{msgPrefix}失败：接收数据长度有误！";
#if DEBUG
                Trace.WriteLine(msg);
#endif
                throw new Exception(msg);
            }

            if (rev[4] != commandId)
            {
                msg = $"ExecuteCommand=>{msgPrefix}失败：接收数据不是预期的响应！";
#if DEBUG
                Trace.WriteLine(msg);
#endif
                throw new Exception(msg);
            }

            ret = rev[5] == 1;
            //var msg = ret ? $"{prefix}成功" : $"{prefix}失败";
            return ret;
        }

        /// <summary>
        /// 执行命令并返回数据
        /// </summary>
        /// <param name="commandId">命令ID</param>
        /// <param name="msgPrefix">消息前缀</param>
        /// <param name="paras">命令参数</param>
        /// <returns></returns>
        private byte[] ExecuteQuery(ushort commandId, string msgPrefix, params object[] paras)
        {
            lock (lockObj)
            {
                var msg = string.Empty;
                var rev = WriteCommand(commandId, paras);
                if (rev == null || rev.Length < 6)
                {
                    msg = $"ExecuteQuery=>{msgPrefix}失败：接收数据为空或者数据长度有误！";
                    Trace.WriteLine(msg);
                    throw new Exception(msg);
                }

                //if (rev[3] != 2)
                //{
                //    msg = $"ExecuteQuery=>{msgPrefix}失败：接收数据长度有误！";
                //    Trace.WriteLine(msg);
                //    throw new Exception(msg);
                //}

                if (rev[4] != commandId)
                {
                    msg = $"ExecuteQuery=>{msgPrefix}失败：接收数据不是预期的响应！";
                    Trace.WriteLine(msg);
                    throw new Exception(msg);
                }

                if (rev[5] != 1)
                {
                    msg = $"ExecuteQuery=>{msgPrefix}失败：命令被拒绝！";
                    Trace.WriteLine(msg);
                    throw new Exception(msg);
                }
                return rev;
            }
        }

        /// <summary>
        /// 异步执行命令并返回数据
        /// </summary>
        /// <param name="commandId">命令ID</param>
        /// <param name="msgPrefix">消息前缀</param>
        /// <param name="paras">命令参数</param>
        /// <returns></returns>
        private async Task<byte[]> ExecuteQueryAsync(ushort commandId, string msgPrefix, params object[] paras)
        {
            var msg = string.Empty;
            var rev = await WriteCommandAsync(commandId, paras);
            if (rev == null || rev.Length < 6)
            {
                msg = $"ExecuteQuery=>{msgPrefix}失败：接收数据为空或者数据长度有误！";
                Trace.WriteLine(msg);
                throw new Exception(msg);
            }

            //if (rev[3] != 2)
            //{
            //    msg = $"ExecuteQuery=>{msgPrefix}失败：接收数据长度有误！";
            //    Trace.WriteLine(msg);
            //    throw new Exception(msg);
            //}

            if (rev[4] != commandId)
            {
                msg = $"ExecuteQuery=>{msgPrefix}失败：接收数据不是预期的响应！";
                Trace.WriteLine(msg);
                throw new Exception(msg);
            }

            if (rev[5] != 1)
            {
                msg = $"ExecuteQuery=>{msgPrefix}失败：命令被拒绝！";
                Trace.WriteLine(msg);
                throw new Exception(msg);
            }
            return rev;
        }

        /// <summary>
        /// 接收到的数据转换为指定的常用类型的数据
        /// </summary>
        /// <typeparam name="T">指定的常用类型的数据</typeparam>
        /// <param name="bytes">接收到的数据</param>
        /// <returns></returns>
        private T ToValue<T>(byte[] bytes)
        {
            dynamic val = default(T);
            try
            {
                int bodyLen = (int)bytes[3] - 2;
                var data = new byte[bodyLen];
                Buffer.BlockCopy(bytes, 6, data, 0, bodyLen);

                //var type = typeof(T);
                val = data.ToValue<T>();
            }
            catch
            {
                throw;
            }
            return val;
        }

        /// <summary>
        /// 接收到的数据转换为指定的常用类型的数据
        /// </summary>
        /// <typeparam name="T">指定的常用类型的数据</typeparam>
        /// <param name="bytes">接收到的数据</param>
        /// <returns></returns>
        private List<T> ToValues<T>(byte[] bytes, int inArgCounts)
        {
            List<T> values = new List<T>();
            try
            {
                var startOffset = 6;
                int bodyLen = (int)bytes[3] - 2;// 有效数据长度

                var stepOffset = bodyLen / inArgCounts;// 每个参数取值的偏移长度
                var data = new byte[stepOffset];

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

        /// <summary>
        /// 接收到的数据转换为指定的常用类型的数据
        /// </summary>
        /// <param name="bytes">指定的常用类型的数据</param>
        /// <param name="inArgTypes">入参数据类型的集合</param>
        /// <returns></returns>
        private ArrayList ToValues(byte[] bytes, List<Type> inArgTypes)
        {
            ArrayList values = new ArrayList();
            try
            {
                var startOffset = 6;
                int bodyLen = (int)bytes[3] - 2;

                foreach (var type in inArgTypes)
                {
                    var typeLen = Marshal.SizeOf(type);
                    var data = new byte[typeLen];

                    Buffer.BlockCopy(bytes, startOffset, data, 0, typeLen);
                    var val = Convert.ChangeType(data, type);
                    values.Add(val);

                    startOffset += typeLen;
                }
            }
            catch
            {
                throw;
            }
            return values;
        }
        #endregion 私有方法 start
    }
}