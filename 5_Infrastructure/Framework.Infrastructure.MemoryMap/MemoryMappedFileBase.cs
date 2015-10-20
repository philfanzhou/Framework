using System;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;

namespace Framework.Infrastructure.MemoryMap
{
    public class MemoryMappedFileBase : IDisposable
    {
        protected MemoryMappedFile Mmf;
        protected readonly string FullPath;
        protected readonly string FileName;

        #region IDisposable Member

        protected bool Disposed;

        ~MemoryMappedFileBase()
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
                throw new ObjectDisposedException("MemoryMappedFile", "MemoryMappedFile has been disposed.");
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

            return this.FullPath;
        }

        #endregion

        #region Constructor

        protected MemoryMappedFileBase(string fullPath) : this(fullPath, -1) { }

        protected MemoryMappedFileBase(string fullPath, long capacity)
        {
            this.FileName = Path.GetFileName(fullPath);
            this.FullPath = fullPath;
            string mapName = FileName;

            bool createNewFile = capacity > 0;
            if (createNewFile)
            {
                string directory = Path.GetDirectoryName(fullPath);
                if(!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                // FileMode一定要使用CreateNew，否则可能出现覆盖文件的情况
                this.Mmf = MemoryMappedFile.CreateFromFile(fullPath, FileMode.CreateNew, mapName, capacity);
            }
            else
            {
                this.Mmf = MemoryMappedFile.CreateFromFile(fullPath, FileMode.Open, mapName);
            }
        }

        #endregion

        protected static byte[] StructToBytes<T>(T structObj)
            where T : struct
        {
            int size = Marshal.SizeOf(typeof(T));
            IntPtr buffer = Marshal.AllocHGlobal(size);
            try
            {
                Marshal.StructureToPtr(structObj, buffer, false);
                byte[] bytes = new byte[size];
                Marshal.Copy(buffer, bytes, 0, size);
                return bytes;
            }
            finally
            {
                Marshal.FreeHGlobal(buffer);
            }
        }

        protected static T BytesToStruct<T>(byte[] bytes)
            where T : struct
        {
            int size = Marshal.SizeOf(typeof(T));
            IntPtr buffer = Marshal.AllocHGlobal(size);
            try
            {
                Marshal.Copy(bytes, 0, buffer, size);
                return Marshal.PtrToStructure<T>(buffer);
            }
            finally
            {
                Marshal.FreeHGlobal(buffer);
            }
        }
    }
}
