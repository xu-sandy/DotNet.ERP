﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using Pharos.Logic.ApiData.Mobile.Exceptions;
using Pharos.Logic.ApiData.Pos.Exceptions;
using Pharos.Logic.BLL;
using Pharos.Logic.Entity;
using Pharos.Utility.Helpers;
namespace Pharos.Logic.ApiData.Mobile.Services
{
    public class OrderService
    {
        public static object GetOrderDetail(string orderId,string userCode,short type)
        {
            if (string.IsNullOrWhiteSpace(orderId))
                throw new MessageException("订单号为空!");
            if (string.IsNullOrWhiteSpace(userCode))
                throw new MessageException("用户编号为空!");
            var query = from a in BaseService<VwOrder>.CurrentRepository.QueryEntity
                        join b in BaseService<IndentOrderList>.CurrentRepository.QueryEntity on a.IndentOrderId equals b.IndentOrderId
                        join c in BaseService<VwProduct>.CurrentRepository.QueryEntity on new { a.CompanyId, b.Barcode } equals new { c.CompanyId, c.Barcode }
                        where a.CompanyId==Sys.SysCommonRules.CompanyId && a.IndentOrderId == orderId
                        select new
                        {
                            a,
                            c.ProductCode,
                            c.Barcode,
                            c.Title,
                            SysPrice = b.SysPrice ?? c.SysPrice,
                            c.SubUnit,
                            b.IndentNum,
                            b.DeliveryNum,
                            b.Subtotal,
                            b.Nature
                        };
            var list = query.ToList();
            if (!list.Any()) return null;
            var order = list.Select(o => o.a).FirstOrDefault();
            var details = list.Where(o => o.Nature == 0).Select(o => new Detail() { 
                ProductCode=o.ProductCode,
                Barcode=o.Barcode,
                Title=o.Title,
                IndentNum = o.IndentNum,
                DeliveryNum=o.DeliveryNum,
                SysPrice=o.SysPrice,
                SubUnit=o.SubUnit,
                Subtotal=o.Subtotal,
                Nature=""
            }).ToList();
            foreach (var gift in list.Where(o => o.Nature == 1))
            {
                var gt = details.FirstOrDefault(o => o.Barcode == gift.Barcode && o.Nature == "赠品");
                if (gt == null)
                    details.Add(new Detail()
                    {
                        ProductCode = gift.ProductCode,
                        Barcode = gift.Barcode,
                        Title = gift.Title,
                        IndentNum = gift.IndentNum,
                        DeliveryNum = gift.DeliveryNum,
                        SysPrice = gift.SysPrice,
                        SubUnit = gift.SubUnit,
                        Subtotal = gift.Subtotal,
                        Nature = "赠品"
                    });
                else
                {
                    gt.IndentNum += gift.IndentNum;
                    gt.Subtotal += gift.Subtotal;
                }
            }
            var obj = new
            {
                OrderId=order.IndentOrderId,
                order.StoreTitle,
                order.RecipientsTitle,
                order.SupplierTitle,
                order.OrderTotal,
                order.ShippingAddress,
                order.DeliveryDate,
                StateTitle=GetStateTitle(order.State,type),
                Details = details
            };
            var user = UserInfoService.Find(o => o.CompanyId == Sys.SysCommonRules.CompanyId && o.UserCode == userCode);
            if (user == null)
                throw new MessageException("用户编码不存在!");
            ReaderService.Add(type, user.UID, new List<int>() { order.Id });
            return obj;
        }
        public static object GetOrderApproveList(string date, string pageIndex, string pageSize)
        {
            if (string.IsNullOrWhiteSpace(date))
                throw new MessageException("当前日期为空!");
            var query = BaseService<VwOrder>.CurrentRepository.QueryEntity.Where(o=>o.CompanyId==Sys.SysCommonRules.CompanyId);
            var st1 = DateTime.Parse(date).Date;
            var st2 = st1.AddMonths(1);
            query = query.Where(o => o.CreateDT >= st1 && o.CreateDT < st2 && (o.State ==0 || o.State== 1));
            var nl= new NameValueCollection();
            nl.Add("page", pageIndex);
            nl.Add("rows", pageSize);
            var list = query.OrderBy(o => o.State).ThenByDescending(o => o.CreateDT).ToList();
            var days = new List<object>();
            var dates =list.Select(o=>o.CreateDT.Date).GroupBy(o=>o.Date).ToList();
            foreach(var dt in dates)
            {
                var orders = list.Where(o => o.CreateDT.Date == dt.Key).Select(o => new
                {
                    o.StoreTitle,
                    o.OrderTotal,
                    OrderId = o.IndentOrderId,
                    StateTitle = o.State==0?"待审批":o.State==1?"已审批":"未提交",
                    Time = o.CreateDT.ToShortTimeString()
                }).ToList();
                days.Add(new { Day = dt.Key.Day, Orders = orders });
            }
            return new {Year=st1.Year,Month=st1.Month,Days=days };

        }
        public static IEnumerable<dynamic> GetOrderApproveNewList(string userCode)
        {
            if (userCode.IsNullOrEmpty()) throw new MessageException("员工编号为空!");
            var user = UserInfoService.Find(o => o.CompanyId == Sys.SysCommonRules.CompanyId && o.UserCode == userCode);
            if (user == null) throw new MessageException("用户不存在!");
            var query = from x in BaseService<VwOrder>.CurrentRepository.QueryEntity
                        let o = from y in BaseService<Reader>.CurrentRepository.QueryEntity where x.Id == y.MainId && y.Type == 2 && y.ReadCode == user.UID select y
                        orderby x.CreateDT descending
                        where x.State == 0 && !o.Any()
                        select x;
            var list = query.ToList();
            var orders = list.Select(o => new
            {
                o.StoreTitle,
                o.OrderTotal,
                OrderId = o.IndentOrderId,
                o.IndentNums,
                Time = o.CreateDT.ToString("yyyy-MM-dd HH:mm")
            }).ToList();
            return orders;
        }
        public static object GetOrderApproveDayList(string date)
        {
            if (string.IsNullOrWhiteSpace(date))
                throw new MessageException("当前日期为空!");
            var query = BaseService<VwOrder>.CurrentRepository.QueryEntity.Where(o=>o.CompanyId==Sys.SysCommonRules.CompanyId);
            var st1 = DateTime.Parse(date);
            var st2 = st1.AddDays(1);
            query = query.Where(o => o.CreateDT >= st1 && o.CreateDT < st2 && o.State < 2);
            return query.ToList().Select(o => new
            {
                o.StoreTitle,
                o.OrderTotal,
                OrderId = o.IndentOrderId,
                StateTitle = o.StateTitle,
                Time = o.CreateDT.ToShortTimeString()
            });

        }
        public static void Approve(string orderId)
        {
            var obj = BaseService<IndentOrder>.Find(o => o.IndentOrderId == orderId && o.State<1 && o.CompanyId==Sys.SysCommonRules.CompanyId);
            if (obj == null) throw new MessageException("订单号不存在!");
            obj.State = 1;
            obj.ApproveDT = DateTime.Now;
            BaseService<IndentOrder>.Update(obj);
        }
        // <param name="type">2-订单新审批,3-供应商新订单</param>
        static string GetStateTitle(short state, short type)
        {
            if (type == 2)
                return state == 0 ? "待审批" : state == 1 ? "已审批" : "未提交";
            else if (type == 3)
                return state == 0 ? "待审批" : state == 1 ? "未配送" : state == 4 ? "已配送" : state == 3 ? "已中止" : state == 5 ? "已收货" : "配送中";
            return "";
        }
        class Detail
        {
            public string ProductCode{get;set;}
            public string Barcode{get;set;}
            public string Title{get;set;}
            public decimal  SysPrice{get;set;}
            public string  SubUnit{get;set;}
            public decimal IndentNum{get;set;}
            public decimal DeliveryNum{get;set;}
            public decimal Subtotal{get;set;}
            public string Nature { get; set; }
        }
    }
}
