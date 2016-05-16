/// <copyright>天志  1999-2006</copyright>
/// <version>1.0</version>
/// <author>Xumr</author>
/// <email>Xumr@DOTNET.com</email>
/// <log date="2006-03-10">创建</log>

using System;
using System.Data.Common;

namespace HiCSProvider.DB.Impl
{
	/// <summary>
	/// 存储SQL语句信息（包括SQL语句和相关的参数数组）的类。
	/// </summary>
	public sealed class SqlInfo
	{
		/// <summary>
		/// 构造函数。
		/// </summary>
		/// <param name="sql">SQL语句</param>
		/// <param name="sqlParameter">SQL参数数组</param>
		/// <example>调用案例：
		/// <code>
		/// string sql = "SELECT * FROM Table WHERE ColumnName = @ColumnName";
		/// IDataParameter[] parameters = 
		///    {
		///       new SqlParameter("@ColumnName", "value")
	    ///     };
		/// SqlInfo info = new SqlInfo(sql, parameters);
		/// </code>
		/// </example>
        public SqlInfo(string sql, DbParameter[] sqlParameter)
		{
            SQL = sql;
            Parameters = sqlParameter;
		}

		/// <summary>
		/// 构造函数。
		/// </summary>
		/// <param name="sql">SQL语句</param>
		/// <example>调用案例：
		/// <code>
		/// string sql = "SELECT * FROM Table WHERE ColumnName = @ColumnName";
		/// SqlInfo info = new SqlInfo(sql);
		/// </code>
		/// </example>
		public SqlInfo(string sql)
		{
            SQL = sql;
		}

		/// <summary>
		/// SQL语句。
		/// </summary>
		/// <example>调用案例：
		/// <code>
		/// SqlInfo info = 
		///     new SqlInfo(
		///      "SELECT * FROM Table WHERE ColumnName = @ColumnName",
		///       { new SqlParameter("@ColumnName", "value")});
		///  Response.Write(info.SQL);
		/// </code>
		/// </example>
        public string SQL { set; get; }

        /// <summary>
        /// 分页查询时,取得行总数的SQL语句
        /// </summary>
        public string CountSQL { set; get; }

		/// <summary>
		/// SQL参数数组。
		/// </summary>
		/// <example>调用案例：
		/// <code>
		/// SqlInfo info =
		///  new SqlInfo(
		///    "SELECT * FROM Table WHERE ColumnName = @ColumnName",
		///    { new SqlParameter("@ColumnName", "value")});
		///  IDataParameter[] parameters = info.
		/// </code>
		/// </example>
        public DbParameter[] Parameters { set; get; }
	}
}
