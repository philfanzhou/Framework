using System;
using System.IO;

namespace Framework.Infrastructure.MemoryMappedFile
{
    public class MyMemoryMappedFile : IDisposable
    {
        protected System.IO.MemoryMappedFiles.MemoryMappedFile Mmf;
        protected readonly string Path;
        protected readonly string MapName;
        protected readonly string FileName;

        #region Constructor
        
        protected MyMemoryMappedFile(string path) : this(path, -1) { }

        protected MyMemoryMappedFile(string path, long capacity)
        {
            this.FileName = System.IO.Path.GetFileName(path);
            this.Path = path;
            this.MapName = this.FileName;

            bool createNewFile = capacity > 0;
            if (createNewFile)
            {
                string directory = System.IO.Path.GetDirectoryName(path);
                if(!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                // FileMode一定要使用CreateNew，否则可能出现覆盖文件的情况
                this.Mmf = System.IO.MemoryMappedFiles.MemoryMappedFile.CreateFromFile(path, FileMode.CreateNew, this.FileName, capacity);
            }
            else
            {
                this.Mmf = System.IO.MemoryMappedFiles.MemoryMappedFile.CreateFromFile(path, FileMode.Open, this.FileName);
            }
        }

        #endregion

        #region IDisposable Member

        protected bool Disposed;

        ~MyMemoryMappedFile()
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
                throw new ObjectDisposedException("MarketDataMemoryMappedFile", "MarketDataMemoryMappedFile has been disposed.");
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
                if (Mmf != null)
                {
                    Mmf.Dispose();
                    Mmf = null;
                }
            }

            Disposed = true;
        }

        #endregion

        #region Override

        public override string ToString()
        {
            ThrowIfDisposed();

            return this.Path;
        }

        #endregion
    }
}
