using Pharos.Logic.LocalEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.BLL.LocalServices
{
    public class SaleDetailLocalService : BaseLocalService<SaleDetail>
    {

        public static void Save(List<SaleDetail> billList)
        {
            SaleDetailLocalService.IsForcedExpired = true;
            var repository = SaleDetailLocalService.CurrentRepository;
            var first = billList.FirstOrDefault();
            if (first == null)
                return;
            if (repository.IsExist(o => o.PaySN == first.PaySN))
            {
                return;
            }
            repository.AddRange(billList);
        }

        public static List<SaleDetail> GetLastOrderDetails(string sn)
        {
            return CurrentRepository.Entities.Where(o => o.PaySN == sn).ToList();
        }


        public static List<SaleDetailDao> GetOrderDetails(string sn)
        {
            var query = (from a in CurrentRepository.Entities.Where(o => o.PaySN == sn)
                         from b in ProductInfoLocalService.CurrentRepository.Entities
                         where a.Barcode == b.Barcode
                         select new SaleDetailDao()
                         {
                             Barcode = a.Barcode,
                             Number = a.PurchaseNumber,
                             ProductCode = b.ProductCode,
                             SalePrice = a.ActualPrice,
                             SysPrice = b.SysPrice,
                             Title = b.Title + " " + (String.IsNullOrEmpty(b.Size) ? "" : b.Size)
                         }
                ).ToList();
            //var query1 = (from a in CurrentRepository.Entities.Where(o => o.PaySN == sn)
            //              from b in BundlingService.CurrentRepository.Entities
            //              where a.Barcode == b.NewBarcode && a.ActualPrice > 0
            //              select new SaleDetailDao()
            //              {
            //                  Barcode = a.Barcode,
            //                  Number = a.PurchaseNumber,
            //                  ProductCode = "无货号",
            //                  SalePrice = a.ActualPrice,
            //                  SysPrice = b.BundledPrice,
            //                  Title = b.Title

            //              }).ToList();
            //query.Concat(query1);
            return query.ToList();
        }
        public static SalesStatistics GetSalesStatistics(DateTime date, int mode, string machineSn = "", string uid = "")
        {
            var result = new SalesStatistics() { Title = "赠送合计" };
            if (mode == 2)
            {
                date = new DateTime(date.Year, date.Month, 1);
            }
            var enddate = (mode == 1 ? date.AddDays(1) : date.AddMonths(1));

            var list = (from a in CurrentRepository.Entities
                        from b in SaleOrdersLocalService.CurrentRepository.Entities
                        where a.PaySN == b.PaySN && b.Type == 0 && b.CreateDT >= date && b.CreateDT <= enddate && (b.MachineSN == machineSn || string.IsNullOrEmpty(machineSn)) && (b.CreateUID == uid || string.IsNullOrEmpty(uid)) && a.ActualPrice == 0
                        select a
                           ).ToList();
            result.Amount = list.Sum(o => o.SysPrice * o.PurchaseNumber);
            result.Count = list.Select(o => o.PaySN).Distinct().Count();
            return result;
        }

    }
    public class SaleDetailDao
    {
        public string Barcode { get; set; }
        public string ProductCode { get; set; }

        public string Title { get; set; }

        public decimal Number { get; set; }

        public decimal SysPrice { get; set; }

        public decimal SalePrice { get; set; }


    }
}
