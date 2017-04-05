using Pharos.Logic.OMS.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;

namespace Pharos.Logic.OMS.Mapping
{
    public class SysRoleMap : EntityTypeConfiguration<SysRoles>
    {
        public SysRoleMap()
        {
            HasKey(o => o.RoleId);
            HasMany(o => o.SysRoleDatas).WithOptional().HasForeignKey(o => o.RoleId);
            Property(o => o.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }
    }
}
