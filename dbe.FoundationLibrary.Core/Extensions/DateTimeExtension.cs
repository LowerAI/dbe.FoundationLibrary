using System;

namespace dbe.FoundationLibrary.Core.Extensions
{
    /// <summary>
    /// DateTime扩展类
    /// </summary>
    public static class DateTimeExtension
    {
        /// <summary>
        /// 计算指定时间点距离当前时间点已过去的秒数(包含小数形式的毫秒数)
        /// </summary>
        /// <param name="dt">指定的时间点</param>
        /// <returns></returns>
        public static double GetElapsedSeconds(this DateTime dt)
        {
            return (dt - DateTime.Now).TotalSeconds;
        }

        /// <summary>
        /// 计算指定时间点距离指定时间点已过去的秒数(包含小数形式的毫秒数)
        /// </summary>
        /// <param name="dt">指定的时间点</param>
        /// <returns></returns>
        public static double GetElapsedSeconds(this DateTime dt, DateTime tp)
        {
            return (dt - tp).TotalSeconds;
        }

        /// <summary>
        /// 获取指定日期所属的季度
        /// </summary>
        /// <returns>季度的数值形式</returns>
        public static int GetQuarter(this DateTime now)
        {
            int Quarter = (now.Month - 1) / 3 + 1;
            return Quarter;
        }
    }
}