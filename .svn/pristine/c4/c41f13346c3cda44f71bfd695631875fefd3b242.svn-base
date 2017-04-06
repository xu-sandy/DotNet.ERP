using System;

namespace Pharos.Infrastructure.Data.Cache.Interface
{
    public interface IRedisCacheWrapper<T> : ICache<T>
    {
        string Name { get; }

        TimeSpan SlidingExpiration { get; set; }
    }
}
