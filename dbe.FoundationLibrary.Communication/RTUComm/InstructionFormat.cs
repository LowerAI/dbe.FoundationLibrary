using dbe.FoundationLibrary.Core.Common;

using System;

namespace dbe.FoundationLibrary.Communication.RTUComm
{
    /// <summary>
    /// 指令的格式
    /// </summary>
    //[AttributeUsage(AttributeTargets.Class)]
    public class InstructionFormat //: Attribute
    {
        #region    字段 Start
        /// <summary>
        /// 唯一标识符
        /// </summary>
        public string Key;
        /// <summary>
        /// 帧头占字节数
        /// </summary>
        public int Head_Len;
        /// <summary>
        /// 指令全长占字节数
        /// </summary>
        public int TotalLength_Len;
        /// <summary>
        /// 通道号/广播地址占字节数
        /// </summary>
        public int ChNum_Len;
        /// <summary>
        /// 功能码占字节数
        /// </summary>
        public int FunCode_Len;
        /// <summary>
        /// 有效数据占字节数
        /// </summary>
        [Obsolete("由于本类暂无法与数据帧绑定故无法及时更新帧长度，也就无法算出有效数据的长度")]
        public int ValidData_Len;
        /// <summary>
        /// Crc类型
        /// </summary>
        public CrcType CrcType;
        /// <summary>
        /// Crc字节序(默认小端即DCBA)
        /// </summary>
        public Endianness CrcEndian;
        /// <summary>
        /// 校验和占字节数
        /// </summary>
        public int Crc_Len;
        /// <summary>
        /// 帧尾占字节数
        /// </summary>
        public int Tail_Len;
        #endregion 字段 End

        #region    属性 Start
        /// <summary>
        /// 通道值(每一种协议基本)
        /// </summary>
        //public int ChNum { get; set; }
        /// <summary>
        /// 功能码的偏移地址，即功能码在指令中的起始位置
        /// </summary>
        public int FunCode_Offset { get => Head_Len + TotalLength_Len + ChNum_Len; }
        /// <summary>
        /// 有效数据的偏移地址，即有效数据在指令中的起始位置
        /// </summary>
        public int ValidData_Offset { get => FunCode_Offset + FunCode_Len; }
        /// <summary>
        /// 校验和的偏移地址(ValidData_Len未算出时无效)，即校验和在指令中的起始位置
        /// </summary>
        //public int Crc_Offset { get => Head_Len + Total_Len + ChNum_Len + FunCode_Len + ValidData_Len; }
        #endregion 属性 End

        #region    公开方法 Start
        /// <summary>
        /// 返回指令数据的基准长度：就是除掉有效数据以外的长度
        /// </summary>
        /// <returns>基准长度</returns>
        public int GetBaseLength()
        {
            return Head_Len + TotalLength_Len + ChNum_Len + FunCode_Len + Crc_Len + Tail_Len;
        }

        /// <summary>
        /// 返回指令数据的最小长度，假设有效数据至少占1个字节
        /// </summary>
        /// <returns>最小长度</returns>
        public int GetMinLength()
        {
            return Head_Len + TotalLength_Len + ChNum_Len + FunCode_Len + 1 + Crc_Len + Tail_Len;
        }

        /// <summary>
        /// 返回指令数据的长度
        /// </summary>
        /// <returns>当指令内容完整时等于Total的值</returns>
        public int GetLength()
        {
            return Head_Len + TotalLength_Len + ChNum_Len + FunCode_Len + ValidData_Len + Crc_Len + Tail_Len;
        }
        #endregion 公开方法 End
    }

