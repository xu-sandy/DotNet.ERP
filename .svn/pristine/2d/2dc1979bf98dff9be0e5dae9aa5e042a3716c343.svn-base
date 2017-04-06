using Pharos.Logic.LocalEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.BLL.LocalServices
{
    public class KeyValue
    {
        public string Title { get; set; }
        public decimal Value { get; set; }
    }
    public class ConsumptionPaymentLocalService : BaseLocalService<ConsumptionPayment>
    {

        //所有支付方式统计
        public static Dictionary<string, decimal> GetOnePayWayAmount(DateTime startTime, string uid, string machineSn)
        {
            var result = (from a in CurrentRepository.QueryEntity
                          join b in SaleOrdersLocalService.CurrentRepository.QueryEntity on a.PaySN equals b.PaySN
                          join d in ApiLibraryLocalService.CurrentRepository.QueryEntity on a.ApiCode equals d.ApiCode
                          where b.CreateDT >= startTime && b.CreateUID == uid && b.MachineSN == machineSn
                          group a by d.Title into c
                          orderby c.Key
                          select new { Key = c.Key, Value = c.Sum(p => (decimal?)p.Amount) ?? 0 }).ToList();

            return result.ToDictionary(o => o.Key, p => p.Value);
        }

        public static Dictionary<string, decimal> GetOnePayWayAmountForRJ(DateTime startTime, int mode, string machineSn, string uid = "")
        {

            if (mode == 2)
            {
                startTime = new DateTime(startTime.Year, startTime.Month, 1);
            }
            var date = (mode == 1 ? startTime.AddDays(1) : startTime.AddMonths(1));
            //var result = (from a in CurrentRepository.QueryEntity
            //              join b in SaleOrdersLocalService.CurrentRepository.QueryEntity on a.PaySN equals b.PaySN
            //              join d in ApiLibraryLocalService.CurrentRepository.QueryEntity on a.ApiCode equals d.ApiCode
            //              where b.CreateDT >= startTime && b.CreateDT <= date && (b.CreateUID == uid || string.IsNullOrEmpty(uid)) && (b.MachineSN == machineSn || string.IsNullOrEmpty(machineSn))
            //              group a by d.Title into c
            //              orderby c.Key
            //              select new { Key = c.Key, Value = c.Sum(p => (decimal?)p.Amount) ?? 0 }).ToList();

            var machineSnP = string.Empty;
            var uidP = string.Empty;
            if (!string.IsNullOrEmpty(machineSn))
                machineSnP = string.Format(" and b.MachineSN='{0}'", machineSn);
            if (!string.IsNullOrEmpty(uid))
                uidP = string.Format(" and b.CreateUID='{0}'", uid);
            var result = CurrentRepository._context.Database.SqlQuery<KeyValue>(string.Format(@"select c.Title,sum(a.Amount) as Value from  ConsumptionPayment a ,SaleOrders b ,ApiLibrary c
 where a.PaySN = b.PaySN and  a.ApiCode = c.ApiCode
 and  a.id  in ( select Id    from ConsumptionPayment  group by paysn,apicode )
 and b.CreateDT >= '{0}' and  b.CreateDT <= '{3}'{1} {2}
 group by c.Title", startTime.ToString("yyyy-MM-dd"), machineSnP, uidP, date.ToString("yyyy-MM-dd")));


            return result.ToDictionary(o => o.Title, o => o.Value);
        }

        public static decimal GetCashPayWay(DateTime startTime, string uid, string machineSn)
        {
            var cashApiCode = ApiLibraryLocalService.GetPayCode("现金支付");
            var result = from a in CurrentRepository.QueryEntity
                         join b in SaleOrdersLocalService.CurrentRepository.QueryEntity on a.PaySN equals b.PaySN
                         where b.CreateDT >= startTime && b.CreateUID == uid && b.MachineSN == machineSn && a.ApiCode == cashApiCode
                         select a;
            return result.Sum(o => (decimal?)o.Amount) ?? 0;
        }

        public static void Save(ConsumptionPayment record)
        {
            ConsumptionPaymentLocalService.IsForcedExpired = true;
            var repository = ConsumptionPaymentLocalService.CurrentRepository;
            repository.Add(record);
        }

        public static string GetPayWay(string sn)
        {
            var result = CurrentRepository.Entities.Where(o => o.PaySN == sn);
            var query = (from a in result
                         from b in ApiLibraryLocalService.CurrentRepository.Entities
                         from c in SysDataDictionaryLocalService.CurrentRepository.Entities
                         where b.ApiCode == a.ApiCode && b.ApiType == c.DicSN
                         select c.Title
                            ).ToList();

            if (query.Count > 1)
            {
                return "多方式付款";
            }
            else if (query.Count == 1)
            {
                return query[0];
            }
            else
            {
                return "未知付款方式";
            }
        }
    }
}
