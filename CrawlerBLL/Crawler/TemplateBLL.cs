using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Crawler.DAL.Crawler;
using System.IO;
using System.Xml;
namespace Crawler.BLL.Crawler
{
    public class TemplateBLL
    {  
        const int cateId = 1;
        public void MainWork()
        {
            TemplateDAL templateDal = new TemplateDAL();
            TestProjectDAL testProjectDal = new TestProjectDAL();
            int tid = 0;
            string filePath = @"D:\程序运行目录\Temp_XML";//获取这个 文件夹 下面的 所有的xml 文件,得到文件名, 文件名就是 tid

            DirectoryInfo directInfo = new DirectoryInfo(filePath);
            if (directInfo.Exists)
	        {
                FileInfo[] fileAttr = directInfo.GetFiles("*.xml");

                StringBuilder sb = new StringBuilder();
                foreach (FileInfo fileInfo in fileAttr)
                {
                    int.TryParse(fileInfo.Name.Replace(".xml", ""), out tid);
                    if (tid > 0)
                    {
                        string xml = File.ReadAllText(fileInfo.FullName, Encoding.UTF8);
                        XmlTemplate  xmlTemplate= new XmlTemplate(xml);
                        if (xmlTemplate.SiteName.ToLower()=="blog")
                        {
                            sb.Append("" + tid+",");
                        }
                        //Console.WriteLine(tid);
                        //testProjectDal.InserttestProject(1,1,tid,0);
                        //templateDal.ReadTemplateService(tid, cateId);
                    }
                   
                }

                Console.WriteLine(sb.ToString());
	        }
        }
    }

    public class XmlTemplate
    {
        public XmlTemplate() { }
        public XmlTemplate(string xmlText)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xmlText);
                XmlNode root = xmlDoc.SelectSingleNode("template");//查找<template> 

                Node = GetRegex(root, "Node");
                Title = GetRegex(root, "Title");
                SrcUrl = GetRegex(root, "SrcUrl");
                SiteEncoding = GetRegex(root, "SiteEncoding");
                InnerEncoding = GetRegex(root, "InnerEncoding");
                InnerContent = GetRegex(root, "Content");
                Layer = GetRegex(root, "Layer");
                // AuthorRegex = GetRegex(root, "Author");
                InnerDate = GetRegex(root, "ContentDate");
                SiteName = GetRegex(root, "SiteName");

                PageStart = GetPageNum(root, "PageStart");
                PageEnd = GetPageNum(root, "PageEnd");
            }
            catch (Exception ex)
            {

                LogNet.LogBLL.Error("XmlTemplate", ex);
            }


        }
        private static string GetRegex(XmlNode root, string nodeName)
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

        private static int GetPageNum(XmlNode root, string nodeName)
        {

            if (root.InnerXml.Contains(nodeName))
            {
                return int.Parse(root.SelectSingleNode(nodeName).InnerText.Trim());
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 模块
        /// </summary>
        public string Node { get; set; }

        /// <summary>
        /// 站点编码
        /// </summary>
        public string SiteEncoding { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Url
        /// </summary>
        public string SrcUrl { get; set; }



        /// <summary>
        /// 内容编码
        /// </summary>
        public string InnerEncoding { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string InnerContent { get; set; }

        /// <summary>
        /// 页面层级
        /// </summary>
        public string Layer { get; set; }

        /// <summary>
        /// 内容时间
        /// </summary>
        public string InnerDate { get; set; }

        public string SiteName { get; set; }

        /// <summary>
        /// 开始页
        /// </summary>
        public int PageStart { get; set; }
        /// <summary>
        /// 结束页
        /// </summary>
        public int PageEnd { get; set; }
    }
}
