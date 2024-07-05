using dbe.FoundationLibrary.Core.Extensions;

using System;
using System.Collections.Generic;

namespace dbe.FoundationLibrary.Core.Util
{
    /// <summary>
    /// 枚举辅助类，因为很多辅助函数没法写成枚举的扩展函数
    /// </summary>
    public class EnumHelper
    {
        /// <summary>
        /// 返回由枚举值及其注释组成的kvp列表
        /// </summary>
        /// <typeparam name="TEnum">枚举类型</typeparam>
        /// <returns>KeyValuePair列表</returns>
        public static List<KeyValuePair<string, TEnum>> GetEnumValuesWithComments<TEnum>() where TEnum : Enum
        {
            var enumType = typeof(TEnum);
            var values = new List<KeyValuePair<string, TEnum>>();

            foreach (TEnum value in Enum.GetValues(enumType))
            {
                //int intValue = (int)value;
                //string stringValue = GetEnumComment(enumType, value);
                string stringValue = value.GetDescription();

                values.Add(new KeyValuePair<string, TEnum>(stringValue, value));
            }

            return values;
        }

        /// <summary>
        /// 返回由枚举值及其基类值组成的kvp列表
        /// </summary>
        /// <typeparam name="TKey">枚举继承的基类型，如byte、int</typeparam>
        /// <typeparam name="TEnum">枚举类型</typeparam>
        /// <returns>KeyValuePair列表</returns>
        public static List<KeyValuePair<TKey, TEnum>> GetEnumValuesWithNumber<TKey, TEnum>() where TKey : struct where TEnum : Enum
        {
            var enumType = typeof(TEnum);
            //var baseType = enumType.BaseType;// 此处只能获取到Enum
            var values = new List<KeyValuePair<TKey, TEnum>>();

            foreach (TEnum value in Enum.GetValues(enumType))
            {
                TKey keyValue = (TKey)Convert.ChangeType(value, typeof(TKey));

                values.Add(new KeyValuePair<TKey, TEnum>(keyValue, value));
            }

            return values;
        }

        /// <summary>
        /// 返回由枚举基类值组成的kvp列表
        /// </summary>
        /// <typeparam name="TKey">枚举继承的基类型，如byte、int</typeparam>
        /// <typeparam name="TEnum">枚举类型</typeparam>
        /// <returns>KeyValuePair列表</returns>
        public static List<KeyValuePair<TKey, TKey>> GetEnumValuesToNumbers<TKey, TEnum>() where TKey : struct where TEnum : Enum
        {
            var enumType = typeof(TEnum);
            //var baseType = enumType.BaseType;// 此处只能获取到Enum
            var values = new List<KeyValuePair<TKey, TKey>>();

            foreach (TEnum value in Enum.GetValues(enumType))
            {
                TKey keyValue = (TKey)Convert.ChangeType(value, typeof(TKey));

                values.Add(new KeyValuePair<TKey, TKey>(keyValue, keyValue));
            }

            return values;
        }
    }
}