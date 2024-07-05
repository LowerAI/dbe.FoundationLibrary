/*************************************
 * 描    述：
 * 作    者：LianChao
 * 创建时间：2023年2月22日 13点04分
 * 版    本：
 * Copyright © Shanghai GND eTech Co., Ltd. All Rights Reserved.
 * **********************************/
using GNDView.GND_para;
using GNDView.WirelessWafer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GNDView.CommunicationProtocol
{
    class WirelessWaferCommunicationProtocol_TLV : ICommunicationProtocol
    {
        /// <summary>
        /// 功能码枚举
        /// </summary>
        public enum FunctionCode
        {
            /// <summary>
            /// 读取实时温度
            /// </summary>
            QUERY_REAL_TIME_TEMPERATURE = 0xFF,
            /// <summary>
            /// 读取历史存储温度
            /// </summary>
            QUERY_HISTORY_TEMPERATURE = 0x01,
            /// <summary>
            /// 查询存储的温度信息
            /// </summary>
            QUERY_STORAGE_INFO = 0x02,
            /// <summary>
            /// 开始存储温度数据
            /// </summary>
            START_STORAGE = 0x10,
            /// <summary>
            /// 立即结束存储
            /// </summary>
            STOP_STORAGE = 0X11,
            /// <summary>
            /// 晶圆关机
            /// </summary>
            POWER_OFF_WAFER = 0X12,
        }

        /// <summary>
        /// 报文的长度
        /// </summary>
        public static class TelegramLength
        {
            /// <summary>
            /// 查询报文的长度
            /// </summary>
            public static UInt16 Query = 32;
            /// <summary>
            /// 配置报文的长度
            /// </summary>
            public static UInt16 Config = 192;
            /// <summary>
            /// 接收报文的长度
            /// </summary>
            public static UInt16 Receive = 192;
        }

        /// <summary>
        /// 在buf中占用的字节数
        /// </summary>
        public class BufBytes
        {
            //帧头占用的字节数
            public const UInt16 HEAD_BYTES = 4;
            //TLV长度占用的字节数
            public const UInt16 TLV_LENGTH_BYTES = 2;
            //功能码占用的字节数
            public const UInt16 FUN_CODE_BYTES = 1;
            //蓝牙名称占用的字节数
            public const UInt16 BLE_NAME_BYTES = 16;
            //系统时间
            public const UInt16 SYSTEM_TIME_BYTES = 6;
            //分成几段采集
            public const UInt16 STORAGE_SECTION_COUNT_BYTES = 1;
            //延时多久后开始采集
            public const UInt16 START_STORAGE_AFTER_TIME_BYTES = 2;
            //每一段存储总时长
            public const UInt16 SECTION_STORAGE_TOTAL_TIME_BYTES = 2;
            //采集时间间隔
            public const UInt16 STORAGE_INTERVAL_BYTES = 2;



            //crc校验占用的字节数
            public const UInt16 CRC_BYTES = 2;
            //帧尾占用的字节数
            public const UInt16 TAIL_BYTES = 2;

            //电池电量
            public const UInt16 BATTERY_BYTES = 1;
            //是否在充电
            public const UInt16 CHARGING_BYTES = 1;


            //上一次设置的温度存储总时长
            public const UInt16 LAST_TIME_SET_STORAGE_TOTAL_TIME = 2;

            //配置相关
        }

        /// <summary>
        /// 共用的索引
        /// </summary>
        public class CommonBufIndex
        {
            //协议头
            public static UInt16 Head = 0;
            //tlv协议长度
            public static UInt16 TlvLength = (UInt16)(Head + BufBytes.HEAD_BYTES);
            //功能码    
            public static UInt16 FunCode = (UInt16)(TlvLength + BufBytes.TLV_LENGTH_BYTES);
            //sn
            public static UInt16 BleName = (UInt16)(FunCode + BufBytes.FUN_CODE_BYTES);

        }

        /// <summary>
        /// 查询协议中各个参数在buf中的索引
        /// </summary>
        public class QueryBufIndex : CommonBufIndex
        {
            //存储的第几次温度
            public static UInt16 StorageIndex = 24;
            //crc    
            public static UInt16 Crc = (UInt16)(TelegramLength.Query - BufBytes.TAIL_BYTES - BufBytes.CRC_BYTES);
            //协议尾    
            public static UInt16 Tail = (UInt16)(Crc + BufBytes.CRC_BYTES);
        }

        /// <summary>
        /// 配置协议中各个参数在buf中的索引
        /// </summary>
        public class ConfigBufIndex : CommonBufIndex
        {
            //系统时间 
            public static UInt16 SystemTime = (UInt16)(BleName + BufBytes.BLE_NAME_BYTES);

            //温度分成几段存储
            public static UInt16 StorageSectionCount = (UInt16)(SystemTime + BufBytes.SYSTEM_TIME_BYTES);

            //第1段延时多少时间后开始存储
            public static UInt16 FirstSectionStartStorageAfterTime = (UInt16)(StorageSectionCount + BufBytes.STORAGE_SECTION_COUNT_BYTES);

            //第1段存储总时长
            public static UInt16 FirstStorageSectionTotalTime = (UInt16)(FirstSectionStartStorageAfterTime + BufBytes.START_STORAGE_AFTER_TIME_BYTES);

            //第1段存储时间间隔
            public static UInt16 FirstStorageSectionInterval = (UInt16)(FirstStorageSectionTotalTime + BufBytes.SECTION_STORAGE_TOTAL_TIME_BYTES);

            //crc    
            public static UInt16 Crc = (UInt16)(TelegramLength.Config - BufBytes.TAIL_BYTES - BufBytes.CRC_BYTES);

            //协议尾    
            public static UInt16 Tail = (UInt16)(TelegramLength.Config - BufBytes.TAIL_BYTES);
        }

        /// <summary>
        /// 接收实时和历史温度协议参数在buf中的索引
        /// </summary>
        public class ReceiveRealTimeAndHistoryTemperatureBufIndex : CommonBufIndex
        {
            /// <summary>
            /// 1. 用户上一次配置的时间
            /// </summary>
            public static UInt16 LastConfigDateTime = 23;

            /// <summary>
            /// 2. EEPROM一共需要存储多少时间的温度
            /// </summary>
            public static UInt16 TotalNeedStorageTime = 29;

            /// <summary>
            /// 3. EEPROM里面一共存储了多少时间的温度数据
            /// </summary>
            public static UInt16 TotalStorageTimeInEEPROM = 32;

            /// <summary>
            /// 4. EEPORM一共需要存储多少次温度数据
            /// </summary>
            public static UInt16 TotalNeedStorageTimes = 51;

            /// <summary>
            /// 5. EEPROM里面一共存储了多少次的温度数据
            /// </summary>
            public static UInt16 TotalStorageTimesInEEPROM = 35;

            /// <summary>
            /// 6. 一共分成几段进行存储
            /// </summary>
            public static UInt16 TotalSectionCount = 37;

            /// <summary>
            /// 7.当前已存储多少段温度
            /// </summary>
            public static UInt16 CurrentSectionIndex = 38;

            /// <summary>
            /// 8. 当前正在存储/返回总存储的第几次的温度
            /// </summary>
            public static UInt16 CurrentTemperatureIndex = 39;

            /// <summary>
            /// 9. 当前正在存储段/返回段需要存储的总时长
            /// </summary>
            public static UInt16 CurrentSectionNeedStorageTotalTime = 41;

            /// <summary>
            /// 10. 当前正在存储段/返回段需要存储的总次数
            /// </summary>
            public static UInt16 CurrentSectionNeedStorageTotalCount = 43;

            /// <summary>
            /// 11. 当前正在存储段/返回段的时间间隔
            /// </summary>
            public static UInt16 CurrentSectionInterval = 45;

            /// 12. <summary>
            /// 当前段已存储多长时间的温度数据
            /// </summary>
            public static UInt16 CurrentSectionStorageTime = 47;

            /// <summary>
            /// 13. 当前段已经存储了多少次的温度数据
            /// </summary>
            public static UInt16 CurrentSectionStorageCount = 49;

            ///// <summary>
            ///// 14. 当前段还有多长时间的温度数据还没有存储
            ///// </summary>
            //public static UInt16 CurrentSectionStorageTimeLeft = 51;

            ///// <summary>
            ///// 15. 当前段还有多少次的温度数据还没有存储
            ///// </summary>
            //public static UInt16 CurrentSectionStorageCountLeft = 53;

            /// <summary>
            /// 16. 多久之后会开始存储下一次温度
            /// </summary>
            public static UInt16 StartStorageNextSectionAfterTime = 55;

            /// <summary>
            /// 17. 存储任务总时长（每一段的存储时间加上延时时间加起来的总时间）
            /// </summary>
            public static UInt16 TaskNeedStorageTime = 67;

            /// <summary>
            /// 18. 存储任务已经运行了多少时间（每一段的存储时间加上延时时间加起来的总时间）
            /// </summary>
            public static UInt16 TaskAlreadyRuningTime = 70;


            /// <summary>
            /// W1. 圆晶电池剩余电量
            /// </summary>
            public static UInt16 Battery = 57;

            /// <summary>
            /// W2. 晶圆的通道数
            /// </summary>
            public static UInt16 ChannelTotalCount = 58;

            /// <summary>
            /// W3. 实时温度采集的总次数（采集一次加一）
            /// </summary>
            public static UInt16 NewestCollectCount = 59;

            /// <summary>
            /// W4. 当前距离最新的温度采集经过了多少ms
            /// </summary>
            public static UInt16 NewestCollectElapsedMs = 61;

            /// <summary>
            /// W5. 圆晶是否在充电
            /// </summary>
            public static UInt16 Charging = 63;

            /// <summary>
            /// W7. 任务运行状态
            /// </summary>
            public static UInt16 TaskRuningStatus = 64;

            /// <summary>
            /// W8. 蓝牙关闭倒计时
            /// </summary>
            public static UInt16 BleOffRemainingTime = 65;

            /// <summary>
            /// W9. 关机倒计时
            /// </summary>
            public static UInt16 PowerOffRemainingTime = 73;

            /// <summary>
            /// W6.通道1~16温度
            /// </summary>
            public static UInt16 Temperature = 79;


            //crc    
            public static UInt16 Crc = (UInt16)(TelegramLength.Receive - BufBytes.TAIL_BYTES - BufBytes.CRC_BYTES);

            //协议尾    
            public static UInt16 Tail = (UInt16)(TelegramLength.Receive - BufBytes.TAIL_BYTES);
        }

        /// <summary>
        /// 发送的buf数组
        /// </summary>
        public class SendBufArray
        {
            /// <summary>
            /// 读取实时温度发送的buf
            /// </summary>
            public static byte[] QueryRealTimeTemperatureBuf = new byte[TelegramLength.Query];
            /// <summary>
            /// 读取存储信息发送的buf
            /// </summary>
            public static byte[] QueryStorageInfoBuf = new byte[TelegramLength.Query];
            /// <summary>
            /// 读取历史温度发送的buf
            /// </summary>
            public static byte[] QueryHistoryTemperatureBuf = new byte[TelegramLength.Query];
            /// <summary>
            /// 设置晶圆关机发送的buf
            /// </summary>
            public static byte[] ConfigWaferPowerOffBuf = new byte[TelegramLength.Config];
            /// <summary>
            /// 设置开始存储的buf
            /// </summary>
            public static byte[] ConfigStartStorageBuf = new byte[TelegramLength.Config];
            /// <summary>
            /// 设置停止存储的buf
            /// </summary>
            public static byte[] ConfigStopStorageBuf = new byte[TelegramLength.Config];
        }

        /// <summary>
        /// 需要发送的内容
        /// </summary>
        public class BufNeedSend
        {
            /// <summary>
            /// 查询存储信息
            /// </summary>
            public static bool QueryStorageInfo = false;
            /// <summary>
            /// 读取历史温度
            /// </summary>
            public static bool QureyHistoryTemperature = false;
            /// <summary>
            /// 无线晶圆关机
            /// </summary>
            public static bool WirelessWaferPowerOff = false;
            /// <summary>
            /// 开始存储数据
            /// </summary>
            public static bool StartStorage = false;
            /// <summary>
            /// 停止存储数据
            /// </summary>
            public static bool StopStorage = false;
        }

        /// <summary>
        /// 发送的字符串
        /// </summary>
        public class SendBufString
        {
            /// <summary>
            /// 读取实时温度
            /// </summary>
            public static string QueryRealTimeTemperature
            {
                get
                {
                    return ReturnTelegram_QueryRealTimeTemperature();
                }
            }
            /// <summary>
            /// 查询存储信息
            /// </summary>
            public static string QueryStorageInfo
            {
                get
                {
                    return ReturnTelegram_QueryStorageInfo();
                }
            }
            /// <summary>
            /// 查询历史温度
            /// </summary>
            public static string QueryHistoryTemperautre
            {
                get
                {
                    return ReturnTelegram_QueryHistoryTemperature();
                }
            }
            /// <summary>
            /// 晶圆关机
            /// </summary>
            public static string WaferPowerOff
            {
                get
                {
                    return ReturnTelegram_WaferPowerOff();
                }
            }
            /// <summary>
            /// 开始存储
            /// </summary>
            public static string StartStorage
            {
                get
                {
                    return ReturnTelegram_StartStorage(WirelessWaferPara.StorageInfoSet);
                }
            }
            /// <summary>
            /// 开始存储
            /// </summary>
            public static string StopStorage
            {
                get
                {
                    return ReturnTelegram_StopStorage();
                }
            }
        }

        /// <summary>
        /// 返回“查询实时温度”报文
        /// </summary>
        /// <returns></returns>
        private static string ReturnTelegram_QueryRealTimeTemperature()
        {
            //头
            for (int i = 0; i < protocolHead.Length; i++)
            {
                SendBufArray.QueryRealTimeTemperatureBuf[QueryBufIndex.Head + i] = protocolHead[i];
            }

            //一帧的长度
            GNDCommon.WriteUint16ToArray((UInt16)SendBufArray.QueryRealTimeTemperatureBuf.Length,
                    ref SendBufArray.QueryRealTimeTemperatureBuf,
                    QueryBufIndex.TlvLength);

            //功能码
            SendBufArray.QueryRealTimeTemperatureBuf[QueryBufIndex.FunCode] = (byte)FunctionCode.QUERY_REAL_TIME_TEMPERATURE;

            //SN
            for (int i = 0; i < WirelessWaferPara.WaferSNByteArray.Count(); i++)
            {
                SendBufArray.QueryRealTimeTemperatureBuf[QueryBufIndex.BleName + i] = WirelessWaferPara.WaferSNByteArray[i];
            }
            //crc
            CRC.calCrc(ref SendBufArray.QueryRealTimeTemperatureBuf, SendBufArray.QueryRealTimeTemperatureBuf.Length - 4);

            //尾
            for (int i = 0; i < protocolTail.Length; i++)
            {
                SendBufArray.QueryRealTimeTemperatureBuf[QueryBufIndex.Tail + i] = protocolTail[i];
            }

            //将buf转为字符串
            StringBuilder hex = new StringBuilder();
            foreach (byte b in SendBufArray.QueryRealTimeTemperatureBuf)
            {
                hex.Append(b.ToString("X2")).Append(" ");
            }
            return hex.ToString();
        }

        /// <summary>
        /// 返回“查询存储信息”报文
        /// </summary>
        /// <returns></returns>
        private static string ReturnTelegram_QueryStorageInfo()
        {
            //头
            for (int i = 0; i < protocolHead.Length; i++)
            {
                SendBufArray.QueryStorageInfoBuf[QueryBufIndex.Head + i] = protocolHead[i];
            }

            //一帧的长度
            GNDCommon.WriteUint16ToArray((UInt16)SendBufArray.QueryStorageInfoBuf.Length,
                    ref SendBufArray.QueryStorageInfoBuf,
                    QueryBufIndex.TlvLength);

            //功能码
            SendBufArray.QueryStorageInfoBuf[QueryBufIndex.FunCode] = (byte)FunctionCode.QUERY_STORAGE_INFO;

            //SN
            for (int i = 0; i < WirelessWaferPara.WaferSNByteArray.Count(); i++)
            {
                SendBufArray.QueryStorageInfoBuf[QueryBufIndex.BleName + i] = WirelessWaferPara.WaferSNByteArray[i];
            }
            //crc
            CRC.calCrc(ref SendBufArray.QueryStorageInfoBuf, SendBufArray.QueryStorageInfoBuf.Length - 4);

            //尾
            for (int i = 0; i < protocolTail.Length; i++)
            {
                SendBufArray.QueryStorageInfoBuf[QueryBufIndex.Tail + i] = protocolTail[i];
            }

            //将buf转为字符串
            StringBuilder hex = new StringBuilder();
            foreach (byte b in SendBufArray.QueryStorageInfoBuf)
            {
                hex.Append(b.ToString("X2")).Append(" ");
            }
            return hex.ToString();
        }

        /// <summary>
        /// 返回“查询历史温度”报文
        /// </summary>
        /// <returns></returns>
        private static string ReturnTelegram_QueryHistoryTemperature()
        {
            //头
            for (int i = 0; i < protocolHead.Length; i++)
            {
                SendBufArray.QueryHistoryTemperatureBuf[QueryBufIndex.Head + i] = protocolHead[i];
            }

            //一帧的长度
            GNDCommon.WriteUint16ToArray((UInt16)SendBufArray.QueryHistoryTemperatureBuf.Length,
                    ref SendBufArray.QueryHistoryTemperatureBuf,
                    QueryBufIndex.TlvLength);

            //功能码
            SendBufArray.QueryHistoryTemperatureBuf[QueryBufIndex.FunCode] = (byte)FunctionCode.QUERY_HISTORY_TEMPERATURE;

            //SN
            for (int i = 0; i < WirelessWaferPara.WaferSNByteArray.Count(); i++)
            {
                SendBufArray.QueryHistoryTemperatureBuf[QueryBufIndex.BleName + i] = WirelessWaferPara.WaferSNByteArray[i];
            }

            //存储的第几次温度
            GNDCommon.WriteUint16ToArray(WirelessWaferPara.QueryInfo.StorageIndex,
                    ref SendBufArray.QueryHistoryTemperatureBuf,
                    QueryBufIndex.StorageIndex);

            //crc
            CRC.calCrc(ref SendBufArray.QueryHistoryTemperatureBuf, SendBufArray.QueryHistoryTemperatureBuf.Length - 4);

            //尾
            for (int i = 0; i < protocolTail.Length; i++)
            {
                SendBufArray.QueryHistoryTemperatureBuf[QueryBufIndex.Tail + i] = protocolTail[i];
            }

            //将buf转为字符串
            StringBuilder hex = new StringBuilder();
            foreach (byte b in SendBufArray.QueryHistoryTemperatureBuf)
            {
                hex.Append(b.ToString("X2")).Append(" ");
            }
            return hex.ToString();
        }

        /// <summary>
        /// 返回“晶圆关机”报文
        /// </summary>
        /// <returns></returns>
        private static string ReturnTelegram_WaferPowerOff()
        {
            //头
            for (int i = 0; i < protocolHead.Length; i++)
            {
                SendBufArray.ConfigWaferPowerOffBuf[CommonBufIndex.Head + i] = protocolHead[i];
            }

            //一帧的长度
            GNDCommon.WriteUint16ToArray((UInt16)SendBufArray.ConfigWaferPowerOffBuf.Length,
                    ref SendBufArray.ConfigWaferPowerOffBuf,
                    ConfigBufIndex.TlvLength);

            //功能码
            SendBufArray.ConfigWaferPowerOffBuf[CommonBufIndex.FunCode] = (byte)FunctionCode.POWER_OFF_WAFER;

            //SN
            for (int i = 0; i < WirelessWaferPara.WaferSNByteArray.Count(); i++)
            {
                SendBufArray.ConfigWaferPowerOffBuf[CommonBufIndex.BleName + i] = WirelessWaferPara.WaferSNByteArray[i];
            }

            //crc
            CRC.calCrc(ref SendBufArray.ConfigWaferPowerOffBuf, SendBufArray.ConfigWaferPowerOffBuf.Length - 4);

            //尾
            for (int i = 0; i < protocolTail.Length; i++)
            {
                SendBufArray.ConfigWaferPowerOffBuf[ConfigBufIndex.Tail + i] = protocolTail[i];
            }

            //将buf转为字符串
            StringBuilder hex = new StringBuilder();
            foreach (byte b in SendBufArray.ConfigWaferPowerOffBuf)
            {
                hex.Append(b.ToString("X2")).Append(" ");
            }
            return hex.ToString();
        }

        /// <summary>
        /// 返回“开始存储”报文
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        private static string ReturnTelegram_StartStorage(WirelessWaferPara.StorageInfo info)
        {
            //清空数组
            Array.Clear(SendBufArray.ConfigStartStorageBuf, 0, SendBufArray.ConfigStartStorageBuf.Length);

            //头
            for (int i = 0; i < protocolHead.Length; i++)
            {
                SendBufArray.ConfigStartStorageBuf[ConfigBufIndex.Head + i] = protocolHead[i];
            }

            //一帧的长度
            GNDCommon.WriteUint16ToArray((UInt16)SendBufArray.ConfigStartStorageBuf.Length,
                    ref SendBufArray.ConfigStartStorageBuf,
                    ConfigBufIndex.TlvLength);

            //功能码
            SendBufArray.ConfigStartStorageBuf[ConfigBufIndex.FunCode] = (byte)FunctionCode.START_STORAGE;

            //SN
            for (int i = 0; i < WirelessWaferPara.WaferSNByteArray.Count(); i++)
            {
                SendBufArray.ConfigStartStorageBuf[ConfigBufIndex.BleName + i] = WirelessWaferPara.WaferSNByteArray[i];
            }

            //时间
            DateTime dt = DateTime.Now;
            SendBufArray.ConfigStartStorageBuf[ConfigBufIndex.SystemTime + 0] = (byte)(dt.Year - 2000);
            SendBufArray.ConfigStartStorageBuf[ConfigBufIndex.SystemTime + 1] = (byte)dt.Month;
            SendBufArray.ConfigStartStorageBuf[ConfigBufIndex.SystemTime + 2] = (byte)dt.Day;
            SendBufArray.ConfigStartStorageBuf[ConfigBufIndex.SystemTime + 3] = (byte)dt.Hour;
            SendBufArray.ConfigStartStorageBuf[ConfigBufIndex.SystemTime + 4] = (byte)dt.Minute;
            SendBufArray.ConfigStartStorageBuf[ConfigBufIndex.SystemTime + 5] = (byte)dt.Second;

            //分成几段存储
            SendBufArray.ConfigStartStorageBuf[ConfigBufIndex.StorageSectionCount] = info.SectionCount;

            UInt16 offset = BufBytes.START_STORAGE_AFTER_TIME_BYTES + BufBytes.SECTION_STORAGE_TOTAL_TIME_BYTES + BufBytes.STORAGE_INTERVAL_BYTES;
            for (UInt16 i = 0; i < info.SectionCount; i++)
            {
                //延时多少时间后开始采集温度
                GNDCommon.WriteUint16ToArray(info.DelayTime[i],
                    ref SendBufArray.ConfigStartStorageBuf,
                    (UInt16)(ConfigBufIndex.FirstSectionStartStorageAfterTime + offset * i));

                //存储总时长
                GNDCommon.WriteUint16ToArray(info.StorageSectionTime[i],
                    ref SendBufArray.ConfigStartStorageBuf,
                    (UInt16)(ConfigBufIndex.FirstStorageSectionTotalTime + offset * i));

                //存储时间间隔
                GNDCommon.WriteUint16ToArray(info.StorageInterval[i],
                    ref SendBufArray.ConfigStartStorageBuf,
                    (UInt16)(ConfigBufIndex.FirstStorageSectionInterval + offset * i));
            }

            //crc
            CRC.calCrc(ref SendBufArray.ConfigStartStorageBuf, SendBufArray.ConfigStartStorageBuf.Length - 4);

            //尾
            for (int i = 0; i < protocolTail.Length; i++)
            {
                SendBufArray.ConfigStartStorageBuf[ConfigBufIndex.Tail + i] = protocolTail[i];
            }
            //将buf转为字符串
            StringBuilder hex = new StringBuilder();
            foreach (byte b in SendBufArray.ConfigStartStorageBuf)
            {
                hex.Append(b.ToString("X2")).Append(" ");
            }
            return hex.ToString();
        }

        /// <summary>
        /// 返回“停止存储”报文
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        private static string ReturnTelegram_StopStorage()
        {
            //清空数组
            Array.Clear(SendBufArray.ConfigStopStorageBuf, 0, SendBufArray.ConfigStopStorageBuf.Length);

            //头
            for (int i = 0; i < protocolHead.Length; i++)
            {
                SendBufArray.ConfigStopStorageBuf[ConfigBufIndex.Head + i] = protocolHead[i];
            }

            //一帧的长度
            GNDCommon.WriteUint16ToArray((UInt16)SendBufArray.ConfigStopStorageBuf.Length,
                    ref SendBufArray.ConfigStopStorageBuf,
                    ConfigBufIndex.TlvLength);

            //功能码
            SendBufArray.ConfigStopStorageBuf[ConfigBufIndex.FunCode] = (byte)FunctionCode.STOP_STORAGE;

            //SN
            for (int i = 0; i < WirelessWaferPara.WaferSNByteArray.Count(); i++)
            {
                SendBufArray.ConfigStopStorageBuf[ConfigBufIndex.BleName + i] = WirelessWaferPara.WaferSNByteArray[i];
            }

            //crc
            CRC.calCrc(ref SendBufArray.ConfigStopStorageBuf, SendBufArray.ConfigStopStorageBuf.Length - 4);

            //尾
            for (int i = 0; i < protocolTail.Length; i++)
            {
                SendBufArray.ConfigStopStorageBuf[ConfigBufIndex.Tail + i] = protocolTail[i];
            }
            //将buf转为字符串
            StringBuilder hex = new StringBuilder();
            foreach (byte b in SendBufArray.ConfigStopStorageBuf)
            {
                hex.Append(b.ToString("X2")).Append(" ");
            }
            return hex.ToString();
        }

        //协议帧头
        static byte[] protocolHead = new byte[BufBytes.HEAD_BYTES] { 0XBC, 0XAF, 0XE5, 0XC8 };
        //协议帧尾
        static byte[] protocolTail = new byte[BufBytes.TAIL_BYTES] { 0X0D, 0X0A };
        //波特率
        private string baudrate = "115200";
        //发送时间间隔
        private int sendInterval = 0;
        //一帧的长度，这里根据TLV协议来确定长度，所以不需要指定长度
        private int frameLength = 0;
        //发送配置指令后，需要接收到的一模一样的字符串
        private static string targetRxString = "";

        /// <summary>
        /// 一帧数据长度
        /// </summary>
        public int FrameLength
        {
            get
            {
                return frameLength;
            }

            set
            {
                frameLength = value;
            }
        }

        /// <summary>
        /// 波特率
        /// </summary>
        public string Baudrate
        {
            get
            {
                return baudrate;
            }

            set
            {
                baudrate = value;
            }
        }

        public int SendInterval
        {
            get
            {
                return sendInterval;
            }

            set
            {
                sendInterval = value;
            }
        }

        public StringBuilder RecHexString { get; set; }
        public static string TargetRxString { get => targetRxString; set => targetRxString = value; }

        /// <summary>
        /// 判断帧头是否正确
        /// </summary>
        /// <param name="buf"></param>
        /// <returns></returns>
        public bool IsFrameHeadCorrect(ref List<byte> buf, ref int deleteCount)
        {
            //帧头是否正确
            bool isFrameHeadCorrect = false;

            //判断帧头是否正确
            if (buf.Count >= 4)
            {
                if (buf[0] == protocolHead[0] && buf[1] == protocolHead[1]
                    && buf[2] == protocolHead[2] && buf[3] == protocolHead[3])
                {
                    isFrameHeadCorrect = true;
                }
                else
                {
                    buf.RemoveAt(0);
                    deleteCount++;
                }
            }

            return isFrameHeadCorrect;
        }

        /// <summary>
        /// 判断帧的长度是否正确
        /// </summary>
        /// <param name="buf"></param>
        /// <returns></returns>
        public bool IsFrameLengthCorrect(ref List<byte> buf, ref int deleteCount)
        {
            //帧长度是否正确
            bool isFrameLengthCorrect = false;

            if (buf.Count > 12)
            {
                //从buf解析到的TLV长度
                Int16 length = (Int16)(buf[4] << 8 | buf[5]);
                if (length >= 8)
                {
                    if (buf.Count >= length)
                    {
                        if (buf[length - 2] == 0x0D && buf[length - 1] == 0x0A)
                        {
                            isFrameLengthCorrect = true;
                        }
                        else
                        {
                            buf.RemoveAt(0);
                            deleteCount++;
                        }
                    }
                }
                else
                {
                    buf.RemoveAt(0);
                    deleteCount++;
                }
            }


            return isFrameLengthCorrect;
        }

        /// <summary>
        /// 判断帧的校验是否正确
        /// </summary>
        /// <param name="buf"></param>
        /// <returns></returns>
        public bool IsFrameCrcCorrect(ref List<byte> buf, ref int deleteCount)
        {
            bool isCrcCorrect = CRC.crc16TB(buf, buf.Count - 2);

            if (buf.Count > 10)
            {
                //从buf解析到的TLV长度
                Int16 length = (Int16)(buf[4] << 8 | buf[5]);

                if (buf.Count >= length)
                {
                    if (buf[length - 2] == protocolTail[0] && buf[length - 1] == protocolTail[1])
                    {
                        if (isCrcCorrect == true)
                        {
                            ParaSoftware.CrcOkCount++;
                        }
                        else
                        {
                            ParaSoftware.CrcErrorCount++;
                            //buf.RemoveAt(0);
                            //deleteCount++;
                            buf.Clear();
                        }
                        //注意crc无论正确或者错误，都不能删除buf中的内容，因为crc正确以后，需要对数据解析完以后再将buf中的内容删除
                    }
                }
            }

            //将buf转为字符串
            StringBuilder hex = new StringBuilder();
            foreach (byte i in buf)
            {
                hex.Append(i.ToString("X2")).Append(" ");
            }
            RecHexString = hex;

            return isCrcCorrect;
        }

        /// <summary>
        /// 解析一帧数据
        /// </summary>
        /// <param name="buf"></param>
        /// <param name="temperature"></param>
        public int AnalyseCorrectFrame(List<byte> buf, ref float[] temperature, ref float[] kalmanValue)
        {
            FunctionCode functionCode = (FunctionCode)buf[CommonBufIndex.FunCode];
            try
            {
                switch (functionCode)
                {
                    case (FunctionCode.QUERY_REAL_TIME_TEMPERATURE):
                        AnalyseRealTimeTemperautre(buf, ref temperature);
                        break;
                    case (FunctionCode.QUERY_STORAGE_INFO):
                        AnalyseStorageInfo(buf, ref WirelessWaferPara.StorageInfoReaded);
                        break;
                    case (FunctionCode.QUERY_HISTORY_TEMPERATURE):
                        AnalyseHistoryStorageTemperature(buf, ref temperature);
                        break;
                    case (FunctionCode.POWER_OFF_WAFER):
                        CheckReturnWaferPowerOffString(buf);
                        break;
                    case (FunctionCode.START_STORAGE):
                        CheckReturnStartStorageString(buf);
                        break;
                    case (FunctionCode.STOP_STORAGE):
                        CheckReturnStopStorageString(buf);
                        break;
                    default:
                        break;
                }
            }
            catch
            {
                //deleteCount += buf.Count;
                buf.Clear();
            }
            return (int)functionCode;
        }

        /// <summary>
        /// 解析接收到的wafer序列号
        /// </summary>
        /// <param name="buf"></param>
        private void AnalyseReceivedWaferSn(List<byte> buf)
        {
            int index = CommonBufIndex.BleName;
            byte[] byteArray = new byte[BufBytes.BLE_NAME_BYTES];
            for (int i = 0; i < byteArray.Length; i++)
            {
                if (buf[index + i] != 0xff)
                {
                    byteArray[i] = buf[index + i];
                }
            }
            //HEX串口数据转换为字符串
            string str = Encoding.Default.GetString(byteArray);
            if (str.IndexOf('\0') > 0)
            {
                str = str.Substring(0, str.IndexOf('\0'));
            }
            else
            {
                str = "";
            }

            WirelessWaferPara.ReceivedWaferSN = str;
        }

        /// <summary>
        /// 解析温度数据
        /// </summary>
        /// <param name="buf"></param>
        /// <param name="temperature"></param>
        private void AnalyseRealTimeTemperautre(List<byte> buf, ref float[] temperature)
        {
            //长度要是192个字节
            if (buf.Count != TelegramLength.Receive)
                return;

            //解析接收到的序列号
            AnalyseReceivedWaferSn(buf);

            //1. 用户上一次配置的时间
            //WirelessWaferPara.Storage.LastConfigDateTime = GNDCommon.arrayYYMMDDHHmmssToDateTime(buf, ReceiveRealTimeAndHistoryTemperatureBufIndex.LastConfigDateTime);

            //2. EEPROM里面一共需要存储多长时间的温度数据
            WirelessWaferPara.Storage.TotalNeedStorageTime = GNDCommon.arrayToUint24(buf, ReceiveRealTimeAndHistoryTemperatureBufIndex.TotalNeedStorageTime);

            //3. EEPROM里面一共存储了多长时间的温度数据
            WirelessWaferPara.Storage.TotalStorageTimeInEEPROM = GNDCommon.arrayToUint24(buf, ReceiveRealTimeAndHistoryTemperatureBufIndex.TotalStorageTimeInEEPROM);

            //4. EEPORM一共需要存储多少次温度数据
            WirelessWaferPara.Storage.TotalNeedStorageTimes = GNDCommon.arrayToUint16(buf, ReceiveRealTimeAndHistoryTemperatureBufIndex.TotalNeedStorageTimes);

            //5.EEPORM里面一共存了多少次的温度数据
            WirelessWaferPara.Storage.TotalStorageTimesInEEPROM = GNDCommon.arrayToUint16(buf, ReceiveRealTimeAndHistoryTemperatureBufIndex.TotalStorageTimesInEEPROM);

            //6.一共分成几段进行存储
            WirelessWaferPara.Storage.TotalSectionCount = buf[ReceiveRealTimeAndHistoryTemperatureBufIndex.TotalSectionCount];

            //7.当前已存储多少段温度
            WirelessWaferPara.Storage.CurrentStoredSectionCount = buf[ReceiveRealTimeAndHistoryTemperatureBufIndex.CurrentSectionIndex];

            //8. 当前正在存储/返回总存储的第几次的温度
            WirelessWaferPara.Storage.CurrentTemperatureIndex = GNDCommon.arrayToUint16(buf, ReceiveRealTimeAndHistoryTemperatureBufIndex.CurrentTemperatureIndex);

            //9. 当前正在存储段/返回段需要存储的总时长
            WirelessWaferPara.Storage.CurrentSectionNeedStorageTotalTime = GNDCommon.arrayToUint16(buf, ReceiveRealTimeAndHistoryTemperatureBufIndex.CurrentSectionNeedStorageTotalTime);

            //10. 当前正在存储段/返回段需要存储的总次数
            WirelessWaferPara.Storage.CurrentSectionNeedStorageTotalCount = GNDCommon.arrayToUint16(buf, ReceiveRealTimeAndHistoryTemperatureBufIndex.CurrentSectionNeedStorageTotalCount);

            //11. 当前正在存储段/返回段时间间隔
            WirelessWaferPara.Storage.CurrentSectionInterval = GNDCommon.arrayToUint16(buf, ReceiveRealTimeAndHistoryTemperatureBufIndex.CurrentSectionInterval);

            //12. 当前段已经存储了多长时间的温度数据
            WirelessWaferPara.Storage.CurrentSectionStorageTime = GNDCommon.arrayToUint16(buf, ReceiveRealTimeAndHistoryTemperatureBufIndex.CurrentSectionStorageTime);

            //13. 当前段已经存储了多少次的温度数据
            WirelessWaferPara.Storage.CurrentSectionStorageCount = GNDCommon.arrayToUint16(buf, ReceiveRealTimeAndHistoryTemperatureBufIndex.CurrentSectionStorageCount);

            ////14.当前段还有多长时间的温度数据还没有存储
            //WirelessWaferPara.Storage.CurrentSectionStorageTimeLeft = GNDCommon.arrayToUint16(buf, ReceiveRealTimeAndHistoryTemperatureBufIndex.CurrentSectionStorageTimeLeft);

            ////15.当前段还有多少次的温度数据还没有存储
            //WirelessWaferPara.Storage.CurrentSectionStorageCountLeft = GNDCommon.arrayToUint16(buf, ReceiveRealTimeAndHistoryTemperatureBufIndex.CurrentSectionStorageCountLeft);

            //16.多久之后会开始存储下一次温度
            WirelessWaferPara.Storage.StartStorageNextSectionAfterTime = GNDCommon.arrayToUint16(buf, ReceiveRealTimeAndHistoryTemperatureBufIndex.StartStorageNextSectionAfterTime);

            //17. 存储任务总时长（每一段的存储时间加上延时时间加起来的总时间）
            WirelessWaferPara.Storage.TaskNeedStorageTime = GNDCommon.arrayToUint24(buf, ReceiveRealTimeAndHistoryTemperatureBufIndex.TaskNeedStorageTime);

            //18. 存储任务已经运行了多少时间（每一段的存储时间加上延时时间加起来的总时间）
            WirelessWaferPara.Storage.TaskAlreadyRuningTime = GNDCommon.arrayToUint24(buf, ReceiveRealTimeAndHistoryTemperatureBufIndex.TaskAlreadyRuningTime);

            //预计任务结束时间更新（当还有没有存储完成 且 当时间误差超过1s的时候才更新，否者结束时间这个值会一直在变）
            int _leftSecond = (int)(WirelessWaferPara.Storage.TaskNeedStorageTime - WirelessWaferPara.Storage.TaskAlreadyRuningTime);
            DateTime datetime = DateTime.Now.AddSeconds(_leftSecond);
            if (_leftSecond > 0
                && Math.Abs((datetime - WirelessWaferPara.Storage.TaskEndDateTime).TotalSeconds) > 1)
            {
                WirelessWaferPara.Storage.TaskEndDateTime = datetime;
            }

            //解析实时和历史温度共用参数
            AnalyseRealTimeAndHistoryCommonPara(buf);

            //W2.晶圆的通道数
            WirelessWaferPara.WaferInfo.ChannelTotalCount = buf[ReceiveRealTimeAndHistoryTemperatureBufIndex.ChannelTotalCount];

            //W3. 实时温度采集的总次数（采集一次加一）
            WirelessWaferPara.WaferInfo.NewestCollectCount = GNDCommon.arrayToUint16(buf, ReceiveRealTimeAndHistoryTemperatureBufIndex.NewestCollectCount);
            ParaRtdTcCollector.CollectCount = WirelessWaferPara.WaferInfo.NewestCollectCount;

            //W4. 当前距离最新的温度采集经过了多少ms
            WirelessWaferPara.WaferInfo.NewestCollectElapsedMs = GNDCommon.arrayToUint16(buf, ReceiveRealTimeAndHistoryTemperatureBufIndex.NewestCollectElapsedMs);
            ParaRtdTcCollector.LatestTemperatureElapsedTimeMs = WirelessWaferPara.WaferInfo.NewestCollectElapsedMs;

            //W7. 任务运行状态
            WirelessWaferPara.WaferInfo.TaskRuningStatus = buf[ReceiveRealTimeAndHistoryTemperatureBufIndex.TaskRuningStatus];

            //W6. 通道1-16温度
            int index = ReceiveRealTimeAndHistoryTemperatureBufIndex.Temperature;
            for (int i = 0; i < ParaSoftware.ChannelTotalNumber; i++)
            {
                temperature[i] = (float)Math.Round((float)(((buf[index + i * 2] << 8) | (buf[index + i * 2 + 1])) * 0.0078125),
                    ParaSoftware.Decimalplaces);
            }

            EventReceivedRealTimeTemperature();
        }

        /// <summary>
        /// 解析实时和历史温度共用参数
        /// </summary>
        /// <param name="buf"></param>
        private void AnalyseRealTimeAndHistoryCommonPara(List<byte> buf)
        {
            //W1. 圆晶电池剩余电量
            WirelessWaferPara.WaferInfo.Battery = buf[ReceiveRealTimeAndHistoryTemperatureBufIndex.Battery];

            //W5. 圆晶是否在充电
            if (buf[ReceiveRealTimeAndHistoryTemperatureBufIndex.Charging] == 0)
            {
                WirelessWaferPara.WaferInfo.IsCharging = false;
            }
            else
            {
                WirelessWaferPara.WaferInfo.IsCharging = true;
            }

            //W8. 蓝牙关闭倒计时
            WirelessWaferPara.WaferInfo.BleOffRemainingTime = GNDCommon.arrayToUint16(buf, ReceiveRealTimeAndHistoryTemperatureBufIndex.BleOffRemainingTime);

            //W9. 关机倒计时
            WirelessWaferPara.WaferInfo.PowerOffRemainingTime = GNDCommon.arrayToUint16(buf, ReceiveRealTimeAndHistoryTemperatureBufIndex.PowerOffRemainingTime);
        }

        /// <summary>
        /// 解析历史存储温度
        /// </summary>
        /// <param name="buf"></param>
        /// <param name="temperature"></param>
        private void AnalyseHistoryStorageTemperature(List<byte> buf, ref float[] temperature)
        {
            //长度要是192个字节
            if (buf.Count != TelegramLength.Receive)
                return;

            //1. 用户上一次配置的时间
            WirelessWaferPara.Storage.LastConfigDateTime = GNDCommon.arrayYYMMDDHHmmssToDateTime(buf, ReceiveRealTimeAndHistoryTemperatureBufIndex.LastConfigDateTime);

            //2. EEPROM里面一共需要存储多长时间的温度数据
            WirelessWaferPara.Storage.TotalNeedStorageTime = GNDCommon.arrayToUint16(buf, ReceiveRealTimeAndHistoryTemperatureBufIndex.TotalNeedStorageTime);

            //3. EEPROM里面一共存储了多长时间的温度数据
            WirelessWaferPara.Storage.TotalStorageTimeInEEPROM = GNDCommon.arrayToUint16(buf, ReceiveRealTimeAndHistoryTemperatureBufIndex.TotalStorageTimeInEEPROM); ;

            //4. EEPORM一共需要存储多少次温度数据
            WirelessWaferPara.Storage.TotalNeedStorageTimes = GNDCommon.arrayToUint16(buf, ReceiveRealTimeAndHistoryTemperatureBufIndex.TotalNeedStorageTimes);

            //5.EEPORM里面一共存了多少次的温度数据
            WirelessWaferPara.Storage.TotalStorageTimesInEEPROM = GNDCommon.arrayToUint16(buf, ReceiveRealTimeAndHistoryTemperatureBufIndex.TotalStorageTimesInEEPROM);

            //6.一共分成几段进行存储
            WirelessWaferPara.Storage.TotalSectionCount = buf[ReceiveRealTimeAndHistoryTemperatureBufIndex.TotalSectionCount];

            //7.当前已存储多少段温度
            WirelessWaferPara.Storage.CurrentStoredSectionCount = buf[ReceiveRealTimeAndHistoryTemperatureBufIndex.CurrentSectionIndex];

            //8. 当前正在存储/返回总存储的第几次的温度
            WirelessWaferPara.Storage.CurrentTemperatureIndex = GNDCommon.arrayToUint16(buf, ReceiveRealTimeAndHistoryTemperatureBufIndex.CurrentTemperatureIndex);
            WirelessWaferPara.QueryInfo.StorageIndex = WirelessWaferPara.Storage.CurrentTemperatureIndex;

            //9. 当前正在存储段/返回段需要存储的总时长
            WirelessWaferPara.Storage.CurrentSectionNeedStorageTotalTime = GNDCommon.arrayToUint16(buf, ReceiveRealTimeAndHistoryTemperatureBufIndex.CurrentSectionNeedStorageTotalTime);

            //10. 当前正在存储段/返回段需要存储的总次数
            WirelessWaferPara.Storage.CurrentSectionNeedStorageTotalCount = GNDCommon.arrayToUint16(buf, ReceiveRealTimeAndHistoryTemperatureBufIndex.CurrentSectionNeedStorageTotalCount);

            //11. 当前正在存储段/返回段时间间隔
            WirelessWaferPara.Storage.CurrentSectionInterval = GNDCommon.arrayToUint16(buf, ReceiveRealTimeAndHistoryTemperatureBufIndex.CurrentSectionInterval);

            //12. 当前段已经存储了多长时间的温度数据
            WirelessWaferPara.Storage.CurrentSectionStorageTime = GNDCommon.arrayToUint16(buf, ReceiveRealTimeAndHistoryTemperatureBufIndex.CurrentSectionStorageTime);

            //13. 当前段已经存储了多少次的温度数据
            WirelessWaferPara.Storage.CurrentSectionStorageCount = GNDCommon.arrayToUint16(buf, ReceiveRealTimeAndHistoryTemperatureBufIndex.CurrentSectionStorageCount);

            //16. 多久之后开始存储下一段的温度
            WirelessWaferPara.Storage.StartStorageNextSectionAfterTime = GNDCommon.arrayToUint16(buf, ReceiveRealTimeAndHistoryTemperatureBufIndex.StartStorageNextSectionAfterTime);

            //解析实时和历史温度共用参数
            AnalyseRealTimeAndHistoryCommonPara(buf);

            //W6.通道1~16温度
            int index = ReceiveRealTimeAndHistoryTemperatureBufIndex.Temperature;
            for (int i = 0; i < ParaSoftware.ChannelTotalNumber; i++)
            {
                temperature[i] = (float)(((buf[index + i * 2] << 8) | (buf[index + i * 2 + 1])) * 0.0078125);
                //数据校准（实时温度在添加数据的时候校准，历史数据在解析温度的时候校准）
                temperature[i] = (float)Math.Round(Form1.CalibrationChannelValue(i, temperature[i]), ParaSoftware.Decimalplaces);

                WirelessWaferPara.QueryInfo.CurrentHistoryTemperature[i] = temperature[i];

                int _times = WirelessWaferPara.Storage.CurrentTemperatureIndex;
                if (WirelessWaferPara.HistoryStorage.xDateTime != null)
                {
                    //时间
                    WirelessWaferPara.HistoryStorage.xDateTime[_times] = WirelessWaferPara.StorageInfoReaded.StartStorageDateTime.AddSeconds(WirelessWaferPara.StorageInfoReaded.TimeNeedAdd[_times]);
                }//温度

                if (WirelessWaferPara.HistoryStorage.yTemperature != null)
                {
                    WirelessWaferPara.HistoryStorage.yTemperature[i, _times] = temperature[i];
                }
            }

            //收到历史温度事件
            EventReceivedHistoryTemperature(WirelessWaferPara.QueryInfo.StorageIndex);

            WirelessWaferPara.QueryInfo.StorageIndex++;
        }

        /// <summary>
        /// 解析存储信息
        /// </summary>
        /// <param name="buf"></param>
        /// <param name="temperature"></param>
        private void AnalyseStorageInfo(List<byte> buf, ref WirelessWaferPara.StorageInfo info)
        {
            //长度要是192个字节
            if (buf.Count != TelegramLength.Receive)
                return;

            //时间
            byte[] arr = new byte[BufBytes.SYSTEM_TIME_BYTES];
            for (int i = 0; i < BufBytes.SYSTEM_TIME_BYTES; i++)
            {
                arr[i] = buf[ConfigBufIndex.SystemTime + i];
            }
            info.StartStorageDateTime = WirelessWaferPara.YYMMDDHHmmssByteArrayToDateTime(arr);

            //分成几段存储
            info.SectionCount = buf[ConfigBufIndex.StorageSectionCount];
            if (info.SectionCount > WirelessWaferPara.MAX_SECTION_COUNT)
            {
                info.SectionCount = WirelessWaferPara.MAX_SECTION_COUNT;
            }

            info.DelayTime = new UInt16[WirelessWaferPara.MAX_SECTION_COUNT];
            info.StorageSectionTime = new UInt16[WirelessWaferPara.MAX_SECTION_COUNT];
            info.StorageInterval = new UInt16[WirelessWaferPara.MAX_SECTION_COUNT];

            UInt16 offset = BufBytes.START_STORAGE_AFTER_TIME_BYTES + BufBytes.SECTION_STORAGE_TOTAL_TIME_BYTES + BufBytes.STORAGE_INTERVAL_BYTES;
            for (UInt16 i = 0; i < info.SectionCount; i++)
            {
                //延时多少时间后开始采集温度
                info.DelayTime[i] = GNDCommon.arrayToUint16(buf, (UInt16)(ConfigBufIndex.FirstSectionStartStorageAfterTime + offset * i));

                //存储总时长
                info.StorageSectionTime[i] = GNDCommon.arrayToUint16(buf, (UInt16)(ConfigBufIndex.FirstStorageSectionTotalTime + offset * i));

                //存储时间间隔
                info.StorageInterval[i] = GNDCommon.arrayToUint16(buf, (UInt16)(ConfigBufIndex.FirstStorageSectionInterval + offset * i));
            }

            //计算与存储相关的参数
            WirelessWaferPara.StorageInfoReaded.CalculateStoragePara();

            //收到“存储信息”帧事件
            EventReceivedHistoryStorageInfo();

            //历史数据的时间变量初始化
            int _length = WirelessWaferPara.StorageInfoReaded.TimeNeedAdd.Length;
            WirelessWaferPara.HistoryStorage.xDateTime = new DateTime[_length];
            WirelessWaferPara.HistoryStorage.yTemperature = new float[ParaSoftware.ChannelTotalNumber, _length];

            //需要读取存储信息标志位
            WirelessWaferCommunicationProtocol_TLV.BufNeedSend.QueryStorageInfo = false;

            //需要读取历史温度标志位
            WirelessWaferCommunicationProtocol_TLV.BufNeedSend.QureyHistoryTemperature = true;
        }

        /// <summary>
        /// 比较Hex数组和字符串是否相等
        /// </summary>
        /// <param name="buf"></param>
        /// <param name="str"></param>
        /// <returns></returns>
        public bool CompareHexListAndString(List<byte> buf, string str)
        {
            bool pass = false;
            //将buf转为字符串
            StringBuilder hex = new StringBuilder();
            foreach (byte b in buf)
            {
                hex.Append(b.ToString("X2")).Append(" ");
            }

            //比较HEX数组和字符串是否相等
            if (str == hex.ToString())
            {
                pass = true;
            }
            else
            {
                pass = false;
            }
            return pass;
        }

        /// <summary>
        /// 检查返回的wafer关机指令是否正确
        /// </summary>
        /// <param name="buf"></param>
        private void CheckReturnWaferPowerOffString(List<byte> buf)
        {
            //比较收到的内容和需要收到的内容是否相等，如果相等，证明指令发送成功了
            bool pass = CompareHexListAndString(buf, WirelessWaferCommunicationProtocol_TLV.TargetRxString);

            if (pass == true)
            {
                WirelessWaferCommunicationProtocol_TLV.BufNeedSend.WirelessWaferPowerOff = false;
            }
        }

        /// <summary>
        /// 检查返回的开始存储指令是否正确
        /// </summary>
        /// <param name="buf"></param>
        private void CheckReturnStartStorageString(List<byte> buf)
        {
            //比较收到的内容和需要收到的内容是否相等，如果相等，证明指令发送成功了
            bool pass = CompareHexListAndString(buf, WirelessWaferCommunicationProtocol_TLV.TargetRxString);

            if (pass == true)
            {
                WirelessWaferCommunicationProtocol_TLV.BufNeedSend.StartStorage = false;
            }
        }

        /// <summary>
        /// 检查返回的结束存储指令是否正确
        /// </summary>
        /// <param name="buf"></param>
        private void CheckReturnStopStorageString(List<byte> buf)
        {
            //比较收到的内容和需要收到的内容是否相等，如果相等，证明指令发送成功了
            bool pass = CompareHexListAndString(buf, WirelessWaferCommunicationProtocol_TLV.TargetRxString);

            if (pass == true)
            {
                WirelessWaferCommunicationProtocol_TLV.BufNeedSend.StopStorage = false;
            }
        }

        /// <summary>
        /// 不带参数的委托
        /// </summary>
        public delegate void DelUpdate();
        /// <summary>
        /// 收到“实时温度”事件
        /// </summary>
        public static event DelUpdate EventReceivedRealTimeTemperature;
        /// <summary>
        /// 收到“历史存储信息”事件
        /// </summary>
        public static event DelUpdate EventReceivedHistoryStorageInfo;
        /// <summary>
        /// Wafer SN不匹配事件
        /// </summary>
        public static event DelUpdate EventWaferSNNotMaching;

        /// <summary>
        /// 带一个UInt16的委托
        /// </summary>
        /// <param name="a"></param>
        public delegate void DelUpdate_UInt16(UInt16 a);
        /// <summary>
        /// 收到“存储温度”
        /// </summary>
        public static event DelUpdate_UInt16 EventReceivedHistoryTemperature;

        /// <summary>
        /// 带string和bool的委托
        /// </summary>
        /// <param name="str"></param>
        /// <param name="showCancelButton"></param>
        public delegate void DelUpdate_String_Bool(string str, bool showCancelButton);
        /// <summary>
        /// 更新Tx提示内容事件
        /// </summary>
        public static event DelUpdate_String_Bool EventUpdateTxTips;


        public int GetArrayListLength()
        {
            return bufList.Count;
        }

        /// <summary>
        /// 串口数据缓冲区（从串口队列中获取的数据）
        /// </summary>
        List<byte> bufList = new List<byte>();//

        /// <summary>
        /// 处理串口队列数据
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="deleteQueueCount"></param>
        /// <returns></returns>
        public bool ProcessQueue(ref Queue queue, ref int deleteQueueCount)
        {
            bool isQueueRight = false;
            //串口缓冲区数据放入qList中去处理
            lock (queue.SyncRoot)
            {
                if (queue.Count >= 1)
                {
                    bufList.Add((byte)queue.Dequeue());//把接收到的数据添加进可变长度数组
                }
            }

            //如果帧头正确，进行后续处理
            if (IsFrameHeadCorrect(ref bufList, ref deleteQueueCount) == true)
            {
                //如果帧长度正确，进行后续处理
                if (IsFrameLengthCorrect(ref bufList, ref deleteQueueCount) == true)//
                {
                    //如果帧校验正确，对数据进行解析，然后将解析完的串口数据删除
                    if (IsFrameCrcCorrect(ref bufList, ref deleteQueueCount) == true)
                    {
                        float[] temperature = new float[ParaSoftware.ChannelTotalNumber + ParaSoftware.ChannelExtraNumber];//温度
                        float[] kalmanValue = new float[4];//温度

                        FunctionCode functionCodeType = (FunctionCode)AnalyseCorrectFrame(bufList, ref temperature, ref kalmanValue);
                        //解析数据
                        if (functionCodeType == FunctionCode.QUERY_REAL_TIME_TEMPERATURE)
                        {
                            for (int i = 0; i < ParaSoftware.ChannelTotalNumber; i++)
                            {
                                ParaSoftware.DataRecReal1.ucConnectCount[i] = ParaSoftware.CONNECT_OUT_TIME;//连接超时时间重新赋值
                                ParaSoftware.DataRecReal1.bConnectFlag[i] = true;//连接超时时间重新赋值
                                ParaSoftware.DataRecReal1.OriginTemp[i] = temperature[i];
                            }
                            RtdWaferOrder.ChangeRtdOrder(temperature);//修改RTD输出顺序
                            isQueueRight = true;
                        }
                        else if (functionCodeType == FunctionCode.QUERY_HISTORY_TEMPERATURE)
                        {

                        }
                        bufList.Clear();//清除数据
                    }
                }
            }
            return isQueueRight;
        }

        public void AnalyseCorrectFrame(List<byte> buf, ref float[] temperature)
        {
            throw new NotImplementedException();
        }
    }
}
