using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crawler.Model.XMSongOrder
{
    public class AlbumInfo
    {
        public int AlbumId { get; set; }
        public string AlbumName { get; set; }
        public string AlbumSinger { get; set; }
        public string AlbumDesc { get; set; }
        public string AlbumImgPath { get; set; }
        
        public string AlbumStyle { get; set; }
        public string AlbumType { get; set; }
        public string AlbumXmId { get; set; }

        //@albumname,@albumsinger,@albumdesc,@albumstyle,@albumtype,@albumxmid
            
    }
}
