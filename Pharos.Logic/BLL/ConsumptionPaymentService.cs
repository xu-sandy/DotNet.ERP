using Pharos.Logic.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.BLL
{
    public class ConsumptionPaymentService : BaseService<ConsumptionPayment>
    {
        //所有支付方式统计
        public static Dictionary<string, decimal> GetOnePayWayAmount(DateTime startTime, DateTime endTime, string uid, string machineSn)
        {
            var result = from a in CurrentRepository.QueryEntity
                         join b in SaleOrdersService.CurrentRepository.QueryEntity on a.PaySN equals b.PaySN
                         join d in ApiLibraryService.CurrentRepository.QueryEntity on a.ApiCode equals d.ApiCode
                         where  b.CreateDT >= startTime && b.CreateDT <= endTime && b.CreateUID == uid && b.MachineSN == machineSn
                         group a by d.Title into c
                         orderby c.Key
                         select new { Key = c.Key, Value = c.Sum(p => (decimal?)p.Amount) ?? 0 };

            return result.ToDictionary(o => o.Key, p => p.Value);
        }

        public static decimal GetCashPayWay(DateTime startTime, DateTime endTime, string uid, string machineSn, int cashApiCode)
        {
            var result = from a in CurrentRepository.QueryEntity
                         join b in SaleOrdersService.CurrentRepository.QueryEntity on a.PaySN equals b.PaySN
                         where  b.CreateDT >= startTime && b.CreateDT <= endTime && b.CreateUID == uid && b.MachineSN == machineSn && a.ApiCode == cashApiCode
                         select a;
            return result.Sum(o => (decimal?)o.Amount) ?? 0;
        }
    }
}
