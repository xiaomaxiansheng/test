using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IBussiness
{
    /// <summary>
    /// 多线程 计数器类
    /// </summary>
    public class ThreadCounter
    {
        /// <summary>
        /// 线程数量计数器
        /// </summary>
        private volatile int threadNumCounter = 0;
        /// <summary>
        /// 完成的数量
        /// </summary>
        private volatile int finishedCounter = 0;
        /// <summary>
        /// 未决定的数量
        /// </summary>
        private volatile int pendingCounter = 0;
        /// <summary>
        /// 失败的数量
        /// </summary>
        private volatile int errorCounter = 0;
        private volatile bool writerFlag = false;

        public void writerCounter(string action)
        {
            lock (this)
            {
                writerFlag = true;
                if (action == "fcAdd")
                {
                    pendingCounter--;
                    finishedCounter++;
                }
                else if (action == "ecAdd")
                {
                    pendingCounter--;
                    errorCounter++;
                }
                else if (action == "tcAdd")
                {
                    threadNumCounter++;
                }
                else if (action == "tcSub")
                {
                    threadNumCounter--;
                }
                if (action == "pcAdd")
                {
                    pendingCounter++;
                }
                writerFlag = false;
            }
        }

        public int getThreadNum(string action)
        {
            lock (this)
            {
                if (!writerFlag)
                {
                    if (action == "fc")
                    {
                        return finishedCounter;
                    }
                    else if (action == "tc")
                    {
                        return threadNumCounter;
                    }
                    else if (action == "pc")
                    {
                        return pendingCounter;
                    }
                    else if (action == "ec")
                    {
                        return errorCounter;
                    }
                }

                return -1;
            }
        }


    }
}
