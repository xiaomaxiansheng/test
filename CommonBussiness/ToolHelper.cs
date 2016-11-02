using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Configuration;
namespace IBussiness
{
    public class ToolHelper
    {
        private static string appConfigPath;
        /// <summary>
        /// 赋值的时候, value 为程序集配置文件的名称
        /// </summary>
        public static string  AppConfigPath 
        {
            get{return appConfigPath;}
            set { appConfigPath = AppDomain.CurrentDomain.BaseDirectory + "\\" + value + ".exe.config"; }
        } 

        public static string ReadAppSettings(string AppKey)
        {
            return ConfigurationManager.AppSettings[AppKey].ToString();
        }
        public static void AppSetValue(string AppKey, string AppValue)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(AppConfigPath);
            XmlNode xmlNode = xmlDocument.SelectSingleNode("//appSettings");
            XmlElement xmlElement = (XmlElement)xmlNode.SelectSingleNode("//add[@key='" + AppKey + "']");
            if (xmlElement != null)
            {
                xmlElement.SetAttribute("value", AppValue);
            }
            else
            {
                XmlElement xmlElement2 = xmlDocument.CreateElement("add");
                xmlElement2.SetAttribute("key", AppKey);
                xmlElement2.SetAttribute("value", AppValue);
                xmlNode.AppendChild(xmlElement2);
            }
            xmlDocument.Save(AppConfigPath);
        }
    }
}
