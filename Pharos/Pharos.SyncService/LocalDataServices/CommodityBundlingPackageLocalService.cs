﻿using Pharos.Logic.ApiData.Pos.DAL;
using Pharos.SyncService.SyncEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pharos.SyncService.Helpers;
using System.Data.Entity;
using System.Data.Entity.Validation;
using Pharos.Infrastructure.Data.Redis;
using Pharos.Logic.ApiData.Pos;

namespace Pharos.SyncService.LocalDataServices
{
    /// <summary>
    /// 捆绑促销
    /// </summary>
    public class CommodityBundlingPackageLocalService : ISyncDataService, ILocalDescribe
    {
        public TimeSpan SyncInterval
        {
            get
            {
                return new TimeSpan(0, 30, 0);
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
            get { return "捆绑促销数据包"; }
        }
        public Microsoft.Synchronization.SyncDirectionOrder SyncDirectionOrder
        {
            get { return Microsoft.Synchronization.SyncDirectionOrder.Download; }
        }

        public IEnumerable<ISyncDataObject> GetSyncObjects(int companyId, string storeId)
        {
            try
            {
                using (var db = SyncDbContextFactory.Factory<LocalCeDbContext>())
                {//PromotionType 1:单品折扣、 2:捆绑促销、 3:组合促销、4:买赠促销、 5:满元促销


                    var result = db.Database.SqlQuery<SyncDataObject>(@" select 'CommodityBundlingPackage' as  EntityType, SyncItemId,max (SyncItemVersion)  as SyncItemVersion from (
select  s.syncitemid,s.SyncItemVersion from [CommodityPromotion] s 
union all
select  s.syncitemid,d.SyncItemVersion from [CommodityPromotion] s,Bundling d where s.Id = d.CommodityId  
union all
select  s.syncitemid,b.SyncItemVersion from [CommodityPromotion] s,BundlingList b where s.Id = b.CommodityId
) as t group by SyncItemId ").ToList();

                    return result;
                }
            }
            catch
            {
                return new List<ISyncDataObject>();
            }
        }
        private ISyncDataObject GetVersion(Guid syncId, int companyId, string storeId, LocalCeDbContext db)
        {
            var result = db.Database.SqlQuery<SyncDataObject>(@" select 'CommodityBundlingPackage' as  EntityType, SyncItemId,max (SyncItemVersion)  as SyncItemVersion from (
select  s.syncitemid,s.SyncItemVersion from [CommodityPromotion] s where s.syncitemid=@p0
union all
select  s.syncitemid,d.SyncItemVersion from [CommodityPromotion] s,Bundling d where s.Id = d.CommodityId and s.syncitemid=@p0 
union all
select  s.syncitemid,b.SyncItemVersion from [CommodityPromotion] s,BundlingList b where s.Id = b.CommodityId and s.syncitemid=@p0
) as t group by SyncItemId ", syncId).ToList();

            return result.FirstOrDefault();
        }
        public ISyncDataObject GetItem(Guid guid, int companyId, string storeId)
        {
            using (var db = SyncDbContextFactory.Factory<LocalCeDbContext>())
            {
                var promitions = db.CommodityPromotions.FirstOrDefault(o => o.SyncItemId == guid);
                var bundling = db.Bundlings.FirstOrDefault(o => o.CommodityId == promitions.Id);
                var package = new Package() { SyncItemId = guid, EntityType = "CommodityBundlingPackage" };
                var promitionItem = new List<CommodityPromotion>();
                var bundlingItem = new List<Bundling>();

                promitionItem.Add(new CommodityPromotion().InitEntity(promitions));
                package.Init(promitionItem);

                bundlingItem.Add(new Bundling().InitEntity(bundling));
                package.Init(bundlingItem);

                package.Init(db.CommodityDiscounts.Where(o => o.CommodityId == promitions.Id).ToList().Select(o => new CommodityDiscount().InitEntity(o, true)));
                return package;
            }
        }
        public IDictionary<Microsoft.Synchronization.SyncId, ISyncDataObject> GetItems(IEnumerable<Microsoft.Synchronization.SyncId> ids, int companyId, string StoreId)
        {
            using (var db = SyncDbContextFactory.Factory<LocalCeDbContext>())
            {
                var syncidsdict = ids.ToDictionary(o => o.GetGuidId(), o => o);
                var syncids = syncidsdict.Keys;
                var query = db.CommodityPromotions.Where(o => syncids.Contains(o.SyncItemId)).Include(o => o.Bundlings).Include(o => o.BundlingDetails).ToList();
                return query.ToDictionary(o => syncidsdict[o.SyncItemId], o =>
                {
                    var package = new Package() { SyncItemId = o.SyncItemId, EntityType = "CommodityBundlingPackage" };
                    package.Init(new CommodityPromotion[] { new CommodityPromotion().InitEntity(o) });
                    package.Init(o.Bundlings.Select(p => new Bundling().InitEntity(p)).ToList());
                    package.Init(o.BundlingDetails.Select(p => new BundlingList().InitEntity(p)).ToList());
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
                    var commodityPromotion = temp.GetEntities<CommodityPromotion>();
                    var bundling = temp.GetEntities<Bundling>();
                    var bundlingList = temp.GetEntities<BundlingList>();
                    db.CommodityPromotions.AddRange(commodityPromotion.Select(o => new Pharos.Logic.ApiData.Pos.Entity.LocalCeEntity.CommodityPromotion().InitEntity(o)));
                    db.Bundlings.AddRange(bundling.Select(o => new Pharos.Logic.ApiData.Pos.Entity.LocalCeEntity.Bundling().InitEntity(o)));
                    db.BundlingLists.AddRange(bundlingList.Select(o => new Pharos.Logic.ApiData.Pos.Entity.LocalCeEntity.BundlingList().InitEntity(o)));
                    db.SaveChanges();
                    // RedisManager.Publish("MarketingRefresh", new { StoreId = storeId, CompanyId = companyId });
                    StoreManager.PubEvent("MarketingRefresh", new { StoreId = storeId, CompanyId = companyId });
                    var version = GetVersion(guid, companyId, storeId, db);
                    return version.SyncItemVersion;
                }
                catch (DbEntityValidationException dbEx)
                {
                    throw dbEx;
                }
            }
        }

        public byte[] UpdateItem(Guid guid, ISyncDataObject mergedData, int companyId, string storeId)
        {
            var temp = mergedData as Package;

            using (var db = SyncDbContextFactory.Factory<LocalCeDbContext>())
            {
                var _promotions = temp.GetEntities<CommodityPromotion>();
                var _promotionsSyncIds = _promotions.Select(o => o.SyncItemId).ToList();
                var commodityBundlings = temp.GetEntities<Bundling>();
                var commodityBundlingsSyncIds = commodityBundlings.Select(o => o.SyncItemId).ToList();
                var commodityBundlingLists = temp.GetEntities<BundlingList>();
                var commodityBundlingListsSyncIds = commodityBundlingLists.Select(o => o.SyncItemId).ToList();

                db.CommodityPromotions.Where(o => _promotionsSyncIds.Contains(o.SyncItemId)).ToList().ForEach(o => o.InitEntity(_promotions.FirstOrDefault(p => o.SyncItemId == p.SyncItemId)));
                db.Bundlings.Where(o => commodityBundlingsSyncIds.Contains(o.SyncItemId)).ToList().ForEach(o => o.InitEntity(commodityBundlings.FirstOrDefault(p => o.SyncItemId == p.SyncItemId)));
                db.BundlingLists.Where(o => commodityBundlingListsSyncIds.Contains(o.SyncItemId)).ToList().ForEach(o => o.InitEntity(commodityBundlingLists.FirstOrDefault(p => o.SyncItemId == p.SyncItemId)));
                db.SaveChanges();

                StoreManager.PubEvent("MarketingRefresh", new { StoreId = storeId, CompanyId = companyId });
                try
                {
                    var bunding = commodityBundlings.FirstOrDefault();
                    if (bunding != null)
                    {
                        var qRefresh = new List<Pharos.ObjectModels.DTOs.MemoryCacheRefreshQuery>();
                        var item = new ObjectModels.DTOs.MemoryCacheRefreshQuery() { CompanyId = companyId, StoreId = storeId, Barcode = bunding.NewBarcode, ProductType = ObjectModels.ProductType.Bundling, ProductCode = "" };
                        qRefresh.Add(item);
                        Pharos.Logic.ApiData.Pos.Cache.ProductCache.RefreshProduct(qRefresh);
                    }
                }
                catch { }

                var version = GetVersion(guid, companyId, storeId, db);
                return version.SyncItemVersion;
            }
        }

        public void DeleteItem(Guid syncItemId, int companyId, string storeId)
        {
            using (var db = SyncDbContextFactory.Factory<LocalCeDbContext>())
            {
                var promotions = db.CommodityPromotions.FirstOrDefault(o => o.SyncItemId == syncItemId);
                db.CommodityPromotions.Remove(promotions);
                db.Bundlings.RemoveRange(db.Bundlings.Where(o => o.CommodityId == promotions.Id).ToList());
                db.BundlingLists.RemoveRange(db.BundlingLists.Where(o => o.CommodityId == promotions.Id).ToList());
                db.SaveChanges();
                StoreManager.PubEvent("MarketingRefresh", new { StoreId = storeId, CompanyId = companyId });
            }
        }

        public ISyncDataObject Merge(ISyncDataObject syncDataObject1, ISyncDataObject syncDataObject2, int companyId, string storeId)
        {
            throw new NotImplementedException();
        }
    }
}
