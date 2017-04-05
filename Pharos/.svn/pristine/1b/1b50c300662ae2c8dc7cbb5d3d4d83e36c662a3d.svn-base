using Pharos.Logic.BLL;
using Pharos.Logic.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;

namespace Pharos.Logic.Mapping
{
    public class PrivilegeSolutionMap : EntityTypeConfiguration<PrivilegeSolution>
    {
        public PrivilegeSolutionMap()
        {
            HasMany(o => o.Regions).WithRequired(o => o.Solution).HasForeignKey(o => o.PrivilegeSolutionId);
            HasMany(o => o.Products).WithRequired(o => o.Solution).HasForeignKey(o => o.PrivilegeSolutionId);
        }
    }
}
