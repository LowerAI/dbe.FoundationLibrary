using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace dbe.FoundationLibrary.Windows.UI.CustomControls
{
    /// <summary>
    /// 自定义的开关按钮
    /// C# Winform 开关按钮_huang714的博客-CSDN博客_winform开关按钮 https://blog.csdn.net/huang714/article/details/124187896
    /// </summary>
    public class ToggleButton : CheckBox
    {
        #region    字段 start
        private Color onBackColor = Color.MediumSlateBlue;
        private Color onForeColor = Color.WhiteSmoke;
        private Color offBackColor = Color.Gray;
        private Color offForeColor = Color.Gainsboro;

        private bool solidStyle = true;
        #endregion 字段 end

        #region    属性 start
        /// <summary>
        /// 打开/按下时的背景色
        /// </summary>
        [DefaultValue(typeof(Color), "MediumSlateBlue")]
        [Category("外观"), Description("打开/按下时的背景色")]
        public Color OnBackColor
        {
            get
            {
                return onBackColor;
            }
            set
            {
                onBackColor = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// 打开/按下时的前景色
        /// </summary>
        [DefaultValue(typeof(Color), "WhiteSmoke")]
        [Category("外观"), Description("打开/按下时的前景色")]
        public Color OnForeColor
        {
            get
            {
                return onForeColor;
            }
            set
            {
                onForeColor = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// 关闭/弹起时的背景色
        /// </summary>
        [DefaultValue(typeof(Color), "Gray")]
        [Category("外观"), Description("关闭/弹起时的背景色")]
        public Color OffBackColor
        {
            get
            {
                return offBackColor;
            }
            set
            {
                offBackColor = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// 关闭/弹起时的前景色
        /// </summary>
        [DefaultValue(typeof(Color), "Gainsboro")]
        [Category("外观"), Description("关闭/弹起时的前景色")]
        public Color OffForeColor
        {
            get
            {
                return offForeColor;
            }
            set
            {
                offForeColor = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// 文本
        /// </summary>
        [Category("外观"), Description("文本")]
        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                base.Text = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// 是否使用实线样式
        /// </summary>
        [DefaultValue(false)]
        [Category("外观"), Description("是否使用实线样式")]
        public bool SolidStyle
        {
            get
            {
                return solidStyle;
            }
            set
            {
                solidStyle = value;
                this.Invalidate();
            }
        }
        #endregion 属性 end

        #region    构造函数 start
        public ToggleButton()
        {
            //SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.Selectable, true);
            this.MinimumSize = new Size(45, 22);
        }
        #endregion 构造函数 end

        #region    私有方法 start
        private GraphicsPath GetFigurePath()
        {
            int arcSize = this.Height - 1;
            Rectangle leftArc = new Rectangle(0, 0, arcSize, arcSize);
            Rectangle rightArc = new Rectangle(this.Width - arcSize - 2, 0, arcSize, arcSize);
            GraphicsPath path = new GraphicsPath();
            path.StartFigure();
            path.AddArc(leftArc, 90, 180);
            path.AddArc(rightArc, 270, 180);
            path.CloseAllFigures();
            return path;
        }
        #endregion 私有方法 end

        #region    事件重写 start
        protected override void OnPaint(PaintEventArgs pevent)
        {
            int toggleSize = this.Height - 5;
            Graphics graphics = pevent.Graphics;
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.Clear(this.Parent.BackColor);

            if (this.Checked)
            {
                if (solidStyle)
                {
                    graphics.FillPath(new SolidBrush(onBackColor), GetFigurePath());
                }
                else
                {
                    graphics.DrawPath(new Pen(OnBackColor, 2), GetFigurePath());
                }
                graphics.FillEllipse(new SolidBrush(onForeColor), new Rectangle(this.Width - this.Height + 1
                    , 2, toggleSize, toggleSize));
            }
            else
            {
                if (solidStyle)
                {
                    graphics.FillPath(new SolidBrush(offBackColor), GetFigurePath());
                }
                else
                {
                    graphics.DrawPath(new Pen(offBackColor, 2), GetFigurePath());
                }
                graphics.FillEllipse(new SolidBrush(offForeColor), new Rectangle(2, 2, toggleSize, toggleSize));
            }
        }
        #endregion 事件重写 end
    }
}