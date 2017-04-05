﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pharos.SyncService.Helpers;
using Pharos.SyncService.Exceptions;
using Pharos.Logic.ApiData.Pos.DAL;
using Pharos.SyncService.SyncEntities;
using System.Data.Entity;
using System.Data.Entity.Validation;

namespace Pharos.SyncService.LocalDataServices
{
    public class SalePackageSyncLocalService : ISyncDataService, ILocalDescribe
    {
        public TimeSpan SyncInterval
        {
            get
            {
                return new TimeSpan(0, 0, 1);
            }
        }
        public string Name
        {
            get
            {
                return this.GetType().ToString();
            }

        }
        public string Describe
        {
            get { return "销售数据包"; }
        }
        public Microsoft.Synchronization.SyncDirectionOrder SyncDirectionOrder
        {
            get { return Microsoft.Synchronization.SyncDirectionOrder.UploadAndDownload; }
        }

        public IEnumerable<ISyncDataObject> GetSyncObjects(int companyId, string storeId)
        {
            try
            {
                using (var db = SyncDbContextFactory.Factory<LocalCeDbContext>())
                {
                    var result = db.Database.SqlQuery<SyncDataObject>(@"select 'SalePackage' as  EntityType, SyncItemId,max (SyncItemVersion)  as SyncItemVersion from (
select  s.syncitemid,s.SyncItemVersion from saleorders s 
union all
select  s.syncitemid,d.SyncItemVersion from saleorders s,SaleDetail d where s.paysn = d.paysn
union all
select  s.syncitemid,b.SyncItemVersion from saleorders s,ConsumptionPayment b where s.paysn = b.paysn
union all
select  s.syncitemid,w.SyncItemVersion from saleorders s,WipeZero w where s.paysn =w.paysn ) as t group by SyncItemId ").ToList();
                    return result;
                }
            }
            catch
            {
                return new List<ISyncDataObject>();
            }
        }
        private ISyncDataObject GetVersion(Guid syncid, int companyId, string storeId, LocalCeDbContext db)
        {
            var result = db.Database.SqlQuery<SyncDataObject>(@"select 'SalePackage' as  EntityType, SyncItemId,max (SyncItemVersion)  as SyncItemVersion from (
select  s.syncitemid,s.SyncItemVersion from saleorders s where s.syncitemid = @p0
union all
select  s.syncitemid,d.SyncItemVersion from saleorders s,SaleDetail d where s.paysn = d.paysn and s.syncitemid = @p0
union all
select  s.syncitemid,b.SyncItemVersion from saleorders s,ConsumptionPayment b where s.paysn = b.paysn and s.syncitemid = @p0
union all
select  s.syncitemid,w.SyncItemVersion from saleorders s,WipeZero w where s.paysn =w.paysn  and s.syncitemid = @p0) as t group by SyncItemId ", syncid).ToList();
            return result.FirstOrDefault();
        }

        public ISyncDataObject GetItem(Guid guid, int companyId, string storeId)
        {
            using (var db = SyncDbContextFactory.Factory<LocalCeDbContext>())
            {
                var order = db.SaleOrders.FirstOrDefault(o => o.SyncItemId == guid);
                var package = new Package() { SyncItemId = guid, EntityType = "SalePackage" };
                var orderItem = new List<SaleOrders>();
                orderItem.Add(new SaleOrders().InitEntity(order));
                package.Init(orderItem);
                package.Init(db.SaleDetails.Where(o => o.PaySN == order.PaySN && o.CompanyId == companyId).ToList().Select(o => new SaleDetail().InitEntity(o, true)));
                package.Init(db.ConsumptionPayments.Where(o => o.PaySN == order.PaySN && o.CompanyId == companyId).ToList().Select(o => new ConsumptionPayment().InitEntity(o, true)));
                package.Init(db.WipeZeros.Where(o => o.PaySN == order.PaySN && o.CompanyId == companyId).ToList().Select(o => new WipeZero().InitEntity(o, true)));
                return package;

            }
        }
        public IDictionary<Microsoft.Synchronization.SyncId, ISyncDataObject> GetItems(IEnumerable<Microsoft.Synchronization.SyncId> ids, int companyId, string StoreId)
        {
            using (var db = SyncDbContextFactory.Factory<LocalCeDbContext>())
            {
                var syncidsdict = ids.ToDictionary(o => o.GetGuidId(), o => o);
                var syncids = syncidsdict.Keys;
                var query = db.SaleOrders.Where(o => syncids.Contains(o.SyncItemId)).Include(o => o.SaleDetails).Include(o => o.WipeZeros).Include(o => o.ConsumptionPayments).ToList();
                return query.ToDictionary(o => syncidsdict[o.SyncItemId], o =>
                {
                    var package = new Package() { SyncItemId = o.SyncItemId, EntityType = "SalePackage" };
                    package.Init(new SaleOrders[] { new SaleOrders().InitEntity(o) });
                    package.Init(o.SaleDetails.Select(p => new SaleDetail().InitEntity(p)).ToList());
                    package.Init(o.ConsumptionPayments.Select(p => new ConsumptionPayment().InitEntity(p)).ToList());
                    package.Init(o.WipeZeros.Select(p => new WipeZero().InitEntity(p)).ToList());
                    return package as ISyncDataObject;
                });
            }
        }
        public byte[] CreateItem(ISyncDataObject data, Guid guid, int companyId, string storeId)
        {
            var temp = data as Package;
            using (var db = SyncDbContextFactory.Factory<LocalCeDbContext>())
            {
                try
                {
                    var orders = temp.GetEntities<SaleOrders>();
                    var saleDetails = temp.GetEntities<SaleDetail>();
                    var consumptionPayments = temp.GetEntities<ConsumptionPayment>();
                    var wipeZeros = temp.GetEntities<WipeZero>();
                    db.SaleOrders.AddRange(orders.Select(o => new Pharos.Logic.ApiData.Pos.Entity.LocalCeEntity.SaleOrders().InitEntity(o)));
                    db.SaleDetails.AddRange(saleDetails.Select(o => new Pharos.Logic.ApiData.Pos.Entity.LocalCeEntity.SaleDetail().InitEntity(o)));
                    db.ConsumptionPayments.AddRange(consumptionPayments.Select(o => new Pharos.Logic.ApiData.Pos.Entity.LocalCeEntity.ConsumptionPayment().InitEntity(o)));
                    db.WipeZeros.AddRange(wipeZeros.Select(o => new Pharos.Logic.ApiData.Pos.Entity.LocalCeEntity.WipeZero().InitEntity(o)));
                    db.SaveChanges();
                    var version = GetVersion(guid, companyId, storeId, db);
                    return version.SyncItemVersion;
                }
                catch (DbEntityValidationException dbEx)
                {
                    throw dbEx;
                }
                catch (Exception dbEx)
                {
                    throw dbEx;
                }
            }
        }

