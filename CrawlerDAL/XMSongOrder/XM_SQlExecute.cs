using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.Data.SqlClient;
using Crawler.Model.XMSongOrder;


namespace Crawler.DAL.XMSongOrder
{
    public class XM_SQlExecute
    {
       
        #region Scalar
        public int Check_XM_IsFinished()
        {
            try
            {
                SqlParameter[] parms = new SqlParameter[]{
                     new  SqlParameter("startyear",AlbumStartArgsInfo.StartYear),
                     new  SqlParameter("startmonth",AlbumStartArgsInfo.StartMonth),
                     new  SqlParameter("endyear",AlbumStartArgsInfo.EndYear),
                     new  SqlParameter("endmonth",AlbumStartArgsInfo.EndMonth),
                     new  SqlParameter("spiderType",SpiderTimeInfo.SpiderType)
                 };
                object obj = HelperSQL.ExecScalar("usp_XM_Check_XM_IsFinished", parms, CommandType.StoredProcedure);
                return Convert.ToInt32(obj);
            }
            catch (Exception ex)
            {
                LogNet.LogBLL.Error("Check_XM_IsFinished", ex);
                return 0;

            }
        }

        /// <summary>
        ///  从头开始运行 得到 SpiderTimeId
        /// </summary>
        /// <returns></returns>
        public int Insert_XM_SpiderTime_A_AlbumStartArgs()
        {
            try
            {
                SqlParameter[] parms = new SqlParameter[]{
                     new  SqlParameter("startyear",AlbumStartArgsInfo.StartYear),
                     new  SqlParameter("startmonth",AlbumStartArgsInfo.StartMonth),
                     new  SqlParameter("endyear",AlbumStartArgsInfo.EndYear),
                     new  SqlParameter("endmonth",AlbumStartArgsInfo.EndMonth),
                     new SqlParameter("delaytime",AlbumStartArgsInfo.DelayTime),
                     new  SqlParameter("spiderType",SpiderTimeInfo.SpiderType)
                 };
                object obj = HelperSQL.ExecScalar("usp_XM_Insert_XM_SpiderTime_A_AlbumStartArgs", parms, CommandType.StoredProcedure);
                return Convert.ToInt32(obj);
            }
            catch (Exception ex)
            {
                LogNet.LogBLL.Error("Insert_XM_SpiderTime_A_AlbumStartArgs", ex);
                return 0;
            }
        }

        public int Update_XM_SpiderTime_A_AlbumStartArgs(int spiderTimeId)
        {
            try
            {
                SqlParameter[] parms = new SqlParameter[]{
                     new  SqlParameter("spiderTimeId",spiderTimeId)
                 };
                object obj = HelperSQL.ExecScalar("usp_XM_Update_XM_SpiderTime_A_AlbumStartArgs", parms, CommandType.StoredProcedure);
                return Convert.ToInt32(obj);
            }
            catch (Exception ex)
            {
                LogNet.LogBLL.Error("Update_XM_SpiderTime_A_AlbumStartArgs", ex);
                return 0;
            }
        }


        /// <summary>
        /// 得到专辑ID
        /// </summary>
        /// <param name="albumInfo"></param>
        /// <returns></returns>
        public int GetAlbumId(AlbumInfo albumInfo)
        {
            try
            {
                SqlParameter[] parms = new SqlParameter[]{
                     new  SqlParameter("albumname",albumInfo.AlbumName),
                     new  SqlParameter("albumsinger",albumInfo.AlbumSinger),
                     new  SqlParameter("albumdesc",albumInfo.AlbumDesc),
                     new  SqlParameter("albumstyle",albumInfo.AlbumStyle),
                     new  SqlParameter("albumtype",albumInfo.AlbumType),
                     new  SqlParameter("albumxmid",albumInfo.AlbumXmId),

                 };
                object obj = HelperSQL.ExecScalar("usp_XM_Insert_XM_AlbumTB", parms, CommandType.StoredProcedure);
                return Convert.ToInt32(obj);
            }
            catch (Exception ex)
            {
                LogNet.LogBLL.Error("GetAlbumId", ex);
                return 0;
            }

        }

