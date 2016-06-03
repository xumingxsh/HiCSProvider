using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;

using System.Web;

namespace HiCSProvider.DB.Impl
{
    class RestHepler
    {
        public static string RequestOnJson(string action, string json)
        {
            string parm = "?parms=" + HttpUtility.UrlEncode(json);
            string uri = RemoteURI + "/api/Sql/" + action + "/" + parm;
            return RequestRest8Get(uri);
        }

        public static string GetRquestParams(string id, IDictionary<string, string> mp, params object[] args)
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
            return  HiCSUtil.Json.Obj2Json(parms);

        }

        public static string RequestRest8PostOnID(string id, IDictionary<string, string> mp = null)
        {
            string uri = GetUri(id);
            return RequestRest8Post(uri, mp);
        }

        public static string RequestRest8Post(string uri, IDictionary<string, string> mp = null)
        {
            // Create the web request  
            HttpWebRequest request = WebRequest.Create(uri) as HttpWebRequest; // Get response  
            request.Method = "Post";
                 //请求超时时间  
            request.Timeout = 50000;
            request.ContentType = "application/json";// "application/x-www-form-urlencoded";
            if (mp != null && mp.Count > 0)
            {
                string parms = GetParams(mp);
                var dataArray = Encoding.UTF8.GetBytes(parms);

                request.ContentLength = dataArray.Length;
                //创建输入流  
                Stream dataStream;  
                //using (var dataStream = request.GetRequestStream())  
                //{  
                //    dataStream.Write(dataArray, 0, dataArray.Length);  
                //    dataStream.Close();  
                //}  
                try  
                {  
                    dataStream = request.GetRequestStream();  
                }  
                catch (Exception)  
                {  
                    return "";//连接服务器失败  
                }  

                //发送请求  
                dataStream.Write(dataArray, 0, dataArray.Length);  
                dataStream.Close();  
            }

            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                // Get the response stream  
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        public static string RequestRest8Get(string uri)
        {
            // Create the web request  
            HttpWebRequest request = WebRequest.Create(uri) as HttpWebRequest; // Get response  
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                // Get the response stream  
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    return reader.ReadToEnd();
                }
            }  
        }

        public static string RequestRest8GetOnID(string id, params object[] args)
        {
            string uri = GetUri(id, args);

            return RequestRest8Get(uri);
        }

        private static string GetParams(IDictionary<string, string> mp)
        {
            if (mp == null)
            {
                return null;
            }

            StringBuilder sb = new StringBuilder();
            foreach(string key in mp.Keys)
            {
                if (sb.Length > 0)
                {
                    sb.Append("&");
                }
                sb.AppendFormat("\"\":\"{0}\"", HttpUtility.UrlEncode(mp[key]));
            }
            return "{" + sb.ToString() + "}";
        }

        public static string UrlEncode(string text)
        {
            return HttpUtility.UrlEncode(text);
        }

        public static string GetParam(params object[] args)
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
        private static string GetUri(string id, params object[] args)
        {
            string param = GetParam(args);

            if (!string.IsNullOrWhiteSpace(param))
            {

                return RemoteURI + "/api/" + id.Replace(".", "/") + "/?parms=" + HttpUtility.UrlEncode(param);
            }
            return RemoteURI + "/api/" + id.Replace(".", "/") + "/";
        }

        public static string RemoteURI { set; get; }
    }
}
