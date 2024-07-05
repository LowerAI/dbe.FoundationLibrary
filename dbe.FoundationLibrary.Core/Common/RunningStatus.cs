using System.ComponentModel;

namespace dbe.FoundationLibrary.Core.Common
{
    /// <summary>
    /// 运行状态
    /// </summary>
    public enum RunningStatus : int
    {
        [Description("运行中")]
        Running,
        [Description("暂停")]
        Pause,
        [Description("停止")]
        Stop
    }
}