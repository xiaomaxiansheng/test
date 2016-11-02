using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Xml;
using System.Net;

namespace IBussiness
{
    public static class CommonHelper
    {

        /// <summary>
        /// 以1900年1月 作为基准数据1
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public static int FormatYearMonth(int year, int month)
        {
            int yearMonth = (year - 1900) * 12 + month;
            return yearMonth;
        }
        public static  void FormatYearMonth(int yearMonth, out int year, out int month)
        {
            year = (yearMonth / 12) + 1900;
            month = yearMonth % 12;
        }

        public static List<string> MatchRegexList(string content, string strRegex, string replace = "")
        {
            List<string> macthList = new List<string>();
            Regex reg = new Regex(strRegex, RegexOptions.IgnoreCase | RegexOptions.Multiline);
            MatchCollection mc = reg.Matches(content);
            if (mc.Count > 0)
            {
                for (int i = 0; i < mc.Count; i++)
                {
                    string mess = mc[i].ToString();
                    if (replace != "")
                    {
                        mess = mess.Replace(replace, "");
                    }
                    macthList.Add(mess);
                }
            }
            return macthList;
        }

        /// <summary>
        /// 全字符匹配, 例如 id/121212121  id/\d+  , id/\d{1,10} , 这种得到全部的id/121212121
        /// </summary>
        /// <param name="strRegex"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string MatchRegexFirst(string strRegex,string content)
        {
            Regex reg = new Regex(strRegex, RegexOptions.IgnoreCase);
            MatchCollection mc = reg.Matches(content);
            if (mc.Count > 0)
            {
                return mc[0].Groups[0].ToString();//得到第一个匹配的结果
            }
            else
            {
                return "";
            }
        }

        

        /// <summary>
        /// 通过两次精确匹配
        /// </summary>
        /// <param name="patten1"></param>
        /// <param name="inputMatchHtml"></param>
        /// <param name="strPatton2"></param>
        /// <param name="StrRestlt"></param>
        public static string GetMatchRegex(string patten1, string inputMatchHtml, string strPatton2)
        {
            string StrRestlt = "";
            if (patten1 != null)
            {
                Regex regex = new Regex(patten1, RegexOptions.Singleline | RegexOptions.IgnoreCase);
                MatchCollection mc = regex.Matches(inputMatchHtml);
                if (mc.Count > 0)
                {
                    if (strPatton2 == null || strPatton2 == "")
                    {
                        //一次匹配 就得到了结果
                        StrRestlt = mc[0].Groups[0].Value;
                        return StrRestlt;
                    }
                    //需要精确匹配
                    regex = new Regex(strPatton2, RegexOptions.Singleline | RegexOptions.IgnoreCase);
                    string strInit = mc[0].Groups[0].Value;
                    mc = regex.Matches(strInit);
                    if (mc.Count > 0)
                    {
                        StrRestlt = mc[0].Groups[0].Value.Trim();
                    }
                }
            }
            return StrRestlt;
        }


        /// <summary>
        /// 得到第一个(.+?)匹配项,匹配到的数据
        /// </summary>
        /// <param name="regexStr">带(.+?)的正则表达式</param>
        /// <param name="inputHtml">Html源代码</param>
        /// <returns></returns>
        public static string GetMatchRegex(string regexStr, string inputHtml)
        {
            if (!string.IsNullOrEmpty(regexStr))
            {
                Regex regex = new Regex(regexStr, RegexOptions.Singleline | RegexOptions.IgnoreCase);
                if (regex.IsMatch(inputHtml))
                {
                    MatchCollection mc = regex.Matches(inputHtml);
                    if (mc.Count > 0)
                    {
                        return mc[0].Groups[1].Value;
                    }
                }
            }
            return "";
        }

