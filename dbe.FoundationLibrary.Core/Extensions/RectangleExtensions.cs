using System.Drawing;

namespace dbe.FoundationLibrary.Core.Extensions
{
    /// <summary>
    /// Rectangle扩展类
    /// </summary>
    public static class RectangleExtensions
    {
        /// <summary>
        /// 返回改变矩形大小后的副本
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="widthIncrement"></param>
        /// <param name="heightIncrement"></param>
        /// <returns></returns>
        public static Rectangle SizeIncrease(this Rectangle rect, int widthIncrement, int heightIncrement)
        {
            var newRect = rect;
            newRect.Width += widthIncrement;
            newRect.Height += heightIncrement;
            return newRect;
        }

        /// <summary>
        /// 返回矩形绕中心任意角度旋转后所占区域的矩形
        /// </summary>
        /// <param name="rect">原矩形</param>
        /// <param name="angle">顺时针旋转角度</param>
        /// <returns></returns>
        public static Rectangle GetRotateRectangle(this Rectangle rect, float angle)
        {
            var rotateSize = rect.Size.GetRotateSize(angle);
            return new Rectangle(0, 0, rotateSize.Width, rotateSize.Height);
        }
    }
}