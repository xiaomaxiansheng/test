using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crawler.Model.XMSongOrder
{
    public class SingerInfo
    {
        public int SingerId { get; set; }
        public string SingerName { get; set; }
        public string SingerDesc { get; set; }
        public string SingerAddress { get; set; }
        public string  SingerXmId { get; set; }

        //@singerName,@singerdesc,@singeraddress,@singerxmid

        public string SingerImgPath { get; set; }
        public string SingerType { get; set; }


    }
}
