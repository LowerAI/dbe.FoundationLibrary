using dbe.FoundationLibrary.Core.Win32.API;

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace dbe.FoundationLibrary.Windows.Extensions
{
    /// <summary>
    /// 缩放的部分函数来自c#工具类之Bitmap缩放帮忙类_aiji7208的博客-CSDN博客 https://blog.csdn.net/aiji7208/article/details/101291544
    /// </summary>
    public static class BitmapExtension
    {
        /// <summary>
        /// 缩放图片
        /// </summary>
        /// <param name="bitmap">原图片</param>
        /// <param name="width">新图片宽度</param>
        /// <param name="height">新图片高度</param>
        /// <returns>新图片</returns>
        public static Bitmap ScaleToSize(this Bitmap bitmap, int width, int height)
        {
            if (bitmap.Width == width && bitmap.Height == height)
            {
                return bitmap;
            }

            var scaledBitmap = new Bitmap(width, height);
            //float scaleY = (float)width / bitmap.Width;
            //float scaleX = (float)height / bitmap.Height;
            //scaledBitmap.SetResolution(bitmap.Width * scaleX, bitmap.Height * scaleY);

            using (var g = Graphics.FromImage(scaledBitmap))
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(bitmap, 0, 0, width, height);
            }

            return scaledBitmap;
        }

        /// <summary>
        /// 缩放图片
        /// </summary>
        /// <param name="bitmap">原图片</param>
        /// <param name="size">新图片大小</param>
        /// <returns>新图片</returns>
        public static Bitmap ScaleToSize(this Bitmap bitmap, Size size)
        {
            return bitmap.ScaleToSize(size.Width, size.Height);
        }

        /// <summary>
        /// 按比例来缩放
        /// </summary>
        /// <param name="bitmap">原图</param>
        /// <param name="ratio">ratio大于1,则放大;否则缩小</param>
        /// <returns>新图片</returns>
        public static Bitmap ScaleToSize(this Bitmap bitmap, float ratio)
        {
            return bitmap.ScaleToSize((int)(bitmap.Width * ratio), (int)(bitmap.Height * ratio));
        }

        /// <summary>
        /// 按给定长度/宽度等比例缩放
        /// </summary>
        /// <param name="bitmap">原图</param>
        /// <param name="width">新图片宽度</param>
        /// <param name="height">新图片高度</param>
        /// <returns>新图片</returns>
        public static Bitmap ScaleProportional(this Bitmap bitmap, int width, int height)
        {
            float proportionalWidth, proportionalHeight;

            if (width.Equals(0))
            {
                proportionalWidth = ((float)height) / bitmap.Size.Height * bitmap.Width;
                proportionalHeight = height;
            }
            else if (height.Equals(0))
            {
                proportionalWidth = width;
                proportionalHeight = ((float)width) / bitmap.Size.Width * bitmap.Height;
            }
            else if (((float)width) / bitmap.Size.Width * bitmap.Size.Height <= height)
            {
                proportionalWidth = width;
                proportionalHeight = ((float)width) / bitmap.Size.Width * bitmap.Height;
            }
            else
            {
                proportionalWidth = ((float)height) / bitmap.Size.Height * bitmap.Width;
                proportionalHeight = height;
            }

            return bitmap.ScaleToSize((int)proportionalWidth, (int)proportionalHeight);
        }

        /// <summary>
        /// 按给定长度/宽度缩放,同时可以设置背景色
        /// </summary>
        /// <param name="bitmap">原图</param>
        /// <param name="backgroundColor">背景色</param>
        /// <param name="width">新图片宽度</param>
        /// <param name="height">新图片高度</param>
        /// <returns></returns>
        public static Bitmap ScaleToSize(this Bitmap bitmap, Color backgroundColor, int width, int height)
        {
            var scaledBitmap = new Bitmap(width, height);
            using (var g = Graphics.FromImage(scaledBitmap))
            {
                g.Clear(backgroundColor);

                var proportionalBitmap = bitmap.ScaleProportional(width, height);

                var imagePosition = new Point((int)((width - proportionalBitmap.Width) / 2m), (int)((height - proportionalBitmap.Height) / 2m));
                g.DrawImage(proportionalBitmap, imagePosition);
            }

            return scaledBitmap;
        }

        /// <summary>
        /// 控件(窗口)的截图，控件被其他窗口(而非本窗口内控件)遮挡时也可以正确截图，使用BitBlt方法
        /// </summary>
        /// <param name="control">需要被截图的控件</param>
        /// <returns>该控件的截图，控件被遮挡时也可以正确截图</returns>
        public static Bitmap CaptureControl(this Bitmap bitmap, Control control)
        {
            //调用API截屏
            IntPtr hSrce = User32.GetWindowDC(control.Handle);
            IntPtr hDest = Gdi32.CreateCompatibleDC(hSrce);
            IntPtr hBmp = Gdi32.CreateCompatibleBitmap(hSrce, control.Width, control.Height);
            IntPtr hOldBmp = Gdi32.SelectObject(hDest, hBmp);
            if (Gdi32.BitBlt(hDest, 0, 0, control.Width, control.Height, hSrce, 0, 0, (int)(CopyPixelOperation.SourceCopy | CopyPixelOperation.CaptureBlt)))
            {
                Bitmap bmp = Image.FromHbitmap(hBmp);
                Gdi32.SelectObject(hDest, hOldBmp);
                Gdi32.DeleteObject(hBmp);
                Gdi32.DeleteDC(hDest);
                User32.ReleaseDC(control.Handle, hSrce);
                bitmap = bmp;
                return bmp;
            }
            return null;
        }
    }
}