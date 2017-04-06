using Pharos.Logic.ApiData.Pos.DAL;
using Pharos.Logic.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using Pharos.SyncService.Helpers;
using System.Text;
using Pharos.SyncService.SyncEntities;
using Pharos.SyncService.Exceptions;

namespace Pharos.SyncService.RemoteDataServices
{
    public class ProductCategorySyncRemoteService : ISyncDataService
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
        public IEnumerable<ISyncDataObject> GetSyncObjects(int companyId, string storeId)
        {
            try
            {
                using (var db = SyncDbContextFactory.Factory<EFDbContext>())
                {
                    var result = db.Database.SqlQuery<SyncDataObject>("EXEC SyncStoreProductCategoryVersion @p0,@p1", companyId, storeId).ToList();
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
            using (var db = SyncDbContextFactory.Factory<EFDbContext>())
            {
                return db.Database.SqlQuery<ProductCategory>(@"EXEC [dbo].[SyncStoreProductCategory]
		                @companyId = @p0,
		                @storeId = @p1,
		                @syncId =@p2", companyId, storeId, guid).FirstOrDefault();
            }
        }
        public IDictionary<Microsoft.Synchronization.SyncId, ISyncDataObject> GetItems(IEnumerable<Microsoft.Synchronization.SyncId> ids, int companyId, string storeId)
        {
            using (var db = SyncDbContextFactory.Factory<EFDbContext>())
            {
                var syncidsdict = ids.ToDictionary(o => o.GetGuidId(), o => o);
                var syncids = syncidsdict.Keys;
                var datas = db.Database.SqlQuery<ProductCategory>(@"EXEC [dbo].[SyncStoreProductCategory]
		                @companyId = @p0,
		                @storeId = @p1,
		                @syncId =@p2", companyId, storeId, syncids.SyncIdsToString()).ToList();
                return datas.ToDictionary(o => syncidsdict[o.SyncItemId], o => { o.EntityType = o.GetType().ToString(); return (ISyncDataObject)o; });
            }
        }
        public byte[] CreateItem(ISyncDataObject data, Guid guid, int companyId, string storeId)
        {
            // 该表不允许从本地修改
            throw new SyncException("ProductCategory表不允许本地更新");
        }

        public byte[] UpdateItem(Guid guid, ISyncDataObject mergedData, int companyId, string storeId)
        {
            // 该表不允许从本地修改
            throw new SyncException("ProductCategory表不允许本地更新");
        }

        public void DeleteItem(Guid syncItemId, int companyId, string storeId)
        {
            // 该表不允许从本地修改
            throw new SyncException("ProductCategory表不允许本地更新");
        }

        public ISyncDataObject Merge(ISyncDataObject syncDataObject1, ISyncDataObject syncDataObject2, int companyId, string storeId)
        {
            return syncDataObject1;
        }
        public Microsoft.Synchronization.SyncDirectionOrder SyncDirectionOrder
        {
            get { return Microsoft.Synchronization.SyncDirectionOrder.Download; }
        }
    }
}
