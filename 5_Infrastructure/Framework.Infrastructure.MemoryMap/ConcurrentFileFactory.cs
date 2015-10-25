using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Timers;

namespace Framework.Infrastructure.MemoryMap
{
    public abstract class ConcurrentFileFactory<TFile, TFileHeader, TDataItem> : IDisposable
            where TFile : ConcurrentFile<TFileHeader, TDataItem>
            where TFileHeader : struct, IMemoryMappedFileHeader
            where TDataItem : struct
    {
        #region Field
        private System.Timers.Timer _cleanerTimer = new System.Timers.Timer(3000);

        private ReaderWriterLockSlim _rwLock = new ReaderWriterLockSlim();

        private readonly ConcurrentDictionary<string, TFile> _filePool
            = new ConcurrentDictionary<string, TFile>();
        #endregion

        #region Constructor
        public ConcurrentFileFactory()
        {
            _cleanerTimer.Elapsed += _cleanerTimer_Elapsed;
            _cleanerTimer.Enabled = true;
        }
        #endregion

        #region IDisposable Member

        protected bool Disposed;

        ~ConcurrentFileFactory()
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
                throw new ObjectDisposedException("ConcurrentFileFactory", "ConcurrentFileFactory has been disposed.");
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
                if (_cleanerTimer != null)
                {
                    _cleanerTimer.Stop();
                    _cleanerTimer.Dispose();
                    _cleanerTimer = null;
                }

                if(_rwLock != null)
                {
                    _rwLock.Dispose();
                    _rwLock = null;
                }
            }

            Disposed = true;
        }

        #endregion

        #region Abstract
        protected abstract TFile DoOpen(string path);

        protected abstract TFile DoCreate(string path, TFileHeader fileHeader);
        #endregion

        #region Public Method
        public virtual TFile Open(string path)
        {
            TFile file;
            if (!TryGetFileInPool(path, out file))
            {
                file = TryOpen(path);
            }
            return file;
        }

        public virtual TFile Create(string path, TFileHeader fileHeader)
        {
            TFile file;
            if(!TryGetFileInPool(path, out file))
            {
                file = TryCreate(path, fileHeader);
            }
            return file;
        }
        #endregion

        #region Private Method
        private bool TryGetFileInPool(string path, out TFile file)
        {
            _rwLock.EnterReadLock();
            bool result = false;
            try
            {
                result = _filePool.TryGetValue(path, out file);
            }
            finally
            {
                _rwLock.ExitReadLock();
            }
            return result;
        }

        private TFile TryOpen(string path)
        {
            _rwLock.EnterWriteLock();
            TFile file;
            try
            {
                file = DoOpen(path);
                _filePool.TryAdd(path, file);
            }
            finally
            {
                _rwLock.ExitWriteLock();
            }
            return file;
        }

        private TFile TryCreate(string path, TFileHeader fileHeader)
        {
            _rwLock.EnterWriteLock();
            TFile file;
            try
            {
                file = DoCreate(path, fileHeader);
                _filePool.TryAdd(path, file);
            }
            finally
            {
                _rwLock.ExitWriteLock();
            }
            return file;
        }

        private void _cleanerTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _cleanerTimer.Enabled = false;
            _rwLock.EnterWriteLock();
            try
            {
                // 找出不再使用的文件的key
                List<string> keys = new List<string>();
                foreach(var pair in _filePool)
                {
                    if(!pair.Value.IsAlive)
                    {
                        keys.Add(pair.Key);
                    }
                }

                // 移除文件并释放资源
                foreach(var key in keys)
                {
                    TFile file;
                    if(_filePool.TryRemove(key, out file))
                    {
                        file.Dispose();
                    }
                }
            }
            finally
            {
                _rwLock.ExitWriteLock();
                _cleanerTimer.Enabled = true;
            }
        }
        #endregion
    }
}
