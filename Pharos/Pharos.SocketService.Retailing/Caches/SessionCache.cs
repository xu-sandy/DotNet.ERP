using Pharos.Infrastructure.Data.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.SocketService.Retailing.Caches
{
    public class SessionCache : RedisCacheWrapper<string>
    {
        public SessionCache()
            : base("SessionCache", new TimeSpan(0, 5, 0))
        {
        }
    }
}
