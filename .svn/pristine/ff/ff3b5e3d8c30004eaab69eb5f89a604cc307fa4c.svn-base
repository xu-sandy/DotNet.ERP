using Pharos.Logic.ApiData.Pos.DAL;
using Pharos.SyncService.SyncEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using Pharos.SyncService.Helpers;

namespace Pharos.SyncService.LocalDataServices
{
    public class SysStoreUserInfoSyncLocalService : ISyncDataService, ILocalDescribe
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
            get { return "门店员工信息数据包"; }
        }
        public IEnumerable<ISyncDataObject> GetSyncObjects(int companyId, string storeId)
        {
            try
            {
                using (var db = SyncDbContextFactory.Factory<LocalCeDbContext>())
                {
                    var result = db.SysStoreUserInfos.Select(o => new SyncDataObject() { SyncItemId = o.SyncItemId, SyncItemVersion = o.SyncItemVersion }).ToList();
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
                var entity = db.SysStoreUserInfos.Where(o => o.SyncItemId == guid && o.CompanyId == companyId).FirstOrDefault();
                if (entity != null)
                    return new SysStoreUserInfo().InitEntity(entity);
                return null;
            }
        }
        public IDictionary<Microsoft.Synchronization.SyncId, ISyncDataObject> GetItems(IEnumerable<Microsoft.Synchronization.SyncId> ids, int companyId, string StoreId)
        {
            using (var db = SyncDbContextFactory.Factory<LocalCeDbContext>())
            {
                var syncidsdict = ids.ToDictionary(o => o.GetGuidId(), o => o);
                var syncids = syncidsdict.Keys;
                var datas = db.SysStoreUserInfos.Where(o => syncids.Contains(o.SyncItemId)).ToList();
                return datas.ToDictionary(o => syncidsdict[o.SyncItemId], o => (ISyncDataObject)new Pharos.SyncService.SyncEntities.SysStoreUserInfo().InitEntity(o));
            }
        }
        public byte[] CreateItem(ISyncDataObject data, Guid guid, int companyId, string storeId)
        {
            var temp = data as SysStoreUserInfo;
            using (var db = SyncDbContextFactory.Factory<LocalCeDbContext>())
            {
                var entity = new Pharos.Logic.ApiData.Pos.Entity.LocalCeEntity.SysStoreUserInfo();
                entity.InitEntity(temp);
                entity.SyncItemVersion = temp.SyncItemVersion;
                db.SysStoreUserInfos.Add(entity);
                db.SaveChanges();
                return entity.SyncItemVersion;
            }
        }

        public byte[] UpdateItem(Guid guid, ISyncDataObject mergedData, int companyId, string storeId)
        {
            var temp = mergedData as SysStoreUserInfo;
            using (var db = SyncDbContextFactory.Factory<LocalCeDbContext>())
            {
                var dbEntity = db.SysStoreUserInfos.FirstOrDefault(o => o.SyncItemId == guid && o.CompanyId == companyId);
                dbEntity.InitEntity(temp);
                db.SaveChanges();
                var newEntity = db.SysStoreUserInfos.FirstOrDefault(o => o.SyncItemId == guid);
                return newEntity.SyncItemVersion;
            }
        }

        public void DeleteItem(Guid syncItemId, int companyId, string storeId)
        {
            using (var db = SyncDbContextFactory.Factory<LocalCeDbContext>())
            {
                var dbEntity = db.SysStoreUserInfos.FirstOrDefault(o => o.SyncItemId == syncItemId && o.CompanyId == companyId);
                db.SysStoreUserInfos.Remove(dbEntity);
                db.SaveChanges();
            }
        }
        /// <summary>
        /// 冲突合并
        /// </summary>
        /// <param name="syncDataObject1">本地数据</param>
        /// <param name="syncDataObject2">远程数据</param>
        /// <returns>合并冲突对象</returns>
        public ISyncDataObject Merge(ISyncDataObject local, ISyncDataObject remote, int companyId, string storeId)
        {
            if (local != null)
            {
                var tempLocal = local as SysStoreUserInfo;
                var tempRemote = remote as SysStoreUserInfo;

                if (tempLocal.LoginDT != tempRemote.LoginDT)
                {
                    tempRemote.LoginDT = tempLocal.LoginDT;
                }
                return tempRemote;
            }
            else
            {
                return remote;
            }
        }
        public Microsoft.Synchronization.SyncDirectionOrder SyncDirectionOrder
        {
            get { return Microsoft.Synchronization.SyncDirectionOrder.UploadAndDownload; }
        }
    }
}
