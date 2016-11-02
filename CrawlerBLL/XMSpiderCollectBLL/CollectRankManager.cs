using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Xml;
using System.IO;

using IBussiness;
using Crawler.Model.XMSpiderCollect;
using Crawler.DAL.XMSpiderCollectDAL;
using System.Threading;
namespace Crawler.BLL.XMSpiderCollectBLL
{
    public class CollectRankManager
    {
       static string[] array = new string[] { 
                "http://www.xiami.com/search/orinew/orderstatus/play_count/page/@pageIndex",
                "http://www.xiami.com/search/orinew/orderstatus/favorites/page/@pageIndex",
                "http://www.xiami.com/search/orinew/orderstatus/recommends/page/@pageIndex",
                "http://www.xiami.com/search/orinew/orderstatus/comments/page/@pageIndex"
            };

        static string Type = "";
        CollectService collectService = new CollectService();
        public void MainWork()
        {
            for (int i = 0; i < array.Length; i++)
            {
                string urlHeader = array[i];
                for (int k = 1; k < 5; k++)
                {
                    Console.WriteLine("当前第{0}项,第{1}页",i,k);
                    string url = urlHeader.Replace("@pageIndex",k.ToString());
                    //这里抓取
                    if (i==0) Type = "最多播放";
                    else if (i==1) Type = "最多收藏";
                    else if (i == 2) Type = "最多分享";
                    else if (i == 3) Type = "最多评论";
                    RunWork(url);
                }
            }
        }


        public void RunWork(string url)
        {
            //打开页面
            Thread.Sleep(6000);
            string html = HttpHelper.Open(url,"UTF-8");
            List<string> list = CommonHelper.GetMatchRegexList(CollectTemplateXml.CollectRank_Unit, html);
            //一个页面的
            foreach (string unit in list)
            {
                //每个单元 有Id 有 Titlle
                string collectId = CommonHelper.GetMatchRegex(CollectTemplateXml.CollectRank_CollectId, unit);
                string title = CommonHelper.GetMatchRegex(CollectTemplateXml.CollectRank_Title, unit);
                CollectSongAction(int.Parse(collectId), title);
            }
        }
       
        public void CollectSongAction(int collectId,string title)
        {
            //http://www.xiami.com/song/showcollect/id/2678242?spm=0.0.0.0.zBzAv4
          
             object[]objArr = new object[9];
             Thread.Sleep(6000);
            string html = HttpHelper.Open("http://www.xiami.com/song/showcollect/id/" + collectId,"UTF-8");
            string palyTimes = CommonHelper.GetMatchRegex(CollectTemplateXml.CollectSong_Play, html);
            string recommend = CommonHelper.GetMatchRegex(CollectTemplateXml.CollectSong_Recommend, html);
            string collect = CommonHelper.GetMatchRegex(CollectTemplateXml.CollectSong_Collect, html);
            string tags = CommonHelper.NoHTML(CommonHelper.GetMatchRegex(CollectTemplateXml.CollectSong_Tags, html));

            Console.WriteLine(palyTimes);
            Console.WriteLine(recommend);
            Console.WriteLine(collect);
            Console.WriteLine(tags);
            string main = CommonHelper.GetMatchRegexFull(CollectTemplateXml.SongInfo_Main, html);
            List<string> ListUnit = CommonHelper.GetMatchRegexList(CollectTemplateXml.SongInfo_Unit, main);
            foreach (string unit in ListUnit)
            {
               
                string songId = CommonHelper.GetMatchRegex(CollectTemplateXml.SongInfo_SongId, unit);
                string songname = CommonHelper.NoHTML(CommonHelper.GetMatchRegex(CollectTemplateXml.SongInfo_SongName, unit));
                string singer = CommonHelper.NoHTML(CommonHelper.GetMatchRegex(CollectTemplateXml.SongInfo_Singer, unit));
              

                objArr[0] = songId;
                objArr[1] = songname;
                objArr[2] = singer;
                objArr[3] = title;
                objArr[4] = tags;
                objArr[5] = Type;
                objArr[6] = palyTimes;
                objArr[7] = recommend;
                objArr[8] = collect;
                //把Type,存一下
                collectService.InsertCollectRank(objArr);
            }
            Console.WriteLine("CollectSongAction 完毕");
        }
       
    }
}
