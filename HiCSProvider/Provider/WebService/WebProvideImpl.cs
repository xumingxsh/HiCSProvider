using System;
using System.Collections.Generic;
using System.Data;

using HiCSProvider.WService;
using HiCSUtil;

namespace HiCSProvider.DB.Impl
{
    class WebProvideImpl : IProviderHelper
    {
        public DataTable ExecuteQuery(string id, IDictionary<string, string> mp, params object[] args)
        {
            string arg = ParamInfo.Build(id, mp, args);
            return Provide.ExecuteQuery(arg);
        }


        public int ExecuteNoQuery(string id, IDictionary<string, string> mp, params object[] args)
        {
            string arg = ParamInfo.Build(id, mp, args);
            return Provide.ExecuteNoQuery(arg);
        }

        public string ExecuteScalar(string id, IDictionary<string, string> mp, params object[] args)
        {
            string arg = ParamInfo.Build(id, mp, args);
            return Provide.ExecuteScalar(arg);
        }

        public int ExecuteNoQuery8SQL(string sql)
        {
            throw new NotImplementedException("this function not support");
        }

        public int ExecuteScalarInt8SQL(string sql)
        {
            throw new NotImplementedException("this function not support");
        }

        CommonProvide provide = null;
        public  CommonProvide Provide
        {
            get
            {
                if (provide == null)
                {
                    provide = new CommonProvide();
                    provide.Url = WebProvideImpl.WebUri;
                }
                return provide;
            }
        }

        public static string WebUri = null;
    }
}
