using System;
using System.Collections.Generic;
using System.Data;

namespace HiCSProvider.DB.Impl
{
    class RestHelperImpl : IProviderHelper
    {
        /// <summary>
        /// 执行查询
        /// </summary>
        /// <param name="id"></param>
        /// <param name="mp"></param>
        /// <returns></returns>
        public DataTable ExecuteQuery(string id, IDictionary<string, string> mp, params object[] args)
        {
            string json = RestHepler.GetRquestParams(id, mp, args);
            string ret = RestHepler.RequestOnJson("ExecuteQuery8ID", json);
            return Ret2DataTable(ret);
        }

        /// <summary>
        /// 执行非查询语句
        /// </summary>
        /// <param name="id"></param>
        /// <param name="mp"></param>
        /// <returns></returns>
        public int ExecuteNoQuery(string id, IDictionary<string, string> mp, params object[] args)
        {
            string json = RestHepler.GetRquestParams(id, mp, args);
            string ret = RestHepler.RequestOnJson("ExecuteNoQuery8ID", json);
            return ToInt(ret);
        }

        public string ExecuteScalar(string id, IDictionary<string, string> mp, params object[] args)
        {
            string json = RestHepler.GetRquestParams(id, mp, args);
            return RestHepler.RequestOnJson("ExecuteScalar8ID", json);
        }
        
        /// <summary>
        /// 执行非查询SQL语句
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public int ExecuteNoQuery8SQL(string sql)
        {
            string ret = RestHepler.RequestRest8GetOnID("Sql.ExecuteNoQuery", sql);
            return ToInt(ret);
        }

        /// <summary>
        /// 执行SQL语句，并取得第一行第一列
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public int ExecuteScalarInt8SQL(string sql)
        {
            string ret = RestHepler.RequestRest8GetOnID("Sql.ExecuteScalarInt", sql);
            return ToInt(ret);
        }

        private int ToInt(string ret)
        {
            if (string.IsNullOrWhiteSpace(ret))
            {
                return -1;
            }

            try
            {
                return Convert.ToInt32(ret);
            }
            catch (Exception ex)
            {
                ex.ToString();
                return -1;
            }
        }

        private DataTable Ret2DataTable(string ret)
        {
            ret = ret.Replace("\\", "");
            ret = ret.Substring(1, ret.Length - 2);
            return HiCSUtil.Json.Json2DataTable(ret);
        }
    }
}
