using log4net;
using Crawler.DAL.XMSongOrder;
using Crawler.Model.XMSongOrder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using IBussiness;

namespace Crawler.BLL.XMSongOrder
{
    public class WorkWrongAgainManager
    {
      
        ThreadCounter threadCounter = new ThreadCounter();
        static int MaxThreadNum = 10;
        bool isreturn = false;
        XM_SQlExecute sqlExecute = new XM_SQlExecute();
        /// <summary>
        /// 所有的任务总数
        /// </summary>
        public static int AllTaskCount { get; set; }
        /// <summary>
        /// 分页 取批次总数
        /// </summary>
        public static int  PageCount { get; set; }
        /// <summary>
        /// 分页 页大小
        /// </summary>
        public static int  PageSize { get; set; }

        IList<HttpWrongInfo> listWrongList;
        public void WorkWrongAgainAction()
        {
            listWrongList = sqlExecute.GetHttpWrongBySpiderTimeId(SpiderTimeInfo.SpiderTimeId);
            //为多线程准备数据
            AllTaskCount = listWrongList.Count;
            //开始插入数据,然后开始运行
            if (AllTaskCount > 0)
           {
               TaskWorkWrongAgainStart();
           }
        }

        public void TaskWorkWrongAgainStart()
        {
           
            //多线程处理
            DateTime t1 = DateTime.Now;
            int pcNum = 1;
           //初始化队列
            while (true)
            {
                Thread.Sleep(1000);
                #region 工作信息输出
                OutPutWrite(listWrongList.Count);
                #endregion

                #region  处理工作队列
                while ((threadCounter.getThreadNum("tc") < MaxThreadNum) && listWrongList.Count > 0 && !isreturn)
                {
                    //没有 批次,不应该开启新的队列,而要等到所有队列处理结束
                    threadCounter.writerCounter("pcAdd");
                    threadCounter.writerCounter("tcAdd"); //放在线程外调整线程数。前防止主线程提前更新该计数器。
                    WorkWrongAgainArgs workWrongAgainArgs = new WorkWrongAgainArgs(threadCounter, listWrongList[0]);
                    ThreadPool.QueueUserWorkItem(new WaitCallback(WorkWrongAgainAction), workWrongAgainArgs);
                    listWrongList.RemoveAt(0);
                }

                #endregion

                #region 所有任务处理完毕
                if ((threadCounter.getThreadNum("tc") == 0) && (listWrongList.Count <= 0) && pcNum > PageCount)//判断线程数和之前显示会有延迟
                {
                    Console.WriteLine("WorkWrongAgain任务处理完毕");
                    DateTime t2 = DateTime.Now;
                    Console.WriteLine(CommonHelper.DateDiff(t1, t2));
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

        public void WorkWrongAgainAction(object objArgs)
        {
               //工作处理单个月份
            WorkWrongAgainArgs workWrongAgainArgs = objArgs as WorkWrongAgainArgs; //数包装器
            HttpWrongInfo httpWrongInfo = workWrongAgainArgs.HttpWrongInfo;  //参数分解
            ThreadCounter treadCounter = workWrongAgainArgs.ThreadCounter; //参数分解
	        try
            {
                AlbumWorkManager albumClawWorkManager =  new AlbumWorkManager();
                albumClawWorkManager.GetOneAlbumAllSong(httpWrongInfo.AlbumUrl);
                sqlExecute.Update_XM_HttpWrongByWrongId(httpWrongInfo);
                
	            threadCounter.writerCounter("fcAdd");//完成 计数
                threadCounter.writerCounter("tcSub");//释放线程数
	        }
	        catch (Exception ex)
	        {
		        threadCounter.writerCounter("tcSub");//释放线程数
                threadCounter.writerCounter("ecAdd");//错误  计数
                LogNet.LogBLL.Info("XM_AlbumClawWorkAction", ex);
			    return;
		    }
        }
    }



    public class WorkWrongAgainArgs
    {
        public WorkWrongAgainArgs() { }
        public WorkWrongAgainArgs(ThreadCounter treadCounter, HttpWrongInfo httpWrongInfo)
        {
            this.ThreadCounter = treadCounter;
            this.HttpWrongInfo = httpWrongInfo;
        }
        public ThreadCounter ThreadCounter { get; set; }
        public HttpWrongInfo HttpWrongInfo { get; set; }
    }
     
}
