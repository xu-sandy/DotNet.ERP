using Pharos.Logic.LocalEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.BLL.LocalServices
{
    public class PromotionBlendListLocalService : BaseLocalService<PromotionBlendList>
    {
        public static List<GiftDAO> GetGiftList(string commodityId)
        {
            var result = CurrentRepository.FindList(o => o.CommodityId == commodityId).ToList();
            List<GiftDAO> giftList = new List<GiftDAO>();
            var series = result.Where(o => o.BlendType == 4);
            foreach (var item in series)
            {
                var sn = Convert.ToInt32(item.BarcodeOrCategorySN);
                var gifts = ProductInfoLocalService.GetSeries(sn).Select(o => new GiftDAO()
                {
                    GiftId = commodityId,
                    Barcode = o.Barcode,
                    Number = 1,
                    SysPrice = o.SysPrice,
                    Title = o.Title,
                    AddMoney = 0,
                    StoreNumber = o.StockNumber
                });
                giftList = giftList.Concat(gifts).ToList();
            }
            var products = result.Where(o => o.BlendType == 3);
            foreach (var item in products)
            {
                var product = ProductInfoLocalService.Find(o => o.Barcode == item.BarcodeOrCategorySN);
                if (product != null)
                    giftList.Add(new GiftDAO()
                    {
                        Title = product.Title,
                        SysPrice = product.SysPrice,
                        Number = 1,
                        Barcode = product.Barcode,
                        GiftId = commodityId,
                        AddMoney = 0,
                        StoreNumber = product.StockNumber
                    });
            }
            return giftList;
        }
    }
}
