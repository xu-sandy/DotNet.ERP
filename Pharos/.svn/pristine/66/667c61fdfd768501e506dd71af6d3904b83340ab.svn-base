using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pharos.Infrastructure.Data.Cache;

namespace Pharos.Logic
{
    public class StockTakingCache : RedisCacheWrapper<string>
    {
        public StockTakingCache()
            : base("StockTakingCache", new TimeSpan(72, 0, 0))
        {
        }
    }
}
