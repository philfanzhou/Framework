using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
        private ReaderWriterLockSlim _rwLock;
        private NonConcurrentFile<TFileHeader, TDataItem> _mmf = null;
        private string _fullPath;

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

        private ReaderWriterLockSlim RWLock
        {
            get
            {
                if(null == _rwLock)
                {
                    _rwLock = LockPool.Instance.GetLock(_fullPath);
                }
                return _rwLock;
            }
        }

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
                RWLock.EnterReadLock();
                try
                {
                    return _mmf.Header;
                }
                finally
                {
                    RWLock.ExitReadLock();
                }
            }
        }

        public void Add(TDataItem item)
        {
            RWLock.EnterWriteLock();
            try
            {
                _mmf.Add(item);
            }
            finally
            {
                RWLock.ExitWriteLock();
            }
        }

        public void Add(IEnumerable<TDataItem> items)
        {
            RWLock.EnterWriteLock();
            try
            {
                _mmf.Add(items);
            }
            finally
            {
                RWLock.ExitWriteLock();
            }
        }

        public void Delete(int index)
        {
            RWLock.EnterWriteLock();
            try
            {
                _mmf.Delete(index);
            }
            finally
            {
                RWLock.ExitWriteLock();
            }
        }

        public void Delete(int index, int count)
        {
            RWLock.EnterWriteLock();
            try
            {
                _mmf.Delete(index, count);
            }
            finally
            {
                RWLock.ExitWriteLock();
            }
        }

        public void DeleteAll()
        {
            RWLock.EnterWriteLock();
            try
            {
                _mmf.DeleteAll();
            }
            finally
            {
                RWLock.ExitWriteLock();
            }
        }

        public void Update(IEnumerable<TDataItem> items, int index)
        {
            RWLock.EnterWriteLock();
            try
            {
                _mmf.Update(items, index);
            }
            finally
            {
                RWLock.ExitWriteLock();
            }
        }

        public void Update(TDataItem item, int index)
        {
            RWLock.EnterWriteLock();
            try
            {
                _mmf.Update(item, index);
            }
            finally
            {
                RWLock.ExitWriteLock();
            }
        }

        public void Insert(IEnumerable<TDataItem> items, int index)
        {
            RWLock.EnterWriteLock();
            try
            {
                _mmf.Insert(items, index);
            }
            finally
            {
                RWLock.ExitWriteLock();
            }
        }

        public void Insert(TDataItem item, int index)
        {
            RWLock.EnterWriteLock();
            try
            {
                _mmf.Insert(item, index);
            }
            finally
            {
                RWLock.ExitWriteLock();
            }
        }

        public TDataItem Read(int index)
        {
            RWLock.EnterReadLock();
            try
            {
                return _mmf.Read(index);
            }
            finally
            {
                RWLock.ExitReadLock();
            }
        }

        public IEnumerable<TDataItem> Read(int index, int count)
        {
            RWLock.EnterReadLock();
            try
            {
                return _mmf.Read(index, count);
            }
            finally
            {
                RWLock.ExitReadLock();
            }
        }

        public IEnumerable<TDataItem> ReadAll()
        {
            RWLock.EnterReadLock();
            try
            {
                return _mmf.ReadAll();
            }
            finally
            {
                RWLock.ExitReadLock();
            }
        }
        #endregion
    }
}
