using System;
using System.Drawing;

namespace dbe.FoundationLibrary.Core.Extensions
{
    /// <summary>
    /// Size扩展类
    /// </summary>
    public static class SizeExtensions
    {
        /// <summary>
        /// 返回改变<see cref="SizeF">大小后的副本
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="heightIncrement"></param>
        /// <param name="widthIncrement"></param>
        /// <returns></returns>
        public static Size Subtract(this Size size, int widthIncrement, int heightIncrement)
        {
            var newSize = new Size(widthIncrement, heightIncrement);
            return Size.Subtract(size, newSize);
        }

        /// <summary>
        /// 计算矩形绕中心任意角度旋转后所占区域矩形的宽高
        /// 来源链接：C#中基于GDI+(Graphics)图像处理系列之任意角度旋转图像_米儿不可爱的博客-CSDN博客_c# 任意角度旋转 https://blog.csdn.net/weixin_41203450/article/details/118101503
        /// </summary>
        /// <param name="size">原矩形的宽高</param>
        /// <param name="angle">顺时针旋转角度</param>
        /// <returns></returns>
        public static Size GetRotateSize(this Size size, float angle)
        {
            double radian = angle * Math.PI / 180; ;
            double cos = Math.Cos(radian);
            double sin = Math.Sin(radian);
            //只需要考虑到第四象限和第三象限的情况取大值(中间用绝对值就可以包括第一和第二象限)
            int newWidth = (int)(Math.Max(Math.Abs(size.Width * cos - size.Height * sin), Math.Abs(size.Width * cos + size.Height * sin)));
            int newHeight = (int)(Math.Max(Math.Abs(size.Width * sin - size.Height * cos), Math.Abs(size.Width * sin + size.Height * cos)));
            return new Size(newWidth, newHeight);
        }
    }
}