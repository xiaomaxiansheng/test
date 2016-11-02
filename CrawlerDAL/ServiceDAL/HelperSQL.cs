using System;
using System.Collections;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Data.Common;
using System.Collections.Generic;

namespace Crawler.DAL
{
    /// <summary>
    /// ���ݷ��ʳ��������
    /// Copyright
    /// </summary>
    public abstract class HelperSQL
    {
        //���ݿ������ַ���(web.config������)�����Զ�̬����connectionString֧�ֶ����ݿ�.		
        public static string connectionString = SystemConst.ConnStr;
        public HelperSQL() { }

        public static int ExecNonQuery(string sqlStr)
        {
            return ExecNonQuery(sqlStr, null, CommandType.Text);
        }
        public static int ExecNonQuery(string sqlStr, SqlParameter[] parms)
        {
            return ExecNonQuery(sqlStr, parms, CommandType.StoredProcedure);
        }
        public static int ExecNonQuery(string sqlStr, SqlParameter[] parms, CommandType cmdType)
        {
            int result = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(sqlStr, connection))
                    {

                        cmd.Connection = connection;
                        cmd.CommandText = sqlStr;
                        cmd.CommandType = cmdType;

                        if (parms != null)
                        {
                            cmd.Parameters.AddRange(parms);
                        }
                        try
                        {
                            connection.Open();
                            result = cmd.ExecuteNonQuery();
                        }
                        catch
                        {
                            connection.Close();
                        }
                    }
                }
                return result;
            }
            catch (Exception)
            {
                return 0;
            }

        }



        public static object ExecScalar(string sqlStr)
        {
            return ExecScalar(sqlStr, null, CommandType.Text);
        }
        public static object ExecScalar(string sqlStr, SqlParameter[] parms)
        {
            return ExecScalar(sqlStr, parms, CommandType.StoredProcedure);
        }
        public static object ExecScalar(string sqlStr, SqlParameter[] parms, CommandType cmdType)
        {
            object result = null;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(sqlStr, connection))
                    {
                        cmd.Connection = connection;
                        cmd.CommandText = sqlStr;
                        cmd.CommandType = cmdType;

                        if (parms != null)
                        {
                            cmd.Parameters.AddRange(parms);
                        }
                        try
                        {
                            connection.Open();
                            result = cmd.ExecuteScalar();
                        }
                        catch
                        {
                            connection.Close();
                        }
                    }
                }
                return result;
            }
            catch (Exception)
            {
                return 0;
            }

        }

        public static DataTable SelectData(string sqlStr, SqlParameter[] parms, CommandType cmdType)
        {

            DataTable dt = new DataTable();

            try
            {

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(sqlStr, connection))
                    {
                        cmd.Connection = connection;
                        cmd.CommandText = sqlStr;
                        cmd.CommandType = cmdType;

                        if (parms != null)
                        {
                            cmd.Parameters.AddRange(parms);
                        }
                        try
                        {
                            connection.Open();
                            SqlDataAdapter dap = new SqlDataAdapter(cmd);
                            dap.Fill(dt);
                        }
                        catch
                        {
                            connection.Close();
                        }
                    }
                }
            }
            catch (Exception)
            {
                
                throw;
            }

            return dt;
        }




        public static SqlDataReader ExecuteReader(string sql)
        {
            return ExecuteReader(sql, null, CommandType.Text);
        }

        public static SqlDataReader ExecuteReader(string sql, SqlParameter[] parms)
        {
            return ExecuteReader(sql,parms,CommandType.Text);
        }

        public static SqlDataReader ExecuteReader(string sql, SqlParameter[] parms, CommandType cmdType)
        {
            try
            {
                SqlConnection connection = new SqlConnection(connectionString);

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = sql;
                cmd.CommandType = cmdType;
                if (parms != null)
                {
                    cmd.Parameters.AddRange(parms);
                }
               
                connection.Open();
                return cmd.ExecuteReader();
            }
            catch (Exception)
            {

                throw;
            }
        }




    

        /// <summary>
        /// ִ�ж���SQL��䣬ʵ�����ݿ�����
        /// </summary>
        /// <param name="SQLStringList">����SQL���</param>		
        public static int ExecuteSqlTran(List<String> SQLStringList)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                SqlTransaction tx = conn.BeginTransaction();
                cmd.Transaction = tx;
                try
                {
                    int count = 0;
                    for (int n = 0; n < SQLStringList.Count; n++)
                    {
                        string strsql = SQLStringList[n];
                        if (strsql.Trim().Length > 1)
                        {
                            cmd.CommandText = strsql;
                            count += cmd.ExecuteNonQuery();
                        }
                    }
                    tx.Commit();
                    return count;
                }
                catch
                {
                    tx.Rollback();
                    return 0;
                }
            }
        }
       
        /// <summary>
        /// �����ݿ������ͼ���ʽ���ֶ�(������������Ƶ���һ��ʵ��)
        /// </summary>
        /// <param name="strSQL">SQL���</param>
        /// <param name="fs">ͼ���ֽ�,���ݿ���ֶ�����Ϊimage�����</param>
        /// <returns>Ӱ��ļ�¼��</returns>
        public static int ExecuteSqlInsertImg(string strSQL, byte[] fs)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(strSQL, connection);
                System.Data.SqlClient.SqlParameter myParameter = new System.Data.SqlClient.SqlParameter("@fs", SqlDbType.Image);
                myParameter.Value = fs;
                cmd.Parameters.Add(myParameter);
                try
                {
                    connection.Open();
                    int rows = cmd.ExecuteNonQuery();
                    return rows;
                }
                catch (System.Data.SqlClient.SqlException e)
                {
                    throw e;
                }
                finally
                {
                    cmd.Dispose();
                    connection.Close();
                }
            }
        }

      
     

    }

}