        /// <summary>
        ///  得到歌手Id
        /// </summary>
        /// <param name="singerInfo"></param>
        /// <returns></returns>
        public int GetSingerId(SingerInfo singerInfo)
        {
            try
            {
                SqlParameter[] parms = new SqlParameter[]{
                     new  SqlParameter("singerName",singerInfo.SingerName),
                     new  SqlParameter("singerdesc",singerInfo.SingerDesc),
                     new  SqlParameter("singeraddress",singerInfo.SingerAddress),
                     new  SqlParameter("singerxmid",singerInfo.SingerXmId)
                 };
                object obj = HelperSQL.ExecScalar("usp_XM_Insert_XM_Singer", parms, CommandType.StoredProcedure);
                return Convert.ToInt32(obj);
            }
            catch (Exception ex)
            {
                LogNet.LogBLL.Error("GetSingerId", ex);
                return 0;
            }
        }


        public int GetAlbumYearMonthId(int spiderTimeId, int yearMonth)
        {
            try
            {
                SqlParameter[] parms = new SqlParameter[]{
                     new  SqlParameter("spiderTimeId",spiderTimeId),
                     new  SqlParameter("yearMonth",yearMonth)
                 };
                object obj = HelperSQL.ExecScalar("usp_XM_Insert_XM_AlbumYearMonth", parms, CommandType.StoredProcedure);
                return Convert.ToInt32(obj);
            }
            catch (Exception ex)
            {
                LogNet.LogBLL.Error("GetAlbumYearMonthId", ex);
                return 0;
            }
        }

        public int GetAlbumYearMonthDoneCount(int spiderTimeId)
        {
            try
            {
                SqlParameter[] parms = new SqlParameter[]{
                     new  SqlParameter("spiderTimeId",spiderTimeId)
                 };
                object obj = HelperSQL.ExecScalar("usp_XM_Select_XM_AlbumYearMonthDoneCount", parms, CommandType.StoredProcedure);
                return Convert.ToInt32(obj);
            }
            catch (Exception ex)
            {
                LogNet.LogBLL.Error("GetAlbumYearMonthDoneCount", ex);
                return 0;

            }
        }

        #endregion


        #region NonQuery

        /// <summary>
        /// 插入专辑-歌曲表
        /// </summary>
        /// <param name="albumSongInfo"></param>
        /// <param name="songNameList"></param>
        /// <returns></returns>
        public int InsertAlbumSong(AlbumSongInfo albumSongInfo)
        {
            int result = 0;
            try
            {
                for (int i = 0; i < albumSongInfo.SongNameList.Count; i++)
                {
                    SqlParameter[] parms = new SqlParameter[]{
                         new  SqlParameter("spiderTimeId",SpiderTimeInfo.SpiderTimeId),
                         new  SqlParameter("songName",albumSongInfo.SongNameList[i]),
                         new SqlParameter("xm_songid",albumSongInfo.XM_SongIdList[i]),
                         new  SqlParameter("songLanguage",albumSongInfo.SongLanguage),
                         new  SqlParameter("oriRightsHolder",albumSongInfo.OriRightsHolder),
                         new  SqlParameter("publishDate",albumSongInfo.PublishDate),
                         new  SqlParameter("fromPage",ModelArgs.FromPage),
                         new  SqlParameter("lyricTxt",albumSongInfo.LyricTxt),
                         new  SqlParameter("albumId",albumSongInfo.AlbumInfo.AlbumId),
                         new  SqlParameter("singerId",albumSongInfo.SingerInfo.SingerId)
                    };
                    result += HelperSQL.ExecNonQuery("usp_XM_Insert_XM_AlbumSong", parms, CommandType.StoredProcedure);
                }

                return result;
            }
            catch (Exception ex)
            {
                LogNet.LogBLL.Error("InsertAlbumSong", ex);
                return 0;
            }
        }

