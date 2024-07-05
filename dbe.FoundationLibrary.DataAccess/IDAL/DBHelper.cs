using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;

namespace dbe.FoundationLibrary.DataAccess.IDAL
{
    /// <summary>
    /// 数据库类型
    /// </summary>
    public enum DatabaseType
    {
        /// <summary>
        /// MySql
        /// </summary>
        MySqlClient = 0,
        /// <summary>
        /// OleDb
        /// </summary>
        OleDbClient = 1,
        /// <summary>
        /// Oracle
        /// </summary>
        OracleClient = 2,
        /// <summary>
        /// SqlServer
        /// </summary>
        SqlServerClient = 3
    }

    /// <summary>
    /// 数据库操作核心类
    /// </summary>
    public class DBHelper
    {
        #region 数据库连接串举例
        /// 警告：
        /// 使用本类的实例之前请在App.comfig/Web.comfig中添加如下配直节：
        /// <connectionStrings>
        // <add name="Self" connectionString="Data Source=192.168.128.111;Initial Catalog=ShipRecord;Persist Security Info=True;User ID=sa;Password=sccs123;" providerName="SqlServerClient"/>
        // <add name="ERP" connectionString="Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=192.168.21.16)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=PROD)));User Id=mes;Password=H9i1q2w5r;" providerName="OracleClient"/>
        /// </connectionStrings>

        /*
         * * 数据库连接串说明举例
         * 
         * 
         * 
         ********* Sql Server ********
         * · ODBC 
         * 标准连接（Standard Security）: 
         * "Driver={SQL Server};Server=Aron1;Database=pubs;Uid=sa;Pwd=asdasd;" 
         * · OLE DB, OleDbConnection (.NET) 
         * 标准连接（Standard Security）: 
         * "Provider=sqloledb;Data Source=Aron1;Initial Catalog=pubs;User Id=sa;Password=asdasd;" 
         * · SqlConnection (.NET) 
         *  标准连接（Standard Security）: 
         * "Data Source=Aron1;Initial Catalog=pubs;User Id=sa;Password=asdasd;" 
         * 或者
         * "Server=Aron1;Database=pubs;User ID=sa;Password=asdasd;Trusted_Connection=False" 
         * 
         * 
         * 
         ********* Access ********
         * · ODBC 
         * 标准连接（Standard Security）: 
         * "Driver={Microsoft Access Driver (*.mdb)};Dbq=C:\mydatabase.mdb;Uid=Admin;Pwd=;" 
         * · OLE DB, OleDbConnection (.NET) 
         * 标准连接（Standard Security）: 
         * "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=\somepath\mydb.mdb;User Id=admin;Password=;" 
         * "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=\somepath\mydb.mdb;Jet OLEDB:Database Password=MyDbPassword;","admin", "" 
         * 
         * ********* Oracle *********
         * · ODBC 
         * 新版本: 
         * "Driver={Microsoft ODBC for Oracle};Server=OracleServer.world;Uid=Username;Pwd=asdasd;" 
         * 旧版本: 
         * "Driver={Microsoft ODBC Driver for Oracle};ConnectString=OracleServer.world;Uid=myUsername;Pwd=myPassword;"
         * · OLE DB, OleDbConnection (.NET)
         * 标准连接（Standard Security）: 
         * "Provider=msdaora;Data Source=MyOracleDB;User Id=UserName;Password=asdasd;" 这是Microsoft的格式
         * 或者
         * "Provider=OraOLEDB.Oracle;Data Source=MyOracleDB;User Id=Username;Password=asdasd;" 这是Oracle的格式
         * · OracleConnection (.NET) 
         * 标准连接: 
         * "User ID=用户名; Password=密码; Data Source=服务名"
         *  
         *           
         ********** MySQL *********
         * · ODBC 
         * 本地数据库: 
         * "Driver={mySQL};Server=mySrvName;Option=16834;Database=mydatabase;" 
         * 远程数据库: 
         * "Driver={mySQL};Server=data.domain.com;Port=3306;Option=131072;Stmt=;Database=my-database;Uid=username;Pwd=password;" 
         * · OLE DB, OleDbConnection (.NET) 
         * 标准连接: 
         * "Provider=MySQLProv;Data Source=mydb;User Id=UserName;Password=asdasd;" 
         * · MySqlConnection (.NET)
         * "server=localhost; user id=15secs; password=password; database=mydatabase; pooling=false;"
         * 
         *  
         * 
         */
        #endregion 数据库连接串举例

