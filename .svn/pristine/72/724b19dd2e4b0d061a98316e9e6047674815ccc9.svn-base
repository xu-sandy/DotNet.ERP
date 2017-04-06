using System;
using System.Collections.Generic;
using System.Runtime.Caching;

namespace Pharos.Infrastructure.Data.Cache.Interface
{
    public interface IMemoryCacheWrapper<T> : ICache<T>
    {
        string Name { get; }
        long CacheMemoryLimitInBytes { get; }
        long PhysicalMemoryLimit { get; }
        TimeSpan PollingInterval { get; }
        IEnumerable<string> Keys { get; }
        IEnumerable<T> Items { get; }
        CacheItemPolicy DefaultCacheItemPolicy { get; set; }
        long Count { get; }
        void Dispose();
    }
}
