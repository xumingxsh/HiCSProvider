using System;
using System.Collections.Generic;
using System.Data;

using HiCSSQL;
using HiCSDB;
using HiCSUtil;

namespace HiCSProvider.DB.Impl
{
    class DBHelperImpl : IProviderHelper
    {
        DBOperate db = null;

        DBOperate DB
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

        /// <summary>
        /// 执行查询
        /// </summary>
        /// <param name="id"></param>
        /// <param name="mp"></param>
        /// <returns></returns>
        public DataTable ExecuteQuery(string id, IDictionary<string, string> mp, params object[] args)
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
        public int ExecuteNoQuery(string id, IDictionary<string, string> mp, params object[] args)
        {
            return TryHelper.OnTryEx(-1, () =>
            {
                SqlInfo sql = SQLHelper.GetSqlInfo(id, mp, args);
                return DB.ExecuteNonQuery(sql.SQL, sql.Parameters);
            });
        }

        public string ExecuteScalar(string id, IDictionary<string, string> mp, params object[] args)
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
        public int ExecuteNoQuery8SQL(string sql)
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
        public int ExecuteScalarInt8SQL(string sql)
        {
            return TryHelper.OnTryEx(-1, () =>
            {
                return DB.ExecuteScalar(sql);
            });
        }

        private DataTable ExcelDataTable(DataTable dt)
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
