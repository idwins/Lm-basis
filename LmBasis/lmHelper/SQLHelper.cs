using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data.Common;
using System.Data;

/// <summary>
/// 数据库操作类
/// </summary>
public class SQLHelper
{
    #region 全局变量定义
    private static SQLHelper sqldb;
    /// <summary>
    /// 定义数据库链接字符串
    /// </summary>
    public string sqlConnStr = "";
    private SqlConnection con;
    #endregion

    #region <!------------------SQlhelper对象重载------------------->
    /// <summary>
    /// 根据默认连接字符串实例化
    /// </summary>
    public SQLHelper()
    {
        sqlConnStr = System.Configuration.ConfigurationManager.ConnectionStrings["conn_data"].ToString();
    }
    public SQLHelper(string conn)
    {
        sqlConnStr = System.Configuration.ConfigurationManager.ConnectionStrings[conn].ToString();
    }
    #endregion

    #region <!------------------返回一个SlqHelper类的实例------------------->
    /// <summary>
    /// 创建SqlHelper的对象（默认数据库链接connName=conn_data）
    /// </summary>
    /// <returns>sqlhelper实例</returns>
    public static SQLHelper CreateSQLHelper()
    {

        if (sqldb == null)
        {
            sqldb = new SQLHelper();
            return sqldb;
        }
        else
        {
            return sqldb;
        }
    }
    #endregion

    #region  <!------------------连接数据库 获得一个sqlconnection的连接 及数据库链接操作------------------->
    /// <summary>
    /// 连接数据库对象及数据库链接操作
    /// </summary>
    public SqlConnection CreateConnecton()
    {
        string connstr = sqlConnStr;
        try
        {
            con = new SqlConnection(connstr);
            ConnOpen();
        }
        catch (SqlException ex)
        {
            throw ex;
        }
        return con;
    }
    /// <summary>
    /// 打开数据库链接
    /// </summary>
    public void ConnOpen()
    {
        if (con.State != ConnectionState.Open)
        {
            con.Open();
        }
    }

    public void ConnClose()
    {
        if (con.State == ConnectionState.Open || con.State == ConnectionState.Broken)
        {
            con.Close();
            con.Dispose();
        }
        else
        {
            con.Dispose();
        }
    }
    #endregion

    #region <!------------------数据库的增、删、改更新操作------------------->
    #region <!------------------sql语句执行------------------->
    /// <summary>
    /// 执行不带参数的sql语句，更新数据库返回受影响的行数
    /// </summary>
    /// <param name="cmdText">T-SQL语句</param>
    /// <returns></returns>
    public int TSQLExecuteNonQuery(string cmdText)
    {
        return TSQLExecuteNonQuery(cmdText, null);
    }
    /// <summary>
    /// 执行带参数的sql语句，更新数据库返回受影响的行数
    /// </summary>
    /// <param name="cmdText">T-SQL语句</param>
    /// <param name="commandParameters">一组用于执行命令的参数</param>
    /// <returns></returns>
    public int TSQLExecuteNonQuery(string cmdText, params SqlParameter[] commandParameters)
    {
        int val = 0;
        try
        {
            SqlCommand cmd = new SqlCommand();
            using (SqlConnection conn = CreateConnecton())
            {
                PrepareCommand(cmd, null, CommandType.Text, cmdText, commandParameters, conn);
                val = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                return val;
            }
        }
        catch (SqlException ex)
        {
            throw ex;
        }
        return val;
    }
    #endregion

