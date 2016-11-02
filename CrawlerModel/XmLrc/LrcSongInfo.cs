using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crawler.Model.XmLrc
{
    public class LrcSongInfo
    {
        /// <summary>
        /// 虾米歌曲Id
        /// </summary>
        public string XM_SongId { get; set; }

        /// <summary>
        /// 歌曲名称
        /// </summary>
        public string SongName { get; set; }

        /// <summary>
        /// 所属专辑
        /// </summary>
        public string BelongAlbum { get; set; }

        /// <summary>
        /// 演唱者
        /// </summary>
        public string Singer { get; set; }

        /// <summary>
        /// 词作者
        /// </summary>
        public string Lyricist { get; set; }

        /// <summary>
        /// 曲作者
        /// </summary>
        public string Composer { get; set; }


        /// <summary>
        /// 文本歌词
        /// </summary>
        public string LrcText { get; set; }

        /// <summary>
        /// 歌手图片Url
        /// </summary>
        public string ArtistImgUrl { get; set; }

        /// <summary>
        /// 专辑图片Url
        /// </summary>
        public string AlbumImgUrl { get; set; }

        public string AlbumUrl { get; set; }

        public string ArtistUrl { get; set; }

        /// <summary>
        /// 带时间戳 歌词
        /// </summary>
        public string Lyric { get; set; }

    }
}