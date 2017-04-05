using Pharos.Infrastructure.Data.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.ApiData.Pos.Cache
{
    public class SyncLogCache : MemoryCacheWrapper<string>
    {
        public SyncLogCache()
            : base("SyncLogCache", new TimeSpan(7, 0, 0, 0), true)
        { }
    }
}
