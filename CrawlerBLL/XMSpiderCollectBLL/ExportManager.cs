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
using System.Text.RegularExpressions;

namespace Crawler.BLL.XMSpiderCollectBLL
{
    public class ExportManager
    {
        private static string row_Temp;

        public static string Row_Temp
        {
            get
            {

                row_Temp = File.ReadAllText("Template/Row.txt");
                return row_Temp;
            }

        }
        private static string fileHeader;

        public static string FileHeader
        {
            get
            {

                fileHeader = File.ReadAllText("Template/Temp.txt");
                return fileHeader;
            }

        }
        public void CollectSongReportWork(int size)
        {
            string directInfoStr = "Report_" + DateTime.Now.ToString("yyyyMMdd");
            DirectoryInfo directInfo = new DirectoryInfo(directInfoStr);
            if (!directInfo.Exists)
	        {
		         directInfo.Create();
	        }
            for (int i = 1; i <= 2; i++)
            {
                //获取数据
                Console.WriteLine("当前生成第"+i+"个文件");
                List<CollectSongReport> list = new CollectService().GetCollectSongReportListService(i,size);

                string outputPath = directInfoStr+"/"+i+".xml";
                //写数据
                StringBuilder sBulider = new StringBuilder();
                foreach (CollectSongReport song in list)
                {
                   string row = Row_Temp.Replace("#KeyId",song.KeyId).Replace("#CateName",song.CateName)
                        .Replace("#ShowCollectId",song.ShowCollectId).Replace("#ShowCollectName",song.ShowCollectName)
                        .Replace("#Tags",song.Tags).Replace("#SongName",song.SongName)
                        .Replace("#Singer",song.Singer).Replace("#XM_SongId",song.XM_SongId);
                    //拼接文本,输出文件流,考虑不到文件可能过大,所以分批次
                    sBulider.Append(row+"\n");
                }

                File.WriteAllText(outputPath, FileHeader.Replace("#NumCount", (list.Count + 1).ToString()).Replace("#RowContent", sBulider.ToString()));
                Console.WriteLine("当前第" + i + "个文件,生成完毕");  
            }
        }
       static  string songnameRegex = ConfigurationManager.AppSettings["SongNameRegex"].ToString();
       static string singerRegex = ConfigurationManager.AppSettings["SingerRegex"].ToString();
        public void UpdateSongNameWork()
        {
            //更新掉韩文的
           CollectService serviceDal =  new CollectService();

           string[] array = File.ReadAllLines("xm_songId.txt");

           List<string> list = array.ToList();
               //serviceDal.GetListSongService();
                //
                 //
          
            foreach (string xm_songid in list)
	        {
                //1769129596
                string url = string.Format("http://www.xiami.com/song/playlist/id/{0}/object_name/default/object_id/0",xm_songid);
                string mess = HttpHelper.OpenWebClient(url, Encoding.UTF8);
                mess = Regex.Replace(mess, "\r\n", "").Replace("\t", "");

                string songname = CommonHelper.GetMatchRegex(songnameRegex, mess).Replace("<![", "").Replace("]]>", "").Trim();
                songname = songname.Replace("'", "''");
                string singer = CommonHelper.GetMatchRegex(singerRegex, mess).Replace("<![", "").Replace("]]>", "").Trim();
                singer = singer.Replace("'", "''");
                string sql = "update CollectSongInfo set SongName=N'" + songname + "',Singer =N'" + singer + "' where XM_SongId = '" + xm_songid + "' \r\n";
             
                 
                File.AppendAllText("sql.sql", sql,Encoding.UTF8);
               
                //int result = serviceDal.UpdateSongNameByXm_SongIdService(xm_songid, songanem, singer);
                //if (result > 0)
                //{
                //    Console.WriteLine("更新成功!");
                //}
                //else
                //{
                //    Console.WriteLine("更新失败!");
                //}
                Thread.Sleep(1000);
	        }
            Console.WriteLine("文件写入完毕");
                 //UpdateSongNameByXm_SongId
            //找到要更新的列表
            // 打开  //http://www.xiami.com/song/playlist/id/1769193354/object_name/default/object_id/0
            //得到歌手名,歌曲名
            //更新
            //执行下一个
        }

        public void test1()
        {
            string url = "http://www.xiami.com/song/playlist/id/1769129596/object_name/default/object_id/0";
            string mess = HttpHelper.OpenWebClient(url, Encoding.UTF8);
            mess = Regex.Replace(mess, "\r\n", "").Replace("\t", "");
            //&lt;title&gt;&lt;!\[CDATA\[ (.+?) \]\]&gt;&lt;/title&gt;
            //&lt;artist&gt;&lt;!\[CDATA\[ (.+?) \]\]&gt;&lt;/artist&gt;

            string songanem = CommonHelper.GetMatchRegex(songnameRegex, mess).Replace("<![", "").Replace("]]>","").Trim();

            string singer = CommonHelper.GetMatchRegex(singerRegex, mess).Replace("<![", "").Replace("]]>", "").Trim(); 
            Console.WriteLine(songanem);
            Console.WriteLine(singer);
        }


    }
}
