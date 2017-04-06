using Newtonsoft.Json;
using Pharos.Infrastructure.Data.Redis;
using Pharos.Logic.ApiData.Pos.Cache;
using System.Collections.Generic;

namespace Pharos.Service.Retailing.RefreshCache
{
    public class ProductCacheManager
    {
        public static void Subscribe()
        {
            RedisManager.Subscribe("RefreshProductCache", (channel, info) =>
            {
                try
                {
                   // Console.WriteLine(info);
                    var qrefresh = JsonConvert.DeserializeObject<IEnumerable<ObjectModels.DTOs.MemoryCacheRefreshQuery>>(info);
                    ProductCache.RefreshProduct(qrefresh);
                }
                catch { }
            });
        }
    }
}
