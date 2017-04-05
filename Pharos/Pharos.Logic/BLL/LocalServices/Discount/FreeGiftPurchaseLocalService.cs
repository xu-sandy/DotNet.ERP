﻿using Pharos.Logic.LocalEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.BLL.LocalServices
{
    public class FreeGiftPurchaseLocalService : BaseLocalService<FreeGiftPurchase>, IDiscountService<FreeGiftPurchaseDAO>
    {
        public IEnumerable<FreeGiftPurchaseDAO> LoadDiscount()
        {
            var date = DateTime.Now.Date;
            var query = (from a in CurrentRepository.QueryEntity
                         join b in CommodityPromotionLocalService.CurrentRepository.QueryEntity on a.CommodityId equals b.Id
                         where b.EndDate >= date && b.PromotionType == 4 && b.State != 2
                         select new FreeGiftPurchaseDAO()
                         {
                             CommodityPromotionId = a.CommodityId,
                             BarcodeOrCategorySN = a.BarcodeOrCategorySN,
                             GiftRestrictionBuyNum = a.RestrictionBuyNum,
                             GiftId = a.GiftId,
                             MinPurchaseNum = a.MinPurchaseNum,
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
                         }).ToList();
            return query;
        }
    }
    public class FreeGiftPurchaseDAO : CommodityPromotionDAO
    {

        public string BarcodeOrCategorySN { get; set; }

        public short GiftRestrictionBuyNum { get; set; }

        public string GiftId { get; set; }

        public int MinPurchaseNum { get; set; }
    }
}
