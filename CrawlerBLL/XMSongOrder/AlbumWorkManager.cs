
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using log4net.Config;
using log4net;


using Crawler.DAL.XMSongOrder;
using IBussiness;
using Crawler.Model.XMSongOrder;
namespace Crawler.BLL.XMSongOrder
{
    public class AlbumWorkManager
    {

        XM_SQlExecute sqlExecute = new XM_SQlExecute();

        public void ClawWorkOneMonth(int yearMonth, int startPage = 1)
        {

            int yearMonthId = sqlExecute.GetAlbumYearMonthId(SpiderTimeInfo.SpiderTimeId, yearMonth);
            if (yearMonthId == 0)
            {
                return;
            }
            else if (startPage < 1)
            {
                //插入初始化的 AlbumPage
                sqlExecute.Insert_XM_AlbumPage(yearMonthId);
            }
            int year = 0;
            int month = 0;
            CommonHelper.FormatYearMonth(yearMonth, out year, out month);
            //这个时候 要插入啊 usp_XM_Insert_XM_AlbumPage

            try
            {
                string strMainSite = "http://www.xiami.com/album/list?type=" + ModelArgs.AlbumListType + "&year=" + year + "&month=" + month + "&p=";
                string contentHtml = HttpHelper.Open(strMainSite, Encoding.GetEncoding("UTF-8"));
                if (contentHtml.Equals(string.Empty) || !contentHtml.Contains("p_redirect_l"))
                {
                    //为空,或者不包含下一页 即返回
                    return;
                }
                bool isFinished = false;
                int pageIndex = startPage > 1 ? startPage : 1;
                while (!isFinished)
                {
                    //循环抓取某一年月 的数据  这里循环的是 页数
                    //如果过去不到下一页的数据,即返回false跳出循环
                    ModelArgs.FromPage = yearMonthId + "_" + pageIndex;
                    isFinished = SpiderOneAlbumPage(year, month, pageIndex); //为false 表示本月还没有跑完
                    sqlExecute.Update_XM_AlbumPage_A_AlbumYearMonth(yearMonthId, pageIndex, isFinished);
                    pageIndex++;
                }
            }
            catch (Exception ex)
            {
                LogNet.LogBLL.Info("ClawWorkOneMonth", ex);
            }
        }


        /// <summary>
        /// 某年某月,某页,某语种 的所有专辑
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="pageInfo"></param>
        public bool SpiderOneAlbumPage(int year, int month, int pageIndex)
        {
            Thread.Sleep(AlbumStartArgsInfo.DelayTime);

            bool isFinished = false;
            try
            {


                object[] objParms = new object[] { 
               ModelArgs.AlbumListType, year, month, pageIndex
            };
                //http://www.xiami.com/album/list/type/huayu/year/2012/month/5/p//page/1
                string siteUrl = string.Format("http://www.xiami.com/album/list/type/{0}/year/{1}/month/{2}/p//page/{3}", objParms);
                string strContentHtml = HttpHelper.Open(siteUrl, Encoding.GetEncoding("UTF-8"));
                strContentHtml = strContentHtml.Replace("\r\n", "").Replace("\r", "").Replace("\n", "");
                if (strContentHtml.Equals(string.Empty) || strContentHtml.Contains("没有该时期发行的专辑"))
                {
                    isFinished = true;  //本月结束
                }


                strContentHtml = CommonHelper.GetMatchRegexFull("<div class=\"info\">(.+?)</div>", strContentHtml);


                List<string> albumMatchList = CommonHelper.MatchRegexList(strContentHtml, "<p><strong><a href=\"/album/(\\d+)\">[^@]{1,100}</a></strong></p>");

                for (int i = 0; i < albumMatchList.Count; i++)
                {
                    albumMatchList[i] = "http://www.xiami.com" + CommonHelper.MatchRegexFirst("/album/(\\d+)",albumMatchList[i]);

                    bool isHttpWrong = GetOneAlbumAllSong(albumMatchList[i]);
                    if (!isHttpWrong)
                    {
                        HttpWrongInfo httpWrongInfo = new HttpWrongInfo();
                        httpWrongInfo.SpiderTimeId = SpiderTimeInfo.SpiderTimeId;
                        httpWrongInfo.AlbumUrl = albumMatchList[i];
                        //#xiang这里要重新爬// 年,将之记录到数据库
                        sqlExecute.Insert_XM_HttpWrong(httpWrongInfo);
                    }
                }
            }
            catch (Exception ex)
            {

                isFinished = false;
                LogNet.LogBLL.Error("SpiderOneAlbumPage", ex);
            }
            return isFinished;
        }

