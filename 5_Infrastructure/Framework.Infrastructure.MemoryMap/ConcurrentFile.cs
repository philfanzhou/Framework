using System.Collections.Generic;
using System.Threading;

namespace Framework.Infrastructure.MemoryMap
{
    public class ConcurrentFile<TDataHeader, TDataItem>
        : NonConcurrentFile<TDataHeader, TDataItem>
        where TDataHeader : struct, IMemoryMappedFileHeader
        where TDataItem : struct
    {
        private ReaderWriterLockSlim _rwLock = new ReaderWriterLockSlim();

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

        protected override void Dispose(bool disposing)
        {
            if(disposing)
            {
                if(_rwLock != null)
                {
                    _rwLock.Dispose();
                    _rwLock = null;
                }
            }

            base.Dispose(disposing);
        }

        internal bool IsAlive
        {
            get
            {
                ThrowIfDisposed();
                return _rwLock.IsReadLockHeld || _rwLock.IsWriteLockHeld;
            }
        }

        protected override IEnumerable<TDataItem> DoRead(int index, int count)
        {
            _rwLock.EnterReadLock();
            IEnumerable<TDataItem> result;
            try
            {
                result = base.DoRead(index, count);
            }
            finally
            {
                _rwLock.ExitReadLock();
            }

            return result;
        }

        protected override void DoDelete(int index, int count)
        {
            _rwLock.EnterWriteLock();
            try
            {
                base.DoDelete(index, count);
            }
            finally
            {
                _rwLock.ExitWriteLock();
            }
        }

        protected override void DoInsert(IEnumerable<TDataItem> items, int index, bool ChangeDataCount)
        {
            _rwLock.EnterWriteLock();
            try
            {
                base.DoInsert(items, index, ChangeDataCount);
            }
            finally
            {
                _rwLock.ExitWriteLock();
            }
        }
    }
}
