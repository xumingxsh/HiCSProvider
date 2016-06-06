using System;

using HiCSDBProvider;

namespace HiCSProvider
{
    /// <summary>
    /// 用户配置类
    /// XuminRong 2016.04.15
    /// </summary>
    public static class UserConfig
    {
        /// <summary>
        /// 初始化配置参数
        /// </summary>
        /// <param name="dbType">数据库类型 1: MSSQLSERVER;2:OLEDB;4:MySQL;6:Oralce </param>
        /// <param name="conn">数据库连接字符串</param>
        /// <param name="xmlFolder">xml文件夹</param>
        public static void Init(int dbType, string conn, string xmlFolder)
        {
            try
            {
                DBProvider.Init(dbType, conn, xmlFolder);
            }
            catch(Exception ex)
            {
                HiCSUtil.HiLog.Error(ex.ToString());
            }
        }

        public static void SetUri(string uri)
        {
            HiCSProvider.DB.Impl.RestHepler.RemoteURI = uri;
        }

        public static void SetWebUri(string uri)
        {
            HiCSProvider.DB.Impl.WebProvideImpl.WebUri = uri;
        }
    }
}
