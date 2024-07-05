using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace dbe.FoundationLibrary.Core.Extensions
{
    /// <summary>
    /// DateView扩展类
    /// </summary>
    public static class DateViewExtension
    {
        /// <summary>
        /// 将视图转换成泛型集合
        /// 来源链接：多年前写的DataTable与实体类的转换，已放github - sinodzh - 博客园 https://www.cnblogs.com/mephisto/p/4349086.html
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="dataView">视图</param>
        /// <param name="model">泛型实例</param>
        /// <returns></returns>
        public static List<T> ToModel<T>(DataView dataView, T model) where T : class
        {
            List<T> listReturn = new List<T>();
            Type modelType = model.GetType();
            DataTable dt = dataView.Table;
            //获取model所有类型
            PropertyInfo[] modelProperties = modelType.GetProperties();

            //遍历所有行，逐行添加对象
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                object obj = modelType.Assembly.CreateInstance(modelType.FullName);
                listReturn.Add((T)obj);
                //遍历model所有属性
                foreach (PropertyInfo pi in modelProperties)
                {
                    //遍历所有列
                    foreach (DataColumn col in dt.Columns)
                    {
                        //如果列数据类型与model的数据类型相同、名称相同
                        if (col.DataType == pi.PropertyType
                            && col.ColumnName == pi.Name)
                        {
                            pi.SetValue(obj, dt.Rows[i][col.ColumnName], null);
                        }
                    }
                }
            }

            return listReturn;
        }
    }
}