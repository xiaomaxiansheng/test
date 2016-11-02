
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Crawler.Model.XMSongOrder;
using Crawler.DAL.XMSongOrder;
namespace Crawler.BLL.XMSongOrder
{
    public class XM_DoneManager
    {
        /// <summary>
        /// 检查是否有没做完的任务
        /// </summary>
        public static void CheckUnDone()
        {
            AlbumManager xm_AlbumClawManager = new AlbumManager();
            AlbumWorkManager xm_AlbumClawWorkManager = new AlbumWorkManager();
            WorkWrongAgainManager xm_WorkWrongAgainManager = new WorkWrongAgainManager();

            XM_SQlExecute sqlExecute = new XM_SQlExecute();
            SpiderTimeInfo.SpiderTimeId = sqlExecute.Check_XM_IsFinished();
            if (ModelArgs.ISCheckContinue && SpiderTimeInfo.SpiderTimeId > 1)
            {
                Console.WriteLine("检查到有未完成的任务,将继续上一次运行");
                //得到哪一年,运行到了一月

                //得到 frompage 和 YearMonth //然后调用...
                int startPage = 0;
                int yearMonth = 0;
                sqlExecute.GetUnDoneBySpiderId(SpiderTimeInfo.SpiderTimeId, ref startPage, ref yearMonth);

                if (startPage > 0 && yearMonth > 0)
                {
                    xm_AlbumClawWorkManager.ClawWorkOneMonth(yearMonth, startPage);
                }
            }
            else
            {
                SpiderTimeInfo.SpiderTimeId = sqlExecute.Insert_XM_SpiderTime_A_AlbumStartArgs();
            }

            Console.WriteLine("开启 剩下的任务");
            //开启 剩下的任务
            xm_AlbumClawManager.XM_AlbumClawAction();

            // SpiderTimeInfo.SpiderTimeId = 1;
            //运行中间过程中出错的
            Console.WriteLine("重新抓取中间被屏蔽的专辑连接");
            //剩下的也要开多线程

            xm_WorkWrongAgainManager.WorkWrongAgainAction();

            //  xm_AlbumClawWorkManager.WorkWrongAgain();
        }
    }
}
