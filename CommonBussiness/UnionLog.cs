using System;
using log4net;

[assembly: log4net.Config.XmlConfigurator(ConfigFile = "Logxml/logConfig.xml", Watch = true)]

namespace IBussiness
{
    public enum LogType
    {
        UNION_DEBUG = 1,
        UNION_ERROR = 2,
        UNION_INFO = 3,
        UNION_WARN = 4
    }

    /// <summary>
    /// 日志记录工具类
    /// </summary>
    public class UnionLog
    {
        #region 全局声明
        //日志对象
        static ILog log = LogManager.GetLogger(typeof(UnionLog));
        #endregion

        #region 默认构造
        private UnionLog()
        {
        }
        #endregion

        #region 方法
        public static void WriteLog(LogType type, object message, Exception exception)
        {
            switch (type)
            {
                case LogType.UNION_DEBUG:
                    log.Debug(message, exception);
                    break;
                case LogType.UNION_ERROR:
                    UnionLog.WriteLog(LogType.UNION_INFO, message, exception);
                    break;
                case LogType.UNION_INFO:
                    log.Info(message, exception);
                    break;
                case LogType.UNION_WARN:
                    log.Warn(message, exception);
                    break;
                default:
                    break;
            }
        }

        public static void WriteLog(LogType type, object message)
        {
            switch (type)
            {
                case LogType.UNION_DEBUG:
                    log.Debug(message);
                    break;
                case LogType.UNION_ERROR:
                    UnionLog.WriteLog(LogType.UNION_INFO, message);
                    break;
                case LogType.UNION_INFO:
                    log.Info(message);
                    break;
                case LogType.UNION_WARN:
                    log.Warn(message);
                    break;
                default:
                    break;
            }
        }

        #endregion
    }
}
