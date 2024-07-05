using dbe.FoundationLibrary.Core.Win32.API;

using System;
using System.Drawing;
using System.Windows.Forms;

namespace dbe.FoundationLibrary.Windows.Extensions
{
    /// <summary>
    /// Control扩展类
    /// </summary>
    public static class ControlExtension
    {
        /// <summary>
        /// 将矩形的Location从World坐标(自定义原心的坐标系)转换为页面坐标(控件默认坐标系)
        /// 返回指定矩形的Location从World坐标(自定义原心的坐标系)转换为页面坐标(控件默认坐标系)之后的副本
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="control"></param>
        /// <returns></returns>
        public static Rectangle WorldToPage(this Control control, Rectangle rect)
        {
            var point = rect.Location.ViewToClient(control);
            rect.Location = point;
            return rect;
        }

        /// <summary>
        /// 将矩形的Location从World坐标(自定义原心的坐标系)转换为页面坐标(控件默认坐标系)
        /// 返回指定矩形的Location从World坐标(自定义原心的坐标系)转换为页面坐标(控件默认坐标系)之后的副本
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="control"></param>
        /// <returns></returns>
        public static RectangleF WorldToPage(this Control control, RectangleF rectF)
        {
            var point = rectF.Location.ViewToClient(control);
            rectF.Location = point;
            return rectF;
        }

        /// <summary>
        /// 页面坐标(控件默认坐标系)转换为World坐标(自定义原心的坐标系)
        /// 返回指定矩形的Location从页面坐标(控件默认坐标系)转换为World坐标(自定义原心的坐标系)之后的副本
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="control"></param>
        /// <returns></returns>
        public static Rectangle PageToWorld(this Control control, Rectangle rect)
        {
            var point = rect.Location.ClientToView(control);
            rect.Location = point;
            return rect;
        }

        /// <summary>
        /// 页面坐标(控件默认坐标系)转换为World坐标(自定义原心的坐标系)
        /// 返回指定矩形的Location从页面坐标(控件默认坐标系)转换为World坐标(自定义原心的坐标系)之后的副本
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="control"></param>
        /// <returns></returns>
        public static RectangleF PageToWorld(this Control control, RectangleF rectF)
        {
            var point = rectF.Location.ClientToView(control);
            rectF.Location = point;
            return rectF;
        }

        /// <summary>
        /// 获取当前矩形在指定控件(World坐标系)内的位图形式
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="control"></param>
        /// <returns></returns>
        public static Bitmap ToBitmapInWorld(this Control control, Rectangle rect)
        {
            var allBmp = new Bitmap(control.ClientRectangle.Width, control.ClientRectangle.Height);
            control.DrawToBitmap(allBmp, control.ClientRectangle);

            var bmp = allBmp.Clone(control.WorldToPage(rect), allBmp.PixelFormat);
            return bmp;
        }

        /// <summary>
        /// 获取当前矩形在指定控件(页面坐标系)内的位图形式
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="control"></param>
        /// <returns></returns>
        public static Bitmap ToBitmapInPage(this Control control, Rectangle rect)
        {
            var allBmp = new Bitmap(control.ClientRectangle.Width, control.ClientRectangle.Height);
            control.DrawToBitmap(allBmp, control.ClientRectangle);

            var bmp = allBmp.Clone(rect, allBmp.PixelFormat);
            return bmp;
        }

        /// <summary>
        /// 获取当前矩形在指定控件(World坐标系)内的位图形式
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="control"></param>
        /// <returns></returns>
        public static Bitmap ToBitmapInWorld(this Control control, RectangleF rectF)
        {
            var allBmp = new Bitmap(control.ClientRectangle.Width, control.ClientRectangle.Height);
            // 注意此处调用只用到了targetBounds的Size，没有用到targetBounds的Location，所以无法直接截取需要的矩形
            control.DrawToBitmap(allBmp, control.ClientRectangle);

            var bmp = allBmp.Clone(control.WorldToPage(rectF), allBmp.PixelFormat);

            //var rect = Rectangle.Truncate(control.WorldToPage(rectF));
            //var rect = Rectangle.Truncate(rectF);
            //var bmp = new Bitmap(rect.Width, rect.Height);
            //control.DrawToBitmap(bmp, rect);
            return bmp;
        }

        /// <summary>
        /// 获取当前矩形在指定控件(页面坐标系)内的位图形式
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="control"></param>
        /// <returns></returns>
        public static Bitmap ToBitmapInPage(this Control control, RectangleF rectF)
        {
            var allBmp = new Bitmap(control.ClientRectangle.Width, control.ClientRectangle.Height);
            control.DrawToBitmap(allBmp, control.ClientRectangle);

            var bmp = allBmp.Clone(rectF, allBmp.PixelFormat);
            return bmp;
        }