        /// <summary>
        /// 当前数据连接对象的唯一标识符
        /// </summary>
        private string gDbId = "Self";

        private IDBClient dbClient = null;

        //private static DBConnectionPool pool = null;

        private static Dictionary<string, ConnectionStringSettings> ConnStrDict = AppConfig.GetConnStrDict();

        // 否则会报出无法追踪的异常，注意配置节中的name值必须与本类中的gDbId的保持一直。
        private string ConnectionString = AppConfig.GetAppConfig("Self");

        private string m_ProviderName;
        /// <summary>
        /// 数据库类型,值的字符串形如"SqlServerClient"、"OracleClient"
        /// </summary>
        public DatabaseType dbt
        {
            get { return (DatabaseType)Enum.Parse(typeof(DatabaseType), m_ProviderName); }
        }

        //private IDBClient DBClient = DBClientFactory.GetDBClient("SqlServerClient");

        #region Constuctor

        [ThreadStatic]
        private static TransConnection TransConnectionObj = null;

        static DBHelper()
        {
            //pool = new DBConnectionPool();
        }

        public DBHelper(string name)
        {
            Init(name);
        }

        public DBHelper(string name, string connStr, string providerName)
        {
            Init(name, connStr, providerName);
        }

        /// <summary>
        /// 根据指定的参数初始化数据库
        /// </summary>
        /// <param name="name">键名</param>
        /// <param name="connStr">连接字符串</param>
        /// <param name="providerName">数据库类型</param>
        public void Init(string name, string connStr, string providerName)
        {
            gDbId = name;
            if (string.IsNullOrWhiteSpace(providerName))
            {
                providerName = "SqlServerClient";
            }
            dbClient = DBClientFactory.GetDBClient(providerName);
            this.m_ProviderName = providerName;
            if (!ConnStrDict.ContainsKey(name))
            {
                ConnectionStringSettings css = new ConnectionStringSettings(name, connStr, providerName);
                ConnStrDict.Add(name, css);
            }
        }

        /// <summary>
        /// 初始化数据库，数据库类型为SQLServer
        /// </summary>
        /// <param name="name"></param>
        /// <param name="connStr"></param>
        public void Init(string name, string connStr)
        {
            Init(name, connStr, null);
        }

        /// <summary>
        /// 初始化数据库，键名必须在ConnStrDict中已存在
        /// </summary>
        /// <param name="name"></param>
        public void Init(string name)
        {
            ConnectionStringSettings css = ConnStrDict[name];
            if (css == null)
            {
                throw new ArgumentException("找不到指定键名对应的数据库连接参数！");
            }
            Init(name, css.ConnectionString, css.ProviderName);
        }

        #endregion

        #region ExecuteNonQuery

        public int ExecuteNonQuery(CommandType cmdType, string cmdText, params DbParameter[] parameterValues)
        {
            int result = 0;
            bool mustCloseConn = true;

            DbCommand cmd = PrepareCmd(cmdType, cmdText, parameterValues, out mustCloseConn);
            OpenConn(cmd.Connection);
            result = cmd.ExecuteNonQuery();

            if (mustCloseConn)
            {
                CloseConn(cmd.Connection);
            }
            ClearCmdParameters(cmd);
            cmd.Dispose();

            return result;
        }

        public int ExecuteNonQuery(string cmdText)
        {
            CommandType cmdType = CommandType.Text;
            int result = this.ExecuteNonQuery(cmdType, cmdText, null);
            return result;
        }

        #endregion ExecuteNonQuery

        #region ExecuteScalar
        public object ExecuteScalar(CommandType cmdType, string cmdText, params DbParameter[] parameterValues)
        {
            object result = 0;
            bool mustCloseConn = true;

            DbCommand cmd = PrepareCmd(cmdType, cmdText, parameterValues, out mustCloseConn);
            OpenConn(cmd.Connection);
            result = cmd.ExecuteScalar();

            if (mustCloseConn) CloseConn(cmd.Connection);
            ClearCmdParameters(cmd);
            cmd.Dispose();

            return result;
        }

