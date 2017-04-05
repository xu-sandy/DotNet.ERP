using Pharos.Logic.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;

namespace Pharos.Logic.Mapping
{
    public class OrderDistributionMap : EntityTypeConfiguration<OrderDistribution>
    {
        public OrderDistributionMap()
        {
            HasMany(o => o.OrderDistributionGifts).WithOptional().HasForeignKey(o => o.OrderDistributionId);
        }
    }
}
