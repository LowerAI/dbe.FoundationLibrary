using System.Drawing;

namespace dbe.FoundationLibrary.Core.Extensions
{
    /// <summary>
    /// Point扩展类
    /// </summary>
    public static class PointExtensions
    {
        /// <summary>
        /// 翻转<see cref="Point">p坐标的X值
        /// </summary>
        /// <param name="p"></param>
        public static Point FlipX(this Point p)
        {
            p.X = -p.X;
            return p;
        }

        /// <summary>
        /// 翻转<see cref="Point">p坐标的Y值
        /// </summary>
        /// <param name="p"></param>
        public static Point FlipY(this Point p)
        {
            p.Y = -p.Y;
            return p;
        }

        /// <summary>
        /// 翻转<see cref="Point">p坐标
        /// </summary>
        /// <param name="p"></param>
        public static Point Flip(this Point p)
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
        public static Point InflateX(this Point p, int x)
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
        public static Point InflateY(this Point p, int y)
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
        public static Point Inflate(this Point p, int x, int y)
        {
            p.X += x;
            p.Y += y;
            return p;
        }

        /// <summary>
        /// 返回当前坐标点减去指定偏移量后的副本
        /// </summary>
        /// <param name="p">当前坐标点</param>
        /// <param name="x">x轴指定偏移量</param>
        /// <param name="y">y轴指定偏移量</param>
        /// <returns></returns>
        public static Point Subtract(this Point p, int x, int y)
        {
            Point newPoint = new Point();
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
        private static Rectangle GetRectByRadius(this Point originPoint, int r)
        {
            Point ltPoint = originPoint.Subtract(r, r);
            Size size = new Size(2 * r, 2 * r);
            Rectangle rect = new Rectangle(ltPoint, size);
            return rect;
        }
    }
}