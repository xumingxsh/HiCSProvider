using System;
using System.Data;

namespace HiCSProvider
{
    /// <summary>
    /// 分页信息
    /// </summary>
    public class PageData
    {
        /// <summary>
        /// 符合条件的记录总行数
        /// </summary>
        public int Count { set; get; }

        /// <summary>
        /// 请求的页数
        /// </summary>
        public int PageIndex { set; get; }

        /// <summary>
        /// 每页的记录数
        /// </summary>
        public int PageSize { set; get; }

        /// <summary>
        /// 查询结果
        /// </summary>
        public DataTable Data { set; get; }

        /// <summary>
        /// SQL语句的标识
        /// </summary>
        public string SqlId { set; get; }
    }
}
