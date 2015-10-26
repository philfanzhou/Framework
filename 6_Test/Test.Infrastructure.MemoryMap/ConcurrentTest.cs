using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Framework.Infrastructure.MemoryMap;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using System.IO;

namespace Test.Infrastructure.MemoryMap
{
    public partial class MemoryMappedFileTest
    {
        private FileFactory fileFactory = new FileFactory();

        //[TestMethod]
        //public void TestMultiCreate()
        //{
        //    string fileName = "TestCreateConcurrentFile.dat";
        //    // 清理环境
        //    string filePath = GetFilePath(fileName);
        //    if(File.Exists(filePath))
        //    {
        //        File.Delete(filePath);
        //    }

        //    ManualResetEvent eve = new ManualResetEvent(false);
        //    List<Task> taskList = new List<Task>();
        //    List<ConcurrentDataFile> fileList = new List<ConcurrentDataFile>();
        //    int fileCount = 100;
        //    for (int i = 0; i < fileCount; i++)
        //    {
        //        taskList.Add(Task.Factory.StartNew(() =>
        //        {
        //            eve.WaitOne();

        //            string path = CreateFileUseFactory(fileName);
        //            fileList.Add(fileFactory.Open(path));
        //        }));
        //    }

        //    eve.Set();
        //    try
        //    {
        //        Task.WaitAll(taskList.ToArray());
        //    }
        //    finally
        //    {
        //        Assert.AreEqual(fileCount, fileList.Count);

        //        ConcurrentDataFile file0 = fileList[0];
        //        foreach (var file in fileList)
        //        {
        //            Assert.IsTrue(ReferenceEquals(file0, file));
        //        }
        //    }
        //}

        //[TestMethod]
        //[Ignore]
        //public void TestAddAndRead()
        //{
        //    string fileName = "TestAddAndRead.dat";
        //    // 清理环境
        //    string filePath = GetFilePath(fileName);
        //    if (File.Exists(filePath))
        //    {
        //        File.Delete(filePath);
        //    }

        //    CreateFileUseFactory(fileName);

        //    ManualResetEvent startEvent = new ManualResetEvent(false);
        //    List<Task> taskList = new List<Task>();

        //    int dataCount = 64;
        //    List<ManualResetEvent> finishedEvent = new List<ManualResetEvent>();
        //    Dictionary<string, DataItem> expectedList = new Dictionary<string, DataItem>();
        //    List<Dictionary<string, DataItem>> actualResult = new List<Dictionary<string, DataItem>>();
        //    for (int i = 0; i < dataCount; i++)
        //    {
        //        ManualResetEvent selfEvent = new ManualResetEvent(false);
        //        finishedEvent.Add(selfEvent);

        //        var data = CreateDataItem(1)[0];
        //        while (expectedList.ContainsKey(GetDataItemKey(data)))
        //        {
        //            data = CreateDataItem(1)[0];
        //        }

        //        taskList.Add(Task.Factory.StartNew(() =>
        //        {
        //            // 所有人等待发令同时开始
        //            startEvent.WaitOne();

        //            // 作自己的工作
        //            var file = fileFactory.Open(filePath);

        //            expectedList.Add(GetDataItemKey(data), data);
        //            file.Add(data);

        //            // 完成了自己的任务
        //            selfEvent.Set();

        //            // 等待其他人完成任务
        //            WaitHandle.WaitAll(finishedEvent.ToArray());

        //            Dictionary<string, DataItem> readData = new Dictionary<string, DataItem>();
        //            foreach (var dataItem in file.ReadAll())
        //            {
        //                readData.Add(GetDataItemKey(dataItem), dataItem);
        //            }
        //            actualResult.Add(readData);
        //        }));
        //    }

        //    startEvent.Set();
        //    try
        //    {
        //        Task.WaitAll(taskList.ToArray());
        //    }
        //    finally
        //    {
        //        foreach(var actualDic in actualResult)
        //        {
        //            foreach(var keyValuePair in actualDic)
        //            {
        //                CompareDataItem(expectedList[keyValuePair.Key], keyValuePair.Value);
        //            }
        //        }
        //    }
        //}

        private string GetDataItemKey(DataItem dataItem)
        {
            string str = dataItem.StockCode.Value
                + dataItem.StockName.Value
                + dataItem.IntData
                + dataItem.LongData
                + dataItem.FloatData;
            return str.GetHashCode().ToString(); ;
        }

        [TestMethod]
        public void TestResourceRelease()
        {
            string fileName = "TestResourceRelease1.dat";
            // 清理环境
            string filePath = GetFilePath(fileName);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            CreateFileUseFactory(fileName);

            Assert.IsTrue(fileFactory.OpenedFiles >= 1);

            Thread.Sleep(4000);
            Assert.IsTrue(fileFactory.OpenedFiles == 0);
        }

        private string CreateFileUseFactory(string fileName)
        {
            string path = GetFilePath(fileName);
            FileHeader header = CreateHeader(1000);

            fileFactory.Create(path, header);
            return path;
        }

        private string GetFilePath(string fileName)
        {
            return Environment.CurrentDirectory + @"\" + fileName; ;
        }
    }

    internal class ConcurrentDataFile
        : ConcurrentFile<FileHeader, DataItem>
    {
        internal ConcurrentDataFile(string path) : base(path) { }

        internal ConcurrentDataFile(string path, FileHeader header) : base(path, header) { }
    }

    internal class FileFactory : ConcurrentFileFactory<ConcurrentDataFile, FileHeader, DataItem>
    {
        protected override ConcurrentDataFile DoCreate(string path, FileHeader fileHeader)
        {
            return new ConcurrentDataFile(path, fileHeader);
        }

        protected override ConcurrentDataFile DoOpen(string path)
        {
            return new ConcurrentDataFile(path);
        }
    }
}