    #region <!------------------存储过程执行------------------->
    /// <summary>
    /// 执行不带参数的存储过程，更新数据库并返回受影响的行数
    /// </summary>
    /// <param name="cmdText">存储过程名称</param>
    /// <returns>返回受影响的行数</returns>
    public int PROCExecuteNonQuery(string procName)
    {
        return PROCExecuteNonQuery(procName, null, null);
    }
    /// <summary>
    /// 执行带参数的存储过程，更新数据库并返回受影响的行数
    /// </summary>
    /// <param name="cmdText">存储过程名称</param>
    /// <param name="commandParameters">一组用于执行命令的参数</param>
    /// <returns>返回受影响的行数</returns>
    public int PROCExecuteNonQuery(string procName, params SqlParameter[] commandParameters)
    {
        return PROCExecuteNonQuery(procName, 0, commandParameters);
    }
    /// <summary>
    /// 执行带参数的存储过程，更新数据库并返回受影响的行数
    /// </summary>
    /// <param name="cmdText">存储过程名称</param>
    /// <param name="times">设置执行存储过程的时间 单位（秒）</param>
    /// <param name="commandParameters">一组用于执行命令的参数</param>
    /// <returns>返回受影响的行数</returns>
    public int PROCExecuteNonQuery(string procName, int times, params SqlParameter[] commandParameters)
    {
        int val = 0;
        try
        {
            SqlCommand cmd = new SqlCommand();
            using (SqlConnection conn = CreateConnecton())
            {

                PrepareCommand(cmd, null, CommandType.StoredProcedure, procName, commandParameters, times, conn);
                val = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                return val;
            }
        }
        catch (SqlException ex)
        {
            //ConnClose();
            throw ex;
        }
        return val;
    }
    #endregion
    #endregion

    #region <!------------------数据库查询结果集操作------------------->
    #region <!------------------1、sql语句执行------------------->
    /// <summary>
    /// 执行不带参数的SQL语句，返回结果为DataTable
    /// </summary>
    /// <param name="commandText">T-SQL 命令</param>
    /// <returns>一个包含返回结果集的DataTable</returns>
    public DataTable TSQLExecuteDataTable(string cmdText)
    {
        return TSQLExecuteDataTable(cmdText, null);
    }
    /// <summary>
    /// 执行带参数的SQL语句，返回结果为DataTable
    /// </summary>
    /// <param name="commandText">T-SQL 命令</param>
    /// <param name="commandParameters">一组用于执行命令的参数</param>
    /// <returns>一个包含返回结果集的DataTable</returns>
    public DataTable TSQLExecuteDataTable(string cmdText, params SqlParameter[] commandParameters)
    {
        DataTable dtbl = new DataTable();
        try
        {
            //conn = CreateConnecton();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandTimeout = 1800;
            using (SqlConnection conn = CreateConnecton())
            {
                PrepareCommand(cmd, null, CommandType.Text, cmdText, commandParameters, conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dtbl);
                da.Dispose();
                //ConnClose();
            }
        }
        catch (SqlException ex)
        {
            //ConnClose();
            throw ex;
        }
        return dtbl;
    }
    /// <summary>
    /// 执行不带参数的SQL语句，返回结果为DataSet
    /// </summary>
    /// <param name="commandText">T-SQL 命令</param>
    /// <returns>一个包含返回结果集的DataSet</returns>
    public DataSet TSQLExecuteDataSet(string cmdText)
    {
        return TSQLExecuteDataSet(cmdText, null);
    }

    /// <summary>
    /// 执行存带返回参数储过程
    /// </summary>
    /// <param name="cmdText">T-SQL命令</param>
    /// <param name="param">参数集</param>
    /// <param name="returnname">返回参数名称</param>
    /// <returns></returns>
    public int TSQLExecuteProc(string cmdText, SqlParameter[] param, string returnname)
    {
        int number = 0;
        try
        {
            SqlCommand cmd = new SqlCommand();
            using (SqlConnection conn = CreateConnecton())
            {
                //cmd.Connection = conn;
                //cmd.CommandType = CommandType.StoredProcedure;
                //cmd.CommandText = cmdText;
                //if (param != null)
                //{
                //    foreach (SqlParameter sp in param)
                //    {
                //        cmd.Parameters.Add(sp);
                //    }
                //}
                PrepareCommand(cmd, null, CommandType.StoredProcedure, cmdText, param, 360, conn);
                SqlParameter sp1 = cmd.Parameters.Add(returnname, SqlDbType.Int);
                sp1.Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                number = Convert.ToInt32(sp1.Value);
            }
        }
        catch (Exception)
        {
            throw;
        }
        return number;
    }

