using Pharos.Logic.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;

namespace Pharos.Logic.Mapping
{
    public class ProductMap : EntityTypeConfiguration<ProductRecord>
    {
        public ProductMap()
        {
            //HasMany(u => u.UserRoles).WithOptional(ur => ur.User).HasForeignKey(r => r.UserID);
            this.Property(o => o.SyncItemVersion).IsRowVersion();
        }
    }
}
