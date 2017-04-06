﻿using Pharos.Logic.OMS.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;

namespace Pharos.Logic.OMS.Mapping
{
    public class ProductDataVerMap : EntityTypeConfiguration<ProductDataVer>
    {
        public ProductDataVerMap()
        {
            HasKey(o => o.DataId);
            HasMany(o => o.ProductDataSqls).WithOptional().HasForeignKey(o => o.DataId);
            Property(o => o.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }
    }
}