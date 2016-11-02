using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Net;
using System.IO;
using System.Drawing;
using LogNet;
using System.Diagnostics;

namespace IBussiness
{
    public static class HttpHelper
    {

        public static string cookie = "";
        private static string reqUserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.0; SLCC1; .NET CLR 2.0.50727; .NET CLR 3.0.04506; InfoPath.2)";

        public static string HttpJsonPost(string posturl, string postData)
        {
            string content = string.Empty;
            Stream instream = null;
            StreamReader sr = null;
            Encoding encoding = Encoding.UTF8;
            byte[] data = encoding.GetBytes(postData);
            try
            {
                // 设置参数
                HttpWebRequest request = WebRequest.Create(posturl) as HttpWebRequest;
                CookieContainer cookieContainer = new CookieContainer();
                if (request != null)
                {
                    //Uri u = new Uri("http://test.telefen.com");
                    //Cookie c = new Cookie("UserToken", "VA4vpmdbKOw8HYSR3Z03Vi0wzlSncW2ho2voZLtuwO2o94vZi1Valp29+PgF/0FpqYjsd42Xky+zlfGS06khF2h5giLCPrAAo7x63jmqS5XwbBD2J9LDTnaaCuTxqqCm");

                    request.CookieContainer = cookieContainer;
                    //request.CookieContainer.Add(u, c);
                    request.AllowAutoRedirect = true;//返回301、302再次访问
                    request.Method = "POST";
                    //request.Referer = "http://esf.bxgfw.com/shop/admin/renthouse_pub.aspx";//伪造从哪个地址访问
                    request.ContentType = "application/x-www-form-urlencoded";
                    //request.ContentType = "application/json; charset=utf-8";//WEBAPI等支持JSON解码
                    //myRequest.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 6.1; zh-CN; rv:1.9.2.13) Gecko/20101203 Firefox/3.6.13";//模拟浏览器
                    request.ContentLength = data.Length;
                    Stream outstream = request.GetRequestStream();
                    outstream.Write(data, 0, data.Length);
                    outstream.Close();
                    Stopwatch watch = new Stopwatch();//计时器
                    watch.Start();
                    //发送请求并获取相应回应数据
                    HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                    watch.Stop();
                    //直到request.GetResponse()程序才开始向目标网页发送Post请求
                    if (response != null) instream = response.GetResponseStream();
                    if (instream != null) sr = new StreamReader(instream, encoding);
                    //返回结果网页（html）代码
                    if (sr != null)
                    {
                        content = sr.ReadToEnd();
                        return content;
                    }
                }
                return content;
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                UnionLog.WriteLog(LogType.UNION_ERROR, string.Format("Http Post请求异常：{0}；堆栈信息：{1}", err, ex.StackTrace));
                return string.Empty;
            }
        }
        public static string Open(string url, string encoding)
        {
            string result = "";
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(url));
                request.UserAgent = reqUserAgent;
                request.Headers.Add("Cookie", cookie);
                request.Method = "GET";
                request.Referer = url;
                request.Timeout = 10000;
                WebResponse response = request.GetResponse();
                Stream stream = response.GetResponseStream();
                StreamReader reader = new StreamReader(stream, Encoding.GetEncoding(encoding));
                result = reader.ReadToEnd();
            }
            catch (Exception ex)
            {
                LogBLL.Error("HttpGetHtml", ex);
            }
            return result;
        }
        public static string Open(string url, Encoding encoding)
        {
            string result = "";
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(url));
                request.UserAgent = reqUserAgent;
                request.Headers.Add("Cookie", cookie);
                request.Method = "Get";
                request.Referer = url;
                request.Timeout = 10000;
                WebResponse response = request.GetResponse();
                Stream stream = response.GetResponseStream();

                StreamReader reader = new StreamReader(stream, encoding);
                result = reader.ReadToEnd();
            }
            catch (Exception ex)
            {
                LogBLL.Error("HttpGetHtml", ex);
            }
            return result;
        }

        public static Bitmap GetBitmap(string url)
        {
            Bitmap bmp = null;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(url));
                request.UserAgent = reqUserAgent;
                request.Timeout = 10000;
                WebResponse response = request.GetResponse();
                Stream stream = response.GetResponseStream();
                bmp = new Bitmap(stream);
                stream.Close();
                response.Close();
            }
            catch (Exception ex)
            {
                bmp = new Bitmap(1, 1);// 返回1像素的图片
                LogBLL.Error("HttpGetBmp", ex);
            }
            return bmp;
        }

        public static string OpenWebClient(string url, Encoding encoding)
        {
            WebClient webclient = new WebClient();

            Stream stream = webclient.OpenRead(url);
            StreamReader readStream = new StreamReader(stream, encoding);
            string str = readStream.ReadToEnd();
            return str;
        }


        public static void DownloadFile(string url, string fileName)
        {
            try
            {
                WebClient webclient = new WebClient();
                webclient.DownloadFile(new Uri(url), fileName);
            }
            catch (Exception ex)
            {
                Console.WriteLine("文件下载失败:" + ex);
            }

        }


    }
}
