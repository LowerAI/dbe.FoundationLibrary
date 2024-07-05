using dbe.FoundationLibrary.Core.Common;

using System.Drawing;

namespace dbe.FoundationLibrary.Core.Extensions
{
    /// <summary>
    /// PointF扩展类
    /// </summary>
    public static class PointFExtensions
    {
        /// <summary>
        /// 翻转<see cref="PointF">p坐标的X值
        /// </summary>
        /// <param name="p"></param>
        public static PointF FlipX(this PointF p)
        {
            p.X = -p.X;
            return p;
        }

        /// <summary>
        /// 翻转<see cref="PointF">p坐标的Y值
        /// </summary>
        /// <param name="p"></param>
        public static PointF FlipY(this PointF p)
        {
            p.Y = -p.Y;
            return p;
        }

        /// <summary>
        /// 翻转<see cref="PointF">p坐标
        /// </summary>
        /// <param name="p"></param>
        public static PointF Flip(this PointF p)
        {
            p.X = -p.X;
            p.X = -p.Y;
            return p;
        }

        /// <summary>
        /// 缩放X坐标到指定的量
        /// </summary>
        /// <param name="p"></param>
        /// <param name="x">指定的缩放量</param>
        /// <returns></returns>
        public static PointF InflateX(this PointF p, float x)
        {
            p.X += x;
            return p;
        }

        /// <summary>
        /// 缩放Y坐标到指定的量
        /// </summary>
        /// <param name="p"></param>
        /// <param name="y">指定的缩放量</param>
        /// <returns></returns>
        public static PointF InflateY(this PointF p, float y)
        {
            p.Y += y;
            return p;
        }

        /// <summary>
        /// 缩放X坐标和Y坐标到指定的量
        /// </summary>
        /// <param name="p"></param>
        /// <param name="x">X坐标指定的缩放量</param>
        /// <param name="y">Y坐标指定的缩放量</param>
        /// <returns></returns>
        public static PointF Inflate(this PointF p, float x, float y)
        {
            p.X += x;
            p.Y += y;
            return p;
        }

        /// <summary>
        /// 当前坐标点偏移指定的量
        /// </summary>
        /// <param name="p">当前坐标点</param>
        /// <param name="x">x轴指定偏移量</param>
        /// <param name="y">y轴指定偏移量</param>
        /// <returns></returns>
        public static void Offset(this PointF p, float x, float y)
        {
            //p.X += x;
            //p.Y += y;
            p = new PointF(p.X + x, p.Y + y);
        }

        public static PointF Subtract(this PointF p, float x, float y)
        {
            PointF newPoint = new PointF();
            newPoint.X = p.X - x;
            newPoint.Y = p.Y - y;
            return newPoint;
        }

        /// <summary>
        /// 返回指定半径的矩形
        /// </summary>
        /// <param name="originPoint">圆心坐标</param>
        /// <param name="r">半径</param>
        /// <returns></returns>
        private static RectangleF GetRectFByRadius(this PointF originPoint, float r)
        {
            PointF ltPoint = originPoint.Subtract(r, r);
            SizeF size = new SizeF(2 * r, 2 * r);
            RectangleF rectF = new RectangleF(ltPoint, size);
            return rectF;
        }

        /// <summary>
        /// 转换到主窗格的绘图坐标
        /// </summary>
        /// <param name="p"></param>
        /// <param name="masterRectF"></param>
        /// <param name="coordinateType"></param>
        /// <returns></returns>
        public static PointF ToDrawingCoordinate(this PointF p, RectangleF masterRectF, CoordinateType coordinateType)
        {
            PointF pRet = p;
            switch (coordinateType)
            {
                case CoordinateType.DrawingCoordinate:
                    { }
                    break;
                case CoordinateType.BitmapCoordinate:
                    pRet = new PointF(p.X - masterRectF.Width / 2, p.Y - masterRectF.Height / 2);
                    break;
                case CoordinateType.PageCoordinate:
                    pRet = new PointF(p.X - masterRectF.Left - masterRectF.Width / 2, p.Y - masterRectF.Top - masterRectF.Height / 2);
                    break;
            }
            return pRet;
        }

        /// <summary>
        /// 转换到主窗格的位图坐标
        /// </summary>
        /// <param name="p"></param>
        /// <param name="masterRectF"></param>
        /// <param name="coordinateType"></param>
        /// <returns></returns>
        public static PointF ToBitmapCoordinate(this PointF p, RectangleF masterRectF, CoordinateType coordinateType)
        {
            PointF pRet = p;
            switch (coordinateType)
            {
                case CoordinateType.DrawingCoordinate:
                    pRet = new PointF(masterRectF.Width / 2 + p.X, masterRectF.Height / 2 + p.Y);
                    break;
                case CoordinateType.BitmapCoordinate:
                    { }
                    break;
                case CoordinateType.PageCoordinate:
                    pRet = new PointF(p.X - masterRectF.Left, p.Y - masterRectF.Top);
                    break;
            }
            return pRet;
        }

        /// <summary>
        /// 转换到主窗格容器控件的页面坐标
        /// </summary>
        /// <param name="p"></param>
        /// <param name="masterRectF">当前坐标所在的矩形</param>
        /// <param name="coordinateType">转换的目标坐标系</param>
        /// <returns></returns>
        public static PointF ToPageCoordinate(this PointF p, RectangleF masterRectF, CoordinateType coordinateType)
        {
            PointF pRet = p;
            switch (coordinateType)
            {
                case CoordinateType.DrawingCoordinate:
                    pRet = new PointF(masterRectF.Left + masterRectF.Width / 2 + p.X, masterRectF.Top + masterRectF.Height / 2 + p.Y);
                    break;
                case CoordinateType.BitmapCoordinate:
                    pRet = new PointF(p.X + masterRectF.Left, p.Y + masterRectF.Top);
                    break;
                case CoordinateType.PageCoordinate:
                    { }
                    break;
            }
            return pRet;
        }
    }
}