using Pharos.Logic.LocalEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.BLL.LocalServices
{
    public class FreeGiftPurchaseListLocalService : BaseLocalService<FreeGiftPurchaseList>
    {
        public static List<GiftDAO> GetGiftList(string giftId)
        {
            var result = CurrentRepository.FindList(o => o.GiftId == giftId).ToList();
            List<GiftDAO> giftList = new List<GiftDAO>();
            var series = result.Where(o => o.GiftType == 2);
            foreach (var item in series)
            {
                var sn = Convert.ToInt32(item.BarcodeOrCategorySN);
                var gifts = ProductInfoLocalService.GetSeries(sn).Select(o => new GiftDAO()
                   {
                       GiftId = giftId,
                       Barcode = o.Barcode,
                       Number = item.GiftNumber,
                       SysPrice = o.SysPrice,
                       Title = o.Title,
                       AddMoney = 0,
                       StoreNumber = o.StockNumber

                   });
                giftList = giftList.Concat(gifts).ToList();
            }
            var products = result.Where(o => o.GiftType == 1);
            foreach (var item in products)
            {
                var product = ProductInfoLocalService.Find(o => o.Barcode == item.BarcodeOrCategorySN);
                if (product != null)
                    giftList.Add(new GiftDAO()
                    {
                        Title = product.Title,
                        SysPrice = product.SysPrice,
                        Number = item.GiftNumber,
                        Barcode = product.Barcode,
                        GiftId = giftId,
                        AddMoney = 0,
                        StoreNumber = product.StockNumber
                    });
            }
            return giftList;
        }
    }

    public class GiftDAO
    {
        public string CommodityId { get; set; }

        public string Barcode { get; set; }

        public string Title { get; set; }

        public int Number { get; set; }

        public decimal SysPrice { get; set; }

        public string GiftId { get; set; }

        public decimal StoreNumber { get; set; }

        public decimal AddMoney { get; set; }
    }
}
