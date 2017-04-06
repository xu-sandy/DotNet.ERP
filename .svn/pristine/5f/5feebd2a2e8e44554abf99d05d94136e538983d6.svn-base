using Pharos.Logic.ApiData.Pos.DAL;
using Pharos.SyncService.SyncEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pharos.SyncService.Helpers;

namespace Pharos.SyncService.LocalDataServices
{
    public class MemberIntegralSyncLocalService : ISyncDataService, ILocalDescribe
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
            get { return "会员积分数据包"; }
        }
        public IEnumerable<ISyncDataObject> GetSyncObjects(int companyId, string storeId)
        {
            try
            {
                using (var db = SyncDbContextFactory.Factory<LocalCeDbContext>())
                {
                    var result = db.MemberIntegrals.Select(o => new SyncDataObject() { SyncItemId = o.SyncItemId, SyncItemVersion = o.SyncItemVersion }).ToList();
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
                var entity = db.MemberIntegrals.Where(o => o.SyncItemId == guid).FirstOrDefault();
                if (entity != null)
                    return new Pharos.SyncService.SyncEntities.MemberIntegral().InitEntity(entity);
                return null;
            }
        }
        public IDictionary<Microsoft.Synchronization.SyncId, ISyncDataObject> GetItems(IEnumerable<Microsoft.Synchronization.SyncId> ids, int companyId, string StoreId)
        {
            using (var db = SyncDbContextFactory.Factory<LocalCeDbContext>())
            {
                var syncidsdict = ids.ToDictionary(o => o.GetGuidId(), o => o);
                var syncids = syncidsdict.Keys;
                var datas = db.MemberIntegrals.Where(o => syncids.Contains(o.SyncItemId)).ToList();
                return datas.ToDictionary(o => syncidsdict[o.SyncItemId], o => (ISyncDataObject)new Pharos.SyncService.SyncEntities.MemberIntegral().InitEntity(o));
            }
        }
        public byte[] CreateItem(ISyncDataObject data, Guid guid, int companyId, string storeId)
        {
            var temp = data as Pharos.SyncService.SyncEntities.MemberIntegral;
            using (var db = SyncDbContextFactory.Factory<LocalCeDbContext>())
            {
                var entity = new Pharos.Logic.ApiData.Pos.Entity.LocalCeEntity.MemberIntegral();
                entity.InitEntity(temp);
                db.MemberIntegrals.Add(entity);
                db.SaveChanges();
                return entity.SyncItemVersion;
            }
        }

        public byte[] UpdateItem(Guid guid, ISyncDataObject mergedData, int companyId, string storeId)
        {
            var temp = mergedData as Pharos.SyncService.SyncEntities.MemberIntegral;
            using (var db = SyncDbContextFactory.Factory<LocalCeDbContext>())
            {
                var dbEntity = db.MemberIntegrals.FirstOrDefault(o => o.SyncItemId == guid);
                dbEntity.InitEntity(temp);
                db.SaveChanges();
                return dbEntity.SyncItemVersion;
            }
        }

        public void DeleteItem(Guid syncItemId, int companyId, string storeId)
        {
            using (var db = SyncDbContextFactory.Factory<LocalCeDbContext>())
            {
                var dbEntity = db.MemberIntegrals.FirstOrDefault(o => o.SyncItemId == syncItemId);
                db.MemberIntegrals.Remove(dbEntity);
                db.SaveChanges();
            }
        }

        public ISyncDataObject Merge(ISyncDataObject syncDataObject1, ISyncDataObject syncDataObject2, int companyId, string storeId)
        {
            //会员消费积分明细 全部返回本地数据 先不考虑积分
            return syncDataObject1;
        }
        public Microsoft.Synchronization.SyncDirectionOrder SyncDirectionOrder
        {
            get { return Microsoft.Synchronization.SyncDirectionOrder.UploadAndDownload; }
        }
    }
}
