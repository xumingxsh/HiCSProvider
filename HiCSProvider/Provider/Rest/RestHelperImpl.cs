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
        public DataTable ExecuteQuery(string id, IDictionary<string, string> mp = null)
        {
            Dictionary<string, string> parms = new Dictionary<string, string>();
            if (mp != null)
            {
                foreach(string key in mp.Keys)
                {
                    if (!parms.ContainsKey(key))
                    {
                        parms.Add(key, mp[key]);
                    }
                }
            }

            parms["sql_id"] = id;
            string json = HiCSUtil.Json.Obj2Json(parms);
            string ret = RestHepler.RequestOnJson("ExecuteQuery8ID", json);
            return Ret2DataTable(ret);
        }

        /// <summary>
        /// 执行查询
        /// </summary>
        /// <param name="id"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public DataTable ExecuteQuery(string id, params object[] args)
        {
            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms["sql_id"] = id;

            if (args.Length > 0)
            {
                parms["parms"] = RestHepler.GetParam(args);
            }

            string json = HiCSUtil.Json.Obj2Json(parms);
            string ret = RestHepler.RequestOnJson("ExecuteQuery8ID", json);
            return Ret2DataTable(ret);
        }

        /// <summary>
        /// 执行非查询语句
        /// </summary>
        /// <param name="id"></param>
        /// <param name="mp"></param>
        /// <returns></returns>
        public int ExecuteNoQuery(string id, params object[] args)
        {
            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms["sql_id"] = id;

            if (args.Length > 0)
            {
                parms["parms"] = RestHepler.GetParam(args);
            }

            string json = HiCSUtil.Json.Obj2Json(parms);
            string ret = RestHepler.RequestOnJson("ExecuteNoQuery8ID", json);
            return ToInt(ret);
        }

        public int ExecuteNoQuery(string id, IDictionary<string, string> mp)
        {
            Dictionary<string, string> parms = new Dictionary<string, string>();
            if (mp != null)
            {
                foreach (string key in mp.Keys)
                {
                    if (!parms.ContainsKey(key))
                    {
                        parms.Add(key, mp[key]);
                    }
                }
            }

            parms["sql_id"] = id;
            string json = HiCSUtil.Json.Obj2Json(parms);
            string ret = RestHepler.RequestOnJson("ExecuteNoQuery8ID", json);
            return ToInt(ret);
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
            Dictionary<string, string> parms = new Dictionary<string, string>();
            if (mp != null)
            {
                foreach (string key in mp.Keys)
                {
                    if (!parms.ContainsKey(key))
                    {
                        parms.Add(key, mp[key]);
                    }
                }
            }
            if (args.Length > 0)
            {
                parms["parms"] = RestHepler.GetParam(args);
            }

            parms["sql_id"] = id;
            string json = HiCSUtil.Json.Obj2Json(parms);
            string ret = RestHepler.RequestOnJson("ExecuteNoQuery8ID", json);
            return ToInt(ret);
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
