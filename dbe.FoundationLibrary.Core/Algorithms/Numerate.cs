using System;

namespace dbe.FoundationLibrary.Core.Algorithms
{
    /// <summary>
    /// 针对Math功能扩展的数学计算类
    /// </summary>
    public class Numerate
    {
        /// <summary>
        /// 计算均方根
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static double CalcRMS(double[] data)
        {
            double sum = 0;
            for (int i = 0; i < data.Length; i++)
            {
                sum += data[i] * data[i];
            }
            double mean = sum / data.Length;
            double rms = Math.Sqrt(mean);
            return rms;
        }
    }
}