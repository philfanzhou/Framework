using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Framework.Infrastructure.MemoryMappedFile.Test
{
    public partial class MemoryMappedFileTest
    {
        [TestMethod]
        public void TestAddOneAndReadOne()
        {
            // Create file
            string path = CreateFileAnyway("TestAddOneAndReadOne.dat", 1000);

            int dataCount = 100;
            List<DataItem> expectedList = CreateDataItem(dataCount);

            // Open and add date
            using (var file = DataFile.Open(path))
            {
                foreach (var item in expectedList)
                {
                    file.Add(item);
                }
            }

            List<DataItem> actualList = new List<DataItem>();
            using (var file = DataFile.Open(path))
            {
                for (int i = 0; i < dataCount; i++)
                {
                    actualList.Add(file.Read(i));
                }
            }

            CompareListItem(expectedList, actualList);
        }

        [TestMethod]
        public void TestAddArray()
        {
            string path = CreateFileAnyway("TestAddArray.dat", 100);

            int dataCount = 20;
            var expectedList = AddDataToFile(dataCount, path);
            expectedList.AddRange(AddDataToFile(dataCount, path));

            var actualList = ReadAllDataFromFile(path);
            CompareListItem(expectedList, actualList);
        }
    }
}
