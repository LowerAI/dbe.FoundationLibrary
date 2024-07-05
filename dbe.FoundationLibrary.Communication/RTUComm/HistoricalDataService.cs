using dbe.FoundationLibrary.Core.Util;

using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace dbe.FoundationLibrary.Communication.RTUComm
{
    /// <summary>
    /// 历史数据服务类
    /// </summary>
    public class HistoricalDataService : RTUCore
    {
        #region    字段 start
        private string filePath;
        private CsvRW csvRW;
        public ConcurrentQueue<string> fileDataQueue { get; set; } = new ConcurrentQueue<string>();
        private int elementsCountPreFrame = RTUDefault.ElementsCountPreFrame;// 每帧里面元素的数量
        private bool isFirstLine = true;// 是否第一行
        #endregion 字段 end

        #region    属性 start
        public CommunicationMode CommunicationMode => CommunicationMode.File;

        /// <summary>
        /// 历史数据文件的完整路径
        /// </summary>
        public string FilePath
        {
            get { return filePath; }
            set
            {
                Init(value);
                filePath = value;
            }
        }

        /// <summary>
        /// 数据帧中元素的数量
        /// </summary>
        public int ElementsCountPreFrame
        {
            get => elementsCountPreFrame;
            set => elementsCountPreFrame = value;
        }
        #endregion 属性 end

        #region    构造函数 start
        public HistoricalDataService(InstructionFormat instructionFormat, int elementsCount) : base(instructionFormat)
        {
            this.elementsCountPreFrame = elementsCount;
        }
        #endregion 构造函数 end

        #region    公开方法 start
        public void Init(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentException($"“{nameof(filePath)}”不能为 null 或空白。", nameof(filePath));
            }
            csvRW = new CsvRW(filePath, RWMode.Read);
            isFirstLine = true;
        }

        /// <summary>
        /// 历史数据接收(异步地)
        /// </summary>
        /// <returns></returns>
        public override Task ReceiveAdvertisementDataAsync()
        {
            return Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    if (!PauseReceiveData.SafeWaitHandle.IsClosed)
                    {
                        PauseReceiveData.WaitOne();
                    }
                    //if (stopRecv.IsCancellationRequested)
                    //{
                    //    break;
                    //}

                    //sw.Restart();
                    //Logger.Trace($"读取历史数据=>len=【{len}】");
                    try
                    {
                        if (csvRW == null)
                        {
                            continue;
                        }
                        var line = csvRW.ReadNewLine();
                        if (isFirstLine)
                        {// 默认第一行是标题行，所以不读
                            isFirstLine = false;
                            continue;
                        }
                        if (!string.IsNullOrWhiteSpace(line))
                        {
                            fileDataQueue.Enqueue(line);
                        }
                        //Debug.WriteLine($"读取历史数据=>单次接收结束");
                    }
                    catch (Exception ex)
                    {
                        string message = $"接收历史数据发生异常：{filePath}/{ex.StackTrace}";
                        logger.Fatal(message, ex);
                        //UpdateComStatus?.Invoke(false, PortName, message);
                    }
                    //sw.Stop();
                    //var timeSpan = sw.ElapsedMilliseconds;
                    //Debug.WriteLine($"读取历史数据=>本次接收耗时:{timeSpan}毫秒");
                    //await Task.Delay(1);
                }
            }, TaskCreationOptions.LongRunning);
        }

        /// <summary>
        /// 从数据队列中取出新一帧
        /// </summary>
        /// <returns>正确的单帧数据</returns>
        public string[] GetNewFrame()
        {
            var readFrame = new string[ElementsCountPreFrame];

            while (true)
            {
                if (fileDataQueue.IsEmpty)
                {
                    //Debug.WriteLine($"串口数据处理=>0-recvDataQueue.Count=【{recvDataQueue.Count}】");
                    continue;
                }

                if (fileDataQueue.TryDequeue(out string line))
                {
                    var elements = line.Split(',');
                    if (elements.Length != ElementsCountPreFrame)
                    {
                        logger.Trace($"当前行分割后的元素数量{elements.Length}不等于指定的元素数量{ElementsCountPreFrame}");
                        continue;
                    }
                    readFrame = elements;
                    break;
                }
            }
            //logger.Trace($"已解析文本数据队列余量(in HistCommunication=>GetNewFrame):{fileDataQueue.Count}");
            return readFrame;
        }

        //public override Task<bool> WriteRegisterAsync<T>(byte? chNum, byte funCode, params T[] values)
        //{
        //    return Task.FromResult(true);
        //}

        //public override Task<byte[]> WriteRegisterThenRecvAckAsync<T>(byte? chNum, byte funCode, params T[] values)
        //{
        //    return Task.FromResult(new byte[1]);
        //}

        public override Task<bool> WriteRegisterAsync(byte? chNum, byte funCode, params dynamic[] values)
        {
            return Task.FromResult(true);
        }

        public override Task<byte[]> WriteRegisterThenRecvAckAsync(byte? chNum, byte funCode, params dynamic[] values)
        {
            return Task.FromResult(new byte[1]);
        }
        #endregion 公开方法 end

        #region    私有方法 start
        #endregion 私有方法 end
    }
}