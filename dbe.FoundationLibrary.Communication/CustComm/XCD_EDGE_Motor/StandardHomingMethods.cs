namespace dbe.FoundationLibrary.Communication.CustComm.XCD_EDGE_Motor
{
    /// <summary>
    /// 标准复位方法
    /// </summary>
    public enum StandardHomingMethods : sbyte
    {
        /// <summary>
        /// 负硬停
        /// </summary>
        NegativeHardStop = 50,
        /// <summary>
        /// 正硬停
        /// </summary>
        PositiveHardStop = 51,
        /// <summary>
        /// 负硬停和索引脉冲上的原点
        /// </summary>
        NegativeHardStopAndIndexPulse = 60,
        /// <summary>
        /// 正硬停和索引脉冲上的原点
        /// </summary>
        PositiveHardStopAndIndexPulse = 61,
    }
}