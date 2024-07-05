using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace dbe.FoundationLibrary.Windows.UI.CustomControls
{
    /// <summary>
    /// 带进度条的遮罩层
    /// </summary>
    public partial class MaskLayer : Control
    {
        #region    字段 start
        /// <summary>
        /// 进度条
        /// </summary>
        public ProgressBar progressBar = new ProgressBar();
        #endregion 字段 end

        #region    属性 start
        private int _Alpha = 125;
        /// <summary>
        /// 透明度<para>范围：0~255（完全透明~完全不透明）</para><para>默认：125（半透明）</para>
        /// </summary>
        [Category("DemoUI"), Description("透明度\r\n范围：0~255（完全透明~完全不透明）\r\n默认：125（半透明）")]
        public int Alpha
        {
            get { return _Alpha; }
            set
            {
                if (value < 0) value = 0;
                if (value > 255) value = 255;
                _Alpha = value;
                Invalidate();
            }
        }

        /// <summary>
        /// 是否启用点击隐藏功能<para>默认：否</para>
        /// </summary>
        [Category("DemoUI"), Description("是否启用点击隐藏功能\r\n默认：否")]
        public bool EnabledClickHide { get; set; } = false;

        /// <summary>
        /// 是否处于显示状态
        /// </summary>
        [Category("LESLIE_UI"), Description("是否处于显示状态"), Browsable(false)]
        public bool IsShow { get; private set; } = true;
        #endregion 属性 end

        #region    构造函数 start
        /// <summary>
        /// 遮罩层
        /// </summary>
        /// <param name="child">要在其中显示的子控件</param>
        public MaskLayer(Control child)
        {
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.Opaque, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            CreateControl();
            Visible = false;

            this.Dock = DockStyle.Fill;
            this.Controls.Add(child);
        }

        public MaskLayer()
        {
            new MaskLayer(progressBar);
        }
        #endregion 构造函数 end

        #region 重写函数 end
        /// <summary>
        /// OnPaint
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            SolidBrush BackColorBrush = new SolidBrush(Color.FromArgb(_Alpha, BackColor));
            e.Graphics.FillRectangle(BackColorBrush, e.ClipRectangle);
            BackColorBrush.Dispose();
        }

        /// <summary>
        /// 点击事件
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            if (EnabledClickHide)
            {
                HideMask();
            }
        }
        #endregion 重写函数 end

        #region    公开方法 start
        /// <summary>
        /// 显示遮罩层
        /// </summary>
        public void ShowMask()
        {
            try
            {
                this.BeginInvoke(new Action(() =>
                {
                    IsShow = true;
                    SendKeys.Send("{Tab}");

                    BringToFront();
                    this.Visible = true;
                    this.BackColor = Color.Black;
                    Show();

                    int x = (int)(this.Width * 0.1);
                    int y = this.Height / 2;
                    this.progressBar.Location = new Point(x, y);
                    this.progressBar.Name = "progressBar";
                    int w = (int)(this.Width * 0.8);
                    this.progressBar.Size = new Size(w, 23);
                    this.progressBar.TabIndex = 2;
                }));
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 设置进度条显示值
        /// </summary>
        /// <param name="value"></param>
        public void SetProgressBarValue(int value)
        {
            this.BeginInvoke(new Action(() =>
            {
                if (value <= progressBar.Maximum)
                {
                    progressBar.Value = value;
                }
            }));
        }

        /// <summary>
        /// 隐藏遮罩层
        /// </summary>
        public void HideMask()
        {
            try
            {
                this.BeginInvoke(new Action(() =>
                {
                    IsShow = false;
                    SendToBack();
                    Visible = false;
                    Hide();
                }));
            }
            catch (Exception)
            {
            }
        }
        #endregion 公开方法 start
    }
}