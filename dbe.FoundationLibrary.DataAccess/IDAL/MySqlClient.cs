using MySql.Data.MySqlClient;

using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;

namespace dbe.FoundationLibrary.DataAccess.IDAL
{
    public class MySqlClient : IDBClient
    {
        public DbConnection GetDbConnection(string connectionString)
        {
            return new MySqlConnection(connectionString);
        }

        public DbCommand GetDbCommand(string cmdText)
        {
            return new MySqlCommand(cmdText);
        }

        public DbDataAdapter GetDbDataAdappter()
        {
            return new MySqlDataAdapter();
        }

        public DbParameter GetDbParameter()
        {
            return new MySqlParameter();
        }

        public DbParameter GetDbParameter(string type, int? size = null)
        {
            MySqlParameter para = new MySqlParameter();
            para.MySqlDbType = MySqlDbType.VarChar;  //(39种)
            return para;
        }

        public string GetPagingSql(string cmdText, int pageNumber, int pageSize, string orderInfo)
        {
            int startIndex = (pageNumber - 1) * pageSize;
            cmdText = string.Format(@"{0} {1} Limit {2}, {3}", cmdText, orderInfo, startIndex, pageSize);
            return cmdText;
        }

        public static string GetConnectionString(string dataSource, string database, string userID, string password)
        {
            string strConn = string.Format("Data Source={0};Initial Catalog={1};Persist Security Info=True;User ID={2};Password={3};", dataSource, database, userID, password);
            return strConn;
        }

        public int BulkInsert(DbConnection dbConn, DataTable sourceDataTable, string targetTableName, Dictionary<string, string> mapList)
        {
            //if (string.IsNullOrEmpty(table.TableName))
            //{
            //    throw new Exception("请给DataTable的TableName属性附上表名称");
            //}
            if (string.IsNullOrEmpty(sourceDataTable.TableName))
            {
                sourceDataTable.TableName = targetTableName;
            }
            if (sourceDataTable.Rows.Count == 0) return 0;
            int insertCount = 0;
            string tmpPath = Path.GetTempFileName();
            string csv = BatchDataConvert.DataTableToCsv(sourceDataTable);
            File.WriteAllText(tmpPath, csv);
            using (MySqlConnection conn = new MySqlConnection(dbConn.ConnectionString))
            {
                MySqlTransaction tran = null;
                try
                {
                    conn.Open();
                    tran = conn.BeginTransaction();
                    MySqlBulkLoader bulk = new MySqlBulkLoader(conn)
                    {
                        FieldTerminator = ",",
                        FieldQuotationCharacter = '"',
                        EscapeCharacter = '"',
                        LineTerminator = "\r\n",
                        FileName = tmpPath,
                        NumberOfLinesToSkip = 0,
                        TableName = targetTableName,
                    };
                    bulk.Columns.AddRange(sourceDataTable.Columns.Cast<DataColumn>().Select(colum => colum.ColumnName).ToList());
                    insertCount = bulk.Load();
                    tran.Commit();
                }
                catch (MySqlException ex)
                {
                    if (tran != null) tran.Rollback();
                    throw ex;
                }
            }
            File.Delete(tmpPath);
            return insertCount;
        }

        /// <summary>
        ///使用MySqlDataAdapter批量更新数据
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="table">数据表</param>
        //public void BatchUpdate(string connectionString, DataTable table, int cmdTimeOut = 900, int batchSize = 1000)
        public int BatchUpdate(DbConnection dbConn, DataTable table, int batchSize = 1000)
        {
            int ret = -1;
            //MySqlConnection connection = new MySqlConnection(connectionString);

            MySqlCommand command = dbConn.CreateCommand() as MySqlCommand;
            //command.CommandTimeout = cmdTimeOut;
            //command.CommandType = CommandType.Text;
            MySqlDataAdapter adapter = new MySqlDataAdapter(command);
            MySqlCommandBuilder commandBulider = new MySqlCommandBuilder(adapter);
            commandBulider.ConflictOption = ConflictOption.OverwriteChanges;
            commandBulider.SetAllValues = true;//必不可少且不能为false否则Update成功数据表无变化

            MySqlTransaction transaction = null;
            try
            {
                dbConn.Open();
                transaction = dbConn.BeginTransaction() as MySqlTransaction;
                //设置批量更新的每次处理条数
                adapter.UpdateBatchSize = batchSize;
                //设置事物
                adapter.SelectCommand.Transaction = transaction;

                if (table.ExtendedProperties["SQL"] != null)
                {
                    adapter.SelectCommand.CommandText = table.ExtendedProperties["SQL"].ToString();
                }
                ret = adapter.Update(table);
                transaction.Commit();/////提交事务
            }
            catch (MySqlException ex)
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
