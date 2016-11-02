
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using log4net.Config;
using log4net;

using IBussiness;
using Crawler.Model.XMSongOrder;
using Crawler.DAL.XMSongOrder;
namespace Crawler.BLL.XMSongOrder
{
    public class AlbumManager
    {
        public static int YearMothStart;

        ThreadCounter threadCounter = new ThreadCounter();
        static int MaxThreadNum = ModelArgs.MaxThreadNum;
        XM_SQlExecute sqlExecute = new XM_SQlExecute();

        /// <summary>
        /// 变量
        /// </summary>
        public static int AllTaskCount;
       
        public void XM_AlbumClawAction()
        {
            //为多线程准备数据
          
            AllTaskCount = ModelArgs.AllTaskCount - ModelArgs.HavingCompleted;
         
            YearMothStart = ModelArgs.StartYearMonth + ModelArgs.HavingCompleted;

            //开始插入数据,然后开始运行

           if (SpiderTimeInfo.SpiderTimeId>0)
           {
               int i = 0;
               AlbumWorkManager albumClawWorkManager = new AlbumWorkManager();
               int yrarmonth = YearMothStart;
               while (yrarmonth <= ModelArgs.EndYearMonth)
               {
                   Console.WriteLine("TaskCount:{0}  Finish:{1}", ModelArgs.EndYearMonth - yrarmonth, i);
                   yrarmonth = YearMothStart + i;
               
                   try
                   {
                       //抓取一个月份的gedan数据
                       albumClawWorkManager.ClawWorkOneMonth(yrarmonth);
                   }
                   catch (Exception ex)
                   {

                       LogNet.LogBLL.Error("ClawWorkOneMonth", ex);
                   }
                   i++;
               }

           }
           else
           {
               LogNet.LogBLL.Info("XM_AlbumClawAction,获取SpiderTimeId出错");
               Console.WriteLine("XM_AlbumClawAction,获取SpiderTimeId出错");
           }
        }

