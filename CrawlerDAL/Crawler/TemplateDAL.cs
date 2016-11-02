using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
namespace Crawler.DAL.Crawler
{
    public class TemplateDAL
    {

        const string connStr1 = "server=.;database=DataMiningDB;uid=sa;pwd=sa;";
        public void ReadTemplateService(int tid,int cateId)
        {

            //传进来一个Tid, 读取这个 Id 相关的 站点

            string sql = "select SiteId,SiteName from  SiteList where tid =" + tid;

            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(connStr1))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    conn.Open();
                    cmd.CommandText = sql;
                    cmd.CommandTimeout = 2000;
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = conn;
                    

                    SqlDataAdapter dap = new SqlDataAdapter(cmd);
                    dap.Fill(dt);
                }
            }


            if (dt.Rows.Count>0)
            {

                foreach (DataRow row in dt.Rows)
                {

                    int siteId = Convert.ToInt32(row["SiteId"]);
                    string siteName = row["SiteName"].ToString();

                    InsertCateSiteRelationService(siteId, siteName, cateId);

                }
            }

        }


        const string connStr2 = "server=.;database=WebDataDB;uid=sa;pwd=sa;";
        public void InsertCateSiteRelationService(int SiteId, string SiteName, int cateId)
        {

            string sql = string.Format("insert into CategorySiteRelation(SiteId,SiteName,CateId,CreateData) values('{0}','{1}','{2}','{3}')", SiteId.ToString(), SiteName, cateId.ToString(), DateTime.Now.ToString("yyyy-MM-dd HH:ss:mm"));

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
            ReadTemplateService(768,3);
        }
    }
}
