using Pharos.Logic.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;

namespace Pharos.Logic.Mapping
{
    public class PrivilegeProductMap : EntityTypeConfiguration<PrivilegeProduct>
    {
        public PrivilegeProductMap()
        {
            HasMany(o => o.RegionVals).WithRequired(o => o.Product).HasForeignKey(o => o.PrivilegeProductId);
        }
    }
}
