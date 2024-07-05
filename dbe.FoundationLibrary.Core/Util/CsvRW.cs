using dbe.FoundationLibrary.Core.Common;
using dbe.FoundationLibrary.Core.Win32.API;

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dbe.FoundationLibrary.Core.Util
{
    /// <summary>
    /// DataSaver读写模式
    /// </summary>
    public enum RWMode : int
    {
        Read = 1,
        Write = 2
    }

    /// <summary>
    /// Csv文件读写工具类
    /// </summary>
    public sealed class CsvRW
    {
        #region    字段 start
        private LoggerUtil logger = LoggerUtil.Instance;
        private string fullPath;
        private StreamReader sr;
        private FileStream fs;
        private StreamWriter sw;
        #endregion 字段 end

        #region    属性 start
        /// <summary>
        /// 返回csv文件完整路径
        /// </summary>
        public string FullPath { get => fullPath; }

        /// <summary>
        /// 当前读写状态，初始为停止状态
        /// </summary>
        public RunningStatus CurrentStatus { get; set; } = RunningStatus.Stop;

        /// <summary>
        /// 是否暂停读取
        /// </summary>
        public bool IsPauseReading { get; set; }

        /// <summary>
        /// 是否暂停写入
        /// </summary>
        public bool IsPauseWriting { get; set; }

        /// <summary>
        /// 是否停止读取
        /// </summary>
        public bool IsStopReading { get; set; }

        /// <summary>
        /// 是否停止写入
        /// </summary>
        public bool IsStopWriting { get; set; }
        #endregion 属性 end

        #region    构造与析构 start
        public CsvRW(string filePath)
        {
            fullPath = filePath;
        }

        public CsvRW(string filePath, RWMode mode)
        {
            Init(filePath, mode);
        }

        ~CsvRW()
        {
            CloseRead();
            CloseWrite();
        }
        #endregion 构造与析构 end

        #region    特殊功能 start
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="mode"></param>
        private void Init(string filePath, RWMode mode)
        {
            fullPath = filePath;
            try
            {
                switch (mode)
                {
                    case RWMode.Read:
                        sr = new StreamReader(fullPath, Encoding.UTF8);
                        break;
                    case RWMode.Write:
                        fs = new FileStream(fullPath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
                        sw = new StreamWriter(fs, Encoding.UTF8);
                        //sw = new StreamWriter(fullPath, true, Encoding.UTF8);
                        break;
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("csv读写器构造失败", ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool FileIsUsing()
        {
            return FileIsUsing(fullPath);
        }

        /// <summary>   
        /// 检查文件是否被占用  
        /// </summary>   
        /// <param name="strfilepath">要检查的文件路径</param>          
        /// <returns>true文件已经打开,false文件可用未打开</returns>   
        public static bool FileIsUsing(string filePath)
        {
            // 先检查文件是否存在,如果不存在那就不检查了   
            if (!File.Exists(filePath))
            {
                return false;
            }
            // 打开指定文件看看情况   
            IntPtr vHandle = Kernel32._lopen(filePath, Const.OF_READWRITE | Const.OF_SHARE_DENY_NONE);
            if (vHandle == Win32Constants.HFILE_ERROR)
            { // 文件已经被打开                   
                return true;
            }
            Kernel32.CloseHandle(vHandle);
            // 说明文件没被打开，并且可用  
            return false;
        }

        /// <summary>
        /// 开始
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="filePath"></param>
        public void Start(RWMode mode, string filePath)
        {
            IsPauseWriting = false;
            IsStopWriting = false;
            Init(filePath, mode);
            CurrentStatus = RunningStatus.Running;
        }

        /// <summary>
        /// 暂停
        /// </summary>
        /// <param name="mode"></param>
        public void Pause(RWMode mode)
        {
            switch (mode)
            {
                case RWMode.Read:
                    IsPauseReading = true;
                    break;
                case RWMode.Write:
                    IsPauseWriting = true;
                    break;
            }
            CurrentStatus = RunningStatus.Pause;
        }

        /// <summary>
        /// 继续
        /// </summary>
        /// <param name="mode"></param>
        public void Continue(RWMode mode)
        {
            switch (mode)
            {
                case RWMode.Read:
                    IsPauseReading = false;
                    break;
                case RWMode.Write:
                    IsPauseWriting = false;
                    break;
            }
            CurrentStatus = RunningStatus.Running;
        }

        /// <summary>
        /// 停止
        /// </summary>
        /// <param name="mode"></param>
        public void Stop(RWMode mode)
        {
            switch (mode)
            {
                case RWMode.Read:
                    IsStopReading = true;
                    CloseRead();
                    break;
                case RWMode.Write:
                    IsStopWriting = true;
                    CloseWrite();
                    break;
            }
            CurrentStatus = RunningStatus.Stop;
        }
        #endregion 特殊功能 end

        #region    将datatable写入Csv文本文件 start
        /// <summary>
        /// 将datatable数据全部写入Csv中，按指定分隔符分割，列标题作为第一行
        /// </summary>
        /// <param name="dt">datatable数据源</param>
        /// <param name="fullPath">需要写入的Csv文件的完整路径</param>
        public void WriteCsv(DataTable dt, char chr = ',')
        {
            if (dt == null)
                throw new ArgumentNullException(nameof(dt));

            FileInfo file = new FileInfo(FullPath);
            if (!file.Directory.Exists)
            {
                file.Directory.Create();
            }
            FileStream fs = new FileStream(FullPath, FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
            List<string> dataLst = new List<string>();
            //写列名
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                //data += dt.Columns[i].ColumnName + ",";
                dataLst.Add(dt.Columns[i].ColumnName);
            }
            sw.WriteLine(string.Join(",", dataLst));

            //写数据
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dataLst.Clear();
                //data = "";
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    string str = dt.Rows[i][j].ToString();
                    str = str.Replace("\"", "\"\"");
                    if (str.Contains(',') || str.Contains('"') || str.Contains('\r') || str.Contains('\n'))
                    {
                        str = string.Format("\"{0}\"", str);
                    }
                    //data += str + chr;
                    dataLst.Add(str);
                }
                sw.WriteLine(string.Join(chr.ToString(), dataLst));
            }
            sw.Close();
            fs.Close();
        }
        #endregion 将datatable写入Csv文本文件 end

        #region    读取Csv文件到datatable start
        /// <summary>
        /// 读取Csv文件中所有数据到datatable中，第一行为列标题，其余为数据，列数以标题为主，按指定分隔符分割
        /// </summary>
        /// <param name="fullPath">Csv文件的完整路径</param>
        /// <returns>返回包含Csv数据的datatable，如果错误则返回Null</returns>
        public DataTable ReadCsv(char chr = ',')
        {
            char[] chrlist = new char[1] { chr };
            return ReadCsv(chrlist);
        }

        /// <summary>
        /// 读取Csv文件中所有数据到datatable中，第一行为列标题，其余为数据，列数以标题为主，按指定分隔符分割
        /// </summary>
        /// <param name="fullPath">Csv文件的完整路径</param>
        /// <returns>返回包含Csv数据的datatable，如果错误则返回Null</returns>
        public DataTable ReadCsv(char[] splitChars)
        {
            if (!File.Exists(FullPath.Trim())) return null;

            DataTable dt = new DataTable();
            bool isFirst = true;

            var lines = File.ReadAllLines(FullPath);

            foreach (var line in lines)
            {
                if (isFirst)
                {
                    isFirst = false;
                    string[] tableHeader = line.Split(splitChars, StringSplitOptions.None);
                    foreach (var header in tableHeader)
                    {
                        dt.Columns.Add(header);
                    }
                }
                else
                {
                    string[] data = line.Split(splitChars, StringSplitOptions.None);
                    DataRow dr = dt.NewRow();
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        dr[i] = data[i];
                    }
                    dt.Rows.Add(dr);
                }
            }
            return dt;
        }
        #endregion 读取Csv文件到datatable end

        #region    读取一行数据 start
        /// <summary>
        /// 读取Csv第一行数据，按指定分隔符分割为string数组
        /// </summary>
        /// <param name="CsvFilePath">Csv文件的完整路径</param>
        /// <param name="chrSplit">分隔符</param>
        /// <returns></returns>
        public string[] ReadFirstLine(char chrSplit = ',')
        {
            char[] chrlist = new char[1] { chrSplit };
            return ReadFirstLine(chrlist);
        }

        /// <summary>
        /// 读取Csv第一行数据，按多个指定分隔符分割为string数组
        /// </summary>
        /// <param name="fullPath">Csv文件的完整路径</param>
        /// <param name="chrSplit">分隔符列表</param>
        /// <returns></returns>
        public string[] ReadFirstLine(char[] chrSplit)
        {
            var line = ReadFirstLineString();
            return line.Split(chrSplit);
        }

        /// <summary>
        /// 读取Csv第一行数据
        /// </summary>
        /// <param name="fullPath">Csv文件的完整路径</param>
        /// <returns></returns>
        public string ReadFirstLineString()
        {
            return ReadAll(null).First();
        }

        /// <summary>
        /// 读取数据部分(因为头行通常是标题)
        /// </summary>
        /// <returns></returns>
        public string[] ReadDataParts()
        {
            var span = ReadAll(null).AsSpan();
            return span.Length > 1 ? span.Slice(1).ToArray() : new string[0];
        }

        /// <summary>
        /// 读取所有行数据
        /// </summary>
        /// <returns></returns>
        public string[] ReadAll(Encoding encoding)
        {
            if (!File.Exists(fullPath)) return null;
            encoding = encoding ?? Encoding.Default;
            return File.ReadAllLines(fullPath, encoding);
        }

        /// <summary>
        /// 返回新一行数据按指定分隔符分割后的string数组
        /// </summary>
        /// <param name="chrSplit"></param>
        /// <returns></returns>
        public string[] ReadNewLine(char chrSplit = ',')
        {
            var line = ReadNewLine();
            return line.Split(chrSplit);
        }

        /// <summary>
        /// 读取新一行
        /// </summary>
        /// <returns></returns>
        public string ReadNewLine()
        {
            if (IsPauseReading || IsStopReading)
            {
                return null;
            }
            StringBuilder sbLine = new StringBuilder();
            int charCode = 0;

            while (sr.Peek() > 0)
            {
                charCode = sr.Read();
                if (sr.EndOfStream || charCode == 10)  //发现换行符char10就返回拼接字符串
                {
                    break;
                }
                else
                {
                    if (charCode != 13)
                    {    //将一行的数据重新拼接起来
                        sbLine.Append((char)charCode);
                    }
                }
            }
            return sbLine.ToString();
        }
        #endregion 读取一行数据 end

        #region    写入字符串数据 start
        /// <summary>
        /// 追加写入一行字符串
        /// </summary>
        /// <param name="sLogInfo">写入信息</param>
        /// <param name="fullPath">Csv文件的完整路径</param>
        public void WriteCsv(string line)
        {
            if (line == null)
                throw new ArgumentNullException(nameof(line));

            string[] strlist = new string[1] { line };
            WriteCsv(strlist);
        }

        /// <summary>
        /// 写入多行字符串
        /// </summary>
        /// <param name="sLogInfo">写入信息</param>
        /// <param name="fullPath">Csv文件的完整路径</param>
        public void WriteCsv(string[] sLogInfo, FileMode fileMode = FileMode.Append, FileAccess fileAccess = FileAccess.Write, FileShare fileShare = FileShare.ReadWrite)
        {
            try
            {
                if (IsPauseWriting || IsStopWriting)
                {
                    return;
                }
                FileInfo file = new FileInfo(fullPath);
                if (!file.Directory.Exists)
                {
                    file.Directory.Create();
                }
                //FileStream fs = new FileStream(FullPath, fileMode, fileAccess, fileShare);
                //using (StreamWriter sw = new StreamWriter(fs, Encoding.UTF8))
                //using (sw = new StreamWriter(fullPath, true, Encoding.UTF8))
                {
                    sw.AutoFlush = true;
                    foreach (var item in sLogInfo)
                    {
                        sw.WriteLine(item);
                    }
                    //sw.Close();
                    //sw.Dispose();
                }
                //fs.Flush();
                //fs.Close();
                //fs.Dispose();
            }
            catch
            {
            }
        }

        /// <summary>
        /// 写入一行字符串
        /// </summary>
        /// <param name="sLogInfo">写入信息</param>
        /// <param name="fullPath">Csv文件的完整路径</param>
        public async Task WriteCsvAsync(string line)
        {
            if (line == null)
                throw new ArgumentNullException(nameof(line));

            string[] strlist = new string[1] { line };
            await WriteCsvAsync(strlist);
        }

        /// <summary>
        /// 写入多行字符串
        /// </summary>
        /// <param name="sLogInfo">写入信息</param>
        /// <param name="fullPath">Csv文件的完整路径</param>
        public async Task WriteCsvAsync(string[] sLogInfo, FileMode fileMode = FileMode.Append, FileAccess fileAccess = FileAccess.Write, FileShare fileShare = FileShare.ReadWrite)
        {
            try
            {
                if (IsPauseWriting || IsStopWriting)
                {
                    return;
                }
                FileInfo file = new FileInfo(fullPath);
                if (!file.Directory.Exists)
                {
                    file.Directory.Create();
                }
                //FileStream fs = new FileStream(FullPath, fileMode, fileAccess, fileShare);
                //using (StreamWriter sw = new StreamWriter(fs, Encoding.UTF8))
                //using (sw = new StreamWriter(fullPath, true, Encoding.UTF8))
                {
                    sw.AutoFlush = true;
                    foreach (var item in sLogInfo)
                    {
                        await sw.WriteLineAsync(item);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("写入csv报错", ex);
            }
        }
        #endregion 写入字符串数据 end

        #region    释放资源 start
        /// <summary>
        /// 释放读取器
        /// </summary>
        public void CloseRead()
        {
            if (sr != null)
            {
                if (sr.BaseStream.CanRead)
                {
                    sr.Close();
                    sr.Dispose();
                    sr = null;
                }
            }
        }

        /// <summary>
        /// 释放写入器
        /// </summary>
        public void CloseWrite()
        {
            if (sw != null)
            {
                if (sw.BaseStream.CanWrite)
                {
                    sw.Close();
                    sw.Dispose();
                    sw = null;
                }
            }
            if (fs != null)
            {
                //fs.Flush();
                //fs.Close();
                fs.Dispose();
                fs = null;
            }
        }
        #endregion 释放资源 end
    }
}