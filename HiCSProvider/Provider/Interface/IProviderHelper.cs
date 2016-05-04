using System;
using System.Collections.Generic;
using System.Data;

namespace HiCSProvider.DB.Impl
{
    /// <summary>
    ///  数据访问通用接口
    /// </summary>
    interface IProviderHelper
    {
        /// <summary>
        /// 执行查询
        /// </summary>
        /// <param name="id"></param>
        /// <param name="mp"></param>
        /// <returns></returns>
        DataTable ExecuteQuery(string id, IDictionary<string, string> mp = null);

        /// <summary>
        /// 执行查询
        /// </summary>
        /// <param name="id"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        DataTable ExecuteQuery(string id, params object[] args);

        /// <summary>
        /// 执行非查询语句
        /// </summary>
        /// <param name="id"></param>
        /// <param name="mp"></param>
        /// <returns></returns>
        int ExecuteNoQuery(string id, IDictionary<string, string> mp = null);

        /// <summary>
        /// 执行非查询语句
        /// </summary>
        /// <param name="id"></param>
        /// <param name="mp"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        int ExecuteNoQuery(string id, IDictionary<string, string> mp, params object[] args);

        /// <summary>
        /// 执行非查询SQL语句
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        int ExecuteNoQuery8SQL(string sql);

        /// <summary>
        /// 执行SQL语句，并取得第一行第一列
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        int ExecuteScalarInt8SQL(string sql);
    }
}
