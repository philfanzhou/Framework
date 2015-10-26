using System.Collections.Generic;

namespace Framework.Infrastructure.MemoryMap
{
    public interface IMemoryMappedFileModifier<TDataHeader, TDataItem>
        : IMemoryMappedFile<TDataHeader>
        where TDataHeader : struct, IMemoryMappedFileHeader
        where TDataItem : struct
    {
        void Add(TDataItem item);

        void Add(IEnumerable<TDataItem> items);

        void Delete(int index);

        void Delete(int index, int count);

        void DeleteAll();

        void Update(TDataItem item, int index);

        void Update(IEnumerable<TDataItem> items, int index);

        void Insert(TDataItem item, int index);

        void Insert(IEnumerable<TDataItem> items, int index);
    }
}
