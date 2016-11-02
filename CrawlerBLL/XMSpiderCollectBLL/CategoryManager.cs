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
using System.Configuration;

namespace Crawler.BLL.XMSpiderCollectBLL
{
    public class CategoryManager
    {
        public static string FolderPath = @"H:\JoinCrawler\ConXMSpider2\";
        public static string TestFolderPath = @"H:\JoinCrawler\ConXMSpider2\bin\Debug\HtmlFile\";
        CollectService collctService = new CollectService();

        public void CategoryWork()
        {
            int startIndex = int.Parse(ConfigurationManager.AppSettings["pageStart"].ToString());
            int endIndex = int.Parse(ConfigurationManager.AppSettings["pageEnd"].ToString());

        }
       
    }
}
