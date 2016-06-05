using System;
using System.Collections.Generic;
using System.Data;

using HiCSUtil;

using HiCSDBProvider;

namespace HiCSProvider.DB.Impl
{
    class DBHelperImpl : IProviderHelper
    {
        /// <summary>
        /// 执行查询
        /// </summary>
        /// <param name="id"></param>
        /// <param name="mp"></param>
        /// <returns></returns>
        public DataTable ExecuteQuery(string id, IDictionary<string, string> mp, params object[] args)
        {
            return DBProvider.ExecuteQuery(id, mp, args);
        }

        /// <summary>
        /// 执行非查询语句
        /// </summary>
        /// <param name="id"></param>
        /// <param name="mp"></param>
        /// <returns></returns>
        public int ExecuteNoQuery(string id, IDictionary<string, string> mp, params object[] args)
        {
            return DBProvider.ExecuteNoQuery(id, mp, args);
        }

        public string ExecuteScalar(string id, IDictionary<string, string> mp, params object[] args)
        {
            return DBProvider.ExecuteScalar(id, mp, args);
        }
        
        /// <summary>
        /// 执行非查询SQL语句
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public int ExecuteNoQuery8SQL(string sql)
        {
            return DBProvider.ExecuteNoQuery8SQL(sql);
        }

        /// <summary>
        /// 执行SQL语句，并取得第一行第一列
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public int ExecuteScalarInt8SQL(string sql)
        {
            return DBProvider.ExecuteScalarInt8SQL(sql);
        }
    }
}
