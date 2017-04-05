using Pharos.Logic.LocalEntity;
using Pharos.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.BLL.LocalServices
{
    public class BundlingLocalService : BaseLocalService<Bundling>, IDiscountService<BundlingDAO>
    {
        public IEnumerable<BundlingDAO> LoadDiscount()
        {
            var date = DateTime.Now.Date;

            var query = (from a in CurrentRepository.QueryEntity
                         join b in CommodityPromotionLocalService.CurrentRepository.QueryEntity on a.CommodityId equals b.Id
                         where b.EndDate >= date && b.PromotionType == 2 && b.State != 2
                         select new BundlingDAO()
                         {
                             CommodityPromotionId = a.CommodityId,

                             Barcode = a.NewBarcode,
                             BundledPrice = a.BundledPrice,
                             TotalBundled = a.TotalBundled,
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
                         }).ToList();
            return query;
        }

      
    }
    public class BundlingDAO : CommodityPromotionDAO
    {
        public string Barcode { get; set; }
        public decimal BundledPrice { get; set; }
        public int TotalBundled { get; set; }
    }
}
