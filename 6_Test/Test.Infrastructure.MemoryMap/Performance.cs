using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Test.Infrastructure.MemoryMap
{
    public partial class MemoryMappedFileTest
    {
        [TestMethod]
        public void TestPerformance()
        {
            int dataCount = 100000;
            string path = CreateFileAnyway("PerformanceTest.dat", dataCount);

            List<DataItem> expectedList = CreateDataItem(dataCount);
        }
    }
}
