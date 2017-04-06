using Pharos.Logic.Entity;
using Pharos.Sys.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;

namespace Pharos.Logic.Mapping
{
    public class SysLogMap : EntityTypeConfiguration<SysLog>
    {
        public SysLogMap()
        {
            this.ToTable("SysLog");
        }
    }
}
