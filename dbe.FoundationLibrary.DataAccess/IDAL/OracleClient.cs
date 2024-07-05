using Oracle.DataAccess.Client;

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;

namespace dbe.FoundationLibrary.DataAccess.IDAL
{
    public class OracleClient : IDBClient
    {
        public DbConnection GetDbConnection(string connectionString)
        {
            return new OracleConnection(connectionString);
        }

        public DbCommand GetDbCommand(string cmdText)
        {
            return new OracleCommand(cmdText);
        }

        public DbDataAdapter GetDbDataAdappter()
        {
            return new OracleDataAdapter();
        }

        public DbParameter GetDbParameter()
        {
            return new OracleParameter();
        }

        public DbParameter GetDbParameter(string type, int? size = null)
        {
            OracleParameter para = new OracleParameter();
            type = type.ToUpper(); //(31种)
            if (size != null)
            {
                para.Size = size.Value;
            }
            if (type == "INTEGER" && size.Value == 2)
            {
                para.OracleDbType = OracleDbType.Int16;
            }
            else if (type == "INTEGER" && size.Value == 8)
            {
                para.OracleDbType = OracleDbType.Int64;
            }
            else if (type == "FLOAT" && size.Value == 4)
            {
                para.OracleDbType = OracleDbType.Single;
            }
            else if (type == "FLOAT" && size.Value == 8)
            {
                para.OracleDbType = OracleDbType.Double;
            }
            else if (type == "BYTE")
            {
                para.OracleDbType = OracleDbType.Byte;
            }
            else if (type == "BFILE")
            {
                para.OracleDbType = OracleDbType.BFile;
            }
            else if (type == "BINARY_DOUBLE")
            {
                para.OracleDbType = OracleDbType.BinaryDouble;
            }
            else if (type == "BINARY_FLOAT")
            {
                para.OracleDbType = OracleDbType.BinaryFloat;
            }
            else if (type == "BINARY_INTEGER" || type == "PLS_INTEGER")
            {
                para.OracleDbType = OracleDbType.Int32;
            }
            else if (type == "BLOB")
            {
                para.OracleDbType = OracleDbType.Blob;
            }
            else if (type == "CHAR")
            {
                para.OracleDbType = OracleDbType.Char;
            }
            else if (type == "NCHAR")
            {
                para.OracleDbType = OracleDbType.NChar;
            }
            else if (type == "CLOB")
            {
                para.OracleDbType = OracleDbType.Clob;
            }
            else if (type == "COLLECTION")
            {
                para.OracleDbType = OracleDbType.Array;
            }
            else if (type == "DATE")
            {
                para.OracleDbType = OracleDbType.Date;
            }
            else if (type == "INTERVAL DAY TO SECOND")
            {
                para.OracleDbType = OracleDbType.IntervalDS;
            }
            else if (type == "INTERVAL YEAR TO MONTH")
            {
                para.OracleDbType = OracleDbType.IntervalYM;
            }
            else if (type == "LONG")
            {
                para.OracleDbType = OracleDbType.Long;
            }
            else if (type == "LONG RAW")
            {
                para.OracleDbType = OracleDbType.LongRaw;
            }
            else if (type == "NCHAR")
            {
                para.OracleDbType = OracleDbType.NChar;
            }
            else if (type == "NCLOB")
            {
                para.OracleDbType = OracleDbType.NClob;
            }
            else if (type == "NUMBER")
            {
                para.OracleDbType = OracleDbType.Decimal;
            }
            else if (type == "NVARCHAR2")
            {
                para.OracleDbType = OracleDbType.NVarchar2;
            }
            else if (type == "RAW")
            {
                para.OracleDbType = OracleDbType.Raw;
            }
            else if (type == "REF CURSOR")
            {
                para.OracleDbType = OracleDbType.RefCursor;
            }
            else if (type == "TIMESTAMP")
            {
                para.OracleDbType = OracleDbType.TimeStamp;
            }
            else if (type == "TIMESTAMP WITH LOCAL TIME ZONE")
            {
                para.OracleDbType = OracleDbType.TimeStampLTZ;
            }
            else if (type == "TIMESTAMP WITH TIME ZONE")
            {
                para.OracleDbType = OracleDbType.TimeStampTZ;
            }
            else if (type == "VARCHAR2")
            {
                para.OracleDbType = OracleDbType.Varchar2;
            }
            else if (type == "XMLType")
            {
                para.OracleDbType = OracleDbType.XmlType;
            }
            return para;
        }

