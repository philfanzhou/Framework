using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Framework.Infrastructure.MemoryMap
{
    public class ConcurrentFile<TFileHeader, TDataItem>
        : NonConcurrentFile<TFileHeader, TDataItem>
        where TFileHeader : struct, IMemoryMappedFileHeader
        where TDataItem : struct
    {
        private ReaderWriterLockSlim _rwLock;

        #region Constructor
        /// <summary>
        /// 打开文件调用的构造函数
        /// </summary>
        /// <param name="path"></param>
        protected ConcurrentFile(string path) : base(path)
        {
        }

        /// <summary>
        /// 创建文件调用的构造函数
        /// </summary>
        /// <param name="path"></param>
        /// <param name="maxDataCount"></param>
        protected ConcurrentFile(string path, TFileHeader fileHeader)
            : base(path, fileHeader)
        {
        }
        #endregion

        private ReaderWriterLockSlim RWLock
        {
            get
            {
                if(null == _rwLock)
                {
                    _rwLock = LockPool.Instance.GetLock(base.MapName);
                }
                return _rwLock;
            }
        }

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

        public override TFileHeader Header
        {
            get
            {
                RWLock.EnterReadLock();
                try
                {
                    return base.Header;
                }
                finally
                {
                    RWLock.ExitReadLock();
                }
            }
        }

        public override void Add(TDataItem item)
        {
            RWLock.EnterWriteLock();
            try
            {
                base.Add(item);
            }
            finally
            {
                RWLock.ExitWriteLock();
            }
        }

        public override void Add(IEnumerable<TDataItem> items)
        {
            RWLock.EnterWriteLock();
            try
            {
                base.Add(items);
            }
            finally
            {
                RWLock.ExitWriteLock();
            }
        }

        public override void Delete(int index)
        {
            RWLock.EnterWriteLock();
            try
            {
                base.Delete(index);
            }
            finally
            {
                RWLock.ExitWriteLock();
            }
        }

        public override void Delete(int index, int count)
        {
            RWLock.EnterWriteLock();
            try
            {
                base.Delete(index, count);
            }
            finally
            {
                RWLock.ExitWriteLock();
            }
        }

        public override void DeleteAll()
        {
            RWLock.EnterWriteLock();
            try
            {
                base.DeleteAll();
            }
            finally
            {
                RWLock.ExitWriteLock();
            }
        }

        public override void Update(IEnumerable<TDataItem> items, int index)
        {
            RWLock.EnterWriteLock();
            try
            {
                base.Update(items, index);
            }
            finally
            {
                RWLock.ExitWriteLock();
            }
        }

        public override void Update(TDataItem item, int index)
        {
            RWLock.EnterWriteLock();
            try
            {
                base.Update(item, index);
            }
            finally
            {
                RWLock.ExitWriteLock();
            }
        }

        public override void Insert(IEnumerable<TDataItem> items, int index)
        {
            RWLock.EnterWriteLock();
            try
            {
                base.Insert(items, index);
            }
            finally
            {
                RWLock.ExitWriteLock();
            }
        }

        public override void Insert(TDataItem item, int index)
        {
            RWLock.EnterWriteLock();
            try
            {
                base.Insert(item, index);
            }
            finally
            {
                RWLock.ExitWriteLock();
            }
        }

        public override TDataItem Read(int index)
        {
            RWLock.EnterReadLock();
            try
            {
                return base.Read(index);
            }
            finally
            {
                RWLock.ExitReadLock();
            }
        }

        public override IEnumerable<TDataItem> Read(int index, int count)
        {
            RWLock.EnterReadLock();
            try
            {
                return base.Read(index, count);
            }
            finally
            {
                RWLock.ExitReadLock();
            }
        }

        public override IEnumerable<TDataItem> ReadAll()
        {
            RWLock.EnterReadLock();
            try
            {
                return base.ReadAll();
            }
            finally
            {
                RWLock.ExitReadLock();
            }
        }
    }
}
