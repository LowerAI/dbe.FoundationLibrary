using System.Runtime.InteropServices;

namespace dbe.FoundationLibrary.Core.DSP
{
    /// <summary>
    /// 测量
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Size = 1)]
    public struct Measure
    {
        /// <summary>
        /// 基准频率
        /// </summary>
        public float BaseFrequency { get; set; }

        private float measuringFrequency;
        /// <summary>
        /// 测量频率，上位机自定义的数据速率。默认与频谱范围的最大频率同步为500Hz
        /// </summary>
        public float MeasuringFrequency
        {
            get { return measuringFrequency; }
            set { measuringFrequency = value; }
        }

        /// <summary>
        /// 每组采集的间隔时间(默认2ms)
        /// </summary>
        public float IntervalOfPerGroup
        {
            get
            {
                return 1000 / MeasuringFrequency;
            }
        }

        private int groupsNumberOfPerPacket;
        /// <summary>
        /// 每个数据包分多少组数据(默认18组)
        /// </summary>
        public int GroupsNumberOfPerPacket
        {
            get { return groupsNumberOfPerPacket; }
            set { groupsNumberOfPerPacket = value; }
        }

        /// <summary>
        /// 每次采集的间隔时间(默认36ms)
        /// </summary>
        public float IntervalOfPerAcquisition
        {
            get
            {
                return IntervalOfPerGroup * GroupsNumberOfPerPacket;
            }
        }

        /// <summary>
        /// 每次采集的间隔偏移时间(默认34ms)
        /// </summary>
        public float IntervalOffsetOfPerAcquisition
        {
            get
            {
                return IntervalOfPerAcquisition - IntervalOfPerGroup;
            }
        }

        public Measure(int measuringFrequency = 500, int groupsNumberOfPerAcquisition = 18)
        {
            this.BaseFrequency = measuringFrequency;
            this.measuringFrequency = measuringFrequency;
            this.groupsNumberOfPerPacket = groupsNumberOfPerAcquisition;
        }

        /// <summary>
        /// 设置倍频
        /// </summary>
        /// <param name="multiples">倍数</param>
        public void SetMultipleFrequency(float multiples)
        {
            this.measuringFrequency = (int)(this.BaseFrequency * multiples);
        }
    }
}