using dbe.FoundationLibrary.Core.Win32.API;
using dbe.FoundationLibrary.Core.Extensions;

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace dbe.FoundationLibrary.Windows.UI.CustomForms
{
    /// <summary>
    /// 无边框的基类窗体
    /// </summary>
    public partial class BaseWithBorderless : Form
    {
        #region    字段 start
        private int titleHeight = 50;// 标题栏高度
        private int lblCloseLocationY;
        private bool isPressMouseLeftButton = false;//是否已按下鼠标左键
        private Point currentPosition;// 鼠标当前位置
        #endregion 字段 end

        #region    属性 start
        public static bool IsDesingMode
        {
            get
            {
                bool ReturnFlag = false;
                if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                    ReturnFlag = true;
                else if (Process.GetCurrentProcess().ProcessName == "devenv")
                    ReturnFlag = true;
                //if (ReturnFlag)
                //    Msg.Warning("设计模式");
                //else Msg.Warning("非设计模式！");
                return ReturnFlag;
            }
        }
        #endregion 属性 end
        public event EventHandler Button1_Click;
        #region    构造与析构 start
        public BaseWithBorderless()
        {
            InitializeComponent();
            if (IsDesingMode) return;//如果处于设计模式，返回

            User32.SystemParametersInfo(SystemParametersInfo.SPI_SETDROPSHADOW, 0, Const.TRUE, 0); // - 开启阴影效果

            //User32.SystemParametersInfo(SystemParametersInfo.SPI_SETDROPSHADOW, 0, Const.FLASE, 0); // - 关闭阴影效果
            //this.MouseDown.RaiseEvent<MouseEventArgs>(null, MouseEventArgs.Empty);
            
            Button1_Click.RaiseEvent(this, EventArgs.Empty);
            //var btn = new Button();
            //btn.Click+= (sender, e) => { this.Close();  };
            User32.SetClassLong(this.Handle, Const.GCL_STYLE, User32.GetClassLong(this.Handle, Const.GCL_STYLE) | Const.CS_DropSHADOW);
        }
        #endregion 构造与析构 end

        #region    事件重写 start
        protected override void OnLayout(LayoutEventArgs levent)
        {
            //var clipBound = this.ClientRectangle;
            //clipBound.Offset(0, titleHeight);
            //clipBound.Inflate(0, -titleHeight);
            //this.CreateGraphics().SetClip(clipBound);
            //base.OnLayout(levent);
            //this.Refresh();
        }

        // 窗体重绘事件 => 在子窗体中重绘以达到自定义标题和关闭按钮的效果
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            //e.Graphics.PageUnit = GraphicsUnit.Display;
            g.ResetClip();

            //画边框
            ControlPaint.DrawBorder(g, this.ClientRectangle,
            Color.DarkGray, 1, ButtonBorderStyle.Solid, //左边
            Color.DarkGray, 1, ButtonBorderStyle.Solid, //上边
            Color.DarkGray, 1, ButtonBorderStyle.Solid, //右边
            Color.DarkGray, 1, ButtonBorderStyle.Solid);//底边

            LinearGradientBrush brush = new LinearGradientBrush(new Point(0, 0), new Point(this.Width, this.Width), Color.Lavender, Color.Gainsboro);

            Pen pen = new Pen(brush);
            pen.Width = titleHeight;// 标题栏背景的高度
            g.DrawLine(pen, 0, 0, this.Width, 0);// 画标题栏背景

            var font = new Font("新宋体", 13.8F, FontStyle.Bold);
            var titleSizeF = g.MeasureString(this.Text, font);
            var locationY = (pen.Width - titleSizeF.Height) / 8;
            g.DrawString(this.Text, font, SystemBrushes.ControlText, 4F, locationY, StringFormat.GenericTypographic);// 画出Form标题

            var clipBound = g.ClipBounds;
            clipBound.Offset(0, titleHeight);
            clipBound.Inflate(0, -titleHeight);
            g.SetClip(clipBound);

            pen.Dispose();
            brush.Dispose();
        }

        // 窗体尺寸改变事件 => 在子窗体中改变窗体尺寸后立刻重绘以保正关闭按钮处于正确的位置
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            lbl_Close.Left = this.Width - lbl_Close.Size.Width;
            this.Refresh();
        }

        // 重写标题改变事件 => 在子窗体中改变标题后立刻重绘以显示效果
        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            this.Refresh();
        }
        #endregion 事件重写 end

        #region    事件处理 start
        private void BaseWithBorderless_Load(object sender, EventArgs e)
        {
            lblCloseLocationY = lbl_Close.Location.Y + 1;
        }

        private void BaseWithBorderless_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isPressMouseLeftButton = true;
                currentPosition = MousePosition;//鼠标的x坐标为当前窗体左上角坐标
            }
        }

        private void BaseWithBorderless_MouseMove(object sender, MouseEventArgs e)
        {
            if (isPressMouseLeftButton)
            {
                this.Left += MousePosition.X - currentPosition.X;//根据鼠标x坐标确定窗体的左边坐标x
                this.Top += MousePosition.Y - currentPosition.Y;//根据鼠标的y坐标窗体的顶部，即Y坐标
                currentPosition = MousePosition;
            }
        }

        private void BaseWithBorderless_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                currentPosition = Point.Empty; //设置初始状态
                isPressMouseLeftButton = false;
            }
        }

        private void lbl_Close_MouseHover(object sender, EventArgs e)
        {
            lbl_Close.BackColor = Color.White;
        }

        private void lbl_Close_MouseLeave(object sender, EventArgs e)
        {
            lbl_Close.BackColor = SystemColors.Control;
        }

        private void lbl_Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion 事件处理 end

        #region    私有方法 start
        #endregion 私有方法 end
    }
}