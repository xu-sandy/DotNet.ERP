using Pharos.Logic.LocalEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.BLL.LocalServices
{
    public class SalesReturnsLocalService : BaseLocalService<SalesReturns>
    {

        public static void Save(List<SalesReturns> entities)
        {
            SalesReturnsLocalService.IsForcedExpired = true;
            var repository = SalesReturnsLocalService.CurrentRepository;
            repository.AddRange(entities);
        }


        internal static decimal GetReturnSalesForBJ(DateTime startTime, string uid, string machineSn)
        {
            if (CurrentRepository.Entities.Count() > 0)
                return CurrentRepository.Entities.Where(o => o.CreateDT >= startTime && o.ReturnType == 0 && o.CreateUID == uid).ToList().Sum(o => o.ReturnPrice);
            return 0m;
        }

        public static SalesStatistics GetReturnSales(DateTime startTime,int mode, string machineSn="", string uid = "")
        {
            var result = new SalesStatistics();
            if (mode == 2)
            {
                startTime = new DateTime(startTime.Year, startTime.Month, 1);
            }
            var endDate = (mode == 1 ? startTime.AddDays(1) : startTime.AddMonths(1));

            var list = CurrentRepository.Entities.Where(o => o.CreateDT >= startTime && o.CreateDT <= endDate && (o.ReturnType == 0 || o.ReturnType ==2) && (o.MachineSN == machineSn || string.IsNullOrEmpty(machineSn)) && (o.CreateUID == uid || string.IsNullOrEmpty(uid))).ToList().Distinct();
            result.Amount = list.Sum(o => o.ReturnPrice);
            result.Count = list.Count();
            result.Title = "退货合计";
            return result;
        }

    }
    public class SalesStatistics
    {
        /// <summary>
        /// 笔数
        /// </summary>
        public decimal Count { get; set; }

        public decimal Amount { get; set; }

        public string Title { get; set; }

        public DateTime FristSale { get; set; }
        public DateTime LastSale { get; set; }
    }
}
