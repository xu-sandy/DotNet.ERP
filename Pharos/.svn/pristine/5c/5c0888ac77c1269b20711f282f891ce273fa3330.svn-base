using Pharos.Logic.Entity;
using Pharos.Sys.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;

namespace Pharos.Logic.Mapping
{
    public class Sys_UserMap : EntityTypeConfiguration<SysUserInfo>
    {
        public Sys_UserMap()
        {
            this.ToTable("SysUserInfo");
            //HasMany(u => u.UserRoles).WithOptional(ur => ur.User).HasForeignKey(r => r.UserID);
        }
    }
}
