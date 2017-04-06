using Pharos.Infrastructure.Data.Redis;
using Pharos.Logic.ApiData.Pos.DAL;
using Pharos.Logic.ApiData.Pos.Entity.LocalCeEntity;
using Pharos.Logic.ApiData.Pos.User;
using Pharos.Logic.ApiData.Pos.ValueObject;
using Pharos.Logic.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.ApiData.Pos.Services.LocalCeServices
{
    public class PosIncomePayoutService : BaseGeneralService<PosIncomePayout, LocalCeDbContext>
    {
        public static void Save(string storeId, string machineSn, string uid, decimal money, PosIncomePayoutMode mode, int companyId, bool isTest)
        {
            try
            {
                CurrentRepository.Add(new PosIncomePayout()
                {
                    Amount = money,
                    CreateDT = DateTime.Now,
                    CreateUID = uid,
                    MachineSN = machineSn,
                    StoreId = storeId,
                    Type = (short)mode,
                    CompanyId = companyId,
                    IsTest = isTest,
                    SyncItemId = Guid.NewGuid(),
                    SyncItemVersion = BitConverter.GetBytes((long)1)
                });
                //RedisManager.Publish("SyncDatabase", "PosIncomePayout");
                StoreManager.PubEvent("SyncDatabase", "PosIncomePayout");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static void DayMonthReport(DateTime from, DateTime to, ref DayReportResult result, string storeId, string machineSn, int companyId, bool inTestMode)
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
                            SaleInfo = new DayReportDetailItem() { Number = 0, Amount = 0m, Project = "销售合计" },
                            PayWay = new List<PayWayItem>()
                        },
                        Other = new List<DayReportDetailItem>()
                    };
                    result.SalesmanRecords.Add(record);
                }
                var rukuan = item.Where(o => o.Type == 1);
                var rukuanheji = new DayReportDetailItem()
                {
                    Project = "入款合计",
                    Amount = rukuan.Sum(o => o.Amount),
                    Number = rukuan.Count()
                };
                record.Cash += rukuanheji.Amount;
                record.Other.Add(rukuanheji);
                var chukuan = item.Where(o => o.Type == 0);
                var chukuanheji = new DayReportDetailItem()
                {
                    Project = "出款合计",
                    Amount = chukuan.Sum(o => o.Amount),
                    Number = chukuan.Count()
                };
                record.Cash -= chukuanheji.Amount;
                record.Other.Add(chukuanheji);

            }
        }
    }
}
