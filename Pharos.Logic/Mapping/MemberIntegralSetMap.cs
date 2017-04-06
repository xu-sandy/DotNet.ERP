using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using Pharos.Logic.Entity;

namespace Pharos.Logic.Mapping
{
    public class MemberIntegralSetMap : EntityTypeConfiguration<MemberIntegralSet>
    {
        public MemberIntegralSetMap()
        {
            HasMany(o => o.ProductList).WithOptional().HasForeignKey(o => o.IntegralId);
        }
    }
}
