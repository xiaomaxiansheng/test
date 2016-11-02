using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crawler.DAL.XMSpiderCollectDAL;

namespace Crawler.BLL.XMSpiderCollectBLL
{
    public class CollectCate2
    {
        CollectManager collectManager = new CollectManager();
        CollectService collectService = new CollectService();
        public void MainWork()
        {

            List<int> ListKeyId = collectService.GetCateKwyIdListService();

            int index = 0;
            foreach (int keyId in ListKeyId)
            {
                index++;

                Console.WriteLine("当前任务:{0}/{1}", index, ListKeyId.Count);
                try
                {
                    collectManager.LayerCollectAction(keyId, 1);
                    collectService.UpdateCollectCateWorkService(keyId,1);
                    //更新状态为1
                    Console.WriteLine("当前任务抓取成功");
                }
                catch (Exception ex)
                {
                    LogNet.LogBLL.Error("MainWork\t KetId=" + keyId, ex);
                    collectService.UpdateCollectCateWorkService(keyId, 99);
                    //更新状态为99
                    Console.WriteLine("当前任务抓取失败");
                }

            }
            Console.WriteLine("所有任务抓取完毕");

           
        }

    }
}
