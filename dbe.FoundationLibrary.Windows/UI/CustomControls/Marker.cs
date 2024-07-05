using dbe.FoundationLibrary.Core.Extensions;
using dbe.FoundationLibrary.Windows.Extensions;

using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace dbe.FoundationLibrary.Windows.UI.CustomControls
{
    /// <summary>
    /// 标志物
    /// </summary>
    [DefaultProperty("Name")]
    [Docking(DockingBehavior.Ask)]
    [ComVisible(true)]
    public class Marker : Control
    {
        #region    字段 Start
        // 外圆的边框色
        private Color outterBorderColor = Color.Black;
        // 内圆的边框色
        private Color innerBorderColor = Color.Green;
        // 内圆的填充色
        private Color fillColor = Color.LawnGreen;
        // 内外椭圆间距
        private float gap = 3f;
        #endregion 字段 End

        #region    属性 Start
        /// <summary>
        /// 外圆的边框色
        /// </summary>
        [DefaultValue(typeof(Color), "Black")]
        [Category("外观"), Description("外边框色")]
        public Color OutterBorderColor
        {
            get => outterBorderColor;
            set
            {
                outterBorderColor = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// 内圆的边框色
        /// </summary>
        [DefaultValue(typeof(Color), "Green")]
        [Category("外观"), Description("内边框色")]
        public Color InnerBorderColor
        {
            get => innerBorderColor;
            set
            {
                innerBorderColor = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// 内圆的填充色
        /// </summary>
        [DefaultValue(typeof(Color), "LawnGreen")]
        [Category("外观"), Description("内圆的填充色")]
        public Color FillColor
        {
            get => fillColor;
            set
            {
                fillColor = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// 内外圆间距
        /// </summary>
        [DefaultValue(3F)]
        [Category("布局"), Description("内外圆间距")]
        public float Gap
        {
            get => gap;
            set
            {
                gap = value;
                this.Invalidate();
            }
        }
        #endregion 属性 End

        #region    构造与析构 Start
        public Marker()
        {
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.Selectable, true);
            this.DoubleBuffered = true;

            this.Margin = new Padding(0);
            this.Font = new Font("微软雅黑", 9F, FontStyle.Regular | FontStyle.Bold);
        }
        #endregion 构造与析构 End

        #region    事件重写 Start
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.InitAnti();

            var clientRect = g.VisibleClipBounds.ChangeSize(-1f, -1f);
            //var clientRect = g.ClipBounds;
            //g.DrawRectangleF(borderColor, clientRect);

            var outterRectF = clientRect.ChangeSize(Padding.Left, Padding.Top, Padding.Right, Padding.Bottom);

            // 创建椭圆路径
            using (GraphicsPath path = new GraphicsPath())
            {
                //path.AddArc(outterRectF, -20f, 110f);

                // 绘制阴影
                using (Pen shadowPen = new Pen(Color.Gainsboro, 4f))
                {
                    g.DrawPath(shadowPen, path);
                }

                // 绘制椭圆
                using (var outterBrush = new Pen(OutterBorderColor))
                {
                    //g.DrawRectangleF(borderColor, outterRectF);
                    g.DrawEllipse(outterBrush, outterRectF);// 绘制外椭圆
                }
            }

            var innerRectF = outterRectF.Scale(-gap, -gap);
            using (var innerBrush = new Pen(InnerBorderColor))
            {
                g.DrawEllipse(innerBrush, innerRectF);// 绘制内椭圆
            }
            using (var fillBrush = new SolidBrush(fillColor))
            {
                g.FillEllipse(fillBrush, innerRectF);// 填充内椭圆
            }

            using (var fontBrush = new SolidBrush(OutterBorderColor))
            {
                var textRectF = innerRectF.Scale(-2, -2);
                //g.DrawRectangleF(borderColor, textRectF);
                //g.DrawString(this.Text, Font, fontBrush, textRectF);
                TextFormatFlags flags = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter;
                TextRenderer.DrawText(g, this.Text, Font, Rectangle.Round(textRectF), ForeColor, Color.Transparent, flags);
            }
        }
        #endregion 事件重写 End

        #region    公开方法 Start
        public void ChangeFillColor(Color color)
        {
            fillColor = color;
            this.Invalidate();
        }
        #endregion 公开方法 End

        #region    私有方法 Start
        #endregion 私有方法 End
    }
}