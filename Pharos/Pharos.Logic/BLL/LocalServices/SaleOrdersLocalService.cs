﻿using Pharos.Logic.LocalEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.BLL.LocalServices
{
    public class SaleOrdersLocalService : BaseLocalService<SaleOrders>
    {
        /// <summary>
        /// 销售总额
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="uid"></param>
        /// <param name="machineSn"></param>
        /// <returns></returns>
        public static decimal GetTotalSales(DateTime startTime, string uid, string machineSn)
        {
            return CurrentRepository.FindList(o => o.CreateDT >= startTime && o.CreateUID == uid && o.MachineSN == machineSn && o.Type == 0)
                .Select(o => (decimal?)o.TotalAmount)
                .Sum() ?? 0;
        }
        /// <summary>
        /// 换货补价
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="uid"></param>
        /// <param name="machineSn"></param>
        /// <returns></returns>
        public static decimal GetChangingSales(DateTime startTime, string uid, string machineSn)
        {
            return CurrentRepository.FindList(o => o.CreateDT >= startTime && o.CreateUID == uid && o.MachineSN == machineSn && o.Type == 1)
                .Select(o => (decimal?)o.TotalAmount).ToList()
                .Sum() ?? 0;
        }

        public static void Save(SaleOrders Infos)
        {
            SaleOrdersLocalService.IsForcedExpired = true;
            var repository = SaleOrdersLocalService.CurrentRepository;
            var order = SaleOrdersLocalService.Find(o => o.PaySN == Infos.PaySN);
            if (order != null)
            {
                order.ApiCode += "," + Infos.ApiCode;
                repository.Update(order);
            }
            else
            {
                repository.Add(Infos);
            }

        }
        public static SaleOrders GetLastSaleOrder()
        {
            return CurrentRepository.Entities.FirstOrDefault(o => o.CreateDT == CurrentRepository.Entities.Max(p => p.CreateDT));
        }
        public static SalesStatistics GetChangingSalesStatistics(DateTime startTime, int mode, string machineSn = "", string uid = "")
        {
            if (mode == 2)
            {
                startTime = new DateTime(startTime.Year, startTime.Month, 1);
            }
            var endDate = (mode == 1 ? startTime.AddDays(1) : startTime.AddMonths(1));

            var result = new SalesStatistics() { Title = "换货合计" };

            var list = CurrentRepository.FindList(o => o.CreateDT >= startTime && o.CreateDT <= endDate && (o.CreateUID == uid || string.IsNullOrEmpty(uid)) && (o.MachineSN == machineSn || string.IsNullOrEmpty(machineSn)) && o.Type == 1)
                .Select(o => (decimal?)o.TotalAmount).ToList().Distinct();
            result.Amount = list.Sum() ?? 0;
            result.Count = list.Count();
            return result;
        }
        public static SalesStatistics GetSalesStatistics(DateTime data, int mode, string machineSn = "", string uid = "")
        {
            if (mode == 2)
            {
                data = new DateTime(data.Year, data.Month, 1);
            }
            var endDate = (mode == 1 ? data.AddDays(1) : data.AddMonths(1));
            var result = new SalesStatistics() { Title = "销售小计" };
            var list = CurrentRepository.Entities.Where(o => o.CreateDT >= data && o.CreateDT <= endDate && (o.MachineSN == machineSn || string.IsNullOrEmpty(machineSn)) && (o.CreateUID == uid || string.IsNullOrEmpty(uid))).ToList().Distinct();
            result.Amount = list.Sum(o => o.TotalAmount);
            result.Count = list.Count();
            if (result.Count > 0)
            {
                result.FristSale = list.Min(o => o.CreateDT);
                result.LastSale = list.Max(o => o.CreateDT);
            }
            return result;
        }

        public static List<SaleOrders> GetLastSaleOrders(DateTime date)
        {
            var endData = date.AddDays(1);
            var list = CurrentRepository.Entities.Where(o => o.CreateDT >= date && o.CreateDT <= endData).OrderByDescending(o => o.CreateDT);
            return list.ToList();
        }
    }
}

