﻿using Pharos.Logic.ApiData.Pos.DAL;
using Pharos.Logic.ApiData.Pos.Entity.LocalCeEntity;
using Pharos.Logic.ApiData.Pos.Sale;
using Pharos.Logic.ApiData.Pos.ValueObject;
using Pharos.Logic.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.ApiData.Pos.Services.LocalCeServices
{
    public class SaleDetailService : BaseGeneralService<SaleDetail, LocalCeDbContext>
    {
        public static void DayMonthReport(DateTime _from, DateTime _to, ref DayReportResult result, string storeId, string machineSn, int companyId, bool inTestMode, IEnumerable<UserDict> users)
        {
            IQueryable<SaleOrders> queryTemp = SaleOrdersService.CurrentRepository.Entities;
            if (!string.IsNullOrEmpty(machineSn))
            {
                queryTemp = queryTemp.Where(o => o.MachineSN == machineSn);
            }
            var giftStatus = new int[] { (int)SaleStatus.ActivityGifts, (int)SaleStatus.POSGift };

            var query = (from a in CurrentRepository.Entities
                         from b in queryTemp
                         where
                         a.PaySN == b.PaySN
                         && b.CreateDT >= _from
                         && b.CreateDT < _to
                         && b.StoreId == storeId
                         && giftStatus.Contains(a.SalesClassifyId)
                         && a.CompanyId == companyId
                         && b.CompanyId == companyId
                         && b.IsTest == inTestMode
                         group a by b.CreateUID into g
                         select g).ToList();
            var giftAmount = 0m;
            var giftOrderNumber = 0;
            foreach (var item in query)
            {
                //new { b.UserCode, a.CreateUID, b.FullName }
                var user = users.FirstOrDefault(o => o.CreateUID == item.Key);
                var record = result.SalesmanRecords.FirstOrDefault(o => o.UserCode == user.UserCode);
                if (record != null)
                {
                    var tempAmount = item.Sum(o => o.SysPrice * o.PurchaseNumber);
                    giftAmount += tempAmount;
                    var tempOrderNumber = item.Select(o => o.PaySN).Distinct().Count();
                    giftOrderNumber += tempOrderNumber;
                    record.Other.Insert(record.Other.Count - 1, new DayReportDetailItem()
                    {
                        Project = "赠品合计",
                        Amount = tempAmount,
                        Number = tempOrderNumber
                    });
                }
            }
            result.Summary.Insert(result.Summary.Count - 1, new DayReportDetailItem()
            {
                Project = "赠品合计",
                Amount = giftAmount,
                Number = giftOrderNumber
            });

        }
    }
}