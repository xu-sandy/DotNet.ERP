using Pharos.Logic.LocalEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.BLL.LocalServices
{
    public class PromotionBlendLocalService : BaseLocalService<PromotionBlend>, IDiscountService<PromotionBlendDAO>
    {
        public IEnumerable<PromotionBlendDAO> LoadDiscount()
        {
            var date = DateTime.Now.Date;

            var query = (from a in CurrentRepository.QueryEntity
                         join b in CommodityPromotionLocalService.CurrentRepository.QueryEntity on a.CommodityId equals b.Id
                       //  join c in PromotionBlendListLocalService.CurrentRepository.QueryEntity on a.CommodityId equals c.CommodityId
                         where b.EndDate >= date && b.PromotionType == 3 && b.State != 2 && a.RuleType == 1
                         select new PromotionBlendDAO()
                         {
                             CommodityPromotionId = a.CommodityId,
                             RuleType = a.RuleType,
                             PromotionType = a.PromotionType,
                             FullNumber = a.FullNumber,
                             DiscountOrPrice = a.DiscountOrPrice,
                             PriceRange = a.PriceRange,
                             AllowedAccumulate = a.AllowedAccumulate,
                             Timeliness = b.Timeliness,
                             StartAging1 = b.StartAging1,
                             EndAging1 = b.EndAging1,
                             StartAging2 = b.StartAging2,
                             EndAging2 = b.EndAging2,
                             StartAging3 = b.StartAging3,
                             EndAging3 = b.EndAging3,
                             RestrictionBuyNum = b.RestrictionBuyNum,
                             CustomerObj = b.CustomerObj,
                             EndDate = b.EndDate,
                             StartDate = b.StartDate,
                             CreateDT = b.CreateDT
                          //   BarcodeOrCategorySN = c.BarcodeOrCategorySN,
                          //   BlendType = c.BlendType,
                         }).ToList();
            return query;
        }
    }
    public class PromotionBlendDAO : CommodityPromotionDAO
    {
        public short RuleType { get; set; }
        public short PromotionType { get; set; }

        public decimal DiscountOrPrice { get; set; }

        public decimal FullNumber { get; set; }

        public short AllowedAccumulate { get; set; }

        public decimal PriceRange { get; set; }

       // public string BarcodeOrCategorySN { get; set; }

     //   public short BlendType { get; set; }


    }
}
