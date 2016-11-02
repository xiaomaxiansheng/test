using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crawler.Model.XMSongOrder
{
    public class SpiderTimeInfo
    {
        private static int spiderTimeId;

        /// <summary>
        /// 抓取的批次ID
        /// </summary>
        public static int SpiderTimeId
        {
            get
            {
                return spiderTimeId > 0 ? spiderTimeId : 1;
            }
            set{
                spiderTimeId = value;
            }
        }

        /// <summary>
        /// 抓取的类型,比如 CH 表示中文 ,EN 表示英文
        /// </summary>
        public static string SpiderType { get; set; }


        /// <summary>
        /// 抓取的时间
        /// </summary>
        public static string ClawTime { get; set; }

    }
}