        /// <summary>
        /// 控件(窗口)的截图，控件被其他窗口(而非本窗口内控件)遮挡时也可以正确截图，使用BitBlt方法
        /// </summary>
        /// <param name="control"></param>
        /// <param name="rectSrc"></param>
        /// <param name="rectDest"></param>
        /// <returns></returns>
        public static Bitmap GetBmpFromRectangleF0(this Control control, Rectangle rectSrc, Rectangle rectDest)
        {
            //调用API截屏
            IntPtr hSrce = User32.GetWindowDC(control.Handle);
            IntPtr hDest = Gdi32.CreateCompatibleDC(hSrce);
            IntPtr hBmp = Gdi32.CreateCompatibleBitmap(hSrce, rectSrc.Width, rectSrc.Height);
            IntPtr hOldBmp = Gdi32.SelectObject(hDest, hBmp);

            if (Gdi32.BitBlt(hDest, 0, 0, rectDest.Width, rectDest.Height, hSrce, rectSrc.X, rectSrc.Y, (int)(CopyPixelOperation.SourceCopy)))
            {
                Bitmap bmp = Image.FromHbitmap(hBmp);
                Gdi32.SelectObject(hDest, hOldBmp);
                Gdi32.DeleteObject(hBmp);
                Gdi32.DeleteDC(hDest);
                User32.ReleaseDC(control.Handle, hSrce);
                return bmp;
            }
            return null;
        }

        public static Bitmap GetBmpFromRectangleF(this Control control, Graphics g, RectangleF rectF)
        {
            //调用API截屏
            //IntPtr hSrce = User32.GetWindowDC(control.Handle);
            IntPtr hSrce = g.GetHdc();
            int srcWidth = (int)Math.Truncate(rectF.Width);
            int srcHeight = (int)Math.Truncate(rectF.Height);
            IntPtr hBmp = Gdi32.CreateCompatibleBitmap(hSrce, srcWidth, srcHeight);
            Bitmap bmp = Image.FromHbitmap(hBmp);
            //User32.ReleaseDC(control.Handle, hSrce);
            Gdi32.DeleteObject(hBmp);
            return bmp;
        }

        /// <summary>
        /// 控件内部区域截图
        /// </summary>
        /// <param name="control">容器控件</param>
        /// <param name="screenshotArea">截图区域</param>
        /// <returns></returns>
        public static Image CaptureInnerArea(this Control control, Rectangle screenshotArea)
        {
            Image MyImage = null;
            IntPtr hdc = User32.GetWindowDC(control.Handle);

            if (hdc != IntPtr.Zero)
            {
                IntPtr hdcMem = Gdi32.CreateCompatibleDC(hdc);
                if (hdcMem != IntPtr.Zero)
                {
                    IntPtr hbitmap = Gdi32.CreateCompatibleBitmap(hdc, control.Width, control.Height);
                    if (hbitmap != IntPtr.Zero)
                    {
                        IntPtr ip = Gdi32.SelectObject(hdcMem, hbitmap);
                        if (ip != IntPtr.Zero)
                        {
                            bool a = User32.PrintWindow(control.Handle, hdcMem, 1);
                            Gdi32.DeleteObject(hbitmap);
                            Image tempImg = Image.FromHbitmap(hbitmap);
                            Bitmap b = new Bitmap(tempImg);
                            MyImage = b.Clone(screenshotArea, b.PixelFormat);
                        }
                    }
                }
            }

            return MyImage;
        }

        /// <summary>
        /// 设置进度式颜色
        /// </summary>
        /// <param name="lbl">ToolStripItem对象</param>
        /// <param name="color">要设定的前景色</param>
        /// <param name="progress">表示进度的数字，范围是0.01~1.0</param>
        public static void SetProgressColor(this Label lbl, Color color, float progress)
        {
            if (lbl.InvokeRequired)
            {
                while (!lbl.IsHandleCreated)
                {
                    //解决窗体关闭时出现“访问已释放句柄“的异常
                    if (lbl.Disposing || lbl.IsDisposed)
                        return;
                }
                lbl.BeginInvoke(new Action<Label, Color, float>(SetProgressColor), lbl, color, progress);
            }
            else
            {
                if (progress > 1.0F)
                {
                    progress /= 100;
                }

                Bitmap bmp = new Bitmap((int)(lbl.Width * (progress == 0F ? 0.01f : progress)), lbl.Height);
                using (Graphics gl = Graphics.FromImage(bmp))
                {
                    gl.FillRectangle(new SolidBrush(color), lbl.DisplayRectangle.X, lbl.DisplayRectangle.Y, bmp.Width, bmp.Height);
                    lbl.Image = bmp;
                }
                lbl.TextAlign = ContentAlignment.MiddleCenter;
                lbl.ImageAlign = ContentAlignment.MiddleLeft;
            }
        }

        /// <summary>
        /// Sets the cursor to a hand when the mouse is over the control
        /// </summary>
        /// <param name="control"></param>
        static public void SetCursorOnHover(this Control control)
        {
            control.MouseMove += (sender, e) => { Cursor.Current = Cursors.Hand; };
            control.MouseLeave += (sender, e) => { Cursor.Current = Cursors.Default; };
        }
    }
}