using System;
using System.IO;
using Framework.Infrastructure.Log;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.Infrastructure.Log
{
    [TestClass]
    public class UnitTest1
    {
        private void CleanupLogFile(string file)
        {
            if (File.Exists(file))
            {
                File.Delete(file);
            }
        }

        private void CleanupLogFolder(string folder)
        {
            if (Directory.Exists(folder))
            {
                Directory.Delete(folder, true);
            }
        }

        [TestMethod]
        public void TestMethod1()
        {
            CleanupLogFolder(LogHelper.Logger.LogFolder);
            LogHelper.Logger.Write("通过单例访问写入日志");            
            LogHelper.Logger.WriteLine("通过单例访问写入一行日志");
            
            LogHelper.Logger.Write("通过单例访问写入日志", "ExampleType1");
            LogHelper.Logger.WriteLine("通过单例访问写入一行日志", "ExampleType2");

            string logFile1 = Environment.CurrentDirectory + @"\Log\Type1\LogTest1.txt";
            CleanupLogFile(logFile1);
            LogHelper.Logger.WriteToFile(logFile1, "按照指定日志文件，通过单例访问写入日志");
            LogHelper.Logger.WriteLineToFile(logFile1, "按照指定日志文件，通过单例访问写入一行日志");

            //----------------------------------------------------------------------------------//
            LogHelper logger = new LogHelper("SubLgoExample");

            logger.Write("通过单例访问写入日志");
            logger.WriteLine("通过单例访问写入一行日志");

            logger.Write("通过单例访问写入日志", "ExampleType3");
            logger.WriteLine("通过单例访问写入一行日志", "ExampleType4");

            string logFile2 = Environment.CurrentDirectory + @"\Log\SubLgoExample\Type2\LogTest2.txt";
            CleanupLogFile(logFile2);

            logger.WriteToFile(logFile2, "按照指定日志文件，通过单例访问写入日志");
            logger.WriteLineToFile(logFile2, "按照指定日志文件，通过单例访问写入一行日志");
        }        
    }
}
