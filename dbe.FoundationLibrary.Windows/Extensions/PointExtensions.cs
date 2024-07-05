using System.Drawing;
using System.Windows.Forms;

namespace dbe.FoundationLibrary.Windows.Extensions
{
    /// <summary>
    /// Point扩展类
    /// </summary>
    public static class PointExtensions
    {
        /// <summary>
        /// View坐标转换为客户区坐标
        /// </summary>
        /// <param name="control"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public static Point ViewToClient(this Point p, Control control)
        {
            return new Point(control.Width / 2 + p.X, control.Height / 2 + p.Y);
        }

        /// <summary>
        /// 客户区坐标转换为View坐标
        /// </summary>
        /// <param name="control"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public static Point ClientToView(this Point p, Control control)
        {
            return new Point(p.X - control.Width / 2, control.Height / 2 - p.Y);
        }
    }
}