using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Framework.Infrastructure.MemoryMap
{
    internal class ReaderWriterLockPool
    {
        private object _lockObj = new object();
        private Dictionary<int, MyReaderWriterLock> _cache = new Dictionary<int, MyReaderWriterLock>();

        #region Singleton
        private static ReaderWriterLockPool instance = new ReaderWriterLockPool();

        private ReaderWriterLockPool() { }

        internal static ReaderWriterLockPool Instance
        {
            get
            {
                return instance;
            }
        }
        #endregion

        internal ReaderWriterLockSlim Get(string key)
        {
            ReaderWriterLockSlim result = null;
            int keyValue = key.GetHashCode();
            lock (_lockObj)
            {
                if(!_cache.ContainsKey(keyValue))
                {
                    MyReaderWriterLock data = new MyReaderWriterLock();
                    _cache.Add(keyValue, data);
                }

                _cache[keyValue].ReferenceCount++;
                result = _cache[keyValue];
            }

            return result;
        }

        internal void Release(string key)
        {
            int keyValue = key.GetHashCode();
            lock (_lockObj)
            {
                if (_cache.ContainsKey(keyValue))
                {
                    _cache[keyValue].ReferenceCount--;

                    if(_cache[keyValue].ReferenceCount <= 0)
                    {
                        _cache[keyValue].Dispose();
                        _cache.Remove(keyValue);
                    }
                }
            }
        }

        #region Private Class
        private class MyReaderWriterLock : ReaderWriterLockSlim
        {
            internal MyReaderWriterLock()
            {
                this.ReferenceCount = 0;
            }

            public int ReferenceCount { get; set; }
        }
        #endregion
    }
}
