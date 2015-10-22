using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Framework.Infrastructure.MemoryMap
{
    public class ConcurrentFile<TDataHeader, TDataItem>
        : NonConcurrentMemoryMappedFile<TDataHeader, TDataItem>
        where TDataHeader : struct, IMemoryMappedFileHeader
        where TDataItem : struct
    {
        private readonly ReaderWriterLock _rwLock = new ReaderWriterLock();
        private readonly TimeSpan _timeOut = new TimeSpan(0, 0, 20); 

        #region Constructor

        /// <summary>
        /// 打开文件调用的构造函数
        /// </summary>
        /// <param name="path"></param>
        protected ConcurrentFile(string path) : base(path)
        { }

        /// <summary>
        /// 创建文件调用的构造函数
        /// </summary>
        /// <param name="path"></param>
        /// <param name="maxDataCount"></param>
        protected ConcurrentFile(string path, TDataHeader fileHeader)
            : base(path, fileHeader)
        { }
        #endregion

        //internal bool IsAlive
        //{
        //    get { return _rwLock.IsReaderLockHeld}
        //}

        protected override IEnumerable<TDataItem> DoRead(int index, int count)
        {
            _rwLock.AcquireReaderLock(_timeOut);
            var result = base.DoRead(index, count);
            _rwLock.ReleaseReaderLock();

            return result;
        }

        protected override void DoDelete(int index, int count)
        {
            _rwLock.AcquireWriterLock(_timeOut);
            base.DoDelete(index, count);
            _rwLock.ReleaseWriterLock();
        }

        protected override void DoInsert(IEnumerable<TDataItem> items, int index, bool ChangeDataCount)
        {
            _rwLock.AcquireWriterLock(_timeOut);
            base.DoInsert(items, index, ChangeDataCount);
            _rwLock.ReleaseWriterLock();
        }
    }
}
