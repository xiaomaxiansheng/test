using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

using Crawler.Model.XMSpiderCollect;
using LogNet;
namespace Crawler.DAL.XMSpiderCollectDAL
{
    public class CollectService
    {

        public int InsertCollectSongInfo(CollectSongInfo collectSongInfo)
        {
            int result = 0;
            try
            {
                SqlParameter[] parms = new SqlParameter[] { 
                  new SqlParameter("showCollectId",collectSongInfo.ShowCollectId),
                  new SqlParameter("xm_SongId",collectSongInfo.XM_SongId),
                  new SqlParameter("songName",collectSongInfo.SongName),
                  new SqlParameter("singer",collectSongInfo.Singer)
                };
                result = HelperSQL.ExecNonQuery("usp_XMSpiderCollect_Insert_CollectSongInfo", parms);
            }
            catch (Exception ex)
            {
                LogBLL.Error("InsertCollectSongInfo", ex);
            }
            return result;
        }

        public int InsertLayerCollect(LayerCollect layerCollect)
        {
            int result = 0;
            try
            {
                SqlParameter[] parms = new SqlParameter[] { 
                    new SqlParameter("keyId",layerCollect.KeyId),
                    new SqlParameter("pageIndex",layerCollect.PageIndex),
                    new SqlParameter("showCollectId",layerCollect.ShowCollectId),
                    new SqlParameter("showCollectName",layerCollect.ShowCollectName),
                    new SqlParameter("tags",layerCollect.Tags),
                    new SqlParameter("playTimes",layerCollect.PalyTimes),
                    new SqlParameter("recommendTimes",layerCollect.RecommendTimes),
                    new SqlParameter("collectTimes",layerCollect.CollectTimes)
                };
                result = HelperSQL.ExecNonQuery("usp_XMSpiderCollect_Insert_LayerCollect", parms);
            }
            catch (Exception ex)
            {
                LogBLL.Error("InsertLayerCollect", ex);
            }
            return result;
        }

        public int InsertCollectCate(CollectCate collectCate)
        {
            int result = 0;
            try
            {
                SqlParameter[] parms = new SqlParameter[] { 
                    new SqlParameter("keyId",collectCate.KeyId),
                    new SqlParameter("lId",collectCate.LId),
                    new SqlParameter("cateName",collectCate.CateName),
                    new SqlParameter("pageIndex",collectCate.PageIndex),
                };
                result = HelperSQL.ExecNonQuery("usp_XMSpiderCollect_Insert_CollectCate", parms);
            }
            catch (Exception ex)
            {
                LogBLL.Error("InsertCollectCate", ex);
            }
            return result;
        }

        public int InsertCollectRank(object[] array)
        {
            int result = 0;
            try
            {

                SqlParameter[] parms = new SqlParameter[] { 
                    new SqlParameter("XM_SongId",array[0]),
                    new SqlParameter("SongName",array[1]),
                    new SqlParameter("SingerName",array[2]),
                    new SqlParameter("CollectName",array[3]),
                    new SqlParameter("Tags",array[4]),
                    new SqlParameter("CrawType",array[5]),
                    new SqlParameter("PlayTimes",array[6]),
                    new SqlParameter("RecommendTimes",array[7]),
                    new SqlParameter("CollectTimes",array[8])
                };

                result = HelperSQL.ExecNonQuery("usp_XMSpiderCollect_Insert_CollectRank", parms);
            }
            catch (Exception ex)
            {

                LogBLL.Error("InsertCollectRank", ex);
            }

            return result;

        }



