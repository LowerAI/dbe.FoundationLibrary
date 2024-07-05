using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;

namespace dbe.FoundationLibrary.Communication.CustComm.XCD_EDGE_Motor
{
    /// <summary>
    /// XCD EDGE电机系统内置变量(Name = ID)
    /// </summary>
    public class XMSVariable
    {
        #region    字段 start
        private readonly MotorControl edgeMotor;
        private Stopwatch sw = Stopwatch.StartNew();
        #endregion 字段 end

        #region    属性 start
        /// <summary>
        /// 位置
        /// </summary>
        [Browsable(true), Category("所需的运动参数"), Description("滑块的位置"), ReadOnly(true)]
        public float POS { get; set; }
        /// <summary>
        /// 速度
        /// </summary>
        [Browsable(true), Category("所需的运动参数"), Description("滑块移动的速度"), ReadOnly(true)]
        public float VEL { get; set; }
        /// <summary>
        /// 加速度
        /// </summary>
        [Browsable(true), Category("所需的运动参数"), Description("滑块移动的加速度"), ReadOnly(true)]
        public float ACC { get; set; }
        /// <summary>
        /// 减速度
        /// </summary>
        [Browsable(true), Category("所需的运动参数"), Description("滑块移动的减速度"), ReadOnly(true)]
        public float DEC { get; set; }
        /// <summary>
        /// 终止减速 ‑ 用于故障情况；例如，如果限位开关被激活。
        /// </summary>
        [Browsable(true), Category("所需的运动参数"), Description("终止减速 ‑ 用于故障情况；例如，如果限位开关被激活。"), ReadOnly(true)]
        public float KDEC { get; set; }
        /// <summary>
        /// 目标位置
        /// </summary>
        [Browsable(true), Category("即时参考运动变量"), Description("目标位置"), ReadOnly(true)]
        public float TPOS { get; set; }
        /// <summary>
        /// 参考位置
        /// </summary>
        [Browsable(true), Category("即时参考运动变量"), Description("参考位置"), ReadOnly(true)]
        public float RPOS { get; set; }
        /// <summary>
        /// 参考速度
        /// </summary>
        [Browsable(true), Category("即时参考运动变量"), Description("参考速度"), ReadOnly(true)]
        public float RVEL { get; set; }
        /// <summary>
        /// 参考加速度
        /// </summary>
        [Browsable(true), Category("即时参考运动变量"), Description("参考加速度"), ReadOnly(true)]
        public float RACC { get; set; }
        /// <summary>
        /// 反馈位置
        /// </summary>
        [Browsable(true), Category("即时反馈运动变量"), Description("反馈位置"), ReadOnly(true)]
        public float FPOS { get; set; }
        /// <summary>
        /// 反馈速度
        /// </summary>
        [Browsable(true), Category("即时反馈运动变量"), Description("反馈速度"), ReadOnly(true)]
        public float FVEL { get; set; }
        /// <summary>
        /// 位置误差
        /// </summary>
        [Browsable(true), Category("即时反馈运动变量"), Description("位置误差"), ReadOnly(true)]
        public float PE { get; set; }
        /// <summary>
        /// 位置环增益
        /// </summary>
        [Browsable(true), Category("伺服回路和驱动器配置"), Description("位置环增益"), ReadOnly(true)]
        public float KP { get; set; }
        /// <summary>
        /// 速度环增益
        /// </summary>
        [Browsable(true), Category("伺服回路和驱动器配置"), Description("速度环增益"), ReadOnly(true)]
        public float KV { get; set; }
        /// <summary>
        /// 积分增益
        /// </summary>
        [Browsable(true), Category("伺服回路和驱动器配置"), Description("积分增益"), ReadOnly(true)]
        public float KI { get; set; }
        /// <summary>
        /// 速度环积分器限制
        /// </summary>
        [Browsable(true), Category("伺服回路和驱动器配置"), Description("速度环积分器限制"), ReadOnly(true)]
        public float LI { get; set; }
        /// <summary>
        /// 第一个双二阶滤波器参数1
        /// </summary>
        [Browsable(true), Category("伺服回路和驱动器配置"), Description("第一个双二阶滤波器参数1"), ReadOnly(true)]
        public float BQA1 { get; set; }
        /// <summary>
        /// 第一个双二阶滤波器参数2
        /// </summary>
        [Browsable(true), Category("伺服回路和驱动器配置"), Description("第一个双二阶滤波器参数2"), ReadOnly(true)]
        public float BQA2 { get; set; }
        /// <summary>
        /// 第一个双二阶滤波器参数3
        /// </summary>
        [Browsable(true), Category("伺服回路和驱动器配置"), Description("第一个双二阶滤波器参数3"), ReadOnly(true)]
        public float BQB0 { get; set; }
        /// <summary>
        /// 第一个双二阶滤波器参数4
        /// </summary>
        [Browsable(true), Category("伺服回路和驱动器配置"), Description(" 第一个双二阶滤波器参数4"), ReadOnly(true)]
        public float BQB1 { get; set; }
        /// <summary>
        /// 第一个双二阶滤波器参数5
        /// </summary>
        [Browsable(true), Category("伺服回路和驱动器配置"), Description("第一个双二阶滤波器参数5"), ReadOnly(true)]
        public float BQB2 { get; set; }
        /// <summary>
        /// 编码器分辨率（毫米每一个编码器计数）
        /// </summary>
        [Browsable(true), Category("伺服回路和驱动器配置"), Description("编码器分辨率（毫米每一个编码器计数）"), ReadOnly(true)]
        public float ENR { get; set; }
        /// <summary>
        /// 电机频率（PWM频率）
        /// </summary>
        [Browsable(true), Category("伺服回路和驱动器配置"), Description("电机频率（PWM频率）"), ReadOnly(true)]
        public float MFREQ { get; set; }
        /// <summary>
        /// 伺服回路采样周期(毫秒)
        /// </summary>
        [Browsable(true), Category("伺服回路和驱动器配置"), Description("伺服回路采样周期(毫秒)"), ReadOnly(true)]
        public float SPRD { get; set; }
        /// <summary>
        /// 模拟输入0(%)
        /// </summary>
        [Browsable(true), Category("模拟输入/输出"), Description("模拟输入0(%)"), ReadOnly(true)]
        public float AIN0 { get; set; }
        /// <summary>
        /// 模拟输入1(%)
        /// </summary>
        [Browsable(true), Category("模拟输入/输出"), Description("模拟输入1(%)"), ReadOnly(true)]
        public float AIN1 { get; set; }
        /// <summary>
        /// 模拟输入2(%)
        /// </summary>
        [Browsable(true), Category("模拟输入/输出"), Description("模拟输入2(%)"), ReadOnly(true)]
        public float AIN2 { get; set; }
        /// <summary>
        /// 模拟输入3(%)
        /// </summary>
        [Browsable(true), Category("模拟输入/输出"), Description("模拟输入3(%)"), ReadOnly(true)]
        public float AIN3 { get; set; }
        /// <summary>
        /// 模拟输出0(%)
        /// </summary>
        [Browsable(true), Category("模拟输入/输出"), Description("模拟输出0(%)"), ReadOnly(true)]
        public float AOUT0 { get; set; }
        /// <summary>
        /// 模拟输出1(%)
        /// </summary>
        [Browsable(true), Category("模拟输入/输出"), Description("模拟输出1(%)"), ReadOnly(true)]
        public float AOUT1 { get; set; }
        /// <summary>
        /// 模拟输出2(%)
        /// </summary>
        [Browsable(true), Category("模拟输入/输出"), Description("模拟输出2(%)"), ReadOnly(true)]
        public float AOUT2 { get; set; }
        /// <summary>
        /// 模拟输出3(%)
        /// </summary>
        [Browsable(true), Category("模拟输入/输出"), Description("模拟输出3(%)"), ReadOnly(true)]
        public float AOUT3 { get; set; }
        /// <summary>
        /// 以毫秒为单位的经过时间
        /// </summary>
        [Browsable(true), Category("时间变量"), Description("以毫秒为单位的经过时间"), ReadOnly(true)]
        public float TIME { get; set; }
        /// <summary>
        /// 驱动输出限制(最大输出的百分比)
        /// </summary>
        [Browsable(true), Category("安全"), Description("驱动输出限制(最大输出的百分比)"), ReadOnly(true)]
        public float DOL { get; set; }
        /// <summary>
        /// 死区最小值（Nanomotion 专有算法)
        /// </summary>
        [Browsable(true), Category("伺服回路和驱动器配置"), Description("死区最小值（Nanomotion 专有算法)"), ReadOnly(true)]
        public float DZMIN { get; set; }
        /// <summary>
        /// 死区最大值（Nanomotion 专有算法)
        /// </summary>
        [Browsable(true), Category("伺服回路和驱动器配置"), Description("死区最大值（Nanomotion 专有算法)"), ReadOnly(true)]
        public float DZMAX { get; set; }
        /// <summary>
        /// 零前馈（Nanomotion 专有算法)
        /// </summary>
        [Browsable(true), Category("伺服回路和驱动器配置"), Description("零前馈（Nanomotion 专有算法)"), ReadOnly(true)]
        public float ZFF { get; set; }
        /// <summary>
        /// 正方向摩擦
        /// </summary>
        [Browsable(true), Category("伺服回路和驱动器配置"), Description("正方向摩擦"), ReadOnly(true)]
        public float FRP { get; set; }
        /// <summary>
        /// 负方向摩擦
        /// </summary>
        [Browsable(true), Category("伺服回路和驱动器配置"), Description("负方向摩擦"), ReadOnly(true)]
        public float FRN { get; set; }
        /// <summary>
        /// 即时驱动输出(最大输出的百分比)
        /// </summary>
        [Browsable(true), Category("伺服回路和驱动器配置"), Description("即时驱动输出(最大输出的百分比)"), ReadOnly(true)]
        public float DOUT { get; set; }
        /// <summary>
        /// 软件正限位
        /// </summary>
        [Browsable(true), Category("安全"), Description("软件正限位"), ReadOnly(true)]
        public float SLP { get; set; }
        /// <summary>
        /// 软件负限位
        /// </summary>
        [Browsable(true), Category("安全"), Description("软件负限位"), ReadOnly(true)]
        public float SLN { get; set; }
        /// <summary>
        /// 位置误差限制
        /// </summary>
        [Browsable(true), Category("安全"), Description("位置误差限制"), ReadOnly(true)]
        public float PEL { get; set; }
        /// <summary>
        /// 运动时间限制
        /// </summary>
        [Browsable(true), Category("安全"), Description("运动时间限制"), ReadOnly(true)]
        public float MTL { get; set; }
        /// <summary>
        /// 位置锁定在索引脉冲上
        /// </summary>
        [Browsable(true), Category("即时反馈运动变量"), Description("位置锁定在索引脉冲上"), ReadOnly(true)]
        public float POSI { get; set; }
        /// <summary>
        /// 驱动器输出偏移(最大输出的百分比)
        /// </summary>
        [Browsable(true), Category("安全"), Description("驱动器输出偏移(最大输出的百分比)"), ReadOnly(true)]
        public float DOFFS { get; set; }
        /// <summary>
        /// 以毫秒为单位的位置比较脉冲宽度
        /// </summary>
        [Browsable(true), Category("位置比较变量"), Description("以毫秒为单位的位置比较脉冲宽度"), ReadOnly(true)]
        public float PPW { get; set; }
        /// <summary>
        /// 模拟输入4(%)
        /// </summary>
        [Browsable(true), Category("模拟输入/输出"), Description("模拟输入4(%)"), ReadOnly(true)]
        public float AIN4 { get; set; }
        /// <summary>
        /// 模拟输入5(%)
        /// </summary>
        [Browsable(true), Category("模拟输入/输出"), Description("模拟输入5(%)"), ReadOnly(true)]
        public float AIN5 { get; set; }
        /// <summary>
        /// 模拟输入6(%)
        /// </summary>
        [Browsable(true), Category("模拟输入/输出"), Description("模拟输入6(%)"), ReadOnly(true)]
        public float AIN6 { get; set; }
        /// <summary>
        /// 模拟输入7(%)
        /// </summary>
        [Browsable(true), Category("模拟输入/输出"), Description("模拟输入7(%)"), ReadOnly(true)]
        public float AIN7 { get; set; }
        /// <summary>
        /// 模拟输入8(%)
        /// </summary>
        [Browsable(true), Category("模拟输入/输出"), Description("模拟输入8(%)"), ReadOnly(true)]
        public float AIN8 { get; set; }
        /// <summary>
        /// 模拟输入9(%)
        /// </summary>
        [Browsable(true), Category("模拟输入/输出"), Description("模拟输入9(%)"), ReadOnly(true)]
        public float AIN9 { get; set; }
        /// <summary>
        /// 模拟输入10(%)
        /// </summary>
        [Browsable(true), Category("模拟输入/输出"), Description("模拟输入10(%)"), ReadOnly(true)]
        public float AIN10 { get; set; }
        /// <summary>
        /// 模拟输入11(%)
        /// </summary>
        [Browsable(true), Category("模拟输入/输出"), Description("模拟输入11(%)"), ReadOnly(true)]
        public float AIN11 { get; set; }
        /// <summary>
        /// 模拟输入12(%)
        /// </summary>
        [Browsable(true), Category("模拟输入/输出"), Description("模拟输入12(%)"), ReadOnly(true)]
        public float AIN12 { get; set; }
        /// <summary>
        /// 模拟输入13(%)
        /// </summary>
        [Browsable(true), Category("模拟输入/输出"), Description("模拟输入13(%)"), ReadOnly(true)]
        public float AIN13 { get; set; }
        /// <summary>
        /// 模拟输入14(%)
        /// </summary>
        [Browsable(true), Category("模拟输入/输出"), Description("模拟输入14(%)"), ReadOnly(true)]
        public float AIN14 { get; set; }
        /// <summary>
        /// 模拟输入15(%)
        /// </summary>
        [Browsable(true), Category("模拟输入/输出"), Description("模拟输入15(%)"), ReadOnly(true)]
        public float AIN15 { get; set; }
        /// <summary>
        /// 第二个双二阶滤波器参数1
        /// </summary>
        [Browsable(true), Category("伺服回路和驱动器配置"), Description("第二个双二阶滤波器参数1"), ReadOnly(true)]
        public float BQ2A1 { get; set; }
        /// <summary>
        /// 第二个双二阶滤波器参数2
        /// </summary>
        [Browsable(true), Category("伺服回路和驱动器配置"), Description("第二个双二阶滤波器参数2"), ReadOnly(true)]
        public float BQ2A2 { get; set; }
        /// <summary>
        /// 第二个双二阶滤波器参数3
        /// </summary>
        [Browsable(true), Category("伺服回路和驱动器配置"), Description("第二个双二阶滤波器参数3"), ReadOnly(true)]
        public float BQ2B0 { get; set; }
        /// <summary>
        /// 第二个双二阶滤波器参数4
        /// </summary>
        [Browsable(true), Category("伺服回路和驱动器配置"), Description("第二个双二阶滤波器参数4"), ReadOnly(true)]
        public float BQ2B1 { get; set; }
        /// <summary>
        /// 第二个双二阶滤波器参数5
        /// </summary>
        [Browsable(true), Category("伺服回路和驱动器配置"), Description("第二个双二阶滤波器参数5"), ReadOnly(true)]
        public float BQ2B2 { get; set; }
        /// <summary>
        /// 用户变量V0
        /// </summary>
        [Browsable(true), Category("用户变量"), Description("用户变量V0"), ReadOnly(true)]
        public float V0 { get; set; }
        /// <summary>
        /// 用户变量V1
        /// </summary>
        [Browsable(true), Category("用户变量"), Description("用户变量V1"), ReadOnly(true)]
        public float V1 { get; set; }
        /// <summary>
        /// 用户变量V2
        /// </summary>
        [Browsable(true), Category("用户变量"), Description("用户变量V2"), ReadOnly(true)]
        public float V2 { get; set; }
        /// <summary>
        /// 用户变量V3
        /// </summary>
        [Browsable(true), Category("用户变量"), Description("用户变量V3"), ReadOnly(true)]
        public float V3 { get; set; }
        /// <summary>
        /// 用户变量V4
        /// </summary>
        [Browsable(true), Category("用户变量"), Description("用户变量V4"), ReadOnly(true)]
        public float V4 { get; set; }
        /// <summary>
        /// 用户变量V5
        /// </summary>
        [Browsable(true), Category("用户变量"), Description("用户变量V5"), ReadOnly(true)]
        public float V5 { get; set; }
        /// <summary>
        /// 用户变量V6
        /// </summary>
        [Browsable(true), Category("用户变量"), Description("用户变量V6"), ReadOnly(true)]
        public float V6 { get; set; }
        /// <summary>
        /// 用户变量V7
        /// </summary>
        [Browsable(true), Category("用户变量"), Description("用户变量V7"), ReadOnly(true)]
        public float V7 { get; set; }
        /// <summary>
        /// 用户变量V8
        /// </summary>
        [Browsable(true), Category("用户变量"), Description("用户变量V8"), ReadOnly(true)]
        public float V8 { get; set; }
        /// <summary>
        /// 用户变量V9
        /// </summary>
        [Browsable(true), Category("用户变量"), Description("用户变量V9"), ReadOnly(true)]
        public float V9 { get; set; }
        /// <summary>
        /// 用户变量V10
        /// </summary>
        [Browsable(true), Category("用户变量"), Description("用户变量V10"), ReadOnly(true)]
        public float V10 { get; set; }
        /// <summary>
        /// 用户变量V11
        /// </summary>
        [Browsable(true), Category("用户变量"), Description("用户变量V11"), ReadOnly(true)]
        public float V11 { get; set; }
        /// <summary>
        /// 用户变量V12
        /// </summary>
        [Browsable(true), Category("用户变量"), Description("用户变量V12"), ReadOnly(true)]
        public float V12 { get; set; }
        /// <summary>
        /// 用户变量V13
        /// </summary>
        [Browsable(true), Category("用户变量"), Description("用户变量V13"), ReadOnly(true)]
        public float V13 { get; set; }
        /// <summary>
        /// 用户变量V14
        /// </summary>
        [Browsable(true), Category("用户变量"), Description("用户变量V14"), ReadOnly(true)]
        public float V14 { get; set; }
        /// <summary>
        /// 用户变量V15
        /// </summary>
        [Browsable(true), Category("用户变量"), Description("用户变量V15"), ReadOnly(true)]
        public float V15 { get; set; }
        /// <summary>
        /// 用户变量V16
        /// </summary>
        [Browsable(true), Category("用户变量"), Description("用户变量V16"), ReadOnly(true)]
        public float V16 { get; set; }
        /// <summary>
        /// 用户变量V17
        /// </summary>
        [Browsable(true), Category("用户变量"), Description("用户变量V17"), ReadOnly(true)]
        public float V17 { get; set; }
        /// <summary>
        /// 用户变量V18
        /// </summary>
        [Browsable(true), Category("用户变量"), Description("用户变量V18"), ReadOnly(true)]
        public float V18 { get; set; }
        /// <summary>
        /// 用户变量V19
        /// </summary>
        [Browsable(true), Category("用户变量"), Description("用户变量V19"), ReadOnly(true)]
        public float V19 { get; set; }
        /// <summary>
        /// 数字输入/输出标志0
        /// </summary>
        [Browsable(true), Category("标志"), Description("数字输入/输出0"), ReadOnly(true)]
        public bool IO_0 { get; set; }
        /// <summary>
        /// 数字输入/输出标志1
        /// </summary>
        [Browsable(true), Category("标志"), Description("数字输入/输出1"), ReadOnly(true)]
        public bool IO_1 { get; set; }
        /// <summary>
        /// 数字输入/输出标志2
        /// </summary>
        [Browsable(true), Category("标志"), Description("数字输入/输出2"), ReadOnly(true)]
        public bool IO_2 { get; set; }
        /// <summary>
        /// 数字输入/输出标志3
        /// </summary>
        [Browsable(true), Category("标志"), Description("数字输入/输出3"), ReadOnly(true)]
        public bool IO_3 { get; set; }
        /// <summary>
        /// 数字输入/输出标志4
        /// </summary>
        [Browsable(true), Category("标志"), Description("数字输入/输出4"), ReadOnly(true)]
        public bool IO_4 { get; set; }
        /// <summary>
        /// 数字输入/输出标志5
        /// </summary>
        [Browsable(true), Category("标志"), Description("数字输入/输出5"), ReadOnly(true)]
        public bool IO_5 { get; set; }
        /// <summary>
        /// 数字输入/输出标志6
        /// </summary>
        [Browsable(true), Category("标志"), Description("数字输入/输出6"), ReadOnly(true)]
        public bool IO_6 { get; set; }
        /// <summary>
        /// 数字输入/输出标志7
        /// </summary>
        [Browsable(true), Category("标志"), Description("数字输入/输出7"), ReadOnly(true)]
        public bool IO_7 { get; set; }
        /// <summary>
        /// 运动队列已满标志
        /// S_QUEUE满（S_QUEUE为1），nmove命令被禁用并返回错误。
        /// </summary>
        [Browsable(true), Category("标志"), Description("运动队列已满标志\nS_QUEUE满（S_QUEUE为1），nmove命令被禁用并返回错误。"), ReadOnly(true)]
        public bool S_QUEUE { get; set; }
        /// <summary>
        /// 运动正在进行中标志
        /// 该标志在运动开始时切换为1（例如，一旦执行移动命令）。一旦RPOS达到TPOS，该标志在运动结束时切换为0。
        /// </summary>
        [Browsable(true), Category("标志"), Description("运动正在进行中标志\n该标志在运动开始时切换为1（例如，一旦执行移动命令）。一旦RPOS达到TPOS，该标志在运动结束时切换为0。"), ReadOnly(true)]
        public bool S_MOVE { get; set; }
        /// <summary>
        /// 伺服回路忙标志
        /// 当伺服回路处于活动状态时，该标志为1。在move/nmove命令之后，标志与S_MOVE同步切换到1，但通常在S_MOVE之后切换到0（零），一旦FPOS进入TPOS附近的±DZMIN间隔。
        /// </summary>
        [Browsable(true), Category("标志"), Description("伺服回路忙标志\n当伺服回路处于活动状态时，该标志为1。在move/nmove命令之后，标志与S_MOVE同步切换到1，但通常在S_MOVE之后切换到0（零），一旦FPOS进入TPOS附近的±DZMIN间隔。"), ReadOnly(true)]
        public bool S_BUSY { get; set; }
        /// <summary>
        /// 索引位置锁定标志
        /// 该标志指示是否遇到编码器索引并且POSI变量锁存了有效索引位置。有关详细信息，请参阅第7.2节。
        /// </summary>
        [Browsable(true), Category("标志"), Description("索引位置锁定标志\n该标志指示是否遇到编码器索引并且POSI变量锁存了有效索引位置。"), ReadOnly(true)]
        public bool S_IND { get; set; }
        /// <summary>
        /// 复位标志
        /// 控制器上电后标志为0。成功完成Home操作后，控制器将标志切换为1。该标志表示绝对反馈位置可用。
        /// 该标志也可以在XMS脚本中或通过主机命令分配(3)分配。
        /// </summary>
        [Browsable(true), Category("标志"), Description("复位标志\n控制器上电后标志为0。成功完成Home操作后，控制器将标志切换为1。该标志表示绝对反馈位置可用。\n该标志也可以在XMS脚本中或通过主机命令分配(3)分配。"), ReadOnly(true)]
        public bool S_HOME { get; set; }
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
        [Browsable(true), Category("标志"), Description("就位标志\n标志在以下情况下切换为1：\n‧  成功的运动终止\n‧  位置误差PE进入±DZMIN周围目标位置\n‧  中断时间已过（默认为6毫秒）\n标志在以下情况下切换为0：\n‧  运动开始时\n‧  位置误差PE被强制超出±DZMAX区间"), ReadOnly(true)]
        public bool S_INPOS { get; set; }
        #endregion 属性 end

