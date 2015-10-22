using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Infrastructure.MemoryMap
{
    public abstract class ConcurrentFileFactory<TFile, TDataHeader, TDataItem>
            where TFile : ConcurrentFile<TDataHeader, TDataItem>
            where TDataHeader : struct, IMemoryMappedFileHeader
            where TDataItem : struct
    {
        private readonly ConcurrentDictionary<string, TFile> _filePool
            = new ConcurrentDictionary<string, TFile>();

        //public TFile Open(string path)
        //{
        //    if(_filePool.ContainsKey(path))
        //    {
        //        return _filePool[path];
        //    }
        //    else
        //    {
        //        // 可能多个线程同时进入这里
        //        _filePool.TryAdd(path, DoOpen(path));
        //    }
        //}

        public abstract TFile DoOpen(string path);

        public abstract TFile Create(string path, TDataHeader fileHeader);
    }
}
