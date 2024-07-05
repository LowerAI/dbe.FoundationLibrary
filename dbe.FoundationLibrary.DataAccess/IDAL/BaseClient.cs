using System.Collections.Generic;
using System.Data.Common;

namespace dbe.FoundationLibrary.DataAccess.IDAL
{
    public abstract class BaseClient //: IDBClient
    {
        public abstract DbConnection GetDbConnection(string connectionString);

        public abstract DbCommand GetDbCommand(string cmdText);

        public abstract DbDataAdapter GetDbDataAdappter();

        public abstract DbParameter GetDbParameter();

        public abstract DbParameter GetDbParameter(string type, int? size = null);

        public abstract string GetPagingSql(string cmdText, int pageNumber, int pageSize, string orderInfo);

        public abstract int BulkToDB(DbConnection dbConn, System.Data.DataTable sourceDataTable, string targetTableName, Dictionary<string, string> mapList);
    }
}