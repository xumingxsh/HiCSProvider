using System;
using System.Collections.Generic;
using System.Data;

using HiCSSQL;
using HiCSDB;

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
        public DataTable ExecuteQuery(string id, IDictionary<string, string> mp = null)
        {
            return OnDTTry(() =>
            {
                SqlInfo sql = SQLProxy.GetSqlInfo(id, (string propertyName, ref object objVal) =>
                {
                    objVal = mp[propertyName];
                    return objVal != null;
                });
                DataTable dt = DB.ExecuteDataTable(sql.SQL, sql.Parameters);
                return ExcelDataTable(dt);
            });
        }

        /// <summary>
        /// 执行查询
        /// </summary>
        /// <param name="id"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public DataTable ExecuteQuery(string id, params object[] args)
        {
            return OnDTTry(() =>
            {
                SqlInfo info = SQLProxy.GetSqlInfo(id);
                string sql = string.Format(info.SQL, args);
                DataTable dt = DB.ExecuteDataTable(sql);
                return ExcelDataTable(dt);
            });
        }

        /// <summary>
        /// 执行非查询语句
        /// </summary>
        /// <param name="id"></param>
        /// <param name="mp"></param>
        /// <returns></returns>
        public int ExecuteNoQuery(string id, IDictionary<string, string> mp = null)
        {
            return OnIntTry(()=>{
                SqlInfo sql = SQLProxy.GetSqlInfo(id, (string propertyName, ref object objVal) =>
                {
                    objVal = mp[propertyName];
                    return objVal != null;
                });
                return DB.ExecuteNonQuery(sql.SQL, sql.Parameters);
            });
        }

        /// <summary>
        /// 执行非查询语句
        /// </summary>
        /// <param name="id"></param>
        /// <param name="mp"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public int ExecuteNoQuery(string id, IDictionary<string, string> mp, params object[] args)
        {
            return OnIntTry(()=>{
                SqlInfo info = SQLProxy.GetSqlInfo(id, (string propertyName, ref object objVal) =>
                {
                    objVal = mp[propertyName];
                    return objVal != null;
                });
                string sql = string.Format(info.SQL, args);
                return DB.ExecuteNonQuery(sql, info.Parameters);
            });
        }

        /// <summary>
        /// 执行非查询SQL语句
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public int ExecuteNoQuery8SQL(string sql)
        {
            return OnIntTry(()=>{
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
            return OnIntTry(()=>{
                object obj = DB.ExecuteScalar(sql);
                if (obj is DBNull || obj == null)
                {
                    return -1;
                }
                return Convert.ToInt32(obj);
            });
        }


        delegate DataTable OnDTHandler();
        private DataTable OnDTTry(OnDTHandler evt)
        {
            try
            {
                return evt();
            }
            catch(Exception ex)
            {
                HiCSUtil.HiLog.Error(ex.ToString());
                return null;
            }
        }

        delegate int OnIntHandler();
        private int OnIntTry(OnIntHandler evt)
        {
            try
            {
                return evt();
            }
            catch (Exception ex)
            {
                HiCSUtil.HiLog.Error(ex.ToString());
                return -1;
            }
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
