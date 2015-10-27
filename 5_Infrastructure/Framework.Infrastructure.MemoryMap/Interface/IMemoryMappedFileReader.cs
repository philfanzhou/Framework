using System.Collections.Generic;

namespace Framework.Infrastructure.MemoryMap
{
    public interface IMemoryMappedFileReader<TDataHeader, TDataItem>
        where TDataHeader : struct, IMemoryMappedFileHeader
        where TDataItem : struct
    {
        TDataItem Read(int index);

        IEnumerable<TDataItem> Read(int index, int count);

        IEnumerable<TDataItem> ReadAll();
    }
}
