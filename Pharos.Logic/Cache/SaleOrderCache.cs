using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Pharos.Infrastructure.Data.Cache;

namespace Pharos.Logic
{
    public class SaleOrderCache : RedisCacheWrapper<DataTable>
    {
        public SaleOrderCache()
            : base("SaleOrderCache", new TimeSpan(10, 0, 0))
        {
        }
    }
}
