using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crawler.Model.XmLrc;
using IBussiness;
using System.Text.RegularExpressions;
using LogNet;
using System.Threading;
using System.Data;
using Crawler.DAL.XmLrcDAL;
using Crawler.DAL;

namespace Crawler.BLL.XmLrcBLL
{
    public class LrcExportManager
    {
        LrcExportService dalService = new LrcExportService();
        static string ParentPath = @"D:\程序运行目录\Excel导入导出\";
        public void MainWork()
        {


            DataTable dt = dalService.GetLrcTable();
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_data.xls";
            //D:\程序运行目录\Excel导入导出
            //Console.WriteLine(path);
            // new ExportExcel().CreateExcel(dt, path);


            string tempExcelPath = AppDomain.CurrentDomain.BaseDirectory + "\\Template\\data.xls";
            string savePath = path;
            //new ExportExcel().InsertExcel(dt, savePath, tempExcelPath);

        }

        public void Test()
        {
            Console.WriteLine("Program is Start!");
            SystemConst.ConnStr = "server=222.73.68.174;database=XM_Spider;uid=sa;pwd=sunny.841;";
            DataTable dt = dalService.GetLrcTable();
            string path = ParentPath + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_data.xls";
            //D:\程序运行目录\Excel导入导出
            //Console.WriteLine(path);
            // new ExportExcel().CreateExcel(dt, path);

            string tempExcelPath = ParentPath + "Template\\data.xls";
            string savePath = path;
           // new ExportExcel().InsertExcel(dt, savePath, tempExcelPath);
            Console.WriteLine("Program is  Finished!");
        }
        public void Test2()
        {
            Console.WriteLine("Program is Start!");
            SystemConst.ConnStr = "server=222.73.68.174;database=XM_Spider;uid=sa;pwd=sunny.841;";
            DataTable dt = dalService.GetLrcTable();
            string path = ParentPath + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_data.xls";

            string savePath = path;
            new ExportExcel().CreateExcel(dt, savePath);
            Console.WriteLine("Program is  Finished!");
        }

        public void Test3()
        {
            Console.WriteLine("Program is Start!");
            SystemConst.ConnStr = "server=222.73.68.174;database=XM_Spider;uid=sa;pwd=sunny.841;";
            DataTable dt = dalService.GetLrcTable_T1();

            IList<SongInfo> list = ListToTableHelper.ConvertTo<SongInfo>(dt);
            DataTable dt_2 = ListToTableHelper.ToDataTable(list);

            Console.WriteLine(dt_2.Rows.Count);
            Console.WriteLine("Program is  Finished!");
        }

        public DataTable Test4()
        {
            SystemConst.ConnStr = "server=222.73.68.174;database=XM_Spider;uid=sa;pwd=sunny.841;";
            DataTable dt = dalService.GetLrcTable_T1();
            return dt;
        }
    }

    public class SongInfo
    {
        public int Id { get; set; }
        public string XM_SongId { get; set; }
        public string SongName { get; set; }
        public string SingerName { get; set; }
        public string AlbumName { get; set; }

    }
}
