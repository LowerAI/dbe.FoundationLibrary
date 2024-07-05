using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace dbe.FoundationLibrary.Core.Extensions
{
    public static class ObjectExtension
    {
        /// <summary>
        /// 序列化对象-把当前对象转换为byte数组
        /// </summary>
        /// <param name="data">被序列化的对象 object </param>
        /// <returns>返回byte[]</returns>
        public static byte[] Serialize(this object data)
        {
            byte[] ret = null;
            try
            {
                MemoryStream streamMemory = new MemoryStream();
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(streamMemory, data);
                ret = streamMemory.GetBuffer();
            }
            catch
            {
                ret = null;
            }
            return ret;
        }
    }
}