using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace Test.Infrastructure.MemoryMap
{
    public partial class MemoryMappedFileTest
    {
        #region Test exception

        [TestMethod]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void CannotBeAccessAfterDisposed()
        {
            string path = CreateFileAnyway("TestCannotBeAccessAfterDisposed.dat", 1000);

            var file = DataFile.Open(path);
            file.Dispose();

            file.DeleteAll();
        }

        [TestMethod]
        [ExpectedException(typeof(IOException))]
        public void CannotOverwriteFile()
        {
            string path = CreateFileAnyway("TestCanNotOverWriteFile.dat", 1);

            // Can not create again use same path or overwrite file.
            using (DataFile.Create(path, new FileHeader()))
            { }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void DataCountMustBiggerThan0()
        {
            string path = Environment.CurrentDirectory + @"\" + "TestFileHeader2.dat";

            if (File.Exists(path))
            {
                File.Delete(path);
            }
            
            using (DataFile.Create(path, CreateHeader(0)))
            { }
        }

        #endregion

        [TestMethod]
        public void TestToString()
        {
            string path = CreateFileAnyway("TestToString.dat", 1000);
            using (var file = DataFile.Open(path))
            {
                Assert.AreEqual(path, file.ToString());
            }
        }

        [TestMethod]
        public void TestCanCreateFolderAutomatic()
        {
            string path = Environment.CurrentDirectory + @"\TestFolder\" + "TestFileHeader1.dat";
            string directory = Path.GetDirectoryName(path);
            if(File.Exists(path))
            {
                File.Delete(path);
            }
            if(Directory.Exists(directory))
            {
                Directory.Delete(directory);
            }

            FileHeader expected = CreateHeader(10);
            using (var file = DataFile.Create(path, expected))
            {
                Assert.IsNotNull(file);
            }
        }

        #region Header
        [TestMethod]
        public void TestReadHeader()
        {
            string path = Environment.CurrentDirectory + @"\" + "TestFileHeader1.dat";

            if (File.Exists(path))
            {
                File.Delete(path);
            }

            FileHeader expected = CreateHeader(10);
            using (DataFile.Create(path, expected))
            { }

            FileHeader actual;
            using (var file = DataFile.Open(path))
            {
                actual = file.Header;
            }

            Assert.AreEqual(expected, actual);
            Assert.AreEqual(expected.StockCode, actual.StockCode);
            Assert.AreEqual(expected.StockName, actual.StockName);
        }

        [TestMethod]
        public void TestCannotModifyHeader()
        {
            string path = CreateFileAnyway("TestModifyHeader.dat", 10);

            FileHeader expected;
            using (var file = DataFile.Open(path))
            {
                expected = ((FileHeader)file.Header);
                expected.MaxDataCount = 12;
            }

            FileHeader actual;
            using (var file = DataFile.Open(path))
            {
                actual = file.Header;
            }

            Assert.AreNotEqual(expected, actual);
        }
        #endregion
    }
}
