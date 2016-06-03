using System;
using System.Collections.Generic;
using System.Text;

namespace HiCSProvider
{
    /// <summary>
    /// 远程访问组合参数
    /// XuminRong 2016-06-03
    /// </summary>
    public class ParamInfo
    {
        /// <summary>
        /// 请求的标识,类似与函数名称
        /// </summary>
        public string SQL_ID { set; get; }

        /// <summary>
        /// 参数数组
        /// </summary>
        public Dictionary<string, string> Dic { set; get; }

        /// <summary>
        /// 可变参数
        /// </summary>
        public string[] Arr { set; get; }

        /// <summary>
        /// 解析参数
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static  ParamInfo Parse(string text)
        {
            try
            {
                return Parse_i(text);
            }
            catch(Exception ex)
            {
                ex.ToString();
                return null;
            }
        }

        /// <summary>
        /// 创建参数字符串
        /// </summary>
        /// <param name="id"></param>
        /// <param name="mp"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string Build(string id, IDictionary<string, string> mp, params object[] args)
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
                parms["parms"] = GetParam(args);
            }

            parms["sql_id"] = id;
            return HiCSUtil.Json.Obj2Json(parms);
        }

        private static string GetParam(params object[] args)
        {
            StringBuilder sb = new StringBuilder();
            foreach (object obj in args)
            {
                if (sb.Length > 0)
                {
                    sb.Append(",");
                }
                sb.Append(Convert.ToString(obj));
            }
            return sb.ToString();
        }

        private static ParamInfo Parse_i(string text)
        {
            ParamInfo info = new ParamInfo();

            if (string.IsNullOrWhiteSpace(text))
            {
                return null;
            }
            // 将parms还原为id,list,取出list中的parms值,作为可变参数
            Dictionary<string, string> dic = HiCSUtil.Json.Json2Obj<Dictionary<string, string>>(text);
            if (dic == null || dic.Count < 1 || !dic.ContainsKey("sql_id"))
            {
                dic = null;
                return null;
            }

            info.SQL_ID = dic["sql_id"];
            info.Arr = null;
            if (dic.ContainsKey("parms"))
            {
                info.Arr = dic["parms"].Split(',');
            }
            dic.Remove("parms");
            dic.Remove("sql_id");

            if (dic.Count < 1)
            {
                dic = null;
            }

            info.Dic = dic;
            return info;
        }
    }
}