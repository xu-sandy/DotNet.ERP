using Pharos.Logic.ApiData.Pos.DAL;
using Pharos.SyncService.SyncEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using Pharos.SyncService.Helpers;
using System.Text;

namespace Pharos.SyncService.LocalDataServices
{
    public class ProductRecordSyncLocalService : ISyncDataService, ILocalDescribe
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
            get { return "产品档案数据包"; }
        }
        public IEnumerable<ISyncDataObject> GetSyncObjects(int companyId, string storeId)
        {
            try
            {
                using (var db = SyncDbContextFactory.Factory<LocalCeDbContext>())
                {
                    var result = db.ProductRecords.Select(o => new SyncDataObject() { SyncItemId = o.SyncItemId, SyncItemVersion = o.SyncItemVersion }).ToList();
                    return result;
                }
            }
            catch
            {
                return new List<ISyncDataObject>();
            }
        }

        public ISyncDataObject GetItem(Guid guid, int companyId, string storeId)
        {
            using (var db = SyncDbContextFactory.Factory<LocalCeDbContext>())
            {
                var entity = db.ProductRecords.Where(o => o.SyncItemId == guid && o.CompanyId == companyId).FirstOrDefault();
                if (entity != null)
                    return new Pharos.SyncService.SyncEntities.ProductRecord().InitEntity(entity);
                return null;
            }
        }
        public IDictionary<Microsoft.Synchronization.SyncId, ISyncDataObject> GetItems(IEnumerable<Microsoft.Synchronization.SyncId> ids, int companyId, string StoreId)
        {
            using (var db = SyncDbContextFactory.Factory<LocalCeDbContext>())
            {
                var syncidsdict = ids.ToDictionary(o => o.GetGuidId(), o => o);
                var syncids = syncidsdict.Keys;
                var datas = db.ProductRecords.Where(o => syncids.Contains(o.SyncItemId)).ToList();
                return datas.ToDictionary(o => syncidsdict[o.SyncItemId], o => (ISyncDataObject)new Pharos.SyncService.SyncEntities.ProductRecord().InitEntity(o));
            }
        }
        public byte[] CreateItem(ISyncDataObject data, Guid guid, int companyId, string storeId)
        {
            var temp = data as Pharos.SyncService.SyncEntities.ProductRecord;
            using (var db = SyncDbContextFactory.Factory<LocalCeDbContext>())
            {
                var entity = new Pharos.Logic.ApiData.Pos.Entity.LocalCeEntity.ProductRecord();
                entity.InitEntity(temp);
                db.ProductRecords.Add(entity);
                db.SaveChanges();
                return entity.SyncItemVersion;
            }
        }

        public byte[] UpdateItem(Guid guid, ISyncDataObject mergedData, int companyId, string storeId)
        {
            var temp = mergedData as Pharos.SyncService.SyncEntities.ProductRecord;
            using (var db = SyncDbContextFactory.Factory<LocalCeDbContext>())
            {
                var dbEntity = db.ProductRecords.FirstOrDefault(o => o.SyncItemId == guid && o.CompanyId == companyId);
                dbEntity.InitEntity(temp);
                db.SaveChanges();
                try
                {
                    var qRefresh = new List<Pharos.ObjectModels.DTOs.MemoryCacheRefreshQuery>();
                    var item = new ObjectModels.DTOs.MemoryCacheRefreshQuery() { CompanyId = companyId, StoreId = storeId, Barcode = temp.Barcode, ProductCode = temp.ProductCode };
                    switch (temp.Nature)
                    {
                        case 0:
                            item.ProductType = ObjectModels.ProductType.Normal;
                            break;
                        case 1:
                            item.ProductType = ObjectModels.ProductType.Combination;
                            break;
                        case 2:
                            item.ProductType = ObjectModels.ProductType.Split;
                            break;
                    }
                    qRefresh.Add(item);
                    Pharos.Logic.ApiData.Pos.Cache.ProductCache.RefreshProduct(qRefresh);
                }
                catch { }
                return temp.SyncItemVersion;
            }
        }

        public void DeleteItem(Guid syncItemId, int companyId, string storeId)
        {
            using (var db = SyncDbContextFactory.Factory<LocalCeDbContext>())
            {
                var dbEntity = db.ProductRecords.FirstOrDefault(o => o.SyncItemId == syncItemId && o.CompanyId == companyId);
                db.ProductRecords.Remove(dbEntity);
                db.SaveChanges();
            }
        }

        public ISyncDataObject Merge(ISyncDataObject syncDataObject1, ISyncDataObject syncDataObject2, int companyId, string storeId)
        {
            return syncDataObject2;
        }

        public Microsoft.Synchronization.SyncDirectionOrder SyncDirectionOrder
        {
            get { return Microsoft.Synchronization.SyncDirectionOrder.Download; }
        }
    }
}
