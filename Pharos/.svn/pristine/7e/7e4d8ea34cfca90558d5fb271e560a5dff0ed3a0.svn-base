using Pharos.Logic.OMS.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;

namespace Pharos.Logic.OMS.Mapping
{
    public class ProductPublishVerMap : EntityTypeConfiguration<ProductPublishVer>
    {
        public ProductPublishVerMap()
        {
            HasKey(o => o.PublishId);
            HasMany(o => o.ProductPublishSqls).WithOptional().HasForeignKey(o => o.PublishId);
            HasMany(o => o.ProductUpdateLogs).WithOptional().HasForeignKey(o => o.PublishId);
            HasOptional(o => o.ProductModuleVer).WithMany().HasForeignKey(o => o.ModuleId);
            HasOptional(o => o.ProductRoleVer).WithMany().HasForeignKey(o => o.RoleId);
            HasOptional(o => o.ProductDictionaryVer).WithMany().HasForeignKey(o => o.DictId);
            HasOptional(o => o.ProductDataVer).WithMany().HasForeignKey(o => o.DataId);
            Property(o => o.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }
    }
}
