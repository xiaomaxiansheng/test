using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Crawler.Model.XmLrc;

namespace Crawler.DAL.XmLrcDAL
{
    public class ClawLrcService
    {
        //先获取 XM_SongId

        public List<string> GetTaskSongIdList()
        {

            SqlDataReader reader = null;
            List<string> xm_SongidList = new List<string>();
            try
            {
                reader = HelperSQL.ExecuteReader("usp_XM_Spider_select_lyricTask", null, System.Data.CommandType.StoredProcedure);
                while (reader.Read())
                {
                    string xm_songId = reader["XM_SOngId"].ToString();
                    xm_SongidList.Add(xm_songId);
                }
            }
            catch (Exception ex)
            {
                LogNet.LogBLL.Error("GetTaskSongIdList", ex);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }

            return xm_SongidList;
        }

        public int UpdateXM_CN_EN_56876Service(LrcSongInfo lyricInfo)
        {
            int result = 0;
            try
            {
                SqlParameter[] parms = new SqlParameter[] { 
                  new SqlParameter("xm_songid",lyricInfo.XM_SongId),
                  new SqlParameter("lyricist",lyricInfo.Lyricist),
                  new SqlParameter("composer",lyricInfo.Composer),
                  new SqlParameter("lrctxt",lyricInfo.LrcText),
                  new SqlParameter("lyric",lyricInfo.Lyric)
                } ;

               result =  HelperSQL.ExecNonQuery("usp_XM_Spider_updateLrcBySongId",parms, System.Data.CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {

                LogNet.LogBLL.Error("UpdateXM_CN_EN_56876Service", ex);
            }

            return result;

        }

    }
}
