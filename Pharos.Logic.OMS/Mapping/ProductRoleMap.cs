using Pharos.Logic.OMS.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;

namespace Pharos.Logic.OMS.Mapping
{
    public class ProductRoleMap : EntityTypeConfiguration<ProductRole>
    {
        public ProductRoleMap()
        {
            HasMany(o => o.ProductRoleDatas).WithOptional().HasForeignKey(o => o.RoleDataId);
        }
    }
}
