using System.Collections.Generic;
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
                    System.Threading.Interlocked.CompareExchange(ref instance, new LockPool(), null);
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
        private Dictionary<string, ReadWriteLockData> _cache = new Dictionary<string, ReadWriteLockData>();

        internal ReaderWriterLockSlim GetLock(string key)
        {
            ReaderWriterLockSlim result = null;
            lock (_lockObj)
            {
                if(_cache.ContainsKey(key))
                {
                    _cache[key].Count++;
                    result = _cache[key].Lock;
                }
                else
                {
                    ReadWriteLockData data = new ReadWriteLockData();
                    data.Count++;
                    data.Lock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
                    result = data.Lock;
                }
            }

            return result;
        }

        internal void ReleaseLock(string key)
        {
            lock (_lockObj)
            {
                if (_cache.ContainsKey(key))
                {
                    _cache[key].Count--;

                    if(_cache[key].Count <= 0)
                    {
                        ReadWriteLockData data = _cache[key];
                        _cache.Remove(key);
                        data.Lock.Dispose();
                    }
                }
            }
        }
    }
}
