using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

using HiCSSQL;
using HiCSDB;

using HiCSProvider;

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
                SqlInfo sql = SQLHelper.GetSqlInfo(id, (string propertyName, ref object objVal) =>
                {
                    if (mp == null)
                    {
                        Debug.Assert(false, string.Format("sql(id:{0}) need paramers,but not give", id));
                    }

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
                SqlInfo info = SQLHelper.GetSqlInfo(id);
                string sql = string.Format(info.SQL, args);
                DataTable dt = DB.ExecuteDataTable(sql);
                return ExcelDataTable(dt);
            });
        }

        /// <summary>
        /// 查询分页数据,必须包含PageIndex和PageSize两个参数
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="mp"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public PageData ExecutePageData(string id, int pageIndex, int pageSize, IDictionary<string, string> mp, params object[] args)
        {
            try
            {
                SqlInfo info = GetSQLInfo(id, pageIndex,pageSize, mp);

                PageData data = new PageData();
                
                string sql = string.Format(info.CountSQL, args);
                data.Count = DB.ExecuteNonQuery(sql, info.Parameters);
                sql = string.Format(info.SQL, args);
                data.Data = DB.ExecuteDataTable(sql, info.Parameters);
                data.PageIndex = pageIndex;
                data.PageSize = pageSize;
                data.SqlId = id;
                return data;
            }
            catch (Exception ex)
            {
                HiCSUtil.HiLog.Error(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// 执行非查询语句
        /// </summary>
        /// <param name="id"></param>
        /// <param name="mp"></param>
        /// <returns></returns>
        public int ExecuteNoQuery(string id, IDictionary<string, string> mp = null)
        {
            return OnIntTry(() =>
            {
                SqlInfo info = GetSQLInfo(id, mp);
                return DB.ExecuteNonQuery(info.SQL, info.Parameters);
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
                SqlInfo info = GetSQLInfo(id, mp);
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
        private static DataTable OnDTTry(OnDTHandler evt)
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
        private static int OnIntTry(OnIntHandler evt)
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

        private static SqlInfo GetSQLInfo(string id, IDictionary<string, string> mp = null)
        {
            SqlInfo info = SQLHelper.GetSqlInfo(id, (string propertyName, ref object objVal) =>
            {
                if (mp == null)
                {
                    Debug.Assert(false, string.Format("sql(id:{0}) need paramers,but not give", id));
                }

                objVal = mp[propertyName];
                return objVal != null;
            });

            return info;
        }

        /// <summary>
        /// 取得分页的SQL信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="mp"></param>
        /// <returns></returns>
        private static SqlInfo GetSQLInfo(string id, int pageIndex, int pageSize, IDictionary<string, string> mp = null)
        {
            bool hasIndex = false;
            bool hasSize = false;
            SqlInfo info = SQLHelper.GetSqlInfo(id, (string propertyName, ref object objVal) =>
            {
                if (mp == null)
                {
                    Debug.Assert(false, string.Format("sql(id:{0}) need paramers,but not give", id));
                }

                if (propertyName.Equals("PageIndex"))
                {
                    objVal = pageIndex;
                    hasIndex = true;
                    return true;
                }

                if (propertyName.Equals("PageSize"))
                {
                    objVal = pageSize;
                    hasSize = true;
                    return true;
                }
                objVal = mp[propertyName];
                return objVal != null;
            });

            if (!hasIndex || !hasSize)
            {
                Debug.Assert(false, string.Format(
                    "sql(id:{0}) is a page request,must has 2 paramers('PageIndex', 'PageSize') but,this paramers not give", 
                    id));
            }
            if (string.IsNullOrWhiteSpace(info.CountSQL))
            {
                Debug.Assert(false, string.Format("sql(id:{0}) not a Page Request", id));
                throw new Exception("sql(id:{0}) not a Page Request");
            }
            return info;
        }
    }
}
