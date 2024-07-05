using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace dbe.FoundationLibrary.Core.Extensions
{
    /// <summary>
    /// DateTable扩展类
    /// </summary>
    public static class DateTableExtension
    {
        /// <summary>
        /// 将DB中改动的内容同步到泛型集合中
        /// 来源链接：多年前写的DataTable与实体类的转换，已放github - sinodzh - 博客园 https://www.cnblogs.com/mephisto/p/4349086.html
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="source">dt源</param>
        /// <param name="destinationArray">目标Model集合</param>
        /// <returns></returns>
        public static bool ToModel<T>(this DataTable source, List<T> destinationArray)
            where T : class
        {
            if (source == null || destinationArray == null || source.PrimaryKey == null || source.PrimaryKey.Count() <= 0)
                return false;

            DataTable dtChange = source.GetChanges();
            if (dtChange == null)
                return false;

            List<string> keys = new List<string>();
            foreach (var item in source.PrimaryKey)
            {
                keys.Add(item.ColumnName);
            }

            return ToModel(source, destinationArray, keys);
        }

        /// <summary>
        /// 同步table里改动的数据到泛型集合里去（新增，修改，删除）
        /// 来源链接：多年前写的DataTable与实体类的转换，已放github - sinodzh - 博客园 https://www.cnblogs.com/mephisto/p/4349086.html
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="source">dt源</param>
        /// <param name="destinationArray">目标Model集合</param>
        /// <param name="keyColumnArray">主键集合</param>
        /// <returns></returns>
        public static bool ToModel<T>(this DataTable source, List<T> destinationArray, List<string> keyColumnArray)
            where T : class
        {
            if (source == null || destinationArray == null || source.Rows.Count == 0 || keyColumnArray == null || keyColumnArray.Count == 0)
                return false;

            Type modeType = destinationArray.GetType().GetGenericArguments()[0];//模型类型
            PropertyInfo[] ppInfoArray = modeType.GetProperties();//公共属性集合
            List<PropertyInfo> listPPInfo = ppInfoArray.ToList();//方便查询
            //关键列
            List<PropertyInfo> keyPIArray = listPPInfo.FindAll(x => keyColumnArray.Contains(x.Name));

            List<T> listToDelete = new List<T>();
            //新增的数据
            DataRow[] drAddArray = source.Select("", "", DataViewRowState.Added);

            object objItem = modeType.Assembly.CreateInstance(modeType.FullName);
            foreach (DataRow dr in drAddArray)
            {
                destinationArray.Add((T)objItem);
                foreach (PropertyInfo pi in listPPInfo)
                {
                    pi.SetValue(destinationArray[destinationArray.Count - 1], dr[pi.Name], null);
                }
            }
            //修改和删除的数据
            DataView dvForOP = new DataView(source);
            dvForOP.RowStateFilter = DataViewRowState.Deleted | DataViewRowState.ModifiedCurrent;

            foreach (DataRowView drv in dvForOP)
            {
                for (int i = 0; i < destinationArray.Count; i++)
                {
                    bool blIsTheRow = true;
                    //找出关键列对应的行
                    foreach (PropertyInfo pInfo in keyPIArray)
                    {
                        object okey = pInfo.GetValue(destinationArray[i], null);
                        if (okey == null)
                            continue;
                        if (drv[pInfo.Name].ToString() != okey.ToString())
                        {
                            blIsTheRow = false;
                            break;
                        }
                    }
                    if (!blIsTheRow)//非本行
                        continue;
                    //根据行状态同步赋值
                    switch (drv.Row.RowState)
                    {
                        case DataRowState.Modified:
                            {
                                foreach (PropertyInfo pi in listPPInfo)
                                {
                                    if (keyPIArray.Contains(pi))//主键列不更新
                                        continue;
                                    pi.SetValue(destinationArray[i], drv[pi.Name], null);
                                }
                            }
                            break;
                        case DataRowState.Deleted:
                            {
                                listToDelete.Add(destinationArray[i]);
                            }
                            break;
                    }
                }
            }

            for (int i = 0; i < listToDelete.Count; i++)
            {
                destinationArray.Remove(listToDelete[i]);
            }

            return true;
        }
    }
}