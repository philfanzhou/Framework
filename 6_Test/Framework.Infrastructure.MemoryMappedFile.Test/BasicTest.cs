using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace Framework.Infrastructure.MemoryMappedFile.Test
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

            int maxDataCount = 0;
            string comment = "招商银行";
            FileHeader header = new FileHeader();
            header.MaxDataCount = maxDataCount;
            header.StockCode.Value = comment;
            using (DataFile.Create(path, header))
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

        #region Header
        [TestMethod]
        public void TestFileHeader1()
        {
            string path = Environment.CurrentDirectory + @"\" + "TestFileHeader1.dat";

            if (File.Exists(path))
            {
                File.Delete(path);
            }

            FileHeader header = CreateHeader(10);
            using (DataFile.Create(path, header))
            { }

            FileHeader readedHeader;
            using (var file = DataFile.Open(path))
            {
                readedHeader = (FileHeader)file.Header;
            }

            Assert.AreEqual(header, readedHeader);
        }
        #endregion
    }
}
