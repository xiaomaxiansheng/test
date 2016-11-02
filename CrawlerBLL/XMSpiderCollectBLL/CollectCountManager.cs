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
    public class CollectCountManager
    {
        CollectService dalService = new CollectService();

        public const string UrlHeader = "http://www.xiami.com/collect/key/id/";
        const string regex = @"<span>\(第1页, 共(\d+)条\)</span>";
        public void MainWork()
        {

            //首先 获取总数
            int startId = Convert.ToInt32(ConfigurationManager.AppSettings["KeyIdStart"]);
            int endId = Convert.ToInt32(ConfigurationManager.AppSettings["KeyIdEnd"]);
            List<int> cateIdList = dalService.GetCollectCateId(startId, endId);

            foreach (int keyId in cateIdList)
            {
                try
                {
                    int result = GetCateCollectCount(keyId);
                    Console.WriteLine("处理完毕分类序号{0}有{1}个精选集", keyId, result);
                }
                catch (Exception ex)
                {
                    LogNet.LogBLL.Error("keyId ="+keyId.ToString()+"出现异常",ex);
                    Console.WriteLine(ex.Message);
                }
            }
        }

        public int GetCateCollectCount(int keyId)
        {
            int collectCount = 0;
            string url = UrlHeader + keyId.ToString();
            Thread.Sleep(6000);
            string html = HttpHelper.Open(url,"UTF-8");

            string match = CommonHelper.GetMatchRegex(regex, html);

            int.TryParse(match, out collectCount);

            dalService.UpdateCateDate(keyId, collectCount);

            return collectCount;
        }
    }
}
