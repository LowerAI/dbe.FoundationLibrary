using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace dbe.FoundationLibrary.DataAccess.IDAL
{
    public class SqlServerClient : IDBClient
    {
        public DbConnection GetDbConnection(string connectionString)
        {
            return new SqlConnection(connectionString);
        }

        public DbCommand GetDbCommand(string cmdText)
        {
            return new SqlCommand(cmdText);
        }

        public DbDataAdapter GetDbDataAdappter()
        {
            return new SqlDataAdapter();
        }

        public DbParameter GetDbParameter()
        {
            return new SqlParameter();
        }

        public DbParameter GetDbParameter(string type, int? size = null)
        {
            SqlParameter para = new SqlParameter();
            para.SqlDbType = SqlDbType.NVarChar; //(31种)
            return para;
        }

        /// <summary>
        /// 获取翻页查询的sql
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="pageNumber">当前页索引</param>
        /// <param name="pageSize">每页大小</param>
        /// <param name="orderInfo">排序</param>
        /// <returns></returns>
        public string GetPagingSql(string cmdText, int pageNumber, int pageSize, string orderInfo)
        {
            int startIndex = (pageNumber - 1) * pageSize;
            int endIndex = startIndex + pageSize + 1;
            cmdText = string.Format(@";WITH T1 AS ({0}),T2 AS (SELECT *,ROW_NUMBER() OVER (ORDER BY {1}) AS _RowNum FROM T1)
                                       SELECT * FROM T2
                                       WHERE _RowNum > {2} AND _RowNum < {3}", cmdText, orderInfo, startIndex, endIndex);
            return cmdText;
        }

        public static string GetConnectionString(string dataSource, string database, string userID, string password)
        {
            string strConn = string.Format("Data Source={0};Initial Catalog={1};Persist Security Info=True;User ID={2};Password={3};", dataSource, database, userID, password);
            return strConn;
        }
        /// <summary>
        /// 批量往SqlServer数据库中批量插入数据,通常用于先把原始数据原封不动导入到数据库的临时表，之后再使用此临时表方便进行比对和操作
        /// </summary>
        /// <param name="dbConn">数据库连接对象</param>
        /// <param name="sourceDataTable">数据源表</param>
        /// <param name="targetTableName">服务器上目标表(数据表名)</param>
        /// <param name="mapList">创建新的列映射，并使用列序号引用源列和目标列的列名称。</param>
        public int BulkInsert(DbConnection dbConn, DataTable sourceDataTable, string targetTableName, Dictionary<string, string> mapList)
        {
            if (dbConn.GetType().Namespace != "System.Data.SqlClient")
            {
                throw new Exception("数据库连接对象不是SQL Server，无法继续执行!");
            }

            SqlConnection conn = dbConn as SqlConnection;
            //SqlBulkCopy bulkCopy = new SqlBulkCopy(conn.ConnectionString, SqlBulkCopyOptions.KeepNulls);   //用其它源的数据有效批量加载sql server表中
            SqlBulkCopy bulkCopy = new SqlBulkCopy(conn);
            //指定大容量插入是否对表激发触发器。此属性的默认值为 False。
            //SqlBulkCopy bulkCopy = new SqlBulkCopy(DBHelper.ConnectionString, SqlBulkCopyOptions.FireTriggers);
            bulkCopy.DestinationTableName = targetTableName;   //服务器上目标表的名称
            bulkCopy.BatchSize = sourceDataTable.Rows.Count;   //每一批次中的行数
            //bulkCopy.BulkCopyTimeout = 300; //超时之前操作完成所允许的秒数,大批量数量需要的时长5分钟,2013-11-6备注 报错：“超时时间已到。在操作完成之前超时时间已过或服务器未响应”  解决办法：

            if (sourceDataTable != null && sourceDataTable.Rows.Count != 0)
            {
                foreach (KeyValuePair<string, string> map in mapList)
                {
                    bulkCopy.ColumnMappings.Add(map.Key, map.Value);
                }
                //将提供的数据源中的所有行复制到目标表中
                bulkCopy.WriteToServer(sourceDataTable);
            }
            bulkCopy.Close();
            return bulkCopy.BatchSize;
        }

        /// <summary>
        /// 使用MySqlDataAdapter批量更新数据
        /// </summary>
        /// <param name="dbConn"></param>
        /// <param name="table">源数据表，必须指定主键且确保每一行的RowState=Modified</param>
        /// <param name="batchSize">单词更新的最大行数</param>
        /// <returns></returns>
        public int BatchUpdate(DbConnection dbConn, DataTable table, int batchSize = 1000)
        {
            int ret = -1;
            SqlTransaction transaction = null;
            try
            {
                SqlCommand command = dbConn.CreateCommand() as SqlCommand;
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                SqlCommandBuilder commandBulider = new SqlCommandBuilder(adapter);
                commandBulider.ConflictOption = ConflictOption.OverwriteChanges;
                commandBulider.SetAllValues = true;//必不可少且不能为false否则Update成功数据表无变化

                dbConn.Open();
                transaction = dbConn.BeginTransaction() as SqlTransaction;

                //设置批量更新的每次处理条数
                adapter.UpdateBatchSize = batchSize;
                //设置事物
                adapter.SelectCommand.Transaction = transaction;

                if (table.ExtendedProperties["SQL"] != null)
                {
                    adapter.SelectCommand.CommandText = table.ExtendedProperties["SQL"].ToString();
                }
                ret = adapter.Update(table);
                transaction.Commit();//提交事务
            }
            catch (SqlException ex)
            {
                if (transaction != null) transaction.Rollback();
                throw ex;
            }
            finally
            {
                dbConn.Close();
                dbConn.Dispose();
            }
            return ret;
        }
    }
}