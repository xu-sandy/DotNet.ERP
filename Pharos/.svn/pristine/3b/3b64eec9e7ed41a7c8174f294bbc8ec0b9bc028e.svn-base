using Pharos.Logic.OMS.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;

namespace Pharos.Logic.OMS.Mapping
{
    public class ProductDictionaryVerMap : EntityTypeConfiguration<ProductDictionaryVer>
    {
        public ProductDictionaryVerMap()
        {
            HasKey(o => o.DictId);
            HasMany(o => o.ProductDictionaryDatas).WithOptional().HasForeignKey(o => o.DictId);
        }
    }
}
