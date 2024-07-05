using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace dbe.FoundationLibrary.Core.Util
{
    /// <summary>
    /// 常见文件操作
    /// </summary>
    public class FileHelper
    {
        /// <summary>
        /// 将文件读成二进制流
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <returns>二进制流</returns>
        public static byte[] ReadFileToBinary(string fileName)
        {
            byte[] bin = null;

            if (!File.Exists(fileName))
                return bin;
            try
            {
                FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                BinaryReader binaryReader = new BinaryReader(fs);
                bin = binaryReader.ReadBytes(Convert.ToInt32(fs.Length));
                fs.Close();
                binaryReader.Close();
            }
            catch (Exception e)
            {
                throw new Exception($"{e.Message}\r\n读取件失败!");
            }
            return bin;
        }

        /// <summary>
        /// 将二进制流写成文件
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="bin">二进制流</param>
        /// <returns>真假,完成写操作了没有</returns>
        public static bool WriteBinaryToFile(string fileName, byte[] bin)
        {
            if (File.Exists(fileName))
                File.Delete(fileName);
            try
            {
                FileStream fs = new FileStream(fileName, FileMode.CreateNew, FileAccess.Write);
                BinaryWriter binaryWriter = new BinaryWriter(fs);
                binaryWriter.Write(bin, 0, bin.Length);
                fs.Close();
                binaryWriter.Close();
                return true;
            }
            catch (Exception e)
            {
                throw new Exception($"{e.Message}\r\n写文件失败!");
            }
        }

        /// <summary>
        /// 拷贝文件到新路径,如果新路径已经该文件,先删除原有文件
        /// </summary>
        /// <param name="oldpath">旧路径</param>
        /// <param name="newpath">新路径</param>
        public static void CopyFile(string oldpath, string newpath)
        {
            if (!File.Exists(oldpath))
            {
                throw new Exception("要拷贝的文件不存在！");
            }

            if (File.Exists(newpath))
            {
                File.Delete(newpath);
            }
            File.Copy(oldpath, newpath);
        }

        /// <summary>
        /// 随机文件名
        /// </summary>
        /// <returns></returns>
        public static string GetRandomFilenName()
        {
            string strNum = DateTime.Now.ToLongDateString() + "_" + DateTime.Now.ToLongTimeString();
            strNum = strNum.Replace(":", "");
            strNum = strNum.Replace("-", "");
            strNum = strNum.Replace(" ", "");

            Random ran = new Random();
            int intNum = ran.Next(1, 99999);
            ran = null;
            return strNum + "_" + intNum.ToString();
        }

        /// <summary>
        /// 序列化对象后写入到文件
        /// </summary>
        /// <param name="data">需要序列化的对象</param>
        /// <param name="filePath">文件路径</param>
        public static void Serialize(object data, string filePath)
        {
            try
            {
                // 1. 打开文件
                StreamWriter fs = new StreamWriter(filePath, false);
                try
                {
                    MemoryStream streamMemory = new MemoryStream();
                    BinaryFormatter formatter = new BinaryFormatter();

                    // 2. 将对象序列化为流格式
                    formatter.Serialize(streamMemory, data);

                    // 3. 将流文件转化成字符
                    string binaryData = Convert.ToBase64String(streamMemory.GetBuffer());

                    // 4. 将数据写入文件
                    fs.Write(binaryData);
                }
                finally
                {
                    // 5. 关闭文件
                    fs.Flush();
                    fs.Close();
                }
            }
            catch
            {

            }
        }

        /// <summary>
        /// 读取文件内容后反序列化为对象
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>返回反序列化之后的对象</returns>
        public static object Deserialize(string filePath)
        {
            object data = new object();
            try
            {
                // 1. 打开文件
                StreamReader sr = new StreamReader(filePath);
                try
                {
                    MemoryStream streamMemory;
                    BinaryFormatter formatter = new BinaryFormatter();

                    // 2. 读取文件内容
                    string cipherData = sr.ReadToEnd();

                    // 3. 将文件内容转成二进制                  
                    streamMemory = new MemoryStream(Convert.FromBase64String(cipherData));

                    // 4. 反序列化                    
                    data = formatter.Deserialize(streamMemory);
                }
                catch
                {
                    // 不能序列化,则设置为null
                    data = null;
                }
                finally
                {
                    // 5. 关闭读取
                    sr.Close();
                }
            }
            catch
            {
                // 文件不存在
                data = null;
            }
            return data;
        }
    }
}