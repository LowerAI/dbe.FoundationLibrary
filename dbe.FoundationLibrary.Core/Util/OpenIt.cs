using System;
using System.Diagnostics;

namespace dbe.FoundationLibrary.Core.Util
{
    /// <summary>
    /// 打开文件或目录
    /// 源连接：C#实现打开指定目录和指定文件的示例代码_C#教程_脚本之家 https://www.jb51.net/article/253525.htm
    /// </summary>
    public class OpenIt
    {
        /// <summary>
        /// 打开目录
        /// </summary>
        /// <param name="folderPath">目录路径（比如：C:\Users\Administrator\）</param>
        public static string OpenFolder(string folderPath)
        {
            var msg = string.Empty;
            if (string.IsNullOrEmpty(folderPath))
            {
                return "打开操作异常：指定目录不存在";
            }

            Process process = new Process();
            ProcessStartInfo psi = new ProcessStartInfo("Explorer.exe");
            psi.Arguments = folderPath;
            process.StartInfo = psi;

            try
            {
                process.Start();
            }
            catch (Exception ex)
            {
                msg = $"打开操作异常：{ex.Message}";
            }
            finally
            {
                process?.Close();
            }
            return msg;
        }

        /// <summary>
        /// 打开目录且选中文件
        /// </summary>
        /// <param name="filePath">文件路径（比如：C:\Users\Administrator\test.txt）</param>
        public static void OpenFolderAndSelectedFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath)) return;

            Process process = new Process();
            ProcessStartInfo psi = new ProcessStartInfo("Explorer.exe");
            psi.Arguments = "/e,/select," + filePath;
            process.StartInfo = psi;

            //process.StartInfo.UseShellExecute = true;
            try
            {
                process.Start();
            }
            catch (Exception ex)
            {
                //throw ex;
            }
            finally
            {
                process?.Close();
            }
        }

        /// <summary>
        /// 打开文件
        /// </summary>
        /// <param name="filePathOrName">文件路径或文件名（比如：C:\Users\Administrator\test.txt）</param>
        /// <param name="isWaitFileClose">是否等待文件关闭（true：表示等待）</param>
        public static string OpenFile(string filePathOrName, bool isWaitFileClose = false)
        {
            var msg = string.Empty;
            if (string.IsNullOrEmpty(filePathOrName))
            {
                return msg;
            }
            Process process = null;
            ProcessStartInfo psi = new ProcessStartInfo(filePathOrName);
            try
            {
                #region       针对目标文件已正确关联可运行的程序
                psi.UseShellExecute = true;
                psi.CreateNoWindow = true;
                process = Process.Start(psi);
                #endregion
            }
            catch
            {
                #region        针对未关联可运行的程序的txt类文档
                psi.FileName = "notepad.exe";
                psi.Arguments = filePathOrName;
                try
                {
                    process = Process.Start(psi);
                }
                #endregion
                catch
                {
                    #region        针对未关联可运行的程序的非txt类文档
                    psi.FileName = "explorer.exe";
                    psi.Arguments = filePathOrName;
                    try
                    {
                        process = Process.Start(psi);
                    }
                    #endregion
                    catch (Exception ex)
                    {
                        msg = $"打开操作异常：{ex.Message}";
                    }
                }
            }
            finally
            {
                //等待打开的程序关闭
                if (isWaitFileClose)
                {
                    process.WaitForExit();
                }
                process?.Close();
            }
            return msg;
        }
    }
}