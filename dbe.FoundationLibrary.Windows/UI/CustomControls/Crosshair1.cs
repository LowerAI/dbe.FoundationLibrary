using dbe.FoundationLibrary.Core.Common;
using dbe.FoundationLibrary.Core.Extensions;
using dbe.FoundationLibrary.Core.Util;
using dbe.FoundationLibrary.Windows.Extensions;

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace dbe.FoundationLibrary.Windows.UI.CustomControls
{
    /// <summary>
    /// 十字准星
    /// </summary>
    [DefaultProperty("IsShowWaferCicle")]
    [Docking(DockingBehavior.Ask)]
    [ComVisible(true)]
    public class Crosshair1 : Control
    {
        #region    字段 start
        // 控件的外间距
        private Padding margin = new Padding(0);
        // 控件的内间距
        private Padding padding = new Padding(5);

        // 绘制聚焦环所在图层文本的字体
        private Font focusringFont = new Font("Consolas", 18F, FontStyle.Regular | FontStyle.Bold | FontStyle.Italic);
        // 绘制晶圆所在图层文本的字体
        private Font waferFont = new Font("Cascadia Code", 18F, FontStyle.Regular | FontStyle.Bold | FontStyle.Italic);

        /// <summary>
        /// 主面板容器框
        /// </summary>
        private Rectangle masterRect;
        private RectangleF masterRectF;
        /// <summary>
        /// 从面板容器框
        /// </summary>
        private RectangleF slaveRectF;

        // 左右面板的间距
        private int panelGap = 16;
        // 上一个气泡关联的位图和矩形容器
        private Tuple<Bitmap, RectangleF> lastItem;

        // 是否显示聚焦环
        private bool isShowFocusring = false;
        // 是否显示聚焦环的十字线
        private bool isShowFocusringReticle = true;
        // 是否显示聚焦环的弧形豁口
        private bool isShowFocusringNotch = false;
        // 是否显示聚焦环的坐标轴名称
        private bool isShowFocusringAxisName = false;
        // 是否显示聚焦环的原点坐标
        private bool isShowFocusringOrigin = false;
        // 是否显示晶圆
        private bool isShowWafer = true;
        // 是否显示晶圆的十字线
        private bool isShowWaferReticle = false;
        // 是否显示晶圆的弧形豁口
        private bool isShowWaferNotch = true;
        // 是否显示晶圆的坐标轴名称
        private bool isShowWaferAxisName = false;
        // 是否显示晶圆的原点坐标
        private bool isShowWaferOrigin = false;
        // 是否显示容许范围(Go/No Go)的圆
        private bool isShowToleranceRange = false;
        // 是否显示气泡
        private bool isShowBubble = false;
        // 是否显示放大镜
        private bool isShowMagnifier = false;

        // 晶圆矩形框
        private RectangleF waferRectF = RectangleF.Empty;
        // 放大镜选取框
        private RectangleF magnifierRectF = RectangleF.Empty;
        // 气泡矩形框
        private RectangleF bubbleRectF = RectangleF.Empty;

        // 每次移动的偏移量
        //private float MoveOffset = 1.0F;

        // 主窗格的设备半径
        private float masterDeviceRadius = 30F;
        // 聚焦环的设备半径
        private float focusRingDeviceRadius = 29.5F;
        // 聚焦环的内外圆间距
        private float focusRingGap = 30F;
        // 晶圆的设备半径
        private float waferDeviceRadius = 25F;
        // 晶圆十字线旋转的角度
        private float waferReticleAngle = 0F;
        // 放大镜选取框的设备半径
        private float magnifierDeviceRadius = 12F;
        // 容许范围的设备半径
        private float toleranceDeviceRadius = 5F;
        // 气泡的设备半径
        private float bubbleDeviceRadius = 0.75F;

        // 聚焦环十字线X轴起点
        private PointF focusRingReticleXStart = PointF.Empty;
        // 聚焦环十字线X轴终点
        private PointF focusRingReticleXEnd = PointF.Empty;
        // 聚焦环十字线Y轴起点
        private PointF focusRingReticleYStart = PointF.Empty;
        // 聚焦环十字线Y轴终点
        private PointF focusRingReticleYEnd = PointF.Empty;

        // 晶圆十字线X轴起点
        private PointF waferReticleXStart = PointF.Empty;
        // 晶圆十字线X轴终点
        private PointF waferReticleXEnd = PointF.Empty;
        // 晶圆十字线Y轴起点
        private PointF waferReticleYStart = PointF.Empty;
        // 晶圆十字线Y轴终点
        private PointF waferReticleYEnd = PointF.Empty;

        // 聚焦环圆心
        private PointF focusRingOrigin = PointF.Empty;
        // 晶圆圆心
        private PointF waferOrigin = PointF.Empty;
        // 气泡圆心
        private PointF bubbleOrigin = PointF.Empty;

        // 绘制聚焦环所在图层中文本的颜色
        private Color focusRingTextColor = Color.Black;
        // 聚焦环的填充色
        private Color focusRingFillColor = Color.Silver;
        // 绘制晶圆所在图层中文本的颜色
        private Color waferTextColor = Color.Black;
        // 晶圆的边框色
        private Color waferBorderColor = Color.DarkGray;
        // 晶圆豁口的边框色
        private Color waferNotchBorderColor = Color.DarkGray;
        // 晶圆十字线的颜色
        private Color waferReticleColor = Color.Purple;
        // 容许范围的圆周色
        private Color toleranceCicleBorderColor = Color.Blue;
        // 窗格的边框样式
        private BorderStyle paneBorder = BorderStyle.None;
        // 气泡的实时颜色
        private Color bubbleColor = Color.Empty;
        // 容许范围内的气泡颜色
        private Color bubbleColorInTolerance = Color.Green;
        // 超出容许范围的气泡颜色
        private Color bubbleColorOutOfTolerance = Color.Red;
        // 超出范围的气泡颜色
        private Color bubbleColorOutOfRange = Color.Yellow;

        private Brush brush4Background;// 背景画刷
        private SolidBrush fillBrush4FocusRingFill;// 实线画刷 for 聚焦环的填充
        private SolidBrush solidBrush4FocusRingText;// 实线画刷 for 聚焦环的文本
        private SolidBrush solidBrush4WaferBorder;// 实线画刷 for 晶圆的边框
        private SolidBrush solidBrush4WaferNotch;// 实线画刷 for 晶圆的豁口
        private SolidBrush solidBrush4WaferText;// 实线画刷 for 晶圆的文本
        //private SolidBrush fillBrush4Bubble;// 实线画刷 for 气泡的填充
        private Pen dashPen4Reticle;// 虚线画笔 for 十字线
        private Pen dashPen4Magnifier;// 虚线画笔 for 放大框
        private Pen solidPen4TRC;// 实线画笔 for ToleranceRangeCicle
        private Pen dashPen4WaferReticle;// 虚线画笔 for 晶圆的十字线
        private Pen solidPen4WaferBorder;// 实线画笔 for 晶圆的边框
        private Pen solidPen4WaferNotch;// 实线画笔 for 晶圆的豁口

        private Bitmap master;//主窗格位图
        //private Bitmap masterBack;// 主窗格的底图(OnPaint绘完的图)

        private BufferedGraphicsContext bgContext;
        private BufferedGraphics bgGlobal;
        private BufferedGraphics bgMaster;
        //private Graphics gMaster;// 主窗格绘图器

        private Hourglass timer = new Hourglass();
        #endregion 字段 end

        #region    属性 start
        /// <summary>
        /// 放大镜存在时左右面板的间距
        /// </summary>
        [DefaultValue(6)]
        [Range(5, 20)]
        [Category("布局"), Description("放大镜存在时左右面板的间距")]
        public int PanelGap
        {
            get
            {
                return panelGap;
            }
            set
            {
                panelGap = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// 是否显示聚焦环
        /// </summary>
        [DefaultValue(false)]
        [Category("布局"), Description("是否显示聚焦环")]
        public bool IsShowFocusring
        {
            get
            {
                return isShowFocusring;
            }
            set
            {
                isShowFocusring = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// 是否显示聚焦环的十字线
        /// </summary>
        [DefaultValue(true)]
        [Category("布局"), Description("是否显示聚焦环的十字线")]
        public bool IsShowFocusringReticle
        {
            get
            {
                return isShowFocusringReticle;
            }
            set
            {
                isShowFocusringReticle = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// 是否显示聚焦环的弧形豁口
        /// </summary>
        [DefaultValue(false)]
        [Category("布局"), Description("是否显示聚焦环的弧形豁口")]
        public bool IsShowFocusringNotch
        {
            get
            {
                return isShowFocusringNotch;
            }
            set
            {
                isShowFocusringNotch = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// 是否显示聚焦环的坐标轴名称
        /// </summary>
        [DefaultValue(false)]
        [Category("布局"), Description("是否显示聚焦环的坐标轴名称")]
        public bool IsShowFocusringAxisName
        {
            get
            {
                return isShowFocusringAxisName;
            }
            set
            {
                isShowFocusringAxisName = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// 是否显示聚焦环的原点坐标
        /// </summary>
        [DefaultValue(false)]
        [Category("布局"), Description("是否显示聚焦环的原点坐标")]
        public bool IsShowFocusringOrigin
        {
            get
            {
                return isShowFocusringOrigin;
            }
            set
            {
                isShowFocusringOrigin = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// 是否显示晶圆的圆
        /// </summary>
        [DefaultValue(true)]
        [Category("布局"), Description("是否显示晶圆的圆")]
        public bool IsShowWafer
        {
            get
            {
                return isShowWafer;
            }
            set
            {
                isShowWafer = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// 是否显示晶圆的十字线
        /// </summary>
        [DefaultValue(false)]
        [Category("布局"), Description("是否显示晶圆的十字线")]
        public bool IsShowWaferReticle
        {
            get
            {
                return isShowWaferReticle;
            }
            set
            {
                isShowWaferReticle = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// 是否显示晶圆的弧形豁口
        /// </summary>
        [DefaultValue(true)]
        [Category("布局"), Description("是否显示晶圆的弧形豁口")]
        public bool IsShowWaferNotch
        {
            get
            {
                return isShowWaferNotch;
            }
            set
            {
                isShowWaferNotch = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// 是否显示晶圆的坐标轴名称
        /// </summary>
        [DefaultValue(false)]
        [Category("布局"), Description("是否显示晶圆的坐标轴名称")]
        public bool IsShowWaferAxisName
        {
            get
            {
                return isShowWaferAxisName;
            }
            set
            {
                isShowWaferAxisName = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// 是否显示晶圆的原点坐标
        /// </summary>
        [DefaultValue(false)]
        [Category("布局"), Description("是否显示晶圆的原点坐标")]
        public bool IsShowWaferOrigin
        {
            get
            {
                return isShowWaferOrigin;
            }
            set
            {
                isShowWaferOrigin = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// 是否显示放大镜
        /// </summary>
        [DefaultValue(false)]
        [Category("布局"), Description("是否显示放大镜")]
        public bool IsShowMagnifier
        {
            get
            {
                return isShowMagnifier;
            }
            set
            {
                isShowMagnifier = value;
                ResizePane();
                this.Invalidate();
            }
        }

        /// <summary>
        /// 是否显示容许范围(Go/No Go)的圆
        /// </summary>
        [DefaultValue(false)]
        [Category("布局"), Description("是否显示容许范围(Go/No Go)的圆")]
        public bool IsShowToleranceRange
        {
            get
            {
                return isShowToleranceRange;
            }
            set
            {
                isShowToleranceRange = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// 是否显示气泡
        /// </summary>
        [DefaultValue(false)]
        [Category("布局"), Description("是否显示气泡")]
        public bool IsShowBubble
        {
            get
            {
                return isShowBubble;
            }
            set
            {
                isShowBubble = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// 晶圆矩形框
        /// </summary>
        [Category("布局"), Description("晶圆矩形框")]
        public RectangleF WaferRectF
        {
            get
            {
                return waferRectF;
            }
            set
            {
                waferRectF = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// 放大镜选取框
        /// </summary>
        [DefaultValue(true)]
        [Category("布局"), Description("放大镜选取框")]
        public RectangleF MagnifierRectF
        {
            get
            {
                return magnifierRectF;
            }
            set
            {
                magnifierRectF = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// 气泡矩形框
        /// </summary>
        [Category("布局"), Description("气泡矩形框")]
        public RectangleF BubbleRectF
        {
            get
            {
                return bubbleRectF;
            }
            set
            {
                bubbleRectF = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// 聚焦环的内外圆间距
        /// </summary>
        [DefaultValue(30F)]
        [Category("布局"), Description("聚焦环的内外圆间距")]
        public float FocusringGap
        {
            get
            {
                return focusRingGap;
            }
            set
            {
                focusRingGap = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// 主窗格的绘图半径：绘图时的像素长度
        /// </summary>
        [Category("布局"), Description("主窗格的绘图半径：绘图时的像素长度")]
        public float MasterRadius { get; set; }

        /// <summary>
        /// 聚焦环的绘图半径：绘图时的像素长度
        /// </summary>
        [Category("布局"), Description("聚焦环的绘图半径：绘图时的像素长度")]
        public float FocusRingRadius
        {
            get
            {
                return focusRingDeviceRadius / masterDeviceRadius * MasterRadius;
            }
        }

        /// <summary>
        /// 晶圆的绘图半径：绘图时的像素长度
        /// </summary>
        [Category("布局"), Description("晶圆的绘图半径：绘图时的像素长度")]
        public float WaferRadius
        {
            get
            {
                return waferDeviceRadius / masterDeviceRadius * MasterRadius;
            }
        }

        /// <summary>
        /// 晶圆十字线旋转的角度
        /// </summary>
        [DefaultValue(0F)]
        [Category("布局"), Description("晶圆十字线旋转的角度")]
        public float WaferReticleAngle
        {
            get
            {
                return waferReticleAngle;
            }
            set
            {
                waferReticleAngle = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// 放大镜选取框的绘图半径：绘图时的像素长度
        /// </summary>
        [Category("布局"), Description("放大镜选取框的绘图半径：绘图时的像素长度")]
        public float MagnifierRadius
        {
            get
            {
                return magnifierDeviceRadius / masterDeviceRadius * MasterRadius;
            }
        }

        /// <summary>
        /// 容许范围的绘图半径：绘图时的像素长度
        /// </summary>
        [Category("布局"), Description("容许范围的绘图半径：绘图时的像素长度")]
        public float ToleranceRadius
        {
            get
            {
                return toleranceDeviceRadius / masterDeviceRadius * MasterRadius;
            }
        }

        /// <summary>
        /// 气泡的绘图半径：绘图时的像素长度
        /// </summary>
        [Category("布局"), Description("气泡的绘图半径：绘图时的像素长度")]
        public float BubbleRadius
        {
            get
            {
                return bubbleDeviceRadius / masterDeviceRadius * MasterRadius;
            }
        }

        /// <summary>
        /// 主窗格的设备半径：与设备指标关联时主窗格的最大长度
        /// </summary>
        [DefaultValue(30F)]
        [Category("布局"), Description("主窗格的设备半径：与设备指标关联时主窗格的最大长度")]
        public float MasterDeviceRadius
        {
            get
            {
                return masterDeviceRadius;
            }
            set
            {
                masterDeviceRadius = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// 聚焦环的设备半径：与设备指标关联时聚焦环的最大长度
        /// </summary>
        [DefaultValue(29.5F)]
        [Category("布局"), Description("聚焦环的设备半径：与设备指标关联时聚焦环的最大长度")]
        public float FocusRingDeviceRadius
        {
            get
            {
                return focusRingDeviceRadius;
            }
            set
            {
                focusRingDeviceRadius = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// 晶圆的设备半径：与设备指标关联时晶圆的最大长度
        /// </summary>
        [DefaultValue(25F)]
        [Category("布局"), Description("晶圆的设备半径：与设备指标关联时晶圆的最大长度")]
        public float WaferDeviceRadius
        {
            get
            {
                return waferDeviceRadius;
            }
            set
            {
                waferDeviceRadius = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// 放大镜选取框的设备半径：与设备指标关联时放大镜选取框的最大长度
        /// </summary>
        [DefaultValue(12F)]
        [Category("布局"), Description("放大镜选取框的设备半径：与设备指标关联时放大镜选取框的最大长度")]
        public float MagnifierDeviceRadius
        {
            get
            {
                return magnifierDeviceRadius;
            }
            set
            {
                magnifierDeviceRadius = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// 容许范围的设备半径：与设备指标关联时容许范围的最大长度
        /// </summary>
        [DefaultValue(5F)]
        [Category("布局"), Description("容许范围的设备半径：与设备指标关联时容许范围的最大长度")]
        public float ToleranceDeviceRadius
        {
            get
            {
                return toleranceDeviceRadius;
            }
            set
            {
                toleranceDeviceRadius = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// 气泡的设备半径：与设备指标关联时气泡的最大长度
        /// </summary>
        [DefaultValue(0.75F)]
        [Category("布局"), Description("气泡的设备半径：与设备指标关联时气泡的最大长度")]
        public float BubbleDeviceRadius
        {
            get
            {
                return bubbleDeviceRadius;
            }
            set
            {
                bubbleDeviceRadius = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// 控件与容器的外间距
        /// </summary>
        [DefaultValue(typeof(Padding), "0")]
        [Category("外观"), Description("控件与容器的外间距")]
        public new Padding Margin
        {
            get
            {
                return margin;
            }
            set
            {
                margin = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// 控件与内部子控件的间距
        /// </summary>
        [DefaultValue(typeof(Padding), "5")]
        [Category("外观"), Description("控件与内部子控件的间距")]
        public new Padding Padding
        {
            get
            {
                return padding;
            }
            set
            {
                padding = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// 绘制聚焦环所在图层文字的字体
        /// </summary>
        [Category("外观"), Description("绘制聚焦环所在图层文字的字体")]
        public Font FocusringFont
        {
            get
            {
                return focusringFont;
            }
            set
            {
                focusringFont = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// 绘制晶圆所在图层文字的字体
        /// </summary>
        [Category("外观"), Description("绘制晶圆所在图层文字的字体")]
        public Font WaferFont
        {
            get
            {
                return waferFont;
            }
            set
            {
                waferFont = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// 绘制聚焦环所在图层中文本的颜色
        /// </summary>
        [DefaultValue(typeof(Color), "Black")]
        [Category("外观"), Description("绘制聚焦环所在图层中文本的颜色")]
        public Color FocusRingTextColor
        {
            get
            {
                return focusRingTextColor;
            }
            set
            {
                focusRingTextColor = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// 窗格的边框样式
        /// </summary>
        [DefaultValue(typeof(BorderStyle), "None")]
        [Category("外观"), Description("窗格的边框样式")]
        public BorderStyle PaneBorder
        {
            get
            {
                return paneBorder;
            }
            set
            {
                paneBorder = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// 聚焦环的填充色
        /// </summary>
        [DefaultValue(typeof(Color), "DarkGray")]
        [Category("外观"), Description("聚焦环的填充色")]
        public Color FocusringFillColor
        {
            get
            {
                return focusRingFillColor;
            }
            set
            {
                focusRingFillColor = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// 绘制晶圆所在图层中文本的颜色
        /// </summary>
        [DefaultValue(typeof(Color), "Black")]
        [Category("外观"), Description("绘制晶圆所在图层中文本的颜色")]
        public Color WaferTextColor
        {
            get
            {
                return waferTextColor;
            }
            set
            {
                waferTextColor = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// 晶圆的边框色
        /// </summary>
        [DefaultValue(typeof(Color), "ControlDark")]
        [Category("外观"), Description("晶圆的边框色")]
        public Color WaferBorderColor
        {
            get
            {
                return waferBorderColor;
            }
            set
            {
                waferBorderColor = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// 晶圆豁口的边框色
        /// </summary>
        [DefaultValue(typeof(Color), "ControlDark")]
        [Category("外观"), Description("晶圆豁口的边框色")]
        public Color WaferNotchBorderColor
        {
            get
            {
                return waferNotchBorderColor;
            }
            set
            {
                waferNotchBorderColor = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// 晶圆十字线的颜色
        /// </summary>
        [DefaultValue(typeof(Color), "Purple")]
        [Category("外观"), Description("晶圆十字线的颜色")]
        public Color WaferReticleColor
        {
            get
            {
                return waferReticleColor;
            }
            set
            {
                waferReticleColor = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// 容许范围的圆周色
        /// </summary>
        [DefaultValue(typeof(Color), "Blue")]
        [Category("外观"), Description("容许范围的圆周色")]
        public Color ToleranceCicleBorderColor
        {
            get
            {
                return toleranceCicleBorderColor;
            }
            set
            {
                toleranceCicleBorderColor = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// 气泡的实时颜色
        /// </summary>
        //public Color BubbleColor
        //{
        //    get { return bubbleColor; }
        //    set { bubbleColor = value; }
        //}

        /// <summary>
        /// 容许范围内的气泡颜色
        /// </summary>
        [DefaultValue(typeof(Color), "Green")]
        [Category("外观"), Description("容许范围内的气泡颜色")]
        public Color BubbleColorInTolerance
        {
            get
            {
                return bubbleColorInTolerance;
            }
            set
            {
                bubbleColorInTolerance = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// 超出容许范围的气泡颜色
        /// </summary>
        [DefaultValue(typeof(Color), "Red")]
        [Category("外观"), Description("超出容许范围的气泡颜色")]
        public Color BubbleColorOutOfTolerance
        {
            get
            {
                return bubbleColorOutOfTolerance;
            }
            set
            {
                bubbleColorOutOfTolerance = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// 超出范围的气泡颜色
        /// </summary>
        [DefaultValue(typeof(Color), "Yellow")]
        [Category("外观"), Description("超出范围的气泡颜色")]
        public Color BubbleColorOutOfRange
        {
            get
            {
                return bubbleColorOutOfRange;
            }
            set
            {
                bubbleColorOutOfRange = value;
                this.Invalidate();
            }
        }
        #endregion 属性 end

        #region    构造&析构 start
        public Crosshair1()
        {
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.Selectable, true);
            this.DoubleBuffered = true;

            brush4Background = Brushes.White;
            fillBrush4FocusRingFill = new SolidBrush(focusRingFillColor);
            solidBrush4FocusRingText = new SolidBrush(focusRingTextColor);
            solidBrush4WaferBorder = new SolidBrush(waferBorderColor);
            solidBrush4WaferNotch = new SolidBrush(waferNotchBorderColor);
            solidBrush4WaferText = new SolidBrush(waferTextColor);
            //fillBrush4Bubble = new SolidBrush(bubbleColorInTolerance);
            dashPen4Reticle = new Pen(SystemColors.ControlDark, 2f);
            dashPen4Reticle.DashStyle = DashStyle.Dash;
            dashPen4Reticle.DashPattern = new float[] { 5f, 3f };
            dashPen4Magnifier = new Pen(SystemColors.ControlDark, 2f);
            dashPen4Magnifier.DashStyle = DashStyle.Dash;
            dashPen4Magnifier.DashPattern = new float[] { 3f, 1.5f };
            solidPen4TRC = new Pen(toleranceCicleBorderColor);
            dashPen4WaferReticle = new Pen(waferReticleColor);
            solidPen4WaferBorder = new Pen(waferBorderColor);
            solidPen4WaferNotch = new Pen(Color.Red, 5);

            bgContext = BufferedGraphicsManager.Current;
            bgContext.MaximumBuffer = new Size(this.Width + 1, this.Height + 1);
            bgGlobal = bgContext.Allocate(this.CreateGraphics(), new Rectangle(0, 0, this.Width, this.Height));

            masterRectF = new RectangleF();
            slaveRectF = new RectangleF();
        }

        ~Crosshair1()
        {
            brush4Background?.Dispose();
            fillBrush4FocusRingFill?.Dispose();
            solidBrush4FocusRingText?.Dispose();
            solidBrush4WaferBorder?.Dispose();
            solidBrush4WaferNotch?.Dispose();
            solidBrush4WaferText?.Dispose();
            //fillBrush4Bubble?.Dispose();
            dashPen4Reticle?.Dispose();
            dashPen4Magnifier?.Dispose();
            solidPen4TRC?.Dispose();
            dashPen4WaferReticle?.Dispose();
            solidPen4WaferBorder?.Dispose();
            solidPen4WaferNotch?.Dispose();
            //gMaster?.Dispose();
            //master?.Dispose();
            //masterBack?.Dispose();

            bgGlobal?.Dispose();
            bgMaster?.Dispose();
            bgContext?.Dispose();
        }
        #endregion 构造&析构 end

        #region    事件重写 start
        /// <summary>
        /// 重写Paint事件
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            //timer.Start();
            DrawToBuffer(bgGlobal.Graphics, bgMaster.Graphics);

            //timer.Stop();
            //timer.Restart();

            bgGlobal?.Render(e.Graphics);
            bgMaster?.Render(e.Graphics);

            //timer.Stop();
            //timer.PrintAll();
        }

        /// <summary>
        /// 重写Resize事件
        /// </summary>
        /// <param name="eventargs"></param>
        protected override void OnResize(EventArgs eventargs)
        {
            ResizePane();
        }
        #endregion 事件重写 end

        #region    公开方法 start
        public void Test()
        {
            using (var g = this.CreateGraphics())
            {
                GraphicsPath path1 = new GraphicsPath();
                GraphicsPath path2 = new GraphicsPath();
                //g.CompositingMode = CompositingMode.SourceCopy;
                path1.AddArc(new Rectangle(210, 320, 200, 200), 0F, 360F);
                path2.AddRectangle(new Rectangle(200, 130, 400, 350));

                Region region1 = new Region(path1);
                Region region2 = new Region(path2);
                Pen p = new Pen(Color.Red, 5);
                path1.Widen(p);
                path2.Widen(p);
                Region region3 = new Region(path1);
                Region region4 = new Region(path2);
                Region region5 = region1.Clone();

                region1.Intersect(region2);
                region1.Intersect(region3);

                region2.Intersect(region4);
                region2.Exclude(region5);

                g.FillRegion(Brushes.Red, region2);
                g.FillRegion(Brushes.Green, region1);

                //using (var brush = new SolidBrush(Color.Transparent))
                //{
                //    g.FillRectangle(brush, new Rectangle(300,220,500,500)); 
                //}
            }
        }

        public void Test1(PaintEventArgs e)
        {
            Rectangle rect = e.ClipRectangle;
            BufferedGraphicsContext currentContext = BufferedGraphicsManager.Current;
            BufferedGraphics myBuffer = currentContext.Allocate(e.Graphics, e.ClipRectangle);
            Graphics g = myBuffer.Graphics;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.PixelOffsetMode = PixelOffsetMode.HighSpeed;
            g.Clear(this.BackColor);
            //foreach (IShape drawobject in doc.drawObjectList)
            //{
            //    if (rect.IntersectsWith(drawobject.Rect))
            //    {
            //        drawobject.Draw(g);
            //        if (drawobject.TrackerState == config.Module.Core.TrackerState.Selected
            //            && this.CurrentOperator == Enum.Operator.Transfrom)//仅当编辑节点操作时显示图元热点
            //        {
            //            drawobject.DrawTracker(g);
            //        }
            //    }
            //}
            myBuffer.Render(e.Graphics);
            g.Dispose();
            myBuffer.Dispose();//释放资源
        }

        /// <summary>
        /// 以气泡所在矩形底图覆盖原位置的方式不断绘制气泡
        /// </summary>
        /// <param name="point">圆心坐标</param>
        /// <param name="r">圆周半径</param>
        public void RedrawBubble1(PointF point, float r)
        {
            if (this.InvokeRequired)
            {
                while (!this.IsHandleCreated)
                {
                    //解决窗体关闭时出现“访问已释放句柄“的异常
                    if (this.Disposing || this.IsDisposed)
                        return;
                }
                this.BeginInvoke(new Action<PointF, float>(RedrawBubble1), point, r);
            }
            else
            {
                //if (masterBack == null)
                if (master == null)
                {
                    return;
                }
                var g = this.CreateGraphics();

                var mbCopy = master.Clone() as Bitmap;
                //mbCopy.Save("E:\\MasterBackCopy.jpg");
                var gMaster = Graphics.FromImage(mbCopy);
                gMaster.InitAnti();
                var obj = gMaster.ToRightHandedCoordinate();

                if (lastItem != null)
                {
                    gMaster.DrawImage(lastItem.Item1, lastItem.Item2);
                }

                var rectF = GetRectFFromOrigin(point, r);
                //var ltPoint = DrawingCoordinateToBitmapCoordinateInMaster(rectF.Location);
                var ltPoint = rectF.Location.ToBitmapCoordinate(masterRectF, CoordinateType.DrawingCoordinate);
                var rectFInMaster = new RectangleF(ltPoint, rectF.Size);
                rectFInMaster.Inflate(4F, 4F);
                var backBmp = mbCopy.Clone(rectFInMaster, mbCopy.PixelFormat);
                //backBmp.Save("E:\\PrevBubbleBack.jpg");
                gMaster.FillEllipse(Brushes.Green, rectF);
                lastItem = Tuple.Create(backBmp, rectF);

                //mbCopy.Save("E:\\BubbleAndBack.jpg");
                g.DrawImage(mbCopy, masterRectF);
                InterceptContentFromMasterToRightPanel(g, mbCopy);
                mbCopy.Dispose();

                gMaster.Dispose();
                g.Dispose();
            }
        }

        /// <summary>
        /// 以气泡所在矩形底图覆盖原位置的方式不断绘制气泡
        /// </summary>
        /// <param name="point">圆心坐标</param>
        /// <param name="r">圆周半径</param>
        public void RedrawBubble0(PointF point, float r)
        {
            if (this.InvokeRequired)
            {
                while (!this.IsHandleCreated)
                {
                    //解决窗体关闭时出现“访问已释放句柄“的异常
                    if (this.Disposing || this.IsDisposed)
                        return;
                }
                this.BeginInvoke(new Action<PointF, float>(RedrawBubble0), point, r);
            }
            else
            {
                //sw.Restart();
                //if (masterRect == Rectangle.Empty)
                //{
                //    return;
                //}

                var pDrawing = ToDrawingPoint(point);
                var pr = GetPolarRadius(pDrawing);
                if (pr <= ToleranceRadius)
                {
                    bubbleColor = bubbleColorInTolerance;
                }
                else if (ToleranceRadius < pr && pr <= WaferRadius)
                {
                    bubbleColor = bubbleColorOutOfTolerance;
                }
                else if (pr > WaferRadius)
                {
                    bubbleColor = bubbleColorOutOfRange;
                }
                bubbleOrigin = pDrawing;


                this.Invalidate();
                //bgMaster?.Render(this.CreateGraphics());
                //this.CreateGraphics().SetClip(this.ClientRectangle);
                //this.Invalidate(masterRect);
                //this.Update();

                //sw.Stop();
                //Debug.Print($"重绘气泡耗时{sw.ElapsedMilliseconds}ms");
            }
        }

        /// <summary>
        /// 主窗格底图完整覆盖的方式绘制气泡
        /// </summary>
        /// <param name="point">圆心坐标</param>
        /// <param name="zDip">Z轴倾角</param>
        public void RedrawBubble(PointF point, float zDip)
        {
            if (this.InvokeRequired)
            {
                while (!this.IsHandleCreated)
                {
                    //解决窗体关闭时出现“访问已释放句柄“的异常
                    if (this.Disposing || this.IsDisposed)
                        return;
                }
                this.BeginInvoke(new Action<PointF, float>(RedrawBubble), point, zDip);
            }
            else
            {
                //sw.Restart();
                //if (masterBack == null)
                if (master == null)
                {
                    return;
                }
                var g = this.CreateGraphics();

                //sw.Stop();
                //Debug.Print($"初始化g耗时{sw.ElapsedMilliseconds}ms");
                //sw.Restart();

                var mbCopy = master.Clone() as Bitmap;
                //mbCopy.Save("E:\\MasterBackCopy.jpg");
                var gMaster = Graphics.FromImage(mbCopy);
                gMaster.InitAnti();
                var obj = gMaster.ToRightHandedCoordinate();

                //sw.Stop();
                //Debug.Print($"初始化gMaster耗时{sw.ElapsedMilliseconds}ms");
                //sw.Restart();

                var pDrawing = ToDrawingPoint(point);
                var pr = GetPolarRadius(pDrawing);
                var color = bubbleColorInTolerance;
                if (pr <= ToleranceRadius)
                {
                    color = bubbleColorInTolerance;
                }
                else if (ToleranceRadius < pr && pr <= WaferRadius)
                {
                    color = bubbleColorOutOfTolerance;
                }
                else if (pr > WaferRadius)
                {
                    color = bubbleColorOutOfRange;
                }
                gMaster.FillCircle(color, pDrawing, BubbleRadius);

                //sw.Stop();
                //Debug.Print($"绘制气泡耗时{sw.ElapsedMilliseconds}ms");
                //sw.Restart();

                //mbCopy.Save("E:\\BubbleAndBack.jpg");
                g.DrawImage(mbCopy, masterRectF);

                //sw.Stop();
                //Debug.Print($"气泡填充到窗格耗时{sw.ElapsedMilliseconds}ms");
                //sw.Restart();

                InterceptContentFromMasterToRightPanel(g, mbCopy);

                //sw.Stop();
                //Debug.Print($"刷新右窗格耗时{sw.ElapsedMilliseconds}ms");
                //sw.Restart();

                mbCopy.Dispose();
                gMaster.Dispose();
                g.Dispose();

                //sw.Stop();
                //Debug.Print($"释放对象耗时{sw.ElapsedMilliseconds}ms");
            }
        }

        /// <summary>
        /// 刷新气泡
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public Task RefreshBubble(float x, float y, float z)
        {
            return Task.Factory.StartNew(() =>
            {
                var point = new PointF(x, y);
                RedrawBubble0(point, z);
            });
        }
        #endregion 公开方法 end

        #region    私有方法 start
        /// <summary>
        /// 重设窗格大小
        /// </summary>
        private void ResizePane()
        {
            int width = this.ClientRectangle.Width;
            int height = this.ClientRectangle.Height;
            //Debug.Print($"Height={Height},Width={Width}");
            int left = this.Padding.Left;
            int right = this.Padding.Right;
            int top = this.Padding.Top;
            int bottom = this.Padding.Bottom;
            width -= (left + right + right);// 此处需要减2倍的Padding.Right才能修正误差
            height -= (top + bottom);
            int halfWidth = (width - panelGap) / 2;
            //Debug.Print($"height={height},halfWidth={halfWidth}");

            if (isShowMagnifier)
            {// 双面板
                if (height >= halfWidth)
                {
                    masterRectF.Width = masterRectF.Height = halfWidth;
                    masterRectF.Y = (this.Height - masterRectF.Height) / 2;
                    masterRectF.X = left;
                }
                else
                {
                    masterRectF.Width = masterRectF.Height = height;
                    masterRectF.Y = top;
                    masterRectF.X = (this.Width - panelGap) / 2f - masterRectF.Width;
                }

                slaveRectF.Y = masterRectF.Top;
                slaveRectF.X = this.Left + this.Width / 2f + panelGap / 2f;
                slaveRectF.Height = masterRectF.Height;
                slaveRectF.Width = masterRectF.Width;
            }
            else
            {// 单面板
             //Debug.Print($"height={height},width={width}");
                if (height <= width)
                {
                    masterRectF.Width = masterRectF.Height = height;
                    masterRectF.Y = top;
                    masterRectF.X = (this.Width - masterRectF.Width) / 2;
                }
                else
                {
                    masterRectF.Width = masterRectF.Height = width;
                    masterRectF.Y = (this.Height - masterRectF.Height) / 2;
                    masterRectF.X = left;
                }
            }
            MasterRadius = masterRectF.Width / 2;
            //Debug.Print($"pnl_Left.Width={pnl_Left.Width},pnl_Left.Height={pnl_Left.Height}");

            if (this.Width <= 0 || this.Height <= 0)
            {
                return;
            }

            bgContext.MaximumBuffer = new Size(this.Width + 1, this.Height + 1);
            if (bgGlobal != null)
            {
                bgGlobal.Dispose();
                bgGlobal = null;
            }
            bgGlobal = bgContext.Allocate(this.CreateGraphics(), this.ClientRectangle);

            masterRect = Rectangle.Round(masterRectF);
            bgContext.MaximumBuffer = new Size(masterRect.Width + 1, masterRect.Height + 1);
            if (bgMaster != null)
            {
                bgMaster.Dispose();
                bgMaster = null;
            }
            bgMaster = bgContext.Allocate(this.CreateGraphics(), masterRect);
            //bgMaster.Graphics.SetClip(masterRect, CombineMode.Replace);
            //DrawToBuffer(bgGlobal.Graphics, bgMaster.Graphics);
            this.Invalidate();
        }

        /// <summary>
        /// 绘制到缓冲区
        /// </summary>
        /// <param name="gGlobal">全局绘图器</param>
        /// <param name="gMaster">主窗格绘图器</param>
        private void DrawToBuffer(Graphics gGlobal, Graphics gMaster)
        {
            //sw.Restart();

            //gGlobal.InitAnti();
            gGlobal.Clear(SystemColors.Control);// 重绘全局的背景色，否则背景是全黑

            if (paneBorder == BorderStyle.FixedSingle)
            {
                DrawBorder(gGlobal);
            }

            gMaster.InitAnti();
            gMaster.Clear(Color.White);
            var obj = gMaster.ToRightHandedCoordinate();

            //RectangleF rectf = obj.Item1;
            GraphicsState gs = obj.Item2;

            // 是否绘制聚焦环的十字线
            if (isShowFocusringReticle)
            {
                DrawReticle(gMaster);
            }
            // 是否绘制聚焦环
            if (isShowFocusring)
            {
                DrawFocusRing(gMaster);
            }
            // 是否绘制晶圆
            if (isShowWafer)
            {
                DrawWafer(gMaster);
            }

            // 是否绘制容许范围(圆周)
            if (isShowToleranceRange)
            {
                DrawToleranceRangeCicle(gMaster);
            }
            // 是否绘制气泡
            if (isShowBubble)
            {
                DrawBubble(gMaster);
            }
            // 是否绘制放大框
            if (isShowMagnifier)
            {
                DrawMagnifier(gMaster);
                DrawMagnifierLink(gMaster, gGlobal);
                InterceptContentFromMasterToRightPanel1(gGlobal);
            }

            gMaster.Restore(gs);

            // 是否绘制聚焦环的坐标轴名称
            if (isShowFocusringAxisName)
            {
                DrawFocusringAxisName(gMaster);
            }

            // 是否绘制聚焦环的原点坐标
            if (isShowFocusringOrigin)
            {
                DrawFocusringOrigin(gMaster);
            }

            // 是否绘制晶圆的坐标轴名称
            if (isShowWaferAxisName)
            {
                DrawWaferAxisName(gMaster);
            }

            // 是否绘制晶圆的原点坐标
            if (isShowWaferOrigin)
            {
                DrawWaferOrigin(gMaster);
            }
            //gMaster.ScaleTransform(1, -1); // 将纵坐标轴方向变为朝上

            //sw.Stop();
            //timeLst.Add(sw.ElapsedMilliseconds);
            //sw.Restart();

            //if (IsShowMagnifier)
            //{
            //    //InterceptContentFromMasterToRightPanel(g, master);
            //}

            //sw.Stop();
            //timeLst.Add(sw.ElapsedMilliseconds);
            ////Debug.Print($"初始化master耗时{timeLst[0]},绘制master耗时{timeLst[1]},绘制放大内容耗时{timeLst[2]}");
            //Debug.Print($"全部重绘耗时{timeLst[0]}");
            //timeLst.Clear();
        }

        /// <summary>
        /// 绘制边框
        /// </summary>
        /// <param name="g"></param>
        private void DrawBorder(Graphics g)
        {
            var masterRectFCopy = masterRectF;
            masterRectFCopy.Offset(-0.5F, -0.5F);// x左负右正 y上负下正
            masterRectFCopy.Inflate(0.5F, 1F);// 加正减负
            g.DrawRectangleF(Color.Black, masterRectFCopy);
            var slaveRectFCopy = slaveRectF;
            slaveRectFCopy.Offset(-0.5F, -0.5F);
            slaveRectFCopy.Inflate(0.5F, 1F);
            g.DrawRectangleF(Color.Red, slaveRectFCopy);
        }

        /// <summary>
        /// 绘制十字线
        /// </summary>
        /// <param name="g"></param>
        /// <param name="container"></param>
        private void DrawReticle(Graphics g)
        {
            RectangleF container = g.VisibleClipBounds;

            // 聚焦环十字准线X轴起点
            //focusRingCrossXStart = new PointF(container.X + focusRingGap, container.Y + container.Height / 2);
            focusRingReticleXStart = new PointF(container.X, container.Y + container.Height / 2);
            // 聚焦环十字准线X轴终点
            //focusRingCrossXEnd = new PointF(container.Right - focusRingGap, container.Y + container.Height / 2);
            focusRingReticleXEnd = new PointF(container.Right, container.Y + container.Height / 2);

            // 绘制十字准线X轴
            g.DrawLine(dashPen4Reticle, focusRingReticleXStart, focusRingReticleXEnd);

            // 聚焦环十字准线Y轴起点
            //focusRingCrossYStart = new PointF(container.X + container.Width / 2, container.Y + focusRingGap);
            focusRingReticleYStart = new PointF(container.X + container.Width / 2, container.Y);
            // 聚焦环十字准线Y轴终点
            //focusRingCrossYEnd = new PointF(container.X + container.Width / 2, container.Bottom - focusRingGap);
            focusRingReticleYEnd = new PointF(container.X + container.Width / 2, container.Bottom);

            // 绘制十字准线X轴
            g.DrawLine(dashPen4Reticle, focusRingReticleYStart, focusRingReticleYEnd);

            //// 增加斜线标识
            //var pLT = container.Location;
            //var pRB = new PointF(container.Right, container.Bottom);
            //g.DrawLine(dashPen, pLT, pRB);

            //var pRT = new PointF(container.Right, container.Top);
            //var pLB = new PointF(container.Left, container.Bottom);
            //g.DrawLine(dashPen, pRT, pLB);
        }

        /// <summary>
        /// 绘制放大镜选取框
        /// </summary>
        private void DrawMagnifier(Graphics g)
        {
            //var reduction = container.Width * magnifierScale;
            //magnifierRectF = RectangleF.Inflate(container, reduction, reduction);
            //magnifierRectF = RectangleF.Inflate(container, -180F, -180F);
            magnifierRectF = GetRectFFromOrigin(focusRingOrigin, MagnifierRadius);

            //using (var dashPen = new Pen(SystemColors.ControlDark, 2f))
            //{
            //    dashPen.DashStyle = DashStyle.Dash;
            //    dashPen.DashPattern = new float[] { 3f, 1.5f };

            //    g.DrawRectangle(dashPen, magnifierRectF.X, magnifierRectF.Y, magnifierRectF.Width, magnifierRectF.Height);
            //}
            g.DrawRectangle(dashPen4Magnifier, magnifierRectF.X, magnifierRectF.Y, magnifierRectF.Width, magnifierRectF.Height);
        }

        /// <summary>
        /// 绘制容许范围的圆
        /// </summary>
        private void DrawToleranceRangeCicle(Graphics g)
        {
            //var reduction = container.Width * magnifierScale / 0.78F;
            //var toleranceRectF = RectangleF.Inflate(container, reduction, reduction);
            var toleranceRectF = GetRectFFromOrigin(focusRingOrigin, ToleranceRadius);
            //using (var pen = new Pen(toleranceCicleBorderColor))
            //{
            //    g.DrawArc(pen, toleranceRectF, 0f, 360f);
            //}
            g.DrawArc(solidPen4TRC, toleranceRectF, 0f, 360f);
        }

        /// <summary>
        /// 绘制放大镜选取框与右面板的两条连线
        /// </summary>
        /// <param name="gMaster">主窗格的Graphics对象</param>
        /// <param name="g">控件的Graphics对象</param>
        private void DrawMagnifierLink(Graphics gMaster, Graphics g)
        {
            // 放大镜选取框的左边框的左上点和左下点(主窗格内的绘图坐标)
            var ltPoint4Mgr = magnifierRectF.Location;
            var lbPoint4Mgr = new PointF(magnifierRectF.Left, magnifierRectF.Bottom);

            // 主窗格边框的右上点和右下点(页面坐标)
            var mrtPoint = new PointF(masterRectF.Right, masterRectF.Top);
            var mrbPoint = new PointF(masterRectF.Right, masterRectF.Bottom);

            // 副窗格边框的左上点和左下点(页面坐标)
            var sltPoint = new PointF(slaveRectF.Left, slaveRectF.Top);
            var slbPoint = new PointF(slaveRectF.Left, slaveRectF.Bottom);

            // 放大镜选取框的边框的左上点与副窗格边框的左上点连线 与 主窗格右边框 交叉得到的交点(页面坐标)
            var ltPoint4MgrOnPage = ltPoint4Mgr.ToPageCoordinate(masterRectF, CoordinateType.DrawingCoordinate);
            var tcPoint = GetCrossPoint(ltPoint4MgrOnPage, sltPoint, mrtPoint, mrbPoint);
            // 放大镜选取框的边框的左上点与副窗格边框的左上点连线 与 主窗格右边框 交叉得到的交点(绘图坐标)
            var tcPointOnDrawing = tcPoint.ToDrawingCoordinate(masterRectF, CoordinateType.PageCoordinate);

            // 放大镜选取框的边框的左下点与副窗格边框的左下点连线 与 主窗格右边框 交叉得到的交点(页面坐标)
            var lbPoint4MgrOnPage = lbPoint4Mgr.ToPageCoordinate(masterRectF, CoordinateType.DrawingCoordinate);
            var bcPoint = GetCrossPoint(lbPoint4MgrOnPage, slbPoint, mrtPoint, mrbPoint);
            // 放大镜选取框的边框的左下点与副窗格边框的左下点连线 与 主窗格右边框 交叉得到的交点(绘图坐标)
            var bcPointOnDrawing = bcPoint.ToDrawingCoordinate(masterRectF, CoordinateType.PageCoordinate);

            gMaster.DrawLine(dashPen4Reticle, ltPoint4Mgr, tcPointOnDrawing); // 需要绘图坐标(不准)
            g.DrawLine(dashPen4Reticle, tcPoint, sltPoint);// 需要页面坐标

            gMaster.DrawLine(dashPen4Reticle, lbPoint4Mgr, bcPointOnDrawing); // 需要绘图坐标
            g.DrawLine(dashPen4Reticle, bcPoint, slbPoint);// 需要页面坐标(不准)

            //dashPen4Reticle.Dispose();
        }

        /// <summary>
        /// 绘制放大镜选取框与右面板的两条连线
        /// </summary>
        /// <param name="gMaster">主窗格的Graphics对象</param>
        /// <param name="g">控件的Graphics对象</param>
        private void DrawMagnifierLink(Graphics g)
        {
            //dashPen4Reticle = new Pen(Color.Red, 2f);
            //dashPen4Reticle.DashStyle = DashStyle.Dash;
            //dashPen4Reticle.DashPattern = new float[] { 3f, 1.5f };

            // 放大镜选取框的左边框的左上点和左下点(主窗格内的绘图坐标)
            var ltPoint4Mgr = magnifierRectF.Location;
            var lbPoint4Mgr = new PointF(magnifierRectF.Left, magnifierRectF.Bottom);
            // 放大镜选取框的左边框的左上点和左下点(页面坐标)
            var ltPoint4MgrOnPage = ltPoint4Mgr.ToPageCoordinate(masterRectF, CoordinateType.DrawingCoordinate);
            var lbPoint4MgrOnPage = lbPoint4Mgr.ToPageCoordinate(masterRectF, CoordinateType.DrawingCoordinate);

            // 副窗格边框的左上点和左下点(页面坐标)
            var sltPoint = new PointF(slaveRectF.Left, slaveRectF.Top);
            var slbPoint = new PointF(slaveRectF.Left, slaveRectF.Bottom);

            g.DrawLine(dashPen4Reticle, ltPoint4MgrOnPage, sltPoint);// 需要页面坐标
            g.DrawLine(dashPen4Reticle, lbPoint4MgrOnPage, slbPoint);// 需要页面坐标

            //dashPen4Reticle.Dispose();
        }

        /// <summary>
        /// 从主窗格中截取放大镜选取框的内容画到副窗格
        /// </summary>
        /// <param name="g"></param>
        /// <param name="master"></param>
        private void InterceptContentFromMasterToRightPanel(Graphics g, Bitmap bmp)
        {
            var upStartPoint = magnifierRectF.Location.ToBitmapCoordinate(masterRectF, CoordinateType.DrawingCoordinate);
            //upStartPoint.X += 0.089981075376272201541F;
            //upStartPoint.Offset(-20F,-20F);
            var mgrRectF = new RectangleF(upStartPoint.Subtract(-0.09F, 1.09F), magnifierRectF.Size);
            //mgrRectF.Offset(0.089981075375F, -0.05F);
            mgrRectF.Inflate(-1.5F, -2.3F);
            //mgrRectF.Inflate(2F, 2F);
            var slave = bmp.Clone(mgrRectF, bmp.PixelFormat);
            //var slave = this.GetBmpFromRectangleF(magnifierRectF, slaveRectF);
            g.DrawImage(slave, slaveRectF);
            //var newImage = resizeImage(slave, slaveRectF.Size);
            //g.DrawImage(newImage, slaveRectF);
            //slave.Save("F:\\Slave.jpg");
            slave.Dispose();
        }

        /// <summary>
        /// 从主窗格中截取放大镜选取框的内容画到副窗格
        /// </summary>
        /// <param name="g"></param>
        private void InterceptContentFromMasterToRightPanel1(Graphics g)
        {
            //sw.Restart();
            var upStartPoint = magnifierRectF.Location.ToPageCoordinate(masterRectF, CoordinateType.DrawingCoordinate);

            // Location的变化是负向左正向下，Size的变化是负减正加
            var mgrRectF = new RectangleF(upStartPoint.Subtract(0.9f, 0.35f), magnifierRectF.Size.Subtract(-1.301f, -1.301f));
            var slave = this.GetBmpFromRectangleF0(Rectangle.Round(mgrRectF), Rectangle.Round(slaveRectF));
            //sw.Stop();
            //timeLst.Add(sw.ElapsedMilliseconds);

            //sw.Restart();
            if (slave != null)
            {
                g.DrawImage(slave, slaveRectF);
                slave.Dispose();
            }

            //sw.Stop();
            //timeLst.Add(sw.ElapsedMilliseconds);
            //Debug.Print($"生成slave位图耗时{timeLst[0]},绘制位图到slave耗时{timeLst[1]}");
        }

        /// <summary>
        /// 缩放图片-不起作用
        /// </summary>
        /// <param name="imgToResize"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        private static Image resizeImage(Image imgToResize, SizeF size)
        {
            //获取图片宽度
            int sourceWidth = imgToResize.Width;
            //获取图片高度
            int sourceHeight = imgToResize.Height;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;
            //计算宽度的缩放比例
            nPercentW = (size.Width / (float)sourceWidth);
            //计算高度的缩放比例
            nPercentH = (size.Height / (float)sourceHeight);

            if (nPercentH < nPercentW)
                nPercent = nPercentH;
            else
                nPercent = nPercentW;
            //期望的宽度
            int destWidth = (int)(sourceWidth * nPercent);
            //期望的高度
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap b = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage((Image)b);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            //绘制图像
            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();
            return (Image)b;
        }

        /// <summary>
        /// 绘制聚焦环
        /// </summary>
        private void DrawFocusRing(Graphics g)
        {
            //RectangleF outerRectF = RectangleF.Inflate(container, focusRingReduction, focusRingReduction);
            //RectangleF innerRectF = RectangleF.Inflate(outerRectF, -focusRingGap, -focusRingGap);
            var outerRectF = GetRectFFromOrigin(focusRingOrigin, FocusRingRadius);
            var innerRectF = GetRectFFromOrigin(focusRingOrigin, FocusRingRadius - focusRingGap);

            using (var gp = new GraphicsPath())
            {
                gp.StartFigure();
                gp.AddArc(outerRectF, 0F, 360F);
                gp.AddArc(innerRectF, 0F, 360F);
                gp.CloseAllFigures();

                //g.FillPath(new SolidBrush(focusRingFillColor), gp);
                g.FillPath(fillBrush4FocusRingFill, gp);
            }
        }

        /// <summary>
        /// 绘制晶圆
        /// </summary>
        private void DrawWafer(Graphics g)
        {
            //waferRectF = RectangleF.Inflate(g.VisibleClipBounds, -5, -5);
            waferRectF = GetRectFFromOrigin(focusRingOrigin, WaferRadius);

            // 是否绘制晶圆的十字线
            if (isShowWaferReticle)
            {
                int offset = 0;// 直线两端点与矩形边框之间的间距
                // 晶圆十字准线X轴起点(A)
                waferReticleXStart = new PointF(waferRectF.X + offset, waferRectF.Y + WaferRadius);
                // 晶圆十字准线X轴终点(B)
                waferReticleXEnd = new PointF(waferRectF.Right - offset, waferRectF.Y + WaferRadius);

                float width1 = 12F;
                // 晶圆十字准线Y轴起点(D)
                waferReticleYStart = new PointF(waferRectF.X + WaferRadius, waferRectF.Top + offset);
                // 晶圆十字准线Y轴终点(C)
                waferReticleYEnd = new PointF(waferRectF.X + WaferRadius, waferRectF.Bottom - offset - width1);

                //using (var pen = new Pen(waferReticleColor))
                //{
                //    // 绘制十字准线X轴
                //    g.DrawLine(pen, waferReticleXStart, waferReticleXEnd);
                //    // 绘制十字准线X轴
                //    g.DrawLine(pen, waferReticleYStart, waferReticleYEnd);
                //}
                // 绘制十字准线X轴
                g.DrawLine(dashPen4WaferReticle, waferReticleXStart, waferReticleXEnd);
                // 绘制十字准线X轴
                g.DrawLine(dashPen4WaferReticle, waferReticleYStart, waferReticleYEnd);

                waferOrigin = GetCrossPoint(waferReticleXStart, waferReticleXEnd, waferReticleYStart, waferReticleYEnd);
            }

            if (isShowWaferNotch)
            {
                var gp1 = new GraphicsPath();
                gp1.AddArc(waferRectF, 0F, 360F);

                var gp2 = new GraphicsPath();
                //float r = waferRectF.Height / 2;
                float r = WaferRadius;
                float topY = r - 17F;
                var topPointF = new PointF(0, topY);
                var leftPointF = new PointF(-8, r);
                var RightPointF = new PointF(8, r);
                gp2.AddLine(leftPointF, topPointF);
                gp2.AddLine(topPointF, RightPointF);

                Region region1 = new Region(gp2);
                Region region2 = new Region(gp1);
                //Pen p = new Pen(Color.Red, 5);
                //gp1.Widen(p);
                //gp2.Widen(p);
                gp1.Widen(solidPen4WaferNotch);
                gp2.Widen(solidPen4WaferNotch);
                Region region3 = new Region(gp2);
                Region region4 = new Region(gp1);
                Region region5 = region1.Clone();

                region1.Intersect(region2);
                region1.Intersect(region3);

                region2.Intersect(region4);
                region2.Exclude(region5);

                g.FillRegion(solidBrush4WaferBorder, region2);
                g.FillRegion(solidBrush4WaferNotch, region1);

                gp1.Dispose();
                gp2.Dispose();
                region1.Dispose();
                region2.Dispose();
                region3.Dispose();
                region4.Dispose();
                region5.Dispose();
            }
            else
            {
                g.DrawArc(solidPen4WaferBorder, waferRectF, 0f, 360f);
            }
        }

        /// <summary>
        /// 绘制气泡
        /// </summary>
        /// <param name="g"></param>
        /// <param name="container"></param>
        private void DrawBubble(Graphics g)
        {
            if (bubbleOrigin == PointF.Empty)
            {
                return;
            }
            bubbleRectF = GetRectFFromOrigin(bubbleOrigin, BubbleRadius);
            using (var fillBrush4Bubble = new SolidBrush(bubbleColor))
            {
                g.FillEllipse(fillBrush4Bubble, bubbleRectF);
            }
        }

        /// <summary>
        /// 绘制聚焦环的坐标轴名称
        /// </summary>
        /// <param name="g"></param>
        /// <param name="container"></param>
        private void DrawFocusringAxisName(Graphics g)
        {
            SizeF xLeft_Size = g.MeasureString("x-", focusringFont);
            float xLeft_Top = -xLeft_Size.Height / 2;

            SizeF xRight_Size = g.MeasureString("x+", focusringFont);
            float xRight_Left = focusRingReticleXEnd.X - xRight_Size.Width;
            float xRight_Top = -xRight_Size.Height / 2;

            SizeF yBottom_Size = g.MeasureString("y-", focusringFont);
            float yBottom_Left = -yBottom_Size.Width / 2;
            float yBottom_Top = focusRingReticleYEnd.Y - yBottom_Size.Height;

            SizeF yTop_Size = g.MeasureString("y+", focusringFont);
            float yTop_Left = -yTop_Size.Width / 2;
            float yTop_Top = focusRingReticleYStart.Y;

            float pOffset = 10.5F;// 标签坐标的强制偏移距离
            g.DrawString("x-", focusringFont, solidBrush4FocusRingText, focusRingReticleXStart.X + pOffset, xLeft_Top);
            g.DrawString("x+", focusringFont, solidBrush4FocusRingText, xRight_Left - pOffset, xRight_Top);
            g.DrawString("y-", focusringFont, solidBrush4FocusRingText, yBottom_Left, yBottom_Top - pOffset);
            g.DrawString("y+", focusringFont, solidBrush4FocusRingText, yTop_Left, yTop_Top + pOffset);
        }

        /// <summary>
        /// 绘制聚焦环的原点坐标
        /// </summary>
        /// <param name="g"></param>
        /// <param name="container"></param>
        private void DrawFocusringOrigin(Graphics g)
        {
            focusRingOrigin = g.RenderingOrigin;
            g.DrawString($"({focusRingOrigin.X},{focusRingOrigin.Y})", focusringFont, solidBrush4FocusRingText, focusRingOrigin.X, focusRingOrigin.Y);
        }

        /// <summary>
        /// 绘制晶圆的坐标轴名称
        /// </summary>
        /// <param name="g"></param>
        /// <param name="container"></param>
        private void DrawWaferAxisName(Graphics g)
        {
            SizeF xLeft_Size = g.MeasureString("B", waferFont);
            float xLeft_Top = -xLeft_Size.Height / 2;

            SizeF xRight_Size = g.MeasureString("A", waferFont);
            float xRight_Left = waferReticleXEnd.X - xRight_Size.Width;
            float xRight_Top = -xRight_Size.Height / 2;

            SizeF yBottom_Size = g.MeasureString("D", waferFont);
            float yBottom_Left = -yBottom_Size.Width / 2;
            float yBottom_Top = waferReticleYEnd.Y - yBottom_Size.Height;

            SizeF yTop_Size = g.MeasureString("C", waferFont);
            float yTop_Left = -yTop_Size.Width / 2;
            float yTop_Top = waferReticleYStart.Y;

            float pOffset = 10.5F;// 标签坐标的强制偏移距离
            g.DrawString("B", waferFont, solidBrush4WaferText, waferReticleXStart.X + pOffset, xLeft_Top);
            g.DrawString("A", waferFont, solidBrush4WaferText, xRight_Left - pOffset, xRight_Top);
            g.DrawString("D", waferFont, solidBrush4WaferText, yBottom_Left, yBottom_Top - pOffset);
            g.DrawString("C", waferFont, solidBrush4WaferText, yTop_Left, yTop_Top + pOffset);
        }

        /// <summary>
        /// 绘制晶圆的原点坐标
        /// </summary>
        /// <param name="g"></param>
        /// <param name="container"></param>
        private void DrawWaferOrigin(Graphics g)
        {
            PointF waferOriginFlipY = waferOrigin.FlipY();
            g.DrawLine(dashPen4Reticle, focusRingOrigin, waferOriginFlipY);// 绘制晶圆与聚焦环两个圆心之间的连线
            g.DrawString($"({waferOrigin.X},{waferOrigin.Y})", waferFont, solidBrush4WaferText, waferOrigin.X, waferOriginFlipY.Y);
        }

        /// <summary>
        /// 获取两直线交点
        /// </summary>
        /// <param name="pStartA">A线段起点</param>
        /// <param name="pEndA">A线段终点</param>
        /// <param name="pStartB">B线段起点</param>
        /// <param name="pEndB">B线段终点</param>
        /// <returns></returns>
        private PointF GetCrossPoint(PointF pStartA, PointF pEndA, PointF pStartB, PointF pEndB)
        {
            float x1 = pStartA.X;
            float y1 = pStartA.Y;
            float x2 = pEndA.X;
            float y2 = pEndA.Y;
            float x3 = pStartB.X;
            float y3 = pStartB.Y;
            float x4 = pEndB.X;
            float y4 = pEndB.Y;
            float a = x2 == x1 ? float.PositiveInfinity : (y2 - y1) / (x2 - x1); //需考虑分母不能为0 即x2=x1 l1垂直于x轴
            float b = x4 == x3 ? float.PositiveInfinity : (y4 - y3) / (x4 - x3); //需考虑分母不能为0 即x4=x3 l2垂直于x轴

            if (a == b)
            {//斜率相同,说明平行 无交点
                Debug.WriteLine("两直线平行,无交点");
                return PointF.Empty;
            }

            float x, y = 0;

            if (x2 == x1)
            {//L1垂直于x轴  则x=x1=x2 a=infinity 想办法消除a
                x = x1;
                //(y-y3)/(x-x3)=b 且x=x1 变换得y=bx1-bx3+y3
                y = b * x1 - b * x3 + y3;
                //return new PointF(x, y);
            }
            else if (x4 == x3)
            {//L2垂直于x轴 则x=x3=x4 b=infinity 
                x = x3;
                y = a * x - a * x1 + y1;
                //return new PointF(x, y);
            }
            else
            {
                x = (a * x1 - y1 + y3 - b * x3) / (a - b);
                y = a * x - a * x1 + y1;
            }

            float XAmax = x1 > x2 ? x1 : x2;
            float XAmin = x1 < x2 ? x1 : x2;
            float YAmax = y1 > y2 ? y1 : y2;
            float YAmin = y1 < y2 ? y1 : y2;
            float XBmax = x3 > x4 ? x3 : x4;
            float XBmin = x3 < x4 ? x3 : x4;
            float YBmax = y3 > y4 ? y3 : y4;
            float YBmin = y3 < y4 ? y3 : y4;

            if (XAmax < x || x < XAmin || XBmax < x || x < XBmin || YAmax < y || y < YAmin || YBmax < y || y < YBmin)
            {
                return PointF.Empty;
            }

#if !TRACE
            //Debug.WriteLine("[{lng:'" + x1 + "',lat:'" + y1 + "'},{lng:'" + x2 + "',lat:'" + y2 + "'},{lng:'" + x3 + "',lat:'" + y3 + "'},{lng:'" + x4 + "',lat:'" + y4 + "'},{lng:'" + x + "',lat:'" + y + "'}]");
            Debug.Print($"pIntersect = X:{x},Y:{y}");
#endif
            return new PointF(x, y);
        }

        /// <summary>
        /// 返回指定圆心和半径的矩形
        /// </summary>
        /// <param name="originPoint">指定圆心</param>
        /// <param name="r">半径</param>
        /// <returns></returns>
        private RectangleF GetRectFFromOrigin(PointF originPoint, float r)
        {
            PointF ltPoint = originPoint.Subtract(r, r);
            SizeF size = new SizeF(2 * r, 2 * r);
            RectangleF rectF = new RectangleF(ltPoint, size);
            return rectF;
        }

        /// <summary>
        /// 设备坐标转绘图坐标
        /// </summary>
        /// <param name="point">设备坐标</param>
        /// <returns></returns>
        private PointF ToDrawingPoint(PointF point)
        {
            var pDrawing = PointF.Empty;
            pDrawing.X = point.X / masterDeviceRadius * MasterRadius;
            pDrawing.Y = point.Y / masterDeviceRadius * MasterRadius;
            return pDrawing;
        }

        /// <summary>
        /// 返回指定坐标点的极径
        /// </summary>
        /// <param name="point">指定坐标点</param>
        /// <returns>极径</returns>
        private double GetPolarRadius(PointF point)
        {
            return Math.Sqrt(Math.Pow(point.X, 2d) + Math.Pow(point.Y, 2d));
        }
        #endregion 私有方法 end
    }
}