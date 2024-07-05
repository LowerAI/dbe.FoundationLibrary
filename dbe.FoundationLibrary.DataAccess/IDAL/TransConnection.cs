using System.Data.Common;

namespace dbe.FoundationLibrary.DataAccess.IDAL
{
    /// <summary>
    /// 连接的事物控制类
    /// </summary>
    internal class TransConnection
    {
        public TransConnection()
        {
            this.Deeps = 0;
        }

        public DbTransaction DBTransaction { get; set; }

        public int Deeps { get; set; }
    }
}