using System.Drawing;

namespace dbe.FoundationLibrary.Core.Extensions
{
    /// <summary>
    /// RectangleF扩展类
    /// </summary>
    public static class RectangleFExtensions
    {
        /// <summary>
        /// 返回矩形绕中心任意角度旋转后所占区域的矩形
        /// </summary>
        /// <param name="rectF">原矩形</param>
        /// <param name="angle">顺时针旋转角度</param>
        /// <returns></returns>
        public static RectangleF GetRotateRectangle(this RectangleF rectF, float angle)
        {
            var rotateSize = rectF.Size.GetRotateSize(angle);
            return new RectangleF(0, 0, rotateSize.Width, rotateSize.Height);
        }

        /// <summary>
        /// 返回<see cref="RectangleF">缩放指定大小后的副本
        /// </summary>
        /// <param name="rectf">原始矩形</param>
        /// <param name="widthIncrement">宽度增量，正值表示放大/负值表示缩小</param>
        /// <param name="heightIncrement">高度增量，正值表示放大/负值表示缩小</param>
        /// <returns></returns>
        public static RectangleF Scale(this RectangleF rectf, float widthIncrement, float heightIncrement)
        {
            var newRect = rectf;
            newRect.Inflate(widthIncrement, heightIncrement);
            return newRect;
        }

        /// <summary>
        /// 返回改变<see cref="RectangleF">大小后的副本
        /// </summary>
        /// <param name="rect">原始矩形</param>
        /// <param name="widthIncrement">宽度增量</param>
        /// <param name="heightIncrement">高度增量</param>
        /// <returns></returns>
        public static RectangleF ChangeSize(this RectangleF rectf, float widthIncrement, float heightIncrement)
        {
            var newRect = rectf;
            newRect.Width += widthIncrement;
            newRect.Height += heightIncrement;
            return newRect;
        }

        /// <summary>
        /// 返回改变<see cref="RectangleF">大小后的副本
        /// </summary>
        /// <param name="rectf">原始矩形</param>
        /// <param name="leftIncrement">对应Padding.Left</param>
        /// <param name="topIncrement">对应Padding.Top</param>
        /// <param name="rightIncrement">对应Padding.Right</param>
        /// <param name="bottomIncrement">对应Padding.Bottom</param>
        /// <returns></returns>
        public static RectangleF ChangeSize(this RectangleF rectf, float leftIncrement, float topIncrement, float rightIncrement, float bottomIncrement)
        {
            var newRect = rectf;
            newRect.X += leftIncrement;
            newRect.Y += topIncrement;
            newRect.Width -= 2 * rightIncrement;
            newRect.Height -= 2 * bottomIncrement;
            return newRect;
        }
    }
}