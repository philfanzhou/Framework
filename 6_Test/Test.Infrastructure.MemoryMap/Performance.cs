//using System;
//using System.Linq;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using System.Collections.Generic;
//using System.Diagnostics;

//namespace Test.Infrastructure.MemoryMap
//{
//    public partial class MemoryMappedFileTest
//    {
//        private const int dataCount = 50000;
//        private List<DataItem> expectedList = CreateDataItem(dataCount);

//        [TestMethod]
//        public void TestPerformance()
//        {
//            string path = CreateFileAnyway("PerformanceTest.dat", dataCount);

//            Stopwatch stopWath = new Stopwatch();
//            stopWath.Start();
//            using (var file = DataFile.Open(path))
//            {
//                file.Add(expectedList);
//            }
//            stopWath.Stop();
//            TimeSpan addTime = stopWath.Elapsed;
//            Assert.IsTrue(addTime < new TimeSpan(0, 0, 3));

//            stopWath.Reset();
//            stopWath.Start();
//            List<DataItem> actualList;
//            using (var file = DataFile.Open(path))
//            {
//                actualList = file.ReadAll().ToList();
//            }
//            stopWath.Stop();
//            TimeSpan readTime = stopWath.Elapsed;
//            Assert.IsTrue(readTime < new TimeSpan(0, 0, 3));

//            CompareListItem(expectedList, actualList);
//        }
//    }
//}