        /// <summary>
        /// 获取翻页查询的sql
        /// </summary>
        /// <param name="cmdText">查询的sql语句</param>
        /// <param name="pageNumber">当前页号(从1开始)</param>
        /// <param name="pageSize">每页大小</param>
        /// <param name="orderInfo">排序信息(例如：ORDER BY UserId DESC)</param>
        /// <returns></returns>
        public string GetPagingSql(string cmdText, int pageNumber, int pageSize, string orderInfo)
        {
            int startIndex = (pageNumber - 1) * pageSize;
            int endIndex = startIndex + pageSize + 1;
            cmdText = string.Format(@"SELECT *
                                      FROM (SELECT a.*, ROWNUM rn
                                            FROM (
                                              SEELCT *
                                              FROM ({0})
                                              ORDER BY {1}
                                            ) a
                                            WHERE ROWNUM < {3})
                                      WHERE rn > {2}
                                      ", cmdText, orderInfo, startIndex, endIndex);
            return cmdText;
        }

        public static string GetConnectionString(string host, string port, string serviceName, string userID, string password)
        {
            string strConn = string.Format("Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST={0})(PORT={1}))(CONNECT_DATA=(SERVICE_NAME={2})));User Id={3};Password={4};", host, port, serviceName, userID, password);
            return strConn;
        }

        /// <summary>
        /// 批量导入数据插入Oracle数据表
        /// </summary>
        /// <param name="dbConn"></param>
        /// <param name="sourceDataTable">源数据表：必须设定好列名和对应的C#数据类型</param>
        /// <param name="targetTableName">Oracle数据库中的表名</param>
        /// <param name="mapList">映射字典：列名和对应的Oracle字段类型</param>
        public int BulkInsert(DbConnection dbConn, DataTable sourceDataTable, string targetTableName, Dictionary<string, string> mapList)
        {
            int recc = sourceDataTable.Rows.Count;

            OracleCommand cmd = new OracleCommand();
            cmd.Connection = dbConn as OracleConnection;
            cmd.CommandTimeout = 900;                             //查询超时15分钟

            Stopwatch sw = new Stopwatch();
            sw.Start();

            List<string> colValLst = new List<string>();  //存列变量
            string colValStr = "";
            foreach (string key in mapList.Keys)
            {
                string oracleType = mapList[key];                 //取出列名对应的Oracle数据类型
                if (oracleType.ToUpper() == "Date".ToUpper())
                {
                    colValStr = "TO_DATE(:" + key + ",'yyyy-mm-dd hh24:mi:ss')";
                    oracleType = "NVarchar2";
                }
                else if (oracleType.ToUpper() == "TimeStamp".ToUpper())
                {
                    //colValStr = "TO_TIMESTAMP(TO_CHAR(:" + key + ",'yyyy-MM-dd hh24:mi:ss:ff3'),'yyyy-MM-dd hh24:mi:ss:ff3')";
                    colValStr = "TO_TIMESTAMP(:" + key + ",'yyyy-mm-dd hh24:mi:ss.ff')";
                    oracleType = "NVarchar2";
                }
                else
                {
                    colValStr = ":" + key;
                }
                colValLst.Add(colValStr);

                //Type t = sourceDataTable.Columns[key].DataType;         //取出列名对应的C#数据类型
                //var vals = sourceDataTable.AsEnumerable().Select(r => r[key]).ToList();
                var vals = sourceDataTable.AsEnumerable().Select((r) =>
                {
                    //string keyNew = key.Replace("\"", "");                //Oracle表字段区分大小写的时候会带有双引号此时应去除
                    var v = r[key];
                    if (v.GetType() == typeof(bool))                      //bool型需转换为int字符串
                    {
                        return Convert.ToInt32(v).ToString();
                    }
                    else if (v.GetType() == typeof(DateTime))             //DateTime型需精确到毫秒
                    {
                        DateTime dt = Convert.ToDateTime(v);
                        return dt.ToString("yyyy-MM-dd hh:mm:ss.ff3");
                    }
                    return v;
                }).ToArray();

                OracleDbType odt = (OracleDbType)Enum.Parse(typeof(OracleDbType), oracleType);
                OracleParameter param = new OracleParameter(key, odt);
                param.Direction = ParameterDirection.Input;
                param.Value = vals;
                cmd.Parameters.Add(param);
            }

            string colNameStr = string.Join(",", mapList.Keys);
            colValStr = string.Join(",", colValLst);
            string sqlStr = string.Format("INSERT INTO {0}({1}) VALUES ({2})", targetTableName, colNameStr, colValStr);

            sw.Stop();
            Debug.WriteLine("批量转换:" + recc.ToString() + "pcs所耗时间:" + sw.ElapsedMilliseconds.ToString() + "ms.");

            sw.Restart();

            //这个参数需要指定每次批插入的记录数
            cmd.ArrayBindCount = sourceDataTable.Rows.Count;

            //在这个命令行中,用到了参数,参数我们很熟悉,但是这个参数在传值的时候用的是数组,而不是单个的值,这就是它独特的地方
            cmd.CommandText = sqlStr;
            //这个调用将把参数数组传进SQL,同时写入数据库
            int effect = cmd.ExecuteNonQuery();

            sw.Stop();

            Debug.WriteLine("批量插入:" + recc.ToString() + "pcs所耗时间:" + sw.ElapsedMilliseconds.ToString() + "ms.");
            return effect;
        }

        //public int BatchUpdate(string connectionString, DataTable table, int cmdTimeOut = 900, int batchSize = 1000)
        public int BatchUpdate(DbConnection dbConn, DataTable table, int batchSize = 1000)
        {
            return -1;
        }
    }
}