        #region    事件成员
        /// <summary>
        /// 读取完成时
        /// </summary>
        public Action<XMSVariable> ReadCompleted;
        #endregion 事件成员

        #region    构造函数 start
        public XMSVariable(MotorControl motorControl)
        {
            this.edgeMotor = motorControl;

            Task.Factory.StartNew(() =>
            {
                while (this.edgeMotor.SerialPort.IsOpen)
                {
                    Initial();
                    Task.Delay(1);
                }
            });
        }
        #endregion 构造函数 end

        #region    私有方法 start
        /// <summary>
        /// 初始化所有变量
        /// </summary>
        /// <returns></returns>
        private void Initial()
        {
            try
            {
#if DEBUG
                sw.Restart();
                Debug.Print("---------------全部读取开始---------------");
                long total = 0;
#endif
                // 读取[所需的运动参数/即时参考运动变量/时间变量]
                Action ReadRequiredMotionParameters = () =>
                {
                    List<float> values = edgeMotor.Report<float>(XMSVariableID.POS, XMSVariableID.VEL, XMSVariableID.ACC, XMSVariableID.DEC, XMSVariableID.KDEC, XMSVariableID.FPOS, XMSVariableID.FVEL, XMSVariableID.PE, XMSVariableID.POSI, XMSVariableID.TIME);
                    // 所需的运动参数
                    POS = values[0];  // 40
                    VEL = values[1];  // 40
                    ACC = values[2];  // 500
                    DEC = values[3];  // 500
                    KDEC = values[4]; // 1000

                    // 即时反馈运动变量
                    FPOS = values[5];  // 0.0001
                    FVEL = values[6];  // 0
                    PE = values[7];    // 0.0000
                    POSI = values[8];  // 0.0000

                    // 时间变量
                    TIME = values[9];  // 0
                };

                // 读取[标志]
                Action ReadFlags = () =>
                {
                    List<bool> flags = edgeMotor.Report<bool>(XMSVariableID.IO_0, XMSVariableID.IO_1, XMSVariableID.IO_2, XMSVariableID.IO_3, XMSVariableID.IO_4, XMSVariableID.IO_5, XMSVariableID.IO_6, XMSVariableID.IO_7);
                    IO_0 = flags[0];  // 1
                    IO_1 = flags[1];  // 1
                    IO_2 = flags[2];  // 0
                    IO_3 = flags[3];  // 0
                    IO_4 = flags[4];  // 1
                    IO_5 = flags[5];  // 1
                    IO_6 = flags[6];  // 1
                    IO_7 = flags[7];  // 0

                    List<float> values = edgeMotor.Report<float>(XMSVariableID.TPOS, XMSVariableID.RPOS, XMSVariableID.RVEL, XMSVariableID.RACC, XMSVariableID.S_QUEUE, XMSVariableID.S_MOVE, XMSVariableID.S_BUSY, XMSVariableID.S_IND, XMSVariableID.S_HOME, XMSVariableID.S_INPOS);
                    // 即时参考运动变量
                    TPOS = values[0];  // 0.0001
                    RPOS = values[1];  // 0.0001
                    RVEL = values[2];  // 0
                    RACC = values[3];  // 0

                    S_QUEUE = Convert.ToBoolean(values[4]);  // 0
                    S_MOVE = Convert.ToBoolean(values[5]);   // 0
                    S_BUSY = Convert.ToBoolean(values[6]);   // 0
                    S_IND = Convert.ToBoolean(values[7]);    // 0
                    S_HOME = Convert.ToBoolean(values[8]);   // 0
                    S_INPOS = Convert.ToBoolean(values[9]);  // 0
                };

                // 读取[安全/伺服回路和驱动器配置/位置比较变量]
                Action ReadServoLoopAndDriveConfiguration = () =>
                {
                    List<float> values = edgeMotor.Report<float>(XMSVariableID.KP, XMSVariableID.KV, XMSVariableID.KI, XMSVariableID.LI, XMSVariableID.DOL, XMSVariableID.DOFFS, XMSVariableID.SLP, XMSVariableID.SLN, XMSVariableID.PEL, XMSVariableID.MTL);
                    KP = values[0];  // 100
                    KV = values[1];  // 1
                    KI = values[2];  // 350
                    LI = values[3];  // 50

                    // 安全：
                    DOL = values[4];   // 100
                    DOFFS = values[5]; // 0
                    SLP = values[6];   // 0
                    SLN = values[7];   // 0
                    PEL = values[8];   // 0
                    MTL = values[9];   // 2000

                    values = edgeMotor.Report<float>(XMSVariableID.BQA1, XMSVariableID.BQA2, XMSVariableID.BQB0, XMSVariableID.BQB1, XMSVariableID.BQB2, XMSVariableID.BQ2A1, XMSVariableID.BQ2A2, XMSVariableID.BQ2B0, XMSVariableID.BQ2B1, XMSVariableID.BQ2B2);
                    BQA1 = values[0];   // -1.694487
                    BQA2 = values[1];   // 0.7359618
                    BQB0 = values[2];   // 0.01036876
                    BQB1 = values[3];   // 0.02073752
                    BQB2 = values[4];   // 0.01036876
                    BQ2A1 = values[5];  // NaN
                    BQ2A2 = values[6];  // NaN
                    BQ2B0 = values[7];  // NaN
                    BQ2B1 = values[8];  // NaN
                    BQ2B2 = values[9];  // NaN

                    values = edgeMotor.Report<float>(XMSVariableID.ENR, XMSVariableID.MFREQ, XMSVariableID.SPRD, XMSVariableID.DZMIN, XMSVariableID.DZMAX, XMSVariableID.ZFF, XMSVariableID.FRP, XMSVariableID.FRN, XMSVariableID.DOUT, XMSVariableID.PPW);
                    ENR = values[0];   // 
                    MFREQ = values[1]; // 157000
                    SPRD = values[2];  // 
                    DZMIN = values[3]; // 
                    DZMAX = values[4]; // 
                    ZFF = values[5];   // 
                    FRP = values[6];   // 0
                    FRN = values[7];   // 0
                    DOUT = values[8];  // 0

                    // 位置比较变量
                    PPW = values[9];  // 0
                };

                // 读取[模拟输入/输出]
                Action ReadAnalogInputsAndOutputs = () =>
                {
                    List<float> values = edgeMotor.Report<float>(XMSVariableID.AIN0, XMSVariableID.AIN1, XMSVariableID.AIN2, XMSVariableID.AIN3, XMSVariableID.AIN4, XMSVariableID.AIN5, XMSVariableID.AIN6, XMSVariableID.AIN7, XMSVariableID.AIN8, XMSVariableID.AIN9);
                    AIN0 = values[0];  // 0
                    AIN1 = values[1];  // 0
                    AIN2 = values[2];  // 0
                    AIN3 = values[3];  // 0
                    AIN4 = values[4];  // 0
                    AIN5 = values[5];  // NaN
                    AIN6 = values[6];  // NaN
                    AIN7 = values[7];  // NaN
                    AIN8 = values[8];  // NaN
                    AIN9 = values[9];  // NaN

                    values = edgeMotor.Report<float>(XMSVariableID.AIN10, XMSVariableID.AIN11, XMSVariableID.AIN12, XMSVariableID.AIN13, XMSVariableID.AIN14, XMSVariableID.AIN15, XMSVariableID.AOUT0, XMSVariableID.AOUT1, XMSVariableID.AOUT2, XMSVariableID.AOUT3);
                    AIN10 = values[0];  // 0
                    AIN11 = values[1];  // 0
                    AIN12 = values[2];  // 0
                    AIN13 = values[3];  // 0
                    AIN14 = values[4];  // 0
                    AIN15 = values[5];  // NaN
                    AOUT0 = values[6];  // NaN
                    AOUT1 = values[7];  // NaN
                    AOUT2 = values[8];  // NaN
                    AOUT3 = values[9];  // NaN
                };

                // 读取[用户变量]
                Action ReadUserVariables = () =>
                {
                    List<float> values = edgeMotor.Report<float>(XMSVariableID.V0, XMSVariableID.V1, XMSVariableID.V2, XMSVariableID.V3, XMSVariableID.V4, XMSVariableID.V5, XMSVariableID.V6, XMSVariableID.V7, XMSVariableID.V8, XMSVariableID.V9);
                    V0 = values[0];  // 0
                    V1 = values[1];  // 0
                    V2 = values[2];  // 0
                    V3 = values[3];  // 0
                    V4 = values[4];  // 0
                    V5 = values[5];  // NaN
                    V6 = values[6];  // NaN
                    V7 = values[7];  // NaN
                    V8 = values[8];  // NaN
                    V9 = values[9];  // NaN

                    values = edgeMotor.Report<float>(XMSVariableID.V10, XMSVariableID.V11, XMSVariableID.V12, XMSVariableID.V13, XMSVariableID.V14, XMSVariableID.V15, XMSVariableID.V16, XMSVariableID.V17, XMSVariableID.V18, XMSVariableID.V19);
                    V10 = values[0];  // 0
                    V11 = values[1];  // 0
                    V12 = values[2];  // 0
                    V13 = values[3];  // 0
                    V14 = values[4];  // 0
                    V15 = values[5];  // NaN
                    V16 = values[6];  // NaN
                    V17 = values[7];  // NaN
                    V18 = values[8];  // NaN
                    V19 = values[9];  // NaN
                };

                Parallel.Invoke(ReadRequiredMotionParameters, ReadFlags, ReadServoLoopAndDriveConfiguration, ReadAnalogInputsAndOutputs, ReadUserVariables);
# if DEBUG
                sw.Stop();
                //Debug.Print($"读取[标志]耗时{sw.ElapsedMilliseconds}毫秒");
                total += sw.ElapsedMilliseconds;
                Debug.Print($"全部读取总计耗时{total}毫秒");
                Debug.Print("---------------全部读取结束---------------");
#endif

                ReadCompleted?.Invoke(this);
            }
            catch
            {
                throw;
            }
        }
        #endregion 私有方法 start
    }
}