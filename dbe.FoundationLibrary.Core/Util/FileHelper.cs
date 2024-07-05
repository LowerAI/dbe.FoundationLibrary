using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace dbe.FoundationLibrary.Core.Util
{
    /// <summary>
    /// �����ļ�����
    /// </summary>
    public class FileHelper
    {
        /// <summary>
        /// ���ļ����ɶ�������
        /// </summary>
        /// <param name="fileName">�ļ���</param>
        /// <returns>��������</returns>
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
                throw new Exception($"{e.Message}\r\n��ȡ��ʧ��!");
            }
            return bin;
        }

        /// <summary>
        /// ����������д���ļ�
        /// </summary>
        /// <param name="fileName">�ļ���</param>
        /// <param name="bin">��������</param>
        /// <returns>���,���д������û��</returns>
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
                throw new Exception($"{e.Message}\r\nд�ļ�ʧ��!");
            }
        }

        /// <summary>
        /// �����ļ�����·��,�����·���Ѿ����ļ�,��ɾ��ԭ���ļ�
        /// </summary>
        /// <param name="oldpath">��·��</param>
        /// <param name="newpath">��·��</param>
        public static void CopyFile(string oldpath, string newpath)
        {
            if (!File.Exists(oldpath))
            {
                throw new Exception("Ҫ�������ļ������ڣ�");
            }

            if (File.Exists(newpath))
            {
                File.Delete(newpath);
            }
            File.Copy(oldpath, newpath);
        }

        /// <summary>
        /// ����ļ���
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
        /// ���л������д�뵽�ļ�
        /// </summary>
        /// <param name="data">��Ҫ���л��Ķ���</param>
        /// <param name="filePath">�ļ�·��</param>
        public static void Serialize(object data, string filePath)
        {
            try
            {
                // 1. ���ļ�
                StreamWriter fs = new StreamWriter(filePath, false);
                try
                {
                    MemoryStream streamMemory = new MemoryStream();
                    BinaryFormatter formatter = new BinaryFormatter();

                    // 2. ���������л�Ϊ����ʽ
                    formatter.Serialize(streamMemory, data);

                    // 3. �����ļ�ת�����ַ�
                    string binaryData = Convert.ToBase64String(streamMemory.GetBuffer());

                    // 4. ������д���ļ�
                    fs.Write(binaryData);
                }
                finally
                {
                    // 5. �ر��ļ�
                    fs.Flush();
                    fs.Close();
                }
            }
            catch
            {

            }
        }

        /// <summary>
        /// ��ȡ�ļ����ݺ����л�Ϊ����
        /// </summary>
        /// <param name="filePath">�ļ�·��</param>
        /// <returns>���ط����л�֮��Ķ���</returns>
        public static object Deserialize(string filePath)
        {
            object data = new object();
            try
            {
                // 1. ���ļ�
                StreamReader sr = new StreamReader(filePath);
                try
                {
                    MemoryStream streamMemory;
                    BinaryFormatter formatter = new BinaryFormatter();

                    // 2. ��ȡ�ļ�����
                    string cipherData = sr.ReadToEnd();

                    // 3. ���ļ�����ת�ɶ�����                  
                    streamMemory = new MemoryStream(Convert.FromBase64String(cipherData));

                    // 4. �����л�                    
                    data = formatter.Deserialize(streamMemory);
                }
                catch
                {
                    // �������л�,������Ϊnull
                    data = null;
                }
                finally
                {
                    // 5. �رն�ȡ
                    sr.Close();
                }
            }
            catch
            {
                // �ļ�������
                data = null;
            }
            return data;
        }
    }
}