     #region 多线程  
        /* 
        public void TaskXM_AlbumClawStart()
        {
            CommonHelper CommonHelper = new CommonHelper();
            //多线程处理
           List<AlbumYearMonthInfo>albumYearMonthInfo;
            DateTime t1 = DateTime.Now;
            int pcNum = 1;
            bool isFirstLoadData = true;
            albumYearMonthInfo = GetList(pcNum++);//初始化队列
            while (true)
            {
                Thread.Sleep(1000);
                #region 工作信息输出
              
                OutPutWrite(albumYearMonthInfo.Count);
                #endregion

                #region  填充队列
                if (albumYearMonthInfo.Count == 0 && !isFirstLoadData)
                {
                    if (pcNum <= PageCount)
                    {
                        albumYearMonthInfo = GetList(pcNum++);
                    }
                }

                #endregion

                isFirstLoadData = false; //加速第一次加载所设置变量

                #region  处理工作队列
                while ((threadCounter.getThreadNum("tc") < MaxThreadNum) && albumYearMonthInfo.Count > 0 && !isreturn)
                {
                    //没有 批次,不应该开启新的队列,而要等到所有队列处理结束
                    threadCounter.writerCounter("pcAdd");
                    threadCounter.writerCounter("tcAdd"); //放在线程外调整线程数。前防止主线程提前更新该计数器。
                    XM_AlbumClawArgs albumClawArgs = new XM_AlbumClawArgs(threadCounter, albumYearMonthInfo[0]);
                    ThreadPool.QueueUserWorkItem(new WaitCallback(XM_AlbumClawWorkAction), albumClawArgs);
                    albumYearMonthInfo.RemoveAt(0);
                }

                #endregion

                #region 所有任务处理完毕
                if ((threadCounter.getThreadNum("tc") == 0) && (albumYearMonthInfo.Count <= 0) && pcNum > PageCount)//判断线程数和之前显示会有延迟
                {
                    //更新所有的状态
                    sqlExecute.Update_XM_SpiderTime_A_AlbumStartArgs(SpiderTimeInfo.SpiderTimeId);

                    Console.WriteLine("XM_AlbumClaw抓取任务处理完毕");
                    DateTime t2 = DateTime.Now;
                    Console.WriteLine(CommonHelper.DateDiff(t1, t2));
                    //Console.WriteLine("process finished. Press any key to exit...");
                    break;
                }
                #endregion
            }
        }
        int runningMark = 0;
        char[] runMark = { '-', '/', '|', '\\' };
        /// <summary>
        /// 输出多线程处理过程中的信息
        /// </summary>
        /// <param name="taskCount"></param>
        public void OutPutWrite(int taskCount)
        {
            int left = Console.CursorLeft;
            int top = Console.CursorTop;
            Console.SetCursorPosition(0, top);
            Console.WriteLine(new String(' ', 200));
            Console.SetCursorPosition(left, top);

            if (runningMark > 3)
            {
                runningMark = 0;
            }
            object[] args = new object[] 
                { 
                    runMark[runningMark],
                    threadCounter.getThreadNum("tc"),
                    taskCount,
                    threadCounter.getThreadNum("pc"),
                    threadCounter.getThreadNum("ec"),
                    threadCounter.getThreadNum("fc"),
                };
            Console.WriteLine("[ {0} ] ThreadNum:{1} Queue:{2} Pending:{3} Error:{4} Finished:{5}", args);
            Console.SetCursorPosition(left, top);
            runningMark++;
        }

        public void XM_AlbumClawWorkAction(object objArgs)
        {
               //工作处理单个月份
               XM_AlbumClawArgs albumClawArgs = objArgs as XM_AlbumClawArgs; //数包装器
	           AlbumYearMonthInfo AlbumYearMonthInfo = albumClawArgs.AlbumYearMonthInfo;  //参数分解
	           ThreadCounter treadCounter = albumClawArgs.ThreadCounter; //参数分解
	           try
                {
                    XM_AlbumClawWorkManager albumClawWorkManager =  new XM_AlbumClawWorkManager();
                    //单个任务的完成
                    albumClawWorkManager.ClawWorkOneMonth(AlbumYearMonthInfo.YearMonth);
                    //更新库里面 AlbumYearMonthInfo.YearMonth,XM_AlbumPage 更新的时候,已经做了

	                threadCounter.writerCounter("fcAdd");//完成 计数
                    threadCounter.writerCounter("tcSub");//释放线程数
	            }
	            catch (Exception ex)
	            {
		            threadCounter.writerCounter("tcSub");//释放线程数
                    threadCounter.writerCounter("ecAdd");//错误  计数
                    log.Info("XM_AlbumClawWorkAction", ex);
			        return;
		        }
        }

        public List<AlbumYearMonthInfo> GetList(int pcnum)
        {
            //获取一个批次的数据
            //获取一个月份的东西
            List<AlbumYearMonthInfo>list = new List<AlbumYearMonthInfo>();
            int startIndex = (pcnum-1)*PageSize;
            int endIndex = pcnum*PageSize;

            for (int i = startIndex; i < endIndex && i < AllTaskCount; i++)
			{
                AlbumYearMonthInfo albumYearMonthInfo = new AlbumYearMonthInfo();
                albumYearMonthInfo.YearMonth = YearMothStart + i;
                albumYearMonthInfo.FinishStatus = 0;
                list.Add(albumYearMonthInfo);
			}
            return list;
        }
        */
        #endregion

    }


    #region 参数包装器

    /*
    public class XM_AlbumClawArgs
    {
        public XM_AlbumClawArgs() { }
        public XM_AlbumClawArgs(ThreadCounter treadCounter,AlbumYearMonthInfo albumYearMonthInfo)
        {
            this.ThreadCounter = treadCounter;
            this.AlbumYearMonthInfo = albumYearMonthInfo;
        }
        public ThreadCounter ThreadCounter { get; set; }
        public AlbumYearMonthInfo AlbumYearMonthInfo { get; set; }
    }
    */
    #endregion
}
