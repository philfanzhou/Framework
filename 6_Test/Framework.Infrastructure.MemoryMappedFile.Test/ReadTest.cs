using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Framework.Infrastructure.MemoryMappedFile.Test
{
    public partial class MemoryMappedFileTest
    {
        private void TestReadData(int containsCount, int index, int count)
        {
            string path = CreateFileAnyway("TestReadArgumeng1.dat", containsCount);

            AddDataToFile(containsCount, path);
            using (var file = DataFile.Open(path))
            {
                file.Read(index, count);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestReadArgumeng1()
        {
            TestReadData(5, 6, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestReadArgumeng2()
        {
            TestReadData(5, -1, 2);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestReadArgumeng3()
        {
            TestReadData(5, 1, -1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestReadArgumeng4()
        {
            TestReadData(5, 1, 5);
        }
    }
}
