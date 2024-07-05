using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.IO;

namespace dbe.FoundationLibrary.DataAccess.IDAL
{
    public class OleDbClient : IDBClient
    {
        public DbConnection GetDbConnection(string connectionString)
        {
            return new OleDbConnection(connectionString);
        }

        public DbCommand GetDbCommand(string cmdText)
        {
            return new OleDbCommand(cmdText);
        }

        public DbDataAdapter GetDbDataAdappter()
        {
            return new OleDbDataAdapter();
        }

        public DbParameter GetDbParameter()
        {
            return new OleDbParameter();
        }

        public DbParameter GetDbParameter(string type, int? size = null)
        {
            OleDbParameter para = new OleDbParameter();
            para.OleDbType = OleDbType.VarWChar;  //(37种)
            return para;
        }

        public string GetPagingSql(string cmdText, int pageNumber, int pageSize, string orderInfo)
        {
            return "";
        }

        public static string GetConnectionString(string excelFilePath, string xlsVer = "Excel 12.0", string hdr = "yes", string imex = "1", string fmt = "")
        {
            //string provider = "Microsoft.Jet.OLEDB.4.0";
            //if (excelFilePath.EndsWith(".xlsx"))
            //{
            //    provider = "Microsoft.ACE.OLEDB.12.0";
            //    xlsVer = "Excel 12.0";
            //}

            string provider = "Microsoft.ACE.OLEDB.12.0";
            if (excelFilePath.EndsWith(".xls"))
            {
                provider = "Microsoft.Jet.OLEDB.4.0";
                xlsVer = "Excel 8.0";
            }
            else if (excelFilePath.EndsWith(".csv"))
            {
                excelFilePath = Directory.GetParent(excelFilePath).FullName;
                xlsVer = "TEXT";
                fmt = ";FMT=Delimited";
            }

            //2003（Microsoft.Jet.Oledb.4.0）
            //2007（Microsoft.ACE.OLEDB.12.0）
            // csv file:HDR=Yes-- first line is header
            string strConn = string.Format("Provider={0};Data Source={1};Extended Properties='{2};HDR={3};IMEX={4}{5}'", provider, excelFilePath, xlsVer, hdr, imex, fmt);
            return strConn;
        }

        public int BulkInsert(DbConnection dbConn, DataTable sourceDataTable, string targetTableName, Dictionary<string, string> mapList)
        {
            return -1;
        }

        public int BatchUpdate(DbConnection dbConn, DataTable table, int batchSize = 1000)
        {
            return -1;
        }
    }
}