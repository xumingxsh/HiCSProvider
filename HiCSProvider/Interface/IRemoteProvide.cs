using System;
using System.Collections.Generic;
using System.Data;

using HiCSUtil;

namespace HiCSProvider
{
    /// <summary>
    /// 远程数据提供接口
    /// XuminRong 2016-06-03
    /// 注: 在很多时候,用户希望自己定制实现,则可以实现该接口并替换实现接口即可.
    /// eg:
    /// </summary>
    public interface IRemoteProvide
    {
        /// <summary>
        /// 执行无返回值操作
        /// </summary>
        /// <param name="parms"></param>
        /// <returns>-1:失败,其他:影响行数</returns>
        int ExecuteNoQuery(string id, IDictionary<string, string> mp, params string[] args);
        
        /// <summary>
        /// 获得DataTable
        /// </summary>
        /// <param name="parms"></param>
        /// <returns></returns>
        DataTable ExecuteQuery(string id, IDictionary<string, string> mp, params string[] args);

        /// <summary>
        /// 获得一个字符串
        /// </summary>
        /// <param name="parms"></param>
        /// <returns></returns>
        string ExecuteScalar(string id, IDictionary<string, string> mp, params string[] args);
    }
}