    /// <summary>
    /// 指令格式表
    /// </summary>
    public struct InstructionFormats
    {
        /// <summary>
        /// 第1种串口指令格式
        /// 使用该协议的上位机：wls/wvs/wlvs
        /// </summary>
        public static InstructionFormat Format1 = new InstructionFormat { Key = nameof(InstructionType.Serial1), Head_Len = 4, TotalLength_Len = 2, ChNum_Len = 0, FunCode_Len = 1, Crc_Len = 2, CrcType = CrcType.CRC16_MODBUS, CrcEndian = Endianness.DCBA, Tail_Len = 2 };
        /// <summary>
        /// 第1种HCI配置指令格式
        /// 使用该协议的上位机：wls/wvs/wlvs
        /// </summary>
        public static InstructionFormat Format2 = new InstructionFormat { Key = nameof(InstructionType.HCIConfig), Head_Len = 4, TotalLength_Len = 2, ChNum_Len = 1, FunCode_Len = 1, Crc_Len = 2, CrcType = CrcType.CRC16_MODBUS, CrcEndian = Endianness.DCBA, Tail_Len = 2 };
        /// <summary>
        /// 第1种经典蓝牙指令格式
        /// 使用该协议的上位机：wcs(v2.x)
        /// </summary>
        public static InstructionFormat Format3 = new InstructionFormat { Key = nameof(InstructionType.BT1), Head_Len = 4, TotalLength_Len = 4, ChNum_Len = 1, FunCode_Len = 1, Crc_Len = 4, CrcType = CrcType.CRC32_MPEG2, CrcEndian = Endianness.ABCD, Tail_Len = 2 };
        /// <summary>
        /// 第1种低功耗蓝牙指令格式
        /// 使用该协议的上位机：暂无
        /// </summary>
        public static InstructionFormat Format4 = new InstructionFormat { Key = nameof(InstructionType.BLE1), Head_Len = 4, TotalLength_Len = 4, ChNum_Len = 0, FunCode_Len = 1, Crc_Len = 2, CrcType = CrcType.CRC16_MODBUS, CrcEndian = Endianness.DCBA, Tail_Len = 2 };
        /// <summary>
        /// 第2种串口指令格式-avls协议，针对多通道场景
        /// 使用该协议的上位机：mclvs
        /// </summary>
        public static InstructionFormat Format5 = new InstructionFormat { Key = nameof(InstructionType.Serial1), Head_Len = 4, TotalLength_Len = 2, ChNum_Len = 1, FunCode_Len = 1, Crc_Len = 2, CrcType = CrcType.CRC16_MODBUS, CrcEndian = Endianness.DCBA, Tail_Len = 2 };
    }

    /// <summary>
    /// 指令的格式属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class InstructionFormatAttribute : Attribute
    {
        /// <summary>
        /// 名称:唯一标识符
        /// </summary>
        public InstructionType IstrtType { get; set; }
        /// <summary>
        /// 帧头长度
        /// </summary>
        public int HeadLength { get; set; }
        /// <summary>
        /// 指令全长
        /// </summary>
        public int TotalLength { get; set; }
        /// <summary>
        /// 通道号/广播地址长度
        /// </summary>
        public int ChNumLength { get; set; }
        /// <summary>
        /// 功能码长度
        /// </summary>
        public int FunCodeLength { get; set; }
        /// <summary>
        /// 有效数据的长度
        /// </summary>
        public int ValidDataLength { get; set; }
        /// <summary>
        /// Crc类型
        /// </summary>
        public CrcType CrcType { get; set; } = CrcType.CRC16_MODBUS;
        /// <summary>
        /// Crc校验值长度
        /// </summary>
        public int CrcLength { get; set; }
        /// <summary>
        /// 帧尾长度
        /// </summary>
        public int TailLength { get; set; }

        public InstructionFormatAttribute(InstructionType istrtType)
        {
            IstrtType = istrtType;
            switch (istrtType)
            {
                case InstructionType.Serial1:
                    break;
                case InstructionType.HCIConfig:
                    break;
                case InstructionType.BT1:
                    break;
                case InstructionType.BLE1:
                    break;
            }
        }


        /// <summary>
        /// 返回指令数据的基准长度：就是除掉有效数据以外的长度
        /// </summary>
        /// <returns>基准长度</returns>
        public int GetBaseLength()
        {
            return HeadLength + TotalLength + ChNumLength + FunCodeLength + CrcLength + TailLength;
        }

        /// <summary>
        /// 返回指令数据的最小长度，假设有效数据至少占1个字节
        /// </summary>
        /// <returns>最小长度</returns>
        public int GetMinLength()
        {
            return HeadLength + TotalLength + ChNumLength + FunCodeLength + 1 + CrcLength + TailLength;
        }

        /// <summary>
        /// 返回指令数据的长度
        /// </summary>
        /// <returns>当指令内容完整时等于Total的值</returns>
        public int GetLength()
        {
            return HeadLength + TotalLength + ChNumLength + FunCodeLength + ValidDataLength + CrcLength + TailLength;
        }
    }
}