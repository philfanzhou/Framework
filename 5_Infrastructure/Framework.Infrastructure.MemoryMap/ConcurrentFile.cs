using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Framework.Infrastructure.MemoryMap
{
    public class ConcurrentFile<TDataHeader, TDataItem>
        : NonConcurrentFile<TDataHeader, TDataItem>
        where TDataHeader : struct, IMemoryMappedFileHeader
        where TDataItem : struct
    {
        private ReaderWriterLockSlim _rwLock;

        #region Constructor

        /// <summary>
        /// 打开文件调用的构造函数
        /// </summary>
        /// <param name="path"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        protected ConcurrentFile(string path) : base(path)
        {
            _rwLock = LockPool.Instance.GetLock(base.MapName);
        }

        /// <summary>
        /// 创建文件调用的构造函数
        /// </summary>
        /// <param name="path"></param>
        /// <param name="maxDataCount"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        protected ConcurrentFile(string path, TDataHeader fileHeader)
            : base(path, fileHeader)
        {
            _rwLock = LockPool.Instance.GetLock(base.MapName);
        }
        #endregion

        protected override void Dispose(bool disposing)
        {
            if(disposing)
            {
                if(_rwLock != null)
                {
                    _rwLock = null;
                }
                LockPool.Instance.ReleaseLock(base.MapName);
            }

            base.Dispose(disposing);
        }

        public override TDataHeader Header
        {
            get
            {
                _rwLock.EnterReadLock();
                try
                {
                    return base.Header;
                }
                finally
                {
                    _rwLock.ExitReadLock();
                }
            }
        }

        public override void Add(TDataItem item)
        {
            _rwLock.EnterWriteLock();
            try
            {
                base.Add(item);
            }
            finally
            {
                _rwLock.ExitWriteLock();
            }
        }

        public override void Add(IEnumerable<TDataItem> items)
        {
            _rwLock.EnterWriteLock();
            try
            {
                base.Add(items);
            }
            finally
            {
                _rwLock.ExitWriteLock();
            }
        }

        public override void Delete(int index)
        {
            _rwLock.EnterWriteLock();
            try
            {
                base.Delete(index);
            }
            finally
            {
                _rwLock.ExitWriteLock();
            }
        }

        public override void Delete(int index, int count)
        {
            _rwLock.EnterWriteLock();
            try
            {
                base.Delete(index, count);
            }
            finally
            {
                _rwLock.ExitWriteLock();
            }
        }

        public override void DeleteAll()
        {
            _rwLock.EnterWriteLock();
            try
            {
                base.DeleteAll();
            }
            finally
            {
                _rwLock.ExitWriteLock();
            }
        }

        public override void Update(IEnumerable<TDataItem> items, int index)
        {
            _rwLock.EnterWriteLock();
            try
            {
                base.Update(items, index);
            }
            finally
            {
                _rwLock.ExitWriteLock();
            }
        }

        public override void Update(TDataItem item, int index)
        {
            _rwLock.EnterWriteLock();
            try
            {
                base.Update(item, index);
            }
            finally
            {
                _rwLock.ExitWriteLock();
            }
        }

        public override void Insert(IEnumerable<TDataItem> items, int index)
        {
            _rwLock.EnterWriteLock();
            try
            {
                base.Insert(items, index);
            }
            finally
            {
                _rwLock.ExitWriteLock();
            }
        }

        public override void Insert(TDataItem item, int index)
        {
            _rwLock.EnterWriteLock();
            try
            {
                base.Insert(item, index);
            }
            finally
            {
                _rwLock.ExitWriteLock();
            }
        }

        public override TDataItem Read(int index)
        {
            _rwLock.EnterReadLock();
            try
            {
                return base.Read(index);
            }
            finally
            {
                _rwLock.ExitReadLock();
            }
        }

        public override IEnumerable<TDataItem> Read(int index, int count)
        {
            _rwLock.EnterReadLock();
            try
            {
                return base.Read(index, count);
            }
            finally
            {
                _rwLock.ExitReadLock();
            }
        }

        public override IEnumerable<TDataItem> ReadAll()
        {
            _rwLock.EnterReadLock();
            try
            {
                return base.ReadAll();
            }
            finally
            {
                _rwLock.ExitReadLock();
            }
        }
    }
}
