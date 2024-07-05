using System;
using System.Runtime.InteropServices;

namespace dbe.FoundationLibrary.Core.DSP
{
    /// <summary>
    /// 采样
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Size = 1)]
    public struct Samples
    {
        /// <summary>
        /// FT计算的间隔时间，默认值36ms
        /// </summary>
        public static float FTInterval = 36f;

        private int sampleFrequency;
        /// <summary>
        /// 采样频率fs，传感器对被测目标的内部操作速率。默认500Hz
        /// </summary>
        public int SampleFrequency
        {
            get { return sampleFrequency; }
            set { sampleFrequency = value; }
        }

        private int sampleSize;
        /// <summary>
        /// 可变的采样时间长度(样本大小/取样长度)T，默认值256ms
        /// </summary>
        public int SampleSize
        {
            get { return sampleSize; }
            set { sampleSize = value; }
        }

        private int rawSampleSize;
        /// <summary>
        /// 原始采样时间长度(样本大小)T，默认值36ms
        /// </summary>
        public int RawSampleSize
        {
            get { return rawSampleSize; }
            set { rawSampleSize = value; }
        }

        /// <summary>
        /// 采样周期Ts，Ts = 1/fs，默认值2ms
        /// </summary>
        public float SamplingPeriod
        {
            get { return 1f / sampleFrequency * 1000; }
        }

        /// <summary>
        /// 可变的采样点数N，N = T/△t = T/Ts => 256/2 = 128个，默认值128个
        /// 采样点数必须大于1,且为整数
        /// </summary>
        public int SamplingPoints
        {
            get { return (int)Math.Ceiling(SampleSize / SamplingPeriod); }
        }

        /// <summary>
        /// 原始时域的窗口长度（采样点数）N，N = T/△t = T/Ts => 36/2 = 18个，默认值18个
        /// 采样点数是一次向pc发送的数据量包含的点数
        /// </summary>
        public int RawSamplingPoints
        {
            get { return (int)(RawSampleSize / SamplingPeriod); }
        }

        /// <summary>
        /// 数据更新率Rdu,Rdu = fs/N => 500/18 ≈ 27.78次/秒
        /// </summary>
        public float DataUpdateRate
        {
            get { return sampleFrequency / SamplingPoints; }
        }

        /// <summary>
        /// 采样率rs，默认值500SPS(个/秒)
        /// 每秒采样点的个数，即等于采样频率
        /// </summary>
        public float SamplingRate
        {
            get { return sampleFrequency; }
        }

        /// <summary>
        /// 采样时间间隔ts(采样间隔△t),ts = T / N => 36 / 18 = 2ms
        /// </summary>
        public float SamplingInterval
        {
            get { return SampleSize / SamplingPoints; }
        }

        /// <summary>
        /// 每次采集的间隔偏移时间(默认34ms)
        /// </summary>
        public int SamplingIntervalOffset
        {
            get
            {
                return RawSampleSize - (int)SamplingInterval;
            }
        }

        /// <summary>
        /// 频率分辨率△f,△f=1/NTs=1/T => 1 / 18 * 2 * 1000 ≈ 27.78Hz
        /// 在使用DFT时，在频率轴上的所能得到的最小频率间隔
        /// </summary>
        public float FrequencyResolution
        {
            get { return 1 / RawSampleSize * 1000; }
        }

        /// <summary>
        /// 默认采样频率为500Hz，默认样本大小为36ms
        /// </summary>
        /// <param name="samplingFrequenc">采样频率</param>
        /// <param name="sampleSize">样本大小</param>
        public Samples(int sampleSize, int samplingFrequenc = 500, int rawSampleSize = 36)
        {
            this.sampleFrequency = samplingFrequenc;
            this.rawSampleSize = rawSampleSize;
            this.sampleSize = sampleSize;
        }
    }
}