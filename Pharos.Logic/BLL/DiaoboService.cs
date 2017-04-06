using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using Pharos.Logic.Entity;
using System.Collections.Specialized;
using Pharos.Utility.Helpers;
using Pharos.Utility;
namespace Pharos.Logic.BLL
{
    public class DiaoboService:BaseService<STHouseMove>
    {
        /// <summary>
        /// 用于datagrid列表
        /// </summary>
        /// <param name="nvl">传递条件</param>
        /// <param name="count">返回总行数</param>
        /// <returns>list</returns>
        public static object FindPageList(NameValueCollection nvl,out int count)
        {
            var queryHouse = CurrentRepository.QueryEntity;
            var state = nvl["state"];
            var searchText = nvl["searchText"];
            var queryProduct = BaseService<VwProduct>.CurrentRepository.QueryEntity;
            var queryStore = WarehouseService.CurrentRepository.QueryEntity;
            if(!state.IsNullOrEmpty())
            {
                var st = short.Parse(state);
                queryHouse = queryHouse.Where(o => o.State == st);
            }
            if (!searchText.IsNullOrEmpty())
            {
                queryProduct = queryProduct.Where(o => o.ProductCode.StartsWith(searchText) || o.Barcode.StartsWith(searchText) || o.Title.Contains(searchText));
            }
            var type=typeof(HouseMoveState);
            var query = from a in queryHouse
                        from b in queryProduct
                        join c in queryStore on a.InStoreId equals c.StoreId into tempIn
                        join d in queryStore on a.OutStoreId equals d.StoreId into tempOut
                        from tin in tempIn.DefaultIfEmpty()
                        from tout in tempOut.DefaultIfEmpty()
                        where a.Barcode == b.Barcode
                        select new
                        {
                            a.Id,
                            InStoreTitle = tin.Title,
                            OutStoreTitle = tout.Title,
                            InStoreId = tin.StoreId,
                            OutStoreId = tout.StoreId,
                            b.ProductCode,
                            b.Barcode,
                            b.Title,
                            b.BrandTitle,
                            b.SubUnit,
                            a.OrderQuantity,
                            a.DeliveryQuantity,
                            a.ActualQuantity,
                            a.CreateDT,
                            a.State,
                            tin.StoreId
                        };
            if (!Sys.CurrentUser.StoreId.IsNullOrEmpty())
                query = query.Where(o => (o.InStoreId == Sys.CurrentUser.StoreId || o.OutStoreId == Sys.CurrentUser.StoreId));
            count = query.Count();
            return query.ToPageList().Select(o=>new{
                o.Id,
                o.InStoreTitle,
                o.OutStoreTitle,
                o.InStoreId,
                o.OutStoreId,
                o.ProductCode,
                o.Barcode,
                o.Title,
                o.BrandTitle,
                o.SubUnit,
                o.OrderQuantity,
                o.DeliveryQuantity,
                o.ActualQuantity,
                CreateDT=o.CreateDT.ToString("yyyy-MM-dd"),
                o.State,
                StateTitle = Enum.GetName(type, o.State)
            }).ToList();
        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static OpResult SaveOrUpdate(STHouseMove obj)
        {
            var op = new OpResult();
            try
            {
                if(obj.Id.IsNullOrEmpty())
                {
                    obj.Id = CommonRules.GUID;
                    obj.CreateDT = DateTime.Now;
                    obj.CreateUID = Sys.CurrentUser.UID;
                    obj.State =(short)HouseMoveState.调拨中;
                    op=Add(obj);
                }else
                {
                    op = Update(obj);
                }
            }
            catch(Exception ex)
            {
                op.Message = ex.Message;
            }
            return op;
        }
        /// <summary>
        /// 显示扩展属性
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static object FindAllById(string id)
        {
            var queryHouse = CurrentRepository.QueryEntity;
            var queryProduct = BaseService<VwProduct>.CurrentRepository.QueryEntity;
            var queryStore = WarehouseService.CurrentRepository.QueryEntity;
            var query = from a in queryHouse
                        from b in queryProduct
                        join c in queryStore on a.InStoreId equals c.StoreId into tempIn
                        join d in queryStore on a.OutStoreId equals d.StoreId into tempOut
                        from tin in tempIn.DefaultIfEmpty()
                        from tout in tempOut.DefaultIfEmpty()
                        where a.Barcode == b.Barcode && a.Id==id
                        select new
                        {
                            a.Id,
                            InStoreTitle = tin.Title,
                            OutStoreTitle = tout.Title,
                            b.ProductCode,
                            b.Barcode,
                            b.Title,
                            b.BrandTitle,
                            b.SubUnit,
                            b.SysPrice,
                            a.OrderQuantity,
                            a.DeliveryQuantity,
                            a.ActualQuantity,
                            a.CreateDT,
                            a.State,
                            a.Memo
                        };
            var obj = query.FirstOrDefault();
            return obj;
        }
        public static OpResult Receiver(STHouseMove obj)
        {
            var op = new OpResult();
            try
            {
                var house = DiaoboService.FindById(obj.Id);
                house.State = (short)HouseMoveState.已收货;
                house.ActualUID = Sys.CurrentUser.UID;
                house.ActualQuantity = obj.ActualQuantity;
                op= DiaoboService.Update(house);
                if(op.Successed)
                InventoryRecordService.SaveLog(new List<InventoryRecord>(){
                    new InventoryRecord(){Barcode=house.Barcode,StoreId=house.InStoreId,Source=3,Number=house.ActualQuantity},
                    new InventoryRecord(){Barcode=house.Barcode,StoreId=house.OutStoreId,Source=4,Number=house.ActualQuantity}
                });
            }
            catch(Exception ex)
            {
                op.Message = ex.Message;
            }
            return op;
        }
    }
}
