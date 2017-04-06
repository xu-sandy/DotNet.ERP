using Pharos.Logic.ApiData.Pos.DAL;
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
    /// 单品折扣
    /// </summary>
    public class CommodityDiscountPackageLocalService : ISyncDataService, ILocalDescribe
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
            get { return "单品折扣数据包"; }
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
                    var result = db.Database.SqlQuery<SyncDataObject>(@" select 'CommodityDiscountPackage' as  EntityType, SyncItemId,max (SyncItemVersion)  as SyncItemVersion from (
select  s.syncitemid,s.SyncItemVersion from [CommodityPromotion] s 
union all
select  s.syncitemid,d.SyncItemVersion from [CommodityPromotion] s,[CommodityDiscount] d where s.Id = d.CommodityId  
) as t group by SyncItemId").ToList();

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
            var result = db.Database.SqlQuery<SyncDataObject>(@" select 'CommodityDiscountPackage' as  EntityType, SyncItemId,max (SyncItemVersion)  as SyncItemVersion from (
select  s.syncitemid,s.SyncItemVersion from [CommodityPromotion] s  where s.syncitemid=@p0
union all
select  s.syncitemid,d.SyncItemVersion from [CommodityPromotion] s,[CommodityDiscount] d where s.Id = d.CommodityId and s.syncitemid=@p0
) as t group by SyncItemId", syncId).ToList();

            return result.FirstOrDefault();
        }
        public ISyncDataObject GetItem(Guid guid, int companyId, string storeId)
        {
            using (var db = SyncDbContextFactory.Factory<LocalCeDbContext>())
            {
                var promotions = db.CommodityPromotions.FirstOrDefault(o => o.SyncItemId == guid);
                var package = new Package() { SyncItemId = guid, EntityType = "CommodityDiscountPackage" };
                var orderItem = new List<CommodityPromotion>();
                orderItem.Add(new CommodityPromotion().InitEntity(promotions));
                package.Init(orderItem);
                package.Init(db.CommodityDiscounts.Where(o => o.CommodityId == promotions.Id).ToList().Select(o => new CommodityDiscount().InitEntity(o, true)));
                return package;
            }
        }
        public IDictionary<Microsoft.Synchronization.SyncId, ISyncDataObject> GetItems(IEnumerable<Microsoft.Synchronization.SyncId> ids, int companyId, string StoreId)
        {
            using (var db = SyncDbContextFactory.Factory<LocalCeDbContext>())
            {
                var syncidsdict = ids.ToDictionary(o => o.GetGuidId(), o => o);
                var syncids = syncidsdict.Keys;
                var query = db.CommodityPromotions.Where(o => syncids.Contains(o.SyncItemId)).Include(o => o.CommodityDiscounts).ToList();
                return query.ToDictionary(o => syncidsdict[o.SyncItemId], o =>
                {
                    var package = new Package() { SyncItemId = o.SyncItemId, EntityType = "CommodityDiscountPackage" };
                    package.Init(new CommodityPromotion[] { new CommodityPromotion().InitEntity(o) });
                    package.Init(o.CommodityDiscounts.Select(p => new CommodityDiscount().InitEntity(p)).ToList());
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
                    var commodityDiscount = temp.GetEntities<CommodityDiscount>();
                    db.CommodityPromotions.AddRange(commodityPromotion.Select(o => new Pharos.Logic.ApiData.Pos.Entity.LocalCeEntity.CommodityPromotion().InitEntity(o)));
                    db.CommodityDiscounts.AddRange(commodityDiscount.Select(o => new Pharos.Logic.ApiData.Pos.Entity.LocalCeEntity.CommodityDiscount().InitEntity(o)));
                    db.SaveChanges();
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
                var commodityDiscounts = temp.GetEntities<CommodityDiscount>();
                var commodityDiscountsSyncIds = commodityDiscounts.Select(o => o.SyncItemId).ToList();

                db.CommodityPromotions.Where(o => _promotionsSyncIds.Contains(o.SyncItemId)).ToList().ForEach(o => o.InitEntity(_promotions.FirstOrDefault(p => o.SyncItemId == p.SyncItemId)));
                db.CommodityDiscounts.Where(o => commodityDiscountsSyncIds.Contains(o.SyncItemId)).ToList().ForEach(o => o.InitEntity(commodityDiscounts.FirstOrDefault(p => o.SyncItemId == p.SyncItemId)));
                db.SaveChanges();

                StoreManager.PubEvent("MarketingRefresh", new { StoreId = storeId, CompanyId = companyId });

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
                db.CommodityDiscounts.RemoveRange(db.CommodityDiscounts.Where(o => o.CommodityId == promotions.Id).ToList());
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
