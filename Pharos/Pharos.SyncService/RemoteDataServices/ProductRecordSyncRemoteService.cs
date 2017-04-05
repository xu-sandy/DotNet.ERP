using Pharos.Logic.ApiData.Pos.DAL;
using Pharos.Logic.DAL;
using Pharos.SyncService.Exceptions;
using Pharos.SyncService.SyncEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using Pharos.SyncService.Helpers;
using System.Text;

namespace Pharos.SyncService.RemoteDataServices
{
    public class ProductRecordSyncRemoteService : ISyncDataService
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
                    //var result = db.ApiLibrarys.Where(o => o.CompanyId == companyId).Select(o => new SyncDataObject() { SyncItemId = o.SyncItemId, SyncItemVersion = o.SyncItemVersion }).ToList();
                    var result = db.Database.SqlQuery<SyncDataObject>("EXEC [dbo].[SyncProductRecordVersion] @p0, @p1", storeId, companyId).ToList();
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
                // var entity = db.ApiLibrarys.Where(o => o.SyncItemId == guid && o.CompanyId == companyId).FirstOrDefault();
                var entity = db.Database.SqlQuery<ProductRecord>("EXEC [dbo].[SyncProductRecord] @p0,@p1,@p2", storeId, companyId, guid).FirstOrDefault();
                entity.EntityType = entity.GetType().ToString();
                return entity;
            }
        }
        public IDictionary<Microsoft.Synchronization.SyncId, ISyncDataObject> GetItems(IEnumerable<Microsoft.Synchronization.SyncId> ids, int companyId, string storeId)
        {
            using (var db = SyncDbContextFactory.Factory<EFDbContext>())
            {
                var syncidsdict = ids.ToDictionary(o => o.GetGuidId(), o => o);
                var syncids = syncidsdict.Keys;
                var datas = db.Database.SqlQuery<ProductRecord>("EXEC [dbo].[SyncProductRecord] @p0,@p1,@p2", storeId, companyId, syncids.SyncIdsToString()).ToList();
                return datas.ToDictionary(o => syncidsdict[o.SyncItemId], o => { o.EntityType = o.GetType().ToString(); return (ISyncDataObject)o; });
            }
        }
        public byte[] CreateItem(ISyncDataObject data, Guid guid, int companyId, string storeId)
        {
            // 该表不允许从本地修改
            throw new SyncException("ProductRecord表不允许本地更新");
        }

        public byte[] UpdateItem(Guid guid, ISyncDataObject mergedData, int companyId, string storeId)
        {
            // 该表不允许从本地修改
            throw new SyncException("ProductRecord表不允许本地更新");
        }

        public void DeleteItem(Guid syncItemId, int companyId, string storeId)
        {
            // 该表不允许从本地修改
            throw new SyncException("ProductRecord表不允许本地更新");
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
