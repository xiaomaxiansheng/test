using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crawler.Model.XMSpiderCollect
{
    /// <summary>
    /// 精选集分类
    /// </summary>
    public class CollectCate
    {
        public int Id { get; set; }
       /// <summary>
       /// Key Id
       /// </summary>
        public int KeyId { get; set; }
       /// <summary>
       /// L Id
       /// </summary>
        public int LId { get; set; }
       /// <summary>
       /// 精选集分类
       /// </summary>
        public string CateName { get; set; }

       /// <summary>
       ///  当前 精选集分类页数
       /// </summary>
        public int PageIndex { get; set; }

    }
}
