using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IBussiness;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace WEB
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var gbk = Encoding.GetEncoding("gbk").CodePage;
            //var url = "http://test.telefen.com/HGSecKill/ajax/OrderCreate.ashx?";
            var url = "http://localhost:4915/ajax/SendRandomHandler.ashx";

            CookieContainer myCookieContainer = new CookieContainer();
            //var cookieStr = myCookieContainer.GetCookies(new Uri("http://test.telefen.com")).Replace(" ", "");
            //cookieStr = cookieStr.Replace(";", ",");

            //var cc = new CookieContainer();
            //cc.SetCookies(new Uri("http://test.telefen.com"), cookieStr);


            //string cookieValue = "Set－Cookie:UserToken=VA4vpmdbKOw8HYSR3Z03VrKyaRoLGCHlcC+IdrtX9PUTV8TtU1ZWekFDxedUuAVFCnl5iQQfXyKv6QKPXJ0iEp5sbMuzWcVpaLGMMZoA+ObrvQ/g/E1+svNxv++sUV1p;Expires=2016-10-28T12:58:15.548Z;Path=/;Domain=test.telefen.com;";
            //myCookieContainer.SetCookies(new Uri("http://test.telefen.com"), cookieValue);
            //Parallel.For(0, 1000, item =>
            //{
            string nohtmlStr = HttpHelper.HttpJsonPost(url, "appCode=102387");
            UnionLog.WriteLog(LogType.UNION_INFO, "接口返回：" + nohtmlStr);
            //});

            Response.Write("测试结束！");
        }
    }
}