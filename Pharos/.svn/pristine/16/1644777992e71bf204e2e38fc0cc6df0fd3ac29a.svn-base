using Pharos.Infrastructure.Data.Cache;
using Pharos.ObjectModels.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.Cache
{
    public class MarketingRuleCache
#if(Local!=true)
: RedisCacheWrapper<IEnumerable<KeyValuePair<MarketingTimelinessLimit, MarketingRule>>>
#endif
#if(Local)
 : MemoryCacheWrapper<IEnumerable<KeyValuePair<MarketingTimelinessLimit, MarketingRule>>>
#endif
    {
        public MarketingRuleCache()
            : base("MarketingRuleCache", new TimeSpan(1, 0, 0, 0), true)
        {
        }
    }
}
