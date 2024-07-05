using System.ComponentModel;

namespace dbe.FoundationLibrary.Core.Common
{
    /// <summary>
    /// 晶圆类型，会根据选择的类型自动画notch或者是定位边或者是方形掩模版
    /// </summary>
    public enum WaferSize : int
    {
        /// <summary>
        /// 27英寸实为26.6英寸675.64mm
        /// </summary>
        [Description("27英寸(675mm)")]
        _27_inch = 675,
        /// <summary>
        /// 18英寸实为17.7英寸449.58mm
        /// </summary>
        [Description("18英寸(450mm)")]
        _18_inch = 450,
        /// <summary>
        /// 12英寸实为11.8英寸299.72mm
        /// </summary>
        [Description("12英寸(300mm)")]
        _12_inch = 300,
        /// <summary>
        /// 8英寸实为7.9英寸200.66mm
        /// </summary>
        [Description("8英寸(200mm)")]
        _8_inch = 200,
        /// <summary>
        /// 掩模版152mm
        /// </summary>
        [Description("掩模版(152mm)")]
        Mask = 152,
        /// <summary>
        /// 6英寸实为5.9英寸149.86mm
        /// </summary>
        [Description("6英寸(150mm)")]
        _6_inch = 150,
        /// <summary>
        /// 5英寸实为4.9英寸124.46mm
        /// </summary>
        [Description("5英寸(125mm)")]
        _5_inch = 125,
        /// <summary>
        /// 4英寸实为101.6mm
        /// </summary>
        [Description("4英寸(100mm)")]
        _4_inch = 100,
        /// <summary>
        /// 3英寸实为76.2mm
        /// </summary>
        [Description("3英寸(76mm)")]
        _3_inch = 76,
        /// <summary>
        /// 2英寸实为50.8mm
        /// </summary>
        [Description("2英寸(51mm)")]
        _2_inch = 51,
        /// <summary>
        /// 1英寸实为25.4mm
        /// </summary>
        [Description("1英寸(25mm)")]
        _1_inch = 25
    }
}