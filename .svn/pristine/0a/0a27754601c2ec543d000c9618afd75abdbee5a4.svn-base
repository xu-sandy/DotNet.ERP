using Pharos.Logic.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;

namespace Pharos.Logic.Mapping
{
    public class PrivilegeRegionMap : EntityTypeConfiguration<PrivilegeRegion>
    {
        public PrivilegeRegionMap()
        {
            HasMany(o => o.RegionVals).WithRequired(o => o.Region).HasForeignKey(o => o.PrivilegeRegionId);
        }
    }
}
