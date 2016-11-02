using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Crawler.DAL
{
    public static class SystemConst
    {
        private static string connStr;
        public static string ConnStr
        {
            get {
                return connStr??ConfigurationManager.ConnectionStrings["ConnStr"].ToString();
                    //"server=127.0.0.1;database=XM_Spider;uid=sa;pwd=sa;";
            }
            set {
                connStr = value;
            }
        }
    }
}
