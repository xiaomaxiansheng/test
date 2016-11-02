using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crawler.Model.XMSongOrder
{
    public class AlbumSongInfo
    {
      
       /// <summary>
       /// 抓取的批次
       /// </summary>
        public SpiderTimeInfo SpiderTimeInfo { get; set; }

        public AlbumInfo AlbumInfo { get; set; }

        public SingerInfo SingerInfo { get; set; }

        public List<string> SongNameList { get; set; }
        public List<string> XM_SongIdList{ get; set; }
       

        /// <summary>
        /// 语种
        /// </summary>
        public string SongLanguage { get; set; }

        /// <summary>
        /// 唱片公司
        /// </summary>
        public string OriRightsHolder { get; set; }

        /// <summary>
        /// 专辑类别
        /// </summary>
        public string AlbumType { get; set; }

        /// <summary>
        /// 专辑风格
        /// </summary>
        public string AlbumStyle { get; set; }


        /// <summary>
        /// 哪一页被抓进来的数据,取得是 ModelArgs的全局变量(不能跨页使用多线程)
        /// </summary>
        public int PageIndex { get; set; }

        private string lyricTxt; 
        /// <summary>
        /// 不带时间戳的歌词
        /// </summary>
        public string LyricTxt {

            get { return lyricTxt ?? ""; }
            set{lyricTxt = value;} 

        }

        /// <summary>
        /// 出版时间
        /// </summary>
        public DateTime PublishDate { get; set; }
           
        public int FlowStatus { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime UpdateDate { get; set; }
      
       
    }
}
