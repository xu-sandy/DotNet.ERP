﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using Pharos.Logic.Entity;
using Pharos.Utility.Helpers;
using Pharos.Utility;
namespace Pharos.Logic.BLL
{
    public class ShouHuoService
    {
        public static object FindPageList(System.Collections.Specialized.NameValueCollection nvl,out int recordCount)
        {
            var userId = nvl["userId"];
            var state = nvl["state"];
            var orderNo = nvl["orderNo"];
            var queryDistrib = BaseService<OrderDistribution>.CurrentRepository.QueryEntity.Where(o => o.CompanyId == CommonService.CompanyId);
            var queryOrder = OrderService.CurrentRepository.QueryEntity.Where(o => o.CompanyId == CommonService.CompanyId);
            var queryProduct = BaseService<VwProduct>.CurrentRepository.QueryEntity.Where(o => o.CompanyId == CommonService.CompanyId);
            var queryReturn = OrderReturnBLL.CurrentRepository.QueryEntity.Where(o => o.CompanyId == CommonService.CompanyId);
            
            var query=from a in queryDistrib
                      from b in queryOrder
                      from c in queryProduct
                      let e=from d in queryReturn where a.DistributionId == d.DistributionId select d
                      from f in e.DefaultIfEmpty()
                      where a.IndentOrderId == b.IndentOrderId && (a.Barcode == c.Barcode || ("," + c.Barcodes + ",").Contains("," + a.Barcode + ","))
                      select new {
                          Id=a.DistributionId,
                          //f.DistributionId,
                          //ReturnState=f.State,
                          a.State,
                          a.DistributionBatch,
                          a.IndentOrderId,
                          a.Barcode,
                          c.Title,
                          c.SubUnit,
                          a.DeliveryNum,
                          a.ReceivedNum,
                          f.ReturnNum,
                          b.CreateDT,
                          b.CreateUID,
                          a.ReceivedDT,
                          a.DeliveryDT,
                          //f.ReturnType,
                          b.StoreId
                      };
            if (!userId.IsNullOrEmpty())
                query = query.Where(o => o.CreateUID == userId);
            if (!orderNo.IsNullOrEmpty())
                query = query.Where(o => o.IndentOrderId.StartsWith(orderNo));
            if (state == "1")//已收
                query = query.Where(o => o.State == 5);
            if (state == "2")//待收
                query = query.Where(o => o.State < 5);
            if (state == "3")//已约退换
                query = query.Where(o => o.State==6);
            if (!Sys.CurrentUser.StoreId.IsNullOrEmpty())
                query = query.Where(o => o.StoreId == Sys.CurrentUser.StoreId);
            recordCount= query.Count();
            var list= query.ToPageList();
            var sql = query.ToString();
            return list.Select(o => new {
                o.Id,
                StateTitle = StateTitle(o.State),
                o.State,
                o.DistributionBatch,
                o.IndentOrderId,
                o.Barcode,
                o.Title,
                o.SubUnit,
                o.DeliveryNum,
                o.ReceivedNum,
                o.ReturnNum,
                o.CreateDT,
                o.ReceivedDT
            });
        }
        public static object GetObj(string id)
        {
            var queryDistrib = BaseService<OrderDistribution>.CurrentRepository.QueryEntity;
            var queryOrder = BaseService<VwOrder>.CurrentRepository.QueryEntity;
            var queryOrderList = BaseService<IndentOrderList>.CurrentRepository.QueryEntity;
            var queryProduct = BaseService<VwProduct>.CurrentRepository.QueryEntity;
            var queryReturn = OrderReturnBLL.CurrentRepository.QueryEntity;
            var query = from a in queryDistrib
                        join b in queryOrder on a.IndentOrderId equals b.IndentOrderId
                        from c in queryProduct
                        let e = from d in queryReturn where a.DistributionId == d.DistributionId select d
                        from f in e.DefaultIfEmpty()
                        let j=from d in queryOrderList where d.IndentOrderId==a.IndentOrderId && (d.Barcode==a.Barcode || d.AssistBarcode==a.Barcode) && d.Nature==0 select d.IndentNum
                        let h=from i in queryOrderList where i.IndentOrderId == a.IndentOrderId && i.ResBarcode==a.Barcode && i.Nature==1 select i.IndentNum
                        where a.DistributionId == id && (a.Barcode == c.Barcode || ("," + c.Barcodes + ",").Contains("," + a.Barcode + ","))
                        select new
                        {
                            a.Id,
                            a.DistributionId,
                            a.State,
                            a.DistributionBatch,
                            a.IndentOrderId,
                            a.Barcode,
                            c.Title,
                            c.SubUnit,
                            a.DeliveryNum,
                            a.ReceivedNum,
                            f.ReturnNum,
                            IndentNum=j.Any()?j.Sum():0,
                            b.CreateDT,
                            b.OrderTitle,
                            a.ReceivedDT,
                            c.ValuationType,
                            GiftNum=h.Any()? h.Sum():0
                        };
            var list = query.ToList();
            var sql = query.ToString();
            return list.Select(o => new { 
                o.Id,
                o.Barcode,
                o.Title,
                o.DistributionBatch,
                o.IndentOrderId,
                o.DeliveryNum,
                DeliveryNums=o.DeliveryNum.GetValueOrDefault().ToAutoString()+o.SubUnit,
                ReturnNum = o.ReturnNum.GetValueOrDefault().ToAutoString() + o.SubUnit,
                ReceivedNum = o.ReceivedNum.GetValueOrDefault().ToAutoString() + o.SubUnit,
                ReceiveNum=o.ReceivedNum.HasValue?o.ReceivedNum:o.DeliveryNum,
                IndentNum = o.IndentNum.ToAutoString() + o.SubUnit,
                ReceivedNums = ReceivedNums(o.IndentOrderId, o.Barcode).ToAutoString() + o.SubUnit,
                o.CreateDT,
                o.OrderTitle,
                o.ReceivedDT,
                o.DistributionId,
                o.ValuationType,
                GiftNum=o.GiftNum.ToAutoString()
            }).FirstOrDefault();
        }
        public static IEnumerable<object> LoadGiftList(int id,string orderId,string barcode)
        {
            var query = from a in BaseService<IndentOrderList>.CurrentRepository.Entities
                        let b=from x in BaseService<OrderDistributionGift>.CurrentRepository.Entities where a.Barcode== x.Barcode && x.OrderDistributionId==id select x.ReceivedNum
                        join y in ProductService.CurrentRepository.Entities on a.Barcode equals y.Barcode
                        where a.IndentOrderId==orderId && a.ResBarcode==barcode
                        select new { 
                            a.Id,
                            a.Barcode,
                            a.IndentNum,
                            ReceivedNum=b.FirstOrDefault(),
                            y.Title
                        };
            return query.ToList();
        }
        /// <summary>
        /// 更新收货数量
        /// </summary>
        /// <param name="id"></param>
        /// <param name="num">null-设为已收货插入库存表</param>
        /// <returns></returns>
        public static OpResult Update(string id, decimal? num,string returnBarcode="")
        {
            var ids = id.Split(',');
            var op = new OpResult();
            try
            {
                var list = BaseService<OrderDistribution>.CurrentRepository.QueryEntity.Include(o=>o.OrderDistributionGifts).Where(o => ids.Contains(o.DistributionId)).ToList();
                var orderIds = list.Select(o => o.IndentOrderId).ToList();
                var orderDistrIds = list.Select(o => o.Id).ToList();
                var orderList= OrderService.FindList(o => orderIds.Contains(o.IndentOrderId));
                var updated = System.Web.HttpContext.Current.Request["Updated"];
                var gifts = new List<OrderDistributionGift>();
                if(!updated.IsNullOrEmpty())
                {
                    gifts = updated.ToObject<List<OrderDistributionGift>>();
                }
                var records = new List<InventoryRecord>();                
                list.Each(obj =>
                {
                    if(!num.HasValue) 
                        obj.State = 5;
                    else
                    {
                        obj.ReceivedNum = num;
                        obj.ReceivedDT = DateTime.Now;
                    }
                    var store=orderList.FirstOrDefault(o=>o.IndentOrderId==obj.IndentOrderId);
                    foreach(var g in gifts)
                    {
                        var gift= obj.OrderDistributionGifts.FirstOrDefault(o => o.Barcode == g.Barcode);
                        if (gift != null)
                            gift.ReceivedNum = g.ReceivedNum;
                        else
                            obj.OrderDistributionGifts.Add(g);
                    }
                     foreach(var gift in obj.OrderDistributionGifts)
                     {
                         var pro= ProductService.Find(o => o.Barcode == gift.Barcode || ("," + o.Barcodes + ",").Contains("," + gift.Barcode+","));
                         records.Add(new InventoryRecord() { Barcode =(pro!=null?pro.Barcode: gift.Barcode), StoreId = Sys.CurrentUser.StoreId, Number = gift.ReceivedNum.GetValueOrDefault(), Source = 11, OperatId = obj.DistributionId });
                     }
                });
                if (num.HasValue)
                {
                    op=BaseService<OrderDistribution>.Update(list);
                }
                else//影响库存
                {
                    var detailList = BaseService<IndentOrderList>.FindList(o => orderIds.Contains(o.IndentOrderId) && o.Nature==0);
                    foreach (var dist in list)
                    {
                        var obj = detailList.FirstOrDefault(o => o.IndentOrderId == dist.IndentOrderId && (o.Barcode == dist.Barcode || o.AssistBarcode==dist.Barcode));
                        if (obj == null) continue;
                        var store = orderList.FirstOrDefault(o => o.IndentOrderId == obj.IndentOrderId);
                        records.Add(new InventoryRecord() { Barcode = obj.Barcode, StoreId = store.StoreId, Number = dist.ReceivedNum.GetValueOrDefault(), OperatType = 1, Source = 11, OperatId = dist.DistributionId });
                        obj.AcceptNum += dist.ReceivedNum.GetValueOrDefault();
                        if (obj.AcceptNum >= obj.IndentNum || obj.Barcode==returnBarcode)
                            obj.State = 5;
                    }
                    foreach (var order in orderList)
                    {
                        int count = 0;
                        var details = detailList.Where(o => o.IndentOrderId == order.IndentOrderId);
                        foreach (var detail in details)
                        {
                            if (detail.State == 5)
                                count++;
                        }
                        if (details.Count() == count)//都为已收货时,更新订单主表状态
                        {
                            order.State = 5;
                            order.ReceivedDT = DateTime.Now;
                        }
                    }
                    var returnIds= list.Where(o => o.OrderReturnId.HasValue).Select(o=>o.OrderReturnId).ToList();
                    if(returnIds.Any())//已完成换
                    {
                        var rtns= OrderReturnBLL.FindList(o => returnIds.Contains(o.Id));
                        rtns.Each(o => o.State = 2);
                    }
                    BaseService<OrderDistribution>.Update(list, false);
                    BaseService<IndentOrderList>.Update(detailList, false);
                    op = BaseService<IndentOrder>.Update(orderList);
                    if(op.Successed) InventoryRecordService.SaveLog(records);
                    //op = CommodityService.AddRange(commoditys);
                }
            }
            catch (Exception e)
            {
                op.Message = e.Message;
                new Sys.LogEngine().WriteError(e);
            }
            return op;
        }
        public static OpResult SaveTuiHuang(OrderReturns obj,string receiveNum)
        {
            var op = new OpResult();
            try
            {
                obj.CompanyId = CommonService.CompanyId;
                if(obj.Id==0)
                {
                    obj.State = 0;
                    var dist = OrderDistributionService.Find(o => o.DistributionId == obj.DistributionId);
                    if (dist != null)
                    {
                        dist.State = 6;
                        dist.ReceivedNum = dist.DeliveryNum - obj.ReturnNum;
                        dist.ReceivedDT = DateTime.Now;
                        if(obj.ReturnType==0)//退货时
                        {
                            OrderDistributionService.Update(dist);
                            Update(obj.DistributionId, null,obj.Barcode);
                            obj.State = 1;
                        }
                    }
                    obj.CreateDT = DateTime.Now;
                    obj.CreateUID = Sys.CurrentUser.UID;
                    op= OrderReturnBLL.Add(obj);
                    //if(op.Successed)
                    //    InventoryRecordService.SaveLog(new List<InventoryRecord>(){
                    //        new InventoryRecord(){Barcode=obj.Barcode,StoreId=Sys.CurrentUser.StoreId,Source=12,Number=obj.ReturnNum.GetValueOrDefault()}
                    //    });
                }
                else
                {
                    var resour = OrderReturnBLL.FindById(obj.Id);
                    obj.ToCopyProperty(resour);
                    op = OrderReturnBLL.Update(resour);
                }
            }
            catch(Exception e)
            {
                op.Message = e.Message;
                new Sys.LogEngine().WriteError(e);
            }
            return op;
        }
        static string StateTitle(short state, string distributionId,short? returnState,short? returnType)
        {
            if (state == 5) return "已收货";
            if (!distributionId.IsNullOrEmpty())
            {
                if(returnState==2)
                    return returnType==0?"已退":"已换";
                else
                    return returnType == 0 ? "已约待退" : "已约待换";
            }
            return "待收";
        }
        static string StateTitle(short state)
        {
            string title = "";
            switch (state)
            {
                case 5:
                    title = "已收货";
                    break;
                case 6:
                    title = "已约退换";
                    break;
                case 7:
                    title = "已退换";
                    break;
                default:
                    title = "待收";
                    break;
            }
            return title;
        }
        static decimal ReceivedNums(string orderid,string barcode)
        {
            var num = BaseService<OrderDistribution>.CurrentRepository.QueryEntity.Where(o => o.IndentOrderId == orderid && o.Barcode == barcode).Sum(o=>o.ReceivedNum);
            return num.GetValueOrDefault();
        }
    }
}
