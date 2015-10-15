using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Framework.Infrastructure.MemoryMappedFile.Test
{
    public partial class MemoryMappedFileTest
    {
        [TestMethod]
        public void TestUpdate()
        {
            // Create file
            string path = CreateFileAnyway("TestUpdate.dat", 100);

            int dataCount = 10;
            var expectedList = AddDataToFile(dataCount, path);

            int dataIndex = 3;
            var expected = expectedList[dataIndex];
            expected.Amount = 300;
            expectedList[dataIndex] = expected;

            // Open and update
            using (var file = DataFile.Open(path))
            {
                file.Update(expected, dataIndex);
            }

            var actualList = ReadAllDataFromFile(path);
            CompareListItem(expectedList, actualList);
        }

        [TestMethod]
        public void TestUpdateArray()
        {
            int maxDataCount = 100;
            string path = CreateFileAnyway("TestUpdateArray.dat", maxDataCount);

            int dataCount = maxDataCount;
            var expectedList = AddDataToFile(dataCount, path);

            int dataIndex = 5;
            int updateDataCount = 20;
            List<DataItem> updateList = CreateDataItem(updateDataCount);

            int j = 0;
            for (int i = dataIndex; i < dataIndex + updateDataCount; i++)
            {
                expectedList[i] = updateList[j++];
            }

            // Open and update
            using (var file = DataFile.Open(path))
            {
                file.Update(updateList, dataIndex);
            }

            var actualList = ReadAllDataFromFile(path);
            CompareListItem(expectedList, actualList);
        }
    }
}