        /// <summary>
        /// 返回指定的sql语句得到的第一个单元格的值
        /// </summary>
        /// <param name="cmdText"></param>
        /// <returns></returns>
        public object ExecuteScalar(string cmdText)
        {
            CommandType cmdType = CommandType.Text;
            object obj = this.ExecuteScalar(cmdType, cmdText, null);
            return obj;
        }
        #endregion ExecuteScalar

        #region ExecuteReader
        public DbDataReader ExecuteReader(CommandType cmdType, string cmdText, params DbParameter[] parameterValues)
        {
            DbDataReader result = null;
            bool mustCloseConn = true;
            DbCommand cmd = PrepareCmd(cmdType, cmdText, parameterValues, out mustCloseConn);
            try
            {
                OpenConn(cmd.Connection);
                //if (mustCloseConn && pool == null)
                if (mustCloseConn)
                {
                    result = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                }
                else
                {
                    result = cmd.ExecuteReader();
                }
                ClearCmdParameters(cmd);
                return result;
            }
            catch (Exception ex)
            {
                if (mustCloseConn) CloseConn(cmd.Connection);
                ClearCmdParameters(cmd);
                cmd.Dispose();
                throw ex;
            }
        }
        #endregion ExecuteReader

        #region ExecuteDataset
        public DataSet ExecuteDataSet(CommandType cmdType, string cmdText, params DbParameter[] parameterValues)
        {
            DataSet result = null;
            bool mustCloseConn = true;

            DbCommand cmd = PrepareCmd(cmdType, cmdText, parameterValues, out mustCloseConn);
            OpenConn(cmd.Connection);
            using (DbDataAdapter da = dbClient.GetDbDataAdappter())
            {
                da.SelectCommand = cmd;
                result = new DataSet();

                da.Fill(result);
            }

            if (mustCloseConn) CloseConn(cmd.Connection);
            ClearCmdParameters(cmd);
            cmd.Dispose();

            return result;
        }
        #endregion ExecuteDataset

        #region ExecuteDataTable
        public DataTable ExecuteDataTable(CommandType cmdType, string cmdText, params DbParameter[] parameterValues)
        {
            DataTable result = null;
            bool mustCloseConn = true;

            DbCommand cmd = PrepareCmd(cmdType, cmdText, parameterValues, out mustCloseConn);
            OpenConn(cmd.Connection);
            using (DbDataAdapter da = dbClient.GetDbDataAdappter())
            {
                da.SelectCommand = cmd;
                result = new DataTable();

                da.Fill(result);
            }

            if (mustCloseConn)
            {
                CloseConn(cmd.Connection);
            }
            ClearCmdParameters(cmd);
            cmd.Dispose();

            return result;
        }

        /// <summary>
        /// 返回执行指定的sql语句得到的表格
        /// </summary>
        /// <param name="cmdText"></param>
        /// <returns></returns>
        public DataTable ExecuteDataTable(string cmdText)
        {
            CommandType cmdType = CommandType.Text;
            DataTable dt = this.ExecuteDataTable(cmdType, cmdText, null);
            return dt;
        }

        /// <summary>
        /// 获取数据源的架构信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetDataSourceSchema()
        {
            DbConnection conn = GetConn();
            OpenConn(conn);
            DataTable dt = conn.GetSchema("Tables");
            CloseConn(conn);
            return dt;
        }
        #endregion

        #region ExecuteFirstDataRow
        public DataRow ExecuteFirstDataRow(string cmdText)
        {
            CommandType cmdType = CommandType.Text;
            DataRow dr = this.ExecuteFirstDataRow(cmdType, cmdText, null);
            return dr;
        }

        public DataRow ExecuteFirstDataRow(CommandType cmdType, string cmdText, params DbParameter[] parameterValues)
        {
            DataTable dt = ExecuteDataTable(cmdType, cmdText, parameterValues);
            return dt == null ? null : dt.Rows.Count > 0 ? dt.Rows[0] : null;
        }
        #endregion

        #region ExecutePaging
        public DataTable ExecutePagingDataTable(string cmdText, int pageNumber, int pageSize, string orderInfo, out int totalCount)
        {
            return ExecutePagingDataTable(CommandType.Text, cmdText, pageNumber, pageSize, orderInfo, out totalCount, null);
        }

