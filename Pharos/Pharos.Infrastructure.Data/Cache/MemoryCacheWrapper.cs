﻿using Pharos.Infrastructure.Data.Cache.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;

namespace Pharos.Infrastructure.Data.Cache
{
    /*
 * MemoryCache 类类似于 ASP.NET Cache 类。 MemoryCache 类有许多用于访问缓存的属性和方法，
 * 如果您使用过 ASP.NET Cache 类，您将熟悉这些属性和方法。 Cache 和 MemoryCache 类之间的
 * 主要区别是：MemoryCache 类已被更改，以便 .NET Framework 应用程序（非 ASP.NET 应用程序）
 * 可以使用该类。 例如，MemoryCache 类对 System.Web 程序集没有依赖关系。 另一个差别在于您
 * 可以创建 MemoryCache 类的多个实例，以用于相同的应用程序和相同的 AppDomain 实例。【MSDN】
 */
    public class MemoryCacheWrapper<T> : IMemoryCacheWrapper<T>, IDisposable
    where T : class
    {
        private readonly MemoryCache _memoryCache;
        private CacheItemPolicy _defaultCacheItemPolicy;
        private bool _isDisposed;
        private bool _ignoreCase;

        public MemoryCacheWrapper()
            : this(typeof(T).Name)
        {

        }

        public MemoryCacheWrapper(string name, bool ignoreCase = false)
            : this(name, new CacheItemPolicy { AbsoluteExpiration = ObjectCache.InfiniteAbsoluteExpiration, SlidingExpiration = new TimeSpan(0, 5, 0) }, ignoreCase)
        {

        }

        public MemoryCacheWrapper(string name, TimeSpan slidingExpiration, bool ignoreCase = false)
            : this(name, new CacheItemPolicy { AbsoluteExpiration = ObjectCache.InfiniteAbsoluteExpiration, SlidingExpiration = slidingExpiration }, ignoreCase)
        {

        }

        public MemoryCacheWrapper(string name, CacheItemPolicy defaultPolicy, bool ignoreCase = false)
        {
            _isDisposed = false;
            _ignoreCase = ignoreCase;
            _memoryCache = new MemoryCache(name);
            _defaultCacheItemPolicy = defaultPolicy;
            _defaultCacheItemPolicy.RemovedCallback += (o) => { Console.WriteLine(o.RemovedReason); };
        }

        ~MemoryCacheWrapper()
        {
            Dispose(false);
        }

        public string Name
        {
            get { return _memoryCache.Name; }
        }

        public long CacheMemoryLimitInBytes
        {
            get { return _memoryCache.CacheMemoryLimit; }
        }

        public long PhysicalMemoryLimit
        {
            get { return _memoryCache.PhysicalMemoryLimit; }
        }

        public TimeSpan PollingInterval
        {
            get { return _memoryCache.PollingInterval; }
        }

        public CacheItemPolicy DefaultCacheItemPolicy
        {
            get
            {
                return _defaultCacheItemPolicy;
            }
            set
            {
                if (value != null)
                {
                    _defaultCacheItemPolicy = value;
                }
            }
        }

        public virtual void Set(string key, T value)
        {
            if (_ignoreCase)
                key = key.ToUpper();
            _memoryCache.Set(key, value, DefaultCacheItemPolicy);
        }

        public virtual T Get(string key)
        {
            if (_ignoreCase)
                key = key.ToUpper();
            return _memoryCache.Get(key) as T;
        }

        public virtual void Remove(string key)
        {
            if (_ignoreCase)
                key = key.ToUpper();
            _memoryCache.Remove(key);
        }

        public virtual bool ContainsKey(string key)
        {
            if (_ignoreCase)
                key = key.ToUpper();
            return _memoryCache.Contains(key);
        }

        public virtual long Count
        {
            get { return _memoryCache.GetCount(); }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!this._isDisposed)
            {
                if (disposing)
                {
                    _memoryCache.Dispose();
                }
            }
            _isDisposed = true;
        }

        public virtual IEnumerable<string> Keys
        {
            get
            {
                return _memoryCache.Select(p => p.Key);
            }
        }

        public virtual IEnumerable<T> Items
        {
            get
            {
                return _memoryCache.Select(p => (T)p.Value);
            }
        }

        public IDictionary<string, T> Collections
        {
            get
            {
                return _memoryCache.ToDictionary(p => p.Key, p => (T)p.Value);
            }
        }
    }
}
