using System.Drawing;
using System.IO;

namespace dbe.FoundationLibrary.Windows.Extensions
{
    /// <summary>
    /// Bytes数组的扩展类
    /// </summary>
    public static class ByteArrayExtension
    {
        /// <summary>
        /// 转换为图像
        /// </summary>
        /// <param name="source">字节流</param>
        /// <returns>图像</returns>
        public static Image ToImage(this byte[] source)
        {
            MemoryStream ms = new MemoryStream(source);
            Image image = Image.FromStream(ms);
            return image;
        }
    }
}
