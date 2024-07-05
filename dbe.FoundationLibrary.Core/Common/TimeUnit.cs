using System.ComponentModel;

namespace dbe.FoundationLibrary.Core.Common
{
    /// <summary>
    /// 时间单位
    /// </summary>
    public enum TimeUnit
    {
        [Description("毫秒")]
        Millisecond,
        [Description("秒")]
        Second,
        [Description("分")]
        Minute,
        [Description("时")]
        Hour
    }
}