        /// <summary>
        /// 得到所有匹配到的字符
        /// </summary>
        /// <param name="regexStr">带(.+?)的正则表达式</param>
        /// <param name="inputHtml">Html源代码</param>
        /// <returns>string</returns>
        public static string GetMatchRegexFull(string regexStr, string inputHtml)
        {
            StringBuilder sbulider = new StringBuilder();
            if (!string.IsNullOrEmpty(regexStr))
            {
                Regex regex = new Regex(regexStr, RegexOptions.Singleline | RegexOptions.IgnoreCase);
                if (regex.IsMatch(inputHtml))
                {
                    MatchCollection mc = regex.Matches(inputHtml);
                    for (int i = 0; i < mc.Count; i++)
                    {
                        sbulider.Append(mc[i].Value);
                    }
                }
            }
            return sbulider.ToString();
        }

        /// <summary>
        /// 得到所有(.+?)匹配到的集合
        /// </summary>
        /// <param name="regexStr">带(.+?)的正则表达式</param>
        /// <param name="inputHtml">Html源代码</param>
        /// <returns></returns>
        public static List<string> GetMatchRegexList(string regexStr, string inputHtml)
        {
            List<string>list = new List<string>();
            StringBuilder sbulider = new StringBuilder();
            if (!string.IsNullOrEmpty(regexStr))
            {
                Regex regex = new Regex(regexStr, RegexOptions.Singleline | RegexOptions.IgnoreCase);
                if (regex.IsMatch(inputHtml))
                {
                    MatchCollection mc = regex.Matches(inputHtml);
                    for (int i = 0; i < mc.Count; i++)
                    {
                        list.Add(mc[i].Groups[0].Value);
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// 去除所有Html标签,以及换行,制表符
        /// </summary>
        /// <param name="Htmlstring">要格式化的字符串</param>
        /// <returns></returns>
        public static string NoHTML(string Htmlstring) //去除HTML标记   
        {
            //删除脚本   
            Htmlstring = Regex.Replace(Htmlstring, @"<script.+?</script>", "", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            //删除HTML   
            Htmlstring = Regex.Replace(Htmlstring, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);

            // Htmlstring = Regex.Replace(Htmlstring, @"([/r/n])[/s]+", "", RegexOptions.IgnoreCase);

            Htmlstring = Regex.Replace(Htmlstring, @"-->", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);

            Htmlstring = Regex.Replace(Htmlstring, @"&(quot|#34);", "/", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(nbsp|#160);", " ", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(iexcl|#161);", "/xa1", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(cent|#162);", "/xa2", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(pound|#163);", "/xa3", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(copy|#169);", "/xa9", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&#(/d+);", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"\r\n|\n|\t", "");

            Htmlstring.Replace("<", "");
            Htmlstring.Replace(">", "");
            Htmlstring.Replace("/r/n", "");
            Htmlstring.Replace(@"\r", "").Replace(@"\n", "");
            return Htmlstring;
        }

        /// <summary>
        /// 得到Url全路径
        /// </summary>
        /// <param name="strUrl">页面源代码里面的Url,相对路径或者绝对路径</param>
        /// <param name="parentUrl">在哪个网址抓到的连接,绝对Url</param>
        /// <returns></returns>
        public static string GetUrl(string strUrl, string parentUrl)
        {
            #region 得到站点Url

            string resultUrl = "";

            if (strUrl != "")
            {
                if (strUrl.IndexOf("http://") != -1)
                {
                    resultUrl = strUrl;
                }
                else
                {
                    if (strUrl.IndexOf('/') == 0)
                    {
                        if (parentUrl.IndexOf('?') > 0)
                        {
                            int count = Regex.Matches(strUrl, "/").Count;
                            parentUrl = parentUrl.Substring(0, parentUrl.IndexOf('?'));
                            for (int i = 0; i < count; i++)
                            {
                                parentUrl = parentUrl.Substring(0, parentUrl.LastIndexOf('/'));
                            }
                        }
                        else
                        {
                            int count = Regex.Matches(strUrl, "/").Count;
                            for (int i = 0; i < count; i++)
                            {
                                parentUrl = parentUrl.Substring(0, parentUrl.LastIndexOf('/'));
                            }
                        }
                        parentUrl = parentUrl + "/";

                        resultUrl = parentUrl + strUrl.Substring(1);
                    }
                    else if (strUrl.IndexOf("../") == 0)
                    {
                        parentUrl = parentUrl.Substring(0, parentUrl.Length - 1);
                        int count = Regex.Matches(strUrl, "../").Count;
                        for (int i = 0; i < count; i++)
                        {
                            parentUrl = parentUrl.Substring(0, parentUrl.LastIndexOf('/'));

                        }
                        parentUrl = parentUrl + "/";
                        resultUrl = parentUrl + strUrl.Replace("../", "");
                    }
                    else if (strUrl.IndexOf('?') > 0)
                    {
                        parentUrl = parentUrl.Substring(0, parentUrl.IndexOf('?'));
                        parentUrl = parentUrl.Substring(0, parentUrl.LastIndexOf('/') + 1);
                        //域名
                        resultUrl = parentUrl + strUrl;
                    }
                    else if (parentUrl.Substring(parentUrl.LastIndexOf('/') + 1).IndexOf(".") > 0)
                    {
                        parentUrl = parentUrl.Substring(0, parentUrl.LastIndexOf('/') + 1);
                        //域名
                        resultUrl = parentUrl + strUrl;
                    }
                    else if (strUrl.IndexOf('/') > 0)
                    {
                        //截取域名
                        int count = Regex.Matches(strUrl, "/").Count;
                        for (int i = 0; i < count; i++)
                        {
                            parentUrl = parentUrl.Substring(0, parentUrl.LastIndexOf('/'));
                        }
                        parentUrl = parentUrl + "/";
                        resultUrl = parentUrl + strUrl;
                    }

                    else
                    {
                        resultUrl = parentUrl + strUrl;
                    }
                }
            }
            else
            {
                return "";
            }

            #endregion
            return resultUrl;

        }

        /// <summary>
        /// 將可能為相對路徑轉成絕對路徑
        /// </summary>
        /// <param name="checkURL">相對路徑/絕對路徑</param>
        /// <param name="baseURL">網站</param>
        /// <returns></returns>
        public static string ConvertToAbsoluteURL(string checkURL,string baseURL)
        {
            Uri result;
            var res = Uri.TryCreate(new Uri(baseURL, UriKind.Absolute), new Uri(checkURL, UriKind.RelativeOrAbsolute), out  result);
            if (res)
            {
                return result.AbsoluteUri;
            }
            else
            {
                return checkURL;
            }
        }

        /// <summary>
        /// 輸入網址原始碼，透過refex 將所有網址找出來
        /// </summary>
        /// <returns> 所有抓取到的網址 </returns>
        public static string[] GetLinkUrlsFromPath(string htmlSource)
        {
            List<string> res = new List<string>();
            var regex = new Regex(@"(?:href\s*=)(?:[\s""']*)(?!#|mailto|location.|javascript)(?<PARAM1>.*?)(?:[\s>""'])",
                RegexOptions.IgnoreCase);
            MatchCollection matches = regex.Matches(htmlSource);
            foreach (Match match in matches)
            {
                //將href=" 濾掉     
                // res.Add(match.Groups[0].Value.Substring(6, match.Groups[0].Value.Length-7));   
                res.Add(match.Groups["PARAM1"].Value);
            } return res.ToArray();
        }

        /// <summary>
        /// 從網路上取得原始碼
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetSourceFromUrl(string url)
        {
            WebClient client = new WebClient();
            //以防萬一 模擬自己為瀏覽器  
            client.Headers.Add("User-Agent: Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/536.5 (KHTML, like Gecko) Chrome/19.0.1084.56 Safari/536.5");
            client.Headers.Add("Accept: text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
            client.Headers.Add("Accept-Encoding: identity");
            client.Headers.Add("Accept-Language: zh-TW,en;q=0.8");
            client.Headers.Add("Accept-Charset: utf-8;q=0.7,*;q=0.3");
            client.Headers.Add("ContentType", "application/x-www-form-urlencoded");
            return client.DownloadString(url);
        }


        /// <summary>
        ///  得到匹配到的时间
        /// </summary>
        /// <param name="innerdate">匹配到的时间,例如 格式为 yyyy-MM-dd (HH:mm),yy-MM-dd (HH:mm),MM-dd (HH:mm),</param>
        /// <returns></returns>
        public static DateTime GetInnerData(string innerdate)
        {
            DateTime time;
            if (innerdate.Length < 3)
            {
                //格式是 HH:mm
                time = DateTime.Now;
                return time;
            }
            else if (innerdate.Contains("-"))
            {
                // 格式为 MM-dd HH:mm
                if (innerdate.Split('-').Length <= 2)
                {
                    //格式 可能是 MM-dd hh:mm  07-12 07:50 ,我们要 补上年份
                    innerdate = DateTime.Now.ToString("yyyy-") + innerdate;
                    DateTime.TryParse(innerdate, out time);
                }
                DateTime.TryParse(innerdate, out time);
                if (time.Ticks > 100000)
                {
                    return time;
                }
                else
                {
                    return DateTime.Now;
                }
            }
            else if (innerdate.Length >= 10)
            {
                //格式为 yyyy-MM-dd
                DateTime.TryParse(innerdate, out time);
                if (time.Ticks > 100000)
                {
                    return time;
                }
                else
                {
                    return DateTime.Now;
                }
            }
            else
            {
                time = DateTime.Now;
                return time;
            }
        }

        /// <summary>
        /// 得到唯一的文件名
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetUniqueNameByStoreFile(string filePath, string fileName)
        {
            if (filePath == null || filePath.Trim() == "" || fileName == null || fileName.Trim() == "")
            {
                return String.Empty;
            }
            string fileFullPath = Path.Combine(filePath, fileName);
            while (File.Exists(fileFullPath))
            {
                fileName = DateTime.Now.ToString("_yyMMdd_HHmmss_fff_") + fileName.Substring(fileName.LastIndexOf('.')-1);
                fileFullPath = Path.Combine(filePath, fileName);
            }
            return fileName;
        }

        public static string DateDiff(DateTime DateTime1, DateTime DateTime2)
        {
            TimeSpan timeSpan = new TimeSpan(DateTime1.Ticks);
            TimeSpan ts = new TimeSpan(DateTime2.Ticks);
            TimeSpan timeSpan2 = timeSpan.Subtract(ts).Duration();
            int arg_34_0 = timeSpan2.Hours;
            int arg_3C_0 = timeSpan2.Minutes;
            int arg_44_0 = timeSpan2.Seconds;
            return string.Format("本次运行耗时:{0}小时{1}分{2}秒", timeSpan2.Hours, timeSpan2.Minutes, timeSpan2.Seconds);
        }

        public static string GetXmlNode(XmlNode root, string nodeName)
        {
            try
            {
                return root.SelectSingleNode(nodeName).InnerText.Trim();
            }
            catch
            {
                return "";
            }

        }
        public static string GetXmlAttribute(XmlNode xmlNode,string attrName)
        {
            try
            {
                return xmlNode.Attributes[attrName].Value;
            }
            catch
            {
                return "";
            }
        }


        public static void GetAllFileByDirectFile()
        {
            string path = @"D:\所有模板_367个";


            DirectoryInfo dir = new DirectoryInfo(path);
            List<int> list = new List<int>();
            if (dir.Exists)
            {
               foreach (FileInfo file in dir.GetFiles("*.xml"))
               {
                   string filename = file.Name.Replace(".xml", "");
                   list.Add(int.Parse(filename));
                   
               }
            }
            else
            {
                Console.WriteLine("文件夹路径不存在");
            }
            list.Sort();
            for (int i = 0; i < list.Count; i++)
            {
                Console.WriteLine(list[i]);
            }
           
            Console.WriteLine("完毕");
        }
    }
}