    /// <summary>
    /// 执行带参数的SQL语句，返回结果为DataSet
    /// </summary>
    /// <param name="commandText">T-SQL 命令</param>
    /// <param name="commandParameters">一组用于执行命令的参数</param>
    /// <returns>一个包含返回结果集的DataSet</returns>
    public DataSet TSQLExecuteDataSet(string cmdText, params SqlParameter[] commandParameters)
    {
        DataSet ds = new DataSet();
        DataTable dt = new DataTable("0");
        try
        {
            //conn = CreateConnecton();
            SqlCommand cmd = new SqlCommand();
            using (SqlConnection conn = CreateConnecton())
            {
                PrepareCommand(cmd, null, CommandType.Text, cmdText, commandParameters, conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                da.Dispose();
                //ConnClose();

            }
        }
        catch (SqlException ex)
        {
            //ConnClose();
            throw ex;
        }
        ds.Tables.Add(dt);
        return ds;
    }
    public DataSet DataSet(string cmdText, params SqlParameter[] commandParameters)
    {
        DataSet ds = new DataSet();
        try
        {
            //conn = CreateConnecton();
            SqlCommand cmd = new SqlCommand();
            using (SqlConnection conn = CreateConnecton())
            {
                PrepareCommand(cmd, null, CommandType.Text, cmdText, commandParameters, conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                da.Dispose();
                //ConnClose();

            }
        }
        catch (SqlException ex)
        {
            //ConnClose();
            throw ex;
        }
        return ds;
    }

    /// <summary>
    /// 执行不带参数的SQL语句，返回结果为DataReader
    /// </summary>
    /// <param name="cmdText">T-SQL语句</param>
    /// <returns>返回类型为datareader的结果集</returns>
    public SqlDataReader TSQLExecuteReader(string cmdText)
    {
        return TSQLExecuteReader(cmdText, null);
    }
    /// <summary>
    /// 执行带参数的SQL语句，返回结果为DataReader
    /// </summary>
    /// <param name="cmdText">T-SQL语句</param>
    /// <param name="commandParameters">一组用于执行命令的参数</param>
    /// <returns>返回类型为datareader的结果集</returns>
    public SqlDataReader TSQLExecuteReader(string cmdText, params SqlParameter[] commandParameters)
    {
        SqlDataReader rdr = null;
        try
        {
            SqlCommand cmd = new SqlCommand();
            using (SqlConnection conn = CreateConnecton())
            {
                PrepareCommand(cmd, null, CommandType.Text, cmdText, commandParameters, conn);
                rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
                rdr.Dispose();

                return rdr;
            }
        }
        catch (Exception ex)
        {

            throw ex;
        }
    }
    /// <summary>
    /// 执行不带参数的sql语句，返回首行首列值，忽略其他行
    /// </summary>
    /// <param name="cmdText">T-SQL命令</param>
    /// <returns></returns>
    public object TSQLExecuteScalar(string cmdText)
    {
        return TSQLExecuteScalar(cmdText, null);
    }
    /// <summary>
    /// 执行带参数的sql语句，返回首行首列值，忽略其他行
    /// </summary>
    /// <param name="cmdText">T-SQL命令</param>
    /// <param name="commandParameters">一组用于执行命令的参数</param>
    /// <returns></returns>
    public object TSQLExecuteScalar(string cmdText, params SqlParameter[] commandParameters)
    {
        object obj = null;
        SqlCommand cmd = new SqlCommand();
        using (SqlConnection conn = CreateConnecton())
        {
            try
            {

                PrepareCommand(cmd, null, CommandType.Text, cmdText, commandParameters, conn);
                obj = cmd.ExecuteScalar();
                cmd.Parameters.Clear();

            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        return obj;
    }
    #endregion

    #region <!------------------2、存储过程执行------------------->
    /// <summary>
    /// 执行不带参数的存储过程，返回结果为DataTable
    /// </summary>
    /// <param name="procName">存储过程名称</param>
    /// <returns>一个包含返回结果集的DataTable</returns>
    public DataTable PROCExecuteDataTable(string procName)
    {
        return TSQLExecuteDataTable(procName, null, null);
    }
    /// <summary>
    /// 执行带参数的存储过程，返回结果为DataTable
    /// </summary>
    /// <param name="procName">存储过程名称</param>
    /// <param name="commandParameters">一组用于执行命令的参数</param>
    /// <returns>一个包含返回结果集的DataTable</returns>
    public DataTable PROCExecuteDataTable(string procName, params SqlParameter[] commandParameters)
    {
        return PROCExecuteDataTable(procName, 300, commandParameters);
    }
    /// <summary>
    /// 执行带参数的存储过程，返回结果为DataTable
    /// </summary>
    /// <param name="procName">存储过程名称</param>
    /// <param name="times">设置执行存储过程的时间 单位（秒）</param>
    /// <param name="commandParameters">一组用于执行命令的参数</param>
    /// <returns>一个包含返回结果集的DataTable</returns>
    public DataTable PROCExecuteDataTable(string procName, int times, params SqlParameter[] commandParameters)
    {
        DataTable dtbl = new DataTable();
        try
        {
            //conn = CreateConnecton();
            SqlCommand cmd = new SqlCommand();
            using (SqlConnection conn = CreateConnecton())
            {
                PrepareCommand(cmd, null, CommandType.StoredProcedure, procName, commandParameters, times, conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dtbl);
                da.Dispose();
                //ConnClose();
            }
        }
        catch (SqlException ex)
        {
            //ConnClose();
            throw ex;
        }
        return dtbl;
    }
    /// <summary>
    /// 执行带参数的存储过程，返回结果为DataTable
    /// </summary>
    /// <param name="procName">存储过程名称</param>
    /// <param name="times">设置执行存储过程的时间 单位（秒）</param>
    /// <param name="commandParameters">一组用于执行命令的参数</param>
    /// <returns>一个包含返回结果集的DataTable</returns>
    public DataTable PROCExecuteDataTable(string procName, int times, out int Total, params SqlParameter[] commandParameters)
    {
        DataTable dtbl = new DataTable();
        try
        {
            //conn = CreateConnecton();
            SqlCommand cmd = new SqlCommand();
            using (SqlConnection conn = CreateConnecton())
            {
                PrepareCommand(cmd, null, CommandType.StoredProcedure, procName, commandParameters, times, conn);
                SqlParameter tot = cmd.Parameters.Add("@TotalCount", SqlDbType.Int);
                tot.Direction = ParameterDirection.Output;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dtbl);
                Total = (int)tot.Value;
                da.Dispose();
                //ConnClose();
            }
        }
        catch (SqlException ex)
        {
            //ConnClose();
            throw ex;
        }
        return dtbl;
    }
    /// <summary>
    /// 执行不带参数的存储过程，返回结果为DataSet
    /// </summary>
    /// <param name="procName">存储过程名称</param>
    /// <returns>一个包含返回结果集的DataSet</returns>
    public DataSet PROCExecuteDataSet(string procName)
    {
        return PROCExecuteDataSet(procName, 0, null);
    }
    /// <summary>
    /// 执行带参数的存储过程，返回结果为DataSet
    /// </summary>
    /// <param name="procName">存储过程名称</param>
    /// <param name="commandParameters">一组用于执行命令的参数</param>
    /// <returns>一个包含返回结果集的DataSet</returns>
    public DataSet PROCExecuteDataSet(string procName, params SqlParameter[] commandParameters)
    {
        return PROCExecuteDataSet(procName, 0, commandParameters);
    }
    /// <summary>
    /// 执行带参数的存储过程，返回结果为DataSet
    /// </summary>
    /// <param name="procName">存储过程名称</param>
    /// <param name="times">设置执行存储过程的时间 单位（秒）</param>
    /// <param name="commandParameters">一组用于执行命令的参数</param>
    /// <returns>一个包含返回结果集的DataSet</returns>
    public DataSet PROCExecuteDataSet(string procName, int times, params SqlParameter[] commandParameters)
    {
        DataSet ds = new DataSet();
        DataTable dt = new DataTable("0");
        try
        {
            //conn = CreateConnecton();
            SqlCommand cmd = new SqlCommand();
            using (SqlConnection conn = CreateConnecton())
            {
                PrepareCommand(cmd, null, CommandType.Text, procName, commandParameters, times, conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                da.Dispose();
                //ConnClose();

            }
        }
        catch (SqlException ex)
        {
            //ConnClose();
            throw ex;
        }
        ds.Tables.Add(dt);
        return ds;
    }
    /// <summary>
    /// 执行带参数的存储过程，返回结果为DataSet
    /// </summary>
    /// <param name="procName">存储过程名称</param>
    /// <param name="commandParameters">一组用于执行命令的参数</param>
    /// <returns>一个包含返回结果集的DataSet</returns>
    public DataSet PROCExecuteDataSetALL(string procName, params SqlParameter[] commandParameters)
    {
        return PROCExecuteDataSetALL(procName, 0, commandParameters);
    }
    /// <summary>
    /// 执行带参数的存储过程，返回结果为DataSet
    /// </summary>
    /// <param name="procName">存储过程名称</param>
    /// <param name="times">设置执行存储过程的时间 单位（秒）</param>
    /// <param name="commandParameters">一组用于执行命令的参数</param>
    /// <returns>一个包含返回结果集的DataSet</returns>
    public DataSet PROCExecuteDataSetALL(string procName, int times, params SqlParameter[] commandParameters)
    {
        DataSet ds = new DataSet();
        try
        {
            //conn = CreateConnecton();
            SqlCommand cmd = new SqlCommand();
            using (SqlConnection conn = CreateConnecton())
            {
                PrepareCommand(cmd, null, CommandType.Text, procName, commandParameters, times, conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                da.Dispose();
                //ConnClose();

            }
        }
        catch (SqlException ex)
        {
            //ConnClose();
            throw ex;
        }
        return ds;
    }
    /// <summary>
    /// 执行不带参数的存储过程，返回结果为DataReader
    /// </summary>
    /// <param name="procName"></param>
    /// <returns></returns>
    public SqlDataReader PROCExecuteReader(string procName)
    {
        return PROCExecuteReader(procName, 0, null);
    }
    /// <summary>
    /// 执行带参数的存储过程，返回结果为DataReader
    /// </summary>
    /// <param name="procName">存储过程名称</param>
    /// <param name="commandParameters">一组用于执行命令的参数</param>
    /// <returns></returns>
    public SqlDataReader PROCExecuteReader(string procName, params SqlParameter[] commandParameters)
    {
        return PROCExecuteReader(procName, 0, commandParameters);
    }
    /// <summary>
    /// 执行带参数的存储过程，返回结果为DataReader
    /// </summary>
    /// <param name="procName">存储过程名称</param>
    /// <param name="times">设置存储过程执行的时间 单位（秒）</param>
    /// <param name="commandParameters">一组用于执行命令的参数</param>
    /// <returns></returns>
    public SqlDataReader PROCExecuteReader(string procName, int times, params SqlParameter[] commandParameters)
    {
        try
        {
            SqlCommand cmd = new SqlCommand();
            using (SqlConnection conn = CreateConnecton())
            {

                PrepareCommand(cmd, null, CommandType.StoredProcedure, procName, commandParameters, times, conn);
                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
                rdr.Dispose();
                //ConnClose();
                return rdr;
            }
        }
        catch (SqlException ex)
        {
            //ConnClose();
            throw ex;
        }
    }
    /// <summary>
    /// 执行不带参数的存储过程，返回结果为object
    /// </summary>
    /// <param name="procName">存储过程名称</param>
    /// <returns></returns>
    public object PROCExecuteScalar(string procName)
    {
        return PROCExecuteScalar(procName, 0, null);
    }
    /// <summary>
    /// 执行带参数的存储过程，返回结果为object
    /// </summary>
    /// <param name="procName">存储过程名称</param>
    /// <param name="commandParameters">一组用于执行sql命令的参数</param>
    /// <returns></returns>
    public object PROCExecuteScalar(string procName, params SqlParameter[] commandParameters)
    {
        return PROCExecuteScalar(procName, 0, commandParameters);
    }
    /// <summary>
    /// 执行带参数的存储过程，返回结果为object
    /// </summary>
    /// <param name="procName">存储过程名称</param>
    /// <param name="times">设置存储过程执行的时间 单位（秒）</param>
    /// <param name="commandParameters">一组用于执行sql命令的参数</param>
    /// <returns></returns>
    public object PROCExecuteScalar(string procName, int times, params SqlParameter[] commandParameters)
    {
        object obj = null;
        SqlCommand cmd = new SqlCommand();
        using (SqlConnection conn = CreateConnecton())
        {
            try
            {
                PrepareCommand(cmd, null, CommandType.StoredProcedure, procName, commandParameters, conn);
                obj = cmd.ExecuteScalar();
                cmd.Parameters.Clear();
                //ConnClose();
            }
            catch (SqlException ex)
            {
                //ConnClose();
                throw ex;
            }
        }
        return obj;
    }
    #endregion
    #endregion

    #region <!------------------其他常用的特殊数据库相关操作------------------->

    #region <!------------------1、执行多条sql语句的事物处理------------------->
    /// <summary>
    /// 执行多条sql语句的事物处理
    /// </summary>
    /// <param name="sqlList">sql语句列表</param>
    /// <returns></returns>
    public int ExecuteTranSqlList(List<String> sqlList)
    {
        using (SqlConnection conn = CreateConnecton())
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            SqlTransaction tran = conn.BeginTransaction();
            cmd.Transaction = tran;
            cmd.CommandTimeout = 600000;
            try
            {
                int count = 0;
                for (int i = 0; i < sqlList.Count; i++)
                {
                    string strsql = sqlList[i];
                    if (strsql.Trim().Length > 1)
                    {
                        cmd.CommandText = strsql;
                        count += cmd.ExecuteNonQuery();
                    }
                }
                tran.Commit();
                return count;
            }
            catch (SqlException ex)
            {
                tran.Rollback();
                ConnClose();
                return 0;
            }
        }

    }
    #endregion

    #region <!------------------1.2、执行多条sql语句的事物处理，带影响行数判断------------------->
    /// <summary>
    /// 执行多条sql带影响行数判断
    /// </summary>
    /// <param name="sqlList">sql语句列表</param>
    /// <param name="sqlList">应该影响行数</param>
    /// <returns></returns>
    public int ExecuteTranSqlList(List<String> sqlList, int YxCount)
    {
        using (SqlConnection conn = CreateConnecton())
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            SqlTransaction tran = conn.BeginTransaction();
            cmd.Transaction = tran;
            try
            {
                int count = 0;
                for (int i = 0; i < sqlList.Count; i++)
                {
                    string strsql = sqlList[i];
                    if (strsql.Trim().Length > 1)
                    {
                        cmd.CommandText = strsql;
                        count += cmd.ExecuteNonQuery();
                    }
                }//判断影响行数是否正确
                if (YxCount == count)
                {
                    tran.Commit();
                    return count;
                }
                else
                {
                    tran.Rollback();
                    ConnClose();
                    return 0;
                }
            }
            catch (SqlException ex)
            {
                tran.Rollback();
                ConnClose();
                return 0;
            }
        }

    }
    #endregion

    #region <!------------------2、判断是否存在表中的某个字段、表------------------->
    /// <summary>
    /// 判断表中是否存在某个字段
    /// </summary>
    /// <param name="colunmName">字段名</param>
    /// <param name="tableName">表名</param>
    /// <returns>true则表示存在，false表示不存在</returns>
    public bool DiscoverValidOFColumn(string columnName, string tableName)
    {
        object res = null;
        string sql = "select count(1) from syscolumns where [id]=object_id('" + tableName + "') and [name] ='" + columnName + "'";
        try
        {
            res = TSQLExecuteScalar(sql);
        }
        catch (SqlException ex)
        {
            throw ex;
        }
        if (res == null)
        {
            return false;
        }
        return Convert.ToInt32(res) > 0;
    }
    /// <summary>
    /// 检查数据表是否存在
    /// </summary>
    /// <param name="TableName">表名</param>
    /// <returns></returns>
    public bool TableIsExist(string tableName)
    {
        bool blRet = false;
        DataTable dtRet = TSQLExecuteDataTable("Select * From sysobjects Where name='" + tableName + "'");
        if (dtRet != null && dtRet.Rows.Count >= 1) { blRet = true; }
        return blRet;
    }
    #endregion

    #region <!------------------3、返回表中指定列的第一行值 object------------------->
    /// <summary>
    /// 有数据返回true
    /// </summary>
    /// <param name="cmdText">sql</param>
    /// <returns>大于0返回true</returns>
    public bool Exists(string cmdText)
    {
        return Exists(cmdText, null);
    }
    /// <summary>
    /// 有数据返回true
    /// </summary>
    /// <param name="cmdText">sql</param>
    /// <param name="commandParameters">参数</param>
    /// <returns>大于0返回true</returns>
    public bool Exists(string cmdText, params SqlParameter[] commandParameters)
    {
        ConnOpen();
        bool val;
        try
        {
            int colvalue = int.Parse(TSQLExecuteScalar(cmdText, commandParameters).ToString());
            if (colvalue > 0)
                val = true;
            else
                val = false;
        }
        catch
        {
            val = false;
        }
        finally
        {
            ConnClose();
        }

        return val;
    }
    /// <summary>
    /// 获取指表的第一行中指定列的值
    /// </summary>
    /// <param name="columnName">指定列名</param>
    /// <param name="tableName">指定表名</param>
    /// <param name="conditionsValue">sql条件语句（不包含‘where’关键字）例如‘1=1’</param>
    /// <returns></returns>
    public object GetFirstRowAndColumn(string columnName, string tableName, string conditionsValue)
    {
        string cmdText = "select " + columnName + " from " + tableName + " where " + conditionsValue;
        object obj = null;
        DataTable dt = new DataTable();
        try
        {
            //conn = CreateConnecton();
            SqlCommand cmd = new SqlCommand();
            using (SqlConnection conn = CreateConnecton())
            {
                PrepareCommand(cmd, null, CommandType.Text, cmdText, null, conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                da.Dispose();
                obj = dt.Rows[0][0];
                ConnClose();
            }
        }
        catch (SqlException ex)
        {
            ConnClose();
            throw ex;
        }
        return obj;
    }
    #endregion

    #region <!------------------4、分页，传入分页参数直接调用------------------->

    /// <summary>
    /// 分页，传入分页参数直接调用
    /// </summary>
    /// <param name="tabName">表面、可以是虚拟表</param>
    /// <param name="showColumn">显示的列，全部传入*</param>
    /// <param name="PageIndex">分页开始下标</param>
    /// <param name="PageSize">当页条数</param>
    /// <param name="strWhere">查询条件</param>
    /// <param name="StrOrder">排序条件</param>
    /// <param name="OrderType">排序类别</param>
    /// <param name="TotalCount">总条数</param>
    /// <returns></returns>
    public DataTable PROCExecutePage(string tabName, string showColumn, int PageIndex, int PageSize, string strWhere, string StrOrder, string OrderType, out int TotalCount)
    {
        SqlParameter[] parameters = {new SqlParameter("@tabName", SqlDbType.VarChar,8000),
                                    new SqlParameter("@showColumn", SqlDbType.VarChar,8000),
                                    new SqlParameter("@pageIndex", SqlDbType.Int),
                                    new SqlParameter("@PageSize", SqlDbType.Int),
                                    new SqlParameter("@strWhere", SqlDbType.VarChar,8000),
                                    new SqlParameter("@StrOrder", SqlDbType.VarChar,100) ,
                                    new SqlParameter("@OrderType", SqlDbType.VarChar,100)  };
        parameters[0].Value = tabName;
        parameters[1].Value = showColumn;
        parameters[2].Value = PageIndex;
        parameters[3].Value = PageSize;
        parameters[4].Value = strWhere;
        parameters[5].Value = StrOrder;
        parameters[6].Value = OrderType;

        return PROCExecuteDataTable("GetDataByPage", 360, out TotalCount, parameters);
    }
    #endregion

    #endregion

    #region 参数封装
    /// <summary>
    /// Prepare a command for execution
    /// </summary>
    /// <param name="cmd">SqlCommand object</param>
    /// <param name="conn">SqlConnection object</param>
    /// <param name="trans">SqlTransaction object</param>
    /// <param name="cmdType">Cmd type e.g. stored procedure or text</param>
    /// <param name="cmdText">Command text, e.g. Select * from Products</param>
    /// <param name="cmdParms">SqlParameters to use in the command</param>
    private void PrepareCommand(SqlCommand cmd, SqlTransaction trans, CommandType cmdType, string cmdText, SqlParameter[] cmdParms, SqlConnection conn)
    {

        //ConnOpen();
        if (conn.State != ConnectionState.Open)
            conn.Open();

        cmd.Connection = conn;
        cmd.CommandText = cmdText;
        if (trans != null)
            cmd.Transaction = trans;

        cmd.CommandType = cmdType;

        if (cmdParms != null)
        {
            foreach (SqlParameter parm in cmdParms)
                cmd.Parameters.Add(parm);
        }
    }
    /// <summary>
    /// 新增超时判断的重载
    /// </summary>
    /// <param name="cmd">SqlCommand object</param>
    /// <param name="conn">SqlConnection object</param>
    /// <param name="trans">SqlTransaction object</param>
    /// <param name="cmdType">Cmd type e.g. stored procedure or text</param>
    /// <param name="cmdText">Command text, e.g. Select * from Products</param>
    /// <param name="cmdParms">SqlParameters to use in the command</param>
    /// <param name="times">设置连接执行的响应时间  单位为秒</param>
    private void PrepareCommand(SqlCommand cmd, SqlTransaction trans, CommandType cmdType, string cmdText, SqlParameter[] cmdParms, int times, SqlConnection conn)
    {

        //ConnOpen();
        if (conn.State != ConnectionState.Open)
            conn.Open();

        cmd.Connection = conn;
        cmd.CommandText = cmdText;
        if (trans != null)
            cmd.Transaction = trans;
        if (times != 0)
            cmd.CommandTimeout = times;
        cmd.CommandType = cmdType;

        if (cmdParms != null)
        {
            foreach (SqlParameter parm in cmdParms)
                cmd.Parameters.Add(parm);
        }
    }
    #endregion

}

