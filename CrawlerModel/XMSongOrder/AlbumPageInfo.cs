using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crawler.Model.XMSongOrder
{
    /// <summary>
    /// 虾米网站上,收索出来的 页 信息
    /// </summary>
    public class AlbumPageInfo
    {
        /// <summary>
        /// 年月对对应的Id
        /// </summary>
        public int YearMonthId { get; set; }
      
        private int pageIndex;
        /// <summary>
        /// 当前页数
        /// </summary>
        public int PageIndex {
            get { return pageIndex > 0 ? pageIndex : 1; }
            set {pageIndex = value;}
        }

        /// <summary>
        /// 当前页是否完成
        /// </summary>
        public int FinishStatus { get; set; }
    }
}
