using Pharos.Logic.ApiData.Pos.DAL;
using Pharos.Logic.DAL;
using Pharos.SyncService.Exceptions;
using Pharos.SyncService.SyncEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pharos.SyncService.Helpers;
using System.Data.Entity;

namespace Pharos.SyncService.RemoteDataServices
{
    public class CommodityFreeGiftPurchasePackageRemoteService : ISyncDataService
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
        public Microsoft.Synchronization.SyncDirectionOrder SyncDirectionOrder
        {
            get { return Microsoft.Synchronization.SyncDirectionOrder.Download; }
        }

        public IEnumerable<ISyncDataObject> GetSyncObjects(int companyId, string storeId)
        {
            try
            {
                using (var db = SyncDbContextFactory.Factory<EFDbContext>())
                {
                    //PromotionType 1:单品折扣、 2:捆绑促销、 3:组合促销、4:买赠促销、 5:满元促销
                    var result = db.Database.SqlQuery<SyncDataObject>(@" select 'CommodityFreeGiftPackage' as  EntityType, SyncItemId,max (SyncItemVersion) SyncItemVersion from (
select  s.syncitemid,s.SyncItemVersion from [CommodityPromotion] s where s.PromotionType=4 and s.companyid =@p0 and ((','+s.storeid+',') like ('%,'+@p1+',%') or (','+s.storeid+',') like ('%,-1,%')) and State!=2
union all
select  s.syncitemid,d.SyncItemVersion from [CommodityPromotion] s,[FreeGiftPurchase] d where s.Id = d.CommodityId and s.PromotionType=4 and s.companyid =@p0 and ((','+s.storeid+',') like ('%,'+@p1+',%') or (','+s.storeid+',') like ('%,-1,%')) and State!=2
union all
select  s.syncitemid,b.SyncItemVersion from [CommodityPromotion] s,[FreeGiftPurchase] d,[FreeGiftPurchaseList] b where s.Id =d.CommodityId  AND d.GiftId= b.GiftId and s.PromotionType=4 and s.companyid =@p0 and ((','+s.storeid+',') like ('%,'+@p1+',%') or (','+s.storeid+',') like ('%,-1,%')) and State!=2
) as t group by SyncItemId", companyId, storeId).ToList();

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
            var result = db.Database.SqlQuery<SyncDataObject>(@" select 'CommodityFreeGiftPackage' as  EntityType, SyncItemId,max (SyncItemVersion)  as SyncItemVersion from (
select  s.syncitemid,s.SyncItemVersion from [CommodityPromotion] s where s.syncitemid=@p0
union all
select  s.syncitemid,d.SyncItemVersion from [CommodityPromotion] s,[FreeGiftPurchase] d where s.Id = d.CommodityId and s.syncitemid=@p0
union all
select  s.syncitemid,b.SyncItemVersion from [CommodityPromotion] s,[FreeGiftPurchase] d,[FreeGiftPurchaseList] b where s.Id =d.CommodityId  AND d.GiftId= b.GiftId and s.syncitemid=@p0
) as t group by SyncItemId",syncId).ToList();

            return result.FirstOrDefault();
        }
        public ISyncDataObject GetItem(Guid guid, int companyId, string storeId)
        {
            using (var db = SyncDbContextFactory.Factory<EFDbContext>())
            {
                var commodits = db.CommodityPromotions.Where(o => o.SyncItemId == guid).ToList();
                var commodit = commodits.FirstOrDefault();
                var promotionFreeGiftPurchase = db.FreeGiftPurchases.Where(o => o.CommodityId == commodit.Id).ToList();
                var giftIds = promotionFreeGiftPurchase.Select(o => o.GiftId);
                var promotionFreeGiftPurchaseList = db.FreeGiftPurchaseLists.Where(o => giftIds.Contains(o.GiftId)).ToList();
                var package = new Package() { SyncItemId = guid, EntityType = "CommodityFreeGiftPackage" };
                package.Init(commodits.Select(o => new CommodityPromotion().InitEntity(o)).ToList());
                package.Init(promotionFreeGiftPurchase.Select(o => new FreeGiftPurchase().InitEntity(o)).ToList());
                package.Init(promotionFreeGiftPurchaseList.Select(o => new FreeGiftPurchaseList().InitEntity(o)).ToList());
                return package;
            }
        }
        public IDictionary<Microsoft.Synchronization.SyncId, ISyncDataObject> GetItems(IEnumerable<Microsoft.Synchronization.SyncId> ids, int companyId, string StoreId)
        {
            using (var db = SyncDbContextFactory.Factory<EFDbContext>())
            {
                var syncidsdict = ids.ToDictionary(o => o.GetGuidId(), o => o);
                var syncids = syncidsdict.Keys;
                var query = db.CommodityPromotions.Where(o => syncids.Contains(o.SyncItemId)).Include(o => o.FreeGiftPurchases).ToList();
                return query.ToDictionary(o => syncidsdict[o.SyncItemId], o =>
                {
                    var giftids = o.FreeGiftPurchases.Select(p => p.GiftId).ToList();
                    var gifts = db.FreeGiftPurchaseLists.Where(p => giftids.Contains(p.GiftId)).ToList();

                    var package = new Package() { SyncItemId = o.SyncItemId, EntityType = "CommodityFreeGiftPackage" };
                    package.Init(new CommodityPromotion[] { new CommodityPromotion().InitEntity(o) });

                    package.Init(o.FreeGiftPurchases.Select(p => new FreeGiftPurchase().InitEntity(p)).ToList());




                    package.Init(gifts.Select(p => new FreeGiftPurchaseList().InitEntity(p)).ToList());
                    return package as ISyncDataObject;
                });
            }
        }
        public byte[] CreateItem(ISyncDataObject data, Guid guid, int companyId, string storeId)
        {
            throw new SyncException("组合促销设定不允许修改远程数据");

        }

        public byte[] UpdateItem(Guid guid, ISyncDataObject mergedData, int companyId, string storeId)
        {
            throw new SyncException("组合促销设定不允许修改远程数据");

        }

        public void DeleteItem(Guid syncItemId, int companyId, string storeId)
        {
            throw new SyncException("组合促销设定不允许删除远程数据");

        }

        public ISyncDataObject Merge(ISyncDataObject syncDataObject1, ISyncDataObject syncDataObject2, int companyId, string storeId)
        {
            throw new NotImplementedException();
        }
    }
}
