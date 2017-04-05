using Pharos.Logic.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;

namespace Pharos.Logic.Mapping
{
    public class ProductChangePriceMap : EntityTypeConfiguration<ProductChangePrice>
    {
        public ProductChangePriceMap()
        {
            HasMany(u => u.ChangePriceList).WithOptional().HasForeignKey(r => r.ChangePriceId);
        }
    }
}
