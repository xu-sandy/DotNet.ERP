using Pharos.Logic.ApiData.Pos.DAL;
using Pharos.Logic.ApiData.Pos.Entity.LocalCeEntity;
using Pharos.SyncService.SyncEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pharos.SyncService.Helpers;

namespace Pharos.SyncService.LocalDataServices
{
    public class DeviceRegInfoSyncLocalService : ISyncDataService, ILocalDescribe
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
            get { return "设备注册数据包"; }
        }
        public IEnumerable<ISyncDataObject> GetSyncObjects(int companyId, string storeId)
        {
            try
            {
                using (var db = SyncDbContextFactory.Factory<LocalCeDbContext>())
                {
                    var result = db.DeviceRegInfos.Select(o => new SyncDataObject() { SyncItemId = o.SyncItemId, SyncItemVersion = o.SyncItemVersion }).ToList();

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
                var entity = db.DeviceRegInfos.Where(o => o.SyncItemId == guid).FirstOrDefault();
                if (entity != null)
                    return new Pharos.SyncService.SyncEntities.DeviceRegInfo().InitEntity(entity);
                return null;
            }
        }
        public IDictionary<Microsoft.Synchronization.SyncId, ISyncDataObject> GetItems(IEnumerable<Microsoft.Synchronization.SyncId> ids, int companyId, string StoreId)
        {
            using (var db = SyncDbContextFactory.Factory<LocalCeDbContext>())
            {
                var syncidsdict = ids.ToDictionary(o => o.GetGuidId(), o => o);
                var syncids = syncidsdict.Keys;
                var datas = db.DeviceRegInfos.Where(o => syncids.Contains(o.SyncItemId)).ToList();
                return datas.ToDictionary(o => syncidsdict[o.SyncItemId], o => (ISyncDataObject)new Pharos.SyncService.SyncEntities.DeviceRegInfo().InitEntity(o));
            }
        }
        public byte[] CreateItem(ISyncDataObject data, Guid guid, int companyId, string storeId)
        {
            var temp = data as Pharos.SyncService.SyncEntities.DeviceRegInfo;
            using (var db = SyncDbContextFactory.Factory<LocalCeDbContext>())
            {
                var entity = new Pharos.Logic.ApiData.Pos.Entity.LocalCeEntity.DeviceRegInfo();
                entity.InitEntity(temp);
                db.DeviceRegInfos.Add(entity);
                db.SaveChanges();
                return entity.SyncItemVersion;
            }
        }

        public byte[] UpdateItem(Guid guid, ISyncDataObject mergedData, int companyId, string storeId)
        {
            var temp = mergedData as Pharos.SyncService.SyncEntities.DeviceRegInfo;
            using (var db = SyncDbContextFactory.Factory<LocalCeDbContext>())
            {
                var dbEntity = db.DeviceRegInfos.FirstOrDefault(o => o.SyncItemId == guid);
                dbEntity.InitEntity(temp);
                db.SaveChanges();
                var newEntity = db.DeviceRegInfos.FirstOrDefault(o => o.SyncItemId == guid);
                return newEntity.SyncItemVersion;
            }
        }

        public void DeleteItem(Guid syncItemId, int companyId, string storeId)
        {
            using (var db = SyncDbContextFactory.Factory<LocalCeDbContext>())
            {
                var dbEntity = db.DeviceRegInfos.FirstOrDefault(o => o.SyncItemId == syncItemId);
                db.DeviceRegInfos.Remove(dbEntity);
                db.SaveChanges();
            }
        }

        public ISyncDataObject Merge(ISyncDataObject syncDataObject1, ISyncDataObject syncDataObject2, int companyId, string storeId)
        {
            if (syncDataObject1 != null)
            {
                var temp = syncDataObject1 as Pharos.SyncService.SyncEntities.DeviceRegInfo;
                var temSer = syncDataObject2 as Pharos.SyncService.SyncEntities.DeviceRegInfo;
                temp.State = temSer.State;
                temp.Memo = temSer.Memo;
                temp.AuditorUID = temSer.AuditorUID;
                temp.SyncItemVersion = temSer.SyncItemVersion;
                return temp;
            }
            else
            {
                return syncDataObject2;
            }
        }
        public Microsoft.Synchronization.SyncDirectionOrder SyncDirectionOrder
        {
            get { return Microsoft.Synchronization.SyncDirectionOrder.UploadAndDownload; }
        }
    }
}
