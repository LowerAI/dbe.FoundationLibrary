using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace dbe.FoundationLibrary.DataAccess.IDAL
{
    public interface IDBClient
    {
        DbConnection GetDbConnection(string connectionString);

        DbCommand GetDbCommand(string cmdText);

        DbDataAdapter GetDbDataAdappter();

        DbParameter GetDbParameter();

        DbParameter GetDbParameter(string type, int? size = null);

        string GetPagingSql(string cmdText, int pageNumber, int pageSize, string orderInfo);

        int BulkInsert(DbConnection dbConn, DataTable sourceDataTable, string targetTableName, Dictionary<string, string> mapList);

        int BatchUpdate(DbConnection dbConn, DataTable table, int batchSize = 1000);
    }
}