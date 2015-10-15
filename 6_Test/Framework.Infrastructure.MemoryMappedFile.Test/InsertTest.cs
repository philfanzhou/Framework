using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Framework.Infrastructure.MemoryMappedFile.Test
{
    public partial class MemoryMappedFileTest
    {
        [TestMethod]
        public void TestInsert()
        {
            // Create file
            string path = CreateFileAnyway("TestInsert.dat", 50);

            int dataCount = 10;
            var expectedList = AddDataToFile(dataCount, path);

            int dataIndex = 3;
            var addDataItems = CreateDataItem(10);
            expectedList.InsertRange(dataIndex, addDataItems);

            // Open and insert
            using (var file = DataFile.Open(path))
            {
                foreach (var dataItem in addDataItems)
                {
                    file.Insert(dataItem, dataIndex++);
                }
            }

            var actualList = ReadAllDataFromFile(path);
            CompareListItem(expectedList, actualList);
        }

        [TestMethod]
        public void TestInsert1()
        {
            // Create file
            string path = CreateFileAnyway("TestInsert.dat", 50);

            int dataCount = 2;
            var expectedList = AddDataToFile(dataCount, path);

            int dataIndex = 1;
            var addDataItems = CreateDataItem(1);
            expectedList.InsertRange(dataIndex, addDataItems);

            // Open and insert
            using (var file = DataFile.Open(path))
            {
                foreach (var dataItem in addDataItems)
                {
                    file.Insert(dataItem, dataIndex++);
                }
            }

            var actualList = ReadAllDataFromFile(path);
            CompareListItem(expectedList, actualList);
        }

        [TestMethod]
        public void TestInsertArray1()
        {
            string path = CreateFileAnyway("TestInsertArray1.dat", 20);

            int dataCount = 10;
            var expectedList = AddDataToFile(dataCount, path);

            int dataIndex = 3;
            var addDataItems = CreateDataItem(10);
            expectedList.InsertRange(dataIndex, addDataItems);

            // Open and insert
            using (var file = DataFile.Open(path))
            {
                file.Insert(addDataItems, dataIndex);
            }

            var actualList = ReadAllDataFromFile(path);
            CompareListItem(expectedList, actualList);
        }

        [TestMethod]
        public void TestInsertArray2()
        {
            string path = CreateFileAnyway("TestInsertArray2.dat", 20);

            int dataCount = 10;
            AddDataToFile(dataCount, path);

            List<DataItem> dataList = CreateDataItem(5);
            // Open and insert
            using (var file = DataFile.Open(path))
            {
                file.Insert(dataList, 12);
                Assert.AreEqual(12 + 5 - 1, file.Header.DataCount);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestInsertArgument1()
        {
            string path = CreateFileAnyway("TestInsertArgument1.dat", 20);

            using (var file = DataFile.Open(path))
            {
                file.Insert(null, 0);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestInsertArgument2()
        {
            string path = CreateFileAnyway("TestInsertArgument2.dat", 20);

            List<DataItem> expectedList = CreateDataItem(10);
            using (var file = DataFile.Open(path))
            {
                file.Insert(expectedList, 21);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestInsertArgument3()
        {
            string path = CreateFileAnyway("TestInsertArgument3.dat", 20);

            List<DataItem> expectedList = CreateDataItem(10);
            using (var file = DataFile.Open(path))
            {
                file.Insert(expectedList, 18);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestInsertArgument4()
        {
            string path = CreateFileAnyway("TestInsertArgument4.dat", 20);

            int dataCount = 10;
            var expectedList = AddDataToFile(dataCount, path);

            List<DataItem> dataList = CreateDataItem(12);

            using (var file = DataFile.Open(path))
            {
                file.Insert(dataList, 8);
            }
        }
    }
}
