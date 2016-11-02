using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crawler.Model.XMSongOrder
{
    public class HttpWrongInfo
    {
        public int WrongId { get; set; }
        public int SpiderTimeId { get; set; }
        public int YearMonthId { get; set; }
        public string AlbumUrl { get; set; }
        public int ClawTimes { get; set; }
        public int FinishStatus { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
