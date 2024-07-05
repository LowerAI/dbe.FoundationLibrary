using System;
using System.Text.RegularExpressions;

namespace dbe.FoundationLibrary.Core.Util
{
    /// <summary>
    /// 自定义标识类，生成的ID通常用作数据表的主键
    /// </summary>
    public class CustomID
    {
        /// <summary>
        /// 计算的基准时间点
        /// </summary>
        private static readonly DateTime dtBase = DateTime.Parse("1970-01-01 08:00:00");

        /// <summary>
        /// 还原MainID或PB_ID为时间点
        /// </summary>
        /// <param name="mainID"></param>
        /// <returns></returns>
        public static DateTime MainID2DateTime(string mainID)
        {
            DateTime? dt = null;
            int idType = -1;

            if (Regex.IsMatch(mainID, "^[0-9]{13}$", RegexOptions.IgnoreCase))
            {//如果是13位纯数字(MainID)
                idType = 1; //标识为MainID
            }
            else if (Regex.IsMatch(mainID, "^[0-9A-Z]{8}$", RegexOptions.IgnoreCase))
            {//如果是8位数字和字母(认证码)
                //mainID = Compute.ConvertTo10(mainID).ToString();  //转换为13位纯数字字符串
                idType = 1; //标识为MainID
            }
            else if (Regex.IsMatch(mainID, "^[0-9]{10}$", RegexOptions.IgnoreCase))
            {//批次号必须是10纯位数字
                idType = 2; //标识为PB_ID
            }
            else
            {
                throw new Exception("输入的ID不在指定的范围之内，转换失败！");
            }

            if (idType == 1)
            {
                dt = dtBase.AddMilliseconds(Convert.ToInt64(mainID));
            }
            else if (idType == 2)
            {
                dt = dtBase.AddSeconds(Convert.ToInt64(mainID));
            }

            return dt.Value;
        }

        /// <summary>
        /// 生成10位数字，一般用作数据回传的主键
        /// </summary>
        /// <returns></returns>
        public static uint Get10bitNo()
        {
            TimeSpan ts = DateTime.Now - dtBase;
            uint id = (uint)(ts.TotalSeconds);
            return id;
        }

        /// <summary>
        /// 生成10位数字，一般用作数据回传的主键
        /// </summary>
        /// <param name="dt">指定的时间</param>
        /// <returns></returns>
        public static uint Get10bitNo(DateTime dt)
        {
            TimeSpan ts = dt - dtBase;
            uint id = (uint)(ts.TotalSeconds);
            return id;
        }

        /// <summary>
        /// 生成13位数字，一般用作认证码
        /// </summary>
        /// <returns></returns>
        public static ulong Get13bitNo()
        {
            TimeSpan ts = DateTime.Now - dtBase;
            ulong id = (ulong)(ts.TotalMilliseconds);
            return id;
        }

        /// <summary>
        /// 生成13位数字，一般用作认证码
        /// </summary>
        /// <param name="dt">指定的时间</param>
        /// <returns></returns>
        public static ulong Get13bitNo(DateTime dt)
        {
            TimeSpan ts = dt - dtBase;
            ulong id = (ulong)(ts.TotalMilliseconds);
            return id;
        }
    }
}