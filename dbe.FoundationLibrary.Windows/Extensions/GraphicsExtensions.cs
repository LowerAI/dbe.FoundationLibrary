using dbe.FoundationLibrary.Core.Extensions;

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace dbe.FoundationLibrary.Windows.Extensions
{
    /// <summary>
    /// Graphics类的扩展
    /// </summary>
    public static class GraphicsExtensions
    {
        /// <summary>
        /// 初始化抗锯齿
        /// </summary>
        /// <param name="g"></param>
        public static void InitAnti(this Graphics g)
        {
            g.CompositingQuality = CompositingQuality.HighQuality;// 图片呈现质量
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;// 插补模式
            g.PixelOffsetMode = PixelOffsetMode.HighQuality; //高像素偏移质量(高质量低速度)
            g.SmoothingMode = SmoothingMode.HighQuality;// 图像抗锯齿(高质量低速度)
            //g.TextContrast = 10;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;// 文字显示效果
        }

        /// <summary>
        /// 将指定绘图面的坐标系转换为原点在正中的右手坐标系并返回有效绘图区域和绘图状态
        /// </summary>
        /// <param name="g"></param>
        public static Tuple<RectangleF, GraphicsState> ToRightHandedCoordinate0(this Graphics g)
        {
            var clientRect = g.VisibleClipBounds;
            //var clientRect = g.ClipBounds;

            //Debug.WriteLine($"clientRect = {clientRect}");
            var centerX = clientRect.X + clientRect.Width / 2;
            var centerY = clientRect.Y + clientRect.Height / 2;
            //Debug.WriteLine($"centerX = {centerX},centerY = {centerY}");

            //clientRect.Offset(-centerX, -centerY);
            g.TranslateTransform(centerX, centerY, MatrixOrder.Append);// 将坐标原点移动到容器中心

            GraphicsState gs = g.Save();
            g.ScaleTransform(1, -1); // 将纵坐标轴方向变为朝上

            return Tuple.Create(clientRect, gs);
        }

        public static (RectangleF, GraphicsState) ToRightHandedCoordinate(this Graphics g, bool xMirrorFlip = false, bool yMirrorFlip = false)
        {
            var clientRect = g.VisibleClipBounds;
            //var clientRect = g.ClipBounds;

            //Debug.WriteLine($"clientRect = {clientRect}");
            var centerX = clientRect.X + clientRect.Width / 2;
            var centerY = clientRect.Y + clientRect.Height / 2;
            //Debug.WriteLine($"centerX = {centerX},centerY = {centerY}");

            //clientRect.Offset(-centerX, -centerY);
            g.TranslateTransform(centerX, centerY, MatrixOrder.Append);// 将坐标原点移动到容器中心

            var sx = xMirrorFlip ? -1 : 1;
            var sy = yMirrorFlip ? -1 : 1;
            GraphicsState gs = g.Save();
            //g.ScaleTransform(1, -1); // 将y轴方向变为朝上
            g.ScaleTransform(sx, sy); // 根据指定布尔值来决定是否镜像翻转指定的轴

            return (clientRect, gs);
        }

        /// <summary>
        /// 以指定坐标为圆心绘制指定半径的圆
        /// </summary>
        /// <param name="g"></param>
        /// <param name="originPoint">指定坐标</param>
        /// <param name="r">指定半径</param>
        public static Rectangle DrawCircle(this Graphics g, Color color, Point originPoint, int r)
        {
            Point leftUp = originPoint.Subtract(r, r);
            Size size = new Size(2 * r, 2 * r);
            Rectangle rect = new Rectangle(leftUp, size);
            g.DrawArc(new Pen(color), rect, 0F, 360F);
            return rect;
        }

        /// <summary>
        /// 以指定坐标为圆心绘制指定半径的圆
        /// </summary>
        /// <param name="g"></param>
        /// <param name="x">指定的x坐标</param>
        /// <param name="y">指定的y坐标</param>
        /// <param name="r">指定半径</param>
        public static Rectangle DrawCircle(this Graphics g, Color color, int x, int y, int r)
        {
            Point originPoint = new Point(x, y);
            return DrawCircle(g, color, originPoint, r);
        }

        /// <summary>
        /// 以指定坐标为圆心绘制指定半径的圆
        /// </summary>
        /// <param name="g"></param>
        /// <param name="originPoint">指定坐标</param>
        /// <param name="r">指定半径</param>
        public static RectangleF DrawCircle(this Graphics g, Color color, PointF originPoint, float r)
        {
            PointF leftUp = originPoint.Subtract(r, r);
            SizeF size = new SizeF(2 * r, 2 * r);
            RectangleF rectF = new RectangleF(leftUp, size);
            g.DrawArc(new Pen(color), rectF, 0F, 360F);
            return rectF;
        }

        /// <summary>
        /// 以指定坐标为圆心绘制指定半径的圆
        /// </summary>
        /// <param name="g"></param>
        /// <param name="x">指定的x坐标</param>
        /// <param name="y">指定的y坐标</param>
        /// <param name="r">指定半径</param>
        public static RectangleF DrawCircle(this Graphics g, Color color, float x, float y, float r)
        {
            PointF originPoint = new PointF(x, y);
            return DrawCircle(g, color, originPoint, r);
        }

        /// <summary>
        /// 以指定坐标为圆心用指定颜色填充指定半径的实心圆
        /// </summary>
        /// <param name="g"></param>
        /// <param name="color">指定颜色</param>
        /// <param name="originPoint">指定坐标</param>
        /// <param name="r">指定半径</param>
        public static Rectangle FillCircle(this Graphics g, Color color, Point originPoint, int r)
        {
            Point leftUp = originPoint.Subtract(r, r);
            Size size = new Size(2 * r, 2 * r);
            Rectangle rect = new Rectangle(leftUp, size);
            g.FillEllipse(new SolidBrush(color), rect);
            //g.FillPie(new SolidBrush(color), rect, 0F, 360F);
            return rect;
        }

        /// <summary>
        /// 以指定坐标为圆心用指定颜色填充指定半径的实心圆
        /// </summary>
        /// <param name="g"></param>
        /// <param name="color">指定颜色</param>
        /// <param name="x">指定x坐标</param>
        /// <param name="y">指定y坐标</param>
        /// <param name="r">指定半径</param>
        public static Rectangle FillCircle(this Graphics g, Color color, int x, int y, int r)
        {
            Point originPoint = new Point(x, y);
            return FillCircle(g, color, originPoint, r);
        }

        /// <summary>
        /// 以指定坐标为圆心用指定颜色填充指定半径的实心圆
        /// </summary>
        /// <param name="g"></param>
        /// <param name="color">指定颜色</param>
        /// <param name="originPoint">指定坐标</param>
        /// <param name="r">指定半径</param>
        public static RectangleF FillCircle(this Graphics g, Color color, PointF originPoint, float r)
        {
            PointF ltPoint = originPoint.Subtract(r, r);
            SizeF size = new SizeF(2 * r, 2 * r);
            RectangleF rectF = new RectangleF(ltPoint, size);
            g.FillEllipse(new SolidBrush(color), rectF);
            //g.FillPie(new SolidBrush(color), Rectangle.Truncate(rectF), 0F, 360F);
            return rectF;
        }

        /// <summary>
        /// 以指定坐标为圆心用指定颜色填充指定半径的实心圆
        /// </summary>
        /// <param name="g"></param>
        /// <param name="color">指定颜色</param>
        /// <param name="x">指定x坐标</param>
        /// <param name="y">指定y坐标</param>
        /// <param name="r">指定半径</param>
        public static RectangleF FillCircle(this Graphics g, Color color, float x, float y, float r)
        {
            PointF originPoint = new PointF(x, y);
            return FillCircle(g, color, originPoint, r);
        }

        /// <summary>
        /// 以指定颜色绘制指定范围的矩形
        /// </summary>
        /// <param name="g"></param>
        /// <param name="color">指定颜色</param>
        /// <param name="rectF">指定范围</param>
        public static void DrawRectangleF(this Graphics g, Color color, RectangleF rectF)
        {
            g.DrawRectangle(new Pen(color), rectF.X, rectF.Y, rectF.Width, rectF.Height);
        }
    }
}