using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Framework.Infrastructure.MemoryMap;

namespace Test.Infrastructure.MemoryMap
{
    public partial class MemoryMappedFileTest
    {
        private FileFactory factory = new FileFactory();

        [TestMethod]
        public void TestMethod1()
        {

        }

        private string CreateFileUseFactory(string fileName)
        {
            string path = Environment.CurrentDirectory + @"\" + fileName;
            FileHeader header = CreateHeader(1000);

            factory.Create(path, header);
            return path;
        }
    }

    internal class ConcurrentDataFile
        : ConcurrentFile<FileHeader, DataItem>
    {
        internal ConcurrentDataFile(string path) : base(path) { }

        internal ConcurrentDataFile(string path, FileHeader header) : base(path, header) { }
    }

    internal class FileFactory : ConcurrentFileFactory<ConcurrentDataFile, FileHeader, DataItem>
    {
        protected override ConcurrentDataFile DoCreate(string path, FileHeader fileHeader)
        {
            return new ConcurrentDataFile(path, fileHeader);
        }

        protected override ConcurrentDataFile DoOpen(string path)
        {
            return new ConcurrentDataFile(path);
        }
    }
}
