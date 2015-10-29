using System;
using System.Collections.Generic;
using System.Threading;

namespace Framework.Infrastructure.MemoryMap
{
    public class ConcurrentFile<TFileHeader, TDataItem> : 
        IMemoryMappedFile<TFileHeader>,
        IMemoryMappedFileModifier<TFileHeader, TDataItem>,
        IMemoryMappedFileReader<TFileHeader, TDataItem>
        where TFileHeader : struct, IMemoryMappedFileHeader
        where TDataItem : struct
    {
        private string _fullPath;
        private NonConcurrentFile<TFileHeader, TDataItem> _mmf;
        private ReaderWriterLockSlim _rwLock;

        private static object _lockObj = new object();

        #region Constructor
        /// <summary>
        /// 打开文件调用的构造函数
        /// </summary>
        /// <param name="path"></param>
        protected ConcurrentFile(string path)
        {
            lock (_lockObj)
            {
                _fullPath = path;
                _mmf = new NonConcurrentFile<TFileHeader, TDataItem>(path);
                _rwLock = LockPool.Instance.GetLock(_fullPath);
            }
        }

        /// <summary>
        /// 创建文件调用的构造函数
        /// </summary>
        /// <param name="path"></param>
        /// <param name="maxDataCount"></param>
        protected ConcurrentFile(string path, TFileHeader fileHeader)
        {
            lock (_lockObj)
            {
                _fullPath = path;
                _mmf = new NonConcurrentFile<TFileHeader, TDataItem>(path, fileHeader);
                _rwLock = LockPool.Instance.GetLock(_fullPath);
            }
        }
        #endregion

        #region IDisposable Member
        protected bool Disposed;

        ~ConcurrentFile()
        {
            this.Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Throws a <see cref="ObjectDisposedException"/> if this object has been disposed.
        /// </summary>
        /// <exception cref="ObjectDisposedException"></exception>
        protected void ThrowIfDisposed()
        {
            if (Disposed)
            {
                throw new ObjectDisposedException("ConcurrentFile", "ConcurrentFile has been disposed.");
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (Disposed)
            {
                return;
            }

            if (disposing)
            {
                // Clean up managed resources
                if (_rwLock != null)
                {
                    _rwLock = null;
                }
                LockPool.Instance.ReleaseLock(_fullPath);
            }

            Disposed = true;
        }
        #endregion

        #region IMemoryMappedFile Members
        public string FullPath
        {
            get
            {
                return _mmf.FullPath;
            }
        }

        public TFileHeader Header
        {
            get
            {
                _rwLock.EnterReadLock();
                try
                {
                    return _mmf.Header;
                }
                finally
                {
                    _rwLock.ExitReadLock();
                }
            }
        }

        public void Add(TDataItem item)
        {
            _rwLock.EnterWriteLock();
            try
            {
                _mmf.Add(item);
            }
            finally
            {
                _rwLock.ExitWriteLock();
            }
        }

        public void Add(IEnumerable<TDataItem> items)
        {
            _rwLock.EnterWriteLock();
            try
            {
                _mmf.Add(items);
            }
            finally
            {
                _rwLock.ExitWriteLock();
            }
        }

        public void Delete(int index)
        {
            _rwLock.EnterWriteLock();
            try
            {
                _mmf.Delete(index);
            }
            finally
            {
                _rwLock.ExitWriteLock();
            }
        }

        public void Delete(int index, int count)
        {
            _rwLock.EnterWriteLock();
            try
            {
                _mmf.Delete(index, count);
            }
            finally
            {
                _rwLock.ExitWriteLock();
            }
        }

        public void DeleteAll()
        {
            _rwLock.EnterWriteLock();
            try
            {
                _mmf.DeleteAll();
            }
            finally
            {
                _rwLock.ExitWriteLock();
            }
        }

        public void Update(IEnumerable<TDataItem> items, int index)
        {
            _rwLock.EnterWriteLock();
            try
            {
                _mmf.Update(items, index);
            }
            finally
            {
                _rwLock.ExitWriteLock();
            }
        }

        public void Update(TDataItem item, int index)
        {
            _rwLock.EnterWriteLock();
            try
            {
                _mmf.Update(item, index);
            }
            finally
            {
                _rwLock.ExitWriteLock();
            }
        }

        public void Insert(IEnumerable<TDataItem> items, int index)
        {
            _rwLock.EnterWriteLock();
            try
            {
                _mmf.Insert(items, index);
            }
            finally
            {
                _rwLock.ExitWriteLock();
            }
        }

        public void Insert(TDataItem item, int index)
        {
            _rwLock.EnterWriteLock();
            try
            {
                _mmf.Insert(item, index);
            }
            finally
            {
                _rwLock.ExitWriteLock();
            }
        }

        public TDataItem Read(int index)
        {
            _rwLock.EnterReadLock();
            try
            {
                return _mmf.Read(index);
            }
            finally
            {
                _rwLock.ExitReadLock();
            }
        }

        public IEnumerable<TDataItem> Read(int index, int count)
        {
            _rwLock.EnterReadLock();
            try
            {
                return _mmf.Read(index, count);
            }
            finally
            {
                _rwLock.ExitReadLock();
            }
        }

        public IEnumerable<TDataItem> ReadAll()
        {
            _rwLock.EnterReadLock();
            try
            {
                return _mmf.ReadAll();
            }
            finally
            {
                _rwLock.ExitReadLock();
            }
        }
        #endregion
    }
}
