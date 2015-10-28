using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Framework.Infrastructure.MemoryMap
{
    internal class LockPool
    {
        #region Singleton
        private static LockPool instance;

        private LockPool() { }

        internal static LockPool Instance
        {
            get
            {
                if (null == instance)
                {
                    Interlocked.CompareExchange(ref instance, new LockPool(), null);
                }

                return instance;
            }
        }
        #endregion

        private class ReadWriteLockData
        {
            public ReaderWriterLockSlim Lock { get; set; }

            public int Count { get; set; }
        }

        private object _lockObj = new object();
        private Dictionary<int, ReadWriteLockData> _cache = new Dictionary<int, ReadWriteLockData>();

        [MethodImpl(MethodImplOptions.Synchronized)]
        internal ReaderWriterLockSlim GetLock(string key)
        {
            ReaderWriterLockSlim result = null;
            int keyValue = key.GetHashCode();
            lock (_lockObj)
            {
                if(!_cache.ContainsKey(keyValue))
                {
                    ReadWriteLockData data = new ReadWriteLockData();
                    data.Count = 0;
                    data.Lock = new ReaderWriterLockSlim();
                    _cache.Add(keyValue, data);
                }

                _cache[keyValue].Count++;
                result = _cache[keyValue].Lock;
            }

            return result;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        internal void ReleaseLock(string key)
        {
            int keyValue = key.GetHashCode();
            lock (_lockObj)
            {
                if (_cache.ContainsKey(keyValue))
                {
                    _cache[keyValue].Count--;

                    if(_cache[keyValue].Count <= 0)
                    {
                        ReadWriteLockData data = _cache[keyValue];
                        _cache.Remove(keyValue);
                        data.Lock.Dispose();
                    }
                }
            }
        }
    }
}
