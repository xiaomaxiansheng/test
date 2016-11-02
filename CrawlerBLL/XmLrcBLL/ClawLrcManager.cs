using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crawler.Model.XmLrc;
using IBussiness;
using System.Text.RegularExpressions;
using LogNet;
using Crawler.DAL.XmLrcDAL;
using System.Threading;


namespace Crawler.BLL.XmLrc
{
   public class ClawLrcManager
    {
       ClawLrcService serviceDal = new ClawLrcService();
       public void MianWork()
       {
           List<string> list = serviceDal.GetTaskSongIdList();

           int i = 0;
           foreach (string xm_songId in list)
           {
               i++;
               Console.WriteLine("当前任务进度\t{0}/{1}",i, list.Count);
               LrcSongInfo lrcSongInfo = ClawLyricAction(xm_songId);
               //更新数据

               int result = serviceDal.UpdateXM_CN_EN_56876Service(lrcSongInfo);
               if (result>0)
               {
                   Console.WriteLine("\t更新成功!");
               }
               else
               {
                   Console.WriteLine("\t更新失败!");
               }
           }
       }

        private static Encoding encoding = Encoding.UTF8;

        static string SongNameRegex = "<div id=\"title\">.+?<h1>(.+?)</h1>";
        static string BelongAlbumRegex = "valign=\"top\">所属专辑：</td>.+?<a href=\"/album/\\d+\" title=\"\">(.+?)</a></div>";
        static string SingerRegex = "valign=\"top\">演唱者：</td>.+?<a href=\"/artist/\\d+\" title=\"\">(.+?)</a></div>";
        static string LyricistRegex = "valign=\"top\">作词：</td>.+?text-overflow:ellipsis;\">(.+?)</div>";
        static string ComposerRegex = "valign=\"top\">作曲：</td>.+?text-overflow:ellipsis;\">(.+?)</div>";
        static string LrcTxtRegx = "<div id=\"lrc\" class=\"clearfix\">(.+?)</div>";

        static string SingerImgRegex = "<a id=\"cover_lightbox\" href=\"(.+?)\" target=\"_blank\"";

        static string AlbumImgRegex = "<div class=\"cover\">.+?src=\"(.+?)\" /></a></div>";
        static string BelongAlbumUrlRegex = "<div class=\"cover\"><a class=\"CDcover185\" id=\"albumCover\" href=\"(.+?)\"";
        static string ArtistUrlRegex = "valign=\"top\">演唱者：</td>.+?<a href=\"(.+?)\" title=";


        public LrcSongInfo ClawLyricAction(string xm_songid)
        {
            Thread.Sleep(5000);
            LrcSongInfo lyricSong = new LrcSongInfo();
            lyricSong.XM_SongId = xm_songid;
            string url = "http://www.xiami.com/song/" + xm_songid;
            try
            {
                string html  = HttpHelper.Open(url, encoding);

               // lyricSong.SongName = CommonHelper.NoHTML(CommonHelper.GetMatchRegex(SongNameRegex, html));
              //  lyricSong.BelongAlbum = CommonHelper.NoHTML(CommonHelper.GetMatchRegex(BelongAlbumRegex, html));
              //  lyricSong.Singer = CommonHelper.NoHTML(CommonHelper.GetMatchRegex(SingerRegex, html));
                lyricSong.Lyricist = CommonHelper.NoHTML(CommonHelper.GetMatchRegex(LyricistRegex, html));
                lyricSong.Composer = CommonHelper.NoHTML(CommonHelper.GetMatchRegex(ComposerRegex, html));
                lyricSong.LrcText = CommonHelper.GetMatchRegex(LrcTxtRegx, html);
                lyricSong.LrcText = Regex.Replace(lyricSong.LrcText, "<.+?>", "").Replace("				", "").Replace(" 歌词：", "").Replace("                ", "");

               // lyricSong.AlbumUrl = "http://www.xiami.com" + CommonHelper.GetMatchRegex(BelongAlbumUrlRegex, html);
               // lyricSong.ArtistUrl = "http://www.xiami.com" + CommonHelper.GetMatchRegex(ArtistUrlRegex, html);
               // lyricSong.ArtistImgUrl = CommonHelper.GetMatchRegex(SingerImgRegex, html);
               // lyricSong.AlbumImgUrl = CommonHelper.GetMatchRegex(AlbumImgRegex, html);

                lyricSong.Lyric = GetLyricBySongIdAction(xm_songid);

            }
            catch (Exception ex)
            {
              
                lyricSong.Lyricist = "";
                lyricSong.Composer = "";
                lyricSong.LrcText = "";
                lyricSong.Lyric = "";
                LogBLL.Error("ClawLyricAction",ex);
            }
            return lyricSong;

        }


        static string lyricRegex = "<lyric>(.+?)</lyric>";

       /// <summary>
       ///  根据 XM_SongId 得到带时间戳的歌词
       /// </summary>
       /// <param name="xm_songid"></param>
       /// <returns></returns>
        public string GetLyricBySongIdAction(string xm_songId)
        {
            string url = "http://www.xiami.com/song/playlist/id/" + xm_songId.Trim() + "/object_name/default/object_id/0";

            string html = HttpHelper.OpenWebClient(url, Encoding.UTF8);
            if (string.IsNullOrEmpty(html))
            {
                return "";
            }
            string lyricUrl = CommonHelper.GetMatchRegex(lyricRegex, html);

            return HttpHelper.OpenWebClient(lyricUrl, Encoding.UTF8);

        }


         void tet1()
        {
            // 得到 词作者,曲作者, txt歌词
            //http://www.xiami.com/song/1771445580
            string xm_songid = "1771445580";
            LrcSongInfo lrcSongInfo = ClawLyricAction(xm_songid);

            Console.WriteLine(lrcSongInfo.Lyricist);
            Console.WriteLine(lrcSongInfo.Composer);
           
            Console.WriteLine(lrcSongInfo.LrcText);
            Console.WriteLine("完毕");
          
        }

         void get2()
        {
            string xm_songid = "1771445580";
            //http://www.xiami.com/song/playlist/id/1771445580/object_name/default/object_id/0
            string lyric = GetLyricBySongIdAction(xm_songid);
            Console.WriteLine(lyric);
            Console.WriteLine("完毕");
           
        }

           
    }
}