        public DataTable ExecutePagingDataTable(CommandType cmdType, string cmdText, int pageNumber, int pageSize, string orderInfo, out int totalCount, params DbParameter[] parameterValues)
        {
            string sql = cmdText;
            cmdText = dbClient.GetPagingSql(cmdText, pageNumber, pageSize, orderInfo);
            DataTable dt = ExecuteDataTable(CommandType.Text, cmdText, parameterValues);
            cmdText = "SELECT COUNT(1) FROM (@cmdText) T".Replace("@cmdText", sql); //翻页的情况下只有这样才能取到总行数
            totalCount = Convert.ToInt32(ExecuteScalar(CommandType.Text, cmdText, null) ?? "-1");
            return dt;
        }

        public DbDataReader ExecutePagingReader(CommandType cmdType, string cmdText, int pageNumber, int pageSize, string orderInfo, out int totalCount, params DbParameter[] parameterValues)
        {
            string sql = cmdText;
            cmdText = dbClient.GetPagingSql(cmdText, pageNumber, pageSize, orderInfo);
            DbDataReader dreader = ExecuteReader(CommandType.Text, cmdText, parameterValues);
            cmdText = "SELECT COUNT(1) FROM (@cmdText) T".Replace("@cmdText", sql);
            totalCount = Convert.ToInt32(ExecuteScalar(CommandType.Text, cmdText, null) ?? "-1");
            return dreader;
        }
        #endregion

        #region    Batch To Database
        /// <summary>
        /// 批量往数据库中插入大数据
        /// 通常用于先把原始数据原封不动导入到数据库的临时表，之后再使用此临时表方便进行比对和操作
        /// </summary>
        /// <param name="sourceDataTable">数据源表</param>
        /// <param name="targetTableName">服务器上目标表(数据表名)</param>
        /// <param name="mapDict">创建新的列映射，SQLServer和Oracle此处映射的内容不一样。</param>
        public int ExecuteBulkToDB(DataTable sourceDataTable, string targetTableName, Dictionary<string, string> mapDict)
        {
            int result = 0;
            DbConnection conn = GetConn();
            OpenConn(conn);
            result = dbClient.BulkInsert(conn, sourceDataTable, targetTableName, mapDict);
            CloseConn(conn);
            return result;
        }

        /// <summary>
        /// 批量更新指定DataTable(源数据表)的数据到数据库
        /// 注意：如果源数据最开始来自从数据库中查询目的表的数据则无需在DataTable.ExtendedProperties["SQL"]中添加查询目的表的语句，否则此处应写入查询目的表的语句以指定要更新的目标表及其目的列
        /// </summary>
        /// <param name="sourceDataTable">源数据表，必须指定主键且确保每一行的RowState=Modified/param>
        /// <returns></returns>
        public int ExecuteBatchUpdate(DataTable sourceDataTable)
        {
            int result = 0;
            DbConnection conn = GetConn();
            result = dbClient.BatchUpdate(conn, sourceDataTable);
            return result;
        }
        #endregion Batch To Database

        #region Transaction
        /// <summary>
        /// 开启事务
        /// </summary>
        public void BeginTransaction()
        {
            if (TransConnectionObj == null)
            {
                string connStr = ConnStrDict.ContainsKey(gDbId) ? ConnStrDict[gDbId].ConnectionString : ConnectionString;
                DbConnection conn = dbClient.GetDbConnection(connStr);
                //DbConnection conn = ConnStrDict.ContainsKey(gDbId) ? pool.GetConnectionFromPool(ConnStrDict[gDbId]) : DBClient.GetDbConnection(ConnectionString);
                OpenConn(conn);
                DbTransaction trans = conn.BeginTransaction();
                TransConnectionObj = new TransConnection();
                TransConnectionObj.DBTransaction = trans;
            }
            else
            {
                TransConnectionObj.Deeps += 1;
            }
        }

        /// <summary>
        /// 提交事务
        /// </summary>
        public void CommitTransaction()
        {
            if (TransConnectionObj == null) return;
            if (TransConnectionObj.Deeps > 0)
            {
                TransConnectionObj.Deeps -= 1;
            }
            else
            {
                TransConnectionObj.DBTransaction.Commit();
                ReleaseTransaction();
            }
        }

        /// <summary>
        /// 回滚事务
        /// </summary>
        public void RollbackTransaction()
        {
            if (TransConnectionObj == null) return;
            if (TransConnectionObj.Deeps > 0)
            {
                TransConnectionObj.Deeps -= 1;
            }
            else
            {
                TransConnectionObj.DBTransaction.Rollback();
                ReleaseTransaction();
            }
        }

