using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Xml;
using System.IO;
using Crawler.Model.XMSpiderCollect;
using IBussiness;
using System.Configuration;

namespace Crawler.BLL.XMSpiderCollectBLL
{
    public class WorkManager
    {

        public static string FolderPath = ConfigurationManager.AppSettings["FolderPath"].ToString();
        CollectManager collectManager = new CollectManager();
        int RunType = int.Parse(ConfigurationManager.AppSettings["RunType"]);
        //这里负责调度
        public void MainWork()
        {

            int startIndex = int.Parse(ConfigurationManager.AppSettings["pageStart"].ToString());
            int endIndex = int.Parse(ConfigurationManager.AppSettings["pageEnd"].ToString());
            //运行处理部分

            try
            {
                PrepareTemplateRegex();

                if (RunType<0)
                {
                    return;
                }
                if (RunType==0)
                {
                    new CollectRankManager().MainWork();
                }
                else if (RunType == 1)
                {
                    for (; startIndex <= endIndex; startIndex++)
                    {
                        string mess = string.Format("当前第{0}页开始", startIndex);
                        LogNet.LogBLL.Info(mess);
                        Console.WriteLine(mess);
                        collectManager.CollectCateAction(startIndex);
                        //回写配置文件
                        ToolHelper.AppSetValue("pageStart", (startIndex + 1).ToString());
                        mess = string.Format("当前第{0}页完毕,配置文件回写完毕", startIndex);
                        LogNet.LogBLL.Info(mess);
                        Console.WriteLine(mess);
                    }
                }else if(RunType == 2)
                {
                    //抓取 分类, 统计该分类下面的 精选集的 个数预计消耗时间 8小时
                    new CollectCountManager().MainWork();

                   // http://www.xiami.com/collect/key/id/1
                }
                else if (RunType == 3)
                {
                    new CollectCate2().MainWork();
                   // LayerCollectAction(collectCate.KeyId, 1);
                }
            }
            catch (Exception ex)
            {
                LogNet.LogBLL.Fatal("MainWork", ex);
            }

        }


      #region 123
        /*

        public void CollectCateAction()
        {

            //http://www.xiami.com/music/category/page/2?spm=a1z1s.3061713.6856357.145.an13sc
            string fileXmlPath = FolderPath + "TextFile0.txt";
            string html = File.ReadAllText(fileXmlPath, Encoding.UTF8);

            string main = CommonHelper.GetMatchRegexFull(CollectTemplateXml.CollectCate_Main, html);

            List<string> ListUnit = CommonHelper.GetMatchRegexList(CollectTemplateXml.CollectCate_Unit, main);

            foreach (string unit in ListUnit)
            {
                string url = CommonHelper.GetMatchRegex(CollectTemplateXml.CollectCate_Url, unit);
                Console.WriteLine(url);

                string title = CommonHelper.GetMatchRegex(CollectTemplateXml.CollectCate_Title, unit);
                Console.WriteLine(title);

                string keyId = CommonHelper.GetMatchRegex(CollectTemplateXml.CollectCate_KeyId, unit);
                Console.WriteLine(keyId);

            }

            Console.WriteLine("CollectCateAction 完毕");
        }

        public void LayerCollectAction()
        {
            //http://www.xiami.com/collect/key/id/3/page/2?spm=0.0.0.0.XgFa9D
            string fileXmlPath = FolderPath + "TextFile1.txt";
            string html = File.ReadAllText(fileXmlPath, Encoding.UTF8);
            string main = CommonHelper.GetMatchRegexFull(CollectTemplateXml.LayerCollect_Main, html);

            List<string> ListUnit = CommonHelper.GetMatchRegexList(CollectTemplateXml.LayerCollect_Unit, main);

            foreach (string unit in ListUnit)
            {
                string url = CommonHelper.GetMatchRegex(CollectTemplateXml.LayerCollect_Url, unit);
                Console.WriteLine(url);

                string title = CommonHelper.GetMatchRegex(CollectTemplateXml.LayerCollect_Title, unit);
                Console.WriteLine(title);

                string keyId = CommonHelper.GetMatchRegex(CollectTemplateXml.LayerCollect_ShowCollectId, unit);
                Console.WriteLine(keyId);

            }
            Console.WriteLine("LayerCollectAction 完毕");
        }

        public void CollectSongAction()
        {
            //http://www.xiami.com/song/showcollect/id/2678242?spm=0.0.0.0.zBzAv4
            string fileXmlPath = FolderPath + "TextFile3.txt";
            string html = File.ReadAllText(fileXmlPath, Encoding.UTF8);

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
                Console.WriteLine(songId);

                string songname = CommonHelper.NoHTML(CommonHelper.GetMatchRegex(CollectTemplateXml.SongInfo_SongName, unit));
                Console.WriteLine(songname);

                string singer = CommonHelper.NoHTML(CommonHelper.GetMatchRegex(CollectTemplateXml.SongInfo_Singer, unit));
                Console.WriteLine(singer);
            }
            Console.WriteLine("CollectSongAction 完毕");
        }
         * 
         */
      #endregion


