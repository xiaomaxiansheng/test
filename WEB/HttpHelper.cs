using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.IO;
using System.IO.Compression;
using System.Text.RegularExpressions;

namespace CommonLib.Helper
{
    /// <summary>
    /// Http连接操作帮助类
    /// </summary>
    public class HttpHelper
    {
        public string GetHtml(string item)
        {
            Encoding encoding = Encoding.GetEncoding("utf-8");
            string postData = "{\"appCode\":\"102387\",\"minPrice\":\"102387\"}";
            //http://localhost:4915/Index.html
            string strUrl = "http://localhost:4915/ajax/SendRandomHandler.ashx?";
            byte[] data = encoding.GetBytes(postData);
            // 准备请求... {"mcstr":"è¾½ä¸­"}
            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(strUrl);
            myRequest.Method = "POST";
            myRequest.Referer = "http://esf.bxgfw.com/shop/admin/renthouse_pub.aspx";
            myRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            //myRequest.Headers.Add("X-AjaxPro-Method", "getSearchList");
            myRequest.ContentType = "application/json; charset=utf-8";

            //注意这里的Cookie进行更换成你的登录信息
            myRequest.Headers.Add("Cookie", "ASP.NET_SessionId=42jidiukrk5d1cugei0bhsup; cnzz_a2178262=6; sin2178262=; rtime=0; ltime=1294802416124; cnzz_eid=41664513-1294801319-http%3A//esf.bxgfw.com/shop/admin/renthouse_pub.aspx; lzstat_uv=38694578793119418740|1524099; lzstat_ss=3351439025_2_1294830879_1524099; user=hyloginstate=success!&hybh=G11011211150315&hylx=%b8%f6%c8%cb%bb%e1%d4%b1&hyzh=net_lover&hyyj=&hyjb=%c6%d5%cd%a8%bb%e1%d4%b1&hyjf=0&hysftgyz=1&hymc=%c0%ed%cf%eb&hylxdh=13910309166&hylxr=net_lover");
            myRequest.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 6.1; zh-CN; rv:1.9.2.13) Gecko/20101203 Firefox/3.6.13";
            myRequest.ContentLength = data.Length;
            Stream newStream = myRequest.GetRequestStream();
            // 发送数据 
            newStream.Write(data, 0, data.Length);
            newStream.Close();

            //得到服务器端的返回
            HttpWebResponse res = myRequest.GetResponse() as HttpWebResponse;
            StreamReader sr = new StreamReader(res.GetResponseStream(), Encoding.GetEncoding("utf-8"));
            string resultStr = sr.ReadToEnd();
            sr.Close();
            res.Close();
            return resultStr;

        }
    }

}
