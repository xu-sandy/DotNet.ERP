using Pharos.Infrastructure.Data.Cache;
using Pharos.Logic.ApiData.Pos.Sale.Marketings;
using Pharos.ObjectModels.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.ApiData.Pos.Cache
{
    public class ActiveMarketingRuleCache
#if(Local!=true)
: RedisCacheWrapper<IEnumerable<ActiveMarketingRule>>
#endif
#if(Local)
 : MemoryCacheWrapper<IEnumerable<ActiveMarketingRule>>
#endif
    {
        public ActiveMarketingRuleCache()
            : base("ActiveMarketingRuleCache", new TimeSpan(1, 0, 0, 0), true)
        { }
    }
}