        /// <summary>
        /// 准备 正则模板
        /// </summary>
        private static void PrepareTemplateRegex()
        {
            string fileXmlPath = FolderPath + "CollectTemplate.xml";
            string xmlText = File.ReadAllText(fileXmlPath, Encoding.UTF8);
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlText);
            XmlNode xmlRootNode = xmlDoc.SelectSingleNode("Template");

            XmlNode chileNode = xmlRootNode.ChildNodes[0];
            CollectTemplateXml.CollectCateAttr_url = CommonHelper.GetXmlAttribute(chileNode, "url");
            CollectTemplateXml.CollectCate_Main = CommonHelper.GetXmlNode(chileNode, "Main");
            CollectTemplateXml.CollectCate_Unit = CommonHelper.GetXmlNode(chileNode, "Unit");
            CollectTemplateXml.CollectCate_Url = CommonHelper.GetXmlNode(chileNode, "Url");
            CollectTemplateXml.CollectCate_Title = CommonHelper.GetXmlNode(chileNode, "Title");
            CollectTemplateXml.CollectCate_KeyId = CommonHelper.GetXmlNode(chileNode, "KeyId");

            chileNode = xmlRootNode.ChildNodes[1];
            CollectTemplateXml.LayerCollectAttr_url = CommonHelper.GetXmlAttribute(chileNode, "url");
            CollectTemplateXml.LayerCollect_Main = CommonHelper.GetXmlNode(chileNode, "Main");
            CollectTemplateXml.LayerCollect_Unit = CommonHelper.GetXmlNode(chileNode, "Unit");
            CollectTemplateXml.LayerCollect_Url = CommonHelper.GetXmlNode(chileNode, "Url");
            CollectTemplateXml.LayerCollect_Title = CommonHelper.GetXmlNode(chileNode, "Title");
            CollectTemplateXml.LayerCollect_ShowCollectId = CommonHelper.GetXmlNode(chileNode, "CollectId");

            chileNode = xmlRootNode.ChildNodes[2];
            CollectTemplateXml.CollectSongAttr_url = CommonHelper.GetXmlAttribute(chileNode, "url");
            CollectTemplateXml.CollectSong_Play = CommonHelper.GetXmlNode(chileNode, "Play");
            CollectTemplateXml.CollectSong_Recommend = CommonHelper.GetXmlNode(chileNode, "Recommend");
            CollectTemplateXml.CollectSong_Collect = CommonHelper.GetXmlNode(chileNode, "Collect");
            CollectTemplateXml.CollectSong_Tags = CommonHelper.GetXmlNode(chileNode, "Tags");

            chileNode = xmlRootNode.ChildNodes[3];
            CollectTemplateXml.SongInfo_Main = CommonHelper.GetXmlNode(chileNode, "Main");
            CollectTemplateXml.SongInfo_Unit = CommonHelper.GetXmlNode(chileNode, "Unit");
            CollectTemplateXml.SongInfo_SongId = CommonHelper.GetXmlNode(chileNode, "SongId");
            CollectTemplateXml.SongInfo_SongName = CommonHelper.GetXmlNode(chileNode, "SongName");
            CollectTemplateXml.SongInfo_Singer = CommonHelper.GetXmlNode(chileNode, "Singer");

            //排名
            chileNode = xmlRootNode.ChildNodes[4];
            CollectTemplateXml.CollectRank_Unit = CommonHelper.GetXmlNode(chileNode, "Unit");
            CollectTemplateXml.CollectRank_Title = CommonHelper.GetXmlNode(chileNode, "Title");
            CollectTemplateXml.CollectRank_CollectId = CommonHelper.GetXmlNode(chileNode, "CollectId");


        }
        
    }
}
