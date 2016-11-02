using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crawler.Model.XMSpiderCollect
{
    /// <summary>
    /// 歌曲信息
    /// </summary>
    public class CollectSongInfo
    {
        public int Id { get; set; }
        /// <summary>
        /// Url ID
        /// </summary>
        public int ShowCollectId { get; set; }
        /// <summary>
        /// 虾米songId
        /// </summary>
        public int XM_SongId { get; set; }
        /// <summary>
        /// 歌曲名
        /// </summary>
        public string SongName { get; set; }
        /// <summary>
        /// 歌手名
        /// </summary>
        public string Singer { get; set; }
    }
}
