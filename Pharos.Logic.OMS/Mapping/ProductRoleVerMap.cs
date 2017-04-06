using Pharos.Logic.OMS.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;

namespace Pharos.Logic.OMS.Mapping
{
    public class ProductRoleVerMap : EntityTypeConfiguration<ProductRoleVer>
    {
        public ProductRoleVerMap()
        {
            HasKey(o => o.RoleVerId);
            HasMany(o => o.ProductRoles).WithOptional().HasForeignKey(o => o.RoleVerId);
        }
    }
}
