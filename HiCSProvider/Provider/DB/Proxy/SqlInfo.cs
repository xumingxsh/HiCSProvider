/// <copyright>��־  1999-2006</copyright>
/// <version>1.0</version>
/// <author>Xumr</author>
/// <email>Xumr@DOTNET.com</email>
/// <log date="2006-03-10">����</log>

using System;
using System.Data.Common;

namespace HiCSProvider.DB.Impl
{
	/// <summary>
	/// �洢SQL�����Ϣ������SQL������صĲ������飩���ࡣ
	/// </summary>
	public sealed class SqlInfo
	{
		/// <summary>
		/// ���캯����
		/// </summary>
		/// <param name="sql">SQL���</param>
		/// <param name="sqlParameter">SQL��������</param>
		/// <example>���ð�����
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
		/// ���캯����
		/// </summary>
		/// <param name="sql">SQL���</param>
		/// <example>���ð�����
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
		/// SQL��䡣
		/// </summary>
		/// <example>���ð�����
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
        /// ��ҳ��ѯʱ,ȡ����������SQL���
        /// </summary>
        public string CountSQL { set; get; }

		/// <summary>
		/// SQL�������顣
		/// </summary>
		/// <example>���ð�����
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
