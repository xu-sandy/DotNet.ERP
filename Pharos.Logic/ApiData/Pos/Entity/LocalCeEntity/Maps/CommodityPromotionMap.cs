using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;

namespace Pharos.Logic.ApiData.Pos.Entity.LocalCeEntity.Maps
{
    public class CommodityPromotionMap : EntityTypeConfiguration<CommodityPromotion>
    {
        public CommodityPromotionMap()
        {
            //HasMany(o => o.CommodityDiscounts).WithOptional().HasForeignKey(o => o.CommodityId);
            //HasMany(o => o.Bundlings).WithOptional().HasForeignKey(o => o.CommodityId);
            //HasMany(o => o.BundlingDetails).WithOptional().HasForeignKey(o => o.CommodityId);
            //HasMany(o => o.Blends).WithOptional().HasForeignKey(o => o.CommodityId);
            //HasMany(o => o.BlendDetails).WithOptional().HasForeignKey(o => o.CommodityId);
            //HasMany(o => o.FreeGiftPurchases).WithOptional().HasForeignKey(o => o.CommodityId);
        }
    }
}
