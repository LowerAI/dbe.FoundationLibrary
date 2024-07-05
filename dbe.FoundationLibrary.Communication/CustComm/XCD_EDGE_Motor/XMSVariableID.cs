using System.Runtime.InteropServices;

namespace dbe.FoundationLibrary.Communication.CustComm.XCD_EDGE_Motor
{
    /// <summary>
    /// XCD EDGE电机系统内置变量(Name = ID)
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Size = 1)]
    public struct XMSVariableID
    {
        /// <summary>
        /// 未知
        /// </summary>
        //public static ushort Unknown = -1; // 0xFFFF
        /// <summary>
        /// 位置
        /// </summary>
        public static ushort POS = 0;
        /// <summary>
        /// 速度
        /// </summary>
        public static ushort VEL = 1;
        /// <summary>
        /// 加速度
        /// </summary>
        public static ushort ACC = 2;
        /// <summary>
        /// 减速度
        /// </summary>
        public static ushort DEC = 3;
        /// <summary>
        /// 终止减速 ‑ 用于故障情况；例如，如果限位开关被激活。
        /// </summary>
        public static ushort KDEC = 4;
        /// <summary>
        /// 目标位置
        /// </summary>
        public static ushort TPOS = 5;
        /// <summary>
        /// 参考位置
        /// </summary>
        public static ushort RPOS = 6;
        /// <summary>
        /// 参考速度
        /// </summary>
        public static ushort RVEL = 7;
        /// <summary>
        /// 参考加速度
        /// </summary>
        public static ushort RACC = 8;
        /// <summary>
        /// 反馈位置
        /// </summary>
        public static ushort FPOS = 9;
        /// <summary>
        /// 反馈速度 0x000A
        /// </summary>
        public static ushort FVEL = 10;
        /// <summary>
        /// 反馈速度 0x000A
        /// </summary>
        public static ushort FACC = 11;
        /// <summary>
        /// 位置误差
        /// </summary>
        public static ushort PE = 12;
        /// <summary>
        /// 位置环增益
        /// </summary>
        public static ushort KP = 13;
        /// <summary>
        /// 速度环增益
        /// </summary>
        public static ushort KV = 14;
        /// <summary>
        /// 积分增益
        /// </summary>
        public static ushort KI = 15;
        /// <summary>
        /// 速度环积分器限制
        /// </summary>
        public static ushort LI = 16;
        /// <summary>
        /// 第一个双二阶滤波器参数1
        /// </summary>
        public static ushort BQA1 = 17;
        /// <summary>
        /// 第一个双二阶滤波器参数2
        /// </summary>
        public static ushort BQA2 = 18;
        /// <summary>
        /// 第一个双二阶滤波器参数3
        /// </summary>
        public static ushort BQB0 = 19;
        /// <summary>
        /// 第一个双二阶滤波器参数4
        /// </summary>
        public static ushort BQB1 = 20;
        /// <summary>
        /// 第一个双二阶滤波器参数5
        /// </summary>
        public static ushort BQB2 = 21;
        /// <summary>
        /// 编码器分辨率（毫米每一个编码器计数）
        /// </summary>
        public static ushort ENR = 22;
        /// <summary>
        /// 电机频率（PWM频率）
        /// </summary>
        public static ushort MFREQ = 23;
        /// <summary>
        /// 伺服回路采样周期(毫秒)
        /// </summary>
        public static ushort SPRD = 24;
        public static ushort DMODE = 25;
        public static ushort MFREQ1 = 26;
        public static ushort PWMZERO = 27;
        public static ushort PWMMIN = 28;
        public static ushort PWMMAX = 29;
        /// <summary>
        /// 模拟输入0(%)
        /// </summary>
        public static ushort AIN0 = 30;
        /// <summary>
        /// 模拟输入1(%)
        /// </summary>
        public static ushort AIN1 = 31;
        /// <summary>
        /// 模拟输入2(%)
        /// </summary>
        public static ushort AIN2 = 32;
        /// <summary>
        /// 模拟输入3(%)
        /// </summary>
        public static ushort AIN3 = 33;
        /// <summary>
        /// 模拟输出0(%)
        /// </summary>
        public static ushort AOUT0 = 34;
        /// <summary>
        /// 模拟输出1(%)
        /// </summary>
        public static ushort AOUT1 = 35;
        /// <summary>
        /// 模拟输出2(%)
        /// </summary>
        public static ushort AOUT2 = 36;
        /// <summary>
        /// 模拟输出3(%)
        /// </summary>
        public static ushort AOUT3 = 37;
        /// <summary>
        /// 以毫秒为单位的经过时间
        /// </summary>
        public static ushort TIME = 38;
        /// <summary>
        /// 驱动输出限制(最大输出的百分比)
        /// </summary>
        public static ushort DOL = 39;
        /// <summary>
        /// 死区最小值（Nanomotion 专有算法)
        /// </summary>
        public static ushort DZMIN = 40;
        /// <summary>
        /// 死区最大值（Nanomotion 专有算法)
        /// </summary>
        public static ushort DZMAX = 41;
        /// <summary>
        /// 零前馈（Nanomotion 专有算法)
        /// </summary>
        public static ushort ZFF = 42;
        /// <summary>
        /// 正方向摩擦
        /// </summary>
        public static ushort FRP = 43;
        /// <summary>
        /// 负方向摩擦
        /// </summary>
        public static ushort FRN = 44;
        /// <summary>
        /// 即时驱动输出(最大输出的百分比)
        /// </summary>
        public static ushort DOUT = 45;
        public static ushort SCO = 46;
        /// <summary>
        /// 软件正限位
        /// </summary>
        public static ushort SLP = 47;
        /// <summary>
        /// 软件负限位
        /// </summary>
        public static ushort SLN = 48;
        /// <summary>
        /// 位置误差限制
        /// </summary>
        public static ushort PEL = 49;
        public static ushort TEL = 50;
        /// <summary>
        /// 运动时间限制
        /// </summary>
        public static ushort MTL = 51;
        /// <summary>
        /// 位置锁定在索引脉冲上
        /// </summary>
        public static ushort POSI = 52;
        /// <summary>
        /// 驱动器输出偏移(最大输出的百分比)
        /// </summary>
        public static ushort DOFFS = 53;
        /// <summary>
        /// 以毫秒为单位的位置比较脉冲宽度
        /// </summary>
        public static ushort PPW = 54;
        /// <summary>
        /// 模拟输入4(%)
        /// </summary>
        public static ushort AIN4 = 55;
        /// <summary>
        /// 模拟输入5(%)
        /// </summary>
        public static ushort AIN5 = 56;
        /// <summary>
        /// 模拟输入6(%)
        /// </summary>
        public static ushort AIN6 = 57;
        /// <summary>
        /// 模拟输入7(%)
        /// </summary>
        public static ushort AIN7 = 58;
        /// <summary>
        /// 模拟输入8(%)
        /// </summary>
        public static ushort AIN8 = 59;
        /// <summary>
        /// 模拟输入9(%)
        /// </summary>
        public static ushort AIN9 = 60;
        /// <summary>
        /// 模拟输入10(%)
        /// </summary>
        public static ushort AIN10 = 61;
        /// <summary>
        /// 模拟输入11(%)
        /// </summary>
        public static ushort AIN11 = 62;
        /// <summary>
        /// 模拟输入12(%)
        /// </summary>
        public static ushort AIN12 = 63;
        /// <summary>
        /// 模拟输入13(%)
        /// </summary>
        public static ushort AIN13 = 64;
        /// <summary>
        /// 模拟输入14(%)
        /// </summary>
        public static ushort AIN14 = 65;
        /// <summary>
        /// 模拟输入15(%)
        /// </summary>
        public static ushort AIN15 = 66;
        /// <summary>
        /// 第二个双二阶滤波器参数1
        /// </summary>
        public static ushort BQ2A1 = 67;
        /// <summary>
        /// 第二个双二阶滤波器参数2
        /// </summary>
        public static ushort BQ2A2 = 68;
        /// <summary>
        /// 第二个双二阶滤波器参数3
        /// </summary>
        public static ushort BQ2B0 = 69;
        /// <summary>
        /// 第二个双二阶滤波器参数4
        /// </summary>
        public static ushort BQ2B1 = 70;
        /// <summary>
        /// 第二个双二阶滤波器参数5
        /// </summary>
        public static ushort BQ2B2 = 71;
        /// <summary>
        /// 用户变量V0
        /// </summary>
        public static ushort V0 = 1000;
        /// <summary>
        /// 用户变量V1
        /// </summary>
        public static ushort V1 = 1001;
        /// <summary>
        /// 用户变量V2
        /// </summary>
        public static ushort V2 = 1002;
        /// <summary>
        /// 用户变量V3
        /// </summary>
        public static ushort V3 = 1003;
        /// <summary>
        /// 用户变量V4
        /// </summary>
        public static ushort V4 = 1004;
        /// <summary>
        /// 用户变量V5
        /// </summary>
        public static ushort V5 = 1005;
        /// <summary>
        /// 用户变量V6
        /// </summary>
        public static ushort V6 = 1006;
        /// <summary>
        /// 用户变量V7
        /// </summary>
        public static ushort V7 = 1007;
        /// <summary>
        /// 用户变量V8
        /// </summary>
        public static ushort V8 = 1008;
        /// <summary>
        /// 用户变量V9
        /// </summary>
        public static ushort V9 = 1009;
        /// <summary>
        /// 用户变量V10
        /// </summary>
        public static ushort V10 = 1010;
        /// <summary>
        /// 用户变量V11
        /// </summary>
        public static ushort V11 = 1011;
        /// <summary>
        /// 用户变量V12
        /// </summary>
        public static ushort V12 = 1012;
        /// <summary>
        /// 用户变量V13
        /// </summary>
        public static ushort V13 = 1013;
        /// <summary>
        /// 用户变量V14
        /// </summary>
        public static ushort V14 = 1014;
        /// <summary>
        /// 用户变量V15
        /// </summary>
        public static ushort V15 = 1015;
        /// <summary>
        /// 用户变量V16
        /// </summary>
        public static ushort V16 = 1016;
        /// <summary>
        /// 用户变量V17
        /// </summary>
        public static ushort V17 = 1017;
        /// <summary>
        /// 用户变量V18
        /// </summary>
        public static ushort V18 = 1018;
        /// <summary>
        /// 用户变量V19
        /// </summary>
        public static ushort V19 = 1019;
        /// <summary>
        /// 数字输入/输出0
        /// </summary>
        public static ushort IO_0 = 2000;
        /// <summary>
        /// 数字输入/输出1
        /// </summary>
        public static ushort IO_1 = 2001;
        /// <summary>
        /// 数字输入/输出2
        /// </summary>
        public static ushort IO_2 = 2002;
        /// <summary>
        /// 数字输入/输出3
        /// </summary>
        public static ushort IO_3 = 2003;
        /// <summary>
        /// 数字输入/输出4
        /// </summary>
        public static ushort IO_4 = 2004;
        /// <summary>
        /// 数字输入/输出5
        /// </summary>
        public static ushort IO_5 = 2005;
        /// <summary>
        /// 数字输入/输出6
        /// </summary>
        public static ushort IO_6 = 2006;
        /// <summary>
        /// 数字输入/输出7
        /// </summary>
        public static ushort IO_7 = 2007;
        /// <summary>
        /// 运动队列已满
        /// 而运动队列是S_QUEUE满（S_QUEUE为1），nmove命令被禁用并返回错误。
        /// </summary>
        public static ushort S_QUEUE = 2008;
        /// <summary>
        /// 运动正在进行中。
        /// 该标志在运动开始时切换为1（例如，一旦执行移动命令）。一旦RPOS达到TPOS，该标志在运动结束时切换为0。
        /// </summary>
        public static ushort S_MOVE = 2009;
        /// <summary>
        /// 伺服回路忙。
        /// 当伺服回路处于活动状态时，该标志为1。在move/nmove命令之后，标志与S_MOVE同步切换到1，但通常在S_MOVE之后切换到0（零），一旦FPOS进入TPOS附近的±DZMIN间隔。
        /// </summary>
        public static ushort S_BUSY = 2010;
        /// <summary>
        /// 索引位置锁定
        /// 该标志指示是否遇到编码器索引并且POSI变量锁存了有效索引位置。有关详细信息，请参阅第7.2节。
        /// </summary>
        public static ushort S_IND = 2011;
        /// <summary>
        /// 复位标志
        /// 控制器上电后标志为0。成功完成Home操作后，控制器将标志切换为1。该标志表示绝对反馈位置可用。
        /// 该标志也可以在XMS脚本中或通过主机命令分配(3)分配。
        /// </summary>
        public static ushort S_HOME = 2012;
        /// <summary>
        /// 就位标志
        /// 标志在以下情况下切换为1：
        /// ‧  成功的运动终止
        /// ‧  位置误差PE进入±DZMIN周围目标位置
        /// ‧  中断时间已过（默认为6毫秒）
        /// 标志在以下情况下切换为0：
        /// ‧  运动开始时
        /// ‧  位置误差PE被强制超出±DZMAX区间
        /// </summary>
        public static ushort S_INPOS = 2013;
    }
}