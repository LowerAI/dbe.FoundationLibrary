using System;
using System.Drawing;
using System.Windows.Forms;

namespace dbe.FoundationLibrary.Windows.Extensions
{
    public static class ToolStripItemExtension
    {
        /// <summary>
        /// 由于直接设置背景色无效，所以需要桥接一下
        /// </summary>
        /// <param name="tssl"></param>
        /// <param name="color"></param>
        public static void SetBackColor(this ToolStripItem tssl, Color color)
        {
            var toolStrip = tssl.GetCurrentParent();
            if (toolStrip == null)
            {
                return;
            }
            if (toolStrip.InvokeRequired)
            {
                while (!toolStrip.IsHandleCreated)
                {
                    //解决窗体关闭时出现“访问已释放句柄“的异常
                    if (toolStrip.Disposing || toolStrip.IsDisposed)
                        return;
                }
                toolStrip.BeginInvoke(new Action<ToolStripItem, Color>(SetBackColor), tssl, color);
            }
            else
            {
                Bitmap bmp = new Bitmap(tssl.Width, tssl.Height);
                using (Graphics gl = Graphics.FromImage(bmp))
                {
                    gl.FillRectangle(new SolidBrush(color), tssl.ContentRectangle);
                    tssl.BackgroundImage = bmp;
                }
            }
        }

        /// <summary>
        /// 设置进度式颜色
        /// </summary>
        /// <param name="tssl">ToolStripItem对象</param>
        /// <param name="color">要设定的前景色</param>
        /// <param name="progress">表示进度的数字，范围是0.01~1.0</param>
        public static void SetProgressColor(this ToolStripItem tssl, Color color, float progress)
        {
            var toolStrip = tssl.GetCurrentParent();
            if (toolStrip == null)
            {
                return;
            }
            if (toolStrip.InvokeRequired)
            {
                while (!toolStrip.IsHandleCreated)
                {
                    //解决窗体关闭时出现“访问已释放句柄“的异常
                    if (toolStrip.Disposing || toolStrip.IsDisposed)
                        return;
                }
                toolStrip.BeginInvoke(new Action<ToolStripItem, Color, float>(SetProgressColor), tssl, color, progress);
            }
            else
            {
                if (progress > 1.0F)
                {
                    progress /= 100;
                }

                Bitmap bmp = new Bitmap((int)(tssl.Width * (progress == 0F ? 0.01F : progress)), tssl.Height);
                using (Graphics gl = Graphics.FromImage(bmp))
                {
                    gl.FillRectangle(new SolidBrush(color), tssl.ContentRectangle.X, tssl.ContentRectangle.Y, bmp.Width, bmp.Height);
                    tssl.Image = bmp;
                }
                tssl.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
                tssl.TextImageRelation = TextImageRelation.Overlay;
                tssl.TextAlign = ContentAlignment.MiddleCenter;
                tssl.ImageAlign = ContentAlignment.MiddleLeft;
                tssl.ImageScaling = ToolStripItemImageScaling.None;
            }
        }
    }
}