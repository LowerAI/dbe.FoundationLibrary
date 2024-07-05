using System;
using System.ComponentModel;
using System.Reflection;

namespace dbe.FoundationLibrary.Core.Extensions
{
    public static class EnumExtension
    {
        /// <summary>
        /// 获取枚举值的描述
        /// </summary>
        /// <param name="value">枚举值</param>
        /// <returns>枚举值的描述</returns>
        public static string GetDescription(this Enum value)
        {
            Type type = value.GetType();
            string name = Enum.GetName(type, value);
            if (name != null)
            {
                FieldInfo fieldInfo = type.GetField(name);
                var attr = fieldInfo?.GetCustomAttribute<DescriptionAttribute>();
                return attr?.Description;
            }
            return null;
        }
    }
}