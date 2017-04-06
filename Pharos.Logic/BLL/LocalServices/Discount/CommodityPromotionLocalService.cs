using Pharos.Logic.LocalEntity;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Entity;
using System.Linq;
using Pharos.Utility.Helpers;
using Pharos.Utility;

namespace Pharos.Logic.BLL.LocalServices
{
    /// <summary>
    /// 商品促销主表
    /// </summary>
    public class CommodityPromotionLocalService : BaseLocalService<CommodityPromotion>
    {

    }

    public class CommodityPromotionDAO
    {
        public string CommodityPromotionId { get; set; }
        public short Timeliness { get; set; }
        public string StartAging1 { get; set; }
        public string EndAging1 { get; set; }
        public string StartAging2 { get; set; }
        public string EndAging2 { get; set; }
        public string StartAging3 { get; set; }
        public string EndAging3 { get; set; }
        public short RestrictionBuyNum { get; set; }
        public short CustomerObj { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? StartDate { get; set; }

        public DateTime CreateDT { get; set; } 
    }
}
