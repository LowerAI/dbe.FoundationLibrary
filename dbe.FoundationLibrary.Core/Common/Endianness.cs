using System.ComponentModel;

namespace dbe.FoundationLibrary.Core.Common
{
    /// <summary>
    /// 字节顺序，包括大小端
    /// </summary>
    [Description("字节顺序，包括大小端")]
    public enum Endianness
    {
        /// <summary>
        /// 按照倒序排序，即大端-通过网络发送数据文件存储
        /// </summary>
        [Description("按照顺序排序")]
        ABCD = 0,
        /// <summary>
        /// 按照单字反转
        /// </summary>
        [Description("按照单字反转")]
        BADC = 1,
        /// <summary>
        /// 按照双字反转
        /// </summary>
        [Description("按照双字反转")]
        CDAB = 2,
        /// <summary>
        /// 按照顺序排序，即小端-x86架构采用小端字节序
        /// </summary>
        [Description("按照倒序排序")]
        DCBA = 3,
    }
}