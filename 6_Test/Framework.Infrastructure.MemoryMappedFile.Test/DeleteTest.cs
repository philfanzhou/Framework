using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Framework.Infrastructure.MemoryMappedFile.Test
{
    public partial class MemoryMappedFileTest
    {
        #region Delete One
        private void TestDeleteOne(int containsCount, int deleteIndex)
        {
            // Create file
            string path = CreateFileAnyway("TestDelete1.dat", 100);
            
            var expectedList = AddDataToFile(containsCount, path);
            
            expectedList.RemoveAt(deleteIndex);

            // Open and delete
            using (var file = DataFile.Open(path))
            {
                file.Delete(deleteIndex);
            }

            var actualList = ReadAllDataFromFile(path);
            CompareListItem(expectedList, actualList);
        }

        [TestMethod]
        public void TestDelete0()
        {
            TestDeleteOne(3, 1);
        }

        [TestMethod]
        public void TestDelete1()
        {
            TestDeleteOne(10, 3);
        }

        [TestMethod]
        public void TestDelete2()
        {
            // Create file
            string path = CreateFileAnyway("TestDelete2.dat", 100);

            int dataCount = 10;
            var expectedList = AddDataToFile(dataCount, path);

            // Open and delete
            using (var file = DataFile.Open(path))
            {
                file.Delete(dataCount + 5);
            }

            var actualList = ReadAllDataFromFile(path);
            CompareListItem(expectedList, actualList);

            //TestDeleteOne(10, 15);
        }
        #endregion

        [TestMethod]
        public void TestDeleteArray()
        {
            int maxDataCount = 5;
            string path = CreateFileAnyway("TestDeleteArray.dat", maxDataCount);

            int dataCount = maxDataCount;
            var expectedList = AddDataToFile(dataCount, path);

            int dataIndex = 3;
            int removeDataCount = maxDataCount - dataIndex;
            expectedList.RemoveRange(dataIndex, removeDataCount);

            // Open and delete
            using (var file = DataFile.Open(path))
            {
                file.Delete(dataIndex, removeDataCount);
            }

            var actualList = ReadAllDataFromFile(path);
            CompareListItem(expectedList, actualList);
        }

        [TestMethod]
        public void TestDeleteAll()
        {
            int maxDataCount = 20;
            string path = CreateFileAnyway("TestDeleteAll.dat", maxDataCount);

            int dataCount = maxDataCount;
            var expectedList = AddDataToFile(dataCount, path);

            using (var file = DataFile.Open(path))
            {
                file.DeleteAll();
            }

            var actualList = ReadAllDataFromFile(path);

            Assert.AreEqual(0, actualList.Count);
        }

        #region ArgumentTest
        private void TestDeleteArgument(int containsCount, int index, int Count)
        {
            string path = CreateFileAnyway("TestDeleteArgument.dat", containsCount);

            using (var file = DataFile.Open(path))
            {
                file.Delete(index, Count);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestDeleteArgument1()
        {
            TestDeleteArgument(20, -1, 10);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestDeleteArgument2()
        {
            TestDeleteArgument(20, 0, 21);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestDeleteArgument3()
        {
            TestDeleteArgument(20, 5, 16);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestDeleteArgument4()
        {
            TestDeleteArgument(20, 21, 1);
        }
        #endregion
    }
}