        public byte[] UpdateItem(Guid guid, ISyncDataObject mergedData, int companyId, string storeId)
        {
            try
            {
                var temp = mergedData as Package;

                using (var db = SyncDbContextFactory.Factory<LocalCeDbContext>())
                {
                    var orders = temp.GetEntities<SaleOrders>();
                    var ordersSyncIds = orders.Select(o => o.SyncItemId).ToList();
                    var saleDetails = temp.GetEntities<SaleDetail>();
                    var saleDetailsSyncIds = saleDetails.Select(o => o.SyncItemId).ToList();
                    var consumptionPayments = temp.GetEntities<ConsumptionPayment>();
                    var consumptionPaymentsSyncIds = consumptionPayments.Select(o => o.SyncItemId).ToList();
                    var wipeZeros = temp.GetEntities<WipeZero>();
                    var wipeZerosSyncIds = wipeZeros.Select(o => o.SyncItemId).ToList();

                    db.SaleOrders.Where(o => ordersSyncIds.Contains(o.SyncItemId)).ToList().ForEach(o => o.InitEntity(orders.FirstOrDefault(p => o.SyncItemId == p.SyncItemId)));
                    db.SaleDetails.Where(o => saleDetailsSyncIds.Contains(o.SyncItemId)).ToList().ForEach(o => o.InitEntity(saleDetails.FirstOrDefault(p => o.SyncItemId == p.SyncItemId)));
                    db.ConsumptionPayments.Where(o => consumptionPaymentsSyncIds.Contains(o.SyncItemId)).ToList().ForEach(o => o.InitEntity(consumptionPayments.FirstOrDefault(p => o.SyncItemId == p.SyncItemId)));
                    db.WipeZeros.Where(o => wipeZerosSyncIds.Contains(o.SyncItemId)).ToList().ForEach(o => o.InitEntity(wipeZeros.FirstOrDefault(p => o.SyncItemId == p.SyncItemId)));
                    db.SaveChanges();
                    var version = GetVersion(guid, companyId, storeId, db);
                    return version.SyncItemVersion;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void DeleteItem(Guid syncItemId, int companyId, string storeId)
        {
            using (var db = SyncDbContextFactory.Factory<LocalCeDbContext>())
            {
                var order = db.SaleOrders.FirstOrDefault(o => o.SyncItemId == syncItemId);
                db.SaleOrders.Remove(order);
                db.SaleDetails.RemoveRange(db.SaleDetails.Where(o => o.PaySN == order.PaySN).ToList());
                db.ConsumptionPayments.RemoveRange(db.ConsumptionPayments.Where(o => o.PaySN == order.PaySN).ToList());
                db.WipeZeros.RemoveRange(db.WipeZeros.Where(o => o.PaySN == order.PaySN).ToList());
                db.SaveChanges();
            }
        }



        public ISyncDataObject Merge(ISyncDataObject syncDataObject1, ISyncDataObject syncDataObject2, int companyId, string storeId)
        {
            if (syncDataObject1 != null)
            {
                var temp = syncDataObject1 as Package;
                var orders = temp.GetEntities<SaleOrders>();
                var order = orders.FirstOrDefault();

                var tempSer = syncDataObject2 as Package;
                var orderSers = temp.GetEntities<SaleOrders>();
                var orderSer = orderSers.FirstOrDefault();
                if (orderSer.IsProcess && !order.IsProcess && order.InInventory != orderSer.InInventory)
                {
                    return syncDataObject1;
                }
                else
                {
                    return syncDataObject2;
                }

            }
            else
            {
                return syncDataObject2;
            }
        }
    }
}