        public int Insert_XM_AlbumPage(int yearMonthId)
        {
            try
            {
                SqlParameter[] parms = new SqlParameter[]{
                     new  SqlParameter("yearMonthId",yearMonthId)
                 };
                return HelperSQL.ExecNonQuery("usp_XM_Insert_XM_AlbumPage", parms, CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                LogNet.LogBLL.Error("Insert_XM_AlbumPage", ex);
                return 0;

            }
        }
        public int Update_XM_AlbumYearMonth(int yearMonthId)
        {
            SqlParameter[] parms = new SqlParameter[]{
                     new  SqlParameter("yearMonthId",yearMonthId)
                 };
            return HelperSQL.ExecNonQuery("usp_XM_Update_XM_AlbumYearMonth", parms, CommandType.StoredProcedure);

        }

        /// <summary>
        /// 更新,当 pageInfo.PageIndex 与 pageInfo.TotalPage 相等的时候 XM_AlbumYearMonth 流程同时更新为完成状态
        /// </summary>
        /// <param name="albumPageInfo"></param>
        /// <returns></returns>
        public int Update_XM_AlbumPage_A_AlbumYearMonth(int yearMonthId, int pageIndex, bool isFinished)
        {
            try
            {
                SqlParameter[] parms = new SqlParameter[]{
                     new  SqlParameter("yearMonthId",yearMonthId),
                     new SqlParameter("pageIndex",pageIndex),
                     new SqlParameter("isfinished",isFinished)
                 };
                return HelperSQL.ExecNonQuery("usp_XM_Update_XM_AlbumPage_A_AlbumYearMonth", parms, CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                LogNet.LogBLL.Error("Update_XM_AlbumPage_A_AlbumYearMonth", ex);
                return 0;
            }
        }

        public int Insert_XM_HttpWrong(HttpWrongInfo httpWrongInfo)
        {
            try
            {
                SqlParameter[] parms = new SqlParameter[]{
                     new  SqlParameter("spiderTimeId",httpWrongInfo.SpiderTimeId),
                     new  SqlParameter("yearMonthId",httpWrongInfo.YearMonthId),
                     new SqlParameter("albumUrl",httpWrongInfo.AlbumUrl)
                 };
                return HelperSQL.ExecNonQuery("usp_XM_Insert_HttpWrong", parms, CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                LogNet.LogBLL.Error("Insert_XM_HttpWrong", ex);
                return 0;

            }
        }

        public int Update_XM_HttpWrongByWrongId(HttpWrongInfo httpWrongInfo)
        {
            try
            {
                SqlParameter[] parms = new SqlParameter[]{
                     new  SqlParameter("wrongId",httpWrongInfo.WrongId)
                 };
                return HelperSQL.ExecNonQuery("usp_XM_Update_HttpWrongByWrongId", parms, CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                LogNet.LogBLL.Error("Update_XM_HttpWrongByWrongId",ex);
                return 0;

            }
        }
        #endregion


        #region  Query

        /// <summary>
        /// 得到没做完的所有的任务
        /// </summary>
        /// <param name="albumPageList"></param>
        /// <param name="albumYearMonthList"></param>
        public void GetUnDoneBySpiderId(int spiderTimeId, ref int startPage, ref int yearMonth)
        {
            IDataReader reader = null;
            try
            {
                SqlParameter[] parms = new SqlParameter[]{
                     new  SqlParameter("spiderTimeId",spiderTimeId)
                 };
                reader = HelperSQL.ExecuteReader("usp_XM_Select_XM_AlbumYearMonthBySpiderTimeId", parms, CommandType.StoredProcedure);
                while (reader.Read())
                {

                    startPage = Convert.ToInt32(reader["PageIndx"]);

                    yearMonth = Convert.ToInt32(reader["YearMonth"]);
                }
            }
            catch (Exception ex)
            {
                LogNet.LogBLL.Error("GetUnDoneBySpiderId", ex);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        public IList<HttpWrongInfo> GetHttpWrongBySpiderTimeId(int spiderTimeId)
        {
            IDataReader reader = null;
            List<HttpWrongInfo> WrongInfoList = new List<HttpWrongInfo>();
            try
            {

                SqlParameter[] parms = new SqlParameter[]{
                     new  SqlParameter("spiderTimeId",spiderTimeId)
                 };
                reader = HelperSQL.ExecuteReader("usp_XM_Select_HttpWrongBySpiderTimeId", parms, CommandType.StoredProcedure);
                while (reader.Read())
                {
                    HttpWrongInfo httpWrongInfo = new HttpWrongInfo();
                    httpWrongInfo.WrongId = Convert.ToInt32(reader["WrongId"]);
                    httpWrongInfo.YearMonthId = Convert.ToInt32(reader["YearMonthId"]);
                    httpWrongInfo.AlbumUrl =reader["AlbumUrl"].ToString(); 
                    WrongInfoList.Add(httpWrongInfo);
                }
                return WrongInfoList;
            }
            catch (Exception ex)
            {
                LogNet.LogBLL.Error("GetHttpWrongBySpiderTimeId", ex);
                return WrongInfoList;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }
        #endregion

    }
}
