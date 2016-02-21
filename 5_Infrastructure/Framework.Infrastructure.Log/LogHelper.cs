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
        private string _baseFolder = Environment.CurrentDirectory + @"\Log\";
        private string _logFileName = string.Format("{0}.txt", DateTime.Now.ToString("yyyy-MM-dd"));

        private string _logFolder = "";
        public string LogFolder
        {
            get { return _logFolder; }
        }

        private string _logFile = "";
        public string LogFile
        {
            get { return _logFile; }
        }

        private static LogHelper _instance = new LogHelper();
        public static LogHelper Logger
        {
            get { return _instance; }
        }

        /// <summary>
        /// 不带参数的构造函数
        /// </summary>
        public LogHelper()
        {
            _logFolder = _baseFolder;
            _logFile = Path.Combine(_logFolder, _logFileName);
            CreateFolderIsNotExist(_logFolder);
        }

        /// <summary>
        /// 带参数的构造函数
        /// </summary>
        /// <param name="subFolders"></param>
        public LogHelper(params string[] subFolders)
        {
            _logFolder = _baseFolder = CombineFolders(subFolders);
            _logFile = Path.Combine(_logFolder, _logFileName);
            CreateFolderIsNotExist(_logFolder);
        }

        private static void CreateFolderIsNotExist(string folder)
        {
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
        }

        private string CombineFolders(params string[] subFolders)
        {
            return Path.Combine(_baseFolder, Path.Combine(subFolders));
        }

        private void UpdateLogPath(params string[] subFolders)
        {
            _logFolder = CombineFolders(subFolders);
            _logFile = Path.Combine(_logFolder, _logFileName);
            CreateFolderIsNotExist(_logFolder);
        }

        /// <summary>
        /// 追加一条信息
        /// </summary>
        /// <param name="text"></param>
        /// <param name="subFolders"></param>
        public void Write(string text, params string[] subFolders)
        {
            UpdateLogPath(subFolders);
            using (StreamWriter sw = new StreamWriter(_logFile, true, Encoding.UTF8))
            {
                sw.Write(DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss] ") + text);
            }
        }
        /// <summary>
        /// 追加一条信息
        /// </summary>
        /// <param name="logFile"></param>
        /// <param name="text"></param>
        public void WriteToFile(string logFile, string text)
        {
            this._logFile = logFile;
            this._logFolder = logFile.Substring(0, logFile.LastIndexOf('\\'));
            CreateFolderIsNotExist(_logFolder);

            using (StreamWriter sw = new StreamWriter(_logFile, true, Encoding.UTF8))
            {
                sw.Write(DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss] ") + text);
            }
        }
        /// <summary>
        /// 追加一行信息
        /// </summary>
        /// <param name="text"></param>
        /// <param name="subFolders"></param>
        public void WriteLine(string text, params string[] subFolders)
        {
            UpdateLogPath(subFolders);
            using (StreamWriter sw = new StreamWriter(_logFile, true, Encoding.UTF8))
            {
                sw.WriteLine(DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss] ") + text);
            }
        }
        /// <summary>
        /// 追加一行信息
        /// </summary>
        /// <param name="logFile"></param>
        /// <param name="text"></param>
        public void WriteLineToFile(string logFile, string text)
        {
            this._logFile = logFile;
            this._logFolder = logFile.Substring(0, logFile.LastIndexOf('\\'));
            CreateFolderIsNotExist(_logFolder);

            using (StreamWriter sw = new StreamWriter(_logFile, true, Encoding.UTF8))
            {
                sw.WriteLine(DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss] ") + text);
            }
        }        
    }
}
