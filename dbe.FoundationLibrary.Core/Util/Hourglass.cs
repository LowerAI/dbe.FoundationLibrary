using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace dbe.FoundationLibrary.Core.Util
{
    /// <summary>
    /// 沙漏
    /// </summary>
    public class Hourglass
    {
        private readonly Stopwatch stopwatch = new Stopwatch();
        private readonly List<TimeSpan> tsLst = new List<TimeSpan>();// 缓存耗时数值

        /// <summary>
        /// 耗时(时间段)
        /// </summary>
        public TimeSpan Elapsed => stopwatch.Elapsed;

        /// <summary>
        /// 耗时(ms)
        /// </summary>
        public long ElapsedMs => stopwatch.ElapsedMilliseconds;

        /// <summary>
        /// 耗时(时:分:秒.毫秒)
        /// </summary>
        public string ElapsedFormatted => Elapsed.ToString(@"hh\:mm\:ss\.ffff");

        /// <summary>
        /// 启动计时
        /// </summary>
        public void Start()
        {
            tsLst.Clear();
            stopwatch.Start();
        }

        /// <summary>
        /// 重新启动计时
        /// </summary>
        public void Restart()
        {
            stopwatch.Restart();
        }

        /// <summary>
        /// 停止计时
        /// </summary>
        public void Stop()
        {
            stopwatch.Stop();
            tsLst.Add(Elapsed);
        }

        /// <summary>
        /// 打印最后一次测量的耗时
        /// </summary>
        public void PrintLast(string actionName = "测量")
        {
            Trace.WriteLine($"本次{actionName}耗时{ElapsedFormatted}");
        }

        /// <summary>
        /// (异步执行)打印最后一次测量的耗时
        /// </summary>
        /// <param name="actionName"></param>
        /// <returns></returns>
        public async Task PrintLastAsync(string actionName = "测量")
        {
            await Task.Run(() => Trace.WriteLine($"本次{actionName}耗时{ElapsedFormatted}"));
        }

        /// <summary>
        /// 打印所有单次测量的耗时
        /// </summary>
        public void PrintAll()
        {
            var total = tsLst.Count;
            var tsTotal = TimeSpan.Zero;
            var summaryLst = new List<string>();
            for (int i = 0; i < total; i++)
            {
                summaryLst.Add($"item{i}耗时{tsLst[i].ToString(@"hh\:mm\:ss\.ffff")}");
                tsTotal += tsLst[i];
            }
            var summary = string.Join(",", summaryLst);
            Trace.WriteLine($"本次测量{total}项:{summary},总计耗时{tsTotal}");
        }
    }
}