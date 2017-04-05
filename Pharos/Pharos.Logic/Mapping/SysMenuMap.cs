using Pharos.Logic.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;

namespace Pharos.Logic.Mapping
{
    public class SysMenuMap : EntityTypeConfiguration<Sys.Entity.SysMenus>
    {
        public SysMenuMap()
        {
            this.ToTable("SysMenus");
        }
    }
}
