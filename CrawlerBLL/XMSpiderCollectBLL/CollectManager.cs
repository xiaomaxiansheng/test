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
    public class CollectManager
    {
        public static string FolderPath = @"H:\JoinCrawler\ConXMSpider2\";
        public static string TestFolderPath = @"H:\JoinCrawler\ConXMSpider2\bin\Debug\HtmlFile\";
        CollectService collctService = new CollectService();

        public void CollectCateAction(int pageIndex)
        {
            //http://www.xiami.com/music/category/page/2?spm=a1z1s.3061713.6856357.145.an13sc
            // string fileXmlPath = TestFolderPath + "1\\"+pageIndex + ".txt";
            //string html = File.ReadAllText(fileXmlPath, Encoding.UTF8);
            Thread.Sleep(6000);
          
            string html = HttpHelper.Open(string.Format(CollectTemplateXml.CollectCateAttr_url, pageIndex), "UTF-8");

            string path = TestFolderPath + "CollectCatePage\\" + pageIndex + ".txt";
            WriteFile(path,html, FileType.CollectCate);

            string main = CommonHelper.GetMatchRegexFull(CollectTemplateXml.CollectCate_Main, html);
            List<string> ListUnit = CommonHelper.GetMatchRegexList(CollectTemplateXml.CollectCate_Unit, main);
            foreach (string unit in ListUnit)
            {
                CollectCate collectCate = new CollectCate();
                 string keyId = CommonHelper.GetMatchRegex(CollectTemplateXml.CollectCate_KeyId, unit);
                 collectCate.KeyId = int.Parse(keyId);
                //string url = CommonHelper.GetMatchRegex(CollectTemplateXml.CollectCate_Url, unit);
                string title = CommonHelper.GetMatchRegex(CollectTemplateXml.CollectCate_Title, unit);
                collectCate.CateName = title;
                collctService.InsertCollectCate(collectCate);
                LayerCollectAction(collectCate.KeyId,1);
            }
        }

        int pageCount = -1;
     
        public void LayerCollectAction(int keyId,int pageIndex )
        {
            //http://www.xiami.com/collect/key/id/3/page/2?spm=0.0.0.0.XgFa9D
            //string fileXmlPath = FolderPath + "TextFile1.txt";
            //string html = File.ReadAllText(fileXmlPath, Encoding.UTF8);
            string html = "";
            if (pageIndex==1)
            {
                Thread.Sleep(6000);
                html =  HttpHelper.Open(string.Format(CollectTemplateXml.LayerCollectAttr_url, keyId, pageIndex), "UTF-8");

                string regex = "class=\"p_num\">(\\d+)</a> <a class=\"p_redirect_l\"";
                string pageStr = CommonHelper.GetMatchRegex(regex, html);
                int.TryParse(pageStr,out pageCount);//这里得到 总页数
            }
            else if (pageCount < pageIndex)
            {
                return;
            }
            else
            {
                Thread.Sleep(6000);
                html = HttpHelper.Open(string.Format(CollectTemplateXml.LayerCollectAttr_url, keyId, pageIndex), "UTF-8");
            }

          //  string path = TestFolderPath + "LayerCollectPage\\" + pageIndex + "\\" + keyId + ".txt";
           // bool b = WriteFile(path, html, FileType.LayerCollect);
          
                string main = CommonHelper.GetMatchRegexFull(CollectTemplateXml.LayerCollect_Main, html);
                List<string> ListUnit = CommonHelper.GetMatchRegexList(CollectTemplateXml.LayerCollect_Unit, main);
                foreach (string unit in ListUnit)
                {
                    LayerCollect layerCollect = new LayerCollect();

                    layerCollect.KeyId = keyId;
                    layerCollect.PageIndex = pageIndex;
                    //string url = CommonHelper.GetMatchRegex(CollectTemplateXml.LayerCollect_Url, unit);

                    string title = CommonHelper.GetMatchRegex(CollectTemplateXml.LayerCollect_Title, unit);
                    layerCollect.ShowCollectName = title;

                    string showCollectId = CommonHelper.GetMatchRegex(CollectTemplateXml.LayerCollect_ShowCollectId, unit);

                    layerCollect.ShowCollectId = int.Parse(showCollectId);
                    if (layerCollect.ShowCollectId > 0)
                    {
                        //先插入
                     
                        layerCollect.Tags = "";

                        CollectSongAction(layerCollect);
                    }
                }

            //递归回调
            LayerCollectAction(keyId, ++pageIndex);

            Console.WriteLine("LayerCollectAction 完毕");
        }

        public void CollectSongAction(LayerCollect layerCollect)
        {
            CollectSongInfo collectSongInfo = new CollectSongInfo();
         
            //http://www.xiami.com/song/showcollect/id/2678242?spm=0.0.0.0.zBzAv4
           // string fileXmlPath = FolderPath + "TextFile2.txt";
           // string html = File.ReadAllText(fileXmlPath, Encoding.UTF8);

            Thread.Sleep(6000);
            string html = HttpHelper.Open(string.Format(CollectTemplateXml.CollectSongAttr_url, layerCollect.ShowCollectId), "UTF-8");
         
            //string path = TestFolderPath + "CollectSongPage\\" + layerCollect.ShowCollectId + ".txt";
            //bool b =  WriteFile(path, html, FileType.CollectSongInfo);

            //if (!b)
            //{
            //    return;
            //}
            //这个暂时不用
            string palyTimes = CommonHelper.GetMatchRegex(CollectTemplateXml.CollectSong_Play, html);
            string recommend = CommonHelper.GetMatchRegex(CollectTemplateXml.CollectSong_Recommend, html);
            string collect = CommonHelper.GetMatchRegex(CollectTemplateXml.CollectSong_Collect, html);
            string tags = CommonHelper.NoHTML(CommonHelper.GetMatchRegex(CollectTemplateXml.CollectSong_Tags, html));

            layerCollect.PalyTimes = int.Parse(palyTimes);
            layerCollect.RecommendTimes = int.Parse(recommend);
            layerCollect.CollectTimes = int.Parse(collect);
            layerCollect.Tags = tags;

            collctService.InsertLayerCollect(layerCollect);

            string main = CommonHelper.GetMatchRegexFull(CollectTemplateXml.SongInfo_Main, html);
            List<string> ListUnit = CommonHelper.GetMatchRegexList(CollectTemplateXml.SongInfo_Unit, main);
            foreach (string unit in ListUnit)
            {
                try
                {
                    string songId = CommonHelper.GetMatchRegex(CollectTemplateXml.SongInfo_SongId, unit);
                    string songname = CommonHelper.NoHTML(CommonHelper.GetMatchRegex(CollectTemplateXml.SongInfo_SongName, unit));
                    string singer = CommonHelper.NoHTML(CommonHelper.GetMatchRegex(CollectTemplateXml.SongInfo_Singer, unit));

                    if (songId=="")
                    {
                        songId = CommonHelper.GetMatchRegex("id=\"totle_(.+?)\"", unit);
                        singer = CommonHelper.NoHTML(CommonHelper.GetMatchRegex("<a href=\"/artist/\\d+\" title=\"\">(.+?)</a>", unit));
                    }

                    collectSongInfo.ShowCollectId = layerCollect.ShowCollectId;
                    collectSongInfo.XM_SongId = int.Parse(songId);
                    collectSongInfo.SongName = songname;
                    collectSongInfo.Singer = singer;

                    collctService.InsertCollectSongInfo(collectSongInfo);
                }
                catch (Exception ex)
                {
                    LogNet.LogBLL.Error("CollectSongAction [foreach] 异常", ex);
                }
            }
        }

        public bool WriteFile(string path,string html, FileType type)
        {
            bool result = true;
            switch (type)
            {
                case FileType.CollectCate:
                    if (!File.Exists(path))
                    {
                       // File.WriteAllText(path, html, Encoding.UTF8);
                    }
                    break;
                case FileType.LayerCollect:
                    if (!File.Exists(path))
                    {
                        //File.WriteAllText(path, html, Encoding.UTF8);
                    }
                    else
                    {
                        result = false;
                    }
                    break;
                case FileType.CollectSongInfo:
                    if (!File.Exists(path))
                    {
                       // File.WriteAllText(path, html, Encoding.UTF8);
                    }
                    else
                    {
                        result = false;
                    }
                    break;
                default:
                    break;
            }
            return result;
        }

        public enum FileType
        {
            CollectCate =1,
            LayerCollect = 2,
            CollectSongInfo = 3
        }
        public void test()
        {
             string fileXmlPath = FolderPath + "TextFile1.txt";
            string html = File.ReadAllText(fileXmlPath, Encoding.UTF8);
            string regex = "class=\"p_num\">(\\d+)</a> <a class=\"p_redirect_l\"";
            string pageStr = CommonHelper.GetMatchRegex(regex, html);
            int.TryParse(pageStr, out pageCount);//这里得到 总页数
        }


    }
}