        /// <summary>
        /// 释放事务
        /// </summary>
        private void ReleaseTransaction()
        {
            if (TransConnectionObj == null) return;
            DbConnection conn = TransConnectionObj.DBTransaction.Connection;
            TransConnectionObj.DBTransaction.Dispose();
            TransConnectionObj = null;
            CloseConn(conn);
        }
        #endregion

        #region Connection
        /// <summary>
        /// 获取Connection对象
        /// </summary>
        /// <returns></returns>
        private DbConnection GetConn()
        {
            string connStr = ConnStrDict.ContainsKey(gDbId) ? ConnStrDict[gDbId].ConnectionString : ConnectionString;
            DbConnection conn = dbClient.GetDbConnection(connStr);
            //DbConnection conn = ConnStrDict.ContainsKey(gDbId) ? pool.GetConnectionFromPool(ConnStrDict[gDbId]) : DBClient.GetDbConnection(ConnectionString);
            return conn;
        }

        /// <summary>
        /// 打开数据库连接
        /// </summary>
        /// <param name="conn"></param>
        private void OpenConn(DbConnection conn)
        {
            if (conn == null)
            {
                conn = dbClient.GetDbConnection(ConnectionString);
            }
            if (conn.State == ConnectionState.Connecting)
            {
                conn.Close();
            }
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
        }

        /// <summary>
        /// 关闭数据库连接
        /// </summary>
        /// <param name="conn"></param>
        private void CloseConn(DbConnection conn)
        {
            if (conn == null)
            {
                return;
            }
            //if (pool != null && !string.IsNullOrWhiteSpace(gDbId) && !string.IsNullOrWhiteSpace(conn.ConnectionString))
            //{
            //    pool.ReturnConnectionToPool(gDbId);
            //}
            else
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
                conn.Dispose();
                conn = null;
            }
        }

        /// <summary>
        /// 测试当前数据连接是否可用
        /// </summary>
        /// <returns></returns>
        public bool TestConn()
        {
            bool bRet = false;
            DbConnection conn = null;
            try
            {
                conn = GetConn();
                OpenConn(conn);
                bRet = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                CloseConn(conn);
            }
            return bRet;
        }
        #endregion

        #region Create DbParameter

        /// <summary>
        /// 创建参数
        /// </summary>
        /// <param name="paraName">名称</param>
        /// <param name="value">值</param>
        /// <returns>返回参数DbParameter</returns>
        public DbParameter CreateInDbParameter(string paraName, object value = null)
        {
            DbParameter para = dbClient.GetDbParameter();
            para.ParameterName = paraName;
            if (value != null)
            {
                para.Value = value;
            }
            return para;
        }

        public DbParameter CreateInDbParameter(string paraName, DbType type, int size, object value)
        {
            return CreateDbParameter(paraName, type, size, value, ParameterDirection.Input);
        }

        public DbParameter CreateInDbParameter(string paraName, string type, int size, object value)
        {
            return CreateDbParameter(paraName, type, size, value, ParameterDirection.Input);
        }

        public DbParameter CreateInDbParameter(string paraName, DbType type, object value)
        {
            return CreateDbParameter(paraName, type, 0, value, ParameterDirection.Input);
        }

        public DbParameter CreateInDbParameter(string paraName, string type, object value)
        {
            return CreateDbParameter(paraName, type, 0, value, ParameterDirection.Input);
        }

        public DbParameter CreateInOutDbParameter(string paraName, object value = null)
        {
            DbParameter para = dbClient.GetDbParameter();
            para.ParameterName = paraName;
            para.Direction = ParameterDirection.InputOutput;
            if (value != null)
            {
                para.Value = value;
            }
            return para;
        }

        public DbParameter CreateInOutDbParameter(string paraName, DbType type, int size, object value = null)
        {
            return CreateDbParameter(paraName, type, size, value, ParameterDirection.InputOutput);
        }

        public DbParameter CreateInOutDbParameter(string paraName, string type, int size, object value = null)
        {
            return CreateDbParameter(paraName, type, size, value, ParameterDirection.InputOutput);
        }

        public DbParameter CreateOutDbParameter(string paraName, DbType type, int? size = null)
        {
            return CreateDbParameter(paraName, type, size, null, ParameterDirection.Output);
        }

