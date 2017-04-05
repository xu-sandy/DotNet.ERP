using Pharos.Logic.ApiData.Pos.ValueObject;
using Pharos.Logic.BLL;
using Pharos.Logic.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.ApiData.Pos.Services.ServerServices
{
    public class MemberRechargeService : BaseGeneralService<Pharos.Logic.Entity.MemberRecharge, EFDbContext>
    {
        internal static void DayMonthReport(DateTime from, DateTime to, ref DayReportResult result, string storeId, string machineSn, int companyId, bool inTestMode)
        {
            var entities = CurrentRepository.Entities.Where(o => o.CreateDT >= from && o.CreateDT < to && o.StoreId == storeId && o.CompanyId == companyId && o.IsTest == inTestMode);
            if (!string.IsNullOrEmpty(machineSn))
            {
                entities = entities.Where(o => o.MachineSN == machineSn);
            }
            var users = (from a in entities
                         from b in SysStoreUserInfoService.CurrentRepository.Entities
                         where a.CreateUID == b.UID && b.CompanyId == companyId
                         select new { b.UserCode, a.CreateUID, b.FullName }
                         ).ToList();

            var query = entities.GroupBy(o => o.CreateUID).ToList();
            foreach (var item in query)
            {
                var user = users.FirstOrDefault(o => o.CreateUID == item.Key);
                var record = result.SalesmanRecords.FirstOrDefault(o => o.UserCode == user.UserCode);
                if (record == null)
                {
                    record = new SalesmanDayReportResult()
                    {
                        StartTime = DateTime.Now,
                        EndTime = DateTime.Now,
                        UserCode = user.UserCode,
                        Salesman = user.FullName,
                        Sale = new SalesmanDayReportSaleResult()
                        {
                            SaleInfo = new DayReportDetailItem() { Number = 0, Amount = 0m, Project = "销售小计" },
                            PayWay = new List<PayWayItem>()
                        },
                        Other = new List<DayReportDetailItem>()
                    };
                    result.SalesmanRecords.Add(record);
                }
                var chongzhi = item.Where(o => o.Type == 1);
                var chongzhiheji = new DayReportDetailItem()
                {
                    Project = "充值小计",
                    Amount = chongzhi.Sum(o => o.RechargeAmount),
                    Number = chongzhi.Count()
                };
                record.Cash += chongzhiheji.Amount;
                record.Other.Insert(0, chongzhiheji);
                var fanjiesuan = item.Where(o => o.Type == 2);
                var fanjiesuanheji = new DayReportDetailItem()
                {
                    Project = "反结算小计",
                    Amount = -fanjiesuan.Sum(o => o.RechargeAmount),
                    Number = fanjiesuan.Count()
                };
                record.Other.Insert(0, fanjiesuanheji);
                record.Cash += fanjiesuanheji.Amount;

                var heji = record.Other.FirstOrDefault(o => o.Project == "合计");
                if (heji == null)
                {
                    heji = new DayReportDetailItem()
                     {
                         Amount = 0m,
                         Number = 0,
                         Project = "合计"
                     };
                    result.Summary.Add(heji);
                }
                heji.Amount += chongzhiheji.Amount;
                heji.Amount += fanjiesuanheji.Amount;

                heji.Number += chongzhiheji.Number;
            }
            if (result.SalesmanRecords.Count > 0)
            {
                var amount = result.SalesmanRecords.Sum(o => o.Other.Sum(j => { return j != null && j.Project == "充值小计" ? j.Amount : 0m; }));
                var num = result.SalesmanRecords.Sum(o => o.Other.Sum(j => { return j != null && j.Project == "充值小计" ? j.Number : 0; }));


                result.Summary.Insert(0, new DayReportDetailItem()
                 {
                     Amount = amount,
                     Number = num,
                     Project = "充值小计"
                 });
                var amount1 = result.SalesmanRecords.Sum(o => o.Other.Sum(j => { return j != null && j.Project == "反结算小计" ? j.Amount : 0m; }));
                var num1 = result.SalesmanRecords.Sum(o => o.Other.Sum(j => { return j != null && j.Project == "反结算小计" ? j.Number : 0; }));
                result.Summary.Insert(0, new DayReportDetailItem()
                {
                    Amount = amount1,
                    Number = num1,
                    Project = "反结算小计"
                });
                var heji = result.Summary.FirstOrDefault(o => o.Project == "合计");
                if (heji != null)
                {
                    heji.Number += num;
                    heji.Amount += amount;
                    heji.Amount += amount1;
                }
            }
            else
            {
                result.Summary.Insert(0, new DayReportDetailItem()
                {
                    Amount = 0m,
                    Number = 0,
                    Project = "充值小计"
                });
            }
        }
    }
}
