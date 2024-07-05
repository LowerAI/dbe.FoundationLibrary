using dbe.FoundationLibrary.Core.Common;
using dbe.FoundationLibrary.Core.Extensions;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Reflection;

namespace dbe.FoundationLibrary.Core.Util
{
    /// <summary>
    /// 转换工具类
    /// </summary>
    public class ConvertUtil
    {
        /// <summary>
        /// 将泛型集合类转换成DataTable
        /// 来源链接：多年前写的DataTable与实体类的转换，已放github - sinodzh - 博客园 https://www.cnblogs.com/mephisto/p/4349086.html
        /// </summary>
        /// <typeparam name="T">集合项类型</typeparam>
        /// <param name="sourceArray">集合</param>
        /// <param name="propertyNameArray">需要返回的列的列名，如需返回所有列，此参数传入null值</param>
        /// <returns>数据集(表)</returns>
        public static DataTable ConvertModelToDataTable<T>(IList<T> sourceArray, params string[] propertyNameArray)
        //where T : class
        {
            List<string> propertyNameList = new List<string>();
            if (propertyNameArray != null)
                propertyNameList.AddRange(propertyNameArray);

            DataTable result = new DataTable();
            //获取结构
            Type[] typeArr = sourceArray.GetType().GetGenericArguments();
            if (typeArr.Length == 0)
                return result;

            PropertyInfo[] propertys = typeArr[0].GetProperties();
            FieldInfo[] fields = typeArr[0].GetFields();
            foreach (FieldInfo fi in fields)
            {
                if (propertyNameList.Count == 0)
                {
                    result.Columns.Add(fi.Name, fi.FieldType);
                }
                else
                {
                    if (propertyNameList.Contains(fi.Name))
                        result.Columns.Add(fi.Name, fi.FieldType);
                }
            }
            foreach (PropertyInfo pi in propertys)
            {
                if (propertyNameList.Count == 0)
                {
                    result.Columns.Add(pi.Name, pi.PropertyType);
                }
                else
                {
                    if (propertyNameList.Contains(pi.Name))
                        result.Columns.Add(pi.Name, pi.PropertyType);
                }
            }

            for (int i = 0; i < sourceArray.Count; i++)
            {
                ArrayList tempList = new ArrayList();
                foreach (FieldInfo fi in fields)
                {
                    if (propertyNameList.Count == 0)
                    {
                        object obj = fi.GetValue(sourceArray[i]);
                        tempList.Add(obj);
                    }
                    else
                    {
                        if (propertyNameList.Contains(fi.Name))
                        {
                            object obj = fi.GetValue(sourceArray[i]);
                            tempList.Add(obj);
                        }
                    }
                }
                foreach (PropertyInfo pi in propertys)
                {
                    if (propertyNameList.Count == 0)
                    {
                        object obj = pi.GetValue(sourceArray[i], null);
                        tempList.Add(obj);
                    }
                    else
                    {
                        if (propertyNameList.Contains(pi.Name))
                        {
                            object obj = pi.GetValue(sourceArray[i], null);
                            tempList.Add(obj);
                        }
                    }
                }
                object[] array = tempList.ToArray();
                result.LoadDataRow(array, true);
            }

            return result;
        }

        /// <summary>
        /// 返回指定时间类型值经过转换后的指定时间类型值
        /// </summary>
        /// <param name="inUint">输入值的时间类型</param>
        /// <param name="timeValue">输入值</param>
        /// <param name="outUint">返回值的时间类型</param>
        /// <returns></returns>
        public static double TimeConvert(TimeUnit inUint, double timeValue, TimeUnit outUint)
        {
            double outTime = -1;
            TimeSpan inTime = TimeSpan.Zero;
            switch (inUint)
            {
                case TimeUnit.Millisecond:
                    inTime = TimeSpan.FromMilliseconds(timeValue);
                    break;
                case TimeUnit.Second:
                    inTime = TimeSpan.FromSeconds(timeValue);
                    break;
                case TimeUnit.Minute:
                    inTime = TimeSpan.FromMinutes(timeValue);
                    break;
                case TimeUnit.Hour:
                    inTime = TimeSpan.FromHours(timeValue);
                    break;
            }
            switch (outUint)
            {
                case TimeUnit.Millisecond:
                    outTime = inTime.TotalMilliseconds;
                    break;
                case TimeUnit.Second:
                    outTime = inTime.TotalSeconds;
                    break;
                case TimeUnit.Minute:
                    outTime = inTime.TotalMinutes;
                    break;
                case TimeUnit.Hour:
                    outTime = inTime.TotalHours;
                    break;
            }
            return outTime;
        }

        /// <summary>
        /// 返回指定时间类型值的TimeSpan形式
        /// </summary>
        /// <param name="inUint">输入值的时间类型</param>
        /// <param name="timeValue">输入值</param>
        /// <param name="outUint">TimeSpan形式的值</param>
        /// <returns></returns>
        public static TimeSpan TimeConvert(TimeUnit inUint, double timeValue)
        {
            TimeSpan inTime = TimeSpan.Zero;
            switch (inUint)
            {
                case TimeUnit.Millisecond:
                    inTime = TimeSpan.FromMilliseconds(timeValue);
                    break;
                case TimeUnit.Second:
                    inTime = TimeSpan.FromSeconds(timeValue);
                    break;
                case TimeUnit.Minute:
                    inTime = TimeSpan.FromMinutes(timeValue);
                    break;
                case TimeUnit.Hour:
                    inTime = TimeSpan.FromHours(timeValue);
                    break;
            }
            return inTime;
        }

        /// <summary>
        /// 返回动态对象转换为byte[]的值
        /// </summary>
        /// <typeparam name="Dynamic">动态类型，运行时才可知</typeparam>
        /// <param name="source">动态对象</param>
        /// <param name="dataFormat">字节序，默认大端</param>
        /// <returns>转换后的byte[]</returns>
        public static byte[] ToByteArray(dynamic source, Endianness dataFormat = Endianness.ABCD)
        {
            byte[] ret = null;
            try
            {
                byte[] val = null;
                switch (source)
                {
                    case bool boolValue:
                        val = BitConverter.GetBytes(boolValue);
                        break;
                    case char charValue:
                        val = BitConverter.GetBytes(charValue);
                        break;
                    case string stringValue:
                        val = stringValue.ToByteArray();
                        break;
                    case sbyte i8Value:
                        val = new byte[] { (byte)(i8Value & 0xFF) };
                        break;
                    case byte u8Value:
                        val = new byte[] { u8Value };
                        break;
                    case short i16Value:
                        val = BitConverter.GetBytes(i16Value);
                        break;
                    case ushort u16Value:
                        val = BitConverter.GetBytes(u16Value);
                        break;
                    case int i32Value:
                        val = BitConverter.GetBytes(i32Value);
                        break;
                    case uint u32Value:
                        val = BitConverter.GetBytes(u32Value);
                        break;
                    case long i64Value:
                        val = BitConverter.GetBytes(i64Value);
                        break;
                    case ulong u64Value:
                        val = BitConverter.GetBytes(u64Value);
                        break;
                    case float floatValue:
                        val = BitConverter.GetBytes(floatValue);
                        break;
                    case double doubleValue:
                        val = BitConverter.GetBytes(doubleValue);
                        break;
                    case Array arrayValue:
                        val = arrayValue.ToByteArray();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(source));
                }
                ret = val.Format(dataFormat);
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"ConvertUtil.ToByteArray转换异常：{ex.Message}");
                throw ex;
            }
            return ret;
        }
    }
}