        public DbParameter CreateOutDbParameter(string paraName, string type, int? size = null)
        {
            return CreateDbParameter(paraName, type, size, null, ParameterDirection.Output);
        }

        public DbParameter CreateReturnDbParameter(string paraName, DbType type, int? size = null)
        {
            return CreateDbParameter(paraName, type, size, null, ParameterDirection.ReturnValue);
        }

        public DbParameter CreateReturnDbParameter(string paraName, string type, int? size = null)
        {
            return CreateDbParameter(paraName, type, size, null, ParameterDirection.ReturnValue);
        }

        public DbParameter CreateDbParameter(string paraName, dynamic type, int? size, object value, ParameterDirection direction = ParameterDirection.Input)
        {
            DbParameter para = null;
            if (type.GetType() == typeof(string))
            {
                para = dbClient.GetDbParameter(type, size);
            }
            else if (type.GetType() == typeof(DbType))
            {
                para = dbClient.GetDbParameter();
                para.DbType = type;

                if (size != null)
                {
                    para.Size = size.Value;
                }
            }

            para.ParameterName = paraName;

            if (value != null)
            {
                para.Value = value;
            }
            else
            {
                para.Value = DBNull.Value;
            }

            para.Direction = direction;

            return para;
        }

        #endregion

        #region Command and Parameter
        /// <summary>
        /// 预处理用户提供的命令,数据库连接/事务/命令类型/参数
        /// </summary>
        /// <param name="cmdType">命令类型 (存储过程,命令文本, 其它.)</param>
        /// <param name="cmdText">存储过程名或都T-SQL命令文本</param>
        /// <param name="cmdParams">和命令相关联的DbParameter参数数组,如果没有参数为'null'</param>
        /// <param name="mustCloseConn"><c>true</c> 如果连接是打开的,则为true,其它情况下为false.</param>
        private DbCommand PrepareCmd(CommandType cmdType, string cmdText, DbParameter[] cmdParams, out bool mustCloseConn)
        {
            DbCommand cmd = dbClient.GetDbCommand(cmdText);
            cmd.CommandTimeout = 900;                             //查询超时15分钟

            DbConnection conn = GetConn();
            if (TransConnectionObj != null)
            {
                cmd.Transaction = TransConnectionObj.DBTransaction;
                mustCloseConn = ConnStrDict.ContainsKey(gDbId);
            }
            else
            {
                mustCloseConn = true;
            }
            cmd.Connection = conn;
            cmd.CommandType = cmdType;

            AttachParameters(cmd, cmdParams);

            return cmd;
        }

        /// <summary>
        /// CreateAddDbParameter添加参数到对应cmd
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="paraName"></param>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <param name="size"></param>
        /// <param name="direction"></param>
        protected void AttachParameters(DbCommand cmd, string paraName, object value, DbType type, int size, ParameterDirection direction)
        {
            DbParameter para = cmd.CreateParameter();
            para.ParameterName = paraName;
            para.Value = value;
            para.DbType = type;
            para.Size = size;
            para.Direction = direction;
            cmd.Parameters.Add(para);
        }

        /// <summary>
        /// 将DbParameter参数数组(参数值)分配给DbCommand命令.
        /// 这个方法将给任何一个参数分配DBNull.Value;
        /// 该操作将阻止默认值的使用.
        /// </summary>
        /// <param>命令名</param>
        /// <param>SqlParameters数组</param>
        private void AttachParameters(DbCommand command, DbParameter[] commandParameters)
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }
            if (commandParameters != null)
            {
                foreach (DbParameter p in commandParameters)
                {
                    if (p != null)
                    {
                        // 检查未分配值的输出参数,将其分配以DBNull.Value.
                        if ((p.Direction == ParameterDirection.InputOutput || p.Direction == ParameterDirection.Input) &&
                        (p.Value == null))
                        {
                            p.Value = DBNull.Value;
                        }
                        command.Parameters.Add(p);
                    }
                }
            }
        }

        private void ClearCmdParameters(DbCommand cmd)
        {
            bool canClear = true;
            if (cmd.Connection != null && cmd.Connection.State != ConnectionState.Open)
            {
                foreach (DbParameter commandParameter in cmd.Parameters)
                {
                    if (commandParameter.Direction != ParameterDirection.Input)
                    {
                        canClear = false;
                        break;
                    }
                }
            }
            if (canClear)
            {
                cmd.Parameters.Clear();
            }
        }
        #endregion
    }
}