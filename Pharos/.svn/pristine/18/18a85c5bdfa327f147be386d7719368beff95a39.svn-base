using Pharos.Logic.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;

namespace Pharos.Logic.Mapping
{
    public class ProductTradePriceMap : EntityTypeConfiguration<ProductTradePrice>
    {
        public ProductTradePriceMap()
        {
            HasMany(u => u.TradePriceList).WithOptional().HasForeignKey(r => r.TradePriceId);
        }
    }
}
