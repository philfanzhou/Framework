using System;
using System.Collections.Generic;

namespace Framework.Infrastructure.MemoryMap
{
    public interface IMemoryMappedFile<TDataHeader> : IDisposable
        where TDataHeader : struct, IMemoryMappedFileHeader
    {
        string FullPath { get; }

        TDataHeader Header { get; }
    }
}
