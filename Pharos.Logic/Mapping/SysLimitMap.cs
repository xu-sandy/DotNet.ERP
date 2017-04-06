using Pharos.Logic.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;

namespace Pharos.Logic.Mapping
{
    class SysLimitMap : EntityTypeConfiguration<Sys.Entity.SysLimits>
    {
        public SysLimitMap()
        {
            this.ToTable("SysLimits");
        }
    }
}
