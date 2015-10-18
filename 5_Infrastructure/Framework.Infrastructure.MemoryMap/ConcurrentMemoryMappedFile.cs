using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Infrastructure.MemoryMap
{
    public class ConcurrentMemoryMappedFile<TDataHeader, TDataItem>
        : NonConcurrentMemoryMappedFile<TDataHeader, TDataItem>
        where TDataHeader : struct, IMemoryMappedFileHeader
        where TDataItem : struct
    {
        #region Constructor

        /// <summary>
        /// 打开文件调用的构造函数
        /// </summary>
        /// <param name="path"></param>
        public ConcurrentMemoryMappedFile(string path) : base(path)
        { }

        /// <summary>
        /// 创建文件调用的构造函数
        /// </summary>
        /// <param name="path"></param>
        /// <param name="maxDataCount"></param>
        public ConcurrentMemoryMappedFile(string path, TDataHeader fileHeader)
            : base(path, fileHeader)
        { }
        #endregion
    }
}
