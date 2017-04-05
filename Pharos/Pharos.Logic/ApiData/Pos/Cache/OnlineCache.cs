using System;
using Pharos.Infrastructure.Data.Cache;
using Pharos.Infrastructure.Data.Redis;
using Newtonsoft.Json;

namespace Pharos.Logic.ApiData.Pos.Cache
{
    public class OnlineCache
#if(Local!=true)
: RedisCacheWrapper<MachineInformation>
#endif
#if(Local)
 : AppDomainCacheWrapper<MachineInformation>
#endif
    {
        public OnlineCache()
            : base("OnlineCache")
        {
        }
    }
}
