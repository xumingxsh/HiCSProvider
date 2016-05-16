using System;

using HiCSSQL;

namespace HiCSProvider
{
    /// <summary>
    /// 数据库相关配置类
    /// </summary>
    class ProvidConfig
    {
        /// <summary>
        /// 数据库类型
        /// </summary>
        public static int DBType { set; get; }

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public static string Conn { set; get; }


        static string xmlFolder = "";

        /// <summary>
        /// 存储SQL的XML所在文件夹
        /// </summary>
        public static string XMLFolder
        {
            set
            {
                xmlFolder = value;
                HiCSSQL.HiLog.SetLogFun((script) =>
                {
                    HiCSUtil.HiLog.Error(script);
                });
                SQLProxy.LoadXMLs(xmlFolder);
            }
        }
    }
}
