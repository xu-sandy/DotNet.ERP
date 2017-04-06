using Pharos.Logic.ApiData.Pos.DAL;
using Pharos.Logic.DAL;
using Pharos.SyncService.SyncEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using Pharos.SyncService.Helpers;
using Pharos.SyncService.Exceptions;

namespace Pharos.SyncService.RemoteDataServices
{
    public class SysStoreUserInfoSyncRemoteService : ISyncDataService
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
                    var result = db.SysStoreUserInfos.Where(o => o.CompanyId == companyId).Select(o => new SyncDataObject() { SyncItemId = o.SyncItemId, SyncItemVersion = o.SyncItemVersion }).ToList();
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
                var entity = db.SysStoreUserInfos.Where(o => o.SyncItemId == guid && o.CompanyId == companyId).FirstOrDefault();
                return new SysStoreUserInfo().InitEntity(entity);
            }
        }
        public IDictionary<Microsoft.Synchronization.SyncId, ISyncDataObject> GetItems(IEnumerable<Microsoft.Synchronization.SyncId> ids, int companyId, string StoreId)
        {
            using (var db = SyncDbContextFactory.Factory<EFDbContext>())
            {
                var syncidsdict = ids.ToDictionary(o => o.GetGuidId(), o => o);
                var syncids = syncidsdict.Keys;
                var datas = db.SysStoreUserInfos.Where(o => syncids.Contains(o.SyncItemId)).ToList();
                return datas.ToDictionary(o => syncidsdict[o.SyncItemId], o => (ISyncDataObject)new Pharos.SyncService.SyncEntities.SysStoreUserInfo().InitEntity(o));
            }
        }
        public byte[] CreateItem(ISyncDataObject data, Guid guid, int companyId, string storeId)
        {
            throw new SyncException("SysStoreUserInfo表不允许创建远程数据");

        }

        public byte[] UpdateItem(Guid guid, ISyncDataObject mergedData, int companyId, string storeId)
        {
            var temp = mergedData as SysStoreUserInfo;
            using (var db = SyncDbContextFactory.Factory<EFDbContext>())
            {
                var dbEntity = db.SysStoreUserInfos.FirstOrDefault(o => o.SyncItemId == guid && o.CompanyId == companyId);
                if (dbEntity == null)
                    throw new SyncService.Exceptions.SyncException("未能找到更新的项！");
                dbEntity.LoginDT = temp.LoginDT;
                db.SaveChanges();
                var newEntity = db.SysStoreUserInfos.FirstOrDefault(o => o.SyncItemId == guid);
                return newEntity.SyncItemVersion;
            }
        }

        public void DeleteItem(Guid syncItemId, int companyId, string storeId)
        {
            throw new SyncException("SysStoreUserInfo表不允许删除远程数据");
        }
        /// <summary>
        /// 冲突合并
        /// </summary>
        /// <param name="syncDataObject1">本地数据</param>
        /// <param name="syncDataObject2">远程数据</param>
        /// <returns>合并冲突对象</returns>
        public ISyncDataObject Merge(ISyncDataObject local, ISyncDataObject remote, int companyId, string storeId)
        {
            var tempLocal = local as SysStoreUserInfo;
            var tempRemote = remote as SysStoreUserInfo;

            if (tempLocal.LoginDT != tempRemote.LoginDT)
            {
                tempRemote.LoginDT = tempLocal.LoginDT;
            }
            return tempRemote;
        }
        public Microsoft.Synchronization.SyncDirectionOrder SyncDirectionOrder
        {
            get { return Microsoft.Synchronization.SyncDirectionOrder.UploadAndDownload; }
        }
    }
}
