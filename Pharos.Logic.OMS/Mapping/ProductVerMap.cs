﻿using Pharos.Logic.OMS.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;

namespace Pharos.Logic.OMS.Mapping
{
    public class ProductVerMap : EntityTypeConfiguration<ProductVer>
    {
        public ProductVerMap()
        {
            HasKey(o => o.ProductId);
            //HasMany(o => o.ProductMenus).WithRequired().HasForeignKey(o => o.ProductId).WillCascadeOnDelete(true);
            //HasMany(o => o.ProductLimits).WithRequired().HasForeignKey(o => o.ProductId).WillCascadeOnDelete(true);
            //HasMany(o => o.ProductUpdateLogs).WithRequired().HasForeignKey(o => o.ProductId).WillCascadeOnDelete(true);
        }
    }
}