using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
        
namespace Crawler.Model.XMSongOrder
{
    /// <summary>
    /// 全局静态变量
    /// </summary>
    public class ModelArgs
    {
        private static string albumListType;

        /// <summary>
        ///  虾米网站的请求专辑 分语言的时候使用
        ///  "http://www.xiami.com/album/list/type/语言类别/year/2012/month/12/p//page/2;
        /// </summary>
        public static string AlbumListType {
            get{
                if (SpiderTimeInfo.SpiderType=="CH")
                {
                    albumListType = "huayu";
                }
                else if (SpiderTimeInfo.SpiderType == "EN")
                {
                    albumListType = "oumei";
                }
                else
                {
                    //其他类型在这里接着修改
                    albumListType = "huayu";
                }
                return albumListType;
            }
        
        } //huayu //oumei
        public static bool ISCheckContinue;

        public static int AllTaskCount;
        /// <summary>
        /// 已经完成的任务
        /// </summary>
        public static int HavingCompleted = 0;
        public static int TaskPageSize=1000;
        public static int StartYearMonth;
        public static int EndYearMonth;

        public static int MaxThreadNum = 0;
        public static string RunType { get; set; }

        /// <summary>
        /// 存放 XM_AlbumSong 的PageIndex,
        /// 根据 publishDate 得到 在虾米网上 哪一年哪一月,哪一页抓进来的数据,
        /// 这个字段是为了 可以继续上次的任务运行
        /// 格式为: yraemonthId_PageIndex
        /// 
        /// </summary>
        public static string FromPage { get; set; }

    }
}
