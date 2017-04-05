﻿using Pharos.Logic.LocalEntity;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;

namespace Pharos.Logic.Mapping.LocalMapping
{
    public class CommodityPromotionLocalMap : EntityTypeConfiguration<CommodityPromotion>
    {
        public CommodityPromotionLocalMap() 
        {
            this.ToTable("CommodityPromotion");
        }
    }
}
