using System.Runtime.InteropServices;

namespace dbe.FoundationLibrary.Communication.CustComm.XCD_EDGE_Motor
{
    /// <summary>
    /// EDGE电机系统命令(Name = ID)
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Size = 1)]
    public struct XMSCommandID
    {
        /// <summary>
        /// 移动到绝对位置
        /// 位置以用户单位定义新的目标位置。
        /// 如果之前未执行命令启用，则命令移动也启用伺服回路。
        /// 运动执行后，伺服回路保持启用状态，电机保持其最终位置。
        /// </summary>
        public static ushort MOVE = 1;
        /// <summary>
        /// 分配。该值被分配给变量。
        /// </summary>
        public static ushort ASSIGN_INT16 = 2;
        /// <summary>
        /// 分配。
        /// 该值被分配给变量。ID和参数在同一个ASSIGN命令中最多可以重复8次。
        /// </summary>
        public static ushort ASSIGN = 3;
        /// <summary>
        /// 设置一个特殊变量
        /// 该命令类似于赋值，但与常规赋值不同，它会导致特殊操作。
        /// 该命令适用于只读且无法在常规赋值中寻址的有限变量集。命令中只能指定以下变量：
        /// ‧  set  FPOS=  定义新的电机位置。
        /// ‧  set  TIME=0  该命令将TIME重置为零并重新开始计数。右侧的值被忽略；计数从零开始。
        /// ‧  set  S_IND=0  该命令将S_IND重置为零并重新启动position23锁存功能。
        /// </summary>
        public static ushort SET = 3;
        /// <summary>
        /// 启动复位操作
        /// 该命令包含复位方法、原点和可选速度。有关命令示例，请参阅第69页的舞台位置信息。
        /// </summary>
        public static ushort HOME = 4;
        /// <summary>
        /// 执行速度环控制
        /// 速度参数定义了平台所需的速度（每秒线性或旋转单位）。
        /// </summary>
        public static ushort VELOCITY_LOOP = 6;
        /// <summary>
        /// 执行开环控制
        /// 参数命令以百分比定义命令值，从‑100到+100。
        /// </summary>
        public static ushort OPEN_LOOP = 7;
        /// <summary>
        /// 0x0A
        /// </summary>
        public static ushort NOP = 10;
        /// <summary>
        /// 将参数值保存到闪存 0x0D
        /// 在下一次启动时，控制器从闪存中读取参数并以存储的参数而不是默认值启动。
        /// </summary>
        public static ushort SAVE_FLASH = 13;
        /// <summary>
        /// 读取闪存值 0x0E
        /// </summary>
        public static ushort RESTORE_FLASH = 14;
        /// <summary>
        /// 设置串口 0x0F
        /// </summary>
        public static ushort SET_SERIAL = 15;
        /// <summary>
        /// 更改通讯地址 0x10
        /// </summary>
        public static ushort SET_ADDRESS = 16;
        /// <summary>
        /// 启用伺服回路 0x11
        /// 伺服环路使能时，电机主动保持当前位置并修正外力引起的偏差。该命令相当于MOVE FPOS。
        /// </summary>
        public static ushort ENABLE = 17;
        /// <summary>
        /// 禁用伺服回路 0x12
        /// 当伺服回路被禁用时，电机被动地抵抗外力。压电马达提供相对较高的被动力，在许多情况下足以维持其位置。
        /// </summary>
        public static ushort DISABLE = 18;
        /// <summary>
        /// 该命令请求有关控制器固件的信息 0x13
        /// 有关控制器回复的格式，请参阅第59页的特定命令的回复正文。
        /// </summary>
        public static ushort READ_VERSION = 19;
        /// <summary>
        /// 监控变量 0x14
        /// 收到指令后，控制器在每个周期中使用刻度转换变量并将其传递到模拟输出。
        /// </summary>
        public static ushort MONITOR = 20;
        /// <summary>
        /// 监控变量 0x15
        /// 收到命令后，控制器在每个周期中使用刻度转换指定的RAM地址并将其传递到模拟输出。
        /// </summary>
        public static ushort MONITOR_ADDRESS = 21;
        /// <summary>
        ///  0x16
        /// </summary>
        public static ushort MODE = 22;
        /// <summary>
        /// 以KDEC速率导致运动减速 0x17
        /// 此命令不会禁用电机。
        /// </summary>
        public static ushort KILL = 23;
        /// <summary>
        /// 该命令请求当前变量值 0x19
        /// 一个命令可以请求1到10个变量。除了XMS变量之外，该命令还接受伪变量的ID。
        /// </summary>
        public static ushort REPORT_INT16 = 25;
        /// <summary>
        /// 报告变量值 0x1A
        /// 该命令请求当前变量值。一个命令可以请求1到10个变量。除了XMS变量之外，该命令还接受伪变量的ID。
        /// </summary>
        public static ushort REPORT = 26;
        /// <summary>
        /// 反馈方向 0x1E
        /// </summary>
        public static ushort FEEDBACK_DIRECTION = 30;
        /// <summary>
        /// 输出方向 0x1F
        /// </summary>
        public static ushort OUT_DIRECTION = 31;
        /// <summary>
        /// 配置控制器
        /// </summary>
        public static ushort CONFIG = 32;
        /// <summary>
        /// 开始位置比较脉冲生成（增量）
        /// </summary>
        public static ushort PPI = 33;
        public static ushort PWM_DATUM = 38;
        public static ushort ENABLE_SAFETY_INPUTS = 39;
        public static ushort Write_Program = 42;
        public static ushort Read_Program = 43;
        public static ushort Validate_Program = 44;
        public static ushort Execute_Program = 45;
        public static ushort Stop_Program = 46;
        public static ushort Debug_Program = 47;
        public static ushort Pause_Program = 48;
        public static ushort Resume_Program = 49;
        public static ushort Get_Stack = 50;
        public static ushort Erase_Flash = 51;
        public static ushort Write_Flash = 52;
        public static ushort Read_Flash = 53;
        public static ushort Validate_Flash = 54;
        /// <summary>
        /// 控制器返回配置文件中定义的修订字符串(最多30个ASCII字符)。
        /// </summary>
        public static ushort GET_DESCRIPTOR = 55;
        public static ushort Set_Descriptor = 56;
        public static ushort Resave_Flash = 63;
        public static ushort Override_Otp = 64;
        /// <summary>
        /// UHR功能在运动曲线期间提供较慢的电机运动。
        /// 这种较慢的移动提高了载物台移动的准确性。请参阅超高分辨率(UHR)，第16页
        /// </summary>
        public static ushort UHR = 72;
        /// <summary>
        /// 激活校准
        /// </summary>
        //public static ushort CONFIG = 80;
        /// <summary>
        /// 返回有关由ID参数标识的变量的信息。
        /// </summary>
        public static ushort GETVAR = 960;
    }
}