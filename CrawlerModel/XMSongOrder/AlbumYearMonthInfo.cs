using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crawler.Model.XMSongOrder
{
    /// <summary>
    /// 虾米 专辑搜索 的 年月时间
    /// </summary>
    public class AlbumYearMonthInfo
    {
       
        /// <summary>
        /// 自增时间Id,没什么特别的用处
        /// </summary>
        public int YearMonthId { get; set; }
        /// <summary>
        /// 任务正在进行的 年月,计算方式 以1900年1月 为1,每过一月,数字增长1,
        /// 例如 120表示1910年1月代表的时间即是 1910年1月
        /// </summary>
        public int YearMonth { get; set; }
    
        /// <summary>
        /// 这一个月的抓取是否完成
        /// </summary>
        public int FinishStatus { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }

        public int SpiderTimeId { get; set; }
    }
}
