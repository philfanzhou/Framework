using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Runtime.InteropServices;

namespace Framework.Infrastructure.MemoryMap
{
    public class MemoryMappedFileBase : IDisposable
    {
        private MemoryMappedFile _mmf;
        private readonly string _fullPath;
        protected readonly string FileName;

        #region Constructor

        protected MemoryMappedFileBase(string fullPath) : this(fullPath, -1) { }

        protected MemoryMappedFileBase(string fullPath, long capacity)
        {
            this.FileName = Path.GetFileName(fullPath);
            this._fullPath = fullPath;
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
                this._mmf = MemoryMappedFile.CreateFromFile(fullPath, FileMode.CreateNew, mapName, capacity);
            }
            else
            {
                this._mmf = MemoryMappedFile.CreateFromFile(fullPath, FileMode.Open, mapName);
            }
        }

        #endregion

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
                if (_mmf != null)
                {
                    _mmf.Dispose();
                    _mmf = null;
                }
            }

            Disposed = true;
        }

        #endregion

        #region Property
        public string FullPath
        {
            get { return _fullPath; }
        }
        #endregion

        #region Override

        public override string ToString()
        {
            ThrowIfDisposed();

            return this._fullPath;
        }

        #endregion

        protected void MoveData(long position, long destination, long length)
        {
            var mover = DataMover.Create(position, destination, length);
            mover.Move(_mmf);
        }

        protected IEnumerable<T> ReadData<T>(long position, int count)
            where T : struct
        {
            int size = Marshal.SizeOf(typeof(T));
            List<T> result = new List<T>();
            using (var accessor = _mmf.CreateViewAccessor(position, size * count))
            {
                for (int i = 0; i < count; i++)
                {
                    byte[] array = new byte[size];
                    accessor.ReadArray<byte>(size * i, array, 0, array.Length);
                    result.Add(BytesToStruct<T>(array, size));
                }
            }

            return result;
        }

        protected void WriteData<T>(long position, IEnumerable<T> data)
            where T : struct
        {
            int size = Marshal.SizeOf(typeof(T));
            List<T> dataList = data.ToList();

            using (var accessor = _mmf.CreateViewAccessor(position, size * dataList.Count))
            {
                for (int i = 0; i < dataList.Count; i++)
                {
                    byte[] buffer = StructToBytes(dataList[i], size);
                    accessor.WriteArray<byte>(size * i, buffer, 0, buffer.Length);
                }
            }
        }

        private static byte[] StructToBytes<T>(T structObj, int size)
            where T : struct
        {
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

        private static T BytesToStruct<T>(byte[] bytes, int size)
            where T : struct
        {
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