        public List<int> GetCollectCateId(int startId, int endId)
        {
            string sql = "select KeyId from dbo.CollectCate where id>=" + startId + " and id<=" + endId;
            SqlDataReader reader = null;
            List<int> cateIdList = new List<int>();
            try
            {
                reader = HelperSQL.ExecuteReader(sql);
                while (reader.Read())
                {
                    int keyId = Convert.ToInt32(reader["KeyId"]);
                    cateIdList.Add(keyId);
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }

            return cateIdList;
        }

        public void UpdateCateDate(int keyId, int collectCount)
        {
            try
            {

                string sql = "update CollectCate set CollectCount='" + collectCount + "' , CreateDate='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where KeyId =" + keyId;

                HelperSQL.ExecNonQuery(sql);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        public List<int> GetCateKwyIdListService()
        {
            string sql = "select keyId from CollectCateWork where IsFinished is null order by keyId asc";

            SqlDataReader reader = null;
            List<int> cateIdList = new List<int>();
            try
            {
                reader = HelperSQL.ExecuteReader(sql);
                while (reader.Read())
                {
                    int keyId = Convert.ToInt32(reader["KeyId"]);
                    cateIdList.Add(keyId);
                }
            }
            catch (Exception ex)
            {
                LogNet.LogBLL.Error("GetCollectCateWorkInfo", ex);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }

            return cateIdList;
        }

        public int UpdateCollectCateWorkService(int keyId, int isfinished)
        {

            int result = 0;
            try
            {

                string sql = string.Format("update CollectCateWork set IsFinished='{0}',UpdateDate = '{1}' where keyId ='{2}'", isfinished, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), keyId);
                result = HelperSQL.ExecNonQuery(sql);
            }
            catch (Exception ex)
            {

                LogBLL.Error("UpdateCollectCateWork\t KeyId=" + keyId, ex);
                Console.WriteLine(ex.Message);
            }

            return result;
        }

        public int GetMaxKeyId(int num)
        {
            //得到 KeyId的 范围值

            string sql = "select max(keyId) from dbo.CollectCateWork_Row where RowId <= " + 330 * num;

            object objResult = HelperSQL.ExecScalar(sql);

            int result = Convert.ToInt32(objResult);

            return result;

        }

        public List<CollectSongReport> GetCollectSongReportListService(int fileNum, int size)
        {
            List<CollectSongReport> list = new List<CollectSongReport>();


            SqlDataReader reader = null;


            try
            {
                SqlParameter[] parms = new SqlParameter[]{
                 new SqlParameter("filenum",fileNum),
                  new SqlParameter("size",size)
                };
                reader = HelperSQL.ExecuteReader("usp_XM_Spider_exportCollectSOngReport", parms, System.Data.CommandType.StoredProcedure);
                while (reader.Read())
                {
                    CollectSongReport song = new CollectSongReport();
                    //KeyId,CateName,ShowCollectId,ShowCollectName,Tags,SongName,Singer,XM_SongId 
                    song.KeyId = reader["KeyId"].ToString();
                    song.CateName = reader["CateName"].ToString();
                    song.ShowCollectId = reader["ShowCollectId"].ToString();
                    song.ShowCollectName = reader["ShowCollectName"].ToString();
                    song.Tags = reader["Tags"].ToString();
                    song.SongName = reader["SongName"].ToString();
                    song.Singer = reader["Singer"].ToString();
                    song.XM_SongId = reader["XM_SongId"].ToString();

                    list.Add(song);
                }
            }
            catch (Exception ex)
            {

                LogBLL.Error("GetCollectSongReportListService 异常", ex);
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

        public List<string> GetListSongService()
        {
            List<string> list = new List<string>();
            SqlDataReader reader = null;
            try
            {

                string sql = "select XM_SongId from CollectSongInfo where   SongName like '%?%' or Singer like '%?%'";
                reader = HelperSQL.ExecuteReader(sql, null, System.Data.CommandType.Text);
                while (reader.Read())
                {

                    list.Add(reader["XM_SongId"].ToString());
                }
            }
            catch (Exception ex)
            {

                LogBLL.Error("GetCollectSongReportListService 异常", ex);
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

        public int UpdateSongNameByXm_SongIdService(string xm_songId, string songname, string singer)
        {
            int result = 0;
            try
            {
                 // update CollectSongInfo set SongName=N'',Singer =N'' where XM_SongId = '1769129596'
                string sql = "update CollectSongInfo set SongName=N'" + songname + "',Singer =N'" + singer + "' where XM_SongId = @xm_songId ";
               
                SqlParameter[] parms = new SqlParameter[] { 
                    new SqlParameter("xm_songId",singer)
                 };

                result =  HelperSQL.ExecNonQuery(sql, parms, System.Data.CommandType.Text);
            }
            catch (Exception ex)
            {
                LogBLL.Error("UpdateSongNameByXm_SongId 异常", ex);
            }
            return result;
        }

    }
}
