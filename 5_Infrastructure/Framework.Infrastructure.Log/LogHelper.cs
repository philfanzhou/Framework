using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Infrastructure.Log
{
    public class LogHelper
    {
        private string logFolder = Environment.CurrentDirectory + @"\Log\log\";
        private string logFile = "";
        public string LogFile
        {
            get { return logFile; }
        }

        /// <summary>
        /// 不带参数的构造函数
        /// </summary>
        public LogHelper()
        {
            logFile = Path.Combine(logFolder, string.Format("{0}.txt", DateTime.Now.ToString("yyyy-MM-dd")));
            CreateFolderIsNotExist(logFolder);
        }

        /// <summary>
        /// 带参数的构造函数
        /// </summary>
        /// <param name="logFile"></param>
        public LogHelper(string logFile)
        {
            this.logFile = logFile;
            this.logFolder = logFile.Substring(0, logFile.LastIndexOf('\\'));
            CreateFolderIsNotExist(logFolder);
        }

        private static void CreateFolderIsNotExist(string folder)
        {
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
        }

        /// <summary>
        /// 追加一条信息
        /// </summary>
        /// <param name="text"></param>
        public void Write(string text)
        {
            using (StreamWriter sw = new StreamWriter(logFile, true, Encoding.UTF8))
            {
                sw.Write(DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss] ") + text);
            }
        }
        /// <summary>
        /// 追加一条信息
        /// </summary>
        /// <param name="logFile"></param>
        /// <param name="text"></param>
        public void Write(string logFile, string text)
        {
            this.logFile = logFile;
            this.logFolder = logFile.Substring(0, logFile.LastIndexOf('\\'));
            CreateFolderIsNotExist(logFolder);

            using (StreamWriter sw = new StreamWriter(logFile, true, Encoding.UTF8))
            {
                sw.Write(DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss] ") + text);
            }
        }
        /// <summary>
        /// 追加一行信息
        /// </summary>
        /// <param name="text"></param>
        public void WriteLine(string text)
        {
            using (StreamWriter sw = new StreamWriter(logFile, true, Encoding.UTF8))
            {
                sw.WriteLine(DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss] ") + text);
            }
        }
        /// <summary>
        /// 追加一行信息
        /// </summary>
        /// <param name="logFile"></param>
        /// <param name="text"></param>
        public void WriteLine(string logFile, string text)
        {
            this.logFile = logFile;
            this.logFolder = logFile.Substring(0, logFile.LastIndexOf('\\'));
            CreateFolderIsNotExist(logFolder);

            using (StreamWriter sw = new StreamWriter(logFile, true, Encoding.UTF8))
            {
                sw.WriteLine(DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss] ") + text);
            }
        }
    }
}
