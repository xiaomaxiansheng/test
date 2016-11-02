using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crawler.Model.XMSpiderCollect
{
    public class CollectSongReport
    {

        public string KeyId { get; set; }
        public string  CateName { get; set; }
        public string ShowCollectId { get; set; }
        public string ShowCollectName { get; set; }

        public string Tags { get; set; }
        public string SongName { get; set; }
        public string Singer { get; set; }
        public string XM_SongId { get; set; }
    }
}
