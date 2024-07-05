using System.Drawing;
using System.Windows.Forms;

namespace dbe.FoundationLibrary.Windows.Extensions
{
    /// <summary>
    /// PointF扩展类
    /// </summary>
    public static class PointFExtensions
    {
        /// <summary>
        /// View坐标转换为客户区坐标
        /// </summary>
        /// <param name="control"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public static PointF ViewToClient(this PointF p, Control control)
        {
            return new PointF(control.Width / 2 + p.X, control.Height / 2 + p.Y);
        }

        /// <summary>
        /// 客户区坐标转换为View坐标
        /// </summary>
        /// <param name="control"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public static PointF ClientToView(this PointF p, Control control)
        {
            return new PointF(p.X - control.Width / 2, control.Height / 2 - p.Y);
        }
    }
}