using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crawler.Model.XMSongOrder
{
    public class AlbumStartArgsInfo
    {
      
       /// <summary>
       /// 开始的年份时间
       /// </summary>
        public static int StartYear { get; set; }
       /// <summary>
       /// 开始的月份时间
       /// </summary>
        public static int StartMonth { get; set; }
       /// <summary>
       /// 结束的年份时间
       /// </summary>
        public static int EndYear { get; set; }
       /// <summary>
       /// 结束的月份时间
       /// </summary>
        public static int EndMonth { get; set; }
       /// <summary>
       /// 抓取过程中延时的时间
       /// </summary>
        public static int DelayTime { get; set; }
       /// <summary>
       /// 完成的状态
       /// </summary>
        public static int FinishStatus { get; set; }

    }
}
