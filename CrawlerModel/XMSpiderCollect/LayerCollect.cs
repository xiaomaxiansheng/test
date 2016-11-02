using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crawler.Model.XMSpiderCollect
{
    /// <summary>
    /// 精选集
    /// </summary>
    public class LayerCollect
    {
        public int Id { get; set; }

        /// <summary>
        /// Key Id
        /// </summary>
        public int KeyId { get; set; }

        /// <summary>
        /// 当前精选集页数
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// Url 收藏 Id
        /// </summary>
        public int ShowCollectId { get; set; }

        /// <summary>
        /// 精选集 名称
        /// </summary>
        public string ShowCollectName { get; set; }

        /// <summary>
        /// 标签
        /// </summary>
        public string Tags { get; set; }
        /// <summary>
        /// 播放次数
        /// </summary>
        public int PalyTimes { get; set; }
        /// <summary>
        /// 推荐次数
        /// </summary>
        public int RecommendTimes { get; set; }
        /// <summary>
        /// 收藏次数
        /// </summary>
        public int CollectTimes { get; set; }
    }
}
