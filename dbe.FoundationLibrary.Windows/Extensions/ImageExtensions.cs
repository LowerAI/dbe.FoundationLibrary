using dbe.FoundationLibrary.Core.Extensions;

using System.Drawing;

namespace dbe.FoundationLibrary.Windows.Extensions
{
    /// <summary>
    /// Image扩展类
    /// </summary>
    public static class ImageExtensions
    {
        /// <summary>
        /// 获取原图像绕中心任意角度旋转后的图像
        /// 来源链接：C#中基于GDI+(Graphics)图像处理系列之任意角度旋转图像_米儿不可爱的博客-CSDN博客_c# 任意角度旋转 https://blog.csdn.net/weixin_41203450/article/details/118101503
        /// </summary>
        /// <param name="rawImg"></param>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static Image GetRotateImage(this Image srcImage, int angle)
        {
            angle = angle % 360;
            //原图的宽和高
            int srcWidth = srcImage.Width;
            int srcHeight = srcImage.Height;
            //图像旋转之后所占区域宽和高
            //Rectangle rotateRec = GetRotateRectangle(srcWidth, srcHeight, angle);
            //int rotateWidth = rotateRec.Width;
            //int rotateHeight = rotateRec.Height;
            var rotateSize = srcImage.Size.GetRotateSize(angle);
            //目标位图
            Bitmap destImage = null;
            Graphics graphics = null;
            try
            {
                //定义画布，宽高为图像旋转后的宽高
                //destImage = new Bitmap(rotateWidth, rotateHeight);
                destImage = new Bitmap(rotateSize.Width, rotateSize.Height);
                //graphics根据destImage创建，因此其原点此时在destImage左上角
                graphics = Graphics.FromImage(destImage);

                //要让graphics围绕某矩形中心点旋转N度，分三步
                //第一步，将graphics坐标原点移到矩形中心点,假设其中点坐标（x,y）
                //第二步，graphics旋转相应的角度(沿当前原点)
                //第三步，移回（-x,-y）
                //获取画布中心点
                Point centerPoint = new Point(rotateSize.Width / 2, rotateSize.Height / 2);
                //将graphics坐标原点移到中心点
                graphics.TranslateTransform(centerPoint.X, centerPoint.Y);
                //graphics旋转相应的角度(绕当前原点)
                graphics.RotateTransform(angle);
                //恢复graphics在水平和垂直方向的平移(沿当前原点)
                graphics.TranslateTransform(-centerPoint.X, -centerPoint.Y);
                //此时已经完成了graphics的旋转

                //计算:如果要将源图像画到画布上且中心与画布中心重合，需要的偏移量
                Point Offset = new Point((rotateSize.Width - srcWidth) / 2, (rotateSize.Height - srcHeight) / 2);
                //将源图片画到rect里（rotateRec的中心）
                graphics.DrawImage(srcImage, new Rectangle(Offset.X, Offset.Y, srcWidth, srcHeight));
                //重至绘图的所有变换
                graphics.ResetTransform();
                graphics.Save();
            }
            catch
            {
                throw;
            }
            finally
            {
                if (graphics != null)
                    graphics.Dispose();
            }
            return destImage;
        }
    }
}