using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Xml;
using System.IO;
using System.Drawing;
using IBussiness;

namespace CrawlerBLL.SpiderImg
{
    public class ImgCollBLL
    {
        //定义规则,解析XML模板,以及后续的工作
        public void AnalysisXml()
        {
            string filePath = "Task.DAT";
                //"../../SpiderImg/Dat.xml";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(File.ReadAllText(filePath,Encoding.UTF8));//根目录

            XmlNodeList xmlNodeListSite = xmlDoc.GetElementsByTagName("Server");//一阶子节点

            foreach (XmlNode xmlNodeSite in xmlNodeListSite)
            {
               string  url = xmlNodeSite.Attributes["Url"].Value;           //站点Url

                XmlNodeList xmlNodeListExps = xmlNodeSite.SelectNodes("Expressions"); //2阶子节点
                XmlNodeList xmlNodeListExp = xmlNodeListExps[0].SelectNodes("Expression"); //3阶子节点
                foreach (XmlNode xmlNodeExp in xmlNodeListExp)
                {
                    folderName = xmlNodeExp.Attributes["Folder"].Value;//定义文件夹
                    REX_JPG_IN_URL = xmlNodeExp.InnerXml;
                    Work(url);
                    //Url正则
                    //这里是 程序Run的入口
                }
                Console.WriteLine();
            }
        }
        public static string cUrl = @"http://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?";
        /// <summary>
        /// capture the jpg
        /// </summary>
        public static string REX_JPG_IN_URL = "";
            //cUrl + @".jpg";

        private string folderName ="";
         

        private string outPath = "";
        public string Outpath
        {
            get
            {
                if (string.IsNullOrEmpty(outPath))
                {
                    outPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, folderName);
                    if (Directory.Exists(outPath))
                    {
                        LogNet.LogBLL.Debug(string.Format("folder has already existed: {0}", outPath));
                    }
                    else
                    {
                        try
                        {
                            Directory.CreateDirectory(outPath);
                        }
                        catch
                        {
                            outPath = AppDomain.CurrentDomain.BaseDirectory;
                        }
                    }
                }
                return outPath;
            }
        }

        public void Work(string url)
        {

            //string url = "http://image.baidu.com/i?tn=baiduimage&ipn=r&ct=201326592&cl=2&lm=-1&st=-1&fm=result&fr=&sf=1&fmq=1386472544570_R&pv=&ic=0&nc=1&z=&se=1&showtab=0&fb=0&width=&height=&face=0&istype=2&ie=utf-8&word=%E6%98%8E%E6%98%9F&f=3&rsp=0&rn=21&pn={0}&ln=2000";
            Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
              bool isFinished = false;
            int i = 0;
            HttpHelper.cookie = "BAIDUID=2FA2E8772E7182776F3E01DC1FFD8FFC:FG=1; BAIDUVERIFY=3E4A7AC692051010F7E01878884DC8DF94884AB758E448FA0610D4C8DC3189E0A1D594FC8BAD6F2A129FA72BFEDA26949DD7B89D5B6EFEDB6132:1386397807:fe10a1bb7edcf753; BDUSS=JORUd4ckktcVhwV3BOUW1-YzFSZDB1QnE4blhHQzAxLUFoa0YzeFRHYVhUY3BTQVFBQUFBJCQAAAAAAAAAAAEAAABsXP0TbG02NDkyNzM4NDAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAJfAolKXwKJSQ1; userid=1ji2s235h; fb=0";
            while (!isFinished)
            {
                string u = string.Format(url, i * 18);
                string html = HttpHelper.Open(u,"utf-8");
                if (html.Length<20000)
                {
                    Console.WriteLine("被拦截,有验证码!");
                }
                List<string> list = CommonHelper.GetMatchRegexList(REX_JPG_IN_URL, html);
                if (list.Count == 0) isFinished = true;
                if (list.Count==1)
                {
                    Console.WriteLine("剩下一个默认页面");
                    isFinished = true;
                }
                foreach (string item in list)
                {
                    Console.WriteLine(item);
                    //文件的存储
                    ImgChoose(item, Outpath, folderName);

                }
                Console.WriteLine("当前抓取第"+i+"页");
                i++;
               
            }
            Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            Console.WriteLine("总共完成:"+i+"页");
            Console.WriteLine("isFinished!");
        }


        /// <summary>
        /// 图片筛选器
        /// </summary>
        /// <param name="imgUrl">图片Url(绝对路径)</param>
        /// <param name="folderName">文件夹名称</param>
        public void ImgChoose(string imgUrl,string _outPath, string folderName)
        {
            try
            {
                Image img = HttpHelper.GetBitmap(imgUrl);
               
                string filename = imgUrl.Substring(imgUrl.LastIndexOf('/')+1);

                int height = 400;
                int width = 300;
                if (img.Width * img.Height < height * width) return;
                if (File.Exists(Path.Combine(_outPath, filename)))
                {
                    var existImg = Image.FromFile(Path.Combine(_outPath, filename));
                    if (existImg.Flags == img.Flags && existImg.Size == img.Size)
                    {
                        return;
                    }
                }
                string savePath = Path.Combine(_outPath, CommonHelper.GetUniqueNameByStoreFile(_outPath, filename));
                img.Save(savePath);
            }
            catch (Exception ex)
            {
                LogNet.LogBLL.Debug("ImgChoose",ex);
                
            }
          
        }
    }
}
