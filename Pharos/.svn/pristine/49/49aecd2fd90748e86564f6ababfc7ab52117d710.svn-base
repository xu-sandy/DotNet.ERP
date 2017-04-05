using Pharos.Logic.ApiData.Pos.Sale;
using System;
using System.Collections.Generic;
using System.Linq;
using Pharos.Infrastructure.Data.Cache;
using System.Text;
using Pharos.Infrastructure.Data.Redis;
using Newtonsoft.Json;

namespace Pharos.Logic.ApiData.Pos.Cache
{
    public class ShoppingCartCache
#if(Local!=true)
        : RedisCacheWrapper<ShoppingCart>
#endif
#if(Local)
 : MemoryCacheWrapper<ShoppingCart>
#endif
    {
        public ShoppingCartCache()
            : base("ShoppingCartCache", new TimeSpan(4, 0, 0), true)
        { }
    }
}
