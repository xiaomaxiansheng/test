using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crawler.Model.XMSongOrder;
using Crawler.BLL.XMSongOrder;
using System.Configuration;
using IBussiness;

namespace ConXMSongOrder
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            LogNet.LogBLL.Info("程序开始运行");
            bool b = true;
            InitArgs(ref b);
            if (b)
            {
                switch (ModelArgs.RunType)
                {
                    case "XM":
                        XM_DoneManager.CheckUnDone();
                        break;
                    case "BD":
                        // new BD_MusicClawWorkManager().Start();
                        break;
                    default:
                        break;
                }

                // new BD_DownloadManager().GetAllQianQianCloud();
            }
            Console.WriteLine("Main方法运行结束,按回车键退出程序");
            Console.ReadLine();
        }

        private static void InitArgs(ref bool b)
        {
            try
            {
                AlbumStartArgsInfo.StartYear = int.Parse(ConfigurationManager.AppSettings["StartYear"].ToString());
                AlbumStartArgsInfo.StartMonth = int.Parse(ConfigurationManager.AppSettings["StartMonth"].ToString());
                AlbumStartArgsInfo.EndYear = int.Parse(ConfigurationManager.AppSettings["EndYear"].ToString());
                AlbumStartArgsInfo.EndMonth = int.Parse(ConfigurationManager.AppSettings["EndMonth"].ToString());
                AlbumStartArgsInfo.DelayTime = int.Parse(ConfigurationManager.AppSettings["DelayTime"].ToString());
                SpiderTimeInfo.SpiderType = ConfigurationManager.AppSettings["SpiderType"].ToString();//目前只有 CH 和EN

                ModelArgs.ISCheckContinue = bool.Parse(ConfigurationManager.AppSettings["ISCheckContinue"].ToString());
                ModelArgs.EndYearMonth = CommonHelper.FormatYearMonth(AlbumStartArgsInfo.EndYear, AlbumStartArgsInfo.EndMonth);
                ModelArgs.StartYearMonth = CommonHelper.FormatYearMonth(AlbumStartArgsInfo.StartYear, AlbumStartArgsInfo.StartMonth);
                ModelArgs.AllTaskCount = ModelArgs.EndYearMonth - ModelArgs.StartYearMonth;
                ModelArgs.MaxThreadNum = int.Parse(ConfigurationManager.AppSettings["MaxThreadNum"].ToString());
                ModelArgs.RunType = ConfigurationManager.AppSettings["RunType"].ToString();//目前只有 XM 和 BD
            }
            catch (Exception ex)
            {
                Console.WriteLine("初始化配置文件出错");
                Console.WriteLine(ex.Message);
                b = false;
            }
        }
    }

}
