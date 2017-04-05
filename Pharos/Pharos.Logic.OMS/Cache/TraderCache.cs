using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Pharos.Infrastructure.Data.Cache;
using Pharos.Logic.OMS.Entity.View;

namespace Pharos.Logic.OMS.Cache
{
    public class TraderCache : RedisCacheWrapper<List<RedisTraders>>
    {
        public TraderCache()
            : base("TraderCache", new TimeSpan(10, 0, 0))
        {
        }
    }
}
