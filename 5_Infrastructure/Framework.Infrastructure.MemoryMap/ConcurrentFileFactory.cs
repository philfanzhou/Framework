using System;
using System.Collections.Generic;
using System.Timers;

namespace Framework.Infrastructure.MemoryMap
{
    public abstract class ConcurrentFileFactory<TFile, TFileHeader, TDataItem> : IDisposable
            where TFile : ConcurrentFile<TFileHeader, TDataItem>
            where TFileHeader : struct, IMemoryMappedFileHeader
            where TDataItem : struct
    {
        #region Field
        private Timer _cleanerTimer = new Timer(3000);

        private object thisLock = new object();

        private readonly Dictionary<string, TFile> _filePool = new Dictionary<string, TFile>();
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
                    _cleanerTimer.Elapsed -= _cleanerTimer_Elapsed;
                    _cleanerTimer.Dispose();
                    _cleanerTimer = null;
                }
            }

            Disposed = true;
        }

        #endregion

        #region Property
        public int OpenedFiles
        {
            get
            {
                ThrowIfDisposed();
                return _filePool.Count;
            }
        }
        #endregion

        #region Abstract
        protected abstract TFile DoOpen(string path);

        protected abstract TFile DoCreate(string path, TFileHeader fileHeader);
        #endregion

        #region Public Method
        public virtual TFile Open(string path)
        {
            ThrowIfDisposed();
            lock(thisLock)
            {
                if(!_filePool.ContainsKey(path))
                {
                    _filePool.Add(path, DoOpen(path));
                }

                return _filePool[path];
            }
        }

        public virtual TFile Create(string path, TFileHeader fileHeader)
        {
            ThrowIfDisposed();
            lock (thisLock)
            {
                if (!_filePool.ContainsKey(path))
                {
                    _filePool.Add(path, DoCreate(path, fileHeader));
                }

                return _filePool[path];
            }
        }
        #endregion

        #region Private Method
        private void _cleanerTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _cleanerTimer.Enabled = false;
            try
            {
                lock(thisLock)
                {
                    // 找出不再使用的文件的key
                    List<string> keys = new List<string>();
                    foreach (var pair in _filePool)
                    {
                        if (!pair.Value.IsAlive)
                        {
                            keys.Add(pair.Key);
                        }
                    }

                    // 移除文件并释放资源
                    foreach (var key in keys)
                    {
                        var file = _filePool[key];
                        _filePool.Remove(key);
                        file.Dispose();
                    }
                }
            }
            finally
            {
                _cleanerTimer.Enabled = true;
            }
        }
        #endregion
    }
}
