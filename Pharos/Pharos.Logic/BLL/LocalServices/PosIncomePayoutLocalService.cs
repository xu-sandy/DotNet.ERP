using Pharos.Logic.LocalEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.BLL.LocalServices
{
    public class PosIncomePayoutLocalService : BaseLocalService<PosIncomePayout>
    {
        public static void Save(decimal jine, short type, string uid, string machineSN, string storeId)
        {
            PosIncomePayoutLocalService.IsForcedExpired = true;
            var repository = PosIncomePayoutLocalService.CurrentRepository;
            repository.Add(
             new PosIncomePayout()
                            {
                                Amount = jine,
                                Type = type,
                                CreateDT = DateTime.Now,
                                CreateUID = uid,
                                MachineSN = machineSN,
                                StoreId = storeId,
                                IsUpload = true
                            });
        }
        public static SalesStatistics GetSalesStatistics(DateTime date, int mode, string machineSn, int type = 1, string uid = "")
        {
            if (mode == 2)
            {
                date = new DateTime(date.Year, date.Month, 1);
            }
            var endDate = (mode == 1 ? date.AddDays(1) : date.AddMonths(1));
            var list = CurrentRepository.FindList(o => (o.MachineSN == machineSn || string.IsNullOrEmpty(machineSn)) && o.CreateDT >= date && o.CreateDT <= endDate && o.CreateUID == uid && o.Type == type).ToList();
            var result = new SalesStatistics();
            if (type == 1)
            {
                result.Title = "入款合计";
            }
            else
            {
                result.Title = "出款合计";
            }
            result.Amount = list.Select(o => o.Amount).Sum();
            result.Count = list.Count;
            return result;
        }
        public static AccountCheckingDAO GetUserAccountInfos(string uid, string machineSn)
        {
            var startTime = PosCheckingLocalService.GetLastPosChecking();
            //入款
            var depositMoney = CurrentRepository
              .FindList(o => o.MachineSN == machineSn && o.CreateDT >= startTime && o.CreateUID == uid && o.Type == 1).ToList()
              .Select(o => (decimal?)o.Amount).Sum() ?? 0;
            //出款
            var takeOutMoney = CurrentRepository
               .FindList(o => o.MachineSN == machineSn && o.CreateDT >= startTime && o.CreateUID == uid && o.Type == 0).ToList()
               .Select(o => (decimal?)o.Amount).Sum() ?? 0;
            //退货
            var returnSales = SalesReturnsLocalService.GetReturnSalesForBJ(startTime, uid, machineSn);

            return new AccountCheckingDAO()
            {
                DepositMoney = depositMoney,
                TakeOutMoney = takeOutMoney,
                TotalSales = SaleOrdersLocalService.GetTotalSales(startTime, uid, machineSn),//销售总额
                ChangingSales = SaleOrdersLocalService.GetChangingSales(startTime, uid, machineSn),//换货补价
                AllPayWayStatistics = ConsumptionPaymentLocalService.GetOnePayWayAmount(startTime, uid, machineSn),//所有支付方式统计
                SurplusCash = depositMoney - takeOutMoney + ConsumptionPaymentLocalService.GetCashPayWay(startTime, uid, machineSn) - returnSales, //当前现金
                ReturnSales = returnSales
            };
        }
    }

    public class AccountCheckingDAO
    {
        public decimal DepositMoney { get; set; }

        public decimal TotalSales { get; set; }
        public decimal ChangingSales { get; set; }

        public decimal TakeOutMoney { get; set; }

        public Dictionary<string, decimal> AllPayWayStatistics { get; set; }

        public decimal SurplusCash { get; set; }

        public decimal ReturnSales { get; set; }
    }
}