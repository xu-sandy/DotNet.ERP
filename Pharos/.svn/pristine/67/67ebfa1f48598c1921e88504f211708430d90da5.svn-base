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
    public class MemberIntegralPackageSyncRemoteService : ISyncDataService
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
                    var result = db.Database.SqlQuery<SyncDataObject>(@"  select 'MemberIntegralSetPackage' as  EntityType, SyncItemId,max(SyncItemVersion) SyncItemVersion from (
select  s.syncitemid,s.SyncItemVersion from [MemberIntegralSet] s  where s.CompanyId=@p0
union all
select  s.syncitemid,d.SyncItemVersion from [MemberIntegralSet] s,[MemberIntegralSetList] d where s.Id = d.IntegralId  and s.CompanyId=@p0
) as t group by SyncItemId", companyId).ToList();
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
            var result = db.Database.SqlQuery<SyncDataObject>(@"  select 'MemberIntegralSetPackage' as  EntityType, SyncItemId,max (SyncItemVersion)  as SyncItemVersion from (
select  s.syncitemid,s.SyncItemVersion from [MemberIntegralSet] s where s.syncitemid=@p0
union all
select  s.syncitemid,d.SyncItemVersion from [MemberIntegralSet] s,[MemberIntegralSetList] d where s.Id = d.IntegralId and s.syncitemid=@p0
) as t group by SyncItemId",syncId).ToList();

            return result.FirstOrDefault();
        }

        public ISyncDataObject GetItem(Guid guid, int companyId, string storeId)
        {
            using (var db = SyncDbContextFactory.Factory<EFDbContext>())
            {
                var memIntSets = db.MemberIntegralSets.Where(o => o.SyncItemId == guid).ToList();
                var memIntSet = memIntSets.FirstOrDefault();
                var memIntSetLists = db.MemberIntegralSetLists.Where(o => o.IntegralId == memIntSet.Id).ToList();
                var package = new Package() { SyncItemId = guid, EntityType = "MemberIntegralSetPackage" };
                package.Init(memIntSets.Select(o => new MemberIntegralSet().InitEntity(o)).ToList());
                package.Init(memIntSetLists.Select(o => new MemberIntegralSetList().InitEntity(o)).ToList());
                return package;
            }
        }
        public IDictionary<Microsoft.Synchronization.SyncId, ISyncDataObject> GetItems(IEnumerable<Microsoft.Synchronization.SyncId> ids, int companyId, string StoreId)
        {
            using (var db = SyncDbContextFactory.Factory<EFDbContext>())
            {
                var syncidsdict = ids.ToDictionary(o => o.GetGuidId(), o => o);
                var syncids = syncidsdict.Keys;
                var query = db.MemberIntegralSets.Where(o => syncids.Contains(o.SyncItemId)).Include(o => o.ProductList).ToList();
                return query.ToDictionary(o => syncidsdict[o.SyncItemId], o =>
                {
                    var package = new Package() { SyncItemId = o.SyncItemId, EntityType = "MemberIntegralSetPackage" };
                    package.Init(new MemberIntegralSet[] { new MemberIntegralSet().InitEntity(o) });
                    package.Init(o.ProductList.Select(p => new MemberIntegralSetList().InitEntity(p)).ToList());
                    return package as ISyncDataObject;
                });
            }
        }
        public byte[] CreateItem(ISyncDataObject data, Guid guid, int companyId, string storeId)
        {
            throw new SyncException("消费积分设定不允许修改远程数据");
        }

        public byte[] UpdateItem(Guid guid, ISyncDataObject mergedData, int companyId, string storeId)
        {
            throw new SyncException("消费积分设定不允许修改远程数据");
        }

        public void DeleteItem(Guid syncItemId, int companyId, string storeId)
        {
            throw new SyncException("消费积分设定不允许删除远程数据");
        }




        public ISyncDataObject Merge(ISyncDataObject syncDataObject1, ISyncDataObject syncDataObject2, int companyId, string storeId)
        {
            throw new NotImplementedException();
        }
    }
}
