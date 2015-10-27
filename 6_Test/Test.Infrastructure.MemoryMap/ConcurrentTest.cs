using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Framework.Infrastructure.MemoryMap;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;

namespace Test.Infrastructure.MemoryMap
{
    public partial class MemoryMappedFileTest
    {
        //private FileFactory fileFactory = new FileFactory();

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

        private string CreateConcurrentFileAnyway(string fileName, int maxDataCount)
        {
            string path = Environment.CurrentDirectory + @"\" + fileName;

            if (File.Exists(path))
            {
                File.Delete(path);
            }
            // 创建新的文件
            using (ConcurrentDataFile.Create(path, CreateHeader(maxDataCount))) { }
            return path;
        }

        [TestMethod]
        public void TestOpenAgain()
        {
            string path = CreateConcurrentFileAnyway("TestOpenAgain.dat", 100);
            ConcurrentDataFile file1= null;
            ConcurrentDataFile file2 = null;
            ConcurrentDataFile file3 = null;
            try
            {
                file1 = ConcurrentDataFile.Open(path);
                file2 = ConcurrentDataFile.Open(path);
                file3 = ConcurrentDataFile.Open(path);
                Assert.IsNotNull(file1);
                Assert.IsNotNull(file2);
                Assert.IsNotNull(file3);

                TestThreeFile(file1, file2, file3);
            }
            finally
            {
                file1.Dispose();
                file2.Dispose();
                file3.Dispose();
            }
        }

        [TestMethod]
        public void TestCreateAndOpenAgain()
        {
            string fileName = "TestCreateAndOpenAgain.dat";
            string path = Environment.CurrentDirectory + @"\" + fileName;

            if (File.Exists(path))
            {
                File.Delete(path);
            }
            // 创建新的文件
            ConcurrentDataFile createdFile = null;
            ConcurrentDataFile file1 = null;
            ConcurrentDataFile file2 = null;
            try
            {
                createdFile = ConcurrentDataFile.Create(path, CreateHeader(100));
                file1 = ConcurrentDataFile.Open(path);
                file2 = ConcurrentDataFile.Open(path);
                Assert.IsNotNull(createdFile);
                Assert.IsNotNull(file1);
                Assert.IsNotNull(file2);

                TestThreeFile(createdFile, file1, file2);
            }
            finally
            {
                createdFile.Dispose();
                file1.Dispose();
                file2.Dispose();
            }
        }

        private void TestThreeFile(ConcurrentDataFile file1, ConcurrentDataFile file2, ConcurrentDataFile file3)
        {
            List<DataItem> expectedList = new List<DataItem>();
            DataItem data;

            data = CreateDataItem(1)[0];
            expectedList.Add(data);
            file1.Add(data);//
            CompareListItem(expectedList, file2.ReadAll().ToList());
            CompareListItem(expectedList, file3.ReadAll().ToList());

            data = CreateDataItem(1)[0];
            expectedList.Add(data);
            file2.Add(data);//
            CompareListItem(expectedList, file1.ReadAll().ToList());
            CompareListItem(expectedList, file3.ReadAll().ToList());

            data = CreateDataItem(1)[0];
            expectedList.Add(data);
            file3.Add(data);//
            CompareListItem(expectedList, file1.ReadAll().ToList());
            CompareListItem(expectedList, file2.ReadAll().ToList());

            // 关闭直接映射到磁盘的文件后，测试其余两个文件
            file1.Dispose();
            data = CreateDataItem(1)[0];
            expectedList.Add(data);
            file3.Add(data);//
            CompareListItem(expectedList, file2.ReadAll().ToList());

            // 再次打开一个文件
            file1 = ConcurrentDataFile.Open(file2.FullPath);
            data = CreateDataItem(1)[0];
            expectedList.Add(data);
            file1.Add(data);//
            CompareListItem(expectedList, file1.ReadAll().ToList());
            CompareListItem(expectedList, file2.ReadAll().ToList());
            CompareListItem(expectedList, file3.ReadAll().ToList());
        }
    }

    internal class ConcurrentDataFile
        : ConcurrentFile<FileHeader, DataItem>
    {
        internal ConcurrentDataFile(string path) : base(path) { }

        internal ConcurrentDataFile(string path, FileHeader header) : base(path, header) { }

        public static ConcurrentDataFile Open(string path)
        {
            return new ConcurrentDataFile(path);
        }

        public static ConcurrentDataFile Create(string path, FileHeader fileHeader)
        {
            return new ConcurrentDataFile(path, fileHeader);
        }
    }
}
