using CommonLib.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WEB
{
    public partial class WebForm2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpHelper hp = new HttpHelper();
            
            Response.Write(hp.GetHtml(""));
            Response.Write("执行完毕");
            Response.End();
        }
    }
}