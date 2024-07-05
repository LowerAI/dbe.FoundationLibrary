using System.ComponentModel;

namespace dbe.FoundationLibrary.Core.Extensions
{
    /// <summary>
    /// char的扩展类
    /// </summary>
    public static class CharExtension
    {
        /// <summary>
        /// 判断字符是否为布尔 => BitLib.IsBoolean
        /// </summary>
        /// <param name="source">布尔字符</param>
        /// <returns>布尔结果</returns>
        [Description("判断是否为布尔")]
        public static bool ToBoolean(char source)
        {
            return source == '1';
        }
    }
}