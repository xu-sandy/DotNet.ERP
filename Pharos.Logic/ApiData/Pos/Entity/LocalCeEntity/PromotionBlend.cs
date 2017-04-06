using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.ApiData.Pos.Entity.LocalCeEntity
{
    public class PromotionBlend : BaseEntity
    {
        public short RuleType { get; set; }
        public string CommodityId { get; set; }
        public short PromotionType { get; set; }
        public decimal FullNumber { get; set; }
        public decimal DiscountOrPrice { get; set; }
        public decimal PriceRange { get; set; }
        public short AllowedAccumulate { get; set; }
    }
}
