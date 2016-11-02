using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crawler.Model.SpiderImg
{
    /// <summary>
    /// 图片收藏表
    /// </summary>
    public class ImgCollInfo
    {
        public int Id { get; set; }
        public string  SiteUrl { get; set; }
        public string  SaveFileName { get; set; }
        public int ImgWidth { get; set; }
        public int ImgHeight { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
