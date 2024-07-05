using dbe.FoundationLibrary.Core.Util;

using System;
using System.Collections.Generic;

namespace dbe.FoundationLibrary.DataAccess.IDAL
{
    public class DBClientFactory
    {
        private static readonly string path = "MyFramework";

        private static Dictionary<string, IDBClient> DBClientDict = new Dictionary<string, IDBClient>();

        public static IDBClient GetDBClient(string dbClientClassName)
        {
            if (string.IsNullOrEmpty(dbClientClassName))
            {
                dbClientClassName = "SqlServerClient";
            }

            IDBClient db = null;
            if (DBClientDict.ContainsKey(dbClientClassName))
            {
                db = DBClientDict[dbClientClassName];
            }
            else
            {
                string className = string.Format("{0}.IDAL.{1},{0}", path, dbClientClassName);
                Type type = Type.GetType(className); //取得类型
                //db = (IDBClient)Assembly.Load("HQ.Framework").CreateInstance(className);
                //db = Activator.CreateInstance(type, null) as IDBClient;
                Func<IDBClient> fun = Reflection2Instance.GetFuncByET<IDBClient>(type);     //性能最高的反射方式
                //Func<IDBClient> fun = Reflection2Instance.GetFuncByEmit<IDBClient>(type);
                db = fun();

                if (!DBClientDict.ContainsKey(dbClientClassName))
                {
                    DBClientDict.Add(dbClientClassName, db);
                }
            }
            return db;
        }
    }
}