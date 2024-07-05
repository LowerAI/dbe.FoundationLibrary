using System.Runtime.InteropServices;

namespace dbe.FoundationLibrary.Communication.CustComm.XCD_EDGE_Motor
{
    /// <summary>
    /// XCD EDGE电机系统伪变量(Name = ID)
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Size = 1)]
    public struct XMSPseudoVariableID
    {
        /// <summary>
        /// 状态
        /// </summary>
        public static ushort Status = 900;
        /// <summary>
        /// 程序状态
        /// </summary>
        public static ushort ProgramStatus = 901;
        /// <summary>
        /// 安全禁用
        /// </summary>
        public static ushort SafetyDisable = 902;
        /// <summary>
        /// 安全反转
        /// </summary>
        public static ushort SafetyInverse = 903;
        /// <summary>
        /// 安全状态
        /// </summary>
        public static ushort SafetyState = 904;
        /// <summary>
        /// IO方向
        /// </summary>
        public static ushort IODirection = 905;
        /// <summary>
        /// DZMIN停电
        /// </summary>
        public static ushort DZMINBlackout = 906;
        /// <summary>
        /// PWM限制
        /// </summary>
        public static ushort PWMLimit = 907;
        /// <summary>
        /// AIN保护
        /// </summary>
        public static ushort AINProtection = 908;
        /// <summary>
        /// SPI输入整数
        /// </summary>
        public static ushort SPIInputInteger0 = 921;
        public static ushort SPIInputInteger1 = 922;
        public static ushort SPIInputInteger2 = 923;
        public static ushort SPIInputInteger3 = 924;
        /// <summary>
        /// SPI输入实数
        /// </summary>
        public static ushort SPIInputReal0 = 925;
        public static ushort SPIInputReal1 = 926;
        /// <summary>
        /// XMS校验和
        /// </summary>
        public static ushort XMSChecksum = 950;
        /// <summary>
        /// XMS长度
        /// </summary>
        public static ushort XMSLength = 951;
        /// <summary>
        /// 最后一个错误
        /// </summary>
        public static ushort LastError = 960;
        /// <summary>
        /// UART地址
        /// </summary>
        public static ushort UARTAddress = 990;
        /// <summary>
        /// IIC地址
        /// </summary>
        public static ushort IICAddress = 991;
    }
}