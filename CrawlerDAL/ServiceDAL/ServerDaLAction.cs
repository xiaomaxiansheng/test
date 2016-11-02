using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;

using System.Diagnostics;
using LogNet;
namespace Crawler.DAL
{
    public class ServerDaLAction
    {
        #region 调用例子
        public int Test1()
        {
            int result = 0;
            try
            {
                SqlParameter[] parms = new SqlParameter[] { 
                  
                };

                object obj = HelperSQL.ExecScalar("usp_mining_Insert_Tag", parms, CommandType.StoredProcedure);
                result = Convert.ToInt32(obj);
            }
            catch (Exception ex)
            {
              
                 LogBLL.Error("InsertTagInfoService 获取数据异常", ex);
            }
            return result;
        }
        
         public int Test2(int tagId,string tagName,string secondTag,string koreanTranslate)
        {
            int result = 0;
            try
            {
                SqlParameter[] parms = new SqlParameter[] { 
                    new SqlParameter("tagId",tagId),
                    new SqlParameter("tagName",tagName),
                    new SqlParameter("secondTag",secondTag),
                    new SqlParameter("koreanTranslate",koreanTranslate)
                };

                result = HelperSQL.ExecNonQuery("usp_mining_Update_TagByTagId", parms, CommandType.StoredProcedure);
              
            }
            catch (Exception ex)
            {
                LogBLL.Error("UpdateTagInfoByTagIdService 获取数据异常", ex);
            }
            return result;
        }

         public List<string> Test3()
         {
             List<string> list = new List<string>();
             SqlDataReader reader = null;
             try
             {
                 reader = HelperSQL.ExecuteReader("usp_mining_Select_ProjectListRunStatus", null, CommandType.StoredProcedure);
                 while (reader.Read())
                 {

                 }
                 return list;
             }
             catch (Exception ex)
             {
                 LogBLL.Error("GetProjectByRunStatusService 异常", ex);
             }
             finally
             {
                 if (reader != null)
                 {
                     reader.Close();
                 }
             }
             return list;
         }
        #endregion 调用例子

    }
}
