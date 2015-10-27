using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Infrastructure.MemoryMap
{
    public abstract class ConcurrentFileFactory<TFile, TFileHeader, TDataItem>
        where TFile : ConcurrentFile<TFileHeader, TDataItem>
        where TFileHeader : struct, IMemoryMappedFileHeader
        where TDataItem : struct
    {
        private object _lockObj = new object();

        public TFile Create(string path, TFileHeader fileHeader)
        {
            lock(_lockObj)
            {
                return DoCreate(path, fileHeader);
            }
        }

        public TFile Open(string path)
        {
            lock (_lockObj)
            {
                return DoOpen(path); 
            }
        }

        protected abstract TFile DoCreate(string path, TFileHeader fileHeader);

        protected abstract TFile DoOpen(string path);
    }
}
