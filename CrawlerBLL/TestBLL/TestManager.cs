using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Xml;
using System.IO;
using System.Drawing;
using IBussiness;
using System.Net;
using System.IO.Compression;

namespace Crawler.BLL.TestBLL
{
    class TestManager
    {
        private static string reqUserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.0; SLCC1; .NET CLR 2.0.50727; .NET CLR 3.0.04506; InfoPath.2)";

        string url = "http://club.autohome.com.cn/bbs/forum-c-530-1.html";
        public void Test1()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(url));
            request.UserAgent = reqUserAgent;

            request.Method = "GET";
            request.Referer = url;
            request.Timeout = 10000;
            WebResponse response = request.GetResponse();
            Stream stream = response.GetResponseStream();
            StreamReader reader = new StreamReader(stream,Encoding.Default);
            string result = reader.ReadToEnd();
            Console.WriteLine(result);
           
            Console.WriteLine("完毕");

        }

        Encoding encoding = Encoding.Default;

        public void test2()
        {
            string mess = HttpHelper.Open(url,encoding);
            Console.WriteLine(mess);
            Console.WriteLine("完毕");
        }

     

    }
}
