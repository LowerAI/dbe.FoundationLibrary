using System;
using System.ComponentModel;

namespace dbe.FoundationLibrary.Communication.CustComm.XCD_EDGE_Motor
{
    /// <summary>
    /// XCD EDGE电机系统内置变量(Name = ID)
    /// </summary>
    public class XMSVariable1
    {
        #region    字段 start
        private readonly MotorControl motorControl;
        //private Stopwatch sw = Stopwatch.StartNew();
        #endregion 字段 end

        #region    属性 start
        /// <summary>
        /// 速度
        /// </summary>
        [Browsable(true), Category("所需的运动参数"), Description("滑块移动的速度"), ReadOnly(false)]
        public float VEL
        {
            get { return motorControl.Report<float>(XMSVariableID.VEL); }
            set { motorControl.Assign(XMSVariableID.VEL, value); }
        }
        /// <summary>
        /// 加速度
        /// </summary>
        [Browsable(true), Category("所需的运动参数"), Description("滑块移动的加速度"), ReadOnly(false)]
        public float ACC
        {
            get { return motorControl.Report<float>(XMSVariableID.ACC); }
            set { motorControl.Assign(XMSVariableID.ACC, value); }
        }
        /// <summary>
        /// 减速度
        /// </summary>
        [Browsable(true), Category("所需的运动参数"), Description("滑块移动的减速度"), ReadOnly(false)]
        public float DEC
        {
            get { return motorControl.Report<float>(XMSVariableID.DEC); }
            set { motorControl.Assign(XMSVariableID.DEC, value); }
        }
        /// <summary>
        /// 终止减速 ‑ 用于故障情况；例如，如果限位开关被激活。
        /// </summary>
        [Browsable(true), Category("所需的运动参数"), Description("终止减速 ‑ 用于故障情况；例如，如果限位开关被激活。"), ReadOnly(false)]
        public float KDEC
        {
            get { return motorControl.Report<float>(XMSVariableID.KDEC); }
            set { motorControl.Assign(XMSVariableID.KDEC, value); }
        }
        /// <summary>
        /// 目标位置
        /// </summary>
        [Browsable(true), Category("即时参考运动变量"), Description("目标位置"), ReadOnly(false)]
        public float TPOS
        {
            get { return motorControl.Report<float>(XMSVariableID.TPOS); }
            set { motorControl.Assign(XMSVariableID.TPOS, value); }
        }
        /// <summary>
        /// 参考位置
        /// </summary>
        [Browsable(true), Category("即时参考运动变量"), Description("参考位置"), ReadOnly(true)]
        public float RPOS
        {
            get { return motorControl.Report<float>(XMSVariableID.RPOS); }
            set { motorControl.Assign(XMSVariableID.RPOS, value); }
        }
        /// <summary>
        /// 参考速度
        /// </summary>
        [Browsable(true), Category("即时参考运动变量"), Description("参考速度"), ReadOnly(true)]
        public float RVEL
        {
            get { return motorControl.Report<float>(XMSVariableID.RVEL); }
            set { motorControl.Assign(XMSVariableID.RVEL, value); }
        }
        /// <summary>
        /// 参考加速度
        /// </summary>
        [Browsable(true), Category("即时参考运动变量"), Description("参考加速度"), ReadOnly(true)]
        public float RACC
        {
            get { return motorControl.Report<float>(XMSVariableID.RACC); }
            set { motorControl.Assign(XMSVariableID.RACC, value); }
        }
        /// <summary>
        /// 反馈位置
        /// </summary>
        [Browsable(true), Category("即时反馈运动变量"), Description("反馈位置"), ReadOnly(false)]
        public float FPOS
        {
            get { return motorControl.Report<float>(XMSVariableID.FPOS); }
            set { motorControl.Set(XMSVariableID.FPOS, value); }
        }
        /// <summary>
        /// 反馈速度
        /// </summary>
        [Browsable(true), Category("即时反馈运动变量"), Description("反馈速度"), ReadOnly(false)]
        public float FVEL
        {
            get { return motorControl.Report<float>(XMSVariableID.FVEL); }
            set { motorControl.Assign(XMSVariableID.FVEL, value); }
        }
        /// <summary>
        /// 位置误差
        /// </summary>
        [Browsable(true), Category("即时反馈运动变量"), Description("位置误差"), ReadOnly(false)]
        public float PE
        {
            get { return motorControl.Report<float>(XMSVariableID.PE); }
            set { motorControl.Assign(XMSVariableID.PE, value); }
        }
        /// <summary>
        /// 位置环增益
        /// </summary>
        [Browsable(true), Category("伺服回路和驱动器配置"), Description("位置环增益"), ReadOnly(false)]
        public float KP
        {
            get { return motorControl.Report<float>(XMSVariableID.KP); }
            set { motorControl.Assign(XMSVariableID.KP, value); }
        }
        /// <summary>
        /// 速度环增益
        /// </summary>
        [Browsable(true), Category("伺服回路和驱动器配置"), Description("速度环增益"), ReadOnly(false)]
        public float KV
        {
            get { return motorControl.Report<float>(XMSVariableID.KV); }
            set { motorControl.Assign(XMSVariableID.KV, value); }
        }
        /// <summary>
        /// 积分增益
        /// </summary>
        [Browsable(true), Category("伺服回路和驱动器配置"), Description("积分增益"), ReadOnly(false)]
        public float KI
        {
            get { return motorControl.Report<float>(XMSVariableID.KI); }
            set { motorControl.Assign(XMSVariableID.KI, value); }
        }
        /// <summary>
        /// 速度环积分器限制
        /// </summary>
        [Browsable(true), Category("伺服回路和驱动器配置"), Description("速度环积分器限制"), ReadOnly(false)]
        public float LI
        {
            get { return motorControl.Report<float>(XMSVariableID.LI); }
            set { motorControl.Assign(XMSVariableID.LI, value); }
        }
        /// <summary>
        /// 第一个双二阶滤波器参数1
        /// </summary>
        [Browsable(true), Category("伺服回路和驱动器配置"), Description("第一个双二阶滤波器参数1"), ReadOnly(false)]
        public float BQA1
        {
            get { return motorControl.Report<float>(XMSVariableID.BQA1); }
            set { motorControl.Assign(XMSVariableID.BQA1, value); }
        }
        /// <summary>
        /// 第一个双二阶滤波器参数2
        /// </summary>
        [Browsable(true), Category("伺服回路和驱动器配置"), Description("第一个双二阶滤波器参数2"), ReadOnly(false)]
        public float BQA2
        {
            get { return motorControl.Report<float>(XMSVariableID.BQA2); }
            set { motorControl.Assign(XMSVariableID.BQA2, value); }
        }
        /// <summary>
        /// 第一个双二阶滤波器参数3
        /// </summary>
        [Browsable(true), Category("伺服回路和驱动器配置"), Description("第一个双二阶滤波器参数3"), ReadOnly(false)]
        public float BQB0
        {
            get { return motorControl.Report<float>(XMSVariableID.BQB0); }
            set { motorControl.Assign(XMSVariableID.BQB0, value); }
        }
        /// <summary>
        /// 第一个双二阶滤波器参数4
        /// </summary>
        [Browsable(true), Category("伺服回路和驱动器配置"), Description(" 第一个双二阶滤波器参数4"), ReadOnly(false)]
        public float BQB1
        {
            get { return motorControl.Report<float>(XMSVariableID.BQB1); }
            set { motorControl.Assign(XMSVariableID.BQB1, value); }
        }
        /// <summary>
        /// 第一个双二阶滤波器参数5
        /// </summary>
        [Browsable(true), Category("伺服回路和驱动器配置"), Description("第一个双二阶滤波器参数5"), ReadOnly(false)]
        public float BQB2
        {
            get { return motorControl.Report<float>(XMSVariableID.BQB2); }
            set { motorControl.Assign(XMSVariableID.BQB2, value); }
        }
        /// <summary>
        /// 编码器分辨率（毫米每一个编码器计数）
        /// </summary>
        [Browsable(true), Category("伺服回路和驱动器配置"), Description("编码器分辨率（毫米每一个编码器计数）"), ReadOnly(false)]
        public float ENR
        {
            get { return motorControl.Report<float>(XMSVariableID.ENR); }
            set { motorControl.Assign(XMSVariableID.ENR, value); }
        }
        /// <summary>
        /// 电机频率（PWM频率）
        /// </summary>
        [Browsable(true), Category("伺服回路和驱动器配置"), Description("电机频率（PWM频率）"), ReadOnly(false)]
        public float MFREQ
        {
            get { return motorControl.Report<float>(XMSVariableID.MFREQ); }
            set { motorControl.Assign(XMSVariableID.MFREQ, value); }
        }
        /// <summary>
        /// 伺服回路采样周期(毫秒)
        /// </summary>
        [Browsable(true), Category("伺服回路和驱动器配置"), Description("伺服回路采样周期(毫秒)"), ReadOnly(false)]
        public float SPRD
        {
            get { return motorControl.Report<float>(XMSVariableID.SPRD); }
            set { motorControl.Assign(XMSVariableID.SPRD, value); }
        }
        /// <summary>
        /// 模拟输入0(%)
        /// </summary>
        [Browsable(true), Category("模拟输入/输出"), Description("模拟输入0(%)"), ReadOnly(false)]
        public float AIN0
        {
            get { return motorControl.Report<float>(XMSVariableID.AIN0); }
            set { motorControl.Assign(XMSVariableID.AIN0, value); }
        }
        /// <summary>
        /// 模拟输入1(%)
        /// </summary>
        [Browsable(true), Category("模拟输入/输出"), Description("模拟输入1(%)"), ReadOnly(false)]
        public float AIN1
        {
            get { return motorControl.Report<float>(XMSVariableID.AIN1); }
            set { motorControl.Assign(XMSVariableID.AIN1, value); }
        }
        /// <summary>
        /// 模拟输入2(%)
        /// </summary>
        [Browsable(true), Category("模拟输入/输出"), Description("模拟输入2(%)"), ReadOnly(false)]
        public float AIN2
        {
            get { return motorControl.Report<float>(XMSVariableID.AIN2); }
            set { motorControl.Assign(XMSVariableID.AIN2, value); }
        }
        /// <summary>
        /// 模拟输入3(%)
        /// </summary>
        [Browsable(true), Category("模拟输入/输出"), Description("模拟输入3(%)"), ReadOnly(false)]
        public float AIN3
        {
            get { return motorControl.Report<float>(XMSVariableID.AIN3); }
            set { motorControl.Assign(XMSVariableID.AIN3, value); }
        }
        /// <summary>
        /// 模拟输出0(%)
        /// </summary>
        [Browsable(true), Category("模拟输入/输出"), Description("模拟输出0(%)"), ReadOnly(false)]
        public float AOUT0
        {
            get { return motorControl.Report<float>(XMSVariableID.AOUT0); }
            set { motorControl.Assign(XMSVariableID.AOUT0, value); }
        }
        /// <summary>
        /// 模拟输出1(%)
        /// </summary>
        [Browsable(true), Category("模拟输入/输出"), Description("模拟输出1(%)"), ReadOnly(false)]
        public float AOUT1
        {
            get { return motorControl.Report<float>(XMSVariableID.AOUT1); }
            set { motorControl.Assign(XMSVariableID.AOUT1, value); }
        }
        /// <summary>
        /// 模拟输出2(%)
        /// </summary>
        [Browsable(true), Category("模拟输入/输出"), Description("模拟输出2(%)"), ReadOnly(false)]
        public float AOUT2
        {
            get { return motorControl.Report<float>(XMSVariableID.AOUT2); }
            set { motorControl.Assign(XMSVariableID.AOUT2, value); }
        }
        /// <summary>
        /// 模拟输出3(%)
        /// </summary>
        [Browsable(true), Category("模拟输入/输出"), Description("模拟输出3(%)"), ReadOnly(false)]
        public float AOUT3
        {
            get { return motorControl.Report<float>(XMSVariableID.AOUT3); }
            set { motorControl.Assign(XMSVariableID.AOUT3, value); }
        }
        /// <summary>
        /// 以毫秒为单位的经过时间
        /// </summary>
        [Browsable(true), Category("时间变量"), Description("以毫秒为单位的经过时间"), ReadOnly(false)]
        public float TIME
        {
            get { return motorControl.Report<float>(XMSVariableID.TIME); }
            set { motorControl.Set(XMSVariableID.TIME, value); }
        }
        /// <summary>
        /// 驱动输出限制(最大输出的百分比)
        /// </summary>
        [Browsable(true), Category("安全"), Description("驱动输出限制(最大输出的百分比)"), ReadOnly(false)]
        public float DOL
        {
            get { return motorControl.Report<float>(XMSVariableID.DOL); }
            set { motorControl.Assign(XMSVariableID.DOL, value); }
        }
        /// <summary>
        /// 死区最小值（Nanomotion 专有算法)
        /// </summary>
        [Browsable(true), Category("伺服回路和驱动器配置"), Description("死区最小值（Nanomotion 专有算法)"), ReadOnly(false)]
        public float DZMIN
        {
            get { return motorControl.Report<float>(XMSVariableID.DZMIN); }
            set { motorControl.Assign(XMSVariableID.DZMIN, value); }
        }
        /// <summary>
        /// 死区最大值（Nanomotion 专有算法)
        /// </summary>
        [Browsable(true), Category("伺服回路和驱动器配置"), Description("死区最大值（Nanomotion 专有算法)"), ReadOnly(false)]
        public float DZMAX
        {
            get { return motorControl.Report<float>(XMSVariableID.DZMAX); }
            set { motorControl.Assign(XMSVariableID.DZMAX, value); }
        }
        /// <summary>
        /// 零前馈（Nanomotion 专有算法)
        /// </summary>
        [Browsable(true), Category("伺服回路和驱动器配置"), Description("零前馈（Nanomotion 专有算法)"), ReadOnly(false)]
        public float ZFF
        {
            get { return motorControl.Report<float>(XMSVariableID.ZFF); }
            set { motorControl.Assign(XMSVariableID.ZFF, value); }
        }
        /// <summary>
        /// 正方向摩擦
        /// </summary>
        [Browsable(true), Category("伺服回路和驱动器配置"), Description("正方向摩擦"), ReadOnly(false)]
        public float FRP
        {
            get { return motorControl.Report<float>(XMSVariableID.FRP); }
            set { motorControl.Assign(XMSVariableID.FRP, value); }
        }
        /// <summary>
        /// 负方向摩擦
        /// </summary>
        [Browsable(true), Category("伺服回路和驱动器配置"), Description("负方向摩擦"), ReadOnly(false)]
        public float FRN
        {
            get { return motorControl.Report<float>(XMSVariableID.FRN); }
            set { motorControl.Assign(XMSVariableID.FRN, value); }
        }
        /// <summary>
        /// 即时驱动输出(最大输出的百分比)
        /// </summary>
        [Browsable(true), Category("伺服回路和驱动器配置"), Description("即时驱动输出(最大输出的百分比)"), ReadOnly(false)]
        public float DOUT
        {
            get { return motorControl.Report<float>(XMSVariableID.DOUT); }
            set { motorControl.Assign(XMSVariableID.DOUT, value); }
        }
        /// <summary>
        /// 软件正限位
        /// </summary>
        [Browsable(true), Category("安全"), Description("软件正限位"), ReadOnly(false)]
        public float SLP
        {
            get { return motorControl.Report<float>(XMSVariableID.SLP); }
            set { motorControl.Assign(XMSVariableID.SLP, value); }
        }
        /// <summary>
        /// 软件负限位
        /// </summary>
        [Browsable(true), Category("安全"), Description("软件负限位"), ReadOnly(false)]
        public float SLN
        {
            get { return motorControl.Report<float>(XMSVariableID.SLN); }
            set { motorControl.Assign(XMSVariableID.SLN, value); }
        }
        /// <summary>
        /// 位置误差限制
        /// </summary>
        [Browsable(true), Category("安全"), Description("位置误差限制"), ReadOnly(false)]
        public float PEL
        {
            get { return motorControl.Report<float>(XMSVariableID.PEL); }
            set { motorControl.Assign(XMSVariableID.PEL, value); }
        }
        /// <summary>
        /// 运动时间限制
        /// </summary>
        [Browsable(true), Category("安全"), Description("运动时间限制"), ReadOnly(false)]
        public float MTL
        {
            get { return motorControl.Report<float>(XMSVariableID.MTL); }
            set { motorControl.Assign(XMSVariableID.MTL, value); }
        }
        /// <summary>
        /// 位置锁定在索引脉冲上
        /// </summary>
        [Browsable(true), Category("即时反馈运动变量"), Description("位置锁定在索引脉冲上"), ReadOnly(false)]
        public float POSI
        {
            get { return motorControl.Report<float>(XMSVariableID.POSI); }
            set { motorControl.Assign(XMSVariableID.POSI, value); }
        }
        /// <summary>
        /// 驱动器输出偏移(最大输出的百分比)
        /// </summary>
        [Browsable(true), Category("安全"), Description("驱动器输出偏移(最大输出的百分比)"), ReadOnly(false)]
        public float DOFFS
        {
            get { return motorControl.Report<float>(XMSVariableID.DOFFS); }
            set { motorControl.Assign(XMSVariableID.DOFFS, value); }
        }
        /// <summary>
        /// 以毫秒为单位的位置比较脉冲宽度
        /// </summary>
        [Browsable(true), Category("位置比较变量"), Description("以毫秒为单位的位置比较脉冲宽度"), ReadOnly(false)]
        public float PPW
        {
            get { return motorControl.Report<float>(XMSVariableID.PPW); }
            set { motorControl.Assign(XMSVariableID.PPW, value); }
        }
        /// <summary>
        /// 模拟输入4(%)
        /// </summary>
        [Browsable(true), Category("模拟输入/输出"), Description("模拟输入4(%)"), ReadOnly(false)]
        public float AIN4
        {
            get { return motorControl.Report<float>(XMSVariableID.AIN4); }
            set { motorControl.Assign(XMSVariableID.AIN4, value); }
        }
        /// <summary>
        /// 模拟输入5(%)
        /// </summary>
        [Browsable(true), Category("模拟输入/输出"), Description("模拟输入5(%)"), ReadOnly(false)]
        public float AIN5
        {
            get { return motorControl.Report<float>(XMSVariableID.AIN5); }
            set { motorControl.Assign(XMSVariableID.AIN5, value); }
        }
        /// <summary>
        /// 模拟输入6(%)
        /// </summary>
        [Browsable(true), Category("模拟输入/输出"), Description("模拟输入6(%)"), ReadOnly(false)]
        public float AIN6
        {
            get { return motorControl.Report<float>(XMSVariableID.AIN6); }
            set { motorControl.Assign(XMSVariableID.AIN6, value); }
        }
        /// <summary>
        /// 模拟输入7(%)
        /// </summary>
        [Browsable(true), Category("模拟输入/输出"), Description("模拟输入7(%)"), ReadOnly(false)]
        public float AIN7
        {
            get { return motorControl.Report<float>(XMSVariableID.AIN7); }
            set { motorControl.Assign(XMSVariableID.AIN7, value); }
        }
        /// <summary>
        /// 模拟输入8(%)
        /// </summary>
        [Browsable(true), Category("模拟输入/输出"), Description("模拟输入8(%)"), ReadOnly(false)]
        public float AIN8
        {
            get { return motorControl.Report<float>(XMSVariableID.AIN8); }
            set { motorControl.Assign(XMSVariableID.AIN8, value); }
        }
        /// <summary>
        /// 模拟输入9(%)
        /// </summary>
        [Browsable(true), Category("模拟输入/输出"), Description("模拟输入9(%)"), ReadOnly(false)]
        public float AIN9
        {
            get { return motorControl.Report<float>(XMSVariableID.AIN9); }
            set { motorControl.Assign(XMSVariableID.AIN9, value); }
        }
        /// <summary>
        /// 模拟输入10(%)
        /// </summary>
        [Browsable(true), Category("模拟输入/输出"), Description("模拟输入10(%)"), ReadOnly(false)]
        public float AIN10
        {
            get { return motorControl.Report<float>(XMSVariableID.AIN10); }
            set { motorControl.Assign(XMSVariableID.AIN10, value); }
        }
        /// <summary>
        /// 模拟输入11(%)
        /// </summary>
        [Browsable(true), Category("模拟输入/输出"), Description("模拟输入11(%)"), ReadOnly(false)]
        public float AIN11
        {
            get { return motorControl.Report<float>(XMSVariableID.AIN11); }
            set { motorControl.Assign(XMSVariableID.AIN11, value); }
        }
        /// <summary>
        /// 模拟输入12(%)
        /// </summary>
        [Browsable(true), Category("模拟输入/输出"), Description("模拟输入12(%)"), ReadOnly(false)]
        public float AIN12
        {
            get { return motorControl.Report<float>(XMSVariableID.AIN12); }
            set { motorControl.Assign(XMSVariableID.AIN12, value); }
        }
        /// <summary>
        /// 模拟输入13(%)
        /// </summary>
        [Browsable(true), Category("模拟输入/输出"), Description("模拟输入13(%)"), ReadOnly(false)]
        public float AIN13
        {
            get { return motorControl.Report<float>(XMSVariableID.AIN13); }
            set { motorControl.Assign(XMSVariableID.AIN13, value); }
        }
        /// <summary>
        /// 模拟输入14(%)
        /// </summary>
        [Browsable(true), Category("模拟输入/输出"), Description("模拟输入14(%)"), ReadOnly(false)]
        public float AIN14
        {
            get { return motorControl.Report<float>(XMSVariableID.AIN14); }
            set { motorControl.Assign(XMSVariableID.AIN14, value); }
        }
        /// <summary>
        /// 模拟输入15(%)
        /// </summary>
        [Browsable(true), Category("模拟输入/输出"), Description("模拟输入15(%)"), ReadOnly(false)]
        public float AIN15
        {
            get { return motorControl.Report<float>(XMSVariableID.AIN15); }
            set { motorControl.Assign(XMSVariableID.AIN15, value); }
        }
        /// <summary>
        /// 第二个双二阶滤波器参数1
        /// </summary>
        [Browsable(true), Category("伺服回路和驱动器配置"), Description("第二个双二阶滤波器参数1"), ReadOnly(false)]
        public float BQ2A1
        {
            get { return motorControl.Report<float>(XMSVariableID.BQ2A1); }
            set { motorControl.Assign(XMSVariableID.BQ2A1, value); }
        }
        /// <summary>
        /// 第二个双二阶滤波器参数2
        /// </summary>
        [Browsable(true), Category("伺服回路和驱动器配置"), Description("第二个双二阶滤波器参数2"), ReadOnly(false)]
        public float BQ2A2
        {
            get { return motorControl.Report<float>(XMSVariableID.BQ2A2); }
            set { motorControl.Assign(XMSVariableID.BQ2A2, value); }
        }
        /// <summary>
        /// 第二个双二阶滤波器参数3
        /// </summary>
        [Browsable(true), Category("伺服回路和驱动器配置"), Description("第二个双二阶滤波器参数3"), ReadOnly(false)]
        public float BQ2B0
        {
            get { return motorControl.Report<float>(XMSVariableID.BQ2B0); }
            set { motorControl.Assign(XMSVariableID.BQ2B0, value); }
        }
        /// <summary>
        /// 第二个双二阶滤波器参数4
        /// </summary>
        [Browsable(true), Category("伺服回路和驱动器配置"), Description("第二个双二阶滤波器参数4"), ReadOnly(false)]
        public float BQ2B1
        {
            get { return motorControl.Report<float>(XMSVariableID.BQ2B1); }
            set { motorControl.Assign(XMSVariableID.BQ2B1, value); }
        }
        /// <summary>
        /// 第二个双二阶滤波器参数5
        /// </summary>
        [Browsable(true), Category("伺服回路和驱动器配置"), Description("第二个双二阶滤波器参数5"), ReadOnly(false)]
        public float BQ2B2
        {
            get { return motorControl.Report<float>(XMSVariableID.BQ2B2); }
            set { motorControl.Assign(XMSVariableID.BQ2B2, value); }
        }
        /// <summary>
        /// 用户变量V0
        /// </summary>
        [Browsable(true), Category("用户变量"), Description("用户变量V0"), ReadOnly(false)]
        public float V0
        {
            get { return motorControl.Report<float>(XMSVariableID.V0); }
            set { motorControl.Assign(XMSVariableID.V0, value); }
        }
        /// <summary>
        /// 用户变量V1
        /// </summary>
        [Browsable(true), Category("用户变量"), Description("用户变量V1"), ReadOnly(false)]
        public float V1
        {
            get { return motorControl.Report<float>(XMSVariableID.V1); }
            set { motorControl.Assign(XMSVariableID.V1, value); }
        }
        /// <summary>
        /// 用户变量V2
        /// </summary>
        [Browsable(true), Category("用户变量"), Description("用户变量V2"), ReadOnly(false)]
        public float V2
        {
            get { return motorControl.Report<float>(XMSVariableID.V2); }
            set { motorControl.Assign(XMSVariableID.V2, value); }
        }
        /// <summary>
        /// 用户变量V3
        /// </summary>
        [Browsable(true), Category("用户变量"), Description("用户变量V3"), ReadOnly(false)]
        public float V3
        {
            get { return motorControl.Report<float>(XMSVariableID.V3); }
            set { motorControl.Assign(XMSVariableID.V3, value); }
        }
        /// <summary>
        /// 用户变量V4
        /// </summary>
        [Browsable(true), Category("用户变量"), Description("用户变量V4"), ReadOnly(false)]
        public float V4
        {
            get { return motorControl.Report<float>(XMSVariableID.V4); }
            set { motorControl.Assign(XMSVariableID.V4, value); }
        }
        /// <summary>
        /// 用户变量V5
        /// </summary>
        [Browsable(true), Category("用户变量"), Description("用户变量V5"), ReadOnly(false)]
        public float V5
        {
            get { return motorControl.Report<float>(XMSVariableID.V5); }
            set { motorControl.Assign(XMSVariableID.V5, value); }
        }
        /// <summary>
        /// 用户变量V6
        /// </summary>
        [Browsable(true), Category("用户变量"), Description("用户变量V6"), ReadOnly(false)]
        public float V6
        {
            get { return motorControl.Report<float>(XMSVariableID.V6); }
            set { motorControl.Assign(XMSVariableID.V6, value); }
        }
        /// <summary>
        /// 用户变量V7
        /// </summary>
        [Browsable(true), Category("用户变量"), Description("用户变量V7"), ReadOnly(false)]
        public float V7
        {
            get { return motorControl.Report<float>(XMSVariableID.V7); }
            set { motorControl.Assign(XMSVariableID.V7, value); }
        }
        /// <summary>
        /// 用户变量V8
        /// </summary>
        [Browsable(true), Category("用户变量"), Description("用户变量V8"), ReadOnly(false)]
        public float V8
        {
            get { return motorControl.Report<float>(XMSVariableID.V8); }
            set { motorControl.Assign(XMSVariableID.V8, value); }
        }
        /// <summary>
        /// 用户变量V9
        /// </summary>
        [Browsable(true), Category("用户变量"), Description("用户变量V9"), ReadOnly(false)]
        public float V9
        {
            get { return motorControl.Report<float>(XMSVariableID.V9); }
            set { motorControl.Assign(XMSVariableID.V9, value); }
        }
        /// <summary>
        /// 用户变量V10
        /// </summary>
        [Browsable(true), Category("用户变量"), Description("用户变量V10"), ReadOnly(false)]
        public float V10
        {
            get { return motorControl.Report<float>(XMSVariableID.V10); }
            set { motorControl.Assign(XMSVariableID.V10, value); }
        }
        /// <summary>
        /// 用户变量V11
        /// </summary>
        [Browsable(true), Category("用户变量"), Description("用户变量V11"), ReadOnly(false)]
        public float V11
        {
            get { return motorControl.Report<float>(XMSVariableID.V11); }
            set { motorControl.Assign(XMSVariableID.V11, value); }
        }
        /// <summary>
        /// 用户变量V12
        /// </summary>
        [Browsable(true), Category("用户变量"), Description("用户变量V12"), ReadOnly(false)]
        public float V12
        {
            get { return motorControl.Report<float>(XMSVariableID.V12); }
            set { motorControl.Assign(XMSVariableID.V12, value); }
        }
        /// <summary>
        /// 用户变量V13
        /// </summary>
        [Browsable(true), Category("用户变量"), Description("用户变量V13"), ReadOnly(false)]
        public float V13
        {
            get { return motorControl.Report<float>(XMSVariableID.V13); }
            set { motorControl.Assign(XMSVariableID.V13, value); }
        }
        /// <summary>
        /// 用户变量V14
        /// </summary>
        [Browsable(true), Category("用户变量"), Description("用户变量V14"), ReadOnly(false)]
        public float V14
        {
            get { return motorControl.Report<float>(XMSVariableID.V14); }
            set { motorControl.Assign(XMSVariableID.V14, value); }
        }
        /// <summary>
        /// 用户变量V15
        /// </summary>
        [Browsable(true), Category("用户变量"), Description("用户变量V15"), ReadOnly(false)]
        public float V15
        {
            get { return motorControl.Report<float>(XMSVariableID.V15); }
            set { motorControl.Assign(XMSVariableID.V15, value); }
        }
        /// <summary>
        /// 用户变量V16
        /// </summary>
        [Browsable(true), Category("用户变量"), Description("用户变量V16"), ReadOnly(false)]
        public float V16
        {
            get { return motorControl.Report<float>(XMSVariableID.V16); }
            set { motorControl.Assign(XMSVariableID.V16, value); }
        }
        /// <summary>
        /// 用户变量V17
        /// </summary>
        [Browsable(true), Category("用户变量"), Description("用户变量V17"), ReadOnly(false)]
        public float V17
        {
            get { return motorControl.Report<float>(XMSVariableID.V17); }
            set { motorControl.Assign(XMSVariableID.V17, value); }
        }
        /// <summary>
        /// 用户变量V18
        /// </summary>
        [Browsable(true), Category("用户变量"), Description("用户变量V18"), ReadOnly(false)]
        public float V18
        {
            get { return motorControl.Report<float>(XMSVariableID.V18); }
            set { motorControl.Assign(XMSVariableID.V18, value); }
        }
        /// <summary>
        /// 用户变量V19
        /// </summary>
        [Browsable(true), Category("用户变量"), Description("用户变量V19"), ReadOnly(false)]
        public float V19
        {
            get { return motorControl.Report<float>(XMSVariableID.V19); }
            set { motorControl.Assign(XMSVariableID.V19, value); }
        }
        /// <summary>
        /// 数字输入/输出标志0
        /// </summary>
        [Browsable(true), Category("标志"), Description("数字输入/输出0"), ReadOnly(true)]
        public bool IO_0
        {
            get { return motorControl.Report<bool>(XMSVariableID.IO_0); }
            set { motorControl.AssignBoolean(XMSVariableID.IO_0, value); }
        }
        /// <summary>
        /// 数字输入/输出标志1
        /// </summary>
        [Browsable(true), Category("标志"), Description("数字输入/输出1"), ReadOnly(true)]
        public bool IO_1
        {
            get { return motorControl.Report<bool>(XMSVariableID.IO_1); }
            set { motorControl.AssignBoolean(XMSVariableID.IO_1, value); }
        }
        /// <summary>
        /// 数字输入/输出标志2
        /// </summary>
        [Browsable(true), Category("标志"), Description("数字输入/输出2"), ReadOnly(true)]
        public bool IO_2
        {
            get { return motorControl.Report<bool>(XMSVariableID.IO_2); }
            set { motorControl.AssignBoolean(XMSVariableID.IO_2, value); }
        }
        /// <summary>
        /// 数字输入/输出标志3
        /// </summary>
        [Browsable(true), Category("标志"), Description("数字输入/输出3"), ReadOnly(true)]
        public bool IO_3
        {
            get { return motorControl.Report<bool>(XMSVariableID.IO_3); }
            set { motorControl.AssignBoolean(XMSVariableID.IO_3, value); }
        }
        /// <summary>
        /// 数字输入/输出标志4
        /// </summary>
        [Browsable(true), Category("标志"), Description("数字输入/输出4"), ReadOnly(true)]
        public bool IO_4
        {
            get { return motorControl.Report<bool>(XMSVariableID.IO_4); }
            set { motorControl.AssignBoolean(XMSVariableID.IO_4, value); }
        }
        /// <summary>
        /// 数字输入/输出标志5
        /// </summary>
        [Browsable(true), Category("标志"), Description("数字输入/输出5"), ReadOnly(true)]
        public bool IO_5
        {
            get { return motorControl.Report<bool>(XMSVariableID.IO_5); }
            set { motorControl.AssignBoolean(XMSVariableID.IO_5, value); }
        }
        /// <summary>
        /// 数字输入/输出标志6
        /// </summary>
        [Browsable(true), Category("标志"), Description("数字输入/输出6"), ReadOnly(true)]
        public bool IO_6
        {
            get { return motorControl.Report<bool>(XMSVariableID.IO_6); }
            set { motorControl.AssignBoolean(XMSVariableID.IO_6, value); }
        }
        /// <summary>
        /// 数字输入/输出标志7
        /// </summary>
        [Browsable(true), Category("标志"), Description("数字输入/输出7"), ReadOnly(true)]
        public bool IO_7
        {
            get { return motorControl.Report<bool>(XMSVariableID.IO_7); }
            set { motorControl.AssignBoolean(XMSVariableID.IO_7, value); }
        }
        /// <summary>
        /// 运动队列已满标志
        /// S_QUEUE满（S_QUEUE为1），nmove命令被禁用并返回错误。
        /// </summary>
        [Browsable(true), Category("标志"), Description("运动队列已满标志\nS_QUEUE满（S_QUEUE为1），nmove命令被禁用并返回错误。"), ReadOnly(true)]
        public bool S_QUEUE
        {
            get { return motorControl.Report<bool>(XMSVariableID.S_QUEUE); }
            set { motorControl.AssignBoolean(XMSVariableID.S_QUEUE, value); }
        }
        /// <summary>
        /// 运动正在进行中标志
        /// 该标志在运动开始时切换为1（例如，一旦执行移动命令）。一旦RPOS达到TPOS，该标志在运动结束时切换为0。
        /// </summary>
        [Browsable(true), Category("标志"), Description("运动正在进行中标志\n该标志在运动开始时切换为1（例如，一旦执行移动命令）。一旦RPOS达到TPOS，该标志在运动结束时切换为0。"), ReadOnly(true)]
        public bool S_MOVE
        {
            get { return motorControl.Report<bool>(XMSVariableID.S_MOVE); }
            set { motorControl.AssignBoolean(XMSVariableID.S_MOVE, value); }
        }
        /// <summary>
        /// 伺服回路忙标志
        /// 当伺服回路处于活动状态时，该标志为1。在move/nmove命令之后，标志与S_MOVE同步切换到1，但通常在S_MOVE之后切换到0（零），一旦FPOS进入TPOS附近的±DZMIN间隔。
        /// </summary>
        [Browsable(true), Category("标志"), Description("伺服回路忙标志\n当伺服回路处于活动状态时，该标志为1。在move/nmove命令之后，标志与S_MOVE同步切换到1，但通常在S_MOVE之后切换到0（零），一旦FPOS进入TPOS附近的±DZMIN间隔。"), ReadOnly(true)]
        public bool S_BUSY
        {
            get { return motorControl.Report<bool>(XMSVariableID.S_BUSY); }
            set { motorControl.AssignBoolean(XMSVariableID.S_BUSY, value); }
        }
        /// <summary>
        /// 索引位置锁定标志
        /// 该标志指示是否遇到编码器索引并且POSI变量锁存了有效索引位置。有关详细信息，请参阅第7.2节。
        /// </summary>
        [Browsable(true), Category("标志"), Description("索引位置锁定标志\n该标志指示是否遇到编码器索引并且POSI变量锁存了有效索引位置。"), ReadOnly(true)]
        public bool S_IND
        {
            get { return motorControl.Report<bool>(XMSVariableID.S_IND); }
            set { motorControl.SetBoolean(XMSVariableID.S_IND, value); }
        }
        /// <summary>
        /// 复位标志
        /// 控制器上电后标志为0。成功完成Home操作后，控制器将标志切换为1。该标志表示绝对反馈位置可用。
        /// 该标志也可以在XMS脚本中或通过主机命令分配(3)分配。
        /// </summary>
        [Browsable(true), Category("标志"), Description("复位标志\n控制器上电后标志为0。成功完成Home操作后，控制器将标志切换为1。该标志表示绝对反馈位置可用。\n该标志也可以在XMS脚本中或通过主机命令分配(3)分配。"), ReadOnly(true)]
        public bool S_HOME
        {
            get { return motorControl.Report<bool>(XMSVariableID.S_HOME); }
            set { motorControl.AssignBoolean(XMSVariableID.S_HOME, value); }
        }
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
        public bool S_INPOS
        {
            get { return motorControl.Report<bool>(XMSVariableID.S_INPOS); }
            set { motorControl.AssignBoolean(XMSVariableID.S_INPOS, value); }
        }
        #endregion 属性 end

        #region    事件成员
        /// <summary>
        /// 读取完成时
        /// </summary>
        public Action<XMSVariable> ReadCompleted;
        #endregion 事件成员

        #region    构造函数 start
        public XMSVariable1(MotorControl motorControl)
        {
            this.motorControl = motorControl;
        }
        #endregion 构造函数 end
    }
}