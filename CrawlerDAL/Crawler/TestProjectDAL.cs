using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace Crawler.DAL.Crawler
{
    public class TestProjectDAL
    {
        const string connStr2 = "server=.;database=WebDataDB;uid=sa;pwd=sa;";
        public void InserttestProject(int pid, int classid, int tid, int tstatus)
        {
            string sql = string.Format("insert into TestProject(pid,classid,tid,tstatus) values('{0}','{1}','{2}','{3}')", pid.ToString(), classid.ToString(), tid.ToString(), tstatus.ToString());

            using (SqlConnection conn = new SqlConnection(connStr2))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    conn.Open();
                    cmd.CommandText = sql;
                    cmd.CommandTimeout = 2000;
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = conn;
                    cmd.ExecuteNonQuery();
                }
            }

        }

        public void Test1()
        {
            InserttestProject(1, 3, 768, 0);
        }
    }
}
