using System;
using System.Collections.Generic;
using System.Data;

using HiCSDB;
using HiCSUtil;

using HiCSDBProvider.Impl;

namespace HiCSDBProvider
{
    /// <summary>
    /// 数据提供者有很多,此处只提供以数据库为数据源,以XML为SQL存储位置的数据提供者
    /// 注: 此处未考虑多数据库问题,以后会根据实际情况提供
    /// XuminRong 2016-06-03
    /// </summary>
    public class DBProvider
    {
        static DBOperate db = null;

        static DBOperate DB
        {
            get
            {
                if (db == null)
                {
                    db = new DBOperate(ProvidConfig.Conn, ProvidConfig.DBType);
                }
                return db;
            }
        }

        public static void Init(int dbType, string connStr, string xmlFolder)
        {
            ProvidConfig.DBType = dbType;
            ProvidConfig.Conn = connStr;
            ProvidConfig.XMLFolder = xmlFolder;
        }

        /// <summary>
        /// 执行查询
        /// </summary>
        /// <param name="id"></param>
        /// <param name="mp"></param>
        /// <returns></returns>
        public static DataTable ExecuteQuery(string id, IDictionary<string, string> mp, params object[] args)
        {
            return TryHelper.OnTry(null, () =>
            {
                SqlInfo sql = SQLHelper.GetSqlInfo(id, mp, args);
                DataTable dt = DB.ExecuteDataTable(sql.SQL, sql.Parameters);
                return ExcelDataTable(dt);
            });
        }

        /// <summary>
        /// 执行非查询语句
        /// </summary>
        /// <param name="id"></param>
        /// <param name="mp"></param>
        /// <returns></returns>
        public static int ExecuteNoQuery(string id, IDictionary<string, string> mp, params object[] args)
        {
            return TryHelper.OnTryEx(-1, () =>
            {
                SqlInfo sql = SQLHelper.GetSqlInfo(id, mp, args);
                return DB.ExecuteNonQuery(sql.SQL, sql.Parameters);
            });
        }

        public static string ExecuteScalar(string id, IDictionary<string, string> mp, params object[] args)
        {
            return TryHelper.OnTryEx("", () =>
            {
                SqlInfo sql = SQLHelper.GetSqlInfo(id, mp, args);
                return DB.ExecuteScalar(sql.SQL, sql.Parameters);
            });
        }

        /// <summary>
        /// 执行非查询SQL语句
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static int ExecuteNoQuery8SQL(string sql)
        {
            return TryHelper.OnTryEx(-1, () =>
            {
                return DB.ExecuteNonQuery(sql);
            });
        }

        /// <summary>
        /// 执行SQL语句，并取得第一行第一列
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static int ExecuteScalarInt8SQL(string sql)
        {
            return TryHelper.OnTryEx(-1, () =>
            {
                return DB.ExecuteScalar(sql);
            });
        }

        private static DataTable ExcelDataTable(DataTable dt)
        {
            if (ProvidConfig.DBType != DBOperate.OLEDB)
            {
                return dt;
            }
            DataTable dtNew = new DataTable();
            foreach (DataColumn cl in dt.Columns)
            {
                dtNew.Columns.Add(cl.ColumnName);
            }

            foreach (DataRow dr in dt.Rows)
            {
                DataRow drNew = dtNew.NewRow();

                foreach (DataColumn cl in dt.Columns)
                {
                    drNew[cl.ColumnName] = Convert.ToString(dr[cl.ColumnName]);
                }

                dtNew.Rows.Add(drNew);
            }

            return dtNew;
        }
    }
}
