using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Timers;

namespace dbe.FoundationLibrary.DataAccess.IDAL
{
    //参考链接：
    //C# 线程手册 第三章 使用线程 实现一个数据库连接池(实战篇) - DanielWise - 博客园
    //http://www.cnblogs.com/danielWise/archive/2012/02/18/2357252.html

    /// <summary>
    /// 数据库连接池
    /// </summary>
    public sealed class DBConnectionPool
    {
        // Creates an syn object.
        //private static readonly object SynObject = new object();

        //public static DBConnectionPool _instance = null;
        //public static DBConnectionPool Instance
        //{
        //    get
        //    {
        //        if (null == _instance)
        //        {
        //            lock (SynObject)
        //            {
        //                if (null == _instance)
        //                {
        //                    _instance = new DBConnectionPool();
        //                }
        //            }
        //        }
        //        return _instance;
        //    }
        //}

        //Last Checkout time of any object from the pool.
        private long lastCheckOut;

        /// <summary>
        /// 已签出(锁定)的"Id/MyConn"组成的键值对
        /// </summary>
        private Hashtable locked;

        /// <summary>
        /// 有效(剩余)的"Id/MyConn"组成的键值对
        /// </summary>
        private Hashtable unlocked;

        //Clean-Up interval
        private static long GARBAGE_INTERVAL = 90 * 1000; //90 seconds

        private IDBClient DBClient = null;

        public DBConnectionPool()
        {
            locked = Hashtable.Synchronized(new Hashtable());
            unlocked = Hashtable.Synchronized(new Hashtable());

            lastCheckOut = DateTime.Now.Ticks;

            //Create a Time to track the expired objects for cleanup.
            Timer aTimer = new Timer();
            aTimer.Enabled = true;
            aTimer.Interval = GARBAGE_INTERVAL;
            aTimer.Elapsed += new ElapsedEventHandler(CollectGarbage);
        }

        private MyConn Create(ConnectionStringSettings css)
        {
            DBClient = DBClientFactory.GetDBClient(css.ProviderName);
            DbConnection conn = DBClient.GetDbConnection(css.ConnectionString);
            MyConn mc = new MyConn();
            mc.conn = conn;
            return mc;
        }

        private bool Validate(object key)
        {
            try
            {
                //DbConnection conn = (DbConnection)o;
                //return !conn.State.Equals(ConnectionState.Closed);
                return !locked.ContainsKey(key);  //暂时不需要什么条件来验证
            }
            catch (DbException)
            {
                return false;
            }
        }

        private void Expire(DbConnection conn, bool isRelease)
        {
            try
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
                if (isRelease)
                {
                    conn.Dispose();
                    conn = null;
                }
            }
            catch (DbException) { }
        }

        /// <summary>
        /// 从连接池取出DbConnection实例
        /// </summary>
        /// <param name="css"></param>
        /// <returns></returns>
        public DbConnection GetConnectionFromPool(ConnectionStringSettings css)
        {
            string key = css.Name;
            long now = DateTime.Now.Ticks;
            lastCheckOut = now;
            MyConn mc = null;

            lock (this)
            {
                if (locked.ContainsKey(key))
                {
                    mc = locked[key] as MyConn;
                    mc.ticks = now;
                }
                else if (unlocked.ContainsKey(key))
                {
                    mc = unlocked[key] as MyConn;
                    unlocked.Remove(key);
                    mc.ticks = now;
                    locked.Add(key, mc);
                }
                else
                {
                    mc = Create(css);
                    mc.ticks = now;
                    locked.Add(key, mc);
                }
            }
            return mc == null ? null : mc.conn;
        }

        /// <summary>
        /// 归还DbConnection实例到连接池
        /// </summary>
        /// <param name="key"></param>
        public void ReturnConnectionToPool(string key)
        {
            if (locked.ContainsKey(key))
            {
                lock (this)
                {
                    MyConn mc = locked[key] as MyConn;
                    locked.Remove(key);
                    mc.ticks = DateTime.Now.Ticks;
                    unlocked.Add(key, mc);
                }
            }
        }

        private void CollectGarbage(object sender, ElapsedEventArgs e)
        {
            lock (this)
            {
                long now = DateTime.Now.Ticks;
                IDictionaryEnumerator ide = unlocked.GetEnumerator();

                try
                {
                    while (ide.MoveNext())
                    {
                        MyConn mc = ide.Value as MyConn;
                        if ((now - (long)mc.ticks) > GARBAGE_INTERVAL)
                        {
                            unlocked.Remove(ide.Key);
                            Expire(mc.conn, true);
                            mc = null;
                        }
                    }
                }
                catch (Exception) { }
            }
        }
    }

    internal class MyConn
    {
        /// <summary>
        /// 数据库连接对象
        /// </summary>
        public DbConnection conn { get; set; }

        /// <summary>
        /// 计时周期数
        /// </summary>
        public long ticks { get; set; }
    }
}