        /// <summary>
        /// 得到一个专辑下面的所有的个歌曲
        /// </summary>
        /// <param name="albunNameUrl"> /album/519075 专辑相对Url路径</param>
        /// <returns></returns>
        public bool GetOneAlbumAllSong(string strUrl)
        {
            Thread.Sleep(AlbumStartArgsInfo.DelayTime);

            string strHtmlContent = HttpHelper.Open(strUrl, Encoding.GetEncoding("UTF-8"));
            strHtmlContent = strHtmlContent.Replace("\r\n", "").Replace("\r", "").Replace("\n", "");
            if (strHtmlContent.Equals(string.Empty))
            {
                return true;
            }
            try
            {
                List<string> XM_SongIdList = new List<string>();

                List<string> SongNameList = GetAlbum_SongName(strHtmlContent, ref XM_SongIdList);

                AlbumSongInfo albumSongInfo = GetAlbumSongInfo(strHtmlContent);


                //http://www.xiami.com/album/70909
                albumSongInfo.AlbumInfo.AlbumXmId = CommonHelper.GetMatchRegex(strUrl, "http://www.xiami.com/album/(\\d+)").Replace("http://www.xiami.com/album/", "");

                foreach (string songname in SongNameList)
                {
                    if (songname.Contains("套广播体操"))
                    {
                        return false;
                    }
                }
                //得到AlbumId
                albumSongInfo.AlbumInfo.AlbumId = sqlExecute.GetAlbumId(albumSongInfo.AlbumInfo);
                //得到歌手ID
                albumSongInfo.SingerInfo.SingerId = sqlExecute.GetSingerId(albumSongInfo.SingerInfo);

                if (albumSongInfo.AlbumInfo.AlbumId > 0 && albumSongInfo.SingerInfo.SingerId > 0)
                {
                    if (SongNameList.Count > 0)
                    {
                        albumSongInfo.SongNameList = SongNameList;
                        albumSongInfo.XM_SongIdList = XM_SongIdList;
                        sqlExecute.InsertAlbumSong(albumSongInfo);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                LogNet.LogBLL.Info("GetOneAlbumAllSong", ex);
                return true;
            }
        }



        #region 对于单个专辑页面

        /// <summary>
        /// 得到专辑歌曲 的基本信息,包括 艺人,语种,唱片公司,发行时间
        /// </summary>
        /// <param name="strHtmlContent"></param>
        /// <returns></returns>
        public AlbumSongInfo GetAlbumSongInfo(string strHtmlContent)
        {
            AlbumSongInfo albumSongInfo = new AlbumSongInfo();

            try
            {
                AlbumInfo albumInfo = new AlbumInfo();
                SingerInfo singerInfo = new SingerInfo();
                string pattern = "";
                pattern = "<h1 property=\"v:item(.+?)</h1>";
                string matchResult = CommonHelper.GetMatchRegex(pattern, strHtmlContent);
                albumInfo.AlbumName = CommonHelper.NoHTML(matchResult);

                albumInfo.AlbumSinger = CommonHelper.GetMatchRegex("艺人：</td>(.+?)</tr>", strHtmlContent, "<a(.+?)</a>");
                albumInfo.AlbumSinger = CommonHelper.NoHTML(albumInfo.AlbumSinger);//href="/artist/111382">洪佳琦

                /*----------虾米 艺人介绍----------------------------------------------------------------------*/
                singerInfo.SingerXmId = CommonHelper.GetMatchRegex("艺人：</td>(.+?)</tr>", strHtmlContent, "<a href=\"/artist/(\\d+)");
                singerInfo.SingerXmId = singerInfo.SingerXmId.Replace("<a href=\"/artist/", "");
                // 进入歌手 信息 得到 歌手的 档案,和地区
                singerInfo = GetSingerInfoBySingerXmId(singerInfo.SingerXmId);

                albumInfo.AlbumDesc = CommonHelper.GetMatchRegex("专辑介绍(.+?)album_intro_toggle\">",strHtmlContent);
                albumInfo.AlbumDesc = CommonHelper.NoHTML(albumInfo.AlbumDesc).Replace("专辑介绍:", "").Replace("\"", "");

                /*----------专辑类别,专辑风格----------------------------------------------------------------------*/

                albumInfo.AlbumStyle = CommonHelper.NoHTML(CommonHelper.GetMatchRegex("专辑风格：</td>(.+?)</tr>", strHtmlContent));
                albumInfo.AlbumStyle = Regex.Replace(albumInfo.AlbumStyle, "\\t", "").Replace("专辑风格：", ""); ;


                albumInfo.AlbumType = CommonHelper.NoHTML(CommonHelper.GetMatchRegex("专辑类别：</td>(.+?)</tr>", strHtmlContent));
                albumInfo.AlbumType = Regex.Replace(albumInfo.AlbumType, "\\t", "").Replace("专辑类别：", "");

                albumSongInfo.AlbumInfo = albumInfo;

                //得到歌手
                singerInfo.SingerName = albumInfo.AlbumSinger;
                albumSongInfo.SingerInfo = singerInfo;

                albumSongInfo.SongLanguage = CommonHelper.GetMatchRegex(">语种：</td>(.+?)</td>", strHtmlContent, "<td(.+?)</td>");
                albumSongInfo.SongLanguage = CommonHelper.NoHTML(albumSongInfo.SongLanguage);

                albumSongInfo.OriRightsHolder = CommonHelper.GetMatchRegex(">唱片公司：</td>(.+?)</td>", strHtmlContent, "<td(.+?)</td>");
                albumSongInfo.OriRightsHolder = CommonHelper.NoHTML(albumSongInfo.OriRightsHolder);

                string pubsishDate = CommonHelper.GetMatchRegex(">发行时间：</td>(.+?)</td>", strHtmlContent, "<td(.+?)</td>");

                pubsishDate = CommonHelper.NoHTML(pubsishDate);
                DateTime dtime = new DateTime(2010, 1, 1);
                DateTime.TryParse(pubsishDate, out dtime);
                albumSongInfo.PublishDate = dtime;


                return albumSongInfo;
            }
            catch (Exception ex)
            {
                LogNet.LogBLL.Info("GetAlbumSongInfo", ex);
                return null;
            }
        }

        /// <summary>
        /// 得到专辑下面的所有的歌曲名称
        /// </summary>
        /// <param name="strHtmlContent"></param>
        /// <returns></returns>
        public List<string> GetAlbum_SongName(string strHtmlContent, ref List<string> XM_SongidList)
        {
            List<string> SongNameList = new List<string>();
            string pattern = "song_name(.+?)</a>";
            Regex reg = new Regex(pattern, RegexOptions.Singleline | RegexOptions.IgnoreCase);
            MatchCollection nodesSongList = reg.Matches(strHtmlContent);
            foreach (Match ms in nodesSongList)
            {

                string xm_songId = CommonHelper.MatchRegexFirst("/song/\\d{5,12}",ms.Value);
                xm_songId = xm_songId.Replace("/song/", "");
                XM_SongidList.Add(xm_songId);

                string songName = CommonHelper.NoHTML(ms.Value);
                //<a href="/song/1770692721" title="">我不愿 让你一个人</a>

                songName = Regex.Replace(songName, "song_name\">", "", RegexOptions.Singleline | RegexOptions.IgnoreCase);
                SongNameList.Add(songName);
            }
            return SongNameList;
        }
        #endregion


        public SingerInfo GetSingerInfoBySingerXmId(string singerXmId)
        {

            SingerInfo singerInfo = new SingerInfo();
            singerInfo.SingerXmId = singerXmId;
            string strUrl = "http://www.xiami.com/artist/" + singerXmId;

            string strHtmlContent = HttpHelper.Open(strUrl, Encoding.GetEncoding("UTF-8"));
            strHtmlContent = strHtmlContent.Replace("\r\n", "").Replace("\r", "").Replace("\n", "");

            singerInfo.SingerAddress = CommonHelper.NoHTML(CommonHelper.GetMatchRegex("<tr>.+?地区：</td>(.+?)</tr>", strHtmlContent));
            singerInfo.SingerAddress = Regex.Replace(singerInfo.SingerAddress, "\\t", "");

            singerInfo.SingerDesc = CommonHelper.NoHTML(CommonHelper.GetMatchRegex("档案：</td>(.+?)</div>", strHtmlContent));
            singerInfo.SingerDesc = Regex.Replace(singerInfo.SingerDesc, "\\t", "");
            return singerInfo;
        }

        /// <summary>
        /// 重新运行 给的假链接的 专辑连接
        /// </summary>
        public void WorkWrongAgain()
        {
            //#xiang

            //先查出来,然后递归
            IList<HttpWrongInfo> listWrongList = sqlExecute.GetHttpWrongBySpiderTimeId(SpiderTimeInfo.SpiderTimeId);

            foreach (HttpWrongInfo httpWrongInfo in listWrongList)
            {
                GetOneAlbumAllSong(httpWrongInfo.AlbumUrl);
                sqlExecute.Update_XM_HttpWrongByWrongId(httpWrongInfo);
            }
        }

        public void test()
        {
            string strUrl = "http://www.xiami.com/album/576410?spm=a1z1s.3057849.0.0.bWYn4y";
            string xmid = CommonHelper.GetMatchRegex(strUrl, "http://www.xiami.com/album/(\\d+)").Replace("http://www.xiami.com/album/", "");

            Console.WriteLine(xmid);
        }
    }
}
