using dbe.FoundationLibrary.Core.Extensions;
using dbe.FoundationLibrary.Core.Util;

using System;
using System.Collections;
using System.Collections.Generic;

namespace dbe.FoundationLibrary.Communication.RTUComm
{
    public class Instruction : IEnumerable<byte>
    {
        #region    变量 start
        private InstructionFormat format;
        private int dataLen;
        private List<byte> validData = new List<byte>();
        /// <summary>
        /// 指令的完整内容
        /// </summary>
        private List<byte> content;
        /// <summary>
        /// 校验值
        /// </summary>
        private byte[] crc;
        #endregion 变量 end

        #region    属性 start
        /// <summary>
        /// 获取指令格式
        /// </summary>
        public InstructionFormat Format { get => format; }
        /// <summary>
        /// 获取或设置帧头
        /// </summary>
        public byte[] Head { get; set; } = new byte[] { };
        /// <summary>
        /// 获取(或设置)指令字长：指令内容的总长度
        /// </summary>
        public int TotalLength { get => dataLen; private set => dataLen = value; }
        /// <summary>
        /// 获取或设置通道号或广播地址
        /// </summary>
        public byte[] ChNum { get; set; } = new byte[] { };
        /// <summary>
        /// 获取或设置功能码
        /// </summary>
        public byte[] FunCode { get; set; } = new byte[] { };
        /// <summary>
        /// 获取或设置有效数据
        /// </summary>
        public List<byte> ValidData
        {
            get => validData;
            set
            {
                validData = value;
                format.ValidData_Len = validData.Count;
                dataLen = format.GetBaseLength() + ValidData.Count;
            }
        }
        /// <summary>
        /// 获取校验值
        /// </summary>
        public byte[] Crc { get => crc; private set => crc = value; }
        /// <summary>
        /// 获取或设置帧尾
        /// </summary>
        public byte[] Tail { get; set; } = new byte[] { };
        /// <summary>
        /// 获取指令的完整内容(List形式)
        /// </summary>
        public List<byte> Content { get => MakeContent(); }
        /// <summary>
        /// 获取指令的完整内容(数组形式)
        /// </summary>
        public byte[] Data { get => Content.ToArray(); }
        /// <summary>
        /// 获取指令数据的16进制字符串形式
        /// </summary>
        public string HexStr { get => Data.ToHexString(); }
        #endregion 属性 end

        #region    构造与析构 start
        public Instruction(InstructionFormat format)
        {
            this.format = format;
        }
        #endregion 构造与析构 end

        #region    公开方法 start
        /// <summary>
        /// 生成完整的指令内容
        /// </summary>
        private List<byte> MakeContent()
        {
            try
            {
                content = new List<byte>();
                content.AddRange(Head);

                var bufLen = BitConverter.GetBytes(dataLen);
                var index = bufLen.Length - format.TotalLength_Len;
                if (index > 0)
                {// 如果格式规定的指令全长小于默认的int=>byte[]的长度，比如格式规定的指令全长为2，而dataLen=2=>byte[]的长度为4即0x00000002
                    bufLen = bufLen.Subbytes(index, format.TotalLength_Len);
                }
                else if (index < 0)
                {
                    bufLen = bufLen.ToFixedLengthByteArray(format.TotalLength_Len);
                }
                content.AddRange(bufLen);

                content.AddRange(ChNum);
                content.AddRange(FunCode);
                content.AddRange(ValidData);
                crc = content.ToArray().GetCRC(format.CrcType, format.CrcEndian);
                content.AddRange(crc);
                content.AddRange(Tail);

                if (!VerifyDataFormat())
                {
                    throw new ArgumentException("数据块各节的长度与指令格式规定的长度不匹配！");
                }
            }
            catch
            {
                throw;
            }
            return content;
        }

        /// <summary>
        /// 校验数据格式
        /// </summary>
        /// <returns></returns>
        public bool VerifyDataFormat()
        {
            var success = false;
            success = Head.Length == format.Head_Len;
            success = success && (TotalLength == format.TotalLength_Len);
            success = success && (ChNum.Length == format.ChNum_Len);
            success = success && (FunCode.Length == format.FunCode_Len);
            success = success && (ValidData.Count == format.ValidData_Len);
            success = success && (Crc.Length == format.Crc_Len);
            success = success && (Tail.Length == format.Tail_Len);
            return success;
        }

        /// <summary>
        /// 将指定的单个值转换为byte[]之后并入有效数据
        /// </summary>
        /// <typeparam name="T">单个值的类型</typeparam>
        /// <param name="val">指定的单个值</param>
        /// <returns>true表示并入成功/false表示并入失败</returns>
        public void AddToValidData<Dynamic>(Dynamic val)
        {
            try
            {
                byte[] buf = ConvertUtil.ToByteArray(val);
                ValidData.AddRange(buf);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 返回指定了指令格式的源数组的指令形式
        /// </summary>
        /// <param name="data">源数组</param>
        /// <param name="format">指令格式</param>
        /// <returns>指令</returns>
        /// <exception cref="ArgumentException"></exception>
        public static Instruction FromByteArray(byte[] data, InstructionFormat format)
        {
            if (data == null)
            {
                throw new ArgumentException(nameof(data));
            }

            var itrt = new Instruction(format);
            try
            {
                int headOffset = 0;
                itrt.Head = data.Subbytes(headOffset, format.Head_Len);
                int totalLengthOffset = format.Head_Len;
                itrt.TotalLength = data.Subbytes(totalLengthOffset, format.TotalLength_Len).ToInt();
                int chNumOffset = totalLengthOffset + format.TotalLength_Len;
                itrt.ChNum = data.Subbytes(chNumOffset, format.ChNum_Len);
                int funCodeOffset = chNumOffset + format.ChNum_Len;
                itrt.FunCode = data.Subbytes(funCodeOffset, format.FunCode_Len);

                int validDataOffset = funCodeOffset + format.FunCode_Len;// 有效数据起始索引
                int validDataLen = data.Length - validDataOffset - format.Crc_Len - format.Tail_Len;
                itrt.ValidData = new List<byte>(data.Subbytes(validDataOffset, validDataLen));

                int tailOffset = -1;
                itrt.Tail = data.Subbytes(tailOffset, format.Tail_Len);
                int crcOffset = tailOffset - format.Tail_Len;
                itrt.Crc = data.Subbytes(crcOffset, format.Crc_Len);
            }
            catch
            {
                throw;
            }
            return itrt;
        }

        public IEnumerator<byte> GetEnumerator()
        {
            throw new NotImplementedException();
        }
        #endregion 公开方法 end

        #region    私有方法 start
        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
        #endregion 私有方法 end
    }
}