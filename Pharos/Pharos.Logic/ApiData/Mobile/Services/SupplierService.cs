﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pharos.Logic.ApiData.Mobile.Exceptions;
using Pharos.Logic.ApiData.Pos.Exceptions;
using Pharos.Logic.BLL;
using Pharos.Logic.DAL;
using Pharos.Logic.Entity;
using Pharos.Utility.Helpers;
namespace Pharos.Logic.ApiData.Mobile.Services
{
    public class SupplierService : BaseGeneralService<Supplier, EFDbContext>
    {
        public static object GetOrderDeliveryList(string supplierId, string date)
        {
            if (string.IsNullOrWhiteSpace(supplierId))
                throw new MessageException("供应商ID为空!");
            if (string.IsNullOrWhiteSpace(date))
                throw new MessageException("当前日期为空!");
            var query = BaseService<VwOrder>.CurrentRepository.QueryEntity.Where(o=>o.CompanyId==CommonService.CompanyId);
            var st1 = DateTime.Parse(date);
            var st2 = st1.AddMonths(1);
            query = query.Where(o => o.ApproveDT >= st1 && o.ApproveDT < st2 && o.State > 0 && o.SupplierID == supplierId);
            var list = query.OrderBy(o => o.State).OrderByDescending(o => o.ApproveDT).ToList();
            var days = new List<object>();
            var dates = list.Select(o => o.ApproveDT.GetValueOrDefault().Date).GroupBy(o => o.Date).ToList();
            foreach (var dt in dates)
            {
                var orders = list.Where(o => o.ApproveDT.GetValueOrDefault().Date == dt.Key).Select(o => new
                {
                    o.StoreTitle,
                    o.OrderTotal,
                    OrderId = o.IndentOrderId,
                    StateTitle = o.StateTitle,
                    Time = o.ApproveDT.GetValueOrDefault().ToShortTimeString()
                }).ToList();
                days.Add(new { Day = dt.Key.Day, Orders = orders });
            }
            return new { Year = st1.Year, Month = st1.Month, Days = days };
        }
        public static IEnumerable<dynamic> GetOrderNewList(string supplierId)
        {
            if (string.IsNullOrWhiteSpace(supplierId))
                throw new MessageException("供应商ID为空!");
            var query = from x in BaseService<VwOrder>.CurrentRepository.QueryEntity
                        let o = from y in BaseService<Reader>.CurrentRepository.QueryEntity where x.Id == y.MainId && y.Type == 3 && y.ReadCode == supplierId select y
                        orderby x.CreateDT descending
                        where x.CompanyId==CommonService.CompanyId && x.State == 1 && x.SupplierID==supplierId && !o.Any()
                        select x;
            var list = query.ToList();
            var orders = list.Select(o => new
            {
                o.StoreTitle,
                o.OrderTotal,
                OrderId = o.IndentOrderId,
                o.IndentNums,
                Time = o.ApproveDT.HasValue ? o.ApproveDT.Value.ToString("yyyy-MM-dd HH:mm") : o.CreateDT.ToString("yyyy-MM-dd HH:mm")
            }).ToList();
            return orders;
        }
        public static object GetOrderDeliveryDayList(string supplierId, string date)
        {
            if (string.IsNullOrWhiteSpace(supplierId))
                throw new MessageException("供应商ID为空!");
            if (string.IsNullOrWhiteSpace(date))
                throw new MessageException("当前日期为空!");
            var query = BaseService<VwOrder>.CurrentRepository.QueryEntity.Where(o => o.CompanyId == CommonService.CompanyId);
            var st1 = DateTime.Parse(date);
            var st2 = st1.AddDays(1);
            query = query.Where(o => o.CreateDT >= st1 && o.CreateDT < st2 && o.State > 0 && o.SupplierID == supplierId);
            return query.ToList().Select(o => new
            {
                o.StoreTitle,
                o.OrderTotal,
                OrderId = o.IndentOrderId,
                StateTitle = o.StateTitle,
                Time = o.CreateDT.ToShortTimeString()
            });

        }
        public static void OrderDelivery(string orderId, string barcode,string number)
        {
            if (string.IsNullOrWhiteSpace(orderId))
                throw new MessageException("订单号为空!");
            if (string.IsNullOrWhiteSpace(barcode))
                throw new MessageException("条码为空!");
            if (string.IsNullOrWhiteSpace(number))
                throw new MessageException("配送量为空!");
            var bars = barcode.Split(',');
            var nums = number.Split(',');
            if(bars.Count()!=nums.Count())
                throw new MessageException("条码和配送量不一致!");
            var distris =new List<OrderDistribution>();
            for (int i = 0; i < bars.Count(); i++)
            {
                distris.Add(new OrderDistribution() { 
                    Barcode=bars[i],
                    DeliveryNum=decimal.Parse(nums[i]),
                    IndentOrderId=orderId
                });
            }
            var op= BLL.MsppBLL.Save(orderId, distris.ToJson(), "", "", "",false);
            if(!op.Successed)
                throw new MessageException(op.Message);
        }
